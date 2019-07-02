// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using HelloDW.Functions.Helpers;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.Management.DataFactory;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using System;
using System.Threading.Tasks;

namespace HelloDW.Functions
{
    public static class RunDFPipeline
    {
        static IConfiguration _config = null;

        [FunctionName("RunDFPipeline")]
        public static async Task Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log, ExecutionContext context)
        {
            _config = FunctionsHelper.GetConfig(context);

            if (string.IsNullOrEmpty(_config["DataFactory.ResourceGroup"])) throw new InvalidOperationException("App Setting \"DataFactory.TenantId\" is missing.");
            if (string.IsNullOrEmpty(_config["DataFactory.Name"])) throw new InvalidOperationException("App Setting \"DataFactory.TenantId\" is missing.");
            if (string.IsNullOrEmpty(_config["DataFactory.PipelineName"])) throw new InvalidOperationException("App Setting \"DataFactory.TenantId\" is missing.");

            log.LogInformation(eventGridEvent.Data.ToString());

            DataFactoryManagementClient client = await InitDataFactoryClient();

            var runResponse = await client.Pipelines.CreateRunWithHttpMessagesAsync(
                _config["DataFactory.ResourceGroup"],
                _config["DataFactory.Name"],
                _config["DataFactory.PipelineName"]);
        }

        private static async Task<DataFactoryManagementClient> InitDataFactoryClient()
        {
            // Client has to re-aquire a token each run because the token expires

            if (string.IsNullOrEmpty(_config["DataFactory.TenantId"])) throw new InvalidOperationException("App Setting \"DataFactory.TenantId\" is missing.");
            if (string.IsNullOrEmpty(_config["DataFactory.SubscriptionId"])) throw new InvalidOperationException("App Setting \"DataFactory.SubscriptionId\" is missing.");
            if (string.IsNullOrEmpty(_config["DataFactory.ClientId"])) throw new InvalidOperationException("App Setting \"DataFactory.ClientId\" is missing.");
            if (string.IsNullOrEmpty(_config["DataFactory.ClientSecret"])) throw new InvalidOperationException("App Setting \"DataFactory.ClientSecret\" is missing.");

            // Authenticate and create a data factory management client
            var context = new AuthenticationContext("https://login.windows.net/" + _config["DataFactory.TenantId"]);
            ClientCredential cc = new ClientCredential(_config["DataFactory.ClientId"], _config["DataFactory.ClientSecret"]);
            AuthenticationResult result = await context.AcquireTokenAsync("https://management.azure.com/", cc);
            ServiceClientCredentials cred = new TokenCredentials(result.AccessToken);
            return new DataFactoryManagementClient(cred) { SubscriptionId = _config["DataFactory.SubscriptionId"] };
        }
    }
}
