USE SolarMonitorDb;

##########################################################################################################################
# Triggers
##########################################################################################################################

CREATE TRIGGER ShuntsGuidTrigger                     BEFORE INSERT ON Shunts                        FOR EACH ROW SET new.Guid = uuid();
CREATE TRIGGER CurrentSensorsGuidTrigger             BEFORE INSERT ON CurrentSensors                FOR EACH ROW SET new.Guid = uuid();
CREATE TRIGGER BatterySocSensorsTrigger              BEFORE INSERT ON BatterySocSensors             FOR EACH ROW SET new.Guid = uuid();
CREATE TRIGGER BatteryChargeStageSensorsTrigger      BEFORE INSERT ON BatteryChargeStageSensors     FOR EACH ROW SET new.Guid = uuid();
CREATE TRIGGER ChargeControllerLoadOutputsTrigger    BEFORE INSERT ON ChargeControllerLoadOutputs   FOR EACH ROW SET new.Guid = uuid();
CREATE TRIGGER BatteryStatsSensorsTrigger            BEFORE INSERT ON BatteryStatsSensors           FOR EACH ROW SET new.Guid = uuid();
CREATE TRIGGER EnergyStatsSensorsTrigger             BEFORE INSERT ON EnergyStatsSensors            FOR EACH ROW SET new.Guid = uuid();
CREATE TRIGGER TemperatureSensorsGuidTrigger         BEFORE INSERT ON TemperatureSensors            FOR EACH ROW SET new.Guid = uuid();
CREATE TRIGGER HumiditySensorsGuidTrigger            BEFORE INSERT ON HumiditySensors               FOR EACH ROW SET new.Guid = uuid();
CREATE TRIGGER BarometricPressureSensorsGuidTrigger  BEFORE INSERT ON BarometricPressureSensors     FOR EACH ROW SET new.Guid = uuid();
CREATE TRIGGER WindSensorsGuidTrigger                BEFORE INSERT ON WindSensors                   FOR EACH ROW SET new.Guid = uuid();

#CREATE TRIGGER BatteryBanksTrigger                   BEFORE INSERT ON BatteryBanks                  FOR EACH ROW SET new.Guid = uuid();
#CREATE TRIGGER ChargeControllersTrigger              BEFORE INSERT ON ChargeControllers             FOR EACH ROW SET new.Guid = uuid();
#CREATE TRIGGER SolarArraysTrigger                    BEFORE INSERT ON SolarArrays                   FOR EACH ROW SET new.Guid = uuid();
#CREATE TRIGGER InvertersTrigger                      BEFORE INSERT ON Inverters                     FOR EACH ROW SET new.Guid = uuid();
#CREATE TRIGGER WeatherStationsTrigger                BEFORE INSERT ON WeatherStations               FOR EACH ROW SET new.Guid = uuid();

##########################################################################################################################
# Authentication
##########################################################################################################################

INSERT INTO Users (Id, Username)
  VALUES (1, "user1"),
         (2, "user2"),
         (3, "demo");


##########################################################################################################################
# Main types
##########################################################################################################################

INSERT INTO LoadTypes (Id, Type, Voltage_V)
  VALUES (1, "DC", 12),
         (2, "AC", 230);
           
INSERT INTO SensorTypes (Id, Name)
  VALUES (1, "Shunt"),
         (2, "CurrentSensor"),
         (3, "BatterySocSensor"),
         (4, "BatteryChargeStageSensor"),
         (5, "ChargeControllerLoadOutput"),
         (6, "BatteryStatsSensor"),
         (7, "EnergyStatsSensor"),
         (8, "TemperatureSensor"),
         (9, "HumiditySensor"),
         (10, "BarometricPressureSensor"),
         (11, "WindSensor");
           
INSERT INTO DeviceTypes (Id, Name)
  VALUES (1, "ChargeController"),
         (2, "BatteryBank"),
         (3, "SolarArray"),
         (4, "Inverter"),
         (5, "WeatherStation");
           
##########################################################################################################################
# Solar system
##########################################################################################################################

INSERT INTO Sites (Id, Name, Timezone)
  VALUES (1, "Lake Ohau", "NZST"),
         (21, "Test system", "NZST"),
         (31, "Simulation 1", "NZST");

-- INSERT INTO SolarSystems (Id, SiteId, Name, Description)
--   VALUES (1, 1, "Main system", ""),
--          (21, 21, "Test system", ""),
--          (31, 31, "Simulated system 1", "");

