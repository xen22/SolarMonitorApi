namespace SolarMonitor.Data.Resources
{
    public class SolarArray : Device
    {
        public string Configuration { get; set; }
        public int? NumPanels { get; set; }

        public int? PanelMaxPower_W { get; set; }
        public float? PanelOpenCircuitVoltage_V { get; set; }
        public float? PanelShortCircuitCurrent_A { get; set; }
    }
}
