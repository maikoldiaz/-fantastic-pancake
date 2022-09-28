// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateNodesSteps.cs" company="Microsoft">
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
    using Bogus;

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
    public class UpdateNodesSteps : EcpWebStepDefinitionBase
    {
        [When(@"I update valid ""(.*)"" (into "".*"" "".*"")")]
        public void WhenIUpdateValidInto(string fieldValue, ElementLocator elementLocator)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            fieldValue = fieldValue.EqualsIgnoreCase("Name") ? string.Concat($"Automation_", new Faker().Random.AlphaNumeric(5)) : fieldValue;
            this.Get<ElementPage>().GetElement(elementLocator).Clear();
            this.EnterValueIntoTextBox(elementLocator, fieldValue);
        }

        [When(@"validate (that "".*"" "".*"") is disabled")]
        public void WhenValidateThatIsDisabled(ElementLocator elementLocator)
        {
            Assert.IsFalse(this.Get<ElementPage>().GetElement(elementLocator).Enabled);
        }

        [When(@"I enter ""(.*)"" characters (into "".*"" "".*"")")]
        public void WhenIEnterCharactersInto(int limit, ElementLocator elementLocator)
        {
            string fieldValue = new Faker().Random.AlphaNumeric(limit + 1);
            this.EnterValueIntoTextBox(elementLocator, fieldValue);
            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(Keys.Tab);
        }

        [When(@"I enter ""(.*)"" characters (into "".*"" "".*"") modal")]
        public void WhenIEnterCharactersIntoModal(int limit, ElementLocator elementLocator)
        {
            string fieldValue = new Faker().Random.AlphaNumeric(limit + 1);
            this.Get<ElementPage>().GetElements(elementLocator).ElementAt(1).SendKeys(fieldValue);
            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(Keys.Tab);
        }

        [When(@"I click (on "".*"" "".*"") to '(.*)'")]
        public void WhenIClickOnTo(ElementLocator elementLocator, string status)
        {
            if ((this.Get<ElementPage>().GetElement(elementLocator).GetAttribute("value").EqualsIgnoreCase("true") && status.EqualsIgnoreCase("Inactive")) || (this.Get<ElementPage>().GetElement(elementLocator).GetAttribute("value").EqualsIgnoreCase("false") && status.EqualsIgnoreCase("Active")))
            {
                ////IWebElement element = this.Get<ElementPage>().GetElement(elementLocator).FindElement(By.XPath("//span"));
                this.Get<ElementPage>().ClientClick(elementLocator);
                this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            }
        }

        [When(@"I change existing ""(.*)"" (into "".*"" "".*"")")]
        public async System.Threading.Tasks.Task WhenIChangeExistingIntoAsync(string fieldValue, ElementLocator elementLocator)
        {
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: UIContent.GetRow["FirstNode"]).ConfigureAwait(false);
            fieldValue = lastCreatedRow[fieldValue];
            this.Get<ElementPage>().GetElement(elementLocator).Clear();
            this.EnterValueIntoTextBox(elementLocator, fieldValue);
            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(Keys.Tab);
        }

        [Then(@"validate (that "".*"" "".*"") is ""(.*)""")]
        public void ThenValidateThatIs(ElementLocator elementLocator, string expectedValue)
        {
            if (this.Get<ElementPage>().GetElement(elementLocator).GetAttribute("class").ContainsIgnoreCase("disabled"))
            {
                Assert.AreEqual("disabled", expectedValue);
            }
            else
            {
                Assert.AreEqual("enabled", expectedValue);
            }
        }

        [When(@"I enter new ""(.*)"" (into "".*"" "".*"")")]
        public void WhenIEnterNewInto(string value, ElementLocator elementLocator)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            this.ScenarioContext[value] = value.ContainsIgnoreCase("Name") ? string.Concat($"Automation_", new Faker().Random.AlphaNumeric(5)) : value;
            this.EnterValueIntoTextBox(elementLocator, this.ScenarioContext[value].ToString());
        }

        [When(@"I enter new ""(.*)"" (into "".*"" "".*"") modal")]
        public void WhenIEnterNewIntoModal(string value, ElementLocator elementLocator)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            this.ScenarioContext[value] = value.ContainsIgnoreCase("Name") ? string.Concat($"Automation_", new Faker().Random.AlphaNumeric(5)) : value;
            this.Get<ElementPage>().GetElements(elementLocator).ElementAt(1).SendKeys(this.ScenarioContext[value].ToString());
            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(Keys.Tab);
        }

        [When(@"I clear value (into "".*"" "".*"") modal")]
        public void WhenIClearValueIntoModal(ElementLocator elementLocator)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            this.Get<ElementPage>().GetElements(elementLocator).ElementAt(1).Clear();
            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(Keys.Tab);
        }

        [When(@"I click (on "".*"" "".*"") for (.*) Product")]
        public void WhenIClickOnForProduct(ElementLocator elementLocator, int productNumber)
        {
            this.IClickOnForProduct(elementLocator, productNumber);
        }

        [When(@"I enter new ""(.*)"" (into "".*"" "".*"") and select")]
        public void WhenIEnterNewIntoAndSelect(string value, ElementLocator elementLocator)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            this.ScenarioContext[value] = value.ContainsIgnoreCase("Product") ? UIContent.Conversion[value] : value;
            this.EnterValueIntoTextBox(elementLocator, this.ScenarioContext[value].ToString());
            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(Keys.Enter);
        }

        [When(@"I should see ""(.*)"" (on "".*"" "".*"") for (.*) Product")]
        public void ThenIShouldSeeOnForProduct(string value, ElementLocator elementLocator, int productNumber)
        {
            var productRow = this.Get<ElementPage>().GetElements(elementLocator);
            Assert.AreEqual(value, productRow[productNumber - 1].Text);
        }
    }
}