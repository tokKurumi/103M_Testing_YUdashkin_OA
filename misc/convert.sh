#!/bin/bash
# Convert Markdown to DOCX via pandoc
# Usage:
#   ./convert.sh <directory>  — convert all .md files in a directory
#   ./convert.sh <file.md>    — convert a single file

set -euo pipefail

if ! command -v pandoc &>/dev/null; then
    echo "Error: pandoc is not installed. Install it: sudo pacman -S pandoc"
    exit 1
fi

if [[ $# -lt 1 ]]; then
    echo "Usage:"
    echo "  $0 <directory>  — convert all .md files in a directory"
    echo "  $0 <file.md>    — convert a single file"
    exit 1
fi

convert_file() {
    local src="$1"
    local dst="${src%.md}.docx"
    pandoc "$src" -o "$dst"
    echo "OK: $src -> $dst"
}

target="$1"

if [[ -d "$target" ]]; then
    count=0
    while IFS= read -r -d '' file; do
        convert_file "$file"
        ((count++))
    done < <(find "$target" -maxdepth 1 -name '*.md' -print0 | sort -z)

    if [[ $count -eq 0 ]]; then
        echo "No .md files found in '$target'"
        exit 1
    fi
    echo "Done: $count file(s) converted"

elif [[ -f "$target" && "$target" == *.md ]]; then
    convert_file "$target"

else
    echo "Error: '$target' is not a directory or .md file"
    exit 1
fi
