#!/usr/bin/env python3
"""
LocTranslatedReport.py

Report all strings that have been translated for a given language, reading
Unity Localization asset files directly from the working tree.

An entry is considered translated when its m_Id exists in both the English
table and the target language table and the m_Localized value is non-empty.

Requires: fpdf2 (pip install fpdf2)

Usage:
    python LocTranslatedReport.py <language>
    python LocTranslatedReport.py fr
    python LocTranslatedReport.py de
"""

from __future__ import annotations

import argparse
import sys
from dataclasses import dataclass
from datetime import datetime
from pathlib import Path
from typing import Dict, List, Optional, Sequence


LOCALIZATION_RELATIVE_DIR = Path("Assets/Features/Localization/Data/Tables")


class LocTranslatedReportError(Exception):
    pass


@dataclass(frozen=True)
class TranslatedEntry:
    key: str
    english: str
    translated: str


@dataclass(frozen=True)
class TableResult:
    table_name: str
    entries: List[TranslatedEntry]


@dataclass(frozen=True)
class ReportData:
    language: str
    generated_at: datetime
    tables: List[TableResult]

    @property
    def total_translated(self) -> int:
        return sum(len(t.entries) for t in self.tables)


def main() -> int:
    arguments = parse_arguments()

    try:
        repository_root = resolve_repository_root()
        localization_directory = repository_root / LOCALIZATION_RELATIVE_DIR
        ensure_localization_directory_exists(localization_directory)

        validate_language(localization_directory, arguments.language)

        table_names = discover_table_names(localization_directory)
        table_results = build_report(localization_directory, table_names, arguments.language)

        report_data = ReportData(
            language=arguments.language,
            generated_at=datetime.now(),
            tables=table_results,
        )

        markdown_report = render_markdown_report(report_data)
        print(markdown_report)

        folder_name = "Changelogs"
        pdf_filename = (
            f"Localization_Translated_{arguments.language.upper()}_"
            f"{report_data.generated_at.strftime('%d%m%Y')}.pdf"
        )
        pdf_path = repository_root / folder_name / pdf_filename
        render_pdf_report(report_data, pdf_path)
        print(f"\nPDF: {pdf_path}", file=sys.stderr)
        import os
        os.startfile(pdf_path)

        return 0

    except LocTranslatedReportError as error:
        print(f"Error: {error}", file=sys.stderr)
        return 1
    except KeyboardInterrupt:
        print("Error: Cancelled.", file=sys.stderr)
        return 130


def parse_arguments() -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description="Report all translated strings for a given language."
    )
    parser.add_argument(
        "language",
        help="Language identifier to report on (e.g. fr, de, es).",
    )
    return parser.parse_args()


def resolve_repository_root() -> Path:
    import subprocess

    try:
        result = subprocess.run(
            ["git", "rev-parse", "--show-toplevel"],
            check=False,
            capture_output=True,
            text=True,
            encoding="utf-8",
            errors="replace",
        )
    except FileNotFoundError as error:
        raise LocTranslatedReportError("Git executable not found.") from error

    if result.returncode != 0 or not result.stdout.strip():
        raise LocTranslatedReportError(
            "Could not determine the git repository root. Run this script inside the repository."
        )

    return Path(result.stdout.strip())


def ensure_localization_directory_exists(localization_directory: Path) -> None:
    if not localization_directory.is_dir():
        raise LocTranslatedReportError(
            f"Localization directory does not exist: {localization_directory}"
        )


def discover_languages(localization_directory: Path) -> List[str]:
    languages = set()
    for asset_file in localization_directory.glob("*_*.asset"):
        parts = asset_file.stem.rsplit("_", 1)
        if len(parts) == 2:
            lang = parts[1]
            if lang and lang != "en" and lang != "comment":
                languages.add(lang)
    return sorted(languages)


def validate_language(localization_directory: Path, language: str) -> None:
    known = discover_languages(localization_directory)
    if language not in known:
        available = ", ".join(known) if known else "(none found)"
        raise LocTranslatedReportError(
            f"Unknown language '{language}'. Available languages: {available}"
        )


def discover_table_names(localization_directory: Path) -> List[str]:
    table_names = set()
    for asset_file in localization_directory.glob("*.asset"):
        name = asset_file.name
        if name.endswith("SharedData.asset"):
            table_name = name.removesuffix("SharedData.asset")
            if table_name:
                table_names.add(table_name)
        elif name.endswith(" Shared Data.asset"):
            table_name = name.removesuffix(" Shared Data.asset")
            if table_name:
                table_names.add(table_name)
    return sorted(table_names)


