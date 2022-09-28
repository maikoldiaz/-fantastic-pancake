// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManageOwnershipRulesThroughCategorySteps.cs" company="Microsoft">
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
    using Ecp.True.Bdd.Tests.Properties;

    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Ocaramba.Types;
    using TechTalk.SpecFlow;

    [Binding]
    public class ManageOwnershipRulesThroughCategorySteps : EcpWebStepDefinitionBase
    {
        [When(@"I select ""(.*)"" from Category combobox")]
        public void WhenISelectFromCategoryCombobox(string field)
        {
            this.Get<ElementPage>().Click(nameof(Resources.SelectBox), formatArgs: UIContent.Conversion[ConstantValues.Category]);
            ////this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.SelectBoxMenu), 5, formatArgs: UIContent.Conversion[field]);
            this.Get<ElementPage>().Click(nameof(Resources.SelectBoxOption), formatArgs: UIContent.Conversion[field]);
        }

        [When(@"I select created OwnershipStrategy element (from "".*"" "".*"" "".*"")")]
        public void WhenISelectCreatedOwnershipStrategyElementFrom(ElementLocator elementLocator)
        {
            this.Get<ElementPage>().GetElement(elementLocator).Click();
            this.Get<ElementPage>().GetElement(nameof(Resources.SelectBoxOptionByValue), this.ScenarioContext["OwnerShipRuleName"]).Click();
        }

        [When(@"I select OwnershipStrategy element (from "".*"" "".*"")")]
        public async Task WhenISelectOwnershipStrategyElementFromAsync(ElementLocator elementLocator)
        {
            var topOwnershipRule = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastOwnershipRule).ConfigureAwait(false);
            this.ScenarioContext["OwnerShipRuleName"] = topOwnershipRule[ConstantValues.Name];
            this.Get<ElementPage>().GetElement(elementLocator).Click();
            this.Get<ElementPage>().GetElement(nameof(Resources.SelectBoxOptionByValue), this.ScenarioContext["OwnerShipRuleName"]).Click();
        }

        [Then(@"I should be able to select ""(.*)"" from Category combobox")]
        public void ThenIShouldBeAbleToSelectFromCategoryCombobox(string rule)
        {
            Assert.IsTrue(this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.SelectBoxOption), formatArgs: UIContent.Conversion[rule]));
        }

        [Then(@"I should see empty value for ownership strategy in the grid")]
        public void ThenIShouldSeeEmptyValueForOwnershipStrategyInTheGrid()
        {
            Assert.AreEqual(string.Empty, this.Get<ElementPage>().GetElement(nameof(Resources.GetValueFromGrid), UIContent.GridPosition[ConstantValues.OwnershipRule]).Text);
        }

        [Then(@"I should see selected OwnershipStrategy element in the Grid")]
        public void ThenIShouldSeeSelectedOwnershipStrategyElementInTheGrid()
        {
            Assert.AreEqual(this.ScenarioContext["OwnerShipRuleName"], this.Get<ElementPage>().GetElement(nameof(Resources.GetValueFromGrid), UIContent.GridPosition[ConstantValues.OwnershipRule]).Text);
        }

        [Then(@"I should see selected OwnershipStrategy element under actual OwnershipStrategy label")]
        public void ThenIShouldSeeSelectedOwnershipStrategyElementUnderActualOwnershipStrategyLabel()
        {
            Assert.AreEqual(this.ScenarioContext["OwnerShipRuleName"], this.Get<ElementPage>().GetElement(nameof(Resources.ActualOwnershipStrategy), UIContent.GridPosition[ConstantValues.Description]).Text);
        }

        [Then(@"I should see title for the only rule column as ""(.*)""")]
        public void ThenIShouldSeeTitleForTheOnlyRuleColumnAs(string columnName)
        {
            Assert.AreEqual(UIContent.Conversion[columnName], this.Get<ElementPage>().GetElement(nameof(Resources.ElementByText), UIContent.Conversion[columnName]).Text);
        }
    }
}
