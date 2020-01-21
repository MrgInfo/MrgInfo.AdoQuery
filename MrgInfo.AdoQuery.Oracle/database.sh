#!/bin/bash

TOOL_DIR=$ORACLE_HOME/bin
DBA_DIR=$HOME/dba
LOG=$DBA_DIR/database.log

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
    for f in $(find $DBA_DIR -name *.sql); do
        echo "Running script $f." >>$LOG
        BASH_RC=/home/oracle/.bashrc
        source $BASH_RC
        #$TOOL_DIR/
        sqlplus -L / as sysdba @$f 2>&1 >>$LOG
    done
fi
