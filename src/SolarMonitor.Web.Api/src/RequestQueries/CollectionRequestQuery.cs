using System.ComponentModel.DataAnnotations;

namespace SolarMonitorApi.RequestQueries
{
    /// <summary>
    /// Parameters common across all resource collections.
    /// </summary>
    public class CollectionRequestQuery
    {
        /// <summary>The page number </summary>
        [Range(1, int.MaxValue, ErrorMessage = "PageIndex parameter must start at 1")]
        public int? PageIndex { get; set; } = QueryParameterDefaults.PageIndex;

        /// <summary>Number of entries per page</summary>
        [Range(1, int.MaxValue, ErrorMessage = "PageSize parameter must start at 1")]
        public int? PageSize { get; set; } = QueryParameterDefaults.PageSize;


        // Note: this is not necessary here -> it might be useful in the Measurement queries as a limit 
        // for the number of elements to return if only startTime or endTime is specified.
        /// <summary>Total number of entries</summary>
        [Range(1, int.MaxValue, ErrorMessage = "TotalCount parameter must start at 1")]
        public int? TotalCount { get; set; } = 10;

    }
}
