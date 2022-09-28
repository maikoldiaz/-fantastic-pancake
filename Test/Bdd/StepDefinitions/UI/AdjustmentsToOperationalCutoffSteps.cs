// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdjustmentsToOperationalCutoffSteps.cs" company="Microsoft">
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

    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class AdjustmentsToOperationalCutoffSteps : EcpWebStepDefinitionBase
    {
        [Given(@"I have a segment where operational cutoff is already calculated")]
        public async Task GivenIHaveASegmentWhereOperationalCutoffIsAlreadyCalculatedAsync()
        {
            var operatioanlCutoffTicketDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetTopProcessedCutoffTicket).ConfigureAwait(false);
            this.SetValue("CategorySegment", operatioanlCutoffTicketDetails[ConstantValues.Name]);
            this.SetValue("FinalDate", operatioanlCutoffTicketDetails[ConstantValues.EndDate]);
            this.SetValue("StartDate", operatioanlCutoffTicketDetails[ConstantValues.StartDate]);
            this.SetValue("LastExecutedDate", operatioanlCutoffTicketDetails[ConstantValues.ExecutionDate]);
        }

        [Given(@"I have a product with input value is zero")]
        public async Task GivenIHaveAProductWithInputValueIsZeroAsync()
        {
            ////this.Given("I want create TestData for Operational Cutoff \"Testdata_20215_0Volume\"");
            await this.TestDataForCutOffAsync("Testdata_20215_0Volume").ConfigureAwait(false);
        }

        [Given(@"I have inventory with empty unit field is processed in Excel")]
        [Given(@"I have movement with empty unit field is processed in Excel")]
        public async Task GivenIHaveMovementWithEmptyUnitFieldIsProcessedInExcelAsync()
        {
            this.SetValue(ConstantValues.MeasurementUnit, ConstantValues.Empty);
            ////this.Given("I want create TestData for Operational Cutoff \"Testdata_20215\"");
            await this.TestDataForCutOffAsync("Testdata_20215").ConfigureAwait(false);
        }

        [Given(@"I have movement with empty unit field is processed in Sinoper")]
        public async Task GivenIHaveMovementWithEmptyUnitFieldIsProcessedInSinoperAsync()
        {
            this.SetValue(ConstantValues.MeasurementUnit, ConstantValues.Empty);
            ////this.Given("I want to register an \"Movements\" in the system");
            await this.IWantToRegisterAnInTheSystemAsync("Movements").ConfigureAwait(false);
            ////this.When("it meets \"not all\" input validations");
            await this.ItMeetsAllInputValidationsAsync("not all").ConfigureAwait(false);
            ////this.When("the \"EventType\" field is equal to \"Insert\"");
            this.ScenarioContext[ConstantValues.EventType] = ApiContent.HomologateLogType["Insert"];
        }

        [Given(@"I have inventory with empty unit field is processed in Sinoper")]
        public async Task GivenIHaveInventoryWithEmptyUnitFieldIsProcessedInSinoperAsync()
        {
            this.SetValue(ConstantValues.MeasurementUnitId, string.Empty);
            ////this.Given("I want to register an \"Inventory\" in the system");
            await this.IWantToRegisterAnInTheSystemAsync("Inventory").ConfigureAwait(false);
            ////this.When("it meets \"not all\" input validations");
            await this.ItMeetsAllInputValidationsAsync("not all").ConfigureAwait(false);
            ////this.When("the \"EventType\" field is equal to \"Insert\"");
            this.ScenarioContext[ConstantValues.EventType] = ApiContent.HomologateLogType["Insert"];
        }

        [Then(@"I should see final date of the last operational cutoff executed and the execution date")]
        public void ThenIShouldSeeFinalDateOfTheLastOperationalCutoffExecutedAndTheExecutionDate()
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);
            var endDateOfCutoff = page.GetElement(nameof(Resources.CutoffDateOnOperationalCutoffPages), formatArgs: 1).Text.Split(':')[1].Trim();
            endDateOfCutoff = endDateOfCutoff.Replace(endDateOfCutoff.Split('-')[1], UIContent.OwnershipCalculationGrid[endDateOfCutoff.Split('-')[1]]);
            Assert.AreEqual(this.GetValue("FinalDate"), endDateOfCutoff);
            var lastExecutedDate = page.GetElement(nameof(Resources.CutoffDateOnOperationalCutoffPages), formatArgs: 2).Text.Split(':')[1].Trim();
            lastExecutedDate = lastExecutedDate.Replace(lastExecutedDate.Split('-')[1], UIContent.OwnershipCalculationGrid[lastExecutedDate.Split('-')[1]]);
            Assert.AreEqual(this.GetValue("LastExecutedDate"), lastExecutedDate);
        }

        [When(@"I select FinalDate lessthan CurrentDate")]
        public void WhenISelectFinalDate()
        {
            this.ScenarioContext.Add("EndDate", DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.FinalDatePickerinSegment));
            this.Get<ElementPage>().Click(nameof(Resources.FinalDatePickerinSegment));
            ////this.Get<ElementPage>().GetElement(nameof(Resources.FinalDatePickerinSegment)).SendKeys(this.ScenarioContext["EndDate"].ToString());
            this.Get<ElementPage>().GetElement(nameof(Resources.FinalDatePickerinSegment)).SendKeys(OpenQA.Selenium.Keys.Enter);
        }

        [Then(@"I should see ""(.*)"" in the %Control column for that product")]
        public void ThenIShouldSeeInTheControlColumnForThatProduct(string value)
        {
            Assert.AreEqual(value, this.Get<ElementPage>().GetElement(nameof(Resources.ElementByText), formatArgs: value).Text);
        }

        [Then(@"Then it must be stored in a Pendingtransactions repository with validation ""(.*)""")]
        public async Task ThenThenItMustBeStoredInAPendingtransactionsRepositoryWithValidationAsync(string errorMessage)
        {
            var pendingTransaction = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetTopPendingTransaction).ConfigureAwait(false);
            Assert.AreEqual(errorMessage, pendingTransaction[ConstantValues.ErrorJson].Trim('[', ']', '"'));
        }
    }
}