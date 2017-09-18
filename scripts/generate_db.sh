#!/bin/bash
. $(dirname $0)/lib/logging

ROOT_DIR=$(realpath $(pwd)/$(dirname $0)/..)

PI_HOST="pi-solar.local"
UPDATE_DB=0
if [[ $# > 0 ]]; then
  if [[ $1 == "-f" ]]; then
    UPDATE_DB=1
  fi
  if [[ $# > 1 ]]; then
    PI_HOST=$2
  fi
fi


#############################################################################################
# Deleting local db
#############################################################################################

loginfo "Destroying database SolarMonitorDb"
mysql --execute "DROP DATABASE SolarMonitorDb;"


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


#############################################################################################
# Recreating local db
#############################################################################################

cd ${ROOT_DIR}/src/SolarMonitorDb/src/

loginfo "Recreating database SolarMonitorDb"
mysql --execute "CREATE DATABASE SolarMonitorDb;"

loginfo "Loading db schema."
mysql --database SolarMonitorDb < ./ef/SolarMonitorDbSchema.sql

loginfo "Loading main data."
mysql --database SolarMonitorDb < ./SampleData.sql


#############################################################################################
# Copying files to pi-solar
#############################################################################################

loginfo "Copying files to ${PI_HOST}"
scp ef/SolarMonitorDbSchema.sql ${PI_HOST}:db/
scp recreate_db.sh ${PI_HOST}:db/
scp SampleData.sql ${PI_HOST}:db/


#############################################################################################
# Recreating db on pi-solar
#############################################################################################

if [[ $# > 0 && $UPDATE_DB == 1 ]]; then
  loginfo "Recreating SolarMonitorDb on ${PI_HOST}"
  ssh ${PI_HOST} /home/sol/db/recreate_db.sh
fi 
