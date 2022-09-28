// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceToSendTheDailyMovementsToTrueSteps.cs" company="Microsoft">
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
    public class ServiceToSendTheDailyMovementsToTrueSteps : EcpApiStepDefinitionBase
    {
        private const int MillisecondsDelayForRequestProcessing = 120000;

        public ServiceToSendTheDailyMovementsToTrueSteps(FeatureContext featureContext)
        : base(featureContext)
        {
        }

        [Then(@"(.*) movement should be registered in system")]
        public async Task ThenMovementShouldBeRegisteredInSystemAsync(int numberOfMovements)
        {
            var actualNumberOfMovements = await this.ReadSqlScalarAsync<int>(input: SqlQueries.GetCountOfMovementsByMovementId, args: new { MovementId = this.GetValue(ConstantValues.MovementId) }).ConfigureAwait(false);
            var i = MillisecondsDelayForRequestProcessing / 30000;
            while (i > 0)
            {
                if (actualNumberOfMovements == numberOfMovements)
                {
                    break;
                }
                else
                {
                    await Task.Delay(30000).ConfigureAwait(true);
                    actualNumberOfMovements = await this.ReadSqlScalarAsync<int>(input: SqlQueries.GetCountOfMovementsByMovementId, args: new { movementId = this.GetValue(ConstantValues.MovementId) }).ConfigureAwait(false);
                }

                i--;
            }

            Assert.AreEqual(numberOfMovements, actualNumberOfMovements);
        }

        [When(@"I have (.*) movement without ""(.*)"" attributes of backupMovement")]
        [When(@"I have (.*) movement with value for ""(.*)""")]
        public void WhenIHaveMovementWithoutAttributesOfBackupMovement(int movementsCount, string property)
        {
            this.SetValue(ConstantValues.TestData, "AttributesOfBackUpMovement");
            this.CommonMethodForMovementRegistration(movementsCount, attribute: property);
        }

        [When(@"I have (.*) movement without value for ""(.*)"" attributes")]
        public void WhenIHaveMovementWithoutValueForAttributes(int movementsCount, string property)
        {
            this.SetValue(ConstantValues.TestData, "OptionalAttributes");
            this.CommonMethodForMovementRegistration(movementsCount, attribute: property);
        }

        [When(@"I have (.*) movement with scenarioId attribute")]
        [When(@"I have (.*) movement with all mandatory attributes")]
        [When(@"I have (.*) movement with all mandatory and optional attributes are homologated")]
        [When(@"I have (.*) movement when shouldHomologate parameter is enabled")]
        [When(@"I have (.*) movement when request is provide with all required attributes")]
        [When(@"I have (.*) movement when request is having direct mapping between registry and canonical attributes")]
        public void WhenIHaveMovementWithScenarioIdAttribute(int movementsCount)
        {
            this.SetValue(ConstantValues.TestData, "WithScenarioId");
            this.SetValue(ConstantValues.BATCHID, new Faker().Random.AlphaNumeric(10).ToString(CultureInfo.InvariantCulture));
            this.CommonMethodForMovementRegistration(movementsCount);
        }

        [When(@"I have not provided ""(.*)"" attribute in the movement request")]
        public void WhenIHaveNotProvidedAttributeInTheMovementRequest(string field)
        {
            this.SetValue(ConstantValues.TestData, "MovementWithoutMandatoryFields");
            this.CommonMethodForMovementRegistration(attribute: field);
        }

        [When(@"I have provided more than (.*) of ""(.*)"" that accepts in movement request")]
        public void WhenIHaveProvidedMoreThanOfThatAcceptsInMovementRequest(int length, string field)
        {
            this.SetValue(ConstantValues.TestData, "FieldWithMoreThanLengthThatAccepts");
            this.CommonMethodForMovementRegistration(lengthOfField: length, attribute: field);
        }

        [When(@"I have (.*) movement with both movement source and destination attributes")]
        public void WhenIHaveMovementWithBothMovementSourceAndDestinationAttributes(int movementsCount)
        {
            this.SetValue(ConstantValues.TestData, "MovementWithSourceAndDestination");
            this.CommonMethodForMovementRegistration(movementsCount);
        }

        [When(@"I have (.*) movement without movement source and movement destination attributes in the request")]
        public void WhenIHaveMovementWithoutMovementSourceAndMovementDestinationAttributesInTheRequest(int movementsCount)
        {
            this.SetValue(ConstantValues.TestData, "MovementWithoutSourceAndDestination");
            this.CommonMethodForMovementRegistration(movementsCount);
        }

        [When(@"I have (.*) movement with movement source attribute")]
        public void WhenIHaveMovementWithMovementSourceAttribute(int movementsCount)
        {
            this.SetValue(ConstantValues.TestData, "MovementWithSource");
            this.CommonMethodForMovementRegistration(movementsCount);
        }

        [When(@"I have (.*) movement with movement destination attribute")]
        public void WhenIHaveMovementWithMovementDestinationAttribute(int movementsCount)
        {
            this.SetValue(ConstantValues.TestData, "MovementWithDestination");
            this.CommonMethodForMovementRegistration(movementsCount);
        }

        [Then(@"(.*) movement should be registered in system with columbian hours")]
        public async Task ThenInventoryShouldBeRegisteredInSystemWithColumbianHoursAsync(int numberOfInventories)
        {
            await this.ThenMovementShouldBeRegisteredInSystemAsync(numberOfInventories).ConfigureAwait(false);
            var inventoryProductDetails = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetMovementId, args: new { movementId = this.GetValue(ConstantValues.MovementId) }).ConfigureAwait(false);
            var columbianTime = DateTime.UtcNow.AddHours(-5).ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);
            Assert.IsTrue(columbianTime.Split(' ')[1].Split(':')[0].ContainsIgnoreCase(inventoryProductDetails[ConstantValues.CreatedDate].Split(' ')[1].Split(':')[0]));
        }

        [When(@"I have (.*) movement with isOfficial and globalMovementId and backupMovementId under OfficialInformation")]
        [When(@"I have (.*) movement with official information of all internal mandatory attributes")]
        public void WhenIHaveMovementWithGlobalMovementIdAndBackupMovementIdUnderOfficialInformation(int movementsCount)
        {
            this.SetValue(ConstantValues.TestData, "WithOfficialInformation");
            this.CommonMethodForMovementRegistration(movementsCount);
        }

        [When(@"I have (.*) movement without official information attribute")]
        public void WhenIHaveMovementWithoutOfficialInformationAttribute(int movementsCount)
        {
            this.SetValue(ConstantValues.TestData, "WithoutOfficialInformation");
            this.CommonMethodForMovementRegistration(movementsCount);
        }

        [Then(@"(.*) movements should be registered in system")]
        public async Task ThenMovementsShouldBeRegisteredInSystemAsync(int numberOfMovements)
        {
            var actualNumberOfMovements = await this.ReadSqlScalarAsync<int>(input: SqlQueries.GetMovementByBatchId, args: new { batchId = this.GetValue(ConstantValues.BATCHID) }).ConfigureAwait(false);
            var i = MillisecondsDelayForRequestProcessing / 30000;
            while (i > 0)
            {
                if (actualNumberOfMovements == numberOfMovements)
                {
                    break;
                }
                else
                {
                    await Task.Delay(30000).ConfigureAwait(true);
                    actualNumberOfMovements = await this.ReadSqlScalarAsync<int>(input: SqlQueries.GetMovementByBatchId, args: new { batchId = this.GetValue(ConstantValues.BATCHID) }).ConfigureAwait(false);
                }

                i--;
            }

            Assert.AreEqual(numberOfMovements, actualNumberOfMovements);
        }

        [When(@"I have (.*) movement with event type is ""(.*)"" and scenarioId attribute as (.*)")]
        public async System.Threading.Tasks.Task WhenIHaveMovementWithEventTypeIsAndScenarioIdAttributeAsAsync(int movementsCount, string eventType, int officialid)
        {
            this.SetValue(ConstantValues.TestData, "BasedOnEvent_INSERT");
            this.SetValue(ConstantValues.MovementId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
            this.MethodForOfficialMovementRegistration(movementsCount, attribute: officialid);
            await this.WhenIRegisterInventoriesOrMovementsInSystemThroughSappoAsync(ConstantValues.Movement).ConfigureAwait(false);
            await Task.Delay(5000).ConfigureAwait(true);
            this.SetValue(ConstantValues.TestData, "BasedOnEvent_" + eventType);
            await this.GivenIHaveDataToRegisterInSystemWithoutHomologationAsync(ConstantValues.Movement).ConfigureAwait(false);
            this.MethodForOfficialMovementRegistration(movementsCount, attribute: officialid);
        }

        [When(@"I register ""(.*)"" in system with invalid token")]
        public async Task WhenIRegisterInSystemWithInvalidTokenAsync(string entity)
        {
            await this.IRegisterInventoriesOrMovementsInSystemThroughSappoAsync(entity, invalidToken: "yes").ConfigureAwait(false);
        }

        [When(@"I have inserted both operative and official movements to verify whether SAP Po got required information using Sap system")]
        public async Task WhenIHaveInsertedBothOperativeAndOfficialMovementsToVerifyWhetherSAPPoGotRequiredInformationUsingSapSystemAsync()
        {
            //// to set nodes from different segments
            this.SetValue("TransferPointNodes", "True");
            //// Creating testdata for SAP
            await this.GivenIHaveInventoryOrMovementDataToProcessInSystemAsync("Movements").ConfigureAwait(false);
            //// Inserting 2 movements
            this.WhenIHaveMovementWithScenarioIdAttribute(2);
            //// Registering movements
            await this.WhenIRegisterInventoriesOrMovementsInSystemThroughSappoAsync("Movements").ConfigureAwait(false);
        }

        [When(@"I have ""(.*)"" both sap inserted operative and official movements to verify whether SAP Po got required information")]
        public async Task WhenIHaveBothSapInsertedOperativeAndOfficialMovementsToVerifyWhetherSAPPoGotRequiredInformationAsync(string action)
        {
            if (action.EqualsIgnoreCase("updated"))
            {
                this.SetValue("SapPoAcknowledgement", "UPDATE");
            }
            else
            {
                this.SetValue("SapPoAcknowledgement", "DELETE");
            }

            this.SapAcknowledgementMovementRegistration();
            //// Registering movements
            await this.WhenIRegisterInventoriesOrMovementsInSystemThroughSappoAsync("Movements").ConfigureAwait(false);
        }

        protected void MethodForOfficialMovementRegistration(int movementsCount = 1, int attribute = 2)
        {
            JArray movementArray = new JArray();
            string content = this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)].ToString();
            for (int i = 0; i < movementsCount; i++)
            {
                switch (this.GetValue(ConstantValues.TestData))
                {
                    case "WithOfficialScenarioId":
                        this.SetValue(ConstantValues.MovementId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonChangePropertyValue("scenarioId", attribute);
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

                content = content.JsonChangePropertyValue("movementId", this.GetValue(ConstantValues.MovementId));
                content = content.JsonChangePropertyValue(ConstantValues.OperationalDate, DateTime.Now.AddDays(-32).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
                content = content.JsonChangePropertyValue(ConstantValues.PeriodStartDate, DateTime.Now.AddDays(-34).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
                content = content.JsonChangePropertyValue(ConstantValues.PeriodEndDate, DateTime.Now.AddDays(-33).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
                if (this.GetValue(Entities.Keys.EntityType).ContainsIgnoreCase("Homologated"))
                {
                    content = content.JsonChangePropertyValue(ConstantValues.SourceNodeId, this.GetValue("NodeId_2"));
                    content = content.JsonChangePropertyValue(ConstantValues.DestinationNodeId, this.GetValue("NodeId_1"));
                    content = content.JsonChangePropertyValue(ConstantValues.SourceProductTypeId, this.GetValue("SourceProductTypeId"));
                    content = content.JsonChangePropertyValue(ConstantValues.DestinationProductTypeid, this.GetValue("DestinationProductTypeId"));
                    content = content.JsonChangePropertyValue("Segment", this.GetValue("SegmentId"));
                }

                movementArray.Add(JsonConvert.DeserializeObject<JObject>(content));
            }

            this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)] = movementArray;
        }

        [When(@"I have (.*) movement with scenarioId attribute as (.*)")]
        protected void WhenIHaveMovementWithScenarioIdAttributeAs(int movementsCount, int officialid)
        {
            this.SetValue(ConstantValues.TestData, "WithOfficialScenarioId");
            this.MethodForOfficialMovementRegistration(movementsCount, attribute: officialid);
        }

        [When(@"I process ""(.*)"" request with event type is ""(.*)""")]
        protected async Task WhenIProcessRequestWithEventTypeIsAsync(string entityType, string eventType)
        {
            await this.GivenIHaveInventoryOrMovementDataToProcessInSystemAsync(entityType).ConfigureAwait(false);
            this.SetValue(ConstantValues.FieldToCheckErrorMessage, ConstantValues.Yes);
            this.SetValue(ConstantValues.TestData, "BasedOnEvent_" + eventType);
            this.SetValue(ConstantValues.MovementId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
            this.MethodForOfficialMovementRegistration(1);
            await this.WhenIRegisterInventoriesOrMovementsInSystemThroughSappoAsync(ConstantValues.Movement).ConfigureAwait(false);
        }
    }
}