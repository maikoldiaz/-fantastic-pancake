// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageForStartingTheOwnershipCalculationForASegmentSteps.cs" company="Microsoft">
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
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TechTalk.SpecFlow;

    [Binding]
    public class PageForStartingTheOwnershipCalculationForASegmentSteps : EcpWebStepDefinitionBase
    {
        [When(@"I select a value from ""(.*)""")]
        public void WhenISelectAValueFrom(string field)
        {
            this.SelectValueFromDropDown(this.ScenarioContext[ConstantValues.Segment].ToString(), field);
        }

        [Given(@"I have ""(.*)"" created")]
        public async Task GivenIHaveCreatedAsync(string ownershipNodes)
        {
            Assert.IsNotNull(ownershipNodes);
            ////this.Given("I want create TestData for " + ownershipNodes);
            await this.TestDataForOwnershipCalculationAsync().ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = this.GetValue("NodeId_2") }).ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = this.GetValue("NodeId_3") }).ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = this.GetValue("NodeId_4") }).ConfigureAwait(false);
        }

        [Then(@"Verify the message displayed ""(.*)""")]
        public void ThenVerifyTheMessageDisplayed(string error)
        {
            Assert.AreEqual(error, this.Get<ElementPage>().GetElement(nameof(Resources.UploadTypeName), formatArgs: error).Text);
        }

        [Then(@"I verify the Initial Date to be Final Date of last balance with ownership calculated plus one day")]
        public async Task ThenIVerifyTheInitialDateToBeFinalDateOfLastBalanceWithOwnershipCalculatedPlusOneDayAsync()
        {
            var ticketRow = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetOwnerhsipCalcTickets, args: new { CategoryElementName = this.GetValue<string>("CategorySegment") }).ConfigureAwait(false);
            var initialDate = this.Get<ElementPage>().GetElement(nameof(Resources.OwnershipInputDate));
            var initialExpected = DateTime.Parse(ticketRow["StartDate"].ToString(), CultureInfo.InvariantCulture);
            initialExpected.AddDays(1);
            Assert.AreEqual(initialExpected.ToShortDateString(), initialDate);
        }

        [Then(@"verify the ownership is calculated successfully")]
        public async Task ThenVerifyTheOwnershipIsCalculatedSuccessfullyAsync()
        {
            var ticketRow = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetLastTicketAndElement).ConfigureAwait(false);
            this.SetValue(ConstantValues.TicketId, ticketRow[ConstantValues.TicketId].ToString());
            Assert.AreEqual(ticketRow[ConstantValues.Name].ToString(), this.GetValue("Segment"));
        }

        [When(@"I verify all validations passed")]
        public void WhenIVerifyAllValidationsPassed()
        {
            this.IVerifyValidationsPassed();
        }

        [When(@"I navigate to ownershipcalculation page")]
        public void WhenINavigateToOwnershipcalculationPage()
        {
            // step implemetaion is already taken care in other step definition
        }

        [Then(@"I should see ownership calculated segment is processed")]
        public void ThenIShouldSeeOwnershipCalculatedSegmentIsProcessed()
        {
            Assert.IsTrue(this.ScenarioContext[ConstantValues.TicketStatus].ToString().EqualsIgnoreCase("Propiedad"));
        }
    }
}