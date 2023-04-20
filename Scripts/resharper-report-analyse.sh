REPORT_FILE="resharper-report.xml"
REPORT_FILE_TXT="resharper-report.txt"

if grep -q 'Severity="WARNING"' $REPORT_FILE; then
    cat $REPORT_FILE_TXT
    exit 1
fi

if grep -q 'Severity="ERROR"' $REPORT_FILE; then
    cat $REPORT_FILE_TXT
    exit 1
fi

if grep -q 'Severity="SUGGESTION"' $REPORT_FILE; then
    cat $REPORT_FILE_TXT
    exit 1
fi