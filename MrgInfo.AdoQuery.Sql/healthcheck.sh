#!/bin/bash

TOOL_DIR=/opt/mssql-tools/bin

$TOOL_DIR/sqlcmd -H localhost -U sa -P "$SA_PASSWORD" -l 1 -t 1 -Q "select name from sys.databases where state_desc != 'ONLINE'" | grep --quiet '0 rows affected' || exit 1
