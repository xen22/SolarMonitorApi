-- MySQL Script generated by MySQL Workbench
-- Sun 14 Aug 2016 15:55:44 NZST
-- Model: New Model    Version: 1.0
-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema SolarMonitorDb
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema SolarMonitorDb
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `SolarMonitorDb` DEFAULT CHARACTER SET utf8 ;
USE `SolarMonitorDb` ;

-- -----------------------------------------------------
-- Table `SolarMonitorDb`.`BatteryBanks`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`BatteryBanks` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `CapacityPerBattery_Ah` INT NOT NULL,
  `TotalCapacity_Ah` INT NOT NULL,
  `BankVoltage_V` INT NOT NULL,
  `BatteryVoltage_V` INT NOT NULL,
  `Configuration` VARCHAR(5) NOT NULL,
  `NumBatteries` INT NOT NULL,
  `Manufacturer` VARCHAR(16) NULL,
  `Name` VARCHAR(45) NOT NULL,
  `Description` VARCHAR(45) NULL,
  PRIMARY KEY (`Id`))
ENGINE = MyISAM;


-- -----------------------------------------------------
-- Table `SolarMonitorDb`.`SolarControllers`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`SolarControllers` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `BatteryBanks_Id` INT NOT NULL,
  `Name` VARCHAR(45) NOT NULL,
  `Description` VARCHAR(255) NULL,
  `Manufacturer` VARCHAR(16) NULL,
  `CurrentRating_A` FLOAT NOT NULL,
  `DetailedSpecs` VARCHAR(255) NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_SolarControllerInfo_BatteryBankInfo1_idx` (`BatteryBanks_Id` ASC))
ENGINE = MyISAM;


-- -----------------------------------------------------
-- Table `SolarMonitorDb`.`SolarArrays`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`SolarArrays` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `SolarControllers_Id` INT NOT NULL,
  `Configuration` VARCHAR(5) NOT NULL,
  `PanelMaxPower_W` INT NOT NULL,
  `PanelOpenCircuitVoltage_V` FLOAT NOT NULL,
  `PanelShortCircuitCurrent_A` FLOAT NOT NULL,
  `Manufacturer` VARCHAR(45) NULL,
  `NumPanels` INT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_SolarArrayInfo_SolarControllerInfo1_idx` (`SolarControllers_Id` ASC))
ENGINE = MyISAM;


-- -----------------------------------------------------
-- Table `SolarMonitorDb`.`Inverters`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`Inverters` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `BatteryBanks_Id` INT NOT NULL,
  `Name` VARCHAR(16) NOT NULL,
  `Description` VARCHAR(255) NULL,
  `Manufacturer` VARCHAR(16) NULL,
  `MaxContinuousPower_W` FLOAT NOT NULL,
  `InputVoltage_V` INT NOT NULL,
  `OutputVoltage_V` INT NOT NULL,
  `DetailedSpecs` VARCHAR(255) NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_InverterInfo_BatteryBankInfo1_idx` (`BatteryBanks_Id` ASC))
ENGINE = MyISAM;


-- -----------------------------------------------------
-- Table `SolarMonitorDb`.`SolarSystems`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`SolarSystems` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `SolarArrays_Id` INT NOT NULL,
  `SolarControllers_Id` INT NOT NULL,
  `Inverters_Id` INT NULL,
  `BatteryBanks_Id` INT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_SystemInfo_SolarArrayInfo1_idx` (`SolarArrays_Id` ASC),
  INDEX `fk_SystemInfo_SolarControllerInfo1_idx` (`SolarControllers_Id` ASC),
  INDEX `fk_SystemInfo_InverterInfo1_idx` (`Inverters_Id` ASC),
  INDEX `fk_SystemInfo_BatteryBankInfo1_idx` (`BatteryBanks_Id` ASC))
ENGINE = MyISAM;


-- -----------------------------------------------------
-- Table `SolarMonitorDb`.`Sites`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`Sites` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `SolarSystems_Id` INT NOT NULL,
  `Name` VARCHAR(45) NOT NULL,
  `Timezone` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_Sites_SystemInfo_idx` (`SolarSystems_Id` ASC),
  UNIQUE INDEX `Name_UNIQUE` (`Name` ASC))
ENGINE = MyISAM;


-- -----------------------------------------------------
-- Table `SolarMonitorDb`.`LoadTypes`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`LoadTypes` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `VoltageType` ENUM('AC', 'DC') NOT NULL,
  `Voltage_V` INT NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = MyISAM;


-- -----------------------------------------------------
-- Table `SolarMonitorDb`.`CurrentSensors`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`CurrentSensors` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `LoadTypes_Id` INT NOT NULL,
  `SolarSystems_Id` INT NOT NULL,
  `Name` VARCHAR(45) NOT NULL,
  `Description` VARCHAR(255) NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_LoadDesriptors_LoadType1_idx` (`LoadTypes_Id` ASC),
  INDEX `fk_CurrentSensors_SolarSystems1_idx` (`SolarSystems_Id` ASC))
