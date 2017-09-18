#!/bin/bash
. $(dirname $0)/lib/logging

# script for running the most common queries that will be needed in the Web API application itself
# (this script will not be invoked from the application but it will be useful when testing SQL queries
# before writing the equivalent LINQ queries needed)

###########################################################################################################################
## Constants 
###########################################################################################################################  

mysql_cmd="mysql" 

# usually this is obtained from db via a query on site name but it will most likely be 
# cached in WebAPI 
site_id=1  
solar_system_id=1

curr_date="2016-08-07"
begin_time="07:00:00"  # begin time for calculating energy for the dashboard data 
num_samples=20 # number of samples for the graphs

# shunts
array_shunt_name="Array"
load_shunt_name="Loads"
battery_shunt_name="Battery"


###########################################################################################################################
## Queries 
###########################################################################################################################  

# Note: format is: "description of the query | SQL QUERY"
# (note the | symbol separating the description from the query, which cannot be used anywhere else in the string)

declare -a options=(

  "List sites | 
      SELECT * FROM Sites;"

  "List users | 
      SELECT * FROM Users;"

  "List shunts | 
      SELECT * FROM Shunts;"

  "List current sensors | 
      SELECT * FROM CurrentSensors;"

  "List temperature sensors | 
      SELECT * FROM TemperatureSensors;"

  "Dashboard: Power in (from solar array) now | 
      SELECT SolarRecords_Timestamp, Current_A, Voltage_V, Current_A*Voltage_V AS Power_W  
      FROM ShuntMeasurements WHERE Shunts_Id IN 
        (select Id from Shunts where SolarSystems_Id=$solar_system_id and Name = \"$array_shunt_name\") 
      ORDER BY SolarRecords_Timestamp DESC LIMIT 1;"

  "Dashboard: Power out (to loads) now | 
      SELECT SolarRecords_Timestamp, Current_A, Voltage_V, Current_A*Voltage_V AS Power_W  
      FROM ShuntMeasurements WHERE Shunts_Id IN 
        (select Id from Shunts where SolarSystems_Id = $solar_system_id and Name = \"$load_shunt_name\") 
      ORDER BY SolarRecords_Timestamp DESC LIMIT 1;"

  "Dashboard: Battery temperature now | 
      SELECT SolarRecords_Timestamp, BatteryTemperature_C  
      FROM ControllerMeasurements WHERE SolarControllers_Id IN 
        (select SolarControllers_Id from SolarSystems where Id = $solar_system_id) 
      ORDER BY SolarRecords_Timestamp DESC LIMIT 1;"

  "Dashboard: Battery SOC now | 
      SELECT SolarRecords_Timestamp, BatterySOC  
      FROM ControllerMeasurements WHERE SolarControllers_Id IN 
        (select SolarControllers_Id from SolarSystems where Id = $solar_system_id) 
      ORDER BY SolarRecords_Timestamp DESC LIMIT 1;"

  "Dashboard: Battery SOC (at 7AM) | 
      SELECT SolarRecords_Timestamp, BatterySOC  
      FROM ControllerMeasurements WHERE DATE(SolarRecords_Timestamp) = CURRENT_DATE AND 
        HOUR(SolarRecords_Timestamp) = $hour AND MINUTE(SolarRecords_Timestamp) = $min AND SolarControllers_Id IN 
        (select SolarControllers_Id from SolarSystems where Id = $solar_system_id) 
      ORDER BY SolarRecords_Timestamp ASC LIMIT 1;"

  "Dashboard: Battery charging state (now) | 
      SELECT SolarRecords_Timestamp, BatteryChargeStatus  
      FROM ControllerMeasurements WHERE SolarControllers_Id IN 
        (select SolarControllers_Id from SolarSystems where Id = $solar_system_id) 
      ORDER BY SolarRecords_Timestamp DESC LIMIT 1;"

  "Dashboard: Battery voltage (now) | 
      SELECT SolarRecords_Timestamp, Voltage_V  
      FROM ShuntMeasurements WHERE Shunts_Id IN 
        (select Id from Shunts where SolarSystems_Id = $solar_system_id and Name = \"$battery_shunt_name\") 
      ORDER BY SolarRecords_Timestamp DESC LIMIT 1;"

  "Dashboard: Energy produced since 7AM | 
      SELECT FORMAT(SUM(ShuntMeasurements.Voltage_V * ShuntMeasurements.Current_A * SolarRecords.Interval_s / 3600), 2) AS TotalEnergy_Wh
      FROM ShuntMeasurements 
      INNER JOIN SolarRecords 
        ON SolarRecords.Timestamp = ShuntMeasurements.SolarRecords_Timestamp 
      WHERE DATE(SolarRecords_Timestamp) = \"$curr_date\" AND TIME(SolarRecords_Timestamp) >= CAST(\"$begin_time\" AS TIME) AND Shunts_Id IN 
        (SELECT Id FROM Shunts WHERE SolarSystems_Id = $solar_system_id AND Name = \"$array_shunt_name\");"

  "Dashboard: Energy produced since 7AM - average | 
      "

  "Dashboard: Energy used since 7AM | 
      SELECT FORMAT(SUM(ShuntMeasurements.Voltage_V * ShuntMeasurements.Current_A * SolarRecords.Interval_s / 3600), 2) AS TotalEnergy_Wh
      FROM ShuntMeasurements 
      INNER JOIN SolarRecords 
        ON SolarRecords.Timestamp = ShuntMeasurements.SolarRecords_Timestamp 
      WHERE DATE(SolarRecords_Timestamp) = \"$curr_date\" AND TIME(SolarRecords_Timestamp) >= CAST(\"$begin_time\" AS TIME) AND Shunts_Id IN 
        (SELECT Id FROM Shunts WHERE SolarSystems_Id = $solar_system_id AND Name = \"$load_shunt_name\");"

  "Dashboard: Energy used since 7AM - average | 
      "

  "Graph: Power in (last 20 samples) | 
      SELECT SolarRecords_Timestamp AS Time, Current_A, Voltage_V, FORMAT(Voltage_V * Current_A, 2) AS Power_W
      FROM ShuntMeasurements 
      WHERE Shunts_Id IN 
        (SELECT Id FROM Shunts WHERE SolarSystems_Id = $solar_system_id AND Name = \"$array_shunt_name\")
      ORDER BY SolarRecords_Timestamp DESC LIMIT $num_samples;"

  "Graph: Power out (last 20 samples) | 
      SELECT SolarRecords_Timestamp AS Time, Current_A, Voltage_V, FORMAT(Voltage_V * Current_A, 2) AS Power_W
      FROM ShuntMeasurements 
      WHERE Shunts_Id IN 
        (SELECT Id FROM Shunts WHERE SolarSystems_Id = $solar_system_id AND Name = \"$load_shunt_name\")
      ORDER BY SolarRecords_Timestamp DESC LIMIT $num_samples;"

  "Graph: Energy produced per day | 
      SELECT DATE(SolarRecords.Timestamp) AS Date,
        FORMAT(SUM(ShuntMeasurements.Voltage_V * ShuntMeasurements.Current_A * SolarRecords.Interval_s / 3600), 2) AS TotalEnergy_Wh
      FROM ShuntMeasurements 
      INNER JOIN SolarRecords 
        ON SolarRecords.Timestamp = ShuntMeasurements.SolarRecords_Timestamp 
      WHERE DATE(SolarRecords_Timestamp) = \"$curr_date\" AND TIME(SolarRecords_Timestamp) >= CAST(\"$begin_time\" AS TIME) AND Shunts_Id IN 
        (SELECT Id FROM Shunts WHERE SolarSystems_Id = $solar_system_id AND Name = \"$array_shunt_name\")
      GROUP BY Date
      ORDER BY Date DESC;"
  )

###########################################################################################################################
## Script body 
###########################################################################################################################  

selected_option=""
query_id=0
query=""

if [[ $# -eq 1 ]]; then
  query_id=$1
  selected_option=""
  query=""
else
  PS3="Select query: "

  declare -a keys
  declare -a values
  for opt in "${options[@]}"
  do
    keys+=("${opt%|*}")
    values+=("${opt#*|}")
  done
  select opt in "${keys[@]}"
  do
    query_id=$(($REPLY-1))
    selected_option=$opt
    query=${values[$(($REPLY-1))]}
    break
  done
fi

echo "
Query description: 
    ${selected_option}
"

echo -e "MySQL query: ${query}\n"
mysql -D SolarMonitorDb -e " ${query}"
