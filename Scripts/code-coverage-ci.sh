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
    -t|--threshold)
      THRESHOLD="$2"
      shift
      shift
      ;;
    *)    # unknown option
      echo "Invalid argument: $1"
      echo "Usage: $0 -p|--project <project_path> -t|--threshold <coverage_threshold>"
      exit 1
      ;;
  esac
done

# Check required arguments
if [ -z "$PROJECT" ]; then
  echo "Project path is required"
  echo "Usage: $0 -p|--project <project_path> -t|--threshold <coverage_threshold>"
  exit 1
fi

if [ -z "$THRESHOLD" ]; then
  echo "Coverage threshold is required"
  echo "Usage: $0 -p|--project <project_path> -t|--threshold <coverage_threshold>"
  exit 1
fi

# Run unit tests with code coverage
dotnet test --no-restore --verbosity normal "$PROJECT" /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura

# Generate code coverage report with ReportGenerator
dotnet "$HOME"/.nuget/packages/reportgenerator/*/tools/net7.0/ReportGenerator.dll -reports:"$PROJECT_DIR"/coverage.cobertura.xml -targetdir:coveragereport -reporttypes:JsonSummary

# Extract code coverage percentage from summary file
coverage=$(sed -n 's/.*"linecoverage":\s*\([^,]*\).*/\1/p' coveragereport/Summary.json | tr -d ' ')

# Check if coverage is below threshold
if (( $(echo "$coverage < $THRESHOLD" | bc -l) )); then
  echo -e "\033[0;31mCode coverage is below threshold of $THRESHOLD% (coverage is $coverage%). Failing pipeline...\033[0m"
  exit 1
else
  echo -e "\033[0;32mCode coverage is $coverage%, which meets the threshold of $THRESHOLD%. Pipeline continues...\033[0m"
fi
