using HelloDW.Functions.Helpers;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloDW.Functions
{
    public static class CallLogger
    {
        static Random _random = new Random();
        static readonly Lazy<CloudBlobClient> _lazyClient = new Lazy<CloudBlobClient>(InitializeCloudBlobClient);
        private static CloudBlobClient CloudBlobClient => _lazyClient.Value;
        static IConfiguration _config = null;

        [FunctionName("CallLogger")]
        public static async Task Run(
            [TimerTrigger("0 */1 * * * *", RunOnStartup = false)]TimerInfo timer,
            ILogger log,
            ExecutionContext context)
        {
            log.LogInformation($"HelloDW.Functions.CallLogger: C# Timer trigger function executed at: {DateTime.Now}");

            _config = FunctionsHelper.GetConfig(context);

            const int subscribers = 1000;
            const int maxCallLength = 600;
            const int secondsInThisPeriod = 60;

            int callsThisPeriod = RandomIntBetweenZeroAnd(1000);
            var now = DateTime.UtcNow;
            // strip the seconds from Now and go back 1 minute
            DateTime startDateTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0).AddMinutes(-1);
            string startDateTimeStamp = startDateTime.ToString("yyyyMMddhhmm");
            string blobName = $"{startDateTimeStamp}.log";
            CloudBlockBlob blob = GetBlobReference(blobName);

            log.LogInformation($"HelloDW.Functions.CallLogger: blob = {blobName}, callsThisPeriod = {callsThisPeriod}");

            // Track this event in Application Insights
            InsightsHelper.Telemetry.TrackEvent(
                "hellodw/calllogger/putblob",
                properties: new Dictionary<string, string> {
                    { "DateTimeStamp", startDateTimeStamp },
                    { "Filename", blobName },
                },
                metrics: new Dictionary<string, double> { { "CallCount", callsThisPeriod } });

            List<string> lines = new List<string>(callsThisPeriod);
            for (int i = 0; i < callsThisPeriod; i++)
            {
                string timestamp = startDateTime
                    .AddSeconds(RandomIntBetweenZeroAnd(secondsInThisPeriod))
                    .ToString("yyyyMMddhhmmss");
                string number = $"5555{RandomIntBetweenZeroAnd(subscribers).ToString("0000")}";
                int length = RandomIntBetweenZeroAnd(maxCallLength);

                lines.Add($"{timestamp},{number},{length}");
            }

            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    writer.Write(string.Join("\r\n", lines.OrderBy(l => l).ToArray()));
                    stream.Position = 0;
                    await blob.UploadFromStreamAsync(stream);
                }
            }
        }

        private static int RandomIntBetweenZeroAnd(int max) => Convert.ToInt32(Math.Ceiling(_random.NextDouble() * max));

        private static CloudBlobClient InitializeCloudBlobClient()
        {
            if (_config["Blob.StorageConnectionString"] == null)
                throw new InvalidOperationException("App Setting \"Blob.StorageConnectionString\" is not set.");

            var account = CloudStorageAccount.Parse(_config["Blob.StorageConnectionString"]);
            return account.CreateCloudBlobClient();
        }

        private static CloudBlockBlob GetBlobReference(string name)
        {
            // Setup blob
            if (_config["Blob.ContainerName"] == null)
                throw new InvalidOperationException("App Setting \"Blob.ContainerName\" is missing.");

            CloudBlobContainer container = CloudBlobClient.GetContainerReference(_config["Blob.ContainerName"]);
            return container.GetBlockBlobReference(name);
        }
    }
}