ENGINE = MyISAM;


-- -----------------------------------------------------
-- Table `SolarMonitorDb`.`SolarRecords`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`SolarRecords` (
  `Timestamp` DATETIME NOT NULL,
  `Sites_Id` INT NOT NULL,
  `Saved` TINYINT(1) NULL,
  PRIMARY KEY (`Timestamp`),
  INDEX `fk_SolarRecords_Sites1_idx` (`Sites_Id` ASC),
  UNIQUE INDEX `Timestamp_UNIQUE` (`Timestamp` ASC))
ENGINE = MyISAM;


-- -----------------------------------------------------
-- Table `SolarMonitorDb`.`LoadMeasurements`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`LoadMeasurements` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Sites_Id` INT NOT NULL,
  `CurrentSensors_Id` INT NOT NULL,
  `SolarRecords_Timestamp` DATETIME NOT NULL,
  `CurrentDraw_A` FLOAT NOT NULL,
  `Interval_s` INT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_LoadRecords_LoadDescriptors1_idx` (`CurrentSensors_Id` ASC),
  INDEX `fk_LoadMeasurements_SolarRecords1_idx` (`SolarRecords_Timestamp` ASC),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC),
  INDEX `fk_LoadMeasurements_Sites1_idx` (`Sites_Id` ASC))
ENGINE = MyISAM;


-- -----------------------------------------------------
-- Table `SolarMonitorDb`.`Shunts`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`Shunts` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `SolarSystems_Id` INT NOT NULL,
  `Name` VARCHAR(45) NOT NULL,
  `Description` VARCHAR(255) NULL,
  `InternalResistor_mOhm` FLOAT NOT NULL,
  `MaxCurrent_A` FLOAT NOT NULL,
  `InternalVoltage_mV` FLOAT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_Shunts_SolarSystems1_idx` (`SolarSystems_Id` ASC))
ENGINE = MyISAM;


-- -----------------------------------------------------
-- Table `SolarMonitorDb`.`ShuntMeasurements`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`ShuntMeasurements` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Sites_Id` INT NOT NULL,
  `Shunts_Id` INT NOT NULL,
  `SolarRecords_Timestamp` DATETIME NOT NULL,
  `Current_A` FLOAT NOT NULL,
  `Voltage_V` FLOAT NOT NULL,
  `Interval_s` INT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_ShuntMeasurements_Shunts1_idx` (`Shunts_Id` ASC),
  INDEX `fk_ShuntMeasurements_SolarRecords1_idx` (`SolarRecords_Timestamp` ASC),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC),
  INDEX `fk_ShuntMeasurements_Sites1_idx` (`Sites_Id` ASC))
ENGINE = MyISAM;


-- -----------------------------------------------------
-- Table `SolarMonitorDb`.`ControllerMeasurements`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`ControllerMeasurements` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Sites_Id` INT NOT NULL,
  `SolarRecords_Timestamp` DATETIME NOT NULL,
  `SolarControllers_Id` INT NOT NULL,
  `ArrayCurrent_A` FLOAT NOT NULL,
  `ArrayVoltage_V` FLOAT NOT NULL,
  `BatteryCurrent_A` FLOAT NULL,
  `BatteryVoltage_V` FLOAT NULL,
  `BatterySOC` FLOAT NULL,
  `BatteryChargeStatus` VARCHAR(10) NULL,
  `BatteryTemperature_C` FLOAT NULL,
  `BatteryMaxVoltage_V` FLOAT NULL,
  `BatteryMinVoltage_V` FLOAT NULL,
  `ControllerTemperature_C` FLOAT NULL,
  `LoadCurrent_A` FLOAT NULL,
  `LoadVoltage_V` FLOAT NULL,
  `LoadOn` TINYINT(1) NULL,
  `EnergyConsumedDaily_kWh` FLOAT NULL,
  `EnergyConsumedMonthly_kWh` FLOAT NULL,
  `EnergyConsumedAnnual_kWh` FLOAT NULL,
  `EnergyConsumedTotal_kWh` FLOAT NULL,
  `EnergyGeneratedDaily_kWh` FLOAT NULL,
  `EnergyGeneratedMonthly_kWh` FLOAT NULL,
  `EnergyGeneratedAnnual_kWh` FLOAT NULL,
  `EnergyGeneratedTotal_kWh` FLOAT NULL,
  `Interval_s` INT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_ControllerMeasurements_SolarControllerInfo1_idx` (`SolarControllers_Id` ASC),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC),
  INDEX `fk_ControllerMeasurements_SolarRecords1_idx` (`SolarRecords_Timestamp` ASC),
  INDEX `fk_ControllerMeasurements_Sites1_idx` (`Sites_Id` ASC))
ENGINE = MyISAM;


-- -----------------------------------------------------
-- Table `SolarMonitorDb`.`Users`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`Users` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Username` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Username_UNIQUE` (`Username` ASC))
ENGINE = MyISAM;


-- -----------------------------------------------------
-- Table `SolarMonitorDb`.`TemperatureSensors`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`TemperatureSensors` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `SolarSystems_Id` INT NOT NULL,
  `Name` VARCHAR(45) NOT NULL,
  `Description` VARCHAR(255) NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_CurrentSensors_SolarSystems1_idx` (`SolarSystems_Id` ASC))
