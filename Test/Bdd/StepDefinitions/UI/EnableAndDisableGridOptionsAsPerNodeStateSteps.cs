// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnableAndDisableGridOptionsAsPerNodeStateSteps.cs" company="Microsoft">
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

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;

    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class EnableAndDisableGridOptionsAsPerNodeStateSteps : EcpWebStepDefinitionBase
    {
        [When(@"I search for a record with ""(.*)"" status")]
        public async System.Threading.Tasks.Task WhenISearchForARecordWithStatusAsync(string status)
        {
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateOwnershipStatusId, args: new { name = UIContent.Conversion[status], ticketId = this.GetValue(ConstantValues.TicketId) }).ConfigureAwait(false);
            this.Get<ElementPage>().GetElement(nameof(Resources.TicketIdFilterInOwnershipCalculationNodeGrid)).SendKeys(this.GetValue(ConstantValues.TicketId));
            this.Get<ElementPage>().GetElement(nameof(Resources.TicketIdFilterInOwnershipCalculationNodeGrid)).SendKeys(OpenQA.Selenium.Keys.Enter);
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
        }

        [When(@"I search for a record with ""(.*)"" status for segments")]
        public async System.Threading.Tasks.Task WhenISearchForARecordWithStatusForSegmentsAsync(string status)
        {
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateOwnershipStatusId, args: new { name = UIContent.Conversion[status], ticketId = this.GetValue(ConstantValues.TicketId) }).ConfigureAwait(false);
            this.Get<ElementPage>().GetElement(nameof(Resources.TicketsGridOwnershipSegment)).SendKeys(this.GetValue(ConstantValues.TicketId));
            this.Get<ElementPage>().GetElement(nameof(Resources.TicketsGridOwnershipSegment)).SendKeys(OpenQA.Selenium.Keys.Enter);
        }

        [Then(@"the state name must be ""(.*)""")]
        public void ThenTheStateNameMustBe(string status)
        {
            Assert.AreEqual(UIContent.Conversion[status], this.Get<ElementPage>().GetElement(nameof(Resources.OwnershipCalculationForNodesGrid), UIContent.OwnershipCalculationGrid["Status"]).Text);
        }

        [Then(@"the state name must be ""(.*)"" for segments")]
        public void ThenTheStateNameMustBeForSegments(string status)
        {
            Assert.AreEqual(UIContent.Conversion[status], this.Get<ElementPage>().GetElement(nameof(Resources.SegementTicketStatus)).Text);
            Assert.IsNotNull(status);
        }

        [When(@"I search for a record with ""(.*)"" status of previous record")]
        public async System.Threading.Tasks.Task WhenISearchForARecordWithStatusOfPreviousRecordAsync(string status)
        {
            var row = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetTicketIdOfPreviousRecord).ConfigureAwait(false);
            this.SetValue(ConstantValues.TicketId, row[ConstantValues.TicketId]);
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateOwnershipStatusId, args: new { name = status, ticketId = this.GetValue(ConstantValues.TicketId) }).ConfigureAwait(false);
            this.Get<ElementPage>().GetElement(nameof(Resources.TicketIdFilterInOwnershipCalculationNodeGrid)).SendKeys(this.GetValue(ConstantValues.TicketId));
            this.Get<ElementPage>().GetElement(nameof(Resources.TicketIdFilterInOwnershipCalculationNodeGrid)).SendKeys(OpenQA.Selenium.Keys.Enter);
        }
    }
}
