namespace SolarMonitor.Data.CommonTypes
{
    public enum SensorType
    {
        Unset = 0,
        Shunt,
        CurrentSensor,
        BatterySocSensor,
        BatteryChargeStageSensor,
        ChargeControllerLoadOutput,
        BatteryStatsSensor,
        EnergyStatsSensor,
        TemperatureSensor,
        HumiditySensor,
        BarometricPressureSensor,
        WindSensor,
    }
}
