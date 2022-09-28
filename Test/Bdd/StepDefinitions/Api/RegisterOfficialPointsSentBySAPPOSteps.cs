// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterOfficialPointsSentBySAPPOSteps.cs" company="Microsoft">
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
    using System.Threading.Tasks;

    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Utils;

    using Flurl;

    using global::Bdd.Core.Utils;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class RegisterOfficialPointsSentBySappoSteps : EcpApiStepDefinitionBase
    {
        public RegisterOfficialPointsSentBySappoSteps(FeatureContext featureContext)
    : base(featureContext)
        {
        }

        [When(@"the ""(.*)"" is not registered in the TRUE application and all the other validations are met")]
        public async Task WhenTheIsNotRegisteredInTheTRUEApplicationAndAllTheOtherValidationsAreMetAsync(string type)
        {
            if (type.EqualsIgnoreCase("official movement"))
            {
                await this.RegisterOfficialPointAsync("OfficialMovementNotRegistered").ConfigureAwait(false);
            }
            else
            {
                await this.RegisterOfficialPointAsync("BackupMovementNotRegistered").ConfigureAwait(false);
            }
        }

        [Then(@"backup movement information with the global identifier should be stored")]
        public async Task ThenBackupMovementInformationWithTheGlobalIdentifierShouldBeStoredAsync()
        {
            var movementDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementDetailsByMovementId, args: new { MovementId = this.GetValue("BackupMovementId") }).ConfigureAwait(false);
            Assert.AreEqual("INSERT", movementDetails["EventType"].ToUpperInvariant());
            Assert.AreEqual(this.GetValue("GlobalMovementId"), movementDetails["GlobalMovementId"]);
        }

        [Then(@"errors should be registered in pending transaction log when there are any errors occured while storing the backup movement for ""(.*)""")]
        public async Task ThenErrorsShouldBeRegisteredInPendingTransactionLogWhenThereAreAnyErrorsOccuredWhileStoringTheBackupMovementAsync(string type)
        {
            var content = this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)].ToString();
            if (type == "registration")
            {
                this.SetValue("BackupMovementId", new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                content = content.JArrayModifyPropertyValue("Array_1 movementId", this.GetValue("BackupMovementId"));
                content = content.JArrayModifyPropertyValue("OfficialInformation_2 backupMovementId", this.GetValue("BackupMovementId"));
            }
            else if (type == "last event type delete")
            {
                await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateMovementEventType, args: new { MovementId = this.GetValue("BackupMovementId") }).ConfigureAwait(false);
            }
            else
            {
                content = content.JArrayModifyPropertyValue("Array_1 netStandardQuantity", "2470.00");
            }

            this.SetValue("GlobalMovementId", new Faker().Random.AlphaNumeric(10).ToString(CultureInfo.InvariantCulture));
            content = content.JArrayModifyPropertyValue("OfficialInformation_1 globalMovementId", this.GetValue("GlobalMovementId"));
            content = content.JArrayModifyPropertyValue("OfficialInformation_2 globalMovementId", this.GetValue("GlobalMovementId"));
            content = content.JArrayModifyPropertyValue("Array_1 measurementUnit", "bbl");
            await this.GivenIAmAuthenticatedForServiceAsync("sappoapi").ConfigureAwait(false);
            await this.SetResultsAsync(async () => await this.SapPostAsync<dynamic>(this.SapEndpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Official]), JArray.Parse(content.ToString())).ConfigureAwait(false)).ConfigureAwait(false);
            var movementDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementDetailsByMovementId, args: new { MovementId = this.GetValue("BackupMovementId") }).ConfigureAwait(false);
            if (type == "registration")
            {
                Assert.IsNull(movementDetails);
            }
            else if (type == "last event type delete")
            {
                Assert.AreEqual("DELETE", movementDetails["EventType"].ToUpperInvariant());
                Assert.AreNotEqual(this.GetValue("GlobalMovementId"), movementDetails["GlobalMovementId"]);
            }
            else
            {
                Assert.AreEqual("2460.00", movementDetails["NetStandardVolume"]);
                Assert.AreNotEqual(this.GetValue("GlobalMovementId"), movementDetails["GlobalMovementId"]);
            }

            await Task.Delay(10000).ConfigureAwait(false);
            var pendingTransactionDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetTopPendingTransaction).ConfigureAwait(false);
            StringAssert.Contains("Debe ser de tipo: Unit", pendingTransactionDetails["ErrorJson"].ToString());
            this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)] = content;
        }

        [Then(@"processing of the official movement should happen")]
        public async Task ThenProcessingOfTheOfficialMovementShouldHappenAsync()
        {
            var content = this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)].ToString();
            var movementId = content.JarrayGetValue("Array_2 movementId");
            var movementDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementDetailsByMovementId, args: new { MovementId = movementId }).ConfigureAwait(false);
            Assert.AreEqual(this.GetValue("BackupMovementId"), movementDetails["BackupMovementId"]);
            Assert.AreEqual(this.GetValue("GlobalMovementId"), movementDetails["GlobalMovementId"]);
        }

        [When(@"the ""(.*)"" exists in TRUE")]
        public void WhenTheExistsInTRUE(string field)
        {
            Assert.IsNotNull(field);
            this.LogToReport("Covered in previous step!");
        }

        [When(@"all the validations are met and the last event type of the ""(.*)"" is a delete")]
        public async Task WhenAllTheValidationsAreMetAndTheLastEventTypeOfTheMovementIsADeleteAsync(string type)
        {
            if (type.EqualsIgnoreCase("backup movement"))
            {
                await this.RegisterOfficialPointAsync("BackupMovementAndLastEventTypeDelete").ConfigureAwait(false);
            }
            else
            {
                await this.RegisterOfficialPointAsync("OfficialMovementAndLastEventTypeDelete").ConfigureAwait(false);
            }
        }

        [When(@"all the validations are met and the net quantity of the last event does not match with the net quantity of the ""(.*)"" received")]
        public async Task WhenAllTheValidationsAreMetAndTheNetQuantityOfTheLastEventDoesNotMatchWithTheNetQuantityOfTheMovementReceivedAsync(string type)
        {
            if (type.EqualsIgnoreCase("backup movement"))
            {
                await this.RegisterOfficialPointAsync("BackupMovementIncorrectNetQuantity").ConfigureAwait(false);
            }
            else
            {
                await this.RegisterOfficialPointAsync("OfficialMovementIncorrectNetQuantity").ConfigureAwait(false);
            }
        }

        [Then(@"backup movement information should be stored")]
        public void ThenBackupMovementInformationShouldBeStored()
        {
            this.LogToReport("Covered in next step");
        }

        [Then(@"""(.*)"" with data of the last event type with negative net quantity should be stored")]
        public async Task ThenMovementWithDataOfTheLastEventTypeWithNegativeNetQuantityShouldBeStoredAsync(string type)
        {
            Assert.AreEqual(3, await this.ReadSqlScalarAsync<int>(input: SqlQueries.GetCountOfMovementsByMovementId, args: new { MovementId = this.GetValue(type == "backup movement" ? "BackupMovementId" : "OfficialMovementId") }).ConfigureAwait(false));
            var movementDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementDetailsByMovementId, args: new { MovementId = this.GetValue(type == "backup movement" ? "BackupMovementId" : "OfficialMovementId") }).ConfigureAwait(false);
            Assert.AreEqual("2460.00", movementDetails["NetStandardVolume"]);
            Assert.AreEqual("UPDATE", movementDetails["EventType"].ToUpperInvariant());
            Assert.AreEqual(this.GetValue("GlobalMovementId"), movementDetails["GlobalMovementId"]);
            movementDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastButOneMovementDetailsByMovementId, args: new { MovementId = this.GetValue(type == "backup movement" ? "BackupMovementId" : "OfficialMovementId") }).ConfigureAwait(false);
            Assert.AreEqual("-2450.00", movementDetails["NetStandardVolume"]);
        }

        [Then(@"last event type of the movement should be updated with the global identifier")]
        [Then(@"the global identifier of last event type reported to SAP PO should be updated")]
        public async Task ThenLastEventTypeOfTheMovementShouldBeUpdatedWithTheGlobalIdentifierAsync()
        {
            var movementDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementDetailsByMovementId, args: new { MovementId = this.GetValue("BackupMovementId") }).ConfigureAwait(false);
            Assert.AreEqual(this.GetValue("GlobalMovementId"), movementDetails["GlobalMovementId"]);
        }

        [When(@"all the validations are met and the ""(.*)"" has events reported to SAP PO as transfer points")]
        [When(@"all the validations are met and the net quantity of the last event matches with the net quantity of the ""(.*)"" received")]
        public async Task WhenAllTheValidationsAreMetAndTheBackupMovementHasEventsReportedToSAPPOAsTransferPointsAsync(string type)
        {
            if (type.EqualsIgnoreCase("backup movement"))
            {
                await this.RegisterOfficialPointAsync("BackupMovementWithEventsReportedToSAP").ConfigureAwait(false);
            }
            else
            {
                await this.RegisterOfficialPointAsync("OfficialMovementWithEventsReportedToSAP").ConfigureAwait(false);
            }
        }

        [Then(@"official movement information should be stored with all its details")]
        [Then(@"last event type of the movement should be updated with all the mentioned details")]
        [Then(@"last event type reported to SAP PO should be updated with all the mentioned details")]
        public async Task ThenOfficialMovementInformationShouldBeStoredWithAllItsDetailsAsync()
        {
            var movementDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementDetailsByMovementId, args: new { MovementId = this.GetValue("OfficialMovementId") }).ConfigureAwait(false);
            Assert.AreEqual(this.GetValue("GlobalMovementId"), movementDetails["GlobalMovementId"]);
            Assert.AreEqual("True", movementDetails["IsOfficial"].ToString());
            Assert.AreEqual(this.GetValue("BackupMovementId"), movementDetails["BackupMovementId"]);
        }

        [Then(@"errors should be registered in pending transaction log when there are any errors occured while storing the official movement for ""(.*)""")]
        public async Task ThenErrorsShouldBeRegisteredInPendingTransactionLogWhenThereAreAnyErrorsOccuredWhileStoringTheOfficialMovementForAsync(string type)
        {
            var content = this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)].ToString();
            if (type == "registration")
            {
                this.SetValue("OfficialMovementId", new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                content = content.JArrayModifyPropertyValue("Array_2 movementId", this.GetValue("OfficialMovementId"));
            }
            else if (type == "last event type delete")
            {
                await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateMovementEventType, args: new { MovementId = this.GetValue("OfficialMovementId") }).ConfigureAwait(false);
            }
            else
            {
                content = content.JArrayModifyPropertyValue("Array_2 netStandardQuantity", "2470.00");
            }

            this.SetValue("GlobalMovementId", new Faker().Random.AlphaNumeric(10).ToString(CultureInfo.InvariantCulture));
            content = content.JArrayModifyPropertyValue("OfficialInformation_1 globalMovementId", this.GetValue("GlobalMovementId"));
            content = content.JArrayModifyPropertyValue("OfficialInformation_2 globalMovementId", this.GetValue("GlobalMovementId"));
            content = content.JArrayModifyPropertyValue("Array_2 measurementUnit", "bbl");
            await this.GivenIAmAuthenticatedForServiceAsync("sappoapi").ConfigureAwait(false);
            await this.SetResultsAsync(async () => await this.SapPostAsync<dynamic>(this.SapEndpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Official]), JArray.Parse(content.ToString())).ConfigureAwait(false)).ConfigureAwait(false);
            var movementDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementDetailsByMovementId, args: new { MovementId = this.GetValue("OfficialMovementId") }).ConfigureAwait(false);
            if (type == "registration")
            {
                Assert.IsNull(movementDetails);
            }
            else if (type == "last event type delete")
            {
                Assert.AreEqual("DELETE", movementDetails["EventType"].ToUpperInvariant());
                Assert.AreNotEqual(this.GetValue("GlobalMovementId"), movementDetails["GlobalMovementId"]);
            }
            else
            {
                Assert.AreEqual("2460.00", movementDetails["NetStandardVolume"]);
                Assert.AreNotEqual(this.GetValue("GlobalMovementId"), movementDetails["GlobalMovementId"]);
            }

            var pendingTransactionDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetTopPendingTransaction).ConfigureAwait(false);
            StringAssert.Contains("Debe ser de tipo: Unit", pendingTransactionDetails["ErrorJson"].ToString());
            this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)] = content;
        }

        [When(@"only one official point movement arrives")]
        public void WhenOnlyOneOfficialPointMovementArrives()
        {
            this.LogToReport("Covered in previous step");
        }

        [When(@"all the other validations are met")]
        public async Task WhenAllTheOtherValidationsAreMetAsync()
        {
            await Task.Delay(10000).ConfigureAwait(false);
            var content = this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)].ToString();
            content = content.JArrayModifyPropertyValue("Array_1 sourceSystem", "162");
            content = content.JArrayModifyPropertyValue("Array_1 movementTypeId", this.GetValue("MovementTypeId"));
            content = content.JArrayModifyPropertyValue("Array_1 measurementUnit", "31");
            content = content.JArrayModifyPropertyValue("Array_1 segmentId", this.GetValue("SegmentId"));
            content = content.JArrayModifyPropertyValue("Array_1 operatorId", this.GetValue("OperatorId"));
            content = content.JArrayModifyPropertyValue("MovementSource_1 sourceNodeId", this.GetValue("NodeId_2"));
            content = content.JArrayModifyPropertyValue("MovementSource_1 sourceProductTypeId", string.Empty);
            content = content.JArrayModifyPropertyValue("MovementSource_1 sourceStorageLocationId", string.Empty);
            content = content.JArrayModifyPropertyValue("MovementSource_1 sourceProductId", "10000002318");
            content = content.JArrayModifyPropertyValue("MovementDestination_1 destinationNodeId", this.GetValue("NodeId_1"));
            content = content.JArrayModifyPropertyValue("MovementDestination_1 destinationProductTypeid", string.Empty);
            content = content.JArrayModifyPropertyValue("MovementDestination_1 destinationProductId", "10000002318");
            content = content.JArrayModifyPropertyValue("MovementDestination_1 destinationStorageLocationId", string.Empty);
            content = content.JArrayModifyPropertyValue("Attributes_1 attributeId", this.GetValue("AttributeId"));
            content = content.JArrayModifyPropertyValue("Attributes_1 valueAttributeUnit", this.GetValue("ValueAttributeUnitId"));
            content = content.JArrayModifyPropertyValue("MovementOwner_1 ownerId", this.GetValue("Owner"));
            content = content.JArrayModifyPropertyValue("MovementOwner_1 ownershipValue", "100");
            content = content.JArrayModifyPropertyValue("MovementOwner_1 ownerShipValueUnit", " %");
            content = content.JArrayModifyPropertyValue("OfficialInformation_1 globalMovementId", this.GetValue("GlobalMovementId"));
            var movementId = content.JarrayGetValue("Array_1 movementId");
            var movementTransactionIdValue = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementTransactionId, args: new { MovementId = movementId }).ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(SqlQueries.InsertIntoSapTracking, args: new { movementTransactionId = movementTransactionIdValue["MovementTransactionId"] }).ConfigureAwait(false);
            this.SetValue("OfficialMovementId", movementId);
            this.SetValue("GlobalMovementId", new Faker().Random.AlphaNumeric(10).ToString(CultureInfo.InvariantCulture));
            content = content.JArrayModifyPropertyValue("OfficialInformation_1 globalMovementId", this.GetValue("GlobalMovementId"));
            await this.GivenIAmAuthenticatedForServiceAsync("sappoapi").ConfigureAwait(false);
            await this.SetResultsAsync(async () => await this.SapPostAsync<dynamic>(this.SapEndpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Official]), JArray.Parse(content.ToString())).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Then(@"last event type reported to SAP PO should be updated with all the mentioned details and backup movement identifier should be updated as null")]
        public async Task ThenLastEventTypeReportedToSAPPOShouldBeUpdatedWithAllTheMentionedDetailsAndBackupMovementIdentifierShouldBeUpdatedAsNullAsync()
        {
            await Task.Delay(10000).ConfigureAwait(false);
            var movementDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementDetailsByMovementId, args: new { MovementId = this.GetValue("OfficialMovementId") }).ConfigureAwait(false);
            Assert.AreEqual(this.GetValue("GlobalMovementId"), movementDetails["GlobalMovementId"]);
            Assert.AreEqual("True", movementDetails["IsOfficial"].ToString());
            Assert.AreEqual(null, movementDetails["BackupMovementId"]);
        }
    }
}