#!/bin/bash

TOOL_DIR=/opt/mssql-tools/bin
DBA_DIR=$HOME/dba
LOG=$DBA_DIR/database.log
READY=$DBA_DIR/database.ready

/opt/mssql/bin/sqlservr &
PID=$!

if [ ! -f $READY ]; then
    STARTTIME=$(date +%s)

    ERR=1
    while [ $ERR -ne 0 ]; do
        echo "Waiting for SQL Server..." >>$LOG
        sleep 20s
        $TOOL_DIR/sqlcmd -H localhost -U sa -P $SA_PASSWORD -l 10 \ 
            -Q "select name from sys.databases where state_desc != 'ONLINE'" \
            | grep -q --no-messages "0 rows affected"
        ERR=$?
    done

    for FILE_NAME in $(find $DBA_DIR -name *.sql); do
        echo "Running script $FILE_NAME." >>$LOG
        $TOOL_DIR/sqlcmd -H localhost -U sa -P $SA_PASSWORD -l 20 -d master -i $FILE_NAME >>$LOG 2>&1
    done

    ENDTIME=$(date +%s)
    echo "Database was created in $[$ENDTIME - $STARTTIME] seconds." >>$LOG

    echo -n "" >$READY
fi

wait $PID
