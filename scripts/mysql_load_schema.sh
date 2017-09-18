#!/bin/bash
. $(dirname $0)/lib/logging

ROOT_DIR=$(echo $(pwd)/$(dirname $0)/..)

loginfo "Generating sql script from mwb (MySQL Workbench) diagram."
$ROOT_DIR/build/mwb2sql.sh $ROOT_DIR/src/SolarMonitorDb/src/MysqlModel.mwb $ROOT_DIR/_output/SolarMonitorDb.sql

loginfo "Loading the schema into the database."
mysql < $ROOT_DIR/_output/SolarMonitorDb.sql
