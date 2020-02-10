#!/bin/bash

TOOL_DIR=/opt/mssql-tools/bin
DBA_DIR=$HOME/dba
LOG=$DBA_DIR/database.log

/opt/mssql/bin/sqlservr &
PID=$!

if [ ! -f $LOG ]; then
    echo "Waiting for SQL Server..." >$LOG
    while true; do
        PIDS=`pgrep -f sqlservr`
        if [ -z "$PIDS" ]; then
            sleep 2m
        else
            break
        fi
    done
    for file in $(find $DBA_DIR -name *.sql); do
        echo "Running script $file." >>$LOG
        $TOOL_DIR/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d master -l 120 -i $file 2>&1 >>$LOG
    done
fi

wait $PID
