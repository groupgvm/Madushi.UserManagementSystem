using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility.Implementation;

namespace Geveo.Users.Common
{
    public class LoggerService : ILoggerService
    {
        private readonly TelemetryClient _telemetryClient;

        public LoggerService(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }
        public void LogError(string errorMessage)
        {
            Console.WriteLine($"{errorMessage}");
        }

        public void LogInfo(string message)
        {
            Console.WriteLine($"{message}");
        }
    }
}