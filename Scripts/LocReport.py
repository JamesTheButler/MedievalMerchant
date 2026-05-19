#!/usr/bin/env python3
"""
LocReport.py

Generate a localization change report for Unity Localization tables by comparing
a starting git commit against HEAD.

The report contains two sections:
1. New localized strings
2. Existing localized strings whose English source text changed

Only these files are considered:
- <TableName>SharedData.asset
- <TableName>_en.asset

The script compares entries by stable m_Id and always reports the latest key
from HEAD, which naturally handles key renames.

Requires: fpdf2 (pip install fpdf2)

Usage:
    python LocReport.py <from_commit>
    python LocReport.py <from_commit> --max-commits 100
    python LocReport.py <from_commit> --force
"""

from __future__ import annotations

import argparse
import subprocess
import sys
from dataclasses import dataclass
from datetime import datetime
from pathlib import Path
from typing import Dict, Iterable, List, Optional, Sequence, Tuple


LOCALIZATION_RELATIVE_DIR = Path("Assets/Features/Localization/Data/Tables")


class LocReportError(Exception):
    pass


@dataclass(frozen=True)
class TableSnapshot:
    table_name: str
    id_to_key: Dict[int, str]
    id_to_english: Dict[int, str]


@dataclass(frozen=True)
class NewEntry:
    table_name: str
    entry_id: int
    key: str
    english: str


@dataclass(frozen=True)
class ChangedEntry:
    table_name: str
    entry_id: int
    key: str
    old_english: str
    new_english: str


@dataclass(frozen=True)
class ReportData:
    from_commit_short: str
    to_commit_short: str
    start_date: datetime
    end_date: datetime
    new_entries: Sequence[NewEntry]
    changed_entries: Sequence[ChangedEntry]


def main() -> int:
    arguments = parse_arguments()

    try:
        repository_root = resolve_repository_root()
        localization_directory = repository_root / LOCALIZATION_RELATIVE_DIR
        ensure_localization_directory_exists(localization_directory)

        validate_commit(arguments.from_commit)
        ensure_reasonable_commit_range(arguments.from_commit, arguments.max_commits, arguments.force)

        start_date = get_commit_date(arguments.from_commit, repository_root)
        head_date = get_commit_date("HEAD", repository_root)

        start_snapshot = load_project_snapshot(arguments.from_commit, repository_root)
        head_snapshot = load_project_snapshot("HEAD", repository_root)

        new_entries, changed_entries = build_report_entries(start_snapshot, head_snapshot)

        head_commit = get_git_stdout(["rev-parse", "--short", "HEAD"], repository_root).strip()
        start_commit = get_git_stdout(["rev-parse", "--short", arguments.from_commit], repository_root).strip()

        report_data = ReportData(
            from_commit_short=start_commit,
            to_commit_short=head_commit,
            start_date=start_date,
            end_date=head_date,
            new_entries=new_entries,
            changed_entries=changed_entries,
        )

        markdown_report = render_markdown_report(report_data)
        print(markdown_report)

        pdf_filename = (
            f"Localization_Change_Report_"
            f"{start_date.strftime('%d%m%Y')}_{head_date.strftime('%d%m%Y')}.pdf"
        )
        folder_name = f"Changelogs"
        pdf_path = repository_root / folder_name / pdf_filename
        render_pdf_report(report_data, pdf_path)
        print(f"\nPDF: {pdf_path}", file=sys.stderr)

        return 0

    except LocReportError as error:
        print(f"Error: {error}", file=sys.stderr)
        return 1
    except KeyboardInterrupt:
        print("Error: Cancelled.", file=sys.stderr)
        return 130


def parse_arguments() -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description="Generate a localization change report from a starting git commit to HEAD."
    )
    parser.add_argument(
        "from_commit",
        help="Git commit SHA, ref, or tag to compare against HEAD.",
    )
    parser.add_argument(
        "--max-commits",
        type=int,
        default=100,
        help="Maximum number of commits allowed between the starting commit and HEAD. Default: 100.",
    )
    parser.add_argument(
        "--force",
        action="store_true",
        help="Allow comparisons beyond the max commit limit.",
    )
    return parser.parse_args()


