namespace SolarMonitor.Data.Resources
{
    public class UriConstants
    {
        public static string BaseURL = "/api";
        public static string SitesPrefix = $"{BaseURL}/sites";
        public static string SensorsPrefix = $"{BaseURL}/sensors";
        public static string SensorTypesPrefix = $"{BaseURL}/sensor/types";
        public static string DevicesPrefix = $"{BaseURL}/devices";
        public static string DeviceTypesPrefix = $"{BaseURL}/devices/types";
        public static string MeasurementsPrefix = $"{BaseURL}/measurements";

        public static string LinkKeySelf = "self";
    }
}
