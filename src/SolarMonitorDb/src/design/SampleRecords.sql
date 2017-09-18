USE SolarMonitorDb;

delimiter #


CREATE PROCEDURE GenerateSolarRecordsData()
BEGIN

  SET @my_site = "Simulation 1";
  SET @site_id = ( SELECT Sites.Id FROM Sites WHERE Sites.Name = @my_site );
  SET @solar_system_id = ( SELECT SolarSystems.Id FROM SolarSystems INNER JOIN Sites ON 
                            Sites.SolarSystems_Id = SolarSystems.Id AND Sites.Name = @my_site );

  SET @period = 10; -- in seconds
  SET @i=0;
  SET @duration = '00:00:10';
  
--  SET @t = CAST('2015-07-01 00:00:00' AS DATETIME);
  SET @t = CAST('2016-05-07 00:00:00' AS DATETIME);

  SET @prev_t = SUBTIME(@t, '00:00:10');

  WHILE @t <= CAST('2016-08-07 23:59:59' AS DATETIME) DO

    SET @period = UNIX_TIMESTAMP(@t) - UNIX_TIMESTAMP(@prev_t); 

    -- SolarRecords
    INSERT INTO SolarRecords (Timestamp, Sites_Id)
      VALUES(@t, @site_id);

    -- ControllerMeasurements
    INSERT INTO ControllerMeasurements (Sites_Id, SolarRecords_Timestamp, SolarControllers_Id, ArrayCurrent_A, ArrayVoltage_V, 
      BatteryCurrent_A, BatteryVoltage_V, BatterySOC, BatteryChargeStatus, BatteryTemperature_C, BatteryMaxVoltage_V, 
      BatteryMinVoltage_V, ControllerTemperature_C, LoadCurrent_A, LoadVoltage_V, LoadOn, Interval_s)
      VALUES(@site_id, @t, 1, 11.1, 12.3, 10.4, 12.0 + HOUR(@t)/10, 70 + HOUR(@t), "Bulk", 10 + HOUR(@t) + MINUTE(@t)/60, 12.9, 12.1, 18 + HOUR(@t), 1.1 * HOUR(@t), 12, TRUE, @period);

    -- LoadMeasurements
    -- Note: this will only work if the CurrentSensors IDs related to SolarSystem 1 are consecutive and starting at 1  
    SET @j = (SELECT COUNT(*) FROM CurrentSensors WHERE SolarSystems_Id = @solar_system_id);
    SET @adjustment_factor1 = HOUR(@t) % 4;
    START TRANSACTION;
    WHILE @j > 0 DO 
      INSERT INTO LoadMeasurements (Sites_Id, CurrentSensors_Id, SolarRecords_Timestamp, CurrentDraw_A, Interval_s)
        VALUES(@site_id, @j, @t, @j*@adjustment_factor1*1.5, @period);
      SET @j = @j - 1;
    END WHILE;
    COMMIT;

    -- ShuntMeasurements
    -- Note: this will only work if the Shunts IDs related to SolarSystem 1 are consecutive and starting at 1  
    SET @k = (SELECT COUNT(*) FROM Shunts WHERE SolarSystems_Id = @solar_system_id);
    SET @adjustment_factor2 = 0.5 + 1.1 * (MINUTE(@t) % (HOUR(@t)+1) + 2) / 4;
    START TRANSACTION;
    WHILE @k > 0 DO
      INSERT INTO ShuntMeasurements (Sites_Id, Shunts_Id, SolarRecords_Timestamp, Current_A, Voltage_V, Interval_s)
        VALUES(@site_id, @k, @t, (@adjustment_factor2 + @k/2), 12, @period);
      SET @k = @k - 1;
    END WHILE;
    COMMIT;

    SET @i = @i + 1;

    -- set the duration between samples to 10s only for a day, 1 Jul 2016, otherwise set it to 30 min
    -- Note: in production, the duration betwen samples will be 10s but if we used this to generate all data,
    --   it would take too long
    -- IF ((YEAR(@prev_time) = 2016) AND (MONTH(@prev_time) = 7) AND (DAY(@prev_time) = 1 )) THEN
    --   SET @duration = '00:00:10';
    -- ELSE 
    --   SET @duration = '00:30:00';
    -- END IF;
    SET @prev_t = @t;
    SET @t = ADDTIME(@t, @duration);

  END WHILE;
END #

CALL GenerateSolarRecordsData();