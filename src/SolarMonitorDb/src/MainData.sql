USE SolarMonitorDb;

##########################################################################################################################
# Triggers
##########################################################################################################################

CREATE TRIGGER SensorsGuidTrigger   BEFORE INSERT ON Sensors  FOR EACH ROW SET new.Guid = uuid();
CREATE TRIGGER SitesGuidTrigger     BEFORE INSERT ON Sites    FOR EACH ROW SET new.Guid = uuid();

##########################################################################################################################
# Authentication
##########################################################################################################################

INSERT INTO Roles (Id, Name)
  VALUES (1, "user"),
         (2, "admin");

INSERT INTO Users (Id, Username, Password)
  VALUES (1, "user1", "user1"),
         (2, "user2", "user2"),
         (3, "demo", "demo"),
         (4, "admin", "admin");

INSERT INTO RoleAssignments (UserId, RoleId)
  VALUES (1, 1),
         (2, 1),
         (3, 1),
         (4, 1),
         (4, 2);


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
         (2, "Test system", "NZST"),
         (3, "Simulation 1", "NZST");

-- INSERT INTO SolarSystems (Id, SiteId, Name, Description)
--   VALUES (1, 1, "Main system", ""),
--          (21, 21, "Test system", ""),
--          (31, 31, "Simulated system 1", "");

-- INSERT INTO WeatherBases (Id, SiteId, Name, Description)
--   VALUES (1, 1, "Main system", ""),
--          (21, 21, "Main system", ""),
--          (31, 31, "Main system", "");


INSERT INTO Devices (Id, SiteId, TypeId, Name, Model, Guid)
  VALUES
    # ChargeControllers 
    (1011, 1, 1, "Charge Controller", "Tracer 4215BN",    "2e006e20-c40f-11e6-bbbc-002522ab4073"),
    (2011, 2, 1, "Charge Controller", "Tracer 4215BN",    "32e4e466-c40f-11e6-bf27-002522ab4073"),  
    (3011, 3, 1, "Charge Controller", "Classic 150 MPPT", "368aa2ea-c40f-11e6-972a-002522ab4073"),

    # BatteryBanks
    (1021, 1, 2, "Main Battery",      "AGM v1",            "1e32189a-c40f-11e6-a9e0-002522ab4073"),  
    (2021, 2, 2, "Main",              "Powerwall v.2",     "22ef19a0-c40f-11e6-97ca-002522ab4073"),

    # SolarArrays
    (1031,  1,  3, "Solar Array",     "AA4",               "3caf024c-c40f-11e6-b8c1-002522ab4073"),  
    (2031, 2, 3, "Solar Array",       "AA4",               "40b19058-c40f-11e6-a0d2-002522ab4073"), 
    (3031, 3, 3, "Solar Array",       "REC 300",           "445ec9e6-c40f-11e6-8d82-002522ab4073"),

    # Inverters
    (2041, 2, 4, "Inverter",          "MS2024",            "47c3e6d4-c40f-11e6-a44d-002522ab4073"),  
    (3041, 3, 4, "Inverter",          "Power VFXR3048E",   "4b1f5318-c40f-11e6-b0fd-002522ab4073"),

    # WeatherStations
    (1051, 1, 5, "Main station",       "HTU21",            "57ff2bee-c40f-11e6-8d45-002522ab4073"),
    (2051, 2, 5, "Main station",       "HTU21",            "5af19eea-c40f-11e6-ba09-002522ab4073"),
    (3051, 3, 5, "Main station",       "HTU21",            "5e7cb856-c40f-11e6-9019-002522ab4073");


##########################################################################################################################

