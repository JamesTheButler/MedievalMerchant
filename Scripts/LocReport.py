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

Usage:
    python LocReport.py <from_commit>
    python LocReport.py <from_commit> --output LocReport.md
    python LocReport.py <from_commit> --max-commits 100
    python LocReport.py <from_commit> --force
"""

from __future__ import annotations

import argparse
import subprocess
import sys
from dataclasses import dataclass
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


def main() -> int:
    arguments = parse_arguments()

    try:
        repository_root = resolve_repository_root()
        localization_directory = repository_root / LOCALIZATION_RELATIVE_DIR
        ensure_localization_directory_exists(localization_directory)

        validate_commit(arguments.from_commit)
        ensure_reasonable_commit_range(arguments.from_commit, arguments.max_commits, arguments.force)

        start_snapshot = load_project_snapshot(arguments.from_commit, repository_root)
        head_snapshot = load_project_snapshot("HEAD", repository_root)

        new_entries, changed_entries = build_report_entries(start_snapshot, head_snapshot)

        head_commit = get_git_stdout(["rev-parse", "--short", "HEAD"], repository_root).strip()
        start_commit = get_git_stdout(["rev-parse", "--short", arguments.from_commit], repository_root).strip()

        markdown_report = render_markdown_report(
            from_commit_short=start_commit,
            to_commit_short=head_commit,
            new_entries=new_entries,
            changed_entries=changed_entries,
        )

        print(markdown_report)

        if arguments.output_path is not None:
            output_path = Path(arguments.output_path)
            if not output_path.is_absolute():
                output_path = repository_root / output_path
            output_path.parent.mkdir(parents=True, exist_ok=True)
            output_path.write_text(markdown_report, encoding="utf-8")
            print(file=sys.stderr)
            print(f"Wrote report to: {output_path}", file=sys.stderr)

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
        "--output",
        dest="output_path",
        help="Optional output path for the generated Markdown report.",
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


def render_markdown_report(
    from_commit_short: str,
    to_commit_short: str,
    new_entries: Sequence[NewEntry],
    changed_entries: Sequence[ChangedEntry],
) -> str:
    lines: List[str] = []
    lines.append("# Localization Change Report")
    lines.append(f"From: `{from_commit_short}`")
    lines.append(f"To: `{to_commit_short}`")
    lines.append("")

    lines.append("## New localized strings")
    lines.append("")
    if new_entries:
        lines.extend(
            render_markdown_table(
                headers=["Table", "Key", "English"],
                rows=[
                    [entry.table_name, entry.key, entry.english]
                    for entry in new_entries
                ],
            )
        )
    else:
        lines.append("None.")
    lines.append("")

    lines.append("## Changed English strings")
    lines.append("")
    if changed_entries:
        lines.extend(
            render_markdown_table(
                headers=["Table", "Key", "Old English", "New English"],
                rows=[
                    [entry.table_name, entry.key, entry.old_english, entry.new_english]
                    for entry in changed_entries
                ],
            )
        )
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
