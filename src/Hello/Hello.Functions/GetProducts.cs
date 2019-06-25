using Hello.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hello.Functions
{
    public static class GetProducts
    {
        [FunctionName("products")]
        public static async Task<IEnumerable<Product>> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            bool.TryParse(req.Query["modified"], out bool modified);

            // Mock Data
            return new Product[]
            {
                new Product
                {
                    Id = "fa28f4e040f94c8da37aa9d50cf385cb",
                    Name = "KORIK RISE GTX",
                    SKUs = new SKU[]
                    {
                        new SKU
                        {
                            SkuName = "42 Black",
                            SuggestedRetailPrice = 150,
                            WholesalePrice = 75
                        },
                        new SKU
                        {
                            SkuName = "43 Black",
                            SuggestedRetailPrice = 150,
                            WholesalePrice = 75
                        },
                        new SKU
                        {
                            SkuName = "44 Black",
                            SuggestedRetailPrice = 150,
                            WholesalePrice = 75
                        }
                    }
                }
            };
        }
    }
}
