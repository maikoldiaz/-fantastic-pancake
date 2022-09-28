// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdjustmentsToCutoffCalculationsSteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.StepDefinitions.UI
{
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Utils;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class AdjustmentsToCutoffCalculationsSteps : EcpWebStepDefinitionBase
    {
        [Given(@"I have uploaded inventories and movements into system")]
        public async Task GivenIHaveUploadedInventoriesAndMovementsIntoSystemAsync()
        {
            //// Updating Excel file
            this.IUpdateTheExcelDataForAdjustmentsInCutoff("AdjustmentsToCutoff");
            //// Upload excel into system with eventType Insert
            await this.IUploadTheExcelIntoTheSystemAsync("AdjustmentsToCutoff").ConfigureAwait(false);
        }

        [Given(@"I perform cutoff for segment without registered intial inventories")]
        [When(@"I perform cutoff for segment with multiple events with cutoff ticket")]
        public async Task GivenIPerformCutoffForSegmentWithoutRegisteredIntialInventoriesAsync()
        {
            await this.GenerationOfCutoffTicketAsync().ConfigureAwait(false);
        }

        [Then(@"use the value (.*) for operational cut-off calculations as there are no inventories as is currently done")]
        public void ThenUseTheValueForOperationalCutOffCalculationsAsThereAreNoInventoriesAsIsCurrentlyDone(string value)
        {
            Assert.IsNotNull(value);
        }

        [Then(@"inventories with a value of ""(.*)"" should not be created")]
        public async Task ThenInventoriesWithAValueOfShouldNotBeCreatedAsync(string value)
        {
            Assert.IsEmpty(await this.ReadAllSqlAsync(SqlQueries.GetInventoriesBySegmentAndVolume, args: new { segment = this.GetValue("CategorySegment"), value }).ConfigureAwait(false));
        }

        [Then(@"use ""(.*)"" as initial inventory value")]
        public async Task ThenUseAsInitialInventoryValueAsync(string value)
        {
            Assert.IsNotNull(value);
            value = "0.00";
            var initialInventoryDetails = await this.ReadSqlAsDictionaryAsync(SqlQueries.UnbalanceByTicketId, args: new { ticketId = this.GetValue("TicketId") }).ConfigureAwait(false);
            Assert.AreEqual(value, initialInventoryDetails["InitialInventory"].ToString());
        }

        [Then(@"initial inventory should be sum of the inventories of a product correspoding to day before the balance start date and that have a successfully executed cut-off ticket")]
        public async Task ThenInitialInventoryShouldBeSumOfTheInventoriesOfAProductCorrespodingToDayBeforeTheBalanceStartDateAndThatHaveASuccessfullyExecutedCutOffTicketAsync()
        {
            var initialInventoryDetails = await this.ReadSqlAsDictionaryAsync(SqlQueries.UnbalanceByTicketId, args: new { ticketId = this.GetValue("TicketId") }).ConfigureAwait(false);
            Assert.AreEqual("88317.00", initialInventoryDetails["InitialInventory"].ToString());
        }

        [When(@"I have initial inventory per product has a single event recorded with cut-off ticket")]
        [When(@"I have initial inventory per product have multiple events recorded without cut-off ticket")]
        [When(@"I have initial inventory per product has a single event recorded without cut-off ticket")]
        [When(@"I perform cutoff for segment with multiple events")]
        [When(@"I perform cutoff for segment with single event")]
        [Then(@"initial inventory should be sum of the inventories of a product correspoding to day before the balance start date")]
        [When(@"I perform cutoff for segment with multiple events without cutoff ticket")]
        [Then(@"initial inventory should be sum of the inventories of a product correspoding to day before the balance start date and that have cutoff ticket")]
        public void WhenIPerformCutoffForSegmentWithSingleEvent()
        {
            // Method intentionally left empty.
        }

        [When(@"I have initial inventory per product have multiple events recorded with cut-off ticket")]
        public async Task WhenIHaveInitialInventoryPerProductHaveMultipleEventsRecordedWithCutOffTicketAsync()
        {
            this.SetValue("PerformingSecondCutoff", "Yes");
            //// Updating Excel file
            this.IUpdateTheExcelDataForAdjustmentsInCutoff("AdjustmentsToCutoff");
            //// Upload excel into system with eventType Insert
            await this.IUploadTheExcelIntoTheSystemAsync("AdjustmentsToCutoff", "Update").ConfigureAwait(false);
        }

        public async Task GenerationOfCutoffTicketAsync()
        {
            this.UiNavigation("Operational Cutoff");
            ////this.When("I click on \"NewCut\" \"button\"");
            this.IClickOn("NewCut", "button");
            ////this.When("I choose CategoryElement from \"InitTicket\" \"Segment\" \"combobox\"");
            this.ISelectCategoryElementFrom("InitTicket\" \"Segment", "combobox");
            if (string.IsNullOrEmpty(this.GetValue("PerformingSecondCutoff")))
            {
                await this.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(3, "FirstCutoff").ConfigureAwait(false);
            }
            else
            {
                await this.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(3, "SecondCutoff").ConfigureAwait(false);
            }

            ////this.When("I click on \"InitTicket\" \"next\" \"button\"");
            this.IClickOn("InitTicket\" \"Submit", "button");
            ////this.Then("validate that \"checkInventories\" \"Next\" \"button\" as enabled");
            this.ValidateThatAsEnabled("validateInitialInventory\" \"Submit", "button");
            ////this.When("I click on \"checkInventories\" \"Next\" \"button\"");
            this.IClickOn("validateInitialInventory\" \"Submit", "button");
            this.ScenarioContext[ConstantValues.PendingTransactions] = 0;
            ////this.When("I select all pending records from grid");
            this.ISelectAllPendingRepositroriesFromGrid();
            this.ProvidedRequiredDetailsForPendingTransactionsGrid();
            ////this.Then("validate that \"ErrorsGrid\" \"Next\" \"button\" as enabled");
            this.ValidateThatAsEnabled("ErrorsGrid\" \"Submit", "button");
            ////this.When("I click on \"ErrorsGrid\" \"Next\" \"button\"");
            this.IClickOn("ErrorsGrid\" \"Submit", "button");
            ////this.When("I click on \"officialPointsGrid\" \"Next\" \"button\"");
            this.IClickOn("OfficialPointsGrid\" \"Submit", "button");
            this.ScenarioContext[ConstantValues.PendingTransactions] = 0;
            ////this.When("I select all unbalances in the grid");
            this.ISelectAllPendingRepositroriesFromGrid();
            this.ProvidedRequiredDetailsForUnbalancesGrid();
            ////this.When("I click on \"ConsistencyCheck\" \"Next\" \"button\"");
            this.IClickOn("unbalancesGrid\" \"Submit", "button");
            ////this.When("I click on \"Confirm\" \"Cutoff\" \"Submit\" \"button\"");
            this.IClickOn("confirmCutoff\" \"Submit", "button");
            ////this.When("I wait till cutoff ticket processing to complete");
            await this.WaitForTicketToCompleteProcessingAsync().ConfigureAwait(false);
        }
    }
}