INSERT INTO BatteryBanks (Id, SiteId, TypeId, Name, Model, Manufacturer, Description, DetailedSpecs, CapacityPerBattery_Ah, TotalCapacity_Ah, BankVoltage_V, BatteryVoltage_V, Configuration, NumBatteries, Guid)
  VALUES (1, 1, 2, "Main Battery", "AGM v1", "AA Solar", "", "", 100, 400, 24, 12, "2x2", 4, "1e32189a-c40f-11e6-a9e0-002522ab4073" ),  
         (21, 21, 2, "Main", "Powerwall v.2", "Tesla", "", "", 15000, 15000, 600, 600, "1x1", 1, "22ef19a0-c40f-11e6-97ca-002522ab4073" );

INSERT INTO ChargeControllers (Id, SiteId, TypeId, Name, Model, Manufacturer, Description, DetailedSpecs, CurrentRating_A, Guid)
  VALUES (1, 1, 1, "Charge Controller", "Tracer 4215BN", "EPsolar", "12/24V, 40A MPPT solar charge controller", "Nominal system voltage: 12/24VDC. Rated battery current: 40A, MPP Voltage range: +2V~108V, Max. PV input power: 520W@12V/1040W@24V", 40, "2e006e20-c40f-11e6-bbbc-002522ab4073"),
         (21, 21, 1, "Charge Controller", "Tracer 4215BN", "EPsolar", "12/24V, 40A MPPT solar charge controller", "Nominal system voltage: 12/24VDC. Rated battery current: 40A, MPP Voltage range: +2V~18V, Max. PV input power: 520W@12V/1040W@24V", 40, "32e4e466-c40f-11e6-bf27-002522ab4073"),  
         (31, 31, 1, "Charge Controller", "Classic 150 MPPT", "MidNite Solar", "", "Battery bank voltage support: 12/24/48VDC. Max current output: 96A@12V/94A@24V/86A@48V", 96, "368aa2ea-c40f-11e6-972a-002522ab4073");  

INSERT INTO SolarArrays (Id, SiteId, TypeId, Name, Model, Manufacturer, Description, DetailedSpecs, PanelMaxPower_W, PanelOpenCircuitVoltage_V, PanelShortCircuitCurrent_A, Configuration, NumPanels, Guid)
  VALUES (1, 1, 3, "Solar Array", "AA4", "AA Solar", "", "", 250, 30.0, 10.5, "2x2", 4, "3caf024c-c40f-11e6-b8c1-002522ab4073"),  
         (21, 21, 3, "Solar Array", "AA4", "AA Solar", "", "", 250, 30.0, 10.5, "2x2", 4, "40b19058-c40f-11e6-a0d2-002522ab4073"), 
         (31, 31, 3, "Solar Array", "REC 300", "REC", "", "", 300, 30.0, 10.5, "4x2", 8, "445ec9e6-c40f-11e6-8d82-002522ab4073");  

INSERT INTO Inverters (Id, SiteId, TypeId, Name, Model, Manufacturer, Description, DetailedSpecs, MaxContinuousPower_W, MaxSurgePower_W, InputVoltage_V, OutputVoltage_V, Guid)
  VALUES (21, 21, 4, "Inverter", "MS2024", "Magnum Energy", "", "", 2000, 3000, 24, 230, "47c3e6d4-c40f-11e6-a44d-002522ab4073"),  
         (31, 31, 4, "Inverter", "Power VFXR3048E", "Outback", "", "", 3000, 5000, 48, 230, "4b1f5318-c40f-11e6-b0fd-002522ab4073");


##########################################################################################################################
           
