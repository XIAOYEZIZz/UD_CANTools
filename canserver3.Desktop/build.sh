#!/bin/sh

set -eu

project_dir=$(CDPATH= cd -- "$(dirname -- "$0")" && pwd)
cd "$project_dir"

runtime=${1:-}
if [ -z "$runtime" ]; then
    case "$(uname -s)-$(uname -m)" in
        Darwin-arm64) runtime=osx-arm64 ;;
        Darwin-x86_64) runtime=osx-x64 ;;
        *) echo "Usage: sh build.sh {osx-arm64|osx-x64|win-x86|win-x64}" >&2; exit 1 ;;
    esac
fi

case "$runtime" in
    osx-arm64|osx-x64) ;;
    x86|win-x86) runtime=win-x86 ;;
    x64|win-x64) runtime=win-x64 ;;
    *) echo "Unsupported runtime: $runtime" >&2; exit 1 ;;
esac

dotnet publish "$project_dir/canserver3.Desktop.csproj" -c Release -r "$runtime" --self-contained true