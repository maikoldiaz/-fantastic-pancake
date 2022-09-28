// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContractRuleConfigurationAtNodeProductLevelSteps.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Ocaramba.Types;
    using TechTalk.SpecFlow;
    using Keys = OpenQA.Selenium.Keys;

    [Binding]
    public class ContractRuleConfigurationAtNodeProductLevelSteps : EcpWebStepDefinitionBase
    {
        [StepDefinition(@"I have node product without configuration strategy")]
        public void WhenIHaveNodeProductWithoutConfigurationStrategy()
        {
            this.LogToReport("Covered in previous step");
        }

        [StepDefinition(@"I should not see any value in ""(.*)"" column")]
        public void WhenIShouldNotSeeAnyValueInColumn(string columnName)
        {
            Assert.AreEqual(string.Empty, this.Get<ElementPage>().GetElement(nameof(Resources.GetValueFromNodeProductsGrid), UIContent.GridPosition[columnName]).Text);
        }

        [StepDefinition(@"I select mutiple variables ""(.*)"" and ""(.*)"" (from "".*"" "".*"")")]
        public void WhenISelectMutipleVariablesFrom(string firstValue, string secondValue, ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            page.Click(elementLocator);
            page.Click(nameof(Resources.SelectBoxOptionByValue), firstValue);
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            page.Click(elementLocator);
            page.Click(nameof(Resources.SelectBoxOptionByValue), secondValue);
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
        }

        [StepDefinition(@"I should see ""(.*)"" and ""(.*)"" as values in ""(.*)"" column separated by semicolon")]
        public void ThenIShouldSeeAndAsValuesInColumnSeparatedBySemicolon(string value1, string value2, string columnName)
        {
            string variableValue = this.Get<ElementPage>().GetElement(nameof(Resources.GetValueFromNodeProductsGrid), UIContent.GridPosition[columnName]).Text;
            Assert.IsTrue(variableValue.EqualsIgnoreCase(value2 + "; " + value1) || variableValue.EqualsIgnoreCase(value1 + "; " + value2));
        }

        [StepDefinition(@"I enter node name (into "".*"" "".*"")")]
        public async Task WhenIEnterNodeNameIntoAsync(ElementLocator elementLocator)
        {
            var nodeName = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetNodeNameFromNodeId, args: new { nodeId = this.ScenarioContext["NodeId"].ToString() }).ConfigureAwait(false);
            this.EnterValueIntoTextBox(elementLocator, nodeName["Name"].ToString());
            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(Keys.Enter);
            this.ScenarioContext["nodeName"] = nodeName["Name"];
        }

        [StepDefinition(@"I select ownershipstrategy (from "".*"" "".*"")")]
        public async Task WhenISelectOwnershipstrategyFromAsync(ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            var ownershipStrategyInformation = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetNodeProductOwnershipStrategyInformation).ConfigureAwait(false);
            page.Click(elementLocator);
            page.Click(nameof(Resources.SelectBoxOptionByValue), ownershipStrategyInformation[ConstantValues.OwnershipStrategyInformation]);
        }

        [StepDefinition(@"I should see product is updated with the selected variable")]
        public async Task ThenIShouldSeeProductIsUpdatedWithTheSelectedVariableAsync()
        {
            var count = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetVariablesCountOfNode, args: new { nodeName = this.ScenarioContext["nodeName"] }).ConfigureAwait(false);
            int countOfVariable = count["count"].ToInt();
            Assert.IsTrue(countOfVariable > 0);
        }

        [StepDefinition(@"I should see existing values ""(.*)"" ""(.*)"" (in "".*"" "".*"")")]
        public void ThenIShouldSeeExistingValuesIn(string value1, string value2, ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            string value = page.GetElement(elementLocator).Text;
            Assert.IsTrue(value.Contains(value1) && value.Contains(value2));
        }

        [StepDefinition(@"I select ownership strategy as ""(.*)"" (from "".*"" "".*"")")]
        public void ThenISelectOwnershipStrategyAs(string value, ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            page.Click(elementLocator);
            page.Click(nameof(Resources.SelectBoxOptionByValue), value);
        }

        [StepDefinition(@"I should not see value of ""(.*)"" as ""(.*)"" (in "".*"" "".*"")")]
        public void ThenIShouldNotSeeValueInVariablesList(string field, string value, ElementLocator elementLocator)
        {
            this.Get<ElementPage>().Click(elementLocator);
            var actualRules = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenu), formatArgs: field);
            Assert.IsFalse(actualRules.Any(rule => rule.Contains(value)));
        }

        [StepDefinition(@"I see below ""(.*)"" list from ""(.*)"" dropdown (in "".*"" "".*"")")]
        public void ThenISeeBelowListFromDropdownIn(string listValue, string field, ElementLocator elementLocator, Table table)
        {
            this.Get<ElementPage>().Click(elementLocator);
            var variblesList = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenu), formatArgs: field);
            foreach (var row in table?.Rows.Select((value) => (Default: value[listValue], Unused: string.Empty)))
            {
                Assert.IsTrue(variblesList.Any(rule => rule.Contains(row.Default)));
            }
        }
    }
}