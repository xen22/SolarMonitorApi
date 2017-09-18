using System;

namespace SolarMonitorApi.Validators
{
    class InvalidParameterException : Exception
    {
        public InvalidParameterException(string message)
        : base(message)
        { }
    }
}
