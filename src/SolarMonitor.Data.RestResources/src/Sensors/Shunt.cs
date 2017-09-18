//using Lohmann.HALight;

namespace SolarMonitor.Data.Resources
{
    public class Shunt : Sensor
    {

        public float? InternalResistor_mOhm { get; set; }
        public float? MaxCurrent_A { get; set; }
        public float? InternalVoltage_mV { get; set; }
    }
}
