// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterPageForSettingsAuditReportSteps.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Globalization;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Ocaramba.Types;
    using OpenQA.Selenium;
    using TechTalk.SpecFlow;

    [Binding]
    public class FilterPageForSettingsAuditReportSteps : EcpWebStepDefinitionBase
    {
        [Then(@"I should see error message ""(.*)"" below each field on ""(.*)"" page")]
        public void ThenIShouldSeeErrorMessageBelowEachFieldOnPage(string expectedErrorMessage, string pageName)
        {
            int expectedElementsCountBasedOnPage = string.Equals(pageName, "Audit Configuration", System.StringComparison.CurrentCultureIgnoreCase) ? 2 : 3;
            bool errorComparisionFailed = false;
            var errorMessageElements = this.Get<ElementPage>().GetElements(nameof(Resources.ErrorMessageinSegment));
            Assert.AreEqual(expectedElementsCountBasedOnPage, errorMessageElements.Count);
            foreach (IWebElement errorMessageElement in errorMessageElements)
            {
                if (!string.Equals(errorMessageElement.Text.Trim(), expectedErrorMessage, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    errorComparisionFailed = true;
                    Console.WriteLine("Comparision of error text failed. Expected : '" + expectedErrorMessage + "', Actual : '" + errorMessageElement.Text.Trim() + "'");
                }
            }

            Assert.AreEqual(false, errorComparisionFailed, "Validation of error message failed");
        }

        [Then(@"validate previously selected ""(.*)"" date (in "".*"" "".*"") on the filters page")]
        public void ThenValidatePreviouslySelectedDateInOnTheFiltersPage(string dateType, ElementLocator elementLocator)
        {
            string previouslySelectedDate = null;

            if (dateType == "initial")
            {
                previouslySelectedDate = this.ScenarioContext["StartDate"].ToDateTime().ToString("dd-MMM-yyyy", CultureInfo.CreateSpecificCulture("es-CO")).Split('-')[1].Trim('.');
            }
            else
            {
                previouslySelectedDate = this.GetValue("FinalDate").ToDateTime().ToString("dd-MMM-yyyy", CultureInfo.CreateSpecificCulture("es-CO")).Split('-')[1].Trim('.');
            }

            ////this.ScenarioContext["previouslySelectedDateinBogota"] = previouslySelectedDate.ToString("dd-MMM-yyyy", CultureInfo.CreateSpecificCulture("es-CO")).Split('-')[1].Trim('.');
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);
            string currentDateFromUI = this.Get<ElementPage>().GetElement(elementLocator).GetAttribute("value").Trim();
            Assert.IsTrue(previouslySelectedDate.ContainsIgnoreCase(currentDateFromUI.Split('-')[1]));
        }

        [Then(@"I validate that the date (in "".*"" "".*"") should be enabled until one day before the current date")]
        public void ThenIValidateThatTheDateInShouldBeEnabledUntilOneDayBeforeTheCurrentDate(ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);

            page.WaitUntilElementToBeClickable(page.GetElement(elementLocator));
            page.Click(elementLocator);

            IList<IWebElement> dateElements = this.Get<ElementPage>().GetElements(nameof(Resources.DateElementsOnCalendar));
            bool validateCurrentElement = false;

            foreach (IWebElement dateElement in dateElements)
            {
                if (dateElement.GetAttribute("class").Contains("today"))
                {
                    validateCurrentElement = true;
                }

                if (validateCurrentElement)
                {
                    Assert.AreEqual(true, dateElement.GetAttribute("class").Contains("disabled"));
                }
                else
                {
                    Assert.AreEqual(true, dateElement.Enabled);
                }
            }

            page.Click(nameof(Resources.NavigationBar));
        }

        [Then(@"I should or should not see ""(.*)"" page based on user ""(.*)""")]
        public void ThenIShouldOrShouldNotSeePageBasedOnUser(string pageName, string user)
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);

            switch (user)
            {
                case "admin":
                    page.Click(nameof(Resources.NavigationBar));

                    page.ScrollIntoView(nameof(Resources.PageIdentifier), formatArgs: UIContent.Navigation[pageName]);
                    page.WaitUntilElementToBeClickable(nameof(Resources.PageIdentifier), 5, formatArgs: UIContent.Navigation[pageName]);
                    page.Click(nameof(Resources.PageIdentifier), formatArgs: UIContent.Navigation[pageName]);
                    page.ScrollIntoView(nameof(Resources.PageIdentifier), formatArgs: UIContent.Conversion[pageName]);

                    Assert.AreEqual(true, page.CheckIfElementIsPresent(nameof(Resources.PageIdentifier), formatArgs: UIContent.Navigation[pageName]), "'" + user + "' user is expected to have access to '" + pageName + "' page but its not visible");
                    break;
                case "audit":
                    if (pageName != null && pageName.Equals("Segment configuration in TRUE as SON", System.StringComparison.OrdinalIgnoreCase))
                    {
                        page.Click(nameof(Resources.NavigationBar));
                        page.Click(nameof(Resources.PageIdentifier), formatArgs: UIContent.Navigation[pageName]);
                        Assert.AreEqual(false, page.CheckIfElementIsPresent(nameof(Resources.PageIdentifier), formatArgs: UIContent.Conversion[pageName]), "Validation of no acceess to '" + pageName + "' page for user '" + user + "' failed");
                    }
                    else
                    {
                        page.Click(nameof(Resources.NavigationBar));

                        page.ScrollIntoView(nameof(Resources.PageIdentifier), formatArgs: UIContent.Navigation[pageName]);
                        page.WaitUntilElementToBeClickable(nameof(Resources.PageIdentifier), 5, formatArgs: UIContent.Navigation[pageName]);
                        page.Click(nameof(Resources.PageIdentifier), formatArgs: UIContent.Navigation[pageName]);
                        page.ScrollIntoView(nameof(Resources.PageIdentifier), formatArgs: UIContent.Conversion[pageName]);

                        Assert.AreEqual(true, page.CheckIfElementIsPresent(nameof(Resources.PageIdentifier), formatArgs: UIContent.Navigation[pageName]), "'" + user + "' user is expected to have access to '" + pageName + "' page but its not visible");
                    }

                    break;
                case "aprobador":
                case "consulta":
                    page.Click(nameof(Resources.NavigationBar));
                    Assert.AreEqual(false, page.CheckIfElementIsPresent(nameof(Resources.PageIdentifier), formatArgs: UIContent.Navigation[pageName]), "Validation of no acceess to '" + pageName + "' page for user '" + user + "' failed");
                    break;
                case "profesional":
                    page.Click(nameof(Resources.NavigationBar));
                    page.Click(nameof(Resources.PageIdentifier), formatArgs: UIContent.Navigation[pageName]);
                    Assert.AreEqual(false, page.CheckIfElementIsPresent(nameof(Resources.PageIdentifier), formatArgs: UIContent.Conversion[pageName]), "Validation of no acceess to '" + pageName + "' page for user '" + user + "' failed");
                    break;
                case "programador":
                    Assert.AreEqual(false, page.CheckIfElementIsPresent(nameof(Resources.NavigationBar)), "Validation of no acceess to '" + pageName + "' page for user '" + user + "' failed");
                    break;
            }
        }
    }
}
