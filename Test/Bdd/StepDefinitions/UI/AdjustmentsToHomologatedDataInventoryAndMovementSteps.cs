// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdjustmentsToHomologatedDataInventoryAndMovementSteps.cs" company="Microsoft">
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

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.StepDefinitions.Api;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class AdjustmentsToHomologatedDataInventoryAndMovementSteps : EcpApiStepDefinitionBase
    {
        public AdjustmentsToHomologatedDataInventoryAndMovementSteps(FeatureContext featureContext)
          : base(featureContext)
        {
        }

        [Given(@"I want to register an ""(.*)"" in the system using Excel")]
        public async Task GivenIWantToRegisterAnInTheSystemUsingExcelAsync(string entity)
        {
            int flag;
            try
            {
                flag = int.Parse(this.FeatureContext["TestDataCreated"].ToString(), CultureInfo.InvariantCulture);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch
#pragma warning restore CA1031 // Do not catch general exception types
            {
                this.FeatureContext.Add("TestDataCreated", 0);
                flag = int.Parse(this.FeatureContext["TestDataCreated"].ToString(), CultureInfo.InvariantCulture);
            }

#pragma warning disable CA1305 // Specify IFormatProvider
            if (flag == 0)
#pragma warning restore CA1305 // Specify IFormatProvider
            {
                ////this.When("I update the excel with \"Testdata_9790\"");
                this.IUpdateTheExcelFile("Testdata_9790");
                await this.TestDataCreationforInventoryAndMovementAsync().ConfigureAwait(false);
            }

            Assert.IsNotNull(entity);
        }

        [When(@"I receive the data​ with ""(.*)"" less than Current date")]
        public void WhenIReceiveTheDataWithLessThanCurrentDate(string date)
        {
            // Implementation for this method is already taken care in TestDataCreationforInventoryAndMovement()
            Assert.IsNotNull(date);
        }

        [When(@"I receive the data​ with ""(.*)"" greater than current date minus the number of valid days")]
        public void WhenIReceiveTheDataWithGreaterThanCurrentDateMinusTheNumberOfValidDays(string date)
        {
            // Implementation for this method is already taken care in TestDataCreationforInventoryAndMovement()
            Assert.IsNotNull(date);
        }

        [When(@"I receive the data​ with ""(.*)"" is equal to current date minus the number of valid days")]
        public void WhenIReceiveTheDataWithIsEqualToCurrentDateMinusTheNumberOfValidDays(string date)
        {
            // Implementation for this method is already taken care in TestDataCreationforInventoryAndMovement()
            Assert.IsNotNull(date);
        }

        [When(@"I receive the data​ with ""(.*)"" greater than the CurrentDate")]
        public void WhenIReceiveTheDataWithGreaterThanTheCurrentDate(string date)
        {
            // Implementation for this method is already taken care in TestDataCreationforInventoryAndMovement()
            Assert.IsNotNull(date);
        }

        [When(@"I receive the data​ with ""(.*)"" less than the current date minus the number of valid days")]
        public void WhenIReceiveTheDataWithLessThanTheCurrentDateMinusTheNumberOfValidDays(string date)
        {
            // Implementation for this method is already taken care in TestDataCreationforInventoryAndMovement()
            Assert.IsNotNull(date);
        }

        [Then(@"""(.*)"" should be registered")]
        public async Task ThenShouldBeRegisteredAsync(string entity)
        {
            if (entity == "Inventory")
            {
                var inventoryRow = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetMultipleInventories, args: new { inventoryId1 = this.FeatureContext["Inventory1"].ToString(), inventoryId2 = this.FeatureContext["Inventory2"].ToString(), inventoryId3 = this.FeatureContext["Inventory3"].ToString() }).ConfigureAwait(false);
                Assert.IsTrue(inventoryRow.ToDictionaryList().Count.Equals(3));
            }
            else
            {
                var movementRow = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetMultipleMovements, args: new { movementId1 = this.FeatureContext["Movement1"].ToString(), movementId2 = this.FeatureContext["Movement2"].ToString(), movementId3 = this.FeatureContext["Movement3"].ToString() }).ConfigureAwait(false);
                Assert.IsTrue(movementRow.ToDictionaryList().Count.Equals(3));
            }
        }

        [Then(@"""(.*)"" must be stored in a Pendingtransactions repository with validation ""(.*)""")]
        public async Task ThenMustBeStoredInAPendingtransactionsRepositoryWithValidationAsync(string entity, string message)
        {
            var now = DateTime.Now;
            var firstDayCurrentMonth = new DateTime(now.Year, now.Month, 1);
            if (entity == "Inventory")
            {
                message = $"La fecha del inventario debe estar entre {firstDayCurrentMonth.AddDays(-8).ToShortDateString().ToString(CultureInfo.InvariantCulture)} 00:00:00 y {firstDayCurrentMonth.AddDays(-1).ToShortDateString().ToString(CultureInfo.InvariantCulture)} 00:00:00";
            }
            else
            {
                message = $"La fecha operativa del movimiento debe estar entre {firstDayCurrentMonth.AddDays(-8).ToShortDateString().ToString(CultureInfo.InvariantCulture)} 00:00:00 y {firstDayCurrentMonth.AddDays(-1).ToShortDateString().ToString(CultureInfo.InvariantCulture)} 00:00:00";
            }

            var lastMessageId = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["FileRegistration"]).ConfigureAwait(false);
            var pendingTransactionRecords = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetPendingTransactionByMessageId, args: new { messageId = lastMessageId["UploadId"] }).ConfigureAwait(false);
            Assert.IsTrue(pendingTransactionRecords.ToJson().Contains(message));
        }

        public async Task TestDataCreationforInventoryAndMovementAsync()
        {
            ////this.Given("I want to register a \"Homologation\" in the system");
            await this.IWantToRegisterAnExcelHomologationInTheSystemAsync("Homologation", this).ConfigureAwait(false);
            ////this.When("I am logged in as \"admin\"");
            this.LoggedInAsUser("admin");
            ////this.When("I navigate to \"FileUpload\" page");
            this.UiNavigation("FileUpload");
            ////this.When("I click on \"FileUpload\" \"button\"");
            this.IClickOn("FileUpload", "button");
            ////this.When("I select segment from \"FileUpload\" \"Segment\" \"dropdown\"");
            this.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            ////this.When("I select \"Insert\" from FileUpload dropdown");
            this.ISelectFileFromFileUploadDropdown("Insert");
            ////this.When("I click on \"Browse\" to upload");
            this.IClickOnUploadButton("Browse");
            ////this.When("I select \"Testdata_9790\" file from directory");
            await this.ISelectFileFromDirectoryAsync("Testdata_9790").ConfigureAwait(false);
            ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
            this.IClickOn("uploadFile\" \"Submit", "button");
            ////this.When("I wait till file upload to complete");
            await this.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
            this.FeatureContext["TestDataCreated"] = 1;
        }
    }
}
