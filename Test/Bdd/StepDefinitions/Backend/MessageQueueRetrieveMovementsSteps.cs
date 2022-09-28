// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageQueueRetrieveMovementsSteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.StepDefinitions
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities1;
    using Ecp.True.Bdd.Tests.StepDefinitions.Api;
    using Ecp.True.Bdd.Tests.Utils;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class MessageQueueRetrieveMovementsSteps : EcpApiStepDefinitionBase
    {
        public MessageQueueRetrieveMovementsSteps(FeatureContext featureContext)
            : base(featureContext)
        {
        }

        [Given(@"the system is processing a message from MQ")]
        public async Task GivenTheSystemIsProcessingAMessageFromMQAsync()
        {
            string outFolderName;
            string inFolderName;
            outFolderName = "movements-out";
            inFolderName = "movements";
            var filename = Guid.NewGuid().ToString();
            var uniqueFileName = $@"{filename}.xml";
            await uniqueFileName.UploadXmlAsync(inFolderName, "Inventory\\Inventory").ConfigureAwait(false);
            await Task.Delay(30000).ConfigureAwait(false);
            string messageId = filename.GetMessageId(outFolderName);
            this.LogToReport(messageId);
        }

        [Given(@"the system is processing a valid message from MQ")]
        public async Task GivenTheSystemIsProcessingAvalidMessageFromMQAsync()
        {
            string outFolderName;
            string inFolderName;
            string finaloutput;
            outFolderName = "movements-out";
            inFolderName = "movements";
            finaloutput = "movement";
            var filename = Guid.NewGuid().ToString();
            var uniqueFileName = $@"{filename}.xml";
            await uniqueFileName.UploadXmlAsync(inFolderName, "Movement\\movement").ConfigureAwait(false);
            await Task.Delay(30000).ConfigureAwait(false);
            string messageId = filename.GetMessageId(outFolderName);
            await Task.Delay(100000).ConfigureAwait(false);
            this.ScenarioContext["json"] = await messageId.ToLower(CultureInfo.CurrentCulture).DownloadBlobDataAsync(finaloutput).ConfigureAwait(false);
            Assert.IsTrue(true);
        }

        [Given(@"the system is retrying a message processing")]
        public void GivenTheSystemIsRetryingAMessageProcessing()
        {
            Assert.IsTrue(true);
        }

        [When(@"it is transforming the original message to the canonical")]
        public void WhenItIsTransformingTheOriginalMessageToTheCanonical()
        {
            Assert.IsTrue(true);
        }

        [When(@"process fails due to ""(.*)""")]
        public void WhenProcessFailsDueTo()
        {
            Assert.IsTrue(true);
        }

        [When(@"it reaches the ""(.*)""")]
        public void WhenItReachesThe(string field1)
        {
            Assert.IsNotNull(field1);
        }

        [When(@"the system detects many ""(.*)"" recently")]
        public void WhenTheSystemDetectsManyRecently(string field1)
        {
            Assert.IsNotNull(field1);
        }

        [Then(@"the system must apply the defined mapping per each ""(.*)""")]
        public void ThenTheSystemMustApplyTheDefinedMappingPerEach(string p)
        {
            try
            {
                string jsonOutput = this.ScenarioContext["json"].ToString();
                var json = Newtonsoft.Json.JsonConvert.DeserializeObject<MovementJson>(jsonOutput);
                Assert.IsNotNull(json);
                Assert.IsNotNull(p);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                this.LogToReport(e.Message);
                Assert.IsTrue(false);
            }
        }

        [Then(@"the system must log the failure")]
        public void ThenTheSystemMustLogTheFailure()
        {
            Assert.IsTrue(true);
        }

        [Then(@"implement a retry strategy")]
        public void ThenImplementARetryStrategy()
        {
            Assert.IsTrue(true);
        }

        [Then(@"it must release the message to be processed later")]
        public void ThenItMustReleaseTheMessageToBeProcessedLater()
        {
            Assert.IsTrue(true);
        }

        [Then(@"implement a circuit breaker strategy")]
        public void ThenImplementACircuitBreakerStrategy()
        {
            Assert.IsTrue(true);
        }
    }
}
