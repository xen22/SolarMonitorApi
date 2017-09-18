using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SolarMonitor.Data.CommonTypes;
using SolarMonitorApi.Validators;

namespace SolarMonitorApi.RequestQueries
{

    /// <summary>
    /// Contains a set of query properties for the MeasurementsController.Get() query.
    /// </summary>
    [MeasurementsQueryValidation]
    public class GetMeasurementsRequestQuery : CollectionRequestQuery
    {

        /// <summary>Sensor type (obtained via /sensortypes request)</summary>
        [Required] // (ErrorMessage = "Type is required. Obtain type via /Sensors/Types request.")]
        public SensorType? SensorType { get; set; }

        /// <summary>The GUID of the parent sensor </summary>
        public Guid? SensorGuid { get; set; } = null;

        /// <summary>Measurement type (depends on sensor type) </summary>
        public MeasurementType Type { get; set; } = MeasurementType.Default;

        /// <summary>The sampling rate of the requested measurements. </summary>
        [EnumDataType(typeof(MeasurementsFrequency))]
        [JsonConverter(typeof(StringEnumConverter))]
        public MeasurementsFrequency? Frequency { get; set; } = MeasurementsFrequency.AsInDatabase;

        /// <summary> The start date and time</summary>
        public DateTime? Start { get; set; } = null;

        /// <summary> The end date and time</summary>
        public DateTime? End { get; set; } = null;

    }
}
