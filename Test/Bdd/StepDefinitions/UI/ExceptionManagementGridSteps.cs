// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionManagementGridSteps.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.StepDefinitions;
    using global::Bdd.Core.Web.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Ocaramba.Types;
    using OpenQA.Selenium.Interactions;
    using TechTalk.SpecFlow;

    [Binding]
    public class ExceptionManagementGridSteps : WebStepDefinitionBase
    {
        [StepDefinition(@"I have failed messages generated through excel files")]
        public async Task GivenIHaveFailedMessagesGeneratedThroughExcelFilesAsync()
        {
            ////this.When("I navigate to \"FileUpload\" page");
            this.UiNavigation("FileUpload");
            ////this.When("I click on \"FileUpload\" \"button\"");
            this.IClickOn("FileUpload", "button");
            this.SetValue("CategorySegment", "Comercial");
            ////this.When("I select same segment from \"FileUpload\" \"Segment\" \"dropdown\"");
            this.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            ////this.When("I select \"Insert\" from FileUpload dropdown");
            this.ISelectFileFromFileUploadDropdown("Insert");
            ////this.When("I click on \"Browse\" to upload");
            this.IClickOnUploadButton("Browse");
            ////this.When("I select \"TestDataCutOff\" file from directory");
            await this.ISelectFileFromDirectoryAsync("TestDataCutOff").ConfigureAwait(false);
            ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
            this.IClickOn("uploadFile\" \"Submit", "button");
            await Task.Delay(40000).ConfigureAwait(true);
        }

        [Given(@"I have failed messages by Sinoper")]
        public async Task GivenIHaveFailedMessagesBySinoperAsync()
        {
            ////this.Given("I am authenticated as \"admin\"");
            await this.UIAuthenticationForUserAsync("admin").ConfigureAwait(false);
            ////this.Given("I want to register an \"Inventory\" in the system");
            await this.IWantToRegisterAnInTheSystemAsync("Inventory").ConfigureAwait(false);
            ////this.When("I receive the data​ with \"EventType\" that exceeds 10 characters");
            await this.ReceiveTheDataWithThatExceedsCharactersAsync("EventType", 10).ConfigureAwait(false);
            await Task.Delay(40000).ConfigureAwait(true);
        }

        [Then(@"I should see error message in error section")]
        public async Task ThenIShouldNotSeeErrorMessageInErrorSectionAsync()
        {
            IDictionary<string, string> lastCreated = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastException).ConfigureAwait(false);
            string errorMessage = this.Get<ElementPage>().GetElement(nameof(Resources.GetErrorValueOfException), "Error").Text;
            Assert.AreEqual(errorMessage, lastCreated[ConstantValues.ErrorMessage]);
        }

        [When(@"I select any segment value (from "".*"" "".*"")")]
        public void WhenISelectSegmentFrom(ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 15);
            page.Click(elementLocator);
            var options = this.Get<ElementPage>().GetElements(nameof(Resources.UploadTypeName));
            Actions action = new Actions(this.DriverContext.Driver);
            action.MoveToElement(options[options.Count - 1]).Perform();
            options[options.Count - 1].Click();
            page.Click(nameof(Resources.UploadTypeName), this.GetValue("CategorySegment"));
        }

        [Then(@"I should not see any data in file column")]
        public void ThenIShouldNotSeeAnyDataInColumn()
        {
            Assert.IsNull(this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.ExcelFileNameInFileRegistration]).Text);
        }

        [Then(@"I should see the executed Exceptions in the grid")]
        public async Task ThenIShouldSeeTheExecutedExceptionsInTheGridAsync()
        {
            IDictionary<string, string> lastCreated = null;
            lastCreated = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastException).ConfigureAwait(false);
            if (lastCreated[ConstantValues.RecordId] != null)
            {
                Assert.AreEqual(lastCreated[ConstantValues.RecordId], this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.RecordId]).Text);
            }
            else
            {
                Assert.AreEqual(lastCreated[ConstantValues.TransactionId], this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.RecordId]).Text);
            }
        }

        [Then(@"I should see the ExcelFileName in the grid")]
        public async Task ThenIShouldSeeTheExcelFileNameInTheGridAsync()
        {
            IDictionary<string, string> lastCreated = null;
            lastCreated = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetExcelFileNameInFileRegistration).ConfigureAwait(false);
            Assert.AreEqual(lastCreated[ConstantValues.Name], this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.ExcelFileNameInFileRegistration]).Text);
        }

        [When(@"I filter (on "".*"" "".*"") with message id")]
        public async Task WhenIFilterExceptionsWithMessageIdAsync(ElementLocator elementLocator)
        {
            IDictionary<string, string> lastCreated = null;
            lastCreated = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastException).ConfigureAwait(false);
            if (lastCreated[ConstantValues.RecordId] != null)
            {
                this.ScenarioContext["messageID"] = lastCreated[ConstantValues.RecordId];
            }
            else
            {
                this.ScenarioContext["messageID"] = lastCreated[ConstantValues.TransactionId];
            }

            var page = this.Get<ElementPage>();
            page.GetElement(elementLocator).SendKeys(this.ScenarioContext["messageID"] + OpenQA.Selenium.Keys.Enter);
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
        }

        [Then(@"I should see the exceptions that matches the filter")]
        public void ThenIShouldSeeTheExceptionsThatMatchesTheFilter()
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            Assert.AreEqual(this.ScenarioContext["messageID"], this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.RecordId]).Text);
        }
    }
}