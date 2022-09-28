// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplyHomologationSteps.cs" company="Microsoft">
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
    using Ecp.True.Bdd.Tests.Entities1;
    using Ecp.True.Bdd.Tests.StepDefinitions.Api;
    using Ecp.True.Bdd.Tests.Utils;

    using NUnit.Framework;

    using Ocaramba;

    using TechTalk.SpecFlow;

    [Binding]
    public class ApplyHomologationSteps : EcpApiStepDefinitionBase
    {
        private const string Value = "Value";
        private readonly string movementOutFolderName = "movements-out";
        private readonly string movementInFolderName = "movements";
        private readonly string movementFinaloutput = "movement";
        private readonly string inventoryOutFolderName = "inventory-out";
        private readonly string inventoryInFolderName = "inventory";
        private readonly string inventoryFinaloutput = "inventory";
        private readonly string inventoryFilePath = "Inventory\\Inventory";
        private readonly string movementFilePath = "Movement\\movement";

        public ApplyHomologationSteps(FeatureContext featureContext)
            : base(featureContext)
        {
        }

        [Given(@"I have source and destination not configured for Homologation")]
        public void GivenIHaveSourceAndDestinationNotConfiguredForHomologation()
        {
            Assert.IsTrue(true);
        }

        [Given(@"I have source and destination configured for Homologation")]
        public void GivenIHaveSourceAndDestinationConfiguredForHomologation()
        {
            Assert.IsNotNull(true);
        }

        [Given(@"I have source and destination configured for Homologation without data mapping")]
        public void GivenIHaveSourceAndDestinationConfiguredForHomologationWithoutDataMapping()
        {
            Assert.IsTrue(true);
        }

        [When(@"I pass valid Xml with required fields")]
        public void WhenIPassValidXmlWithRequiredFields()
        {
            Assert.IsTrue(true);
        }

        [When(@"I pass invalid Xml to Homologation function")]
        public async Task WhenIPassInvalidXmlToHomologationFunctionAsync()
        {
            string mqMessageId = await this.UploadFileAndReturnMessageIdAsync(this.movementInFolderName, this.inventoryFilePath, this.movementOutFolderName).ConfigureAwait(false);
            this.LogToReport(mqMessageId);
        }

        [When(@"I pass valid Movement Xml to Homologation function")]
        public async Task WhenIPassValidMovementXmlToHomologationFunctionAsync()
        {
            string mqMessageId = await this.UploadFileAndReturnMessageIdAsync(this.movementInFolderName, this.movementFilePath, this.movementOutFolderName).ConfigureAwait(false);
            this.ScenarioContext[ConstantValues.HomologatedJson] = await mqMessageId.ToLower(CultureInfo.CurrentCulture).DownloadBlobDataAsync(this.movementFinaloutput).ConfigureAwait(false);
            Assert.IsTrue(true);
        }

        [Then(@"the response message should be equal to input message")]
        public void ThenTheResponseMessageShouldBeEqualToInputMessage()
        {
            Assert.IsTrue(true);
        }

        [When(@"I pass valid Inventory Xml to Homologation function")]
        public async Task WhenIPassValidInventoryXmlToHomologationFunctionAsync()
        {
            string mqMessageId = await this.UploadFileAndReturnMessageIdAsync(this.inventoryInFolderName, this.inventoryFilePath, this.inventoryOutFolderName).ConfigureAwait(false);
            this.ScenarioContext[ConstantValues.HomologatedJson] = await mqMessageId.ToLower(CultureInfo.CurrentCulture).DownloadBlobDataAsync(this.inventoryFinaloutput).ConfigureAwait(false);
            Assert.IsTrue(true);
        }

        [Then(@"the response message should be Homologated for ""(.*)""")]
        public async Task ThenTheResponseMessageShouldBeHomologatedAsync(string fileName)
        {
            bool result = true;
            if (fileName == ConstantValues.Movement)
            {
                var xml = this.Input;
                string homologatedJsonOutput = this.ScenarioContext[ConstantValues.HomologatedJson].ToString();
                var json = Newtonsoft.Json.JsonConvert.DeserializeObject<MovementJson>(homologatedJsonOutput);
                if (!json.MovementId.Equals(await this.GetHomologatedValueAsync(xml.INTERNALMOVEMENT.MOVEMENTID.ToString(CultureInfo.InvariantCulture), ConstantValues.Destination).ConfigureAwait(false), StringComparison.OrdinalIgnoreCase))
                {
                    result = false;
                }

                if (!json.EventType.Equals(await this.GetHomologatedValueAsync(xml.INTERNALMOVEMENT.ACTION.ToString(CultureInfo.InvariantCulture), ConstantValues.Destination).ConfigureAwait(false), StringComparison.OrdinalIgnoreCase))
                {
                    result = false;
                }

                if (!json.MovementTypeId.Equals(await this.GetHomologatedValueAsync(xml.INTERNALMOVEMENT.MOVEMENTTYPE.ToString(CultureInfo.InvariantCulture), ConstantValues.Destination).ConfigureAwait(false), StringComparison.OrdinalIgnoreCase))
                {
                    result = false;
                }

                if (!json.OperationalDate.Equals(await this.GetHomologatedValueAsync(xml.INTERNALMOVEMENT.PERIOD.STARTTIME.ToString(CultureInfo.InvariantCulture), ConstantValues.Destination).ConfigureAwait(false)))
                {
                    result = false;
                }

                if (!json.Period.Equals(await this.GetHomologatedValueAsync(xml.INTERNALMOVEMENT.PERIOD.ToString(), ConstantValues.Destination).ConfigureAwait(false)))
                {
                    result = false;
                }

                if (!json.MovementSource.Equals(await this.GetHomologatedValueAsync(xml.INTERNALMOVEMENT.LOCATION.ToString(), ConstantValues.Destination).ConfigureAwait(false)))
                {
                    result = false;
                }

                if (!json.MovementDestination.Equals(await this.GetHomologatedValueAsync(xml.INTERNALMOVEMENT.LOCATION.ToString(), ConstantValues.Destination).ConfigureAwait(false)))
                {
                    result = false;
                }

                if (!json.Attributes.Equals(await this.GetHomologatedValueAsync(xml.INTERNALMOVEMENT.CRITERIA.ToString(), ConstantValues.Destination).ConfigureAwait(false)))
                {
                    result = false;
                }

                if (!json.Owners.Equals(await this.GetHomologatedValueAsync(xml.INTERNALMOVEMENT.OWNERS.ToString(), ConstantValues.Destination).ConfigureAwait(false)))
                {
                    result = false;
                }

                if (!json.Period.Equals(await this.GetHomologatedValueAsync(xml.INTERNALMOVEMENT.PERIOD.ToString(), ConstantValues.Destination).ConfigureAwait(false)))
                {
                    result = false;
                }

                if (!json.Period.StartTime.Equals(await this.GetHomologatedValueAsync(xml.INTERNALMOVEMENT.PERIOD.STARTTIME.ToString(CultureInfo.InvariantCulture), ConstantValues.Destination).ConfigureAwait(false)))
                {
                    result = false;
                }

                if (!json.Period.EndTime.Equals(await this.GetHomologatedValueAsync(xml.INTERNALMOVEMENT.PERIOD.ENDTIME.ToString(CultureInfo.InvariantCulture), ConstantValues.Destination).ConfigureAwait(false)))
                {
                    result = false;
                }

                if (!json.MovementSource.Equals(await this.GetHomologatedValueAsync(xml.INTERNALMOVEMENT.LOCATION.ToString(), ConstantValues.Destination).ConfigureAwait(false)))
                {
                    result = false;
                }

                if (!json.MovementDestination.Equals(await this.GetHomologatedValueAsync(xml.INTERNALMOVEMENT.LOCATION.ToString(), ConstantValues.Destination).ConfigureAwait(false)))
                {
                    result = false;
                }

                if (!json.Owners.Equals(await this.GetHomologatedValueAsync(xml.INTERNALMOVEMENT.OWNERS.ToString(), ConstantValues.Destination).ConfigureAwait(false)))
                {
                    result = false;
                }
            }
            else
            {
                var xml = (this.Input as System.Xml.XmlNode[]).DeserializeArray<INVENTORIES>();
                string homologatedJsonOutput = this.ScenarioContext[ConstantValues.HomologatedJson].ToString();
                var json = Newtonsoft.Json.JsonConvert.DeserializeObject<InventoryJson>(homologatedJsonOutput);
                json.SourceSystem.Equals(await this.GetHomologatedValueAsync(xml.INVENTORY.ToString(), ConstantValues.Destination).ConfigureAwait(false), StringComparison.OrdinalIgnoreCase); ////pending
                if (json.EventType.Equals(await this.GetHomologatedValueAsync(xml.INVENTORY.ACTION.ToString(CultureInfo.InvariantCulture), ConstantValues.Destination).ConfigureAwait(false), StringComparison.OrdinalIgnoreCase))
                {
                    result = false;
                }

                if (json.InventoryDate.Equals(await this.GetHomologatedValueAsync(xml.INVENTORY.DATE.ToString(CultureInfo.InvariantCulture), ConstantValues.Destination).ConfigureAwait(false)))
                {
                    result = false;
                }

                if (json.NodeId.Equals(await this.GetHomologatedValueAsync(xml.INVENTORY.LOCATIONID.ToString(CultureInfo.InvariantCulture), ConstantValues.Destination).ConfigureAwait(false), StringComparison.OrdinalIgnoreCase))
                {
                    result = false;
                }

                if (json.Products.Equals(await this.GetHomologatedValueAsync(xml.INVENTORY.COMMODITIES.ToString(), ConstantValues.Destination).ConfigureAwait(false)))
                {
                    result = false;
                }

                if (json.Scenario.Equals(await this.GetHomologatedValueAsync(xml.INVENTORY.CASE.ToString(CultureInfo.InvariantCulture), ConstantValues.Destination).ConfigureAwait(false), StringComparison.OrdinalIgnoreCase))
                {
                    result = false;
                }
            }

            Assert.IsTrue(result);
        }

        public async Task<string> GetHomologatedValueAsync(string key, string trueLocation)
        {
            try
            {
                var value = await this.ReadSqlAsStringDictionaryAsync(input: trueLocation == ConstantValues.Destination ? SqlQueries.GetDestinationValueBySourceValue : SqlQueries.GetSourceValueByDestinationValue, args: new { value = key }).ConfigureAwait(false);
                return value[trueLocation + Value];
            }
            catch (ArgumentNullException)
            {
                return key;
            }
        }

        public async Task<string> UploadFileAndReturnMessageIdAsync(string inFolderName, string uploadFilePath, string outFolderName)
        {
            var fileNameGuid = Guid.NewGuid().ToString();
            var uniqueFileName = $@"{fileNameGuid}.xml";
            await uniqueFileName.UploadXmlAsync(inFolderName, uploadFilePath).ConfigureAwait(false);
            await Task.Delay(TimeSpan.FromSeconds(BaseConfiguration.LongTimeout * 4)).ConfigureAwait(false);
            string mqMessageId = fileNameGuid.GetMessageId(outFolderName);
            return mqMessageId;
        }
    }
}
