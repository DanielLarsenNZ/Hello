using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace HelloDW.Functions.Tests
{
    [TestClass]
    public class RunDFPipelineTests
    {
        [TestMethod]
        public async Task Run()
        {
            await RunDFPipeline.Run(
                new Mock<EventGridEvent>().Object, 
                new Mock<ILogger>().Object, 
                new Mock<ExecutionContext>().Object);
        }
    }
}
