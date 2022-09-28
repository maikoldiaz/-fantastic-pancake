// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceToSendTheDailyInventoriesToTrueSteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.StepDefinitions.Api
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Utils;
    using global::Bdd.Core.Utils;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class ServiceToSendTheDailyInventoriesToTrueSteps : EcpApiStepDefinitionBase
    {
        private const int MillisecondsDelayForRequestProcessing = 120000;

        public ServiceToSendTheDailyInventoriesToTrueSteps(FeatureContext featureContext)
    : base(featureContext)
        {
        }

        [Given(@"I have data to register ""(.*)"" in system")]
        public async Task GivenIHaveDataToRegisterInSystemAsync(string entity)
        {
            await this.GivenIHaveDataToRegisterInSystemWithoutHomologationAsync(entity).ConfigureAwait(false);
        }

        [When(@"I have (.*) inventory with inventoryId attribute is (.*)")]
        public void WhenIHaveInventoryWithInventoryIdAttributeIs(int inventoriesCount, int lengthOfField)
        {
            this.SetValue(ConstantValues.TestData, "InventoryIdentifierIsSet");
            this.CommonMethodForInventoryRegistration(inventoriesCount, lengthOfField);
        }

        [Then(@"(.*) inventory should be registered in system")]
        public async Task ThenInventoryShouldBeRegisteredInSystemAsync(int numberOfInventories)
        {
            var actualNumberOfInventories = await this.ReadSqlScalarAsync<int>(input: SqlQueries.GetCountOfInventoriesByInventoryId, args: new { inventoryId = this.GetValue(ConstantValues.InventoryId) }).ConfigureAwait(false);
            var i = MillisecondsDelayForRequestProcessing / 30000;
            while (i > 0)
            {
                if (actualNumberOfInventories == numberOfInventories)
                {
                    break;
                }
                else
                {
                    await Task.Delay(30000).ConfigureAwait(true);
                    actualNumberOfInventories = await this.ReadSqlScalarAsync<int>(input: SqlQueries.GetCountOfInventoriesByInventoryId, args: new { inventoryId = this.GetValue(ConstantValues.InventoryId) }).ConfigureAwait(false);
                }

                i--;
            }

            Assert.AreEqual(numberOfInventories, actualNumberOfInventories);
        }

        [Then(@"(.*) inventories should be registered in system")]
        public async Task ThenInventoriesShouldBeRegisteredInSystemAsync(int numberOfInventories)
        {
            var actualNumberOfInventories = await this.ReadSqlScalarAsync<int>(input: SqlQueries.GetInventoryProductByBatchId, args: new { batchId = this.GetValue(ConstantValues.BATCHID) }).ConfigureAwait(false);
            var i = MillisecondsDelayForRequestProcessing / 30000;
            while (i > 0)
            {
                if (actualNumberOfInventories == numberOfInventories)
                {
                    break;
                }
                else
                {
                    await Task.Delay(30000).ConfigureAwait(true);
                    actualNumberOfInventories = await this.ReadSqlScalarAsync<int>(input: SqlQueries.GetInventoryProductByBatchId, args: new { batchId = this.GetValue(ConstantValues.BATCHID) }).ConfigureAwait(false);
                }

                i--;
            }

            Assert.AreEqual(numberOfInventories, actualNumberOfInventories);
        }

        [When(@"I have (.*) inventory without BatchId, Version, grossStandardQuantity, ProductType attributes")]
        [When(@"I have (.*) inventory when optional attributes are not provided")]
        public void WhenIHaveInventoryWithoutBatchIdVersionGrossStandardQuantityProductTypeAttributes(int inventoriesCount)
        {
            this.SetValue(ConstantValues.TestData, "WithoutOptionalFields");
            this.CommonMethodForInventoryRegistration(inventoriesCount);
        }

        [When(@"I have not provided ""(.*)"" attribute in the request")]
        public void WhenIHaveNotProvidedAttributeInTheRequest(string field)
        {
            this.SetValue(ConstantValues.TestData, "WithoutMandatoryFields");
            this.CommonMethodForInventoryRegistration(attribute: field);
        }

        [When(@"I have provided more than (.*) of ""(.*)"" that accepts")]
        public void WhenIHaveProvidedMoreThanOfThatAccepts(int length, string field)
        {
            this.SetValue(ConstantValues.TestData, "FieldWithMoreThanLengthThatAccepts");
            this.CommonMethodForInventoryRegistration(lengthOfField: length, attribute: field);
        }

        [When(@"I have (.*) inventory with scenarioId attribute")]
        [When(@"I have (.*) inventories with all mandatory attributes")]
        [When(@"I have (.*) inventory with all mandatory and optional attributes are homologated")]
        [When(@"I have (.*) inventory when shouldHomologate parameter is enabled")]
        [When(@"I have (.*) inventory when request is provide with all required attributes")]
        [When(@"I have (.*) inventory when request is having direct mapping between registry and canonical attributes")]
        [When(@"I have (.*) inventory with event type is insert")]
        public void WhenIHaveInventoryWithScenarioIdAttribute(int inventoriesCount)
        {
            this.SetValue(ConstantValues.TestData, "WithScenarioId");
            this.SetValue(ConstantValues.BATCHID, new Faker().Random.AlphaNumeric(10).ToString(CultureInfo.InvariantCulture));
            this.CommonMethodForInventoryRegistration(inventoriesCount);
        }

        [When(@"I have (.*) inventory without scenarioId attribute")]
        public void WhenIHaveInventoryWithoutScenarioIdAttribute(int inventoriesCount)
        {
            this.SetValue(ConstantValues.TestData, "WithOutScenarioId");
            this.CommonMethodForInventoryRegistration(inventoriesCount);
        }

        [When(@"I have (.*) inventory with valid format for BatchId, Version, grossStandardQuantity, system, scenarioId attributes")]
        public void WhenIHaveInventoryWithValidFormatForBatchIdVersionGrossStandardQuantitySystemScenarioIdAttributes(int inventoriesCount)
        {
            this.SetValue(ConstantValues.TestData, "WithValidFormatForFields");
            this.SetValue(ConstantValues.BATCHID, new Faker().Random.AlphaNumeric(10).ToString(CultureInfo.InvariantCulture));
            this.SetValue(ConstantValues.Version, new Faker().Random.AlphaNumeric(10).ToString(CultureInfo.InvariantCulture));
            this.SetValue(ConstantValues.GrossStandardQuantity, 1000);
            this.SetValue(ConstantValues.System, "Automation_System");
            this.CommonMethodForInventoryRegistration(inventoriesCount);
        }

        [When(@"I have provided invalid format for ""(.*)"" attribute")]
        public void WhenIHaveProvidedInvalidFormatForAttribute(string field)
        {
            switch (field)
            {
                case "batchId":
                    this.SetValue(ConstantValues.TestData, "InvalidFormatFor_" + field);
                    this.SetValue(ConstantValues.BATCHID, 0.01);
                    break;
                case "version":
                    this.SetValue(ConstantValues.TestData, "InvalidFormatFor_" + field);
                    this.SetValue(ConstantValues.Version, 0.01);
                    break;
                case "grossStandardQuantity":
                    this.SetValue(ConstantValues.TestData, "InvalidFormatFor_" + field);
                    this.SetValue(ConstantValues.GrossStandardQuantity, "GrossStandardQuantity");
                    break;
                case "system":
                    this.SetValue(ConstantValues.TestData, "InvalidFormatFor_" + field);
                    this.SetValue(ConstantValues.System, 0.01);
                    break;
                case "scenarioId":
                    this.SetValue(ConstantValues.TestData, "InvalidFormatFor_" + field);
                    this.SetValue(ConstantValues.ScenarioId, 0.01);
                    break;
                default:
                    this.CommonMethodForInventoryRegistration(1);
                    break;
            }
        }

        [When(@"I have (.*) inventory with ""(.*)"" is provided in the request")]
        public void WhenIHaveInventoryWithIsProvidedInTheRequest(int inventoriesCount, string property)
        {
            this.SetValue(ConstantValues.TestData, "SendingOldAttributesInsteadOfRenamedAttributes_" + property);
            this.CommonMethodForInventoryRegistration(inventoriesCount, attribute: property);
        }

        [When(@"I have (.*) inventory with scenarioId, uncertainty, segmentId, operatorId, netStandardQuantity attributes")]
        public void WhenIHaveInventoryWithScenarioIdUncertaintySegmentIdOperatorIdNetStandardQuantityAttributes(int inventoriesCount)
        {
            this.SetValue(ConstantValues.TestData, "RenamedAttributes");
            this.CommonMethodForInventoryRegistration(inventoriesCount);
        }

        [Then(@"(.*) inventory should be registered in system with columbian hours")]
        public async Task ThenInventoryShouldBeRegisteredInSystemWithColumbianHoursAsync(int numberOfInventories)
        {
            await this.ThenInventoryShouldBeRegisteredInSystemAsync(numberOfInventories).ConfigureAwait(false);
            var inventoryProductDetails = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetInventoryId, args: new { inventoryId = this.GetValue(ConstantValues.InventoryId) }).ConfigureAwait(false);
            var columbianTime = DateTime.UtcNow.AddHours(-5).ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);
            Assert.IsTrue(columbianTime.Split(' ')[1].Split(':')[0].ContainsIgnoreCase(inventoryProductDetails[ConstantValues.CreatedDate].Split(' ')[1].Split(':')[0]));
        }

        [When(@"I have (.*) inventory with event type is ""(.*)"" and scenarioId attribute as (.*)")]
        public async Task WhenIHaveInventoryWithEventTypeIsAndScenarioIdAttributeAsAsync(int inventoryCount, string eventType, int officialid)
        {
            this.SetValue(ConstantValues.TestData, "BasedOnEvent_INSERT");
            this.SetValue(ConstantValues.InventoryId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
            this.MethodForOfficialInventoryRegistration(inventoryCount, attribute: officialid);
            await this.WhenIRegisterInventoriesOrMovementsInSystemThroughSappoAsync(ConstantValues.Inventory).ConfigureAwait(false);
            this.SetValue(ConstantValues.TestData, "BasedOnEvent_" + eventType);
            await this.GivenIHaveDataToRegisterInSystemWithoutHomologationAsync(ConstantValues.Inventory).ConfigureAwait(false);
            this.MethodForOfficialInventoryRegistration(inventoryCount, attribute: officialid);
        }

        protected void MethodForOfficialInventoryRegistration(int inventoriesCount = 1, int attribute = 2)
        {
            JArray inventoryArray = new JArray();
            string content = this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)].ToString();
            for (int i = 0; i < inventoriesCount; i++)
            {
                switch (this.GetValue(ConstantValues.TestData))
                {
                    case "WithScenarioId":
                        this.SetValue(ConstantValues.InventoryId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonChangePropertyValue("scenarioId", attribute);
                        content = content.JsonChangePropertyValue("ProductId batchId", this.GetValue(ConstantValues.BATCHID));
                        break;
                    case "BasedOnEvent_INSERT":
                        content = content.JsonChangePropertyValue("scenarioId", attribute);
                        content = content.JsonChangePropertyValue("eventType", "INSERT");
                        break;
                    case "BasedOnEvent_UPDATE":
                        content = content.JsonChangePropertyValue("scenarioId", attribute);
                        content = content.JsonChangePropertyValue("eventType", "UPDATE");
                        break;
                    case "BasedOnEvent_DELETE":
                        content = content.JsonChangePropertyValue("scenarioId", attribute);
                        content = content.JsonChangePropertyValue("eventType", "DELETE");
                        break;
                    default:
                        break;
                }

                if (string.IsNullOrEmpty(this.GetValue(ConstantValues.FieldToCheckMandatory)))
                {
                    if (string.IsNullOrEmpty(this.GetValue(ConstantValues.FieldToCheckErrorMessage)))
                    {
                        content = content.JsonChangePropertyValue("inventoryId", this.GetValue(ConstantValues.InventoryId));
                    }
                    else
                    {
                        content = content.JsonChangePropertyValue("inventoryId", new Faker().Random.AlphaNumeric(21).ToString(CultureInfo.InvariantCulture));
                    }

                    content = content.JsonChangePropertyValue(ConstantValues.InventoryDate, DateTime.Now.AddDays(-32).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
                }

                if (this.GetValue(Entities.Keys.EntityType).ContainsIgnoreCase("Homologated"))
                {
                    content = content.JsonChangePropertyValue("nodeId", this.GetValue("NodeId_1"));
                    content = content.JsonChangePropertyValue("system", this.GetValue("SystemElementId"));
                    content = content.JsonChangePropertyValue("segmentId", this.GetValue("SegmentId"));
                    content = content.JsonChangePropertyValue("operatorId", this.GetValue("OperatorId"));
                    content = content.JsonChangePropertyValue("ProductId productId", "10000002318");
                    content = content.JsonChangePropertyValue("ProductId productType", this.GetValue("ProductTypeId"));
                    content = content.JsonChangePropertyValue("ProductId measurementUnit", 31);
                    content = content.JsonChangePropertyValue("Attribute attributeId", this.GetValue("AttributeId"));
                    content = content.JsonChangePropertyValue("Attribute valueAttributeUnit", this.GetValue("ValueAttributeUnitId"));
                    content = content.JsonChangePropertyValue("OwnerId ownerId", this.GetValue("Owner"));
                }

                inventoryArray.Add(JsonConvert.DeserializeObject<JObject>(content));
            }

            this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)] = inventoryArray;
        }

        [When(@"I have (.*) inventory with scenarioId attribute as (.*)")]
        protected void MethodForOfficialInventoryRegistrations(int inventoriesCount, int officialid)
        {
            this.SetValue(ConstantValues.TestData, "WithScenarioId");
            this.MethodForOfficialInventoryRegistration(inventoriesCount, attribute: officialid);
        }

        [When(@"I process ""(.*)"" request with event is ""(.*)""")]
        protected async Task WhenIProcessRequestWithEventIsAsync(string entityType, string eventType)
        {
            await this.GivenIHaveInventoryOrMovementDataToProcessInSystemAsync(entityType).ConfigureAwait(false);
            this.SetValue(ConstantValues.FieldToCheckErrorMessage, ConstantValues.Yes);
            this.SetValue(ConstantValues.TestData, "BasedOnEvent_" + eventType);
            this.SetValue(ConstantValues.InventoryId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
            this.MethodForOfficialInventoryRegistration(1);
            await this.WhenIRegisterInventoriesOrMovementsInSystemThroughSappoAsync(ConstantValues.Inventory).ConfigureAwait(false);
        }
    }
}