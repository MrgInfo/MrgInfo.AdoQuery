#!/bin/bash


TOOL_DIR=/u01/app/oracle/product/12.2.0/dbhome_1/bin
DBA_DIR=$HOME/dba
LOG=$DBA_DIR/database.log
READY=$DBA_DIR/database.ready

$HOME/setup/dockerInit.sh &
PID=$!

sleep 30s
source $HOME/.bashrc

if [ ! -f $READY ]; then
    STARTTIME=$(date +%s)
    
    ERR=1
    while [ $ERR -ne 0 ]; do
        echo "Waiting for Oracle Database..." >>$LOG
        sleep 20s
        $TOOL_DIR/sqlplus -L <<EOF
whenever oserror exit 1;
whenever sqlerror exit sql.sqlcode;
connect / as sysdba;
select sysdate from dual;
exit;
EOF
        ERR=$?
    done

    for FILE_NAME in $(find $DBA_DIR -name *.sql); do
        echo "Running script $FILE_NAME." >>$LOG
        $TOOL_DIR/sqlplus -L / as sysdba @$FILE_NAME >>$LOG 2>&1
    done

    ENDTIME=$(date +%s)
    echo "Database was created in $[$ENDTIME - $STARTTIME] seconds." >>$LOG 

    echo -n "" >$READY
fi

wait $PID
