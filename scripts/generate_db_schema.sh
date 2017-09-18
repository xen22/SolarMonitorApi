#!/bin/bash
. $(dirname $0)/lib/logging

# Note: this script can be used to generate a schema SQL script without destroying the local 
# SolarMonitorDb database.


ROOT_DIR=$(realpath $(pwd)/$(dirname $0)/..)


#############################################################################################
# Deleting EFMigrationHistory from local db
#############################################################################################

#loginfo "Deleting __EFMigrationHistory table from database SolarMonitorDb"
#/usr/bin/mysql --execute "DROP TABLE IF EXISTS SolarMonitorDb.__EFMigrationsHistory;" > /dev/null


#############################################################################################
# Generating schema for db
#############################################################################################

cd ${ROOT_DIR}/src/SolarMonitor.Web.Api

loginfo "Deleting previous migrations"
rm -rf ./Migrations

loginfo "Creating migration"
dotnet ef migrations add m1

loginfo "Generating db SQL script"
dotnet ef migrations script -o ${ROOT_DIR}/src/SolarMonitorDb/src/ef/SolarMonitorDbSchema.sql

exit 0
