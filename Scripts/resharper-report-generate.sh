#!/bin/bash

# Parse script arguments
while getopts ":p:" opt; do
  case ${opt} in
    p )
      CSPROJ_PATH=$OPTARG
      ;;
    \? )
      echo -e "\033[31mInvalid option: -$OPTARG\033[0m" 1>&2
      echo "Usage: $0 -p <csproj_path>" >&2
      exit 1
      ;;
    : )
      echo -e "\033[31mInvalid option: -$OPTARG requires an argument\033[0m" 1>&2
      echo "Usage: $0 -p <csproj_path>" >&2
      exit 1
      ;;
  esac
done

# Check if csproj path is provided
if [[ -z "$csproj_path" ]]; then
  echo -e "\033[31mError: You must provide a path to the csproj file using the -p option.\033[0m"
  exit 1
fi

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