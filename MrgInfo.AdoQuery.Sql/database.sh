#!/bin/bash

TOOL_DIR=/opt/mssql-tools/bin
DBA_DIR=/dba
LOG=$DBA_DIR/database.log

/opt/mssql/bin/sqlservr &
PID=$!

if [ ! -f $LOG ]; then
    echo "Waiting for SQL Server..." >$LOG
    sleep 1m
    for file in $(find $DBA_DIR -name *.sql); do
        echo "Running script $file." >>$LOG
        $TOOL_DIR/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d master -l 120 -i $file 2>&1 >>$LOG
    done
    cat $LOG
fi

wait $PID
