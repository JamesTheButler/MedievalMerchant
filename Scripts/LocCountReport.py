#!/usr/bin/env python3
"""
LocCountReport.py

Report the number of localization entries and English word count per table,
reading Unity Localization asset files directly from the working tree.

Counts are based on the `_en.asset` table (the English string table). An
entry is counted when it has a non-empty m_Id. Word count is a simple
whitespace split of the English text, useful for estimating translation
scope/cost.

Usage:
    python LocCountReport.py
"""

from __future__ import annotations

import argparse
import subprocess
import sys
from dataclasses import dataclass
from pathlib import Path
from typing import Dict, List, Optional


LOCALIZATION_RELATIVE_DIR = Path("Assets/Features/Localization/Data/Tables")


class LocCountReportError(Exception):
    pass


@dataclass(frozen=True)
class TableCount:
    table_name: str
    entry_count: int
    word_count: int
    character_count: int


def main() -> int:
    parse_arguments()

    try:
        repository_root = resolve_repository_root()
        localization_directory = repository_root / LOCALIZATION_RELATIVE_DIR
        ensure_localization_directory_exists(localization_directory)

        table_names = discover_table_names(localization_directory)
        if not table_names:
            raise LocCountReportError(
                f"No localization tables found in {localization_directory}"
            )

        table_counts = build_report(localization_directory, table_names)

        print(render_report(table_counts))

        return 0

    except LocCountReportError as error:
        print(f"Error: {error}", file=sys.stderr)
        return 1
    except KeyboardInterrupt:
        print("Error: Cancelled.", file=sys.stderr)
        return 130


def parse_arguments() -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description="Report localization entry and word counts per table."
    )
    return parser.parse_args()


def resolve_repository_root() -> Path:
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
        raise LocCountReportError("Git executable not found.") from error

    if result.returncode != 0 or not result.stdout.strip():
        raise LocCountReportError(
            "Could not determine the git repository root. Run this script inside the repository."
        )

    return Path(result.stdout.strip())


def ensure_localization_directory_exists(localization_directory: Path) -> None:
    if not localization_directory.is_dir():
        raise LocCountReportError(
            f"Localization directory does not exist: {localization_directory}"
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


def build_report(localization_directory: Path, table_names: List[str]) -> List[TableCount]:
    results: List[TableCount] = []

    for table_name in table_names:
        english_path = localization_directory / f"{table_name}_en.asset"
        if not english_path.exists():
            continue

        id_to_english = parse_language_asset(english_path)

        entry_count = len(id_to_english)
        word_count = sum(len(text.split()) for text in id_to_english.values())
        character_count = sum(len(text) for text in id_to_english.values())

        results.append(TableCount(
            table_name=table_name,
            entry_count=entry_count,
            word_count=word_count,
            character_count=character_count,
        ))

    return results


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
                raise LocCountReportError(
                    f"Invalid integer for m_Id in {path.name}, line {line_number}: {value_text}"
                ) from error
            continue

        if stripped.startswith("m_Localized:"):
            if current_id is None:
                raise LocCountReportError(
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


def render_report(table_counts: List[TableCount]) -> str:
    lines: List[str] = []
    lines.append("# Localization Entry & Word Count Report")
    lines.append("")

    total_entries = sum(t.entry_count for t in table_counts)
    total_words = sum(t.word_count for t in table_counts)
    total_characters = sum(t.character_count for t in table_counts)

    name_width = max((len(t.table_name) for t in table_counts), default=5)
    header = f"{'Table'.ljust(name_width)}  {'Entries':>8}  {'Words':>8}  {'Characters':>10}"
    lines.append(header)
    lines.append("-" * len(header))

    for table_count in sorted(table_counts, key=lambda t: t.table_name.lower()):
        lines.append(
            f"{table_count.table_name.ljust(name_width)}  "
            f"{table_count.entry_count:>8}  "
            f"{table_count.word_count:>8}  "
            f"{table_count.character_count:>10}"
        )

    lines.append("-" * len(header))
    lines.append(
        f"{'TOTAL'.ljust(name_width)}  "
        f"{total_entries:>8}  "
        f"{total_words:>8}  "
        f"{total_characters:>10}"
    )
    lines.append("")
    lines.append(f"{len(table_counts)} table(s), {total_entries} entries, {total_words} words.")

    return "\n".join(lines)


if __name__ == "__main__":
    raise SystemExit(main())
