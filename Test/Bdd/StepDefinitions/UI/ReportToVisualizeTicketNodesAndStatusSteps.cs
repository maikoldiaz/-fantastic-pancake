// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportToVisualizeTicketNodesAndStatusSteps.cs" company="Microsoft">
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
    using Ocaramba.Types;
    using TechTalk.SpecFlow;

    [Binding]
    public class ReportToVisualizeTicketNodesAndStatusSteps : EcpWebStepDefinitionBase
    {
        [StepDefinition(@"I select segment with ownership (from "".*"" "".*"")")]
        public async Task WhenIselectSegmentWithOwnershipFromAsync(ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            var segment = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetSegmentWithOwnership).ConfigureAwait(false);
            page.Click(elementLocator);
            page.Click(nameof(Resources.SelectBoxOptionByValue), segment[ConstantValues.Name]);
        }

        [StepDefinition(@"I enter end date (into "".*"" "".*"")")]
        public async Task WhenIEnterEndDateIntoAsync(ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            var date = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetEnddateOfTicket).ConfigureAwait(false);
            if (date[ConstantValues.Date].ToDateTime().Date > DateTime.Now.Date)
            {
                this.ScenarioContext["EndDate"] = DateTime.Now.ToShortDateString();
            }
            else
            {
                this.ScenarioContext["EndDate"] = date[ConstantValues.Date];
            }

            var endDate = this.ScenarioContext["EndDate"].ToString().ToDateTime().ToShortDateString();
            this.EnterValueIntoTextBox(elementLocator, endDate);
            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(OpenQA.Selenium.Keys.Enter);
        }

        [StepDefinition(@"I enter start date less than (.*) days (into "".*"" "".*"")")]
        public void WhenIEnterStartDateLessThanDaysInto(int days, ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            var startDate = this.ScenarioContext["EndDate"].ToString().ToDateTime().AddDays(-(days + 1)).ToShortDateString();
            this.EnterValueIntoTextBox(elementLocator, startDate);
            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(OpenQA.Selenium.Keys.Enter);
        }

        [StepDefinition(@"I enter start date (in "".*"" "".*"")")]
        public async Task WhenIEnterStartDateGreaterThanEndDateInAsync(ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            var date = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetStartdateOfTicket).ConfigureAwait(false);
            this.ScenarioContext["StartDate"] = date[ConstantValues.Date];
            var startDate = this.ScenarioContext["StartDate"].ToString().ToDateTime().ToShortDateString();
            this.EnterValueIntoTextBox(elementLocator, startDate);
            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(OpenQA.Selenium.Keys.Enter);
        }

        [StepDefinition(@"I enter end date less than start date (into "".*"" "".*"")")]
        public void WhenIEnterEndDateLessThanStartDateInto(ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            var endDate = this.ScenarioContext["StartDate"].ToString().ToDateTime().AddDays(-1).ToShortDateString();
            this.EnterValueIntoTextBox(elementLocator, endDate);
            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(OpenQA.Selenium.Keys.Enter);
        }

        [StepDefinition(@"I enter end date more than current date  (into "".*"" "".*"")")]
        public async Task WhenIEnterDateAsDayMoreThanCurrentDateIntoAsync(ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            var endDate = DateTime.Now.ToDateTime().AddDays(+1).ToShortDateString();
            await this.ReadSqlAsStringDictionaryAsync(SqlQueries.UpateTicketEndDate).ConfigureAwait(false);
            this.EnterValueIntoTextBox(elementLocator, endDate);
            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(OpenQA.Selenium.Keys.Enter);
        }
    }
}