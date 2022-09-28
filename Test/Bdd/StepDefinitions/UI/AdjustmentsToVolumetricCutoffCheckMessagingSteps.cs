// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdjustmentsToVolumetricCutoffCheckMessagingSteps.cs" company="Microsoft">
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
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.StepDefinitions.UI;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using NUnit.Framework;

    using Ocaramba.Types;

    using TechTalk.SpecFlow;

    [Binding]
    public class AdjustmentsToVolumetricCutoffCheckMessagingSteps : EcpWebStepDefinitionBase
    {
        [Given(@"I am having pending records in Operational Cutoff page")]
        public async Task GivenIAmHavingPendingRecordsInOperationalCutoffAsync()
        {
            await this.TestDataForOperationalCutOffAsync().ConfigureAwait(false);
        }

        [When(@"I perform calculation for OperationalBalance in the system")]
        public async Task WhenIPerformCalculationForOperationalBalanceInTheSystemAsync()
        {
            await this.TestDataForOperationalCutOffAsync().ConfigureAwait(false);
            await this.TestDataForCalculatedMovementsAsync().ConfigureAwait(false);
        }

        [Then(@"I should see the label ""(.*)""")]
        public void ThenIShouldSeeTheLabel(string confirmationLabel)
        {
            Assert.AreEqual(confirmationLabel, this.Get<ElementPage>().GetElement(nameof(Resources.ConfirmationLabel)).Text);
        }

        [Then(@"I should see selected values in the labels Category, CategoryElement and Period")]
        public void ThenIShouldSeeSelectedValuesInTheLabelsAnd()
        {
            Assert.AreEqual(UIContent.Conversion["Segment"], this.Get<ElementPage>().GetElement(nameof(Resources.LabelsOnConfirmationInterface), formatArgs: UIContent.Conversion["CategoryLabelOnConfirmation"]).Text);
            Assert.AreEqual(this.GetValue("SegmentName"), this.Get<ElementPage>().GetElement(nameof(Resources.LabelsOnConfirmationInterface), formatArgs: UIContent.Conversion["SegmentLabelOnConfirmation"]).Text);
            var expectedPeriod = this.ScenarioContext["StartDate"].ToString().Split('/')[0] + " " +
                                 UIContent.Conversion[this.ScenarioContext["StartDate"].ToString().Split('/')[1]] + " " +
                                 this.ScenarioContext["StartDate"].ToString().Split('/')[2].Trim('2', '0') + " al " +
                                 this.ScenarioContext["FinalDate"].ToString().Split('/')[0] + " " +
                                 UIContent.Conversion[this.ScenarioContext["FinalDate"].ToString().Split('/')[1]] + " " +
                                 this.ScenarioContext["FinalDate"].ToString().Split('/')[2].Trim('2', '0');
            var actualPeriod = this.Get<ElementPage>().GetElement(nameof(Resources.PeriodLabelOnConfirmationInterface), formatArgs: UIContent.Conversion["PeriodLabel1OnConfirmation"]).Text + " " +
                               this.Get<ElementPage>().GetElement(nameof(Resources.PeriodLabelOnConfirmationInterface), formatArgs: UIContent.Conversion["PeriodLabel2OnConfirmation"]).Text + " " +
                               this.Get<ElementPage>().GetElement(nameof(Resources.PeriodLabelOnConfirmationInterface), formatArgs: UIContent.Conversion["PeriodLabel3OnConfirmation"]).Text;
            Assert.AreEqual(expectedPeriod, actualPeriod);
        }

        [Then(@"I should see the ""(.*)"" page")]
        public void ThenIShouldSeeThePage(string page)
        {
            this.IShouldSeeTheOperationalCutoffPage(page);
        }

        [Then(@"I should see the message ""(.*)"" in the Page")]
        public void ThenIShouldSeeTheMessageInThePage(string message)
        {
            this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ErrorMessageInInicioTab));
            Assert.AreEqual(message, this.Get<ElementPage>().GetElement(nameof(Resources.ErrorMessageInInicioTab)).Text);
        }

        [Then(@"I should see the message ""(.*)"" when there are no pending records")]
        public void ThenIShouldSeeTheMessageWhenThereAreNoPendingRecords(string message)
        {
            this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.NoRecordsFoundMessage));
            Assert.AreEqual(message, this.Get<ElementPage>().GetElement(nameof(Resources.NoRecordsFoundMessage)).Text);
        }

        [Then(@"I shouldn't see any pending transactions on grid")]
        public void ThenIShouldnTSeeAnyPendingTransactionsOnGrid()
        {
            ////this.Then("I should see the message \"Sin registros\" when there are no pending records");
            this.ThenIShouldSeeTheMessageWhenThereAreNoPendingRecords(ConstantValues.SinRegistros);
        }

        [When(@"I choose CategoryElement (from "".*"" "".*"" "".*"") which does not have movements and inventories in the selected period")]
        public void WhenIChooseCategoryElementFromWhichDoesNotHaveMovementsAndInventoriesInTheSelectedPeriod(ElementLocator elementLocator)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);
            this.Get<ElementPage>().Click(elementLocator);
            ////var segmentWithoutMovementsAndInventory = ((IDictionary<string, object>)await this.Output.ReadAsync<dynamic>(input: SqlQueries.GetSegmentNamewithoutMovementsandInventories).ConfigureAwait(false)).ToStringDictionary();
            this.Get<ElementPage>().GetElement(nameof(Resources.SelectBoxOptionByValue), UIContent.Conversion["SegmentName"]).Click();
        }

        public async Task TestDataForOperationalCutOffAsync()
        {
            ////this.Given("I want create TestData for ownershipnodes");
            await this.TestDataForOwnershipCalculationAsync().ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = this.GetValue("NodeId_2") }).ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = this.GetValue("NodeId_3") }).ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = this.GetValue("NodeId_4") }).ConfigureAwait(false);
            await Task.Delay(10000).ConfigureAwait(true);
        }

        public async Task TestDataForCalculatedMovementsAsync()
        {
            ////this.When("I navigate to \"Operational Cutoff\"  page");
            this.UiNavigation("Operational Cutoff");
            ////this.When("I click on \"NewCut\"  \"button\"");
            this.IClickOn("NewCut", "button");
            ////this.Then("I should see \"Start\"  \"link\"");
            this.IShouldSee("Start", "link");
            ////this.When("I choose CategoryElement from \"InitTicket\"  \"Segment\"  \"combobox\"");
            this.ISelectCategoryElementFrom("InitTicket\" \"Segment", "combobox");
            ////this.When("I select the FinalDate lessthan \"4\" days from CurrentDate on \"Cutoff\" DatePicker");
            await this.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(4, "Cutoff").ConfigureAwait(false);
            ////this.When("I click on \"InitTicket\"  \"next\"  \"button\"");
            this.IClickOn("InitTicket\"  \"next", "button");
            ////this.Then("validate that \"checkInventories\" \"Next\" \"button\" as enabled");
            this.ValidateThatAsEnabled("validateInitialInventory\" \"submit", "button");
            ////this.When("I click on \"checkInventories\" \"Next\" \"button\"");
            this.IClickOn("validateInitialInventory\" \"submit", "button");
            this.ScenarioContext[ConstantValues.PendingTransactions] = 0;
            ////this.When("I select all pending records from grid");
            this.ISelectAllPendingRepositroriesFromGrid();
            ////this.When("I click on \"ErrorsGrid\"  \"AddNote\"  \"button\"");
            this.IClickOn("ErrorsGrid\"  \"AddNote", "button");
            ////this.Then("I should see \"Add Note Functions\"  interface");
            this.IShouldSeeInterface("Add Note Functions");
            ////this.When("I provide value for \"AddComment\"  \"comment\"  \"textbox\"");
            this.IProvideValueFor("AddComment\"  \"comment", "textbox");
            ////this.When("I click on \"AddComment\"  \"submit\"  \"button\"");
            this.IClickOn("AddComment\"  \"submit", "button");
            ////this.Then("I should see the message \"No existen movimientos e inventarios pendientes\"  when there are no pending records");
            this.ThenIShouldSeeTheMessageWhenThereAreNoPendingRecords("No existen movimientos e inventarios pendientes");
            this.ScenarioContext[ConstantValues.PendingTransactions] = 0;
            ////this.And("I select all unbalances in the grid");
            this.ISelectAllPendingRepositroriesFromGrid();
            ////this.And("I click on \"consistencyCheck\"  \"AddNote\"  \"button\"");
            this.IClickOn("consistencyCheck\"  \"AddNote", "button");
            ////this.Then("I should see \"Note for imbalances\"  interface");
            this.IShouldSeeInterface("Note for imbalances");
            ////this.When("I provide value for \"AddComment\"  \"comment\"  \"textbox\"");
            this.IProvideValueFor("AddComment\"  \"comment", "textbox");
            ////this.And("I click on \"AddComment\"  \"submit\"  \"button\"");
            this.IClickOn("AddComment\"  \"submit", "button");
            ////this.And("I click on \"ConsistencyCheck\"  \"Next\"  \"button\"");
            this.IClickOn("unbalancesGrid\" \"submit", "button");
            ////this.Then("I should see \"Confirmation\"  interface");
            this.IShouldSeeInterface("Confirmation");
            ////this.And("I should see the label \"Confirmación de Ejecución de Corte Operativo");
            Assert.AreEqual("Confirmación de Ejecución de Corte Operativo", this.Get<ElementPage>().GetElement(nameof(Resources.ConfirmationLabel)).Text);
            ////this.And("I should see selected values in the labels Category, CategoryElement and Period");
            this.ThenIShouldSeeSelectedValuesInTheLabelsAnd();
            ////this.When("I click on \"Confirm\"  \"Cutoff\"  \"Submit\"  \"button\"");
            this.IClickOn("ConfirmCutoff\" \"Submit", "button");
            ////this.Then("I should see the \"Operational Cutoffs\" page");
            this.IShouldSeeTheOperationalCutoffPage("Operational Cutoffs");
            this.ScenarioContext.Add(ConstantValues.TicketId, this.Get<ElementPage>().GetElement(nameof(Resources.LatestOperationalCutoffTicket)).Text);
            await Task.Delay(10000).ConfigureAwait(true);
        }
    }
}