INSERT INTO Shunts (Id, SiteId, TypeId, Name, Model, Manufacturer, Description, DetailedSpecs, InternalResistor_mOhm, MaxCurrent_A, InternalVoltage_mV, Device)
  VALUES (1, 1, 1, "Array Shunt", "Tracer 4215BN", "EPsolar", "Solar Array Output", "", 0.1, 200, 50, "2e006e20-c40f-11e6-bbbc-002522ab4073"),
         (2, 1, 1, "Battery Shunt", "INA219", "Adafruit", "Controller to battery", "", 0.1, 200, 50, NULL),
         (3, 1, 1, "Loads Shunt", "INA219", "Adafruit", "Battery to DC Loads", "", 0.1, 200, 50, NULL),
         (4, 1, 1, "Inverter Shunt", "INA219", "Adafruit", "Battery to inverter", "", 0.1, 200, 50, NULL),

         (21, 21, 1, "Array Shunt", "Tracer 4215BN", "EPsolar", "Solar Array Output", "", 0.1, 200, 50, "32e4e466-c40f-11e6-bf27-002522ab4073"),
         (22, 21, 1, "Battery Shunt", "INA219", "Adafruit", "Controller to battery", "", 0.1, 200, 50, NULL),
         (23, 21, 1, "Loads Shunt", "INA219", "Adafruit", "Battery to DC Loads", "", 0.1, 200, 50, NULL),
         (24, 21, 1, "Inverter Shunt", "INA219", "Adafruit", "Battery to inverter", "", 0.1, 200, 50, NULL),

         (31, 31, 1, "Array Shunt", "Tracer 4215BN", "EPsolar", "Solar Array Output", "", 0.1, 200, 50, "368aa2ea-c40f-11e6-972a-002522ab4073"),
         (32, 31, 1, "Battery Shunt", "INA219", "Adafruit", "Controller to battery", "", 0.1, 200, 50, NULL),
         (33, 31, 1, "Loads Shunt", "INA219", "Adafruit", "Battery to DC Loads", "", 0.1, 200, 50, NULL),
         (34, 31, 1, "Inverter Shunt", "INA219", "Adafruit", "Battery to inverter", "", 0.1, 200, 50, NULL);

##########################################################################################################################

INSERT INTO CurrentSensors (Id, SiteId, TypeId, LoadTypeId, Name, Model, Manufacturer, Description, DetailedSpecs)
  VALUES (1, 1, 2, 1, "Lounge", "CSLA2CD", "Honeywell", "", ""),
         (2, 1, 2, 1, "Office", "CSLA2CD", "Honeywell", "", ""),
         (3, 1, 2, 1, "Main Bedroom", "CSLA2CD", "Honeywell", "", ""),
         (4, 1, 2, 1, "Kitchen", "CSLA2CD", "Honeywell", "", ""),
         (5, 1, 2, 1, "Garage", "CSLA2CD", "Honeywell", "", ""),
         (6, 1, 2, 1, "Laundry", "CSLA2CD", "Honeywell", "", ""),

         # for solar system #2 
         (21, 21, 2, 1, "Lounge", "CSLA2CD", "Honeywell", "", ""),
         (22, 21, 2, 1, "Office", "CSLA2CD", "Honeywell", "", ""),
         (23, 21, 2, 1, "Main Bedroom", "CSLA2CD", "Honeywell", "", ""),
         (24, 21, 2, 1, "Kitchen", "CSLA2CD", "Honeywell", "", ""),
         (25, 21, 2, 1, "Garage", "CSLA2CD", "Honeywell", "", ""),

         # for solar system #3 
         (31, 31, 2, 1, "Lounge", "CSLA2CD", "Honeywell", "", ""),
         (32, 31, 2, 1, "Office", "CSLA2CD", "Honeywell", "", ""),
         (33, 31, 2, 1, "Main Bedroom", "CSLA2CD", "Honeywell", "", ""),
         (34, 31, 2, 1, "Kitchen", "CSLA2CD", "Honeywell", "", ""),
         (35, 31, 2, 1, "Garage", "CSLA2CD", "Honeywell", "", "");         

##########################################################################################################################

INSERT INTO BatterySocSensors (Id, SiteId, TypeId, Name, Model, Manufacturer, Description, DetailedSpecs, Device)
  VALUES (1, 1, 3, "Battery SOC", "Tracer 4215BN", "EPsolar", "", "", "2e006e20-c40f-11e6-bbbc-002522ab4073"),
         (21, 21, 3, "Battery SOC", "Tracer 4215BN", "EPsolar", "", "", "32e4e466-c40f-11e6-bf27-002522ab4073"),
         (31, 31, 3, "Battery SOC", "Tracer 4215BN", "EPsolar", "", "", "368aa2ea-c40f-11e6-972a-002522ab4073");

##########################################################################################################################

