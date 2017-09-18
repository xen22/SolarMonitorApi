#!/bin/bash

DB_DIR=$(dirname $0)
echo "Command running in: ${DB_DIR}"

echo "Destroying database SolarMonitorDb"
mysql --execute "DROP DATABASE SolarMonitorDb;"

echo "Recreating database SolarMonitorDb"
mysql --execute "CREATE DATABASE SolarMonitorDb;"

echo "Loading db schema."
mysql --database SolarMonitorDb < ${DB_DIR}/ef/SolarMonitorDbSchema.sql

echo "Loading main data."
mysql --database SolarMonitorDb < ${DB_DIR}/MainData.sql

echo "Loading sample measurements."
#mysql --database SolarMonitorDb < ${DB_DIR}/SampleMeasurements.sql

echo "Database SolarMonitorDb: "
mysql --execute "SHOW DATABASES ; SHOW TABLES FROM SolarMonitorDb;"

echo "Creating solarmonitor user with readonly access to SolarMonitorDb"

secrets_file="$HOME/.microsoft/usersecrets/SolarMonitorSecrets/secrets.json"
if [ ! -f ${secrets_file} ]; then
    echo "Error: Secrets file not found at ${secrets_file}"
    echo "Generate secret with the following command (in src/SolarMonitor.Web.Api directory):"
    echo "  dotnet user-secrets set solarmonitor_mysql_pwd  <password>"
    echo "Skipping solarmonitor user creation."
    exit 1;
fi
mysql_pwd=$(jq '.solarmonitor_mysql_pwd' ${secrets_file} | tr '"' "'")
mysql --execute "CREATE USER IF NOT EXISTS 'solarmonitor'@'%' IDENTIFIED BY ${mysql_pwd};"
mysql --execute "GRANT SELECT,INSERT,DELETE ON SolarMonitorDb.* TO 'solarmonitor'@'%';"
