// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InformSapPoTheResultsOfTheProcessingOfMovementsOrInventoriesSteps.cs" company="Microsoft">
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
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class InformSapPoTheResultsOfTheProcessingOfMovementsOrInventoriesSteps : EcpApiStepDefinitionBase
    {
        private const int MillisecondsDelayForRequestProcessing = 30000;
        private const int MillisecondsDelayForBlobGeneration = 300000;

        public InformSapPoTheResultsOfTheProcessingOfMovementsOrInventoriesSteps(FeatureContext featureContext)
        : base(featureContext)
        {
        }

        [Given(@"that the TRUE system receives ""(.*)"" through the Web API exposed to SAP PO")]
        [Given(@"that the TRUE system receives a request through the ""(.*)"" Web APIs exposed for SAP PO")]
        [Given(@"that the TRUE system is processing the ""(.*)"" received through the Web API exposed to SAP PO")]
        public async Task GivenThatTheTRUESystemReceivesARequestThroughTheWebAPIsExposedForSAPPOAsync(string entityType)
        {
            await this.TrueSystemReceivesARequestThroughTheWebAPIsExposedForSAPPOAsync(entityType).ConfigureAwait(false);
        }

        [Given(@"that the TRUE system is processing (.*) ""(.*)"" received through the Web API exposed to SAP PO")]
        public async Task GivenThatTheTRUESystemIsProcessingReceivedThroughTheWebAPIExposedToSAPPOAsync(int numberOfRecords, string entityType)
        {
            await this.TrueSystemReceivesARequestThroughTheWebAPIsExposedForSAPPOAsync(entityType, numberOfRecords).ConfigureAwait(false);
        }

        [When(@"TRUE system generates the process identifier")]
        public void WhenTRUESystemGeneratesTheProcessIdentifier()
        {
            Assert.IsNotNull(this.GetValue<dynamic>(Entities.Keys.Results));
        }

        [Then(@"store the process identifier")]
        public void ThenStoreTheProcessIdentifier()
        {
            // Method intentionally left empty. Step definition already taken care in ThenTheDateTimeOfReceiptOfTheInformation()
        }

        [Then(@"the date time of receipt of the information")]
        public async Task ThenTheDateTimeOfReceiptOfTheInformationAsync()
        {
            Assert.IsNotNull(await this.ReadSqlAsDictionaryAsync(SqlQueries.GetRegistrationDetailsWithProcessIdentifier, args: new { processIdentifier = this.ScenarioContext[ConstantValues.MessageId] }).ConfigureAwait(false));
        }

        [When(@"TRUE system successfully stores ""(.*)"" information")]
        public async Task WhenTRUESystemSuccessfullyStoresInformationAsync(string entityType)
        {
            await Task.Delay(MillisecondsDelayForRequestProcessing).ConfigureAwait(true);
            if (entityType.ContainsIgnoreCase("Inventories"))
            {
                var inventoryDetails = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetInventoryId, args: new { inventoryId = this.GetValue(ConstantValues.InventoryId) }).ConfigureAwait(false);
                this.SetValue(ConstantValues.FileRegistrationTransactionId, inventoryDetails[ConstantValues.FileRegistrationTransactionId].ToString());
                Assert.IsNotNull(inventoryDetails);
            }
            else
            {
                var movementDetails = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetMovementId, args: new { movementId = this.GetValue(ConstantValues.MovementId) }).ConfigureAwait(false);
                this.SetValue(ConstantValues.FileRegistrationTransactionId, movementDetails[ConstantValues.FileRegistrationTransactionId]);
                Assert.IsNotNull(movementDetails);
            }
        }

        [Then(@"each movement must have assigned the process identifier returned to SAP PO")]
        [Then(@"each inventory must have assigned the process identifier returned to SAP PO")]
        public void ThenEachInventoryMustHaveAssignedTheProcessIdentifierReturnedToSAPPO()
        {
            Assert.IsNotNull(this.ScenarioContext[ConstantValues.FileRegistrationTransactionId]);
        }

        [Given(@"that the TRUE system receives ""(.*)"" with errors through the Web API exposed to SAP PO")]
        public async Task GivenThatTheTRUESystemReceivesWithErrosThroughTheWebAPIExposedToSAPPOAsync(string entityType)
        {
            await this.GivenIHaveDataToRegisterInSystemWithoutHomologationAsync(entityType).ConfigureAwait(false);

            this.SetValue(ConstantValues.TestData, "WithScenarioId");
            this.SetValue(ConstantValues.BATCHID, new Faker().Random.AlphaNumeric(10).ToString(CultureInfo.InvariantCulture));

            if (entityType.EqualsIgnoreCase("Inventories"))
            {
                this.CommonMethodForInventoryRegistration(1);
            }
            else
            {
                this.CommonMethodForMovementRegistration(1);
            }

            await this.WhenIRegisterInventoriesOrMovementsInSystemThroughSappoAsync(entityType).ConfigureAwait(false);
        }

        [When(@"errors are generated during inventories validations")]
        [When(@"errors are generated during movements validations")]
        [When(@"the error description does not contains the error code")]
        public void WhenErrorsAreGeneratedDuringInventoriesValidations()
        {
            // Method intentionally left empty. Step definition already taken care in ThenEachMessageInThePendingTransactionLogMustHaveAssignedTheProcessIdentifierReturnedToSAPPOAsync()
        }

        [When(@"the TRUE system finishes the ""(.*)"" processing of a process identifier")]
        public async Task WhenTheTRUESystemFinishesTheProcessingOfAProcessIdentifierAsync(string entityType)
        {
            await Task.Delay(MillisecondsDelayForRequestProcessing).ConfigureAwait(true);
            await this.WhenTRUESystemSuccessfullyStoresInformationAsync(entityType).ConfigureAwait(false);

            // waiting for 6 minutes as timer set for 5 min in DEV code
            await Task.Delay(MillisecondsDelayForBlobGeneration).ConfigureAwait(true);

            var blob = this.ScenarioContext[ConstantValues.MessageId].ToString();
            this.ScenarioContext[ConstantValues.ProcessResultJson] = await blob.DownloadSapProcessResultBlobDataAsync().ConfigureAwait(false);
        }

        [When(@"the TRUE system finishes the inventories processing of a process identifier with error")]
        [When(@"the TRUE system finishes the movements processing of a process identifier with error")]
        public async Task WhenTheTRUESystemFinishesTheProcessingOfAProcessIdentifierWithErrorAsync()
        {
            await Task.Delay(MillisecondsDelayForRequestProcessing).ConfigureAwait(true);
            await this.ThenEachMessageInThePendingTransactionLogMustHaveAssignedTheProcessIdentifierReturnedToSAPPOAsync().ConfigureAwait(false);

            // waiting for 6 minutes as timer set for 5 min in DEV code
            await Task.Delay(MillisecondsDelayForBlobGeneration).ConfigureAwait(true);

            var blob = this.ScenarioContext[ConstantValues.MessageId].ToString();
            this.ScenarioContext[ConstantValues.ProcessResultJson] = await blob.DownloadSapProcessResultBlobDataAsync().ConfigureAwait(false);
        }

        [Then(@"""(.*)"" field should have inventory identifier generated by TRUE")]
        [Then(@"""(.*)"" field should have movement identifier generated by TRUE")]
        public void ThenFieldShouldHaveIdentifierGeneratedByTRUE(string transactionId)
        {
            var processResultJson = JObject.Parse(this.ScenarioContext[ConstantValues.ProcessResultJson].ToString());
            var transactionIdentifier = processResultJson["documents"][0][transactionId].ToString();
            Assert.AreEqual(this.GetValue(ConstantValues.FileRegistrationTransactionId), transactionIdentifier);
        }

        [Then(@"""(.*)"" field should have empty")]
        public void ThenFieldShouldHaveEmpty(string transactionId)
        {
            var processResultJson = JObject.Parse(this.ScenarioContext[ConstantValues.ProcessResultJson].ToString());
            var transactionIdentifier = processResultJson["documents"][0][transactionId].ToString();
            Assert.IsEmpty(transactionIdentifier);
        }

        [Then(@"""(.*)"" collection should have two objects with required details")]
        [Then(@"""(.*)"" collection should have two objects with errorCode and errorDsc")]
        public void ThenCollectionShouldHaveTwoObjectsWithRequiredDetails(string collectionName)
        {
            var processResultJson = JObject.Parse(this.ScenarioContext[ConstantValues.ProcessResultJson].ToString());
            Assert.AreEqual(2, processResultJson[collectionName].Count());
        }

        [Then(@"""(.*)"" field should have ""(.*)""")]
        public void ThenFieldShouldHave(string errorCode, string expectedValue)
        {
            var processResultJson = JObject.Parse(this.ScenarioContext[ConstantValues.ProcessResultJson].ToString());
            var actualValue = processResultJson["documents"][0]["errors"][0][errorCode].ToString();
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Then(@"generate a generic code and it should be used to report the error code to SAP PO")]
        public void ThenGenerateAGenericCodeAndItShouldBeUsedToReportTheErrorCodeToSAPPO()
        {
            var processResultJson = JObject.Parse(this.ScenarioContext[ConstantValues.ProcessResultJson].ToString());
            var actualValue = processResultJson["documents"][0]["errors"][0]["errorCode"].ToString();
            Assert.AreEqual("9999", actualValue);
        }

        [Then(@"result report should send ""(.*)"" as process identifier")]
        public void ThenResultReportShouldSendAsProcessIdentifier(string processId)
        {
            var processResultJson = JObject.Parse(this.ScenarioContext[ConstantValues.ProcessResultJson].ToString());
            var processIdentifier = processResultJson[processId].ToString();
            Assert.AreEqual(this.ScenarioContext[ConstantValues.MessageId].ToString(), processIdentifier.ToString());
        }

        [Then(@"""(.*)"" field should have ""(.*)"" identifier")]
        public void ThenFieldShouldHaveIdentifier(string documentId, string entityType)
        {
            var processResultJson = JObject.Parse(this.ScenarioContext[ConstantValues.ProcessResultJson].ToString());
            var documentIdentifier = processResultJson["documents"][0][documentId].ToString();
            if (entityType == "inventory")
            {
                Assert.AreEqual(this.GetValue(ConstantValues.InventoryId), documentIdentifier);
            }
            else
            {
                Assert.AreEqual(this.GetValue(ConstantValues.MovementId), documentIdentifier);
            }
        }

        [Then(@"""(.*)"" field should have code generated by TRUE")]
        public async Task ThenFieldShouldHaveCodeGeneratedByTRUEAsync(string errorCode)
        {
            var processResultJson = JObject.Parse(this.ScenarioContext[ConstantValues.ProcessResultJson].ToString());
            var errorCodeIdentifier = processResultJson["documents"][0]["errors"][0][errorCode].ToString();
            var errorDetails = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetSapProcessResultDetails, args: new { uploadId = this.ScenarioContext[ConstantValues.MessageId] }).ConfigureAwait(false);
            this.SetValue(ConstantValues.ErrorMessage, errorDetails[ConstantValues.ErrorMessage]);
            Assert.AreEqual(errorDetails[ConstantValues.ErrorCode], errorCodeIdentifier);
        }

        [Then(@"""(.*)"" field should have error registered by TRUE")]
        public void ThenFieldShouldHaveErrorRegisteredByTRUE(string errorDsc)
        {
            var processResultJson = JObject.Parse(this.ScenarioContext[ConstantValues.ProcessResultJson].ToString());
            var errorDscIdentifier = processResultJson["documents"][0]["errors"][0][errorDsc].ToString();
            Assert.AreEqual(this.GetValue(ConstantValues.ErrorMessage), errorDscIdentifier);
        }

        [Then(@"each message in the pending transaction log must have assigned the process identifier returned to SAP PO")]
        public async Task ThenEachMessageInThePendingTransactionLogMustHaveAssignedTheProcessIdentifierReturnedToSAPPOAsync()
        {
            await Task.Delay(MillisecondsDelayForRequestProcessing).ConfigureAwait(true);
            var pendingTransactionDetails = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetPendingTransactionDetailsWithProcessIdentifier, args: new { processIdentifier = this.ScenarioContext[ConstantValues.MessageId] }).ConfigureAwait(false);
            Assert.IsNotNull(pendingTransactionDetails[ConstantValues.ErrorJson]);
        }

        protected async Task TrueSystemReceivesARequestThroughTheWebAPIsExposedForSAPPOAsync(string entityType, int registrationCount = 1)
        {
            await this.GivenIHaveInventoryOrMovementDataToProcessInSystemAsync(entityType).ConfigureAwait(false);

            this.SetValue(ConstantValues.TestData, "WithScenarioId");
            this.SetValue(ConstantValues.BATCHID, new Faker().Random.AlphaNumeric(10).ToString(CultureInfo.InvariantCulture));

            if (entityType.EqualsIgnoreCase("Inventories"))
            {
                this.CommonMethodForInventoryRegistration(registrationCount);
            }
            else
            {
                this.CommonMethodForMovementRegistration(registrationCount);
            }

            await this.WhenIRegisterInventoriesOrMovementsInSystemThroughSappoAsync(entityType).ConfigureAwait(false);
        }
    }
}