namespace SolarMonitor.Data.Resources
{
    public class Inverter : Device
    {
        public float? MaxContinuousPower_W { get; set; }
        public float? MaxSurgePower_W { get; set; }
        public float? InputVoltage_V { get; set; }
        public float? OutputVoltage_V { get; set; }
    }
}
