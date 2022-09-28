// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IncludeOperativeScenarioAndOfficialTransferPointsInOperationalCutoffSteps.cs" company="Microsoft">
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
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Bogus;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.StepDefinitions.UI;
    using Ecp.True.Bdd.Tests.Utils;
    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;

    [Binding]
    public class IncludeOperativeScenarioAndOfficialTransferPointsInOperationalCutoffSteps : EcpWebStepDefinitionBase
    {
        [Given(@"I have ownershipnodes created for node status having movements with transfer point")]
        public void GivenIHaveOwnershipnodesCreatedForNodeStatusHavingMovementsWithTransferPoint()
        {
            this.SetValue("TransferPointNodes", "True");
        }

        [Given(@"I upload the excel into the system")]
        public async System.Threading.Tasks.Task GivenIUploadTheExcelIntoTheSystemAsync()
        {
            await this.IUploadTheExcelIntoTheSystemAsync("TestData_TransferPoint").ConfigureAwait(false);
        }

        [Given(@"I have ""(.*)"" excel having movements with transfer point")]
        public void GivenIHaveExcelHavingMovementsWithTransferPoint(string fileName)
        {
            this.IHaveExcelHavingMovementsWithTransferPoint(fileName);
        }

        [StepDefinition(@"Verify that TransferPoint movements without a global identifier should be displayed in the grid")]
        public async System.Threading.Tasks.Task ThenVerifyThatTransferPointMovementsWithoutAGlobalIdentifierShouldBeDisplayedInTheGridAsync()
        {
            var getTheMovementDetailsRecords = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetTheMovementDetails, args: new { SegmentName = this.GetValue("CategorySegment") }).ConfigureAwait(false);
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);
            var movementIds = this.Get<ElementPage>().GetElements(nameof(Resources.TransferPointMovements));
            Assert.AreEqual(getTheMovementDetailsRecords.Count(), movementIds.Count);
        }

        [Then(@"Verify that Note should be assigned to all the selected Transfer point movements from the grid")]
        public async System.Threading.Tasks.Task ThenVerifyThatNoteShouldBeAssignedToAllTheSelectedTransferPointMovementsFromTheGridAsync()
        {
            var sapTrackingTableRecords = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetNoteFromSAPTrackingTable, args: new { SegmentName = this.GetValue("CategorySegment") }).ConfigureAwait(false);
            var sapTrackingTableRecordList = sapTrackingTableRecords.ToDictionaryList();
            foreach (var transferpointrecords in sapTrackingTableRecordList)
            {
                Assert.AreEqual(ConstantValues.TransferPointAddNote, transferpointrecords["Comment"]);
            }
        }

        [Then(@"It should mark as official points for all the movements")]
        [Then(@"Verify that Ticket Number should be assigned to all the movements")]
        public async System.Threading.Tasks.Task ThenVerifyThatTicketNumberShouldBeAssignedToAllTheMovementsAsync()
        {
            var getTheMovementDetailsRecords = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetTheMovementDetails, args: new { SegmentName = this.GetValue("CategorySegment") }).ConfigureAwait(false);
            var getTheMovementDetailsRecordsList = getTheMovementDetailsRecords.ToDictionaryList();
            foreach (var getMovementDetails in getTheMovementDetailsRecordsList)
            {
                Assert.AreEqual(this.ScenarioContext.Get<string>("TicketId"), getMovementDetails["TicketId"].ToString());
                Assert.AreEqual(1, getMovementDetails["IsOfficial"].ToInt());
            }
        }

        [Given(@"I have official transfer point movements belongs official scenario \(scenario id two\) and have a global identifier")]
        public async System.Threading.Tasks.Task GivenIHaveOfficialTransferPointMovementsBelongsOfficialScenarioScenarioIdTwoAndHaveAGlobalIdentifierAsync()
        {
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateGlobalMovementID, args: new { GlobalMovementId = new Faker().Random.Int(1000, 20000).ToString(CultureInfo.InvariantCulture), MovementId = this.ScenarioContext["Movement5"].ToString() }).ConfigureAwait(false);
        }

        [Given(@"I have inventories of the operative  scenario belongs to the selected segment, with an inventory date of previous day and the last day of the period")]
        [Given(@"I have movements of the operative scenario, which are NOT transfer points belongs to the selected segment, with an operational date equal to the date of the period day")]
        [Given(@"I have inventories of the official scenario belongs to the selected segment with an inventory date of previous day and the last day of the period")]
        [Given(@"I have movements of the official scenario belongs to the selected segment with an operational date equal to the date of the period day")]
        [Given(@"Operational date equal to the date of the period day")]
        public void GivenOperationalDateEqualToTheDateOfThePeriodDay()
        {
            ////this step left blank as this step has been taken care during Excel file creation step.
        }

        [StepDefinition(@"the movements with global identifier should not be there on this list")]
        public async System.Threading.Tasks.Task WhenTheMovementsWithGlobalIdentifierShouldNotBeThereOnThisListAsync()
        {
            var movementIds = this.Get<ElementPage>().GetElements(nameof(Resources.TransferPointMovements));
            foreach (var movementId in movementIds)
            {
                var getMovementDetails = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetMovementId, args: new { movementId = movementId.Text.ToString() }).ConfigureAwait(false);
                Assert.IsNull(getMovementDetails["GlobalMovementId"]);
            }
        }

        [StepDefinition(@"Verify that the transfer point movements belong to the official scenario should not be taken into account in calculations of operational cutoff")]
        public async System.Threading.Tasks.Task ThenVerifyThatTheTransferPointMovementsBelongToTheOfficialScenarioShouldNotBeTakenIntoAccountInCalculationsOfOperationalCutoffAsync()
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            var crypt = new SHA256Managed();
#pragma warning restore CA2000 // Dispose objects before losing scope
            var hashBuilder = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(this.ScenarioContext["Movement5"].ToString()));
            foreach (byte theByte in crypto)
            {
                hashBuilder.Append(theByte.ToString("x2", CultureInfo.InvariantCulture));
            }

            var hash = hashBuilder.ToString();
            this.SetValue("OfficialMovementData", hash.Substring(hash.Length - 50));
            var getTheMovementDetailsRecords = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetMovementId, args: new { movementId = this.GetValue("OfficialMovementData").ToString() }).ConfigureAwait(false);
            Assert.IsNull(getTheMovementDetailsRecords["TicketId"]);
        }

        [Given(@"I have official transfer point movements belongs operative scenario \(scenario id one\) and have a global identifier")]
        public async System.Threading.Tasks.Task GivenIHaveOfficialTransferPointMovementsBelongsOperativeScenarioScenarioIdOneAndHaveAGlobalIdentifierAsync()
        {
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateGlobalMovementID, args: new { GlobalMovementId = new Faker().Random.Int(1000, 20000).ToString(CultureInfo.InvariantCulture), MovementId = this.ScenarioContext["Movement3"].ToString() }).ConfigureAwait(false);
        }

        [Then(@"Verify that the system must assign the corresponding ticket number for all these official transfer point movements")]
        public async System.Threading.Tasks.Task ThenVerifyThatTheSystemMustAssignTheCorrespondingTicketNumberForAllTheseOfficialTransferPointMovementsAsync()
        {
            var getTheMovementDetailsRecords = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetMovementId, args: new { movementId = this.ScenarioContext["Movement3"].ToString() }).ConfigureAwait(false);
            Assert.IsNotNull(getTheMovementDetailsRecords["TicketId"]);
        }

        [Given(@"I have official transfer point movements have update event records belongs operative scenario \(scenario id one\) but do not have global identifier")]
        [Given(@"I have official transfer point movements have insert and update event records belongs operative scenario \(scenario id one\) and have a global identifier")]
        public async System.Threading.Tasks.Task GivenIHaveOfficialTransferPointMovementsHaveInsertAndUpdateEventRecordsBelongsOperativeScenarioScenarioIdOneAndHaveAGlobalIdentifierAsync()
        {
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateGlobalMovementID, args: new { GlobalMovementId = new Faker().Random.Int(1000, 20000).ToString(CultureInfo.InvariantCulture), MovementId = this.ScenarioContext["Movement2"].ToString() }).ConfigureAwait(false);
            this.UiNavigation("FileUpload");
            this.IClickOn("FileUpload", "button");
            this.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            this.ISelectFileFromFileUploadDropdown("Update");
            this.IClickOnUploadButton("Browse");
            await this.ISelectFileFromDirectoryAsync("TestData_TransferPoint").ConfigureAwait(false);
            this.IClickOn("uploadFile\" \"Submit", "button");
            await this.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
        }

        [Then(@"Verify that these movements records that do not have a global identifier must be without a ticket number")]
        [Then(@"Verify that the system must assign the corresponding ticket number for all these official transfer point have insert and update event records movements")]
        public async System.Threading.Tasks.Task ThenVerifyThatTheSystemMustAssignTheCorrespondingTicketNumberForAllTheseOfficialTransferPointHaveInsertAndUpdateEventRecordsMovementsAsync()
        {
            var getInsertMovementDetails = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetMovementsByEventType, args: new { MovementId = this.ScenarioContext["Movement3"].ToString(), EventType = "Insert" }).ConfigureAwait(false);
            Assert.IsNotNull(getInsertMovementDetails["GlobalMovementId"]);
            Assert.AreEqual(this.ScenarioContext.Get<string>("TicketId"), getInsertMovementDetails["TicketId"].ToString());
            var getUpdatedMovementDetails = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetMovementsByEventType, args: new { MovementId = this.ScenarioContext["Movement3"].ToString(), EventType = "Update" }).ConfigureAwait(false);
            Assert.IsNull(getUpdatedMovementDetails["GlobalMovementId"]);
            Assert.IsNull(getUpdatedMovementDetails["TicketID"]);
        }

        [Then(@"Verify that the system should not be taken into account in the calculations of the operational cutoff for the official scenario movements and inventories")]
        public async System.Threading.Tasks.Task ThenVerifyThatTheSystemShouldNotBeTakenIntoAccountInTheCalculationsOfTheOperationalCutoffForTheOfficialScenarioMovementsAndInventoriesAsync()
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            var crypt = new SHA256Managed();
#pragma warning restore CA2000 // Dispose objects before losing scope
            var hashBuilder = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(this.ScenarioContext["Movement5"].ToString()));
            foreach (byte theByte in crypto)
            {
                hashBuilder.Append(theByte.ToString("x2", CultureInfo.InvariantCulture));
            }

            var hash = hashBuilder.ToString();
            this.SetValue("OfficialMovementData", hash.Substring(hash.Length - 50));
            var getTheMovementDetailsRecords = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetMovementId, args: new { movementId = this.GetValue("OfficialMovementData").ToString() }).ConfigureAwait(false);
            Assert.IsNull(getTheMovementDetailsRecords["TicketId"]);
            Assert.IsNull(getTheMovementDetailsRecords["GlobalMovementId"]);
            Assert.AreEqual("False", getTheMovementDetailsRecords["IsOfficial"]);
        }

        [Then(@"Verify that the system must take into account these movements and inventories and assign them the corresponding ticket number")]
        public async System.Threading.Tasks.Task ThenVerifyThatTheSystemMustTakeIntoAccountTheseMovementsAndInventoriesAndAssignThemTheCorrespondingTicketNumberAsync()
        {
            var getTheMovementDetailsRecords = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetMovementId, args: new { movementId = this.ScenarioContext["Movement2"].ToString() }).ConfigureAwait(false);
            Assert.AreEqual(this.ScenarioContext.Get<string>("TicketId"), getTheMovementDetailsRecords["TicketId"].ToString());
            Assert.AreEqual("True", getTheMovementDetailsRecords["IsOfficial"]);
        }

        [Then(@"Verify that the system must calculate the unbalances of the nodes for the above mentioned selected movements")]
        public void ThenVerifyThatTheSystemMustCalculateTheUnbalancesOfTheNodesForTheAboveMentionedSelectedMovements()
        {
            var unbalanceNodes = this.Get<ElementPage>().GetElements(nameof(Resources.UnbalanceGrid));
            Assert.AreEqual(3, unbalanceNodes.Count);
        }

        [Then(@"Verify that the system must calculate the unbalances of the nodes for the above mentioned selected movements and stored in the Databases")]
        public async System.Threading.Tasks.Task ThenVerifyThatTheSystemMustCalculateTheUnbalancesOfTheNodesForTheAboveMentionedSelectedMovementsAndStoredInTheDatabasesAsync()
        {
            var rows = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetUnabalnceByTicketId, args: new { TicketId = this.ScenarioContext.Get<string>("TicketId") }).ConfigureAwait(false);
            Assert.AreEqual(13, rows.ToDictionaryList().Count);
        }

        [Given(@"Backing movement of these  movements should belongs to the selected segment\.")]
        public void GivenBackingMovementOfTheseMovementsShouldBelongsToTheSelectedSegment()
        {
            this.LogToReport("Step Left Intentionally blanks");
        }
    }
}