#!/bin/bash
# Convert Markdown via pandoc.
# Usage:
#   ./convert.sh <directory> [docx|pdf]  - convert all .md files in a directory
#   ./convert.sh <file.md> [docx|pdf]    - convert a single file
#
# Default format is docx.

set -euo pipefail

if ! command -v pandoc &>/dev/null; then
    echo "Error: pandoc is not installed. Install it: sudo pacman -S pandoc"
    exit 1
fi

if [[ $# -lt 1 ]]; then
    echo "Usage:"
    echo "  $0 <directory> [docx|pdf]  - convert all .md files in a directory"
    echo "  $0 <file.md> [docx|pdf]    - convert a single file"
    exit 1
fi

target="$1"
format="${2:-docx}"

if [[ "$format" != "docx" && "$format" != "pdf" ]]; then
    echo "Error: unsupported format '$format'"
    echo "Supported formats: docx, pdf"
    exit 1
fi

convert_file() {
    local src="$1"
    local dst="${src%.md}.${format}"
    local src_dir
    src_dir="$(dirname "$src")"

    if [[ "$format" == "docx" ]]; then
        pandoc "$src" --resource-path="$src_dir" -o "$dst"
    else
        # Preferred path for robust image support: markdown -> docx -> pdf.
        local tmp_docx
        tmp_docx="$(mktemp "$src_dir/.pandoc-pdf-XXXXXX.docx")"
        pandoc "$src" --resource-path="$src_dir" -o "$tmp_docx"

        if command -v soffice &>/dev/null; then
            local tmp_pdf
            tmp_pdf="${tmp_docx%.docx}.pdf"
            soffice --headless --convert-to pdf --outdir "$src_dir" "$tmp_docx" >/dev/null 2>&1

            if [[ -f "$tmp_pdf" ]]; then
                mv -f "$tmp_pdf" "$dst"
            else
                rm -f "$tmp_docx"
                echo "Error: failed to convert DOCX to PDF via soffice"
                exit 1
            fi
        else
            # Fallback path when LibreOffice is unavailable.
            pandoc "$src" --resource-path="$src_dir" -o "$dst"
        fi

        rm -f "$tmp_docx"
    fi

    echo "OK: $src -> $dst"
}

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
