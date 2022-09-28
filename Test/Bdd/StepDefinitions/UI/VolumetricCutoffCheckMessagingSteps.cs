// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumetricCutoffCheckMessagingSteps.cs" company="Microsoft">
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
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.StepDefinitions.UI;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Ocaramba.Types;

    using TechTalk.SpecFlow;

    [Binding]
    public class VolumetricCutoffCheckMessagingSteps : EcpWebStepDefinitionBase
    {
        [When(@"I choose CategoryElement (from "".*"" "".*"" "".*"")")]
        public void WhenISelectCategoryElementFrom(ElementLocator elementLocator)
        {
            this.ISelectCategoryElementFrom(elementLocator);
        }

        [When(@"I choose any CategoryElement (from "".*"" "".*"" "".*"")")]
        public void WhenIChooseAnyCategoryElementFrom(ElementLocator elementLocator)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);
            this.Get<ElementPage>().Click(elementLocator);
            this.Get<ElementPage>().GetElement(nameof(Resources.SelectBoxOptionByValue), ConstantValues.Transporte).Click();
        }

        [When(@"I select the date (in "".*"" "".*"") based on below criteria")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Workaround")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "Workaround")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Workaround")]
        public async Task WhenISelectTheDateInBasedOnBelowCriteriaAsync(ElementLocator elementLocator, Table table)
        {
            var page = this.Get<ElementPage>();
            await Task.Delay(5000).ConfigureAwait(true);

            var dict = table.Rows.ToDictionary(r => r[0], r => r[1]);
            string pageName = dict["page"];
            string dateSelection = dict["dateSelection"];
            int days = Convert.ToInt32(dict["daysLessThen"]);

            if (pageName == "Audit Configuration")
            {
                string draftDate = DateTime.Now.AddDays(-days).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                if (string.Equals(dateSelection, "initial", System.StringComparison.CurrentCultureIgnoreCase))
                {
                    this.ScenarioContext["StartDate"] = DateTime.Now.AddDays(-days).ToString("dd-MMM-y", CultureInfo.InvariantCulture);
                }
                else
                {
                    this.ScenarioContext["FinalDate"] = DateTime.Now.AddDays(-days).ToString("dd-MMM-y", CultureInfo.InvariantCulture);
                }

                page.WaitUntilElementToBeClickable(page.GetElement(elementLocator));
                page.Click(elementLocator);
                page.GetElement(elementLocator).SendKeys(draftDate);
                page.GetElement(elementLocator).SendKeys(OpenQA.Selenium.Keys.Tab);
            }

            if (pageName == "TransactionsAudit")
            {
                string draftDate = DateTime.Now.AddDays(-days).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                if (string.Equals(dateSelection, "initial", System.StringComparison.CurrentCultureIgnoreCase))
                {
                    this.ScenarioContext["StartDate"] = DateTime.Now.AddDays(-days).ToString("dd-MMM-y", CultureInfo.InvariantCulture);
                }
                else
                {
                    this.ScenarioContext["FinalDate"] = DateTime.Now.AddDays(-days).ToString("dd-MMM-y", CultureInfo.InvariantCulture);
                }

                page.WaitUntilElementToBeClickable(page.GetElement(elementLocator));
                page.Click(elementLocator);
                page.GetElement(elementLocator).SendKeys(draftDate);
                page.GetElement(elementLocator).SendKeys(OpenQA.Selenium.Keys.Tab);
            }
        }

        [When(@"I select the FinalDate lessthan ""(.*)"" days from CurrentDate on ""(.*)"" DatePicker")]
        public async Task WhenISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(int days, string type)
        {
            await this.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(days, type).ConfigureAwait(false);
        }

        [When(@"I click (on "".*"" "".*"" "".*"") and didn't choosen category element")]
        public void WhenIClickOnAndDonTChooseCategoryElementAndClickOutsideOfSegmentCombobox(ElementLocator elementLocator)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);
            this.Get<ElementPage>().Click(elementLocator);
            this.Get<ElementPage>().Click(nameof(Resources.SegmentLabel));
        }

        [Then(@"I should see the message ""(.*)""")]
        public void ThenIShouldSeeTheMessage(string message)
        {
            this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ErrorMessage));
            Assert.AreEqual(message, this.Get<ElementPage>().GetElement(nameof(Resources.ErrorMessage)).Text);
        }

        [When(@"I don't enter valid value into ""(.*)"" ""(.*)""")]
        public void WhenIDonTEnterValidValueInto(string field1, string field2)
        {
            Assert.IsNotNull(field1);
            Assert.IsNotNull(field2);
        }

        [When(@"I enter morethan (.*) characters (into "".*"" "".*"")")]
        public void WhenIEnterMorethanCharactersInto(int limit, ElementLocator elementLocator)
        {
            this.IEnterMorethanCharactersInto(limit, elementLocator);
        }
    }
}