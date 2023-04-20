#!/bin/bash

# Parse script arguments
while [[ $# -gt 0 ]]; do
  key="$1"
  case $key in
    -p|--project)
      CSPROJ_PATH="$2"
      shift
      shift
      ;;
    *)    # unknown option
      echo -e "\033[31mInvalid argument: $1\033[0m"
      echo "Usage: $0 -p <csproj_path>"
      exit 1
      ;;
  esac
done

# Generate Resharper report
jb inspectcode "$CSPROJ_PATH" \
          -o=resharper-report.xml \
          -f=Xml \
          -verbosity=INFO \
          -no-swea
          
jb inspectcode "$CSPROJ_PATH" \
          -o=resharper-report.txt \
          -f=Text \
          -verbosity=INFO \
          -no-swea