// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using HelloDW.Functions.Helpers;
using System.Collections.Generic;

namespace HelloDW.Functions
{
    public static class RunSparkJob
    {
        [FunctionName("RunSparkJob")]
        public static void Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            // Track this event in Application Insights
            InsightsHelper.Telemetry.TrackEvent(
                "hellodw/calllogger/runsparkjob",
                properties: new Dictionary<string, string> { { "Filename", "XXXX" } });

            log.LogInformation(eventGridEvent.Data.ToString());
        }
    }
}
