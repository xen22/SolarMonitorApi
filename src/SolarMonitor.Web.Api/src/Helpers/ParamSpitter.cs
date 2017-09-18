using System.Collections.Generic;
using System.Linq;

namespace SolarMonitorApi.Helpers
{
    public class ParamSplitter
    {
        public static IEnumerable<string> SplitParam(string p)
        {
            return string.IsNullOrWhiteSpace(p) ? Enumerable.Empty<string>() :
                p.Split('|').Select(t => t.Trim()).Where(t => !string.IsNullOrWhiteSpace(t));
        }
    }
}