def resolve_repository_root() -> Path:
    try:
        repository_root = get_git_stdout(["rev-parse", "--show-toplevel"]).strip()
    except LocReportError as error:
        raise LocReportError(
            "Could not determine the git repository root. Run this script somewhere inside the repository."
        ) from error

    if not repository_root:
        raise LocReportError("Git did not return a repository root.")

    return Path(repository_root)


def ensure_localization_directory_exists(localization_directory: Path) -> None:
    if not localization_directory.is_dir():
        raise LocReportError(
            f"Localization directory does not exist: {localization_directory}"
        )


def validate_commit(commit_ref: str) -> None:
    try:
        get_git_stdout(["rev-parse", "--verify", f"{commit_ref}^{{commit}}"])
    except LocReportError as error:
        raise LocReportError(f"Commit could not be found: {commit_ref}") from error


def get_commit_date(commit_ref: str, repository_root: Path) -> datetime:
    date_text = get_git_stdout(
        ["log", "-1", "--format=%ai", commit_ref], repository_root
    ).strip()
    return datetime.strptime(date_text[:19], "%Y-%m-%d %H:%M:%S")


def ensure_reasonable_commit_range(from_commit: str, max_commits: int, force: bool) -> None:
    if force:
        return

    try:
        count_text = get_git_stdout(["rev-list", "--count", f"{from_commit}..HEAD"]).strip()
        commit_count = int(count_text)
    except ValueError as error:
        raise LocReportError("Could not parse the number of commits between the starting commit and HEAD.") from error

    if commit_count > max_commits:
        raise LocReportError(
            f"Starting commit is too far back: {commit_count} commits from HEAD. "
            f"Max allowed is {max_commits}. Use --force to override."
        )


def load_project_snapshot(commit_ref: str, repository_root: Path) -> Dict[str, TableSnapshot]:
    table_names = discover_table_names_at_commit(commit_ref, repository_root)
    project_snapshot: Dict[str, TableSnapshot] = {}

    for table_name in table_names:
        shared_file_path = resolve_shared_data_path(commit_ref, table_name, repository_root)
        english_file_path = LOCALIZATION_RELATIVE_DIR / f"{table_name}_en.asset"

        shared_exists = shared_file_path is not None
        english_exists = git_path_exists(commit_ref, english_file_path, repository_root)

        if shared_exists and not english_exists:
            raise LocReportError(
                f"Missing English table for '{table_name}' at {commit_ref}: {english_file_path.as_posix()}"
            )

        if english_exists and not shared_exists:
            raise LocReportError(
                f"Missing shared data table for '{table_name}' at {commit_ref}"
            )

        if not shared_exists and not english_exists:
            continue

        shared_content = get_git_file_content(commit_ref, shared_file_path, repository_root)
        english_content = get_git_file_content(commit_ref, english_file_path, repository_root)

        id_to_key = parse_shared_data_asset(shared_content, shared_file_path.as_posix(), commit_ref)
        id_to_english = parse_english_asset(english_content, english_file_path.as_posix(), commit_ref)

        project_snapshot[table_name] = TableSnapshot(
            table_name=table_name,
            id_to_key=id_to_key,
            id_to_english=id_to_english,
        )

    return project_snapshot


def discover_table_names_at_commit(commit_ref: str, repository_root: Path) -> List[str]:
    try:
        file_listing = get_git_stdout(
            ["ls-tree", "-r", "--name-only", commit_ref, LOCALIZATION_RELATIVE_DIR.as_posix()],
            repository_root,
        )
    except LocReportError as error:
        raise LocReportError(
            f"Could not list localization files at {commit_ref}."
        ) from error

    table_names = set()

    for raw_line in file_listing.splitlines():
        path_text = raw_line.strip()
        if not path_text.endswith(".asset"):
            continue

        path = Path(path_text)
        filename = path.name

        if filename.endswith("SharedData.asset"):
            table_name = filename.removesuffix("SharedData.asset")
            if table_name:
                table_names.add(table_name)
            continue

        if filename.endswith(" Shared Data.asset"):
            table_name = filename.removesuffix(" Shared Data.asset")
            if table_name:
                table_names.add(table_name)
            continue

        if filename.endswith("_en.asset"):
            table_name = filename.removesuffix("_en.asset")
            if table_name:
                table_names.add(table_name)
            continue

    return sorted(table_names)


