CREATE DATABASE SolarMonitorDb;

CREATE TABLE SolarMonitorDb.Sites (
  Id                    INT PRIMARY KEY AUTO_INCREMENT NOT NULL,
  Name                  NVARCHAR(255),
  Timezone              VARCHAR(2)
);

CREATE TABLE SolarMonitorDb.SolarRecords ( 
  Timestamp_id          DATETIME PRIMARY KEY NOT NULL,

  SiteId                REFERENCES SolarMonitorDb.Site.Id,

  ArrayVoltage          FLOAT,
  ArrayCurrent          FLOAT,

  BatteryVoltage        FLOAT,
  BatteryCurrent        FLOAT,
  BatterySOC            FLOAT,
  BatteryChargeStatus   VARCHAR(5)
  BatteryTemperature    FLOAT,
  ControllerTemperature FLOAT,
  LoadVoltage           FLOAT,
  LoadCurrent           FLOAT,

  # number of seconds from previous record (used to calculate energy from voltage and current)
  Interval              INT     

);

CREATE TABLE VoltageType (
  Id                            INT PRIMARY KEY AUTO_INCREMENT NOT NULL,
  # "AC" or "DC"
  Type                          NVARCHAR(2)  
);

CREATE TABLE LoadDescriptors (
  Id                            INT PRIMARY KEY AUTO_INCREMENT NOT NULL,
  SiteId                        REFERENCES SolarMonitorDb.Site.Id,  
  Name                          NVARCHAR(32),
  TypeId                        REFERENCES SolarMonitorDb.VoltageType.Id,  
);

CREATE TABLE ArrayInfo (
  Id                            INT PRIMARY KEY AUTO_INCREMENT NOT NULL,

  NumPanels                     INT,
  # Series x Parallel configuration (e.g. "3x2" stands for "2 parallel strings of 3 panels in series")
  Configuration                 NVARCHAR(16),  
  MaxPower_W                    INT,
  OpenCircuitVoltage_V          FLOAT,
  ShortCircuitCurrent_A         FLOAT,
);

CREATE TABLE LoadInfo (
  Id                            INT PRIMARY KEY AUTO_INCREMENT NOT NULL,

  DcLoadVoltage_V               INT,
  AcLoadVoltage_V               INT,
  DcLoads                       BOOL,
  AcLoads                       BOOL
);

CREATE TABLE BatteryInfo (
  Id                            INT PRIMARY KEY AUTO_INCREMENT NOT NULL,

  BatteryCapacity_Ah            INT,
  # this is the voltage of the battery bank
  SystemVoltage_V               INT,
  # Series x Parallel configuration of the battery bank (e.g. "3x2" stands for "2 parallel strings of 3 batteries in series")
  Configuration                 NVARCHAR(16),  
);

CREATE TABLE SystemInfo (
  Id                            INT PRIMARY KEY AUTO_INCREMENT NOT NULL,
  SiteId                        REFERENCES SolarMonitorDb.Site.Id,

  ArrayInfoId                   REFERENCES SolarMonitorDb.ArrayInfo.Id,
  LoadInfoId                    REFERENCES SolarMonitorDb.LoadInfo.Id,
  BatteryInfoId                 REFERENCES SolarMonitorDb.BatteryInfo.Id,
  ControllerInfoId              REFERENCES SolarMonitorDb.ControllerInfo.Id,
  InverterInfoId                REFERENCES SolarMonitorDb.InverterInfo.Id,

)