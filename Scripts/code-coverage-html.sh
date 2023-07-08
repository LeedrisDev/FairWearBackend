#!/bin/bash

# Parse arguments
while [[ $# -gt 0 ]]; do
  key="$1"
  case $key in
    -p|--project)
      PROJECT="$2"
      PROJECT_DIR=$(dirname "$2")
      shift
      shift
      ;;
    --clean) # delete generated files
      rm -rf coveragereport
      rm -f "$PROJECT_DIR"/coverage.cobertura.xml
      rm -rf "$PROJECT_DIR"/TestResults
      exit 0
      shift
      ;;
    *)    # unknown option
      echo "Invalid argument: $1"
      echo "Usage: $0 -p|--project <project_test_path> [--clean]"
      exit 1
      ;;
  esac
done

# Check required arguments
if [ -z "$PROJECT" ]; then
  echo "Project path is required"
  echo "Usage: $0 -p|--project <project_test_path> [--clean]"
  exit 1
fi

# Run unit tests with code coverage
dotnet test --no-restore --verbosity normal /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:ExcludeByFile=**/obj/** "$PROJECT"
# Generate code coverage report with ReportGenerator
dotnet "$HOME"/.nuget/packages/reportgenerator/*/tools/net7.0/ReportGenerator.dll -reports:"$PROJECT_DIR"/coverage.cobertura.xml -targetdir:coveragereport -reporttypes:Html

# Open code coverage report in browser
open coveragereport/index.html
