// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumetricCutoffCheckConsistencySteps.cs" company="Microsoft">
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

    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.StepDefinitions;
    using global::Bdd.Core.Web.Utils;
    using global::Ecp.True.Bdd.Tests.Entities;
    using global::Ecp.True.Bdd.Tests.Properties;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Ocaramba.Types;

    using TechTalk.SpecFlow;

    [Binding]
    public class VolumetricCutoffCheckConsistencySteps : WebStepDefinitionBase
    {
        [StepDefinition(@"I select all transfer point in the grid")]
        [StepDefinition(@"I select all pending records from grid")]
        [StepDefinition(@"I select all unbalances in the grid")]
        [StepDefinition(@"I unselected all transfer point in the grid")]
        public void WhenISelectSomeUnbalances()
        {
            this.ScenarioContext[ConstantValues.PendingTransactions] = 0;
            this.ISelectAllPendingRepositroriesFromGrid();
        }

        [Then(@"I should see the unbalances updated with the information added in the note")]
        public async Task ThenIShouldSeeTheUnbalancesUpdatedWithTheInformationAddedInTheNoteAsync()
        {
            var notes = await this.ReadAllSqlAsync(input: SqlQueries.GetNotesForUnbalances, args: new { nodeId = "123" }).ConfigureAwait(false);
            var notesList = notes.ToDictionaryList();
            foreach (var row in notesList)
            {
                Assert.AreEqual(this.GetValue(Keys.RandomFieldValue), row);
            }
        }

        [Then(@"the list should be refreshed so that the managed unbalances are no longer displayed")]
        public void ThenTheListShouldBeRefreshedSoThatTheManagedUnbalancesAreNoLongerDisplayed()
        {
            this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.NoRecordsFoundMessage));
        }

        [When(@"I should (see "".*"" "".*"") as enabled")]
        [Then(@"I should (see "".*"" "".*"") as enabled")]
        public void ThenIShouldSeeAsEnabled(ElementLocator elementLocator)
        {
            Assert.IsTrue(this.Get<ElementPage>().GetElement(elementLocator).Enabled);
        }

        [Then(@"I should (see "".*"" "".*"") as disabled")]
        public void ThenIShouldSeeAsDisabled(ElementLocator elementLocator)
        {
            Assert.IsFalse(this.Get<ElementPage>().GetElement(elementLocator).Enabled);
        }

        [When(@"I have no unbalances greater than acceptable balance percentage")]
        public void WhenIHaveNoUnbalancesGreaterThanAcceptableBalancePercentage()
        {
            Assert.IsNotNull("Test Data is created to verify this scenario");
        }

        [Then(@"I should see unbalances of the nodes that exceed the ""(.*)"" value configured for each node")]
        public void ThenIShouldSeeUnbalancesOfTheNodesThatExceedTheValueConfiguredForEachNode(string field)
        {
            var desbalancePercentage1 = this.Get<ElementPage>().GetElement(nameof(Resources.DesbalancePercentage1)).Text;
            var desbalancePercentage2 = this.Get<ElementPage>().GetElement(nameof(Resources.DesbalancePercentage2)).Text;
            var acceptableBalancePercentage1 = this.Get<ElementPage>().GetElement(nameof(Resources.AcceptableBalancePercentage1)).Text;
            var acceptableBalancePercentage2 = this.Get<ElementPage>().GetElement(nameof(Resources.AcceptableBalancePercentage2)).Text;
            Assert.IsTrue(desbalancePercentage1.ToDecimal() > acceptableBalancePercentage1.ToDecimal());
            Assert.IsTrue(desbalancePercentage2.ToDecimal() > acceptableBalancePercentage2.ToDecimal());
            Assert.IsNotNull(field);
        }

        [Then(@"I should see all the column data related to it")]
        public async Task ThenIShouldSeeAllTheColumnDataRelatedToItAsync()
        {
            this.SetValue("Origin_NodeId", this.GetValue("NodeId_1"));
            var nodeRowOrigin = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastNode, args: new { nodeId = this.GetValue("Origin_NodeId") }).ConfigureAwait(false);
            var nodeName = this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 2).Text;
            var productName1 = this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 3).Text;
            var unit = this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 6).Text;
            var desbalancePercentage1 = this.Get<ElementPage>().GetElement(nameof(Resources.DesbalancePercentage1)).Text;
            var desbalancePercentage2 = this.Get<ElementPage>().GetElement(nameof(Resources.DesbalancePercentage2)).Text;
            var acceptableBalancePercentage1 = this.Get<ElementPage>().GetElement(nameof(Resources.AcceptableBalancePercentage1)).Text;
            var acceptableBalancePercentage2 = this.Get<ElementPage>().GetElement(nameof(Resources.AcceptableBalancePercentage2)).Text;
            var desbalance1 = this.Get<ElementPage>().GetElement(nameof(Resources.Desbalance1)).Text;
            var desbalance2 = this.Get<ElementPage>().GetElement(nameof(Resources.Desbalance2)).Text;

            Assert.AreEqual(nodeRowOrigin[ConstantValues.Name], nodeName);
            Assert.AreEqual(ConstantValues.ProductName1, productName1);
            Assert.AreEqual(ConstantValues.MeasurementUnit, unit);
            Assert.AreEqual(ConstantValues.DesbalancePercentage1, desbalancePercentage1);
            Assert.AreEqual(ConstantValues.DesbalancePercentage2, desbalancePercentage2);
            Assert.AreEqual(ConstantValues.AcceptableBalancePercentage, acceptableBalancePercentage1);
            Assert.AreEqual(ConstantValues.AcceptableBalancePercentage, acceptableBalancePercentage2);
            Assert.AreEqual(ConstantValues.Desbalance1, desbalance1);
            Assert.AreEqual(ConstantValues.Desbalance2, desbalance2);
        }

        [StepDefinition(@"validate (that "".*"" "".*"") as enabled")]
        public void ThenValidateThatAsEnabled(ElementLocator elementLocator)
        {
            this.ValidateThatAsEnabled(elementLocator);
        }

        [Then(@"validate (that "".*"" "".*"") as disabled")]
        public void ThenValidateThatAsDisabled(ElementLocator elementLocator)
        {
            Assert.IsFalse(this.Get<ElementPage>().GetElement(elementLocator).Enabled);
        }
    }
}