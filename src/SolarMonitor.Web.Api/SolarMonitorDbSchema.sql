CREATE TABLE `__EFMigrationsHistory` (
    `MigrationId` varchar(95) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
);

CREATE TABLE `DeviceTypes` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(45),
    CONSTRAINT `PK_DeviceTypes` PRIMARY KEY (`Id`)
);

CREATE TABLE `LoadTypes` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Type` varchar(2),
    `Voltage_V` float NOT NULL,
    CONSTRAINT `PK_LoadTypes` PRIMARY KEY (`Id`)
);

CREATE TABLE `SensorTypes` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(45),
    CONSTRAINT `PK_SensorTypes` PRIMARY KEY (`Id`)
);

CREATE TABLE `Sites` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(45),
    `Timezone` varchar(10),
    CONSTRAINT `PK_Sites` PRIMARY KEY (`Id`)
);

CREATE TABLE `Users` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(45),
    `Username` varchar(45),
    CONSTRAINT `PK_Users` PRIMARY KEY (`Id`)
);

CREATE TABLE `SolarSystems` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Description` varchar(255),
    `Name` varchar(45),
    `SiteId` int NOT NULL,
    CONSTRAINT `PK_SolarSystems` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_SolarSystems_Sites_SiteId` FOREIGN KEY (`SiteId`) REFERENCES `Sites` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `WeatherBases` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Description` varchar(255),
    `Name` varchar(45),
    `SiteId` int NOT NULL,
    CONSTRAINT `PK_WeatherBases` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_WeatherBases_Sites_SiteId` FOREIGN KEY (`SiteId`) REFERENCES `Sites` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AuthTokens` (
    `Guid` char(36) NOT NULL,
    `Expiry` datetime(6) NOT NULL,
    `UserId` int NOT NULL,
    CONSTRAINT `PK_AuthTokens` PRIMARY KEY (`Guid`),
    CONSTRAINT `FK_AuthTokens_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `Devices` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Description` varchar(255),
    `DetailedSpecs` longtext,
    `Discriminator` longtext NOT NULL,
    `Guid` char(36) NOT NULL,
    `Manufacturer` varchar(45),
    `Model` varchar(45),
    `Name` varchar(45),
    `SiteId` int NOT NULL,
    `TypeId` int NOT NULL,
    `BankVoltage_V` float,
    `BatteryVoltage_V` float,
    `CapacityPerBattery_Ah` float,
    `Configuration` varchar(5),
    `NumBatteries` int,
    `SolarSystemId` int,
    `TotalCapacity_Ah` float,
    `CurrentRating_A` float,
    `InputVoltage_V` float,
    `MaxContinuousPower_W` float,
    `MaxSurgePower_W` float,
    `OutputVoltage_V` float,
    `NumPanels` int,
    `PanelMaxPower_W` int,
    `PanelOpenCircuitVoltage_V` float,
    `PanelShortCircuitCurrent_A` float,
    `WeatherBaseId` int,
    CONSTRAINT `PK_Devices` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Devices_Sites_SiteId` FOREIGN KEY (`SiteId`) REFERENCES `Sites` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Devices_DeviceTypes_TypeId` FOREIGN KEY (`TypeId`) REFERENCES `DeviceTypes` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Devices_SolarSystems_SolarSystemId` FOREIGN KEY (`SolarSystemId`) REFERENCES `SolarSystems` (`Id`) ON DELETE NO ACTION,
    CONSTRAINT `FK_Devices_WeatherBases_WeatherBaseId` FOREIGN KEY (`WeatherBaseId`) REFERENCES `WeatherBases` (`Id`) ON DELETE NO ACTION
);

CREATE TABLE `Sensors` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Description` varchar(255),
    `DetailedSpecs` longtext,
    `Device` char(36) NOT NULL,
    `Discriminator` longtext NOT NULL,
    `Guid` char(36) NOT NULL,
    `Manufacturer` varchar(45),
    `Model` varchar(45),
    `Name` varchar(45),
    `SiteId` int NOT NULL,
    `TypeId` int NOT NULL,
    `WeatherBaseId` int,
    `LoadTypeId` int,
    `SolarSystemId` int,
    `InternalResistor_mOhm` float,
    `InternalVoltage_mV` float,
    `MaxCurrent_A` float,
    CONSTRAINT `PK_Sensors` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Sensors_Sites_SiteId` FOREIGN KEY (`SiteId`) REFERENCES `Sites` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Sensors_SensorTypes_TypeId` FOREIGN KEY (`TypeId`) REFERENCES `SensorTypes` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Sensors_WeatherBases_WeatherBaseId` FOREIGN KEY (`WeatherBaseId`) REFERENCES `WeatherBases` (`Id`) ON DELETE NO ACTION,
    CONSTRAINT `FK_Sensors_LoadTypes_LoadTypeId` FOREIGN KEY (`LoadTypeId`) REFERENCES `LoadTypes` (`Id`) ON DELETE NO ACTION,
    CONSTRAINT `FK_Sensors_SolarSystems_SolarSystemId` FOREIGN KEY (`SolarSystemId`) REFERENCES `SolarSystems` (`Id`) ON DELETE NO ACTION
);

CREATE TABLE `Measurements` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Discriminator` longtext NOT NULL,
    `SensorGuid` char(36) NOT NULL,
    `SensorId` int,
    `Timestamp` DATETIME NOT NULL,
    `BarometricPressure_mBar` float,
    `ChargeStage` longtext,
    `SOC` int,
    `MaxVoltage` float,
    `MinVoltage` float,
    `On` bit,
    `Current_A` float,
    `Interval_s` int,
    `AnnualEnergy_kWh` float,
    `DailyEnergy_kWh` float,
    `MonthlyEnergy_kWh` float,
    `TotalEnergy_kWh` float,
    `RelativeHumidity` float,
    `Voltage_v` float,
    `Temperature_C` float,
    `WindDirection_degFromN` float,
    `WindSpeed_mps` float,
    CONSTRAINT `PK_Measurements` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Measurements_Sensors_SensorId` FOREIGN KEY (`SensorId`) REFERENCES `Sensors` (`Id`) ON DELETE NO ACTION
);

CREATE INDEX `IX_AuthTokens_UserId` ON `AuthTokens` (`UserId`);

CREATE INDEX `IX_Devices_SiteId` ON `Devices` (`SiteId`);

CREATE INDEX `IX_Devices_TypeId` ON `Devices` (`TypeId`);

CREATE UNIQUE INDEX `IX_Devices_Guid` ON `Devices` (`Guid`);

CREATE INDEX `IX_Devices_SolarSystemId` ON `Devices` (`SolarSystemId`);

CREATE INDEX `IX_Devices_WeatherBaseId` ON `Devices` (`WeatherBaseId`);

CREATE INDEX `IX_Measurements_SensorId` ON `Measurements` (`SensorId`);

CREATE INDEX `IX_Measurements_Timestamp` ON `Measurements` (`Timestamp`);

CREATE INDEX `IX_Sensors_SiteId` ON `Sensors` (`SiteId`);

CREATE INDEX `IX_Sensors_TypeId` ON `Sensors` (`TypeId`);

CREATE UNIQUE INDEX `IX_Sensors_Guid` ON `Sensors` (`Guid`);

CREATE INDEX `IX_Sensors_WeatherBaseId` ON `Sensors` (`WeatherBaseId`);

CREATE INDEX `IX_Sensors_LoadTypeId` ON `Sensors` (`LoadTypeId`);

CREATE INDEX `IX_Sensors_SolarSystemId` ON `Sensors` (`SolarSystemId`);

CREATE INDEX `IX_SolarSystems_SiteId` ON `SolarSystems` (`SiteId`);

CREATE INDEX `IX_WeatherBases_SiteId` ON `WeatherBases` (`SiteId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20170814041159_migration1', '1.1.2');

