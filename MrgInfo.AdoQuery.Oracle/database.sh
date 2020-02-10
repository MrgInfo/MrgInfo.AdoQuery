#!/bin/bash

BASH_RC=$HOME/.bashrc
DBA_DIR=$HOME/dba
LOG=$DBA_DIR/database.log

/home/oracle/setup/dockerInit.sh &
PID=$!

if [ ! -f $LOG ]; then
    echo "Waiting for Oracle Database..." >$LOG
    while true; do
        PIDS=`pgrep -f tnslsnr`
        if [ -z "$PIDS" ]; then
            sleep 2m
        else
            break
        fi
    done
    source $BASH_RC
    for file in $(find $DBA_DIR -name *.sql); do
        echo "Running script $file." >>$LOG
        sqlplus -L / as sysdba @$file 2>&1 >>$LOG
    done
fi

wait $PID
