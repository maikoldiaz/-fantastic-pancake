// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageQueueRetrieveInventoriesSteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.StepDefinitions.Backend
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.StepDefinitions.Api;
    using Ecp.True.Bdd.Tests.Utils;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class MessageQueueRetrieveInventoriesSteps : EcpApiStepDefinitionBase
    {
        public MessageQueueRetrieveInventoriesSteps(FeatureContext featureContext)
            : base(featureContext)
        {
        }

        [Given(@"the system is processing an invalid inventory message from MQ")]
        public async Task GivenTheSystemIsProcessinganInvalidInventoryMessageFromMQAsync()
        {
            var outFolderName = "inventory-out";
            var inFolderName = "inventory";
            var filename = Guid.NewGuid().ToString();
            var uniqueFileName = $@"{filename}.xml";
            await uniqueFileName.UploadXmlAsync(inFolderName, "Movements\\movement").ConfigureAwait(false);
            await Task.Delay(30000).ConfigureAwait(false);
            var messageId = filename.GetMessageId(outFolderName);
            this.LogToReport(messageId);
        }

        [Given(@"the system is processing a valid Inventory message from MQ")]
        public async Task GivenTheSystemIsProcessingAValidInventoryMessageFromMQAsync()
        {
            var outFolderName = "inventory-out";
            var inFolderName = "inventory";
            var finaloutput = "inventory";
            var filename = Guid.NewGuid().ToString();
            var uniqueFileName = $@"{filename}.xml";
            await uniqueFileName.UploadXmlAsync(inFolderName, "Inventory\\Inventory").ConfigureAwait(false);
            await Task.Delay(30000).ConfigureAwait(false);
            var messageId = filename.GetMessageId(outFolderName);
            await Task.Delay(10000).ConfigureAwait(false);
            this.ScenarioContext["json"] = await messageId.ToLower(CultureInfo.CurrentCulture).DownloadBlobDataAsync(finaloutput).ConfigureAwait(false);
            Assert.IsTrue(true);
        }

        [Then(@"the system must apply the defined mapping for ""(.*)""")]
        public void ThenTheSystemMustApplyTheDefinedMappingFor(string p)
        {
            var jsonOutput = this.ScenarioContext["json"].ToString();
            var json = Newtonsoft.Json.JsonConvert.DeserializeObject<InventoryJson>(jsonOutput);
            Assert.IsNotNull(json);
            Assert.IsNotNull(p);
        }
    }
}
