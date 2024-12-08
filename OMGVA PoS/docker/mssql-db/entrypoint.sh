#!/usr/bin/bash

echo "Waiting for SQL Server to start..."

# Start SQL Server in the background
/opt/mssql/bin/sqlservr &

# Wait for SQL Server to be ready
sleep 30

# Run the init script
/init-scripts/init-db.sh

# Wait for SQL Server process to prevent container exit
wait