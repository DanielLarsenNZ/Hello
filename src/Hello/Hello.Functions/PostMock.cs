using Hello.Functions.Helpers;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Hello.Functions
{
    public class PostMock
    {
        /// <summary>
        /// Mock POST endpoint that echoes back the body as a response and will throw random 429 responses
        /// based on a probability determined by an `errormix` query param. For example, `?errormix=90` means
        /// it the endpoint will throw a 429 error 90% of the time, on average, with increasing probability
        /// the more times the endpoint is called ;)
        /// </summary>
        [FunctionName("mock")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string errorMixQuery = req.Query["errormix"];
            string mockServiceName = (string)req.Query["name"] ?? "Hello.Functions.PostMock";

            InsightsHelper.TrackEvent(mockServiceName, req);

            // if an errorMix query param is provided as an integer value between 1 and 100 (inclusive)
            if (int.TryParse(errorMixQuery, out int errorMix) && errorMix > 0 && errorMix <= 100)
            {
                // if random number (between 0 and 100) is greater-than-equal-to (100 - errorMix)
                if ((new Random().NextDouble() * 100d) >= (100 - errorMix))
                {
                    log.LogInformation($"Returning HTTP Status 429. Errormix = {errorMix}");
                    return new StatusCodeResult(429);
                }
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            return new JsonResult(data);
        }
    }
}