INSERT INTO Sensors (Id, SiteId, TypeId, LoadTypeId, Name, Model, Device)
  VALUES 
    # Shunts
    (1011, 1, 1, NULL, "Array Shunt",      "Tracer 4215BN",        "2e006e20-c40f-11e6-bbbc-002522ab4073"),
    (1012, 1, 1, NULL, "Battery Shunt",    "INA219",               NULL),
    (1013, 1, 1, NULL, "Loads Shunt",      "INA219",               NULL),
    (1014, 1, 1, NULL, "Inverter Shunt",   "INA219",               NULL),

    (2011, 2, 1, NULL, "Array Shunt",    "Tracer 4215BN",        "32e4e466-c40f-11e6-bf27-002522ab4073"),
    (2012, 2, 1, NULL, "Battery Shunt",  "INA219",               NULL),
    (2013, 2, 1, NULL, "Loads Shunt",    "INA219",               NULL),
    (2014, 2, 1, NULL, "Inverter Shunt", "INA219",               NULL),

    (3011, 3, 1, NULL, "Array Shunt",    "Tracer 4215BN",        "368aa2ea-c40f-11e6-972a-002522ab4073"),
    (3012, 3, 1, NULL, "Battery Shunt",  "INA219",               NULL),
    (3013, 3, 1, NULL, "Loads Shunt",    "INA219",               NULL),
    (3014, 3, 1, NULL, "Inverter Shunt", "INA219",               NULL),

    # CurrentSensors
    (1021, 1, 2,   1,    "Lounge",         "CSLA2CD",              NULL),
    (1022, 1, 2,   1,    "Office",         "CSLA2CD",              NULL),
    (1023, 1, 2,   1,    "Main Bedroom",   "CSLA2CD",              NULL),
    (1024, 1, 2,   1,    "Kitchen",        "CSLA2CD",              NULL),
    (1025, 1, 2,   1,    "Garage",         "CSLA2CD",              NULL),
    (1026, 1, 2,   1,    "Laundry",        "CSLA2CD",              NULL),

    # for solar system #2 
    (2021, 2, 2,   1,    "Lounge",         "CSLA2CD",              NULL),
    (2022, 2, 2,   1,    "Office",         "CSLA2CD",              NULL),
    (2023, 2, 2,   1,    "Main Bedroom",   "CSLA2CD",              NULL),
    (2024, 2, 2,   1,    "Kitchen",        "CSLA2CD",              NULL),
    (2025, 2, 2,   1,    "Garage",         "CSLA2CD",              NULL),

    # for solar system #3 
    (3021, 3, 2,   1,    "Lounge",         "CSLA2CD",              NULL),
    (3022, 3, 2,   1,    "Office",         "CSLA2CD",              NULL),
    (3023, 3, 2,   1,    "Main Bedroom",   "CSLA2CD",              NULL),
    (3024, 3, 2,   1,    "Kitchen",        "CSLA2CD",              NULL),
    (3025, 3, 2,   1,    "Garage",         "CSLA2CD",              NULL),

    # BatterySocSensors (Id, SiteId, TypeId, Name, Model, Manufacturer, Description, DetailedSpecs, Device)
    (1031,  1, 3,   NULL, "Battery SOC",    "Tracer 4215BN",        "2e006e20-c40f-11e6-bbbc-002522ab4073"),
    (2031, 2, 3,   NULL, "Battery SOC",    "Tracer 4215BN",        "32e4e466-c40f-11e6-bf27-002522ab4073"),
    (3031, 3, 3,   NULL, "Battery SOC",    "Tracer 4215BN",        "368aa2ea-c40f-11e6-972a-002522ab4073"),

    # BatteryChargeStageSensors (Id, SiteId, TypeId, Name, Model, Manufacturer, Description, DetailedSpecs, Device)
    (1041, 1, 4,   NULL, "Battery Charge Stage", "Tracer 4215BN",    "2e006e20-c40f-11e6-bbbc-002522ab4073"),
    (2041, 2, 4, NULL, "Battery Charge Stage", "Tracer 4215BN",    "32e4e466-c40f-11e6-bf27-002522ab4073"),
    (3041, 3, 4, NULL, "Battery Charge Stage", "Tracer 4215BN",    "368aa2ea-c40f-11e6-972a-002522ab4073"),

    # ChargeControllerLoadOutputs (Id, SiteId, TypeId, Name, Model, Manufacturer, Description, DetailedSpecs, Device)
    (1051, 1,   5, NULL, "Charge Controller Load ON", "Tracer 4215BN", "2e006e20-c40f-11e6-bbbc-002522ab4073"),
    (2051, 2, 5, NULL, "Charge Controller Load ON", "Tracer 4215BN", "32e4e466-c40f-11e6-bf27-002522ab4073"),
    (3051, 3, 5, NULL, "Charge Controller Load ON", "Tracer 4215BN", "368aa2ea-c40f-11e6-972a-002522ab4073"),

    # BatteryStatsSensors (Id, SiteId, TypeId, Name, Model, Manufacturer, Description, DetailedSpecs, Device)
    (1061, 1, 6,   NULL, "Battery Stats", "Tracer 4215BN",         "2e006e20-c40f-11e6-bbbc-002522ab4073"),
    (2061, 2, 6, NULL, "Battery Stats", "Tracer 4215BN",         "32e4e466-c40f-11e6-bf27-002522ab4073"),
    (3061, 3, 6, NULL, "Battery Stats", "Tracer 4215BN",         "368aa2ea-c40f-11e6-972a-002522ab4073"),

    # EnergyStatsSensors (Id, SiteId, TypeId, Name, Model, Manufacturer, Description, DetailedSpecs, Device)
    (1071, 1, 7,   NULL, "Energy Generated Stats", "Tracer 4215BN", "2e006e20-c40f-11e6-bbbc-002522ab4073"),
    (1072, 1, 7,   NULL, "Energy Consumed Stats",  "Tracer 4215BN", "2e006e20-c40f-11e6-bbbc-002522ab4073"),

    (2071, 2, 7, NULL, "Energy Generated Stats", "Tracer 4215BN", "32e4e466-c40f-11e6-bf27-002522ab4073"),
    (2072, 2, 7, NULL, "Energy Consumed Stats",  "Tracer 4215BN", "32e4e466-c40f-11e6-bf27-002522ab4073"),

    (3071, 3, 7, NULL, "Energy Generated Stats", "Tracer 4215BN", "368aa2ea-c40f-11e6-972a-002522ab4073"),
    (3072, 3, 7, NULL, "Energy Consumed Stats",  "Tracer 4215BN", "368aa2ea-c40f-11e6-972a-002522ab4073"),

    # TemperatureSensors (Id, SiteId, TypeId, Name, Model, Manufacturer, Description, DetailedSpecs, Device)
    (1081, 1, 8,   NULL, "Outside",            "HTU21",           NULL),
    (1082, 1, 8,   NULL, "Office",             "SHT31",           NULL),
    (1083, 1, 8,   NULL, "Piano Room",         "HTU21",           NULL),
    (1084, 1, 8,   NULL, "Shipping Container", "HTU21",           NULL),
    (1085, 1, 8,   NULL, "Lounge",             "HTU21",           NULL),
    (1086, 1, 8,   NULL, "Battery",            "Tracer 4215BN",   "2e006e20-c40f-11e6-bbbc-002522ab4073"),
    (1087, 1, 8,   NULL, "Charge Controller",  "Tracer 4215BN",   "2e006e20-c40f-11e6-bbbc-002522ab4073"),

    (2181, 2, 8, NULL, "Outside",            "HTU21",           NULL),
    (2182, 2, 8, NULL, "Office",             "SHT31",           NULL),
    (2183, 2, 8, NULL, "Battery",            "Tracer 4215BN",   "32e4e466-c40f-11e6-bf27-002522ab4073"),
    (2184, 2, 8, NULL, "Charge Controller",  "Tracer 4215BN",   "32e4e466-c40f-11e6-bf27-002522ab4073"),

    (3081, 3, 8, NULL, "Outside",            "HTU21",           NULL),
    (3082, 3, 8, NULL, "Office",             "SHT31",           NULL),
    (3083, 3, 8, NULL, "Piano Room",         "HTU21",           NULL),
    (3084, 3, 8, NULL, "Shipping Container", "HTU21",           NULL),
    (3085, 3, 8, NULL, "Lounge",             "HTU21",           NULL),
    (3086, 3, 8, NULL, "Battery",            "Tracer 4215BN",   "368aa2ea-c40f-11e6-972a-002522ab4073"),
    (3087, 3, 8, NULL, "Charge Controller",  "Tracer 4215BN",   "368aa2ea-c40f-11e6-972a-002522ab4073"),

    # HumiditySensors (Id, SiteId, TypeId, Name, Model, Manufacturer, Description, DetailedSpecs)
    (1091, 1, 9,   NULL, "Outside Humidity",            "HTU21",   NULL),
    (1092, 1, 9,   NULL, "Office Humidity",             "SHT31",   NULL),
    (1093, 1, 9,   NULL, "Piano Room Humidity",         "HTU21",   NULL),
    (1094, 1, 9,   NULL, "Shipping Container Humidity", "HTU21",   NULL),
    (1095, 1, 9,   NULL, "Lounge Humidity",             "HTU21",   NULL),

    (2091, 2, 9, NULL, "Outside Humidity",            "HTU21",   NULL),
    (2092, 2, 9, NULL, "Office Humidity",             "SHT31",   NULL),

    (3091, 3, 9, NULL, "Outside Humidity",            "HTU21",   NULL),
    (3092, 3, 9, NULL, "Office Humidity",             "SHT31",   NULL),
    (3093, 3, 9, NULL, "Piano Room Humidity",         "HTU21",   NULL),
    (3094, 3, 9, NULL, "Shipping Container Humidity", "HTU21",   NULL),
    (3095, 3, 9, NULL, "Lounge Humidity",             "HTU21",   NULL),

    # BarometricPressureSensors (Id, SiteId, TypeId, Name, Model, Manufacturer, Description, DetailedSpecs)
    (1101, 1, 10,   NULL, "Outside Pressure",           "HTU21",   NULL),
    (1102, 1, 10,   NULL, "Office Pressure",            "SHT31",   NULL),
    (2101, 2, 10, NULL, "Outside Pressure",           "HTU21",   NULL),
    (2102, 2, 10, NULL, "Office Pressure",            "SHT31",   NULL),
    (3101, 3, 10, NULL, "Outside Pressure",           "HTU21",   NULL),
    (3102, 3, 10, NULL, "Office Pressure",            "SHT31",   NULL),

    # WindSensors (Id, SiteId, TypeId, Name, Model, Manufacturer, Description, DetailedSpecs)
    (1111, 1, 11,   NULL, "Sensor1",                    "HTU21",   NULL),
    (2111, 2, 11, NULL, "Sensor1",                    "HTU21",   NULL),
    (3111, 3, 11, NULL, "Sensor1",                    "HTU21",   NULL);

    

