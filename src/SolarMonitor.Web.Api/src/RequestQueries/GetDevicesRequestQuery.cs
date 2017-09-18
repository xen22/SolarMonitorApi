using System.ComponentModel.DataAnnotations;
using SolarMonitorApi.Validators;

namespace SolarMonitorApi.RequestQueries
{
    /// <summary>
    /// Contains a set of query properties for the DevicesController.Get() query.
    /// </summary>    
    public class GetDevicesRequestQuery : CollectionRequestQuery
    {
        /// <summary>A list of device types separated by '|' (Obtain device types from '/devices/types') </summary>
        [DeviceTypesValidation]
        public string Type { get; set; } = "";

        /// <summary>A list of device names separated by '|'</summary>
        public string Name { get; set; } = "";

        /// <summary>A list of sites separated by '|'</summary>
        public string Site { get; set; } = "";

    }
}
