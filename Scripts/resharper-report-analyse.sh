#!/bin/bash

RESHARPER_REPORT_FILE="resharper-report.xml"
RESHARPER_REPORT_FILE_TXT="resharper-report.txt"

if grep -q 'Severity="WARNING"' $RESHARPER_REPORT_FILE; then
    cat $RESHARPER_REPORT_FILE_TXT
    exit 1
fi

if grep -q 'Severity="ERROR"' $RESHARPER_REPORT_FILE; then
    cat $RESHARPER_REPORT_FILE_TXT
    exit 1
fi

if grep -q 'Severity="SUGGESTION"' $RESHARPER_REPORT_FILE; then
    cat $RESHARPER_REPORT_FILE_TXT
    exit 1
fi

echo "No warnings or errors found in Resharper report"