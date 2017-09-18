#!/bin/bash
. $(dirname $0)/lib/logging

ROOT_DIR=$(echo $(pwd)/$(dirname $0)/..)
RESET_SCHEMA=""

if [[ $# -gt 0 && $1 == "-r" ]]; then
  RESET_SCHEMA="-r"
  $ROOT_DIR/build/mysql_load_main_data.sh $RESET_SCHEMA
fi

loginfo "Loading data records into the database (SolarRecords, ShuntsMeasurements, LoadMeasurements, ControllerMeasurements)..."
loginfo "Start time: $(date +%T)"
mysql < $ROOT_DIR/src/SolarMonitorDb/src/SampleRecords.sql
loginfo "End time: $(date +%T)"
