#!/usr/bin/bash

echo "Running setup.sql"

/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -d master -i init-scripts/setup.sql -C
if [ $? -eq 0 ]
then
    echo "SUCCESS: setup.sql completed"
else
    echo "ERROR: setup.sql failed"
fi