def build_report(
    localization_directory: Path,
    table_names: List[str],
    language: str,
) -> List[TableResult]:
    results: List[TableResult] = []

    for table_name in table_names:
        english_path = localization_directory / f"{table_name}_en.asset"
        target_path = localization_directory / f"{table_name}_{language}.asset"
        shared_path = resolve_shared_data_path(localization_directory, table_name)

        if not english_path.exists() or not target_path.exists():
            continue

        if not shared_path:
            raise LocTranslatedReportError(
                f"No SharedData asset found for table '{table_name}'."
            )

        id_to_key = parse_shared_data_asset(shared_path)
        id_to_english = parse_language_asset(english_path)
        id_to_target = parse_language_asset(target_path)

        entries: List[TranslatedEntry] = []
        for entry_id in sorted(id_to_english.keys()):
            translated_text = id_to_target.get(entry_id, "")
            if translated_text.strip():
                key = id_to_key.get(entry_id, f"<missing key for id {entry_id}>")
                entries.append(TranslatedEntry(
                    key=key,
                    english=id_to_english[entry_id],
                    translated=translated_text,
                ))

        if entries:
            results.append(TableResult(table_name=table_name, entries=entries))

    return results


def resolve_shared_data_path(localization_directory: Path, table_name: str) -> Optional[Path]:
    candidates = [
        localization_directory / f"{table_name}SharedData.asset",
        localization_directory / f"{table_name} Shared Data.asset",
    ]
    for candidate in candidates:
        if candidate.exists():
            return candidate
    return None


def parse_shared_data_asset(path: Path) -> Dict[int, str]:
    content = path.read_text(encoding="utf-8", errors="replace")
    id_to_key: Dict[int, str] = {}
    current_id: Optional[int] = None

    for line_number, line in enumerate(content.splitlines(), start=1):
        stripped = line.strip()

        if stripped.startswith("- m_Id:"):
            value_text = stripped[len("- m_Id:"):].strip()
            try:
                current_id = int(value_text)
            except ValueError as error:
                raise LocTranslatedReportError(
                    f"Invalid integer for m_Id in {path.name}, line {line_number}: {value_text}"
                ) from error
            continue

        if stripped.startswith("m_Key:"):
            if current_id is None:
                raise LocTranslatedReportError(
                    f"Encountered m_Key before m_Id in {path.name}, line {line_number}."
                )
            id_to_key[current_id] = parse_yaml_scalar(stripped[len("m_Key:"):].strip())
            current_id = None

    return id_to_key


def parse_language_asset(path: Path) -> Dict[int, str]:
    content = path.read_text(encoding="utf-8", errors="replace")
    id_to_localized: Dict[int, str] = {}
    current_id: Optional[int] = None

    for line_number, line in enumerate(content.splitlines(), start=1):
        stripped = line.strip()

        if stripped.startswith("- m_Id:"):
            value_text = stripped[len("- m_Id:"):].strip()
            try:
                current_id = int(value_text)
            except ValueError as error:
                raise LocTranslatedReportError(
                    f"Invalid integer for m_Id in {path.name}, line {line_number}: {value_text}"
                ) from error
            continue

        if stripped.startswith("m_Localized:"):
            if current_id is None:
                raise LocTranslatedReportError(
                    f"Encountered m_Localized before m_Id in {path.name}, line {line_number}."
                )
            id_to_localized[current_id] = parse_yaml_scalar(stripped[len("m_Localized:"):].strip())
            current_id = None

    return id_to_localized


def parse_yaml_scalar(raw: str) -> str:
    if len(raw) >= 2 and raw[0] == '"' and raw[-1] == '"':
        return _decode_yaml_double_quoted(raw[1:-1])
    if len(raw) >= 2 and raw[0] == "'" and raw[-1] == "'":
        return raw[1:-1].replace("''", "'")
    return raw


def _decode_yaml_double_quoted(s: str) -> str:
    import re

    def replace_escape(m: "re.Match[str]") -> str:
        esc = m.group(0)
        if esc == "\\n":  return "\n"
        if esc == "\\r":  return "\r"
        if esc == "\\t":  return "\t"
        if esc == "\\\\":  return "\\"
        if esc == '\\"':  return '"'
        if esc == "\\'":  return "'"
        if esc == "\\0":  return "\0"
        if esc.startswith("\\x"):  return chr(int(esc[2:], 16))
        if esc.startswith("\\u"):  return chr(int(esc[2:], 16))
        if esc.startswith("\\U"):  return chr(int(esc[2:], 16))
        return esc

    return re.sub(r'\\(?:x[0-9A-Fa-f]{2}|u[0-9A-Fa-f]{4}|U[0-9A-Fa-f]{8}|[nrt\\"\'0])', replace_escape, s)


