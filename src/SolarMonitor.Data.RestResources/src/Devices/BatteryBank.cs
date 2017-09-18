namespace SolarMonitor.Data.Resources
{
    public class BatteryBank : Device
    {
        public string Configuration { get; set; }
        public int? NumBatteries { get; set; }

        public float? CapacityPerBattery_Ah { get; set; }
        public float? TotalCapacity_Ah { get; set; }
        public float? BankVoltage_V { get; set; }
        public float? BatteryVoltage_V { get; set; }
    }
}