ENGINE = MyISAM;


-- -----------------------------------------------------
-- Table `SolarMonitorDb`.`WeatherRecords`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`WeatherRecords` (
  `Timestamp` DATETIME NOT NULL,
  `Sites_Id` INT NOT NULL,
  `Saved` TINYINT(1) NULL,
  PRIMARY KEY (`Timestamp`),
  INDEX `fk_SolarRecords_Sites1_idx` (`Sites_Id` ASC),
  UNIQUE INDEX `Timestamp_UNIQUE` (`Timestamp` ASC))
ENGINE = MyISAM;


-- -----------------------------------------------------
-- Table `SolarMonitorDb`.`TemperatureMeasurements`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`TemperatureMeasurements` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `TemperatureSensors_Id` INT NOT NULL,
  `WeatherRecords_Timestamp` DATETIME NOT NULL,
  `Temperature_C` FLOAT NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC),
  INDEX `fk_TemperatureMeasurements_TemperatureSensors1_idx` (`TemperatureSensors_Id` ASC),
  INDEX `fk_TemperatureMeasurements_WeatherRecords1_idx` (`WeatherRecords_Timestamp` ASC))
ENGINE = MyISAM;


-- -----------------------------------------------------
-- Table `SolarMonitorDb`.`HumiditySensors`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`HumiditySensors` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `SolarSystems_Id` INT NOT NULL,
  `Name` VARCHAR(45) NOT NULL,
  `Description` VARCHAR(255) NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_CurrentSensors_SolarSystems1_idx` (`SolarSystems_Id` ASC))
ENGINE = MyISAM;


-- -----------------------------------------------------
-- Table `SolarMonitorDb`.`HumidityMeasurements`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`HumidityMeasurements` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `WeatherRecords_Timestamp` DATETIME NOT NULL,
  `HumiditySensors_Id` INT NOT NULL,
  `RelativeHumidity_%` FLOAT NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC),
  INDEX `fk_TemperatureMeasurements_WeatherRecords1_idx` (`WeatherRecords_Timestamp` ASC),
  INDEX `fk_HumidityMeasurements_HumiditySensors1_idx` (`HumiditySensors_Id` ASC))
ENGINE = MyISAM;


-- -----------------------------------------------------
-- Table `SolarMonitorDb`.`BarometricPressureSensors`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`BarometricPressureSensors` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `SolarSystems_Id` INT NOT NULL,
  `Name` VARCHAR(45) NOT NULL,
  `Description` VARCHAR(255) NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_CurrentSensors_SolarSystems1_idx` (`SolarSystems_Id` ASC))
ENGINE = MyISAM;


-- -----------------------------------------------------
-- Table `SolarMonitorDb`.`BarometricPressureMeasurements`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`BarometricPressureMeasurements` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `WeatherRecords_Timestamp` DATETIME NOT NULL,
  `BarometricPressureSensors_Id` INT NOT NULL,
  `BarometricPressure_mbar` FLOAT NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC),
  INDEX `fk_TemperatureMeasurements_WeatherRecords1_idx` (`WeatherRecords_Timestamp` ASC),
  INDEX `fk_AtmPressureMeasurements_AtmPressureSensors1_idx` (`BarometricPressureSensors_Id` ASC))
