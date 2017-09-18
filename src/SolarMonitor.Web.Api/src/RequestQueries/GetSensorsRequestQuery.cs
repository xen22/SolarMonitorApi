using SolarMonitorApi.Validators;

namespace SolarMonitorApi.RequestQueries
{
    /// <summary>
    /// Contains parameters passed to the SensorsController.Get() action, packaged in this class for convenience.
    /// </summary>
    public class GetSensorsRequestQuery : CollectionRequestQuery
    {
        /// <summary>A list of sensor types separated by '|' (Obtain sensor types from '/sensors/types') </summary>
        [SensorTypesValidation]
        public string Type { get; set; } = "";

        /// <summary>A list of sensor names separated by '|'</summary>
        public string Name { get; set; } = "";

        /// <summary>A list of sites where the sensors are located, separated by '|'</summary>
        public string Site { get; set; } = "";

    }
}
