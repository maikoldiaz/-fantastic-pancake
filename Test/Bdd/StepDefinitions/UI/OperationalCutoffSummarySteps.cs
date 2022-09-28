// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationalCutoffSummarySteps.cs" company="Microsoft">
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
    public class OperationalCutoffSummarySteps : EcpWebStepDefinitionBase
    {
        [Then(@"I should (see "".*"" "".*"") and capture it")]
        public void ThenIShouldSeeAndCaptureIt(ElementLocator elementLocator)
        {
            var element = this.Get<ElementPage>().GetElement(elementLocator);
            Assert.IsTrue(element.Displayed);
            this.ScenarioContext["TicketId"] = element.Text;
        }

        [Then(@"enter ""(.*)"" (into "".*"" "".*"")")]
        public void ThenEnterInto(string value, ElementLocator elementLocator)
        {
            this.EnterValueIntoTextBox(elementLocator, value);
        }

        [Then(@"validate ""(.*)"" and ""(.*)"" records displayed as expected (in "".*"" "".*"") chart")]
        public async System.Threading.Tasks.Task ThenValidateAndRecordsDisplayedAsExpectedInAsync(string field1, string field2, ElementLocator elementLocator)
        {
            var elementTotalProcessed = this.Get<ElementPage>().GetElement(elementLocator).FindElement(By.XPath(nameof(Resources.TicketTotalRegisters).Replace("{0}", UIContent.Conversion[field1]).Replace("xpath:", string.Empty)));
            var totalProcessedRecords = elementTotalProcessed.Text;
            var registartionProcessed = await this.ReadAllSqlAsDictionaryAsync(input: UIContent.GetRow["InventoriesByTicketId"], args: new { ticketId = this.ScenarioContext["TicketId"].ToString() }).ConfigureAwait(false);
            Assert.AreEqual(registartionProcessed.ToDictionaryList().Count, totalProcessedRecords);
            var elementTotalCreated = this.Get<ElementPage>().GetElement(elementLocator).FindElement(By.XPath(nameof(Resources.TicketTotalRegisters).Replace("{0}", UIContent.Conversion[field2]).Replace("xpath:", string.Empty)));
            var totalCreatedRecords = elementTotalCreated.Text;
            var registartionCreated = await this.ReadAllSqlAsDictionaryAsync(input: UIContent.GetRow["MovementsByTicketId"], args: new { ticketId = this.ScenarioContext["TicketId"].ToString() }).ConfigureAwait(false);
            Assert.AreEqual(registartionCreated.ToDictionaryList().Count, totalCreatedRecords);
        }

        [Then(@"validate ""(.*)"" and ""(.*)"" records displayed as expected (in "".*"" "".*"") graph")]
        public void ThenValidateAndRecordsDisplayedAsExpectedInGraph(string field1, string field2, ElementLocator elementLocator)
        {
            Assert.IsNotNull(field1);
            Assert.IsNotNull(field2);
            Assert.IsNotNull(elementLocator);
        }

        [Then(@"validate ""(.*)"", ""(.*)"" and ""(.*)"" processed by TRUE are displayed as expected (in "".*"" "".*"")")]
        public async System.Threading.Tasks.Task ThenValidateAndProcessedByTRUEAreDisplayedAsExpectedInAsync(string field1, string field2, string field3, ElementLocator elementLocator)
        {
            Assert.IsNotNull(field1);
            Assert.IsNotNull(field2);
            Assert.IsNotNull(field3);
            var elementTotalProcessed = this.Get<ElementPage>().GetElement(elementLocator).FindElements(By.XPath(nameof(Resources.GraphValue)));
            var interfaceMovementCreated = await this.ReadAllSqlAsDictionaryAsync(input: UIContent.GetRow["MovementsByTicketIdAndInterface"], args: new { ticketId = this.ScenarioContext["TicketId"].ToString() }).ConfigureAwait(false);
            Assert.AreEqual(interfaceMovementCreated.ToDictionaryList().Count, elementTotalProcessed[0].Text);
            var toleranceMovementCreated = await this.ReadAllSqlAsDictionaryAsync(input: UIContent.GetRow["MovementsByTicketIdAndTolerance"], args: new { ticketId = this.ScenarioContext["TicketId"].ToString() }).ConfigureAwait(false);
            Assert.AreEqual(toleranceMovementCreated.ToDictionaryList().Count, elementTotalProcessed[1].Text);
            var pNIMovementCreated = await this.ReadAllSqlAsDictionaryAsync(input: UIContent.GetRow["MovementsByTicketIdAndPNI"], args: new { ticketId = this.ScenarioContext["TicketId"].ToString() }).ConfigureAwait(false);
            Assert.AreEqual(pNIMovementCreated.ToDictionaryList().Count, elementTotalProcessed[2].Text);
        }
    }
}
