using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hello.Functions.Helpers
{
    internal class InsightsHelper
    {
        private static string key = TelemetryConfiguration.Active.InstrumentationKey 
            = Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", EnvironmentVariableTarget.Process);
        private static TelemetryClient _telemetry = new TelemetryClient() { InstrumentationKey = key };

        internal static void TrackEvent(string eventName, HttpRequest req)
        {
            var properties = new Dictionary<string, string>();

            foreach (var header in req.Headers)
            {
                // don't track these headers
                if (new[]
                {
                    "Accept-Encoding",
                    "Connection",
                    "Accept",
                    "Cache-Control",
                    "Range",
                    "Cookie",
                    "Upgrade-Insecure-Requests"
                }.Contains(header.Key)) continue;

                properties.TryAdd($"Request.Header.{header.Key}", header.Value);
            }

            _telemetry.TrackEvent(eventName, properties: properties);
        }

        internal static string GetTrackingId(HttpRequest req)
        {
            const string TrackingIdCookieName = "trackingid";
            string id = req.Cookies[TrackingIdCookieName];
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString("N");
                req.HttpContext.Response.Cookies.Append(TrackingIdCookieName, id,
                    new CookieOptions { Expires = DateTime.Now.AddYears(1), SameSite = SameSiteMode.None });
            }

            return id;
        }

        internal static string GetSessionId(HttpRequest req)
        {
            const string SessionIdCookieName = "sessionid";
            string id = req.Cookies[SessionIdCookieName];
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString("N");
                req.HttpContext.Response.Cookies.Append(SessionIdCookieName, id,
                    new CookieOptions { SameSite = SameSiteMode.None });
            }

            return id;
        }
    }
}
