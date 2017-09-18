-- This script contains the stored procedures for the SolarMonitorDb database
-- Normally, the stored procedures should be part of the MWB model file (MysqlModel.mwb), however
-- the automatic conversion from mwb to sql (see scripts/mwb2sql.sh) does not convert these stored procedures
-- correctly, so we have to import them manually.


DELIMITER $$

DROP PROCEDURE IF EXISTS spEnergyFromShunts;
CREATE PROCEDURE spEnergyFromShunts(
  siteName VARCHAR(45),
  shuntName VARCHAR(45),
  startTime DATETIME, 
  endTime DATETIME,
  period ENUM('Hourly', 'Daily', 'Monthly', 'Yearly'))

BEGIN
  DROP TEMPORARY TABLE IF EXISTS EnergyFromShunts_Temp;

  SET @fmt = "";
  IF period = 'Hourly' THEN SET @fmt = "%Y/%m/%d %H:00:00";
  ELSEIF period = 'Daily' THEN SET @fmt = "%Y/%m/%d 00:00:00";
  ELSEIF period = 'Monthly' THEN SET @fmt = "%Y/%m/01 00:00:00";
  ELSEIF period = 'Yearly' THEN SET @fmt = "%Y/01/01 00:00:00";
  END IF;

  IF period = 'Hourly' THEN
    CREATE TEMPORARY TABLE EnergyFromShunts_Temp  AS
    SELECT
      DATE(SolarRecords_Timestamp) AS Date,
      TIME_FORMAT(SolarRecords_Timestamp, '%H:00') AS Time,
      FORMAT(SUM(Voltage_V * Current_A * Interval_s / 3600), 2) AS Energy_Wh
        FROM
          ShuntMeasurements
      WHERE Sites_Id IN (SELECT Id FROM Sites WHERE Name = siteName) AND
            Shunts_Id IN (SELECT Id FROM Shunts WHERE Name = shuntName) AND
              SolarRecords_Timestamp BETWEEN startTime AND endTime
      GROUP BY Date, Time;
  ELSEIF period = 'Daily' THEN SET @fmt = "%Y/%m/%d 00:00:00";
    CREATE TEMPORARY TABLE EnergyFromShunts_Temp  AS
    SELECT
      DATE(SolarRecords_Timestamp) AS Date,
      FORMAT(SUM(Voltage_V * Current_A * Interval_s / 3600), 2) AS Energy_Wh
        FROM
          ShuntMeasurements
      WHERE Sites_Id IN (SELECT Id FROM Sites WHERE Name = siteName) AND
            Shunts_Id IN (SELECT Id FROM Shunts WHERE Name = shuntName) AND
              SolarRecords_Timestamp BETWEEN startTime AND endTime
      GROUP BY Date;
  ELSEIF period = 'Monthly' THEN SET @fmt = "%Y/%m/01 00:00:00";
    CREATE TEMPORARY TABLE EnergyFromShunts_Temp  AS
    SELECT
      YEAR(SolarRecords_Timestamp) AS Year,
      MONTH(SolarRecords_Timestamp) AS Month,
      FORMAT(SUM(Voltage_V * Current_A * Interval_s / 3600), 2) AS Energy_Wh
        FROM
          ShuntMeasurements
      WHERE Sites_Id IN (SELECT Id FROM Sites WHERE Name = siteName) AND
            Shunts_Id IN (SELECT Id FROM Shunts WHERE Name = shuntName) AND
              SolarRecords_Timestamp BETWEEN startTime AND endTime
      GROUP BY Year, Month;
  ELSEIF period = 'Yearly' THEN SET @fmt = "%Y/01/01 00:00:00";
    CREATE TEMPORARY TABLE EnergyFromShunts_Temp  AS
    SELECT
      YEAR(SolarRecords_Timestamp) AS Year,
      FORMAT(SUM(Voltage_V * Current_A * Interval_s / 3600), 2) AS Energy_Wh
        FROM
          ShuntMeasurements
      WHERE Sites_Id IN (SELECT Id FROM Sites WHERE Name = siteName) AND
            Shunts_Id IN (SELECT Id FROM Shunts WHERE Name = shuntName) AND
              SolarRecords_Timestamp BETWEEN startTime AND endTime
      GROUP BY Year;
  END IF;
    

    SELECT * FROM EnergyFromShunts_Temp;
END;
$$
DELIMITER ;

-- Examples:
-- CALL spEnergyFromShunts("Simulation 1", "Array", '2016-05-07 00:00:00', '2016-08-07 23:59:59', 'Hourly');
-- SHOW PROCEDURE STATUS LIKE 'spEnergyFromShunts';


