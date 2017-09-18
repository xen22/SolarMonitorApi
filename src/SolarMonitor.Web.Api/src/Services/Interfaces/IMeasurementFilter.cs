using System;
using System.Linq.Expressions;

namespace SolarMonitorApi.Services
{
    public interface IMeasurementFilter<T>
    {
        Expression<Func<T, bool>> GetFilterExpression();
    }
}