INSERT INTO BatteryChargeStageSensors (Id, SiteId, TypeId, Name, Model, Manufacturer, Description, DetailedSpecs, Device)
  VALUES (1, 1, 4, "Battery Charge Stage", "Tracer 4215BN", "EPsolar", "", "", "2e006e20-c40f-11e6-bbbc-002522ab4073"),
         (21, 21, 4, "Battery Charge Stage", "Tracer 4215BN", "EPsolar", "", "", "32e4e466-c40f-11e6-bf27-002522ab4073"),
         (31, 31, 4, "Battery Charge Stage", "Tracer 4215BN", "EPsolar", "", "", "368aa2ea-c40f-11e6-972a-002522ab4073");

##########################################################################################################################

INSERT INTO ChargeControllerLoadOutputs (Id, SiteId, TypeId, Name, Model, Manufacturer, Description, DetailedSpecs, Device)
  VALUES (1, 1, 5, "Charge Controller Load ON", "Tracer 4215BN", "EPsolar", "", "", "2e006e20-c40f-11e6-bbbc-002522ab4073"),
         (21, 21, 5, "Charge Controller Load ON", "Tracer 4215BN", "EPsolar", "", "", "32e4e466-c40f-11e6-bf27-002522ab4073"),
         (31, 31, 5, "Charge Controller Load ON", "Tracer 4215BN", "EPsolar", "", "", "368aa2ea-c40f-11e6-972a-002522ab4073");

##########################################################################################################################

INSERT INTO BatteryStatsSensors (Id, SiteId, TypeId, Name, Model, Manufacturer, Description, DetailedSpecs, Device)
  VALUES (1, 1, 6, "Battery Stats", "Tracer 4215BN", "EPsolar", "", "", "2e006e20-c40f-11e6-bbbc-002522ab4073"),
         (21, 21, 6, "Battery Stats", "Tracer 4215BN", "EPsolar", "", "", "32e4e466-c40f-11e6-bf27-002522ab4073"),
         (31, 31, 6, "Battery Stats", "Tracer 4215BN", "EPsolar", "", "", "368aa2ea-c40f-11e6-972a-002522ab4073");

##########################################################################################################################

INSERT INTO EnergyStatsSensors (Id, SiteId, TypeId, Name, Model, Manufacturer, Description, DetailedSpecs, Device)
  VALUES (1, 1, 7, "Energy Generated Stats", "Tracer 4215BN", "EPsolar", "", "", "2e006e20-c40f-11e6-bbbc-002522ab4073"),
         (2, 1, 7, "Energy Consumed Stats", "Tracer 4215BN", "EPsolar", "", "", "2e006e20-c40f-11e6-bbbc-002522ab4073"),

         (21, 21, 7, "Energy Generated Stats", "Tracer 4215BN", "EPsolar", "", "", "32e4e466-c40f-11e6-bf27-002522ab4073"),
         (22, 21, 7, "Energy Consumed Stats", "Tracer 4215BN", "EPsolar", "", "", "32e4e466-c40f-11e6-bf27-002522ab4073"),

         (31, 31, 7, "Energy Generated Stats", "Tracer 4215BN", "EPsolar", "", "", "368aa2ea-c40f-11e6-972a-002522ab4073"),
         (32, 31, 7, "Energy Consumed Stats", "Tracer 4215BN", "EPsolar", "", "", "368aa2ea-c40f-11e6-972a-002522ab4073");

##########################################################################################################################
# Weather system
##########################################################################################################################

-- INSERT INTO WeatherBases (Id, SiteId, Name, Description)
--   VALUES (1, 1, "Main system", ""),
--          (21, 21, "Main system", ""),
--          (31, 31, "Main system", "");