ENGINE = MyISAM;


-- -----------------------------------------------------
-- Table `SolarMonitorDb`.`WindSensors`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`WindSensors` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `SolarSystems_Id` INT NOT NULL,
  `Name` VARCHAR(45) NOT NULL,
  `Description` VARCHAR(255) NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_CurrentSensors_SolarSystems1_idx` (`SolarSystems_Id` ASC))
ENGINE = MyISAM;


-- -----------------------------------------------------
-- Table `SolarMonitorDb`.`WindMeasurements`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`WindMeasurements` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `WeatherRecords_Timestamp` DATETIME NOT NULL,
  `WindSensors_Id` INT NOT NULL,
  `WindSpeed_m/s` FLOAT NOT NULL,
  `WindDirection_deg` FLOAT NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC),
  INDEX `fk_TemperatureMeasurements_WeatherRecords1_idx` (`WeatherRecords_Timestamp` ASC),
  INDEX `fk_AtmPressureMeasurements_copy1_WindSensors1_idx` (`WindSensors_Id` ASC))
ENGINE = MyISAM;

USE `SolarMonitorDb` ;

-- -----------------------------------------------------
-- Placeholder table for view `SolarMonitorDb`.`EnergyFromShuntsPerHour_View`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`EnergyFromShuntsPerHour_View` (`Site` INT, `Source` INT, `Date` INT, `Time` INT, `Energy_Wh` INT);

-- -----------------------------------------------------
-- Placeholder table for view `SolarMonitorDb`.`EnergyFromShuntsPerMonth_View`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`EnergyFromShuntsPerMonth_View` (`Site` INT, `Source` INT, `Year` INT, `Month` INT, `Energy_Wh` INT);

-- -----------------------------------------------------
-- Placeholder table for view `SolarMonitorDb`.`EnergyFromShuntsPerYear_View`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`EnergyFromShuntsPerYear_View` (`Site` INT, `Source` INT, `Year` INT, `Energy_Wh` INT);

-- -----------------------------------------------------
-- Placeholder table for view `SolarMonitorDb`.`EnergyFromShuntsPerDay_View`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`EnergyFromShuntsPerDay_View` (`Site` INT, `Source` INT, `Date` INT, `Energy_Wh` INT);

-- -----------------------------------------------------
-- Placeholder table for view `SolarMonitorDb`.`EnergyFromLoadsPerHour_View`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`EnergyFromLoadsPerHour_View` (`Site` INT, `Source` INT, `VoltageType` INT, `Voltage_V` INT, `Date` INT, `Time` INT, `Energy_Wh` INT);

-- -----------------------------------------------------
-- Placeholder table for view `SolarMonitorDb`.`EnergyFromLoadsPerYear_View`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`EnergyFromLoadsPerYear_View` (`Site` INT, `Source` INT, `VoltageType` INT, `Voltage_V` INT, `Year` INT, `Energy_Wh` INT);

-- -----------------------------------------------------
-- Placeholder table for view `SolarMonitorDb`.`EnergyFromLoadsPerMonth_View`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`EnergyFromLoadsPerMonth_View` (`Site` INT, `Source` INT, `VoltageType` INT, `Voltage_V` INT, `Year` INT, `Month` INT, `Energy_Wh` INT);

-- -----------------------------------------------------
-- Placeholder table for view `SolarMonitorDb`.`EnergyFromLoadsPerDay_View`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SolarMonitorDb`.`EnergyFromLoadsPerDay_View` (`Site` INT, `Source` INT, `Date` INT, `VoltageType` INT, `Voltage_V` INT, `Energy_Wh` INT);

-- -----------------------------------------------------
-- procedure spEnergyFromShunts
-- -----------------------------------------------------

DELIMITER $$
USE `SolarMonitorDb`$$
CREATE PROCEDURE spEnergyFromShunts(
  siteName VARCHAR(45),
  shuntName VARCHAR(45),
  startTime DATETIME, 
  endTime DATETIME,
  period ENUM('Hourly', 'Daily', 'Monthly', 'Yearly'))

