using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System;

namespace HelloDW.Functions.Helpers
{
    internal class InsightsHelper
    {
        private static string key
            = TelemetryConfiguration.Active.InstrumentationKey
            = Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", EnvironmentVariableTarget.Process);
        internal static TelemetryClient Telemetry = new TelemetryClient() { InstrumentationKey = key };
    }
}
