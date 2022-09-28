// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtendedFicoProcessForOfficialDeltaStep.cs" company="Microsoft">
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
    using System.Linq;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Utils;
    using global::Bdd.Core.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json.Linq;
    using TechTalk.SpecFlow;

    [Binding]
    public class ExtendedFicoProcessForOfficialDeltaStep : EcpWebStepDefinitionBase
    {
        [Given(@"I have Calculate deltas by official adjustment for previous period")]
        public async System.Threading.Tasks.Task GivenIHaveCalculateDeltasByOfficialAdjustmentForPreviousPeriodAsync()
        {
            this.SetValue("YearForPeriod", "2020");
            this.SetValue("PreviousMonthName", "Feb");

            this.SetValue("DeltaCategorySegment", this.GetValue("SegmentName"));
            ////When I navigate to "Calculation of deltas by official adjust" page
            this.NavigateToPage("Calculation of deltas by official adjustment");
            ////And I click on "newDeltasCalculation" "button"
            this.IClickOn("newDeltasCalculation", "button");
            ////And I select delta segment from "initOfficialDeltaTicket" "segment" "dropdown"
            this.ISelectOfficialDeltaSegmentFrom("initOfficialDeltaTicket\" \"segment", "dropdown");
            ////And I select "2020" year from drop down
            ////this.ISelectYearFromDropDown(this.GetValue("YearForPeriod"));
            ////And I select "Jun" period from drop down
            this.ISelectAPeriodFromDropDownForParticularPeriod(this.GetValue("PreviousMonthName").ToPascalCase());
            ////And I click on "initOfficialDeltaTicket" "submit" "button"
            this.IClickOn("initOfficialDeltaTicket\" \"submit", "button");
            ////And I click on "validateOfficialDeltaTicket" "submit" "button"
            this.IClickOn("validateOfficialDeltaTicket\" \"submit", "button");
            ////And I click on "confirmOfficialDeltaTicket" "submit" "button"
            this.IClickOn("confirmOfficialDeltaTicket\" \"submit", "button");
            //// wait till ticket processing complete
            await this.VerifyThatOfficialDeltaCalculationShouldBeSuccessfulAsync().ConfigureAwait(false);
        }

        [When(@"I have Calculate deltas by official adjustment for next period")]
        public async System.Threading.Tasks.Task WhenIHaveCalculateDeltasByOfficialAdjustmentForNextPeriodAsync()
        {
            this.SetValue("YearForPeriod", "2020");
            this.SetValue("PreviousMonthName", "Mar");

            this.SetValue("DeltaCategorySegment", this.GetValue("SegmentName"));
            ////When I navigate to "Calculation of deltas by official adjust" page
            this.NavigateToPage("Calculation of deltas by official adjustment");
            ////And I click on "newDeltasCalculation" "button"
            this.IClickOn("newDeltasCalculation", "button");
            ////And I select delta segment from "initOfficialDeltaTicket" "segment" "dropdown"
            this.ISelectOfficialDeltaSegmentFrom("initOfficialDeltaTicket\" \"segment", "dropdown");
            ////And I select "2020" year from drop down
            ////this.ISelectYearFromDropDown(this.GetValue("YearForPeriod"));
            ////And I select "Jun" period from drop down
            this.ISelectAPeriodFromDropDownForParticularPeriod(this.GetValue("PreviousMonthName").ToPascalCase());
            ////And I click on "initOfficialDeltaTicket" "submit" "button"
            this.IClickOn("initOfficialDeltaTicket\" \"submit", "button");
            ////And I click on "validateOfficialDeltaTicket" "submit" "button"
            this.IClickOn("officialDeltaTicket\" \"next", "button");
            ////And I click on "confirmOfficialDeltaTicket" "submit" "button"
            this.IClickOn("confirmOfficialDeltaTicket\" \"submit", "button");
            //// wait till ticket processing complete
            await this.VerifyThatOfficialDeltaCalculationShouldBeSuccessfulAsync().ConfigureAwait(false);
        }

        [When(@"a node of the segment ticket is approved in the previous period")]
        public async System.Threading.Tasks.Task WhenANodeOfTheSegmentTicketIsApprovedInThePreviousPeriodAsync()
        {
            var ticketData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetLastOfficialDeltaTicket).ConfigureAwait(false);
            for (int i = 0; i <= ticketData.Count; i++)
            {
                await this.ReadAllSqlAsDictionaryAsync(SqlQueries.SetDeltaNodeApprover, args: new { ticketID = ticketData["TicketId"] }).ConfigureAwait(false);
            }

            await this.ReadAllSqlAsDictionaryAsync(SqlQueries.ApproveDeltaNodeTicket, args: new { ticketID = ticketData["TicketId"] }).ConfigureAwait(false);
        }

        [When(@"The operational date is equal to the start date of the period minus one day")]
        public async System.Threading.Tasks.Task WhenTheOperationalDateIsEqualToTheStartDateOfThePeriodMinusOneDayAsync()
        {
            await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.UpdateOperationalDateForMovements, args: new { segmentId = this.ScenarioContext["SegmentId"] }).ConfigureAwait(false);
        }

        [When(@"the created date is ""(.*)"" the approval date of the node in the previous period and less than the last approval date of the node in the ticket period")]
        [When(@"the created date is ""(.*)"" the approval date of the node in the previous period")]
        public async System.Threading.Tasks.Task WhenTheCreatedDateIsTheApprovalDateOfTheNodeInThePreviousPeriodAsync(string period)
        {
            if (period.ContainsIgnoreCase("greater than"))
            {
                var ticketData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetLastOfficialDeltaTicket).ConfigureAwait(false);
                for (int i = 0; i <= ticketData.Count; i++)
                {
                    await this.ReadAllSqlAsDictionaryAsync(SqlQueries.SetDeltaNodeApprover, args: new { ticketID = ticketData["TicketId"] }).ConfigureAwait(false);
                }

                await this.ReadAllSqlAsDictionaryAsync(SqlQueries.ApproveDeltaNodeTicketWithPreviousPeriod, args: new { ticketID = ticketData["TicketId"] }).ConfigureAwait(false);
            }
        }

        [Then(@"Verify that system should send the official movements details in the ""(.*)"" FICO Collection")]
        public async System.Threading.Tasks.Task ThenVerifyThatSystemShouldSendTheOfficialMovementsDetailsInTheFICOCollectionAsync(string collection)
        {
            var officialMovementData = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.OFficialMovementInformationByTicketId, args: new { officialDeltaTicketId = this.GetValue("OfficialDeltaTicketId") }).ConfigureAwait(false);
            var ficoData = JObject.Parse(this.ScenarioContext["json"].ToString());
            Assert.IsNotNull(ficoData[collection]);
            Assert.AreEqual(officialMovementData.Count(), ficoData[collection].Count());
            var officialMovementDataList = officialMovementData.ToDictionaryList();
            var i = 0;
            foreach (var officialMovement in officialMovementDataList.Reverse())
            {
                Assert.AreEqual(officialMovement["MovementTransactionId"].ToString(), ficoData[collection][i].SelectToken("idMovimientoDeltaInv").ToString());
                i++;
            }
        }

        [Then(@"Verify that system should not send the official movements details in the ""(.*)"" FICO Collection")]
        public async System.Threading.Tasks.Task ThenVerifyThatSystemShouldNotSendTheOfficialMovementsDetailsInTheFICOCollectionAsync(string collection)
        {
            var officialMovementData = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.OFficialMovementInformationByTicketId, args: new { officialDeltaTicketId = this.GetValue("OfficialDeltaTicketId") }).ConfigureAwait(false);
            var ficoData = JObject.Parse(this.ScenarioContext["json"].ToString());
            Assert.IsNotNull(ficoData[collection]);
            Assert.AreEqual(officialMovementData.Count(), ficoData[collection].Count());
            var officialMovementDataList = officialMovementData.ToDictionaryList();
            var i = 0;
            foreach (var officialMovement in officialMovementDataList.Reverse())
            {
                Assert.AreNotEqual(officialMovement["MovementTransactionId"].ToString(), ficoData[collection][i].SelectToken("idMovimientoDeltaInv").ToString());
                i++;
            }
        }

        [When(@"a node of the segment ticket is Deltas or Rejected state and has been previously approved or is in reopened state for current period")]
        [When(@"a node of the segment ticket is Deltas or Rejected state and has not been previously approved for current period")]
        public void WhenANodeOfTheSegmentTicketIsDeltasOrRejectedStateAndHasNotBeenPreviouslyApprovedForCurrentPeriod()
        {
            ////Intially kept as a blank
        }

        [When(@"a node of the segment ticket is Deltas or Rejected state and has not been previously approved")]
        public void WhenANodeOfTheSegmentTicketIsDeltasOrRejectedStateAndHasNotBeenPreviouslyApproved()
        {
            //// Not require any updation
        }

        [When(@"The operational date is equal to the end date of the period")]
        public void WhenTheOperationalDateIsEqualToTheEndDateOfThePeriod()
        {
            //// Not require any updation
        }

        [When(@"the created date is ""(.*)"" the last approval date of the node in the ticket period")]
        public void WhenTheCreatedDateIsTheLastApprovalDateOfTheNodeInTheTicketPeriod(string period)
        {
            Assert.IsNotNull(period);
            ////Intially kept as a blank
        }

        [When(@"the start and end dates of the movements are equal to the start and end dates of the period")]
        public void WhenTheStartAndEndDatesOfTheMovementsAreEqualToTheStartAndEndDatesOfThePeriod()
        {
            //// Not require any updation
        }
    }
}