def resolve_shared_data_path(commit_ref: str, table_name: str, repository_root: Path) -> Optional[Path]:
    candidates = [
        LOCALIZATION_RELATIVE_DIR / f"{table_name}SharedData.asset",
        LOCALIZATION_RELATIVE_DIR / f"{table_name} Shared Data.asset",
    ]

    for candidate in candidates:
        if git_path_exists(commit_ref, candidate, repository_root):
            return candidate

    return None


def git_path_exists(commit_ref: str, relative_path: Path, repository_root: Path) -> bool:
    try:
        get_git_stdout(["cat-file", "-e", f"{commit_ref}:{relative_path.as_posix()}"], repository_root)
        return True
    except LocReportError:
        return False


def get_git_file_content(commit_ref: str, relative_path: Path, repository_root: Path) -> str:
    try:
        return get_git_stdout(["show", f"{commit_ref}:{relative_path.as_posix()}"], repository_root)
    except LocReportError as error:
        raise LocReportError(
            f"Could not read file at {commit_ref}: {relative_path.as_posix()}"
        ) from error


def parse_shared_data_asset(file_content: str, file_label: str, commit_ref: str) -> Dict[int, str]:
    id_to_key: Dict[int, str] = {}
    current_id: Optional[int] = None

    for line_number, line in enumerate(file_content.splitlines(), start=1):
        stripped_line = line.strip()

        if stripped_line.startswith("- m_Id:"):
            current_id = parse_int_value(stripped_line, "- m_Id", file_label, commit_ref, line_number)
            continue

        if stripped_line.startswith("m_Key:"):
            if current_id is None:
                raise LocReportError(
                    f"Encountered m_Key before m_Id in {file_label} at {commit_ref}, line {line_number}."
                )

            key_value = parse_string_value(stripped_line, "m_Key")
            if current_id in id_to_key:
                raise LocReportError(
                    f"Duplicate entry id {current_id} in {file_label} at {commit_ref}."
                )

            id_to_key[current_id] = key_value
            current_id = None

    return id_to_key


def parse_english_asset(file_content: str, file_label: str, commit_ref: str) -> Dict[int, str]:
    id_to_english: Dict[int, str] = {}
    current_id: Optional[int] = None

    for line_number, line in enumerate(file_content.splitlines(), start=1):
        stripped_line = line.strip()

        if stripped_line.startswith("- m_Id:"):
            current_id = parse_int_value(stripped_line, "- m_Id", file_label, commit_ref, line_number)
            continue

        if stripped_line.startswith("m_Localized:"):
            if current_id is None:
                raise LocReportError(
                    f"Encountered m_Localized before m_Id in {file_label} at {commit_ref}, line {line_number}."
                )

            localized_value = parse_string_value(stripped_line, "m_Localized")
            if current_id in id_to_english:
                raise LocReportError(
                    f"Duplicate entry id {current_id} in {file_label} at {commit_ref}."
                )

            id_to_english[current_id] = localized_value
            current_id = None

    return id_to_english


def parse_int_value(line: str, expected_key: str, file_label: str, commit_ref: str, line_number: int) -> int:
    prefix = f"{expected_key}:"
    value_text = line[len(prefix):].strip()

    try:
        return int(value_text)
    except ValueError as error:
        raise LocReportError(
            f"Invalid integer value for {expected_key} in {file_label} at {commit_ref}, line {line_number}: {value_text}"
        ) from error


def parse_string_value(line: str, expected_key: str) -> str:
    prefix = f"{expected_key}:"
    return line[len(prefix):].strip()


def build_report_entries(
    start_snapshot: Dict[str, TableSnapshot],
    head_snapshot: Dict[str, TableSnapshot],
) -> Tuple[List[NewEntry], List[ChangedEntry]]:
    new_entries: List[NewEntry] = []
    changed_entries: List[ChangedEntry] = []

    for table_name in sorted(head_snapshot.keys()):
        head_table = head_snapshot[table_name]
        start_table = start_snapshot.get(table_name)

        head_english = head_table.id_to_english
        start_english = start_table.id_to_english if start_table is not None else {}

        for entry_id in sorted(head_english.keys()):
            new_text = head_english[entry_id]
            latest_key = head_table.id_to_key.get(entry_id, f"<missing key for id {entry_id}>")

            if entry_id not in start_english:
                new_entries.append(
                    NewEntry(
                        table_name=table_name,
                        entry_id=entry_id,
                        key=latest_key,
                        english=new_text,
                    )
                )
                continue

            old_text = start_english[entry_id]
            if old_text != new_text:
                changed_entries.append(
                    ChangedEntry(
                        table_name=table_name,
                        entry_id=entry_id,
                        key=latest_key,
                        old_english=old_text,
                        new_english=new_text,
                    )
                )

    return new_entries, changed_entries