INSERT INTO TemperatureSensors (Id, SiteId, TypeId, Name, Model, Manufacturer, Description, DetailedSpecs, Device)
  VALUES (1, 1, 8, "Outside", "HTU21", "Adafruit", "", "", NULL),
         (2, 1, 8, "Office", "SHT31", "Adafruit", "", "", NULL),
         (3, 1, 8, "Piano Room", "HTU21", "Adafruit", "", "", NULL),
         (4, 1, 8, "Shipping Container", "HTU21", "Adafruit", "", "", NULL),
         (5, 1, 8, "Lounge", "HTU21", "Adafruit", "", "", NULL),
         (6, 1, 8, "Battery", "Tracer 4215BN", "EPsolar", "", "", "2e006e20-c40f-11e6-bbbc-002522ab4073"),
         (7, 1, 8, "Charge Controller", "Tracer 4215BN", "EPsolar", "", "", "2e006e20-c40f-11e6-bbbc-002522ab4073"),

         (21, 21, 8, "Outside", "HTU21", "Adafruit", "", "", NULL),
         (22, 21, 8, "Office", "SHT31", "Adafruit", "", "", NULL),
         (23, 21, 8, "Battery", "Tracer 4215BN", "EPsolar", "", "", "32e4e466-c40f-11e6-bf27-002522ab4073"),
         (24, 21, 8, "Charge Controller", "Tracer 4215BN", "EPsolar", "", "", "32e4e466-c40f-11e6-bf27-002522ab4073"),

         (31, 31, 8, "Outside", "HTU21", "Adafruit", "", "", NULL),
         (32, 31, 8, "Office", "SHT31", "Adafruit", "", "", NULL),
         (33, 31, 8, "Piano Room", "HTU21", "Adafruit", "", "", NULL),
         (34, 31, 8, "Shipping Container", "HTU21", "Adafruit", "", "", NULL),
         (35, 31, 8, "Lounge", "HTU21", "Adafruit", "", "", NULL),
         (36, 31, 8, "Battery", "Tracer 4215BN", "EPsolar", "", "", "368aa2ea-c40f-11e6-972a-002522ab4073"),
         (37, 31, 8, "Charge Controller", "Tracer 4215BN", "EPsolar", "", "", "368aa2ea-c40f-11e6-972a-002522ab4073");

INSERT INTO HumiditySensors (Id, SiteId, TypeId, Name, Model, Manufacturer, Description, DetailedSpecs)
  VALUES (1, 1, 9, "Outside Humidity", "HTU21", "Adafruit", "", ""),
         (2, 1, 9, "Office Humidity", "SHT31", "Adafruit", "", ""),
         (3, 1, 9, "Piano Room Humidity", "HTU21", "Adafruit", "", ""),
         (4, 1, 9, "Shipping Container Humidity", "HTU21", "Adafruit", "", ""),
         (5, 1, 9, "Lounge Humidity", "HTU21", "Adafruit", "", ""),

         (21, 21, 9, "Outside Humidity", "HTU21", "Adafruit", "", ""),
         (22, 21, 9, "Office Humidity", "SHT31", "Adafruit", "", ""),

         (31, 31, 9, "Outside Humidity", "HTU21", "Adafruit", "", ""),
         (32, 31, 9, "Office Humidity", "SHT31", "Adafruit", "", ""),
         (33, 31, 9, "Piano Room Humidity", "HTU21", "Adafruit", "", ""),
         (34, 31, 9, "Shipping Container Humidity", "HTU21", "Adafruit", "", ""),
         (35, 31, 9, "Lounge Humidity", "HTU21", "Adafruit", "", "");    

INSERT INTO BarometricPressureSensors (Id, SiteId, TypeId, Name, Model, Manufacturer, Description, DetailedSpecs)
  VALUES (1, 1, 10, "Outside Pressure", "HTU21", "Adafruit", "", ""),
         (2, 1, 10, "Office Pressure", "SHT31", "Adafruit", "", ""),

         (21, 21, 10, "Outside Pressure", "HTU21", "Adafruit", "", ""),
         (22, 21, 10, "Office Pressure", "SHT31", "Adafruit", "", ""),

         (31, 31, 10, "Outside Pressure", "HTU21", "Adafruit", "", ""),
         (32, 31, 10, "Office Pressure", "SHT31", "Adafruit", "", "");

INSERT INTO WindSensors (Id, SiteId, TypeId, Name, Model, Manufacturer, Description, DetailedSpecs)
  VALUES (1, 1, 11, "Sensor1", "HTU21", "Adafruit", "", ""),

         (21, 21, 11, "Sensor1", "HTU21", "Adafruit", "", ""),

         (31, 31, 11, "Sensor1", "HTU21", "Adafruit", "", "");

INSERT INTO WeatherStations (Id, SiteId, TypeId, Name, Model, Manufacturer, Description, DetailedSpecs, Guid)
  VALUES (1, 1, 5, "Main station", "HTU21", "Adafruit", "", "", "57ff2bee-c40f-11e6-8d45-002522ab4073"),
         (21, 21, 5, "Main station", "HTU21", "Adafruit", "", "", "5af19eea-c40f-11e6-ba09-002522ab4073"),
         (31, 31, 5, "Main station", "HTU21", "Adafruit", "", "", "5e7cb856-c40f-11e6-9019-002522ab4073");

