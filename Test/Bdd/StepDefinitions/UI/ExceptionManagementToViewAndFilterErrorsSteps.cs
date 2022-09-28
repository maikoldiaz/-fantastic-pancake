// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionManagementToViewAndFilterErrorsSteps.cs" company="Microsoft">
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
    using System.Threading.Tasks;

    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.StepDefinitions;
    using global::Bdd.Core.Web.Utils;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Ocaramba.Types;

    using TechTalk.SpecFlow;

    [Binding]
    public class ExceptionManagementToViewAndFilterErrorsSteps : WebStepDefinitionBase
    {
        [Given(@"I am having exceptions in Exceptions page")]
        public async Task GivenIAmHavingExceptionsInExceptionsPageAsync()
        {
            ////this.When("I navigate to \"FileUpload\" page");
            this.UiNavigation("FileUpload");
            ////this.When("I click on \"FileUpload\" \"button\"");
            this.IClickOn("FileUpload", "button");
            this.SetValue("CategorySegment", "Transporte");
            ////this.When("I select segment from \"FileUpload\" \"Segment\" \"dropdown\"");
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

        [When(@"I select multiple exceptions")]
        public void WhenISelectMultipleExceptions()
        {
            this.Get<ElementPage>().WaitUntilElementExists(nameof(Resources.PaginationCount));
            var paginationCount = this.Get<ElementPage>().GetElement(nameof(Resources.PaginationCount)).Text;
            this.SetValue("ExpectedCount", paginationCount.Split(' ')[4]);
            var checkboxes = this.Get<ElementPage>().GetElements(nameof(Resources.Checkboxes));
            checkboxes[1].Click();
            checkboxes[2].Click();
        }

        [When(@"I get the total exceptions")]
        public void WhenIGetTheTotalExceptions()
        {
            this.Get<ElementPage>().WaitUntilElementExists(nameof(Resources.PaginationCount));
            var paginationCount = this.Get<ElementPage>().GetElement(nameof(Resources.PaginationCount)).Text;
            this.SetValue("ExpectedCount", paginationCount.Split(' ')[4]);
        }

        [Then(@"given note should be saved for (.*) exception")]
        public async Task ThenGivenNoteShouldBeSavedForEachExceptionAsync(int count)
        {
            var exceptionRecords = await this.ReadAllSqlAsync(input: SqlQueries.GetExceptionsByComment, args: new { comment = this.GetValue(Keys.RandomFieldValue) }).ConfigureAwait(false);
            Assert.AreEqual(count, exceptionRecords.ToDictionaryList().Count);
        }

        [Then(@"discarded (.*) exceptions should not be displayed in the grid")]
        public void ThenDiscardedExceptionsShouldNotBeDisplayedInTheGrid(int count)
        {
            var paginationCount = this.Get<ElementPage>().GetElement(nameof(Resources.PaginationCount)).Text;
            var actualCount = paginationCount.Split(' ')[4];
            var expectedCount = Convert.ToInt32(this.GetValue("ExpectedCount"), CultureInfo.InvariantCulture) - count;
            Assert.AreEqual(expectedCount.ToString(CultureInfo.InvariantCulture), actualCount);
        }

        [Then(@"the modal window should be closed")]
        public void ThenTheModalWindowShouldBeClosed()
        {
            this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ControlExceptionsBreadCrumb));
        }

        [When(@"I provide valid value (for "".*"" "".*"" "".*"")")]
        public void WhenIProvideValidValueFor(ElementLocator elementLocator)
        {
            this.SetValue(Keys.RandomFieldValue, string.Concat($"Automation_", new Faker().Random.AlphaNumeric(5)));
            this.Get<ElementPage>().GetElement(elementLocator).Clear();
            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(this.GetValue(Keys.RandomFieldValue));
        }

        [Then(@"I should see a page with the exception detail")]
        public async Task ThenIShouldSeeAPageWithTheExceptionDetailAsync()
        {
            var url = this.DriverContext.Driver.Url;
            var urlWords = url.Split('/');
            var expectedExceptionDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetExceptionDetails, args: new { errorId = urlWords[5] }).ConfigureAwait(false);
            Dictionary<string, string> actualExceptionDetailsForDate = new Dictionary<string, string>()
                    {
                         { "StartDate", this.Get<ElementPage>().GetElement(nameof(Resources.GetExceptionPageValues), "Fecha Inicial").Text },
                         { "EndDate", this.Get<ElementPage>().GetElement(nameof(Resources.GetExceptionPageValues), "Fecha Final").Text },
                         { "CreatedDate", this.Get<ElementPage>().GetElement(nameof(Resources.GetExceptionPageValues), "Fecha creación").Text },
                    };
            actualExceptionDetailsForDate["StartDate"] = actualExceptionDetailsForDate["StartDate"].Replace(actualExceptionDetailsForDate["StartDate"].Split('-')[1], UIContent.OwnershipCalculationGrid[actualExceptionDetailsForDate["StartDate"].Split('-')[1]]);
            actualExceptionDetailsForDate["EndDate"] = actualExceptionDetailsForDate["EndDate"].Replace(actualExceptionDetailsForDate["EndDate"].Split('-')[1], UIContent.OwnershipCalculationGrid[actualExceptionDetailsForDate["EndDate"].Split('-')[1]]);
            actualExceptionDetailsForDate["CreatedDate"] = actualExceptionDetailsForDate["CreatedDate"].Replace(actualExceptionDetailsForDate["CreatedDate"].Split('-')[1], UIContent.OwnershipCalculationGrid[actualExceptionDetailsForDate["CreatedDate"].Split('-')[1]]);
            Dictionary<string, string> actualExceptionDetails = new Dictionary<string, string>
            {
                { "OriginSystem", this.Get<ElementPage>().GetElement(nameof(Resources.GetExceptionPageValues), "Sistema origen").Text },
                { "Error", this.Get<ElementPage>().GetElement(nameof(Resources.GetErrorValueOfException), "Error").Text },
                { "Identifier", this.Get<ElementPage>().GetElement(nameof(Resources.GetExceptionPageValues), "Identificador").Text },
                { "Type", this.Get<ElementPage>().GetElement(nameof(Resources.GetExceptionPageValues), "Tipo").Text },
                { "Segment", this.Get<ElementPage>().GetElement(nameof(Resources.GetExceptionPageValues), "Segmento").Text },
                { "Process", this.Get<ElementPage>().GetElement(nameof(Resources.GetExceptionPageValues), "Proceso").Text },
                { "Volume", this.Get<ElementPage>().GetElement(nameof(Resources.GetExceptionPageValues), "Volumen").Text },
                { "Units", this.Get<ElementPage>().GetElement(nameof(Resources.GetExceptionPageValues), "Unidad").Text },
                { "FileName", this.Get<ElementPage>().GetElement(nameof(Resources.GetExceptionPageValues), "Nombre Archivo").Text },
                { "OriginNode", this.Get<ElementPage>().GetElement(nameof(Resources.GetExceptionNodeDetails), "Nodo", "Origen", "1").Text },
                { "DestinationNode", this.Get<ElementPage>().GetElement(nameof(Resources.GetExceptionNodeDetails), "Nodo", "Destino", "1").Text },
                { "OriginProduct", this.Get<ElementPage>().GetElement(nameof(Resources.GetExceptionNodeDetails), "Nodo", "Origen", "2").Text },
                { "DestinationProduct", this.Get<ElementPage>().GetElement(nameof(Resources.GetExceptionNodeDetails), "Nodo", "Destino", "2").Text },
            };
            actualExceptionDetails.Add("StartDate", actualExceptionDetailsForDate["StartDate"]);
            actualExceptionDetails.Add("EndDate", actualExceptionDetailsForDate["EndDate"]);
            actualExceptionDetails.Add("CreatedDate", actualExceptionDetailsForDate["CreatedDate"]);

            Assert.IsTrue(this.VerifyDiffs(expectedExceptionDetails, actualExceptionDetails));
        }

        [When(@"I search for movements related exceptions")]
        public void WhenISearchForMovementsRelatedExceptions()
        {
            this.Get<ElementPage>().Click(nameof(Resources.ProcessDropdown));
            this.Get<ElementPage>().WaitUntilElementExists(nameof(Resources.ElementByText), formatArgs: ConstantValues.MovementRegistration);
            this.Get<ElementPage>().Click(nameof(Resources.ElementByText), formatArgs: ConstantValues.MovementRegistration);
        }

        [When(@"the fields do not have a registered value")]
        public void WhenTheFieldsDoNotHaveARegisteredValue()
        {
            this.LogToReport("Covered in previous step");
        }

        [Then(@"only the title should be displayed")]
        public void ThenOnlyTheTitleShouldBeDisplayed()
        {
            this.LogToReport("Covered in previous step");
        }
    }
}
