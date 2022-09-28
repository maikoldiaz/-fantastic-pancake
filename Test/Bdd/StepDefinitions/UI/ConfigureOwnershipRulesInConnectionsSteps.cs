// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigureOwnershipRulesInConnectionsSteps.cs" company="Microsoft">
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
    using System.Globalization;
    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.StepDefinitions.UI;

    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Ocaramba.Types;

    using OpenQA.Selenium.Interactions;

    using TechTalk.SpecFlow;

    [Binding]
    public class ConfigureOwnershipRulesInConnectionsSteps : EcpWebStepDefinitionBase
    {
        [When(@"I select value again from ""(.*)""")]
        public void WhenISelectValueAgainFrom(string field)
        {
            var dropDownIndex = int.Parse(this.ScenarioContext["DropDownIndex"].ToString(), CultureInfo.InvariantCulture);
            var dropDownBox = this.Get<ElementPage>().GetElements(nameof(Resources.SelectBox), formatArgs: UIContent.Conversion[field]);
            dropDownBox[dropDownIndex - 1].Click();
            //// this.Get<ElementPage>().Click(nameof(Resources.SelectBox), formatArgs: UIContent.Conversion[field]);
            this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.SelectBoxMenu), 5, formatArgs: UIContent.Conversion[field]);
            var connectionGridRow = this.Get<ElementPage>().GetElements(nameof(Resources.SelectBoxOption), formatArgs: UIContent.Conversion[field]);
            var selectOptionElement = connectionGridRow[connectionGridRow.Count - 2];
            this.SetValue(Entities.Keys.SelectedValue, connectionGridRow[connectionGridRow.Count - 2].Text);
            Actions action = new Actions(this.DriverContext.Driver);
            action.MoveToElement(selectOptionElement).Perform();
            selectOptionElement.Click();
        }

        [When(@"I provide invalid value (for "".*"" "".*"")")]
        public void WhenIProvideInvalidValueFor(ElementLocator elementLocator)
        {
            this.SetValue(Entities.Keys.SelectedValue, ConstantValues.InvalidData);
            this.Get<ElementPage>().GetElement(elementLocator).Clear();
            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(this.GetValue(Entities.Keys.SelectedValue));
        }

        [When(@"I provide an integer value in range ""(.*)"" to ""(.*)"" (for "".*"" "".*"")")]
        public void WhenIProvideAnIntegerValueInRangeToFor(int minValue, int maxValue, ElementLocator elementLocator)
        {
            this.SetValue(Entities.Keys.SelectedValue, new Faker().Random.Number(minValue, maxValue).ToString(CultureInfo.InvariantCulture));
            var page = this.Get<ElementPage>();
            page.GetElement(elementLocator).Clear();
            page.GetElement(elementLocator).SendKeys(this.GetValue(Entities.Keys.SelectedValue));
        }

        [When(@"I provide text value different to a positive integer (for "".*"" "".*"")")]
        public void WhenIProvideTextValueDifferentToAPositiveIntegerFor(ElementLocator elementLocator)
        {
            this.SetValue(Entities.Keys.SelectedValue, "TestPurpose#@)*&;");
            var page = this.Get<ElementPage>();
            page.GetElement(elementLocator).Clear();
            page.GetElement(elementLocator).SendKeys(this.GetValue(Entities.Keys.SelectedValue));
        }

        [Then(@"I should not see the text value in ""(.*)""")]
        public void ThenIShouldNotSeeTheTextValueIn(string field)
        {
            Assert.AreEqual(string.Empty, this.Get<ElementPage>().GetElement(nameof(Resources.GetValueFromConnectionsGrid), formatArgs: UIContent.GridPosition[field]).Text);
        }

        [When(@"I click on edit priority link")]
        public void WhenIClickOnEditPriorityLink()
        {
            this.Get<ElementPage>().Click(nameof(Resources.EditProductPriority));
        }

        [Then(@"I should see ""(.*)"" as the title of modal winow")]
        public void ThenIShouldSeeAsTheTitleOfModalWinow(string title)
        {
            if (title != null)
            {
                title = title.ToUpperInvariant();
            }

            Assert.AreEqual(title, this.Get<ElementPage>().GetElement(nameof(Resources.CategoryHeader)).Text.ToUpper(CultureInfo.InvariantCulture));
        }

        [Then(@"I should see the value of ""(.*)"" as ""(.*)""")]
        public void ThenIShouldSeeTheValueOfAs(string field, string value)
        {
            Assert.AreEqual(value, this.Get<ElementPage>().GetElement(nameof(Resources.GetValueFromConnectionsGrid), formatArgs: UIContent.GridPosition[field]).Text.Replace(".", string.Empty));
        }

        [When(@"I enter value greater than ""(.*)"" (for "".*"" "".*"")")]
        public void WhenIEnterValueGreaterThanFor(int value, ElementLocator elementLocator)
        {
            this.SetValue(Entities.Keys.SelectedValue, new Faker().Random.Number(value + 1, value + 10000).ToString(CultureInfo.InvariantCulture));
            var page = this.Get<ElementPage>();
            page.GetElement(elementLocator).Clear();
            page.GetElement(elementLocator).SendKeys(this.GetValue(Entities.Keys.SelectedValue));
        }

        [Then(@"I should not see value greater than ""(.*)"" (in "".*"" "".*"")")]
        public void ThenIShouldNotSeeValueGreaterThanIn(int value, ElementLocator elementLocator)
        {
            Assert.IsTrue(int.Parse(this.Get<ElementPage>().GetElement(elementLocator).GetProperty("value").Replace(".", string.Empty), CultureInfo.InvariantCulture) <= value);
        }

        [Then(@"the changes should be updated in ""(.*)""")]
        public void ThenTheChangesShouldBeUpdatedIn(string field)
        {
            Assert.IsNotNull(field);
            Assert.AreEqual(this.GetValue(Entities.Keys.SelectedValue), this.Get<ElementPage>().GetElement(nameof(Resources.GetValueFromConnectionsGrid), formatArgs: UIContent.GridPosition[field]).Text.Replace(".", string.Empty));
        }

        [Then(@"I should see the error message ""(.*)""")]
        public void ThenIShouldSeeTheErrorMessage(string message)
        {
            Assert.IsNotNull(message);
        }

        [Then(@"I must see ""(.*)"" value (in "".*"" "".*"")")]
        public void ThenIMustSeeValueIn(string field, ElementLocator elementLocator)
        {
            if (field == "blank")
            {
                Assert.AreEqual(string.Empty, this.Get<ElementPage>().GetElement(elementLocator).GetProperty("value"));
            }
            else
            {
                Assert.AreEqual(field, this.Get<ElementPage>().GetElement(elementLocator).GetProperty("value"));
            }
        }

        [Then(@"I should see ""(.*)"" message in modal window")]
        public void ThenIShouldSeeMessageInModalWindow(string message)
        {
            Assert.AreEqual(message, this.Get<ElementPage>().GetElement(nameof(Resources.ErrorMessage)).Text);
        }
    }
}