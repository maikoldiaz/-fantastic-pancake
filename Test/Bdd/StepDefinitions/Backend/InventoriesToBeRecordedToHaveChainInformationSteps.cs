// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoriesToBeRecordedToHaveChainInformationSteps.cs" company="Microsoft">
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
    using System.Globalization;
    using System.Threading.Tasks;

    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.StepDefinitions.Api;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class InventoriesToBeRecordedToHaveChainInformationSteps : EcpApiStepDefinitionBase
    {
        private const int MillisecondsDelayForRequestProcessing = 90000;

        public InventoriesToBeRecordedToHaveChainInformationSteps(FeatureContext featureContext)
        : base(featureContext)
        {
        }

        [When(@"I have scenario table in the database")]
        [Given(@"I have records in inventories table")]
        public void WhenIHaveScenarioTableInTheDatabase()
        {
            // Method intentionally left empty.
        }

        [When(@"""(.*)"" does not exists in the system")]
        public void WhenDoesNotExistsInTheSystem(string entityType)
        {
            Assert.IsNotNull(entityType);

            // Method intentionally left empty.
        }

        [Then(@"scenarioId and name details should present in the database")]
        public async Task ThenScenarioIdAndNameDetailsShouldPresentInTheDatabaseAsync()
        {
            var scenarioDetails = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetScenarioTypeDetails).ConfigureAwait(false);
            Assert.AreEqual(3, scenarioDetails.ToDictionaryList().Count);
            for (int i = 0; i < scenarioDetails.ToDictionaryList().Count; i++)
            {
                Assert.IsTrue(scenarioDetails.ToDictionaryList()[i]["ScenarioTypeId"].ToInt() == i + 1);
            }

            Assert.IsTrue(scenarioDetails.ToDictionaryList()[0]["Name"].ToString() == "Operativo");
            Assert.IsTrue(scenarioDetails.ToDictionaryList()[1]["Name"].ToString() == "Oficial");
            Assert.IsTrue(scenarioDetails.ToDictionaryList()[2]["Name"].ToString() == "Consolidado");
        }

        [Then(@"for all old records in Inventories scenarioId should be (.*)")]
        public async Task ThenForAllOldRecordsInScenarioIdShouldBeAsync(int value)
        {
            Assert.IsEmpty(await this.ReadAllSqlAsync(input: SqlQueries.GetInventoriesOtherthanOperativoScenarioForOldRecords, args: new { scenarioId = value }).ConfigureAwait(false));
        }

        [When(@"I have (.*) inventory with event type is ""(.*)""")]
        public async Task WhenIHaveInventoryWithEventTypeIsAsync(int inventoriesCount, string eventType)
        {
            this.SetValue(ConstantValues.TestData, "BasedOnEvent_INSERT");
            this.SetValue(ConstantValues.InventoryId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
            this.CommonMethodForInventoryRegistration(inventoriesCount);
            await this.WhenIRegisterInventoriesOrMovementsInSystemThroughSappoAsync(ConstantValues.Inventory).ConfigureAwait(false);
            this.SetValue(ConstantValues.TestData, "BasedOnEvent_" + eventType);
            await this.GivenIHaveDataToRegisterInSystemWithoutHomologationAsync(ConstantValues.Inventory).ConfigureAwait(false);
            this.CommonMethodForInventoryRegistration(inventoriesCount);
        }

        [Then(@"inventory should be updated in system")]
        public async Task ThenInventoryShouldBeUpdatedInSystemAsync()
        {
            await Task.Delay(MillisecondsDelayForRequestProcessing).ConfigureAwait(true);
            Assert.AreEqual(2, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfInventoriesByEventType, args: new { eventType = "UPDATE", inventoryId = this.GetValue(ConstantValues.InventoryId) }).ConfigureAwait(false));
        }

        [Then(@"inventory should be deleted in system")]
        public async Task ThenInventoryShouldBeDeletedInSystemAsync()
        {
            await Task.Delay(MillisecondsDelayForRequestProcessing).ConfigureAwait(true);
            Assert.AreEqual(1, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfInventoriesByEventType, args: new { eventType = "DELETE", inventoryId = this.GetValue(ConstantValues.InventoryId) }).ConfigureAwait(false));
        }

        [When(@"I processed ""(.*)"" request with event type is ""(.*)""")]
        public async Task WhenIProcessedRequestWithEventTypeIsAsync(string entityType, string eventType)
        {
            await this.IHaveInventoryOrMovementDataToProcessInSystemAsync(entityType).ConfigureAwait(false);
            this.SetValue(ConstantValues.FieldToCheckErrorMessage, ConstantValues.Yes);
            this.SetValue(ConstantValues.TestData, "BasedOnEvent_" + eventType);
            await this.GivenIHaveDataToRegisterInSystemWithoutHomologationAsync(ConstantValues.Inventory).ConfigureAwait(false);
            this.CommonMethodForInventoryRegistration(1);
            await this.WhenIRegisterInventoriesOrMovementsInSystemThroughSappoAsync(ConstantValues.Inventory).ConfigureAwait(false);
        }
    }
}