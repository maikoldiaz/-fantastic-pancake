// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewValidationsInExcelUploadSteps.cs" company="Microsoft">
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
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using Bogus;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.StepDefinitions.UI;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using NUnit.Framework;
    using Ocaramba.Types;
    using OfficeOpenXml;
    using TechTalk.SpecFlow;

    [Binding]
    public class NewValidationsInExcelUploadSteps : EcpWebStepDefinitionBase
    {
        [Given(@"I update the ""(.*)"" excel for official inventory with operational date prior the current month")]
        [Given(@"I update the ""(.*)"" excel for official movement with operational date prior the current month")]
        [StepDefinition(@"I update the ""(.*)"" excel")]
        public void WhenIUpdateTheExcelForOfficialMovements(string fileName)
        {
            this.IUpdateTheExcelForOfficialMovements(fileName);
        }

        [Given(@"I have (.*) inventory with scenarioId attribute as (.*)")]
        [Given(@"I have (.*) movement with scenarioId attribute as (.*)")]
        public void GivenIHaveMovementWithScenarioIdAttributeAs(int value1, int value2)
        {
            this.LogToReport(value1 + value2);
        }

        [Then(@"I clicked (on "".*"" "".*"" "".*"")  for ""(.*)""")]
        public void ThenIClickedOnFor(ElementLocator elementLocator, string variable)
        {
            if (variable == "Movements")
            {
                this.Get<ElementPage>().GetElements(elementLocator).ElementAt(0).Click();
            }
            else if (variable == "Inventory")
            {
                this.Get<ElementPage>().GetElements(elementLocator).ElementAt(1).Click();
            }
        }

        [Then(@"Official movement should be stored into the system")]
        public async System.Threading.Tasks.Task ThenOfficialMovementShouldBeStoredIntoTheSystemAsync()
        {
            var movementRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementBySegmentId, args: new { segmentId = this.ScenarioContext["SegmentId"] }).ConfigureAwait(false);
            var movementDetails = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetMovementDetails, args: new { segmentId = this.ScenarioContext["SegmentId"] }).ConfigureAwait(false);
            Assert.AreEqual(ConstantValues.UpdatedMovementCount.ToInt(), movementDetails.ToDictionaryList().Count);
            Assert.AreEqual(this.GetValue("ActionType"), movementRow["EventType"]);
        }

        [Then(@"Existing movement should be stored in the system with negative volume")]
        public async System.Threading.Tasks.Task ThenExistingMovementShouldBeStoredInTheSystemWithNegativeVolumeAsync()
        {
            var movementRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementBySegmentIdWithInsertEvent, args: new { segmentId = this.ScenarioContext["SegmentId"] }).ConfigureAwait(false);
            var updatedMovementRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.EventBasedMovementDetails, args: new { segmentId = this.ScenarioContext["SegmentId"], Event = this.GetValue("ActionType") }).ConfigureAwait(false);
            var commulativeNetStandardVolume = movementRow["NetStandardVolume"].ToDecimal() + updatedMovementRow["NetStandardVolume"].ToDecimal();
            Assert.AreEqual(0, commulativeNetStandardVolume.ToInt());
        }

        [Then(@"Identifier of the negative movement and the movement to be stored must be the same identifier of the original movement")]
        public async System.Threading.Tasks.Task ThenIdentifierOfTheNegativeMovementAndTheMovementToBeStoredMustBeTheSameIdentifierOfTheOriginalMovementAsync()
        {
            var movementRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementBySegmentIdWithInsertEvent, args: new { segmentId = this.ScenarioContext["SegmentId"] }).ConfigureAwait(false);
            var updatedMovementRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.EventBasedMovementDetails, args: new { segmentId = this.ScenarioContext["SegmentId"], Event = this.GetValue("ActionType") }).ConfigureAwait(false);
            Assert.AreEqual(movementRow["MovementId"], updatedMovementRow["MovementId"]);
        }

        [Then(@"Official inventory should be stored into the system")]
        public async System.Threading.Tasks.Task ThenOfficialInventoryShouldBeStoredIntoTheSystemAsync()
        {
            var inventoryRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetInventoryBySegmentId, args: new { segmentId = this.ScenarioContext["SegmentId"] }).ConfigureAwait(false);
            var invenetoryDetails = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetInventoryDetails, args: new { segmentId = this.ScenarioContext["SegmentId"] }).ConfigureAwait(false);
            Assert.AreEqual(ConstantValues.UpdatedInventoryCount.ToInt(), invenetoryDetails.ToDictionaryList().Count);
            Assert.AreEqual(this.GetValue("ActionType"), inventoryRow["EventType"]);
        }

        [Then(@"Existing inventory should be stored in the system with negative volume")]
        public async System.Threading.Tasks.Task ThenExistingInventoryShouldBeStoredInTheSystemWithNegativeVolumeAsync()
        {
            var inventoryRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetInventoryBySegmentIdWithOrder, args: new { segmentId = this.ScenarioContext["SegmentId"] }).ConfigureAwait(false);
            var updatedInventoryRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.EventBasedInventoryDetails, args: new { segmentId = this.ScenarioContext["SegmentId"], Event = this.GetValue("ActionType") }).ConfigureAwait(false);
            var commulativeNetStandardVolume = inventoryRow["ProductVolume"].ToDecimal() + updatedInventoryRow["ProductVolume"].ToDecimal();
            Assert.AreEqual(0, commulativeNetStandardVolume.ToInt());
        }

        [Then(@"Identifier of the negative inventory and the inventory to be stored must be the same identifier of the original inventory")]
        public async System.Threading.Tasks.Task ThenIdentifierOfTheNegativeInventoryAndTheInventoryToBeStoredMustBeTheSameIdentifierOfTheOriginalInventoryAsync()
        {
            var inventoryRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetInventoryBySegmentIdWithOrder, args: new { segmentId = this.ScenarioContext["SegmentId"] }).ConfigureAwait(false);
            var updatedInventoryRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.EventBasedInventoryDetails, args: new { segmentId = this.ScenarioContext["SegmentId"], Event = this.GetValue("ActionType") }).ConfigureAwait(false);
            Assert.AreEqual(inventoryRow["InventoryId"], updatedInventoryRow["InventoryId"]);
        }

        [Given(@"I update the ""(.*)"" excel for official inventory with opearional date of the current month")]
        [Given(@"I update the ""(.*)"" excel for official movements with operational date of the current month")]
        public void GivenIUpdateTheExcelForOfficialMovementsWithOperationalDateOfTheCurrentMonth(string fileName)
        {
            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));
            //// For Inventories
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            worksheet.Cells[2, 3].Value = DateTime.UtcNow.AddDays(-1);
            for (int i = 1; i <= 2; i++)
            {
                worksheet = package.Workbook.Worksheets[i];
                worksheet.Cells[2, 2].Value = DateTime.UtcNow.AddDays(-1);
            }

            if (fileName == "TestData_Official")
            {
                for (int i = 2; i <= worksheet.Dimension.Rows; i++)
                {
                    worksheet = package.Workbook.Worksheets[0];
                    var concatenatedvalue = new Faker().Random.AlphaNumeric(149);
                    worksheet.Cells[i, 2].Value = concatenatedvalue;
                    this.ScenarioContext.Add("InventoryData", worksheet.Cells[i, 2].Value);
                    worksheet = package.Workbook.Worksheets[1];
                    worksheet.Cells[i, 1].Value = concatenatedvalue;
                    worksheet = package.Workbook.Worksheets[2];
                    worksheet.Cells[i, 1].Value = concatenatedvalue;
                }
            }
            //// For Movements
            if (fileName == "TestData_Official")
            {
                worksheet = package.Workbook.Worksheets[3];
                for (int i = 2; i <= worksheet.Dimension.Rows; i++)
                {
                    worksheet = package.Workbook.Worksheets[3];
                    var concatenatedvalue = new Faker().Random.AlphaNumeric(149);
                    worksheet.Cells[i, 2].Value = concatenatedvalue;
                    this.ScenarioContext.Add("MovementData", worksheet.Cells[i, 2].Value);
                    worksheet = package.Workbook.Worksheets[4];
                    worksheet.Cells[i, 1].Value = concatenatedvalue;
                    worksheet = package.Workbook.Worksheets[5];
                    worksheet.Cells[i, 1].Value = concatenatedvalue;
                }
            }

            //// Modifying data in FechaHoraInicial(j=6) and FechaHoraFinal(j=7) columns

            worksheet = package.Workbook.Worksheets[3];
            for (int j = 6; j <= 7; j++)
            {
                worksheet.Cells[2, j].Value = DateTime.UtcNow.AddDays(-1);
            }

            package.Save();
            package.Dispose();
        }

        [StepDefinition(@"Verify that movement Id from excel upload should be hashed and stored in MovementId column in Movement table")]
        public async System.Threading.Tasks.Task ThenVerifyThatMovementIdFromExcelUploadShouldBeHashedAndStoredInMovementIdColumnInMovementTableAsync()
        {
            var movementRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementBySegmentId, args: new { segmentId = this.ScenarioContext["SegmentId"] }).ConfigureAwait(false);
            var randomstring = this.GetValue("MovementData");
            using var crypt = new SHA256Managed();
            var hashBuilder = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomstring));
            foreach (byte theByte in crypto)
            {
                hashBuilder.Append(theByte.ToString("x2", CultureInfo.InvariantCulture));
            }

            var hash = hashBuilder.ToString();
            Assert.AreEqual(hash.Substring(hash.Length - 50), movementRow["MovementId"]);
        }

        [Then(@"Verify that inventory Id from excel upload should be hashed and stored in Inventoryid and InventoryProductUniqueId column in InventoryProduct table")]
        public async System.Threading.Tasks.Task ThenVerifyThatInventoryIdFromExcelUploadShouldBeHashedAndStoredInInventoryidAndInventoryProductUniqueIdColumnInInventoryProductTableAsync()
        {
            var inventoryRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetInventoryBySegmentId, args: new { segmentId = this.ScenarioContext["SegmentId"] }).ConfigureAwait(false);
            var randomstring = this.GetValue("InventoryData");
            using var crypt = new SHA256Managed();
            var hashBuilder = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomstring));
            foreach (byte theByte in crypto)
            {
                hashBuilder.Append(theByte.ToString("x2", CultureInfo.InvariantCulture));
            }

            var hash = hashBuilder.ToString();
            Assert.AreEqual(hash.Substring(hash.Length - 50), inventoryRow["InventoryId"]);
            Assert.AreEqual(hash.Substring(hash.Length - 50), inventoryRow["InventoryProductUniqueId"]);
        }

        [Then(@"movement should be updated in system")]
        public async System.Threading.Tasks.Task ThenMovementShouldBeUpdatedInSystemAsync()
        {
            await Task.Delay(30000).ConfigureAwait(true);
            Assert.AreEqual(2, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfMovementsByEventType, args: new { eventType = "UPDATE", MovementId = this.GetValue(ConstantValues.MovementId) }).ConfigureAwait(false));
        }

        [Then(@"movement should be deleted in system")]
        public async Task ThenMovementShouldBeDeletedInSystemAsync()
        {
            await Task.Delay(30000).ConfigureAwait(true);
            Assert.AreEqual(1, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfMovementsByEventType, args: new { eventType = "DELETE", MovementId = this.GetValue(ConstantValues.MovementId) }).ConfigureAwait(false));
        }

        [Then(@"I have Official information data")]
        public void ThenIHaveOfficialInformationData()
        {
            string randomstring = null;
            if (!string.IsNullOrEmpty(this.GetValue(ConstantValues.InventoryId)))
            {
                randomstring = this.GetValue(ConstantValues.InventoryId);
            }
            else
            {
                randomstring = this.GetValue(ConstantValues.MovementId);
            }

            using var crypt = new SHA256Managed();
            var hashBuilder = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomstring));
            foreach (byte theByte in crypto)
            {
                hashBuilder.Append(theByte.ToString("x2", CultureInfo.InvariantCulture));
            }

            var hash = hashBuilder.ToString();
            if (!string.IsNullOrEmpty(this.GetValue(ConstantValues.InventoryId)))
            {
                this.SetValue(ConstantValues.InventoryId, hash.Substring(hash.Length - 50));
            }
            else
            {
                this.SetValue(ConstantValues.MovementId, hash.Substring(hash.Length - 50));
            }
        }
    }
}