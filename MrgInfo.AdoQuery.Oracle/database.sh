#!/bin/bash

TOOL_DIR=/u01/app/oracle/product/12.2.0/dbhome_1/bin
DBA_DIR=$HOME/dba
LOG=$DBA_DIR/database.log
READY=$DBA_DIR/database.ready

$HOME/setup/dockerInit.sh &
PID=$!

if [ ! -f $READY ]; then
    echo $(date) >>$LOG
    echo "Waiting for Oracle Database..." >>$LOG
    STATUS=0
    while [ $STATUS -eq 0 ]; do
        sleep 20s
        STATUS=$(pgrep -f tnslsnr | wc -l)
    done
    source $HOME/.bashrc
    for FILE_NAME in $(find $DBA_DIR -name *.sql); do
        echo "Running script $FILE_NAME." >>$LOG
        $TOOL_DIR/sqlplus -L / as sysdba @$FILE_NAME >>$LOG 2>&1
    done
    echo -n "" >$READY
fi

wait $PID