BEGIN
	DROP TEMPORARY TABLE IF EXISTS EnergyFromShunts_Temp;

	IF (period = 'Hourly') THEN 
		CREATE TEMPORARY TABLE EnergyFromShunts_Temp  AS
			SELECT
				DATE(SolarRecords_Timestamp) AS Date,
				TIME_FORMAT(SolarRecords_Timestamp, '%H:00') AS Time,
				FORMAT(SUM(Voltage_V * Current_A * Interval_s / 3600), 2) AS Energy_Wh
			FROM
				ShuntMeasurements
			WHERE 
				Sites_Id IN (SELECT Id FROM Sites WHERE Name = siteName) AND
				Shunts_Id IN (SELECT Id FROM Shunts WHERE Name = shuntName) AND
				SolarRecords_Timestamp BETWEEN startTime AND endTime
			GROUP BY 
				Date, Time;

	ELSEIF period = 'Daily' THEN
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

	ELSEIF period = 'Monthly' THEN
		CREATE TEMPORARY TABLE EnergyFromShunts_Temp  AS
			SELECT
				YEAR(SolarRecords_Timestamp) AS Year,
				MONTH(SolarRecords_Timestamp) AS Month,
				FORMAT(SUM(Voltage_V * Current_A * Interval_s / 3600), 2) AS Energy_Wh
			FROM
				ShuntMeasurements
			WHERE 
				Sites_Id IN (SELECT Id FROM Sites WHERE Name = siteName) AND
				Shunts_Id IN (SELECT Id FROM Shunts WHERE Name = shuntName) AND
				SolarRecords_Timestamp BETWEEN startTime AND endTime
			GROUP BY Year, Month;
              
	ELSEIF period = 'Yearly' THEN
		CREATE TEMPORARY TABLE EnergyFromShunts_Temp  AS
			SELECT
				YEAR(SolarRecords_Timestamp) AS Year,
				FORMAT(SUM(Voltage_V * Current_A * Interval_s / 3600), 2) AS Energy_Wh
			FROM
				ShuntMeasurements
			WHERE 
				Sites_Id IN (SELECT Id FROM Sites WHERE Name = siteName) AND
				Shunts_Id IN (SELECT Id FROM Shunts WHERE Name = shuntName) AND
				SolarRecords_Timestamp BETWEEN startTime AND endTime
			GROUP BY Year;

	END IF;

	SELECT 
		*
	FROM
		EnergyFromShunts_Temp;

END;$$

DELIMITER ;

-- -----------------------------------------------------
-- View `SolarMonitorDb`.`EnergyFromShuntsPerHour_View`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `SolarMonitorDb`.`EnergyFromShuntsPerHour_View`;
USE `SolarMonitorDb`;
CREATE  OR REPLACE VIEW `EnergyFromShuntsPerHour_View` AS
    SELECT 
		Sites.Name AS Site,
        Shunts.Name AS Source,
        DATE(SolarRecords_Timestamp) AS Date,
        TIME_FORMAT(SolarRecords_Timestamp, '%H:00') AS Time,
        FORMAT(SUM(ShuntMeasurements.Voltage_V * ShuntMeasurements.Current_A * ShuntMeasurements.Interval_s / 3600), 2) AS Energy_Wh
    FROM
        ShuntMeasurements
            INNER JOIN
        Sites ON Sites.Id = ShuntMeasurements.Sites_Id
            INNER JOIN
        Shunts ON Shunts.Id = ShuntMeasurements.Shunts_Id
 
    GROUP BY Site , Source , Date, Time
    ORDER BY Site , Source ASC , Date , Time DESC;

-- -----------------------------------------------------
-- View `SolarMonitorDb`.`EnergyFromShuntsPerMonth_View`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `SolarMonitorDb`.`EnergyFromShuntsPerMonth_View`;
USE `SolarMonitorDb`;
CREATE  OR REPLACE VIEW `EnergyFromShuntsPerMonth_View` AS
    SELECT 
        Site,
        Source,
        YEAR(Date) AS Year,
        DATE_FORMAT(Date, '%M') AS Month,
        FORMAT(SUM(Energy_Wh), 2) AS Energy_Wh
    FROM
        EnergyFromShuntsPerHour_View
    GROUP BY Site , Source , Year, Month
    ORDER BY Site , Source ASC , Year, Month DESC;

-- -----------------------------------------------------
-- View `SolarMonitorDb`.`EnergyFromShuntsPerYear_View`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `SolarMonitorDb`.`EnergyFromShuntsPerYear_View`;
USE `SolarMonitorDb`;
CREATE  OR REPLACE VIEW `EnergyFromShuntsPerYear_View` AS
    SELECT 
        Site,
        Source,
        YEAR(Date) AS Year,
        FORMAT(SUM(Energy_Wh), 2) AS Energy_Wh
    FROM
        EnergyFromShuntsPerHour_View
    GROUP BY Site , Source , Year
    ORDER BY Site , Source ASC , Year DESC;

-- -----------------------------------------------------
-- View `SolarMonitorDb`.`EnergyFromShuntsPerDay_View`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `SolarMonitorDb`.`EnergyFromShuntsPerDay_View`;
USE `SolarMonitorDb`;
CREATE  OR REPLACE VIEW `EnergyFromShuntsPerDay_View` AS
    SELECT 
        Site, Source, Date,
        FORMAT(SUM(Energy_Wh), 2) AS Energy_Wh
    FROM
        EnergyFromShuntsPerHour_View
    GROUP BY Site , Source , Date
    ORDER BY Site , Source ASC , Date DESC;

-- -----------------------------------------------------
-- View `SolarMonitorDb`.`EnergyFromLoadsPerHour_View`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `SolarMonitorDb`.`EnergyFromLoadsPerHour_View`;
USE `SolarMonitorDb`;
CREATE  OR REPLACE VIEW `EnergyFromLoadsPerHour_View` AS
    SELECT 
		Sites.Name AS Site,
        CurrentSensors.Name AS Source,
        LoadTypes.VoltageType,
        LoadTypes.Voltage_V,
        DATE(SolarRecords_Timestamp) AS Date,
        TIME_FORMAT(SolarRecords_Timestamp, '%H:00') AS Time,
        FORMAT(SUM(LoadMeasurements.CurrentDraw_A * LoadTypes.Voltage_V * LoadMeasurements.Interval_s / 3600), 2) AS Energy_Wh
    FROM
        LoadMeasurements
            INNER JOIN
        Sites ON Sites.Id = LoadMeasurements.Sites_Id
            INNER JOIN
        CurrentSensors ON CurrentSensors.Id = LoadMeasurements.CurrentSensors_Id
            INNER JOIN
        LoadTypes ON LoadTypes.Id = CurrentSensors.LoadTypes_Id
 
    GROUP BY Site , Source , VoltageType , Voltage_V , Date, Time
    ORDER BY Site , Source, VoltageType , Voltage_V ASC , Date , Time DESC;

-- -----------------------------------------------------
-- View `SolarMonitorDb`.`EnergyFromLoadsPerYear_View`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `SolarMonitorDb`.`EnergyFromLoadsPerYear_View`;
USE `SolarMonitorDb`;
CREATE  OR REPLACE VIEW `EnergyFromLoadsPerYear_View` AS
    SELECT 
        Site,
        Source,
        VoltageType,
        Voltage_V,
        YEAR(Date) AS Year,
        FORMAT(SUM(Energy_Wh), 2) AS Energy_Wh
    FROM
        EnergyFromLoadsPerHour_View
    GROUP BY Site , Source , VoltageType , Voltage_V , Year
    ORDER BY Site , Source , VoltageType , Voltage_V ASC , Year DESC;

-- -----------------------------------------------------
-- View `SolarMonitorDb`.`EnergyFromLoadsPerMonth_View`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `SolarMonitorDb`.`EnergyFromLoadsPerMonth_View`;
USE `SolarMonitorDb`;
CREATE  OR REPLACE VIEW `EnergyFromLoadsPerMonth_View` AS
    SELECT 
        Site,
        Source,
		VoltageType, 
        Voltage_V,
        YEAR(Date) AS Year,
        DATE_FORMAT(Date, '%M') AS Month,
        FORMAT(SUM(Energy_Wh), 2) AS Energy_Wh
    FROM
        EnergyFromLoadsPerHour_View
    GROUP BY Site , Source , VoltageType , Voltage_V , Year, Month
    ORDER BY Site , Source , VoltageType , Voltage_V ASC , Year, Month DESC;

-- -----------------------------------------------------
-- View `SolarMonitorDb`.`EnergyFromLoadsPerDay_View`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `SolarMonitorDb`.`EnergyFromLoadsPerDay_View`;
USE `SolarMonitorDb`;
CREATE  OR REPLACE VIEW `EnergyFromLoadsPerDay_View` AS
    SELECT 
        Site, 
        Source, 
        Date, 
        VoltageType, 
        Voltage_V, 
        FORMAT(SUM(Energy_Wh), 2) AS Energy_Wh
    FROM
        EnergyFromLoadsPerHour_View
    GROUP BY Site , Source , VoltageType , Voltage_V , Date
    ORDER BY Site , Source , VoltageType , Voltage_V ASC , Date DESC;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;