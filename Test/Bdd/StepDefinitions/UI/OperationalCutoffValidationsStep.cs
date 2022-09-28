// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationalCutoffValidationsStep.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;
    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using TechTalk.SpecFlow;

    [Binding]
    public class OperationalCutoffValidationsStep : EcpWebStepDefinitionBase
    {
        [Given(@"I update the segment to SON")]
        public async System.Threading.Tasks.Task GivenIUpdateTheSegmentToSONAsync()
        {
            var msg = await this.ReadAllSqlAsync(input: SqlQueries.UpdateSegmentToSON, args: new { name = this.ScenarioContext.Get<string>("CategorySegment") }).ConfigureAwait(false);
        }

        [Given(@"I have deltas calculation process is running for the segment")]
        public async Task GivenIHaveDeltasCalculationProcessIsRunningForTheSegmentAsync()
        {
            this.Given(@"I have nodes of the segment for the selected period already have an operational cutoff executed");
            await this.ReadAllSqlAsync(input: SqlQueries.UpdateTicketStatusDelta, args: new { ticketId = this.ScenarioContext.Get<string>("TicketId") }).ConfigureAwait(false);
        }

        [When(@"I refresh the current page")]
        public void WhenIRefreshTheCurrentPage()
        {
            this.Get<WindowPage>().Refresh();
        }

        [Then(@"I should see the title ""(.*)""")]
        public void ThenIShouldSeeTheTitle(string message)
        {
            var actualMessage = this.Get<ElementPage>().GetElement(nameof(Resources.TitleCutoffGrid)).Text;
            Assert.IsTrue(actualMessage.Contains(message));
        }

        [Then(@"I should see the label contains word segmento at the top left in lowercase")]
        public void ThenIShouldSeeTheLabelContainsWordSegmentoAtTheTopLeftInLowercase()
        {
            var actualMessage = this.Get<ElementPage>().GetElement(nameof(Resources.TitleCutoffGrid)).Text;
#pragma warning disable CA1304 // Specify CultureInfo
            Assert.IsTrue(actualMessage.Contains(ConstantValues.Segmento.ToLower()));
#pragma warning restore CA1304 // Specify CultureInfo
        }

        [Then(@"I should see the ""(.*)"" button name as ""(.*)"" with the first capital letter")]
        public void ThenIShouldSeeTheButtonNameAsWithTheFirstCapitalLetter(string btnName, string title)
        {
            var actualButton = this.Get<ElementPage>().GetElement(nameof(Resources.ButtonsOnCutoffInterface), formatArgs: btnName).Text;
            Assert.IsTrue(title.EqualsIgnoreCase(actualButton));
        }

        [Given(@"I have movement with technical exception")]
        public async System.Threading.Tasks.Task GivenIHaveMovementWithTechnicalExceptionAsync()
        {
            await this.IHaveHomologationDataInTheSystemAsync("Excel").ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
            this.IUpdateTheExcelFile("TestDataCutOff_TechException");
            this.UiNavigation("FileUpload");
            this.IClickOn("FileUpload", "button");
            this.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            this.ISelectFileFromFileUploadDropdown("Insert");
            this.IClickOnUploadButton("Browse");
            await this.ISelectFileFromDirectoryAsync("TestDataCutOff_TechException").ConfigureAwait(false);
            this.IClickOn("uploadFile\" \"Submit", "button");
            await this.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
        }

        [Then(@"I should see message popup window")]
        [Given(@"I had again run the cutoff for the same segment in the another screen/tab")]
        public async Task ThenIShouldSeeMessagePopupWindowAsync()
        {
            await this.ReadAllSqlAsync(input: SqlQueries.UpdateTicketStatusProcessing, args: new { name = this.ScenarioContext.Get<string>("CategorySegment") }).ConfigureAwait(false);
            await Task.Delay(2000).ConfigureAwait(true);
        }

        [When(@"I should the see the messages with exceptions in messaging grid")]
        public void WhenIShouldTheSeeTheMessagesWithExceptionsInMessagingGrid()
        {
            //// this step left intentionaly blank
        }

        [Then(@"I should see the below ""(.*)"" on the page")]
        public void ThenIShouldSeeTheBelowOnThePage(string field, Table table)
        {
            int i = 0;
            IList<IWebElement> tableColumns = this.Get<ElementPage>().GetElements(nameof(Resources.FileUploadHeader));
            foreach (var row in table?.Rows.Select((value) => (Default: value[field], Unused: string.Empty)))
            {
                while (i <= tableColumns.Count)
                {
                    Assert.IsTrue(tableColumns.ElementAt(i).Text.EqualsIgnoreCase(row.Default));
                    i++;
#pragma warning disable S1751 // Loops with at most one iteration should be refactored
                    break;
#pragma warning restore S1751 // Loops with at most one iteration should be refactored
                }
            }
        }

        [Then(@"I should see the tooltip when hovers over an any exception message")]
        public void ThenIShouldSeeTheTooltipWhenHoversOverAnAnyExceptionMessage()
        {
            //// this step left intentionaly blank
        }

        [Then(@"I should see the message ""(.*)"" on the Page")]
        public void ThenIShouldSeeTheMessageOnThePage(string message)
        {
            this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.InfoMessageonCutoffPage));
            Assert.AreEqual(message, this.Get<ElementPage>().GetElement(nameof(Resources.InfoMessageonCutoffPage)).Text);
        }

        [Then(@"I should see the label ""(.*)"" on Note popup")]
        public void ThenIShouldSeeTheLabelOnNotePopup(string message)
        {
            Assert.AreEqual(message, this.Get<ElementPage>().GetElement(nameof(Resources.NoteMessageonCutoffPopup)).Text);
        }

        [Then(@"I should see the mandatory message ""(.*)"" in the Page")]
        public void ThenIShouldSeeTheMandatoryMessageInThePage(string message)
        {
            this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ErrorMessage));
            Assert.AreEqual(message, this.Get<ElementPage>().GetElement(nameof(Resources.ErrorMessage)).Text);
        }

        [Given(@"I had again run the cutoff for the same segment in the another screen")]
        public async Task GivenIHadAgainRunTheCutoffForTheSameSegmentInTheAnotherScreenAsync()
        {
            await this.ReadAllSqlAsync(input: SqlQueries.UpdateTicketStatusFailed, args: new { name = this.ScenarioContext.Get<string>("CategorySegment") }).ConfigureAwait(false);
            await Task.Delay(2000).ConfigureAwait(true);
        }
    }
}
