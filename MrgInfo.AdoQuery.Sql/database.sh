#!/bin/bash

TOOL_DIR=/opt/mssql-tools/bin
DBA_DIR=$HOME/dba
LOG=$DBA_DIR/database.log
READY=$DBA_DIR/database.ready

/opt/mssql/bin/sqlservr &
PID=$!

if [ ! -f $READY ]; then
    echo $(date) >>$LOG
    echo "Waiting for SQL Server..." >>$LOG
    
    CNT=0
    while [ $CNT -eq 0 ]; do
        sleep 20s
        CNT=$(pgrep -f sqlservr -c)
        echo "$CNT instances of sqlservr are running." >>$LOG
    done
    sleep 30s
    
    for FILE_NAME in $(find $DBA_DIR -name *.sql); do
        echo "Running script $FILE_NAME." >>$LOG
        $TOOL_DIR/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d master -i $FILE_NAME -l 60 >>$LOG 2>&1
    done
    
    echo -n "" >$READY
fi

wait $PID
