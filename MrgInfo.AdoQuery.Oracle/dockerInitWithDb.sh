#!/bin/sh

# basic parameters
LOG_DIR=/home/oracle/setup/log
SETUP_DIR=/home/oracle/setup

if [ ! -d $LOG_DIR ]
then
    mkdir $LOG_DIR
    chmod 775 $LOG_DIR
    chown oracle:oinstall $LOG_DIR
fi

# logfile
INIT_LOG=$LOG_DIR/dockerInit.log
echo `date` >> $INIT_LOG

# check setup path
if [ ! -d $SETUP_DIR ]
then
    echo "ERROR : setup files are not found"
    echo "ERROR : setup files are not found" >> $INIT_LOG
    echo "" >> $INIT_LOG
    exit 1
fi

# check whether it is the first time this container is up
# if it is the first time, setup the DB
# if not, startup the existing db
if [ -f /home/oracle/setup/log/setupDB.log ]
then
    echo "Start up Oracle Database"
    echo "Start up Oracle Database" >> $INIT_LOG
    /bin/bash $SETUP_DIR/startupDB.sh 2>&1
else
    echo "Setup Oracle Database"
    echo "Setup Oracle Database" >> $INIT_LOG
    /bin/bash $SETUP_DIR/setupDB.sh 2>&1
fi

echo "" >> $INIT_LOG

# create database
if [ ! -f $LOG_DIR/database.log ]; then
    cat $SETUP_DIR/database.sql | sqlplus sys/$DB_PASSWD@//localhost:1521/$DB_SID as sysdba 2>&1 >$LOG_DIR/database.log
    cat $LOG_DIR/database.log
fi

# remove passwd param
unset DB_PASSWD

# basic parameters
BASH_RC=/home/oracle/.bashrc
source ${BASH_RC}

# Define SIGTERM-handler for graceful shutdown
term_handler() {
  if [ $childPID -ne 0 ]; then
    /bin/bash $SETUP_DIR/shutDB.sh
  fi
  exit 143; # 128 + 15 -- SIGTERM
}

# setup SIGTERM Handler
trap 'kill ${!}; term_handler' SIGTERM

# keep container runing
ALERT_LOG=/u01/app/oracle/diag/rdbms/${ORACLE_SID,,}/${ORACLE_SID}/trace/alert_${ORACLE_SID}.log
tail -f $ALERT_LOG &
childPID=$!
wait $childPID