def render_markdown_report(data: ReportData) -> str:
    lines: List[str] = []
    lines.append(
        f"# Translated Strings - {data.language.upper()} "
        f"({data.generated_at.strftime('%d/%m/%Y')})"
    )
    lines.append("")

    if data.total_translated == 0:
        lines.append(f"No translated entries found for `{data.language}`.")
        return "\n".join(lines)

    lines.append(f"**{data.total_translated} translated entries** across {len(data.tables)} table(s).")
    lines.append("")

    for table_result in data.tables:
        lines.append(f"## {table_result.table_name}  ({len(table_result.entries)} entries)")
        lines.append("")
        lines.extend(
            render_markdown_table(
                headers=["Key", "English", data.language.upper()],
                rows=[[e.key, e.english, e.translated] for e in table_result.entries],
            )
        )
        lines.append("")

    return "\n".join(lines)


def render_markdown_table(headers: Sequence[str], rows: Sequence[Sequence[str]]) -> List[str]:
    escaped_headers = [escape_markdown_cell(h) for h in headers]
    result = [
        "| " + " | ".join(escaped_headers) + " |",
        "| " + " | ".join(["---"] * len(headers)) + " |",
    ]
    for row in rows:
        result.append("| " + " | ".join(escape_markdown_cell(v) for v in row) + " |")
    return result


def escape_markdown_cell(value: object) -> str:
    text = str(value)
    text = text.replace("\\", "\\\\")
    text = text.replace("|", "\\|")
    text = text.replace("\r\n", "<br>")
    text = text.replace("\n", "<br>")
    text = text.replace("\r", "<br>")
    return text


def _to_latin1(text: str) -> str:
    replacements = {
        "—": "-",   # em dash
        "–": "-",   # en dash
        "‘": "'",   # left single quote
        "’": "'",   # right single quote
        "“": '"',   # left double quote
        "”": '"',   # right double quote
        "…": "...", # ellipsis
        " ": " ",   # non-breaking space
    }
    for char, replacement in replacements.items():
        text = text.replace(char, replacement)
    return text.encode("latin-1", errors="replace").decode("latin-1")


def render_pdf_report(data: ReportData, output_path: Path) -> None:
    from fpdf import FPDF

    pdf = FPDF(orientation="L", format="A4")
    pdf.set_auto_page_break(auto=True, margin=15)
    pdf.add_page()

    title = _to_latin1(
        f"Translated Strings - {data.language.upper()} "
        f"({data.generated_at.strftime('%d/%m/%Y')})"
    )
    pdf.set_font("Helvetica", "B", 16)
    pdf.cell(0, 10, title, new_x="LMARGIN", new_y="NEXT", align="C")
    pdf.ln(4)

    if data.total_translated == 0:
        pdf.set_font("Helvetica", "I", 11)
        pdf.cell(0, 8, f"No translated entries found for '{data.language}'.", new_x="LMARGIN", new_y="NEXT")
        pdf.output(str(output_path))
        return

    pdf.set_font("Helvetica", "", 10)
    pdf.cell(
        0, 6,
        f"{data.total_translated} translated entries across {len(data.tables)} table(s).",
        new_x="LMARGIN", new_y="NEXT",
    )
    pdf.ln(4)

    lang_header = data.language.upper()
    for table_result in data.tables:
        pdf.set_font("Helvetica", "B", 13)
        pdf.cell(
            0, 8,
            f"{table_result.table_name}  ({len(table_result.entries)} entries)",
            new_x="LMARGIN", new_y="NEXT",
        )
        pdf.ln(1)
        _render_pdf_table(
            pdf,
            headers=["Key", "English", lang_header],
            col_widths=[80, 120, 120],
            rows=[[e.key, _to_latin1(e.english), _to_latin1(e.translated)] for e in table_result.entries],
        )
        pdf.ln(5)

    output_path.parent.mkdir(parents=True, exist_ok=True)
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
        wrapped: List[List[str]] = []
        max_lines = 1
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

        for col_index, (cell_lines, width) in enumerate(zip(wrapped, col_widths)):
            x = x_start + sum(col_widths[:col_index])
            for line_index, line_text in enumerate(cell_lines):
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


if __name__ == "__main__":
    raise SystemExit(main())
