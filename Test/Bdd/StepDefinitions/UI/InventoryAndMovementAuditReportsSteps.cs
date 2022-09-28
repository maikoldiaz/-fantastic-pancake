// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryAndMovementAuditReportsSteps.cs" company="Microsoft">
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
    using System.Threading.Tasks;
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

    [Binding]
    public class InventoryAndMovementAuditReportsSteps : EcpWebStepDefinitionBase
    {
        private string exceptionMessage;

        [Then(@"I should see ""(.*)"" page")]
        public void ThenIShouldSeePage(ElementLocator pageItem)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            Assert.IsTrue(this.Get<ElementPage>().GetElement(pageItem).Displayed);
        }

        [Then(@"I should see ""(.*)"" radio button selected")]
        public void ThenIShouldSeeRadioSelected(string optionselected)
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            Assert.IsTrue(this.Get<ElementPage>().GetElement(optionselected).Selected);
        }

        [Then(@"I should see movements radio button checked by default")]
        public void ThenIShouldSeeMovementsRadioButtonCheckedByDefault()
        {
            var page = this.Get<ElementPage>();
            Assert.True(page.GetElement(nameof(Resources.MovementRadiobtnChecked)).Selected);
        }

        [When(@"I click on ""(.*)"" RadioButton")]
        public void WhenIClickOnRadioButton(string radioButton)
        {
            var page = this.Get<ElementPage>();
            if (string.Equals(radioButton, "Inventarios", System.StringComparison.CurrentCultureIgnoreCase))
            {
                page.Click(nameof(Resources.InventoryRadio));
            }
            else
            {
                page.Click(nameof(Resources.MovementRadio));
            }
        }

        [Given(@"I have inactive segment ""(.*)""")]
        public async Task GivenIHaveInactiveSegmentAsync(string name)
        {
            var temp = this.UserDetails;
            string categoryElementName = null;
            if (name.EqualsIgnoreCase("Random"))
            {
                categoryElementName = this.ScenarioContext["CategoryElementName"].ToString();
            }

            if (this.UserDetails.Role == "admin" || this.UserDetails.Role == "administrador")
            {
                ////this.Given("I am authenticated as \"admin\"");
                await this.UIAuthenticationForUserAsync("admin").ConfigureAwait(false);
            }

            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateActiveElementToInactive, args: new { name = categoryElementName }).ConfigureAwait(false);
            ////var inactiveElement = await this.ReadAllSqlAsync(input: SqlQueries.GetInActiveCategoryElements).ConfigureAwait(false);
        }

        [When(@"I should not see inactive segment ""(.*)"" (from "".*"" "".*"")")]
        public void WhenIShouldNotSeeInactiveSegmentFrom(string value, ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            string categoryElementName = null;

            if (elementLocator is null)
            {
                throw new System.ArgumentNullException(nameof(elementLocator));
            }

            if (value.EqualsIgnoreCase("Random"))
            {
                categoryElementName = this.ScenarioContext["CategoryElementName"].ToString();
            }

            page.Click(elementLocator);

            try
            {
                page.Click(nameof(Resources.SelectBoxOptionByValue), categoryElementName);
            }
            catch (Exception ex)
            {
                this.exceptionMessage = ex.ToString();
            }

            Assert.IsTrue(this.exceptionMessage.Contains("NoSuchElementException"));
        }
    }
}