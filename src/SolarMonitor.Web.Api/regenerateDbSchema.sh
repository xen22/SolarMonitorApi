#!/bin/bash

DBCONTEXT_PROJECT=../SolarMonitor.Data.Repositories.MySql/SolarMonitor.Data.Repositories.MySql.csproj
MIGRATIONS_PATH=../SolarMonitor.Data.Repositories.MySql

rm -rf ${MIGRATIONS_PATH}/Migrations 
dotnet ef migrations -p ${DBCONTEXT_PROJECT} add m1 
dotnet ef migrations -p ${DBCONTEXT_PROJECT} script -o ../SolarMonitorDb/src/ef/SolarMonitorDbSchema.sql
