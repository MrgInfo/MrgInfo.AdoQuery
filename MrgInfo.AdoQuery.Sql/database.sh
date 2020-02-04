#!/bin/bash

TOOL_DIR=/opt/mssql-tools/bin
DBA_DIR=/dba
LOG=$DBA_DIR/database.log

if [ ! -f $LOG ]; then
    echo "Waiting for SQL Server..." >$LOG
    sleep 1m
    for f in $(find $DBA_DIR -name *.sql); do
        echo "Running script $f." >>$LOG
        $TOOL_DIR/sqlcmd -S localhost -U SA -P $SA_PASSWORD -d master -l 60 -i $f 2>&1 >>$LOG
    done
    cat $LOG
fi

sleep infinity
