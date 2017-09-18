#!/bin/bash
. $(dirname $0)/lib/logging

ROOT_DIR=$(echo $(pwd)/$(dirname $0)/..)

if [[ $# -gt 0 && $1 == "-r" ]]; then
  $ROOT_DIR/build/mysql_load_schema.sh
fi

loginfo "Loading data into the main tables (Sites, Users, SolarSystems, BatteryBanks, SolarControllers, SolarArrays, Inverters, LoadTypes, CurrentSensors, Shunts)..."
mysql < $ROOT_DIR/src/SolarMonitorDb/src/SampleData.sql
