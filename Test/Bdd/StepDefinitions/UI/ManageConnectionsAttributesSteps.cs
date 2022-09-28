// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManageConnectionsAttributesSteps.cs" company="Microsoft">
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
    using System.Globalization;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using NUnit.Framework;

    using Ocaramba.Types;

    using OpenQA.Selenium;

    using TechTalk.SpecFlow;

    using Keys = OpenQA.Selenium.Keys;

    [Binding]
    public class ManageConnectionsAttributesSteps : EcpWebStepDefinitionBase
    {
        [Then(@"I should see the list of ""(.*)"" that meets the filter conditions")]
        public void ThenIShouldSeeTheListOfThatMeetsTheFilterConditions(string entity)
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            if (page.GetElement(nameof(Resources.GridRow), formatArgs: UIContent.Conversion[entity]).Displayed)
            {
                var connectionGridRow = page.GetElements(nameof(Resources.GridRow), formatArgs: UIContent.Conversion[entity]);
                Assert.IsNotNull(connectionGridRow.Count);
            }
        }

        [StepDefinition(@"I should see (.*) product associations relating to that ""(.*)""")]
        public void ThenIShouldSeeProductAssociationsRelatingToThat(int totalProducts, string entity)
        {
            var connectionGridRow = this.Get<ElementPage>().GetElements(nameof(Resources.GridRow), formatArgs: UIContent.Conversion[entity]);
            Assert.AreEqual(totalProducts, connectionGridRow.Count);
        }

        [StepDefinition(@"I should see a ""(.*)"" belongs to ""(.*)"" in the grid")]
        public void ThenIShouldSeeABelongsToInTheGrid(string field, string entity)
        {
            this.IShouldSeeABelongsToInTheGrid(field, entity);
        }

        [When(@"I enter values for all the ""(.*)"" associated owners so that the sum of them is equal to (.*)")]
        public void WhenIEnterValuesForAllTheAssociatedOwnersSoThatTheSumOfThemIsEqualTo(string entityProduct, int total)
        {
            var ownershipPercentageTextBox = this.Get<ElementPage>().GetElements(nameof(Resources.OwnershipPercentageTextBox), formatArgs: entityProduct.ToCamelCase());
            var numberOfOwnershipPercentageTextBox = ownershipPercentageTextBox.Count;
            int split = total / numberOfOwnershipPercentageTextBox;
            for (int i = 0; i < numberOfOwnershipPercentageTextBox; i++)
            {
                ownershipPercentageTextBox[i].SendKeys(Keys.Tab);
                ownershipPercentageTextBox[i].Clear();
                ownershipPercentageTextBox[i].SendKeys(i == (numberOfOwnershipPercentageTextBox - 1) ? (split + 1).ToString(CultureInfo.InvariantCulture) : split.ToString(CultureInfo.InvariantCulture));
                ownershipPercentageTextBox[i].SendKeys(Keys.Tab);
            }
        }

        [When(@"I provide ""(.*)"" (for "".*"" "".*"") filter")]
        public void WhenIProvideValueForFilter(string field, ElementLocator elementLocator)
        {
            this.EnterValueIntoTextBox(elementLocator, this.ScenarioContext[field].ToString());
            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(Keys.Enter);
        }

        [When(@"I provide ""(.*)"" value (for "".*"" "".*"") filter that doesn't matches with any record")]
        public void WhenIProvideValueForFilterThatDoesnTMatchesWithAnyRecord(string field, ElementLocator elementLocator)
        {
            this.WhenIProvideValueForFilter(field.EqualsIgnoreCase("nodeConnectionId") ? "DummyId" : "DummyName", elementLocator);
        }

        [StepDefinition(@"validate the ""(.*)"" column is displayed in ""(.*)"" grid")]
        public void ThenValidateTheColumnIsDisplayedInGrid(string field, string entity)
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            var connectionGridRow = page.GetElement(nameof(Resources.GridColumn), UIContent.Conversion[entity], UIContent.Conversion[field]);
            Assert.IsTrue(connectionGridRow.Displayed);
        }

        [StepDefinition(@"the ""(.*)"" should be updated (in "".*"" "".*"")")]
        [StepDefinition(@"validate ""(.*)"" (in "".*"" "".*"")")]
        public void ThenTheShouldBeUpdatedIn(string value, ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            Assert.IsTrue(page.GetElement(elementLocator).Text.ContainsIgnoreCase(UIContent.Conversion[value]));
        }

        [StepDefinition(@"validate ""(.*)"" is not shown in ""(.*)"" option")]
        public void ThenValidateIsNotShownInOption(string value, string field)
        {
            var page = this.Get<ElementPage>();
            var dropDownIndex = int.Parse(this.ScenarioContext["DropDownIndex"].ToString(), CultureInfo.InvariantCulture);
            var dropDownBox = page.GetElements(nameof(Resources.SelectBox), formatArgs: UIContent.Conversion[field]);
            dropDownBox[dropDownIndex - 1].Click();
            page.WaitUntilElementToBeClickable(nameof(Resources.SelectBoxMenu), 5, formatArgs: UIContent.Conversion[field]);
            var options = page.GetElements(nameof(Resources.SelectBoxOptions), formatArgs: UIContent.Conversion[field]);
            foreach (var option in options)
            {
                Assert.IsFalse(option.Text.EqualsIgnoreCase(value));
            }
        }

        [StepDefinition(@"I should (see "".*"" "".*"") is empty")]
        public void ThenIShouldSeeIsEmpty(ElementLocator elementLocator)
        {
            Assert.IsTrue(this.Get<ElementPage>().GetElement(elementLocator).Text.EqualsIgnoreCase(string.Empty));
        }
    }
}