def render_pdf_report(data: ReportData, output_path: Path) -> None:
    from fpdf import FPDF

    pdf = FPDF(orientation="L", format="A4")
    pdf.set_auto_page_break(auto=True, margin=15)
    pdf.add_page()

    title = (
        f"Localization Change Report "
        f"{data.start_date.strftime('%d/%m/%Y')} - {data.end_date.strftime('%d/%m/%Y')}"
    )
    pdf.set_font("Helvetica", "B", 16)
    pdf.cell(0, 10, title, new_x="LMARGIN", new_y="NEXT", align="C")

    pdf.set_font("Helvetica", "", 9)
    pdf.cell(
        0, 6,
        f"From: {data.from_commit_short}  |  To: {data.to_commit_short}",
        new_x="LMARGIN", new_y="NEXT", align="C",
    )
    pdf.ln(6)

    pdf.set_font("Helvetica", "B", 13)
    pdf.cell(0, 8, "New localized strings", new_x="LMARGIN", new_y="NEXT")
    pdf.ln(2)

    if data.new_entries:
        new_by_table: Dict[str, List] = {}
        for e in data.new_entries:
            new_by_table.setdefault(e.table_name, []).append(e)
        for table_name, entries in new_by_table.items():
            pdf.set_font("Helvetica", "B", 11)
            pdf.cell(0, 7, table_name, new_x="LMARGIN", new_y="NEXT")
            pdf.ln(1)
            _render_pdf_table(
                pdf,
                headers=["Key", "English"],
                col_widths=[100, 175],
                rows=[[e.key, e.english] for e in entries],
            )
            pdf.ln(4)
    else:
        pdf.set_font("Helvetica", "I", 10)
        pdf.cell(0, 6, "None.", new_x="LMARGIN", new_y="NEXT")

    pdf.ln(6)

    pdf.set_font("Helvetica", "B", 13)
    pdf.cell(0, 8, "Changed English strings", new_x="LMARGIN", new_y="NEXT")
    pdf.ln(2)

    if data.changed_entries:
        changed_by_table: Dict[str, List] = {}
        for e in data.changed_entries:
            changed_by_table.setdefault(e.table_name, []).append(e)
        for table_name, entries in changed_by_table.items():
            pdf.set_font("Helvetica", "B", 11)
            pdf.cell(0, 7, table_name, new_x="LMARGIN", new_y="NEXT")
            pdf.ln(1)
            _render_pdf_table(
                pdf,
                headers=["Key", "Old English", "New English"],
                col_widths=[90, 105, 105],
                rows=[[e.key, e.old_english, e.new_english] for e in entries],
            )
            pdf.ln(4)
    else:
        pdf.set_font("Helvetica", "I", 10)
        pdf.cell(0, 6, "None.", new_x="LMARGIN", new_y="NEXT")

    pdf.output(str(output_path))


def _render_pdf_table(
    pdf: "FPDF",
    headers: Sequence[str],
    col_widths: Sequence[int],
    rows: Sequence[Sequence[str]],
) -> None:
    line_height = 6

    pdf.set_font("Helvetica", "B", 9)
    pdf.set_fill_color(220, 220, 220)
    for header, width in zip(headers, col_widths):
        pdf.cell(width, line_height, header, border=1, fill=True)
    pdf.ln(line_height)

    pdf.set_font("Helvetica", "", 8)
    for row in rows:
        max_lines = 1
        wrapped: List[List[str]] = []
        for cell_text, width in zip(row, col_widths):
            char_width = pdf.get_string_width("x")
            max_chars = max(int(width / char_width) - 1, 10)
            lines = _wrap_text(cell_text, max_chars)
            wrapped.append(lines)
            max_lines = max(max_lines, len(lines))

        row_height = line_height * max_lines

        if pdf.get_y() + row_height > pdf.h - pdf.b_margin:
            pdf.add_page()
            pdf.set_font("Helvetica", "B", 9)
            pdf.set_fill_color(220, 220, 220)
            for header, width in zip(headers, col_widths):
                pdf.cell(width, line_height, header, border=1, fill=True)
            pdf.ln(line_height)
            pdf.set_font("Helvetica", "", 8)

        x_start = pdf.get_x()
        y_start = pdf.get_y()

        for col_index, (lines, width) in enumerate(zip(wrapped, col_widths)):
            x = x_start + sum(col_widths[:col_index])
            for line_index, line_text in enumerate(lines):
                pdf.set_xy(x, y_start + line_index * line_height)
                pdf.cell(width, line_height, line_text)
            pdf.set_xy(x, y_start)
            pdf.cell(width, row_height, "", border=1)

        pdf.set_xy(x_start, y_start + row_height)


def _wrap_text(text: str, max_chars: int) -> List[str]:
    if len(text) <= max_chars:
        return [text]

    lines: List[str] = []
    remaining = text
    while remaining:
        if len(remaining) <= max_chars:
            lines.append(remaining)
            break
        split_at = remaining.rfind(" ", 0, max_chars)
        if split_at <= 0:
            split_at = max_chars
        lines.append(remaining[:split_at])
        remaining = remaining[split_at:].lstrip()

    return lines if lines else [""]


def render_markdown_report(data: ReportData) -> str:
    lines: List[str] = []
    lines.append(
        f"# Localization Change Report "
        f"{data.start_date.strftime('%d/%m/%Y')} - {data.end_date.strftime('%d/%m/%Y')}"
    )
    lines.append(f"From: `{data.from_commit_short}` | To: `{data.to_commit_short}`")
    lines.append("")

    lines.append("## New localized strings")
    lines.append("")
    if data.new_entries:
        tables: Dict[str, List] = {}
        for entry in data.new_entries:
            tables.setdefault(entry.table_name, []).append(entry)
        for table_name, entries in tables.items():
            lines.append(f"### {table_name}")
            lines.append("")
            lines.extend(
                render_markdown_table(
                    headers=["Key", "English"],
                    rows=[[e.key, e.english] for e in entries],
                )
            )
            lines.append("")
    else:
        lines.append("None.")
        lines.append("")

    lines.append("## Changed English strings")
    lines.append("")
    if data.changed_entries:
        changed_tables: Dict[str, List] = {}
        for entry in data.changed_entries:
            changed_tables.setdefault(entry.table_name, []).append(entry)
        for table_name, entries in changed_tables.items():
            lines.append(f"### {table_name}")
            lines.append("")
            lines.extend(
                render_markdown_table(
                    headers=["Key", "Old English", "New English"],
                    rows=[[e.key, e.old_english, e.new_english] for e in entries],
                )
            )
            lines.append("")
    else:
        lines.append("None.")
        lines.append("")

    return "\n".join(lines)


def render_markdown_table(headers: Sequence[str], rows: Sequence[Sequence[str]]) -> List[str]:
    escaped_headers = [escape_markdown_cell(header) for header in headers]
    lines = [
        "| " + " | ".join(escaped_headers) + " |",
        "| " + " | ".join(["---"] * len(headers)) + " |",
    ]

    for row in rows:
        escaped_row = [escape_markdown_cell(value) for value in row]
        lines.append("| " + " | ".join(escaped_row) + " |")

    return lines


def escape_markdown_cell(value: object) -> str:
    text = str(value)
    text = text.replace("\\", "\\\\")
    text = text.replace("|", "\\|")
    text = text.replace("\r\n", "<br>")
    text = text.replace("\n", "<br>")
    text = text.replace("\r", "<br>")
    return text


def get_git_stdout(arguments: Sequence[str], repository_root: Optional[Path] = None) -> str:
    command = ["git", *arguments]

    try:
        result = subprocess.run(
            command,
            cwd=repository_root,
            check=False,
            capture_output=True,
            text=True,
            encoding="utf-8",
            errors="replace",
        )
    except FileNotFoundError as error:
        raise LocReportError("Git executable not found.") from error

    if result.returncode != 0:
        stderr_text = (result.stderr or "").strip()
        stdout_text = (result.stdout or "").strip()
        detail = stderr_text or stdout_text or f"git exited with code {result.returncode}"
        raise LocReportError(detail)

    return result.stdout


if __name__ == "__main__":
    raise SystemExit(main())
