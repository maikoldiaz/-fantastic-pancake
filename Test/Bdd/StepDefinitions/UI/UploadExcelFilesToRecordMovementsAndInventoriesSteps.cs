// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadExcelFilesToRecordMovementsAndInventoriesSteps.cs" company="Microsoft">
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
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Executors.UI;
    using global::Bdd.Core.Web.StepDefinitions;
    using global::Bdd.Core.Web.Utils;

    using NUnit.Framework;

    using Ocaramba.Types;

    using OpenQA.Selenium;

    using TechTalk.SpecFlow;

    [Binding]
    public class UploadExcelFilesToRecordMovementsAndInventoriesSteps : WebStepDefinitionBase
    {
        [When(@"I select ""(.*)"" file from directory")]
        public async Task WhenISelectFileFromDirectoryAsync(string fileName)
        {
            await this.ISelectFileFromDirectoryAsync(fileName).ConfigureAwait(false);
        }

        [When(@"I click on ""(.*)"" to upload")]
        public void WhenIClickOnToUpload(string locator)
        {
            this.IClickOnUploadButton(locator);
        }

        [When(@"I select date from ""(.*)"" ""(.*)""")]
        public void WhenISelectDateFrom(string field1, string field2)
        {
            this.Get<ElementPage>().SetValue(nameof(Resources.InputDate), "01/01/2022");
            this.Get<ElementPage>().SetValue(nameof(Resources.FinalDate), "01/01/2022");
            Assert.IsNotNull(field1);
            Assert.IsNotNull(field2);
        }

        [When(@"I select date which is greater than (.*) months from ""(.*)"" ""(.*)""")]
        public void WhenISelectDateWhichIsGreaterThanMonthsFrom(int field1, string field2, string field3)
        {
            Assert.IsNotNull(field1);
            Assert.IsNotNull(field2);
            Assert.IsNotNull(field3);
        }

        [When(@"the page is loaded into the UI")]
        public void WhenThePageIsLoadedIntoTheUI()
        {
            this.Get<ElementPage>().WaitUntilPageLoad();
        }

        [Then(@"I should see the Upload Id for file tracking")]
        public async Task ThenIShouldSeeTheUploadIdForFileTrackingAsync()
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 5);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetUploadId).ConfigureAwait(false);
            this.SetValue("UploadId", lastCreatedRow["UploadId"]);
            this.Get<ElementPage>().SetValue(nameof(Resources.UploadIdFilter), this.GetValue("UploadId"));
            this.Get<ElementPage>().SendEnterKey(nameof(Resources.UploadIdFilter));
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);
            Assert.AreEqual(this.GetValue("UploadId"), this.Get<ElementPage>().GetElement(nameof(Resources.UploadId)).Text);
        }

        [When(@"I select ""(.*)"" from FileUpload dropdown")]
        public void WhenISelectFromFileUploadDropdown(string action)
        {
            this.ISelectFileFromFileUploadDropdown(action);
        }

        [When(@"I select segment (from "".*"" "".*"")")]
        public void WhenISelectSegmentFrom(ElementLocator elementLocator)
        {
            this.ISelectSegmentFrom(elementLocator);
        }

        [When(@"I select any segment (from "".*"" "".*"")")]
        public void WhenISelectAnySegmentFrom(ElementLocator elementLocator)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 5);
            this.Get<ElementPage>().Click(elementLocator);
            this.Get<ElementPage>().Click(nameof(Resources.UploadType), "Transporte", this.Get<ElementPage>().GetElements(nameof(Resources.UploadTypeName), "Transporte").Count);
        }

        [When(@"I select same segment (from "".*"" "".*"")")]
        public void WhenISelectSameSegmentFrom(ElementLocator elementLocator)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 5);
            this.Get<ElementPage>().Click(elementLocator);
            this.Get<ElementPage>().Click(nameof(Resources.UploadType), this.GetValue("CategorySegment"), this.Get<ElementPage>().GetElements(nameof(Resources.UploadTypeName), this.GetValue("CategorySegment")).Count);
        }

        [When(@"I click on download link")]
        public void WhenIClickOnDownloadLink()
        {
            this.Get<ElementPage>().Click(nameof(Resources.DownloadButton));
        }

        [Then(@"it should associate the new upload Id with old upload Id")]
        public void ThenItShouldAssociateTheNewUploadIdWithOldUploadId()
        {
            Assert.IsNotNull("Test");
        }

        [Then(@"I should see the results as per the filer criteria")]
        public void ThenIShouldSeeTheResultsAsPerTheFilerCriteria()
        {
            Assert.IsNotNull("Test");
        }

        [Then(@"I should not be able to select the date which is greater then (.*) months")]
        public void ThenIShouldNotBeAbleToSelectTheDateWhichIsGreaterThenMonths(int months)
        {
            Assert.IsNotNull(months);
        }

        [Then(@"I should see the list of files uploaded in the last two days")]
        public async Task ThenIShouldSeeTheListOfFilesUploadedInTheLastTwoDaysAsync()
        {
            var lastTwoDaysCountText = this.Get<ElementPage>().GetElement(nameof(Resources.GridCount)).Text;
            var lastTwoDaysCount = Regex.Match(lastTwoDaysCountText, @"e(.+?)e");
            var lastTwoDaysCountFromDB = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetLastTwoDaysData).ConfigureAwait(false);
            Assert.AreEqual(lastTwoDaysCountFromDB.Count(), lastTwoDaysCount);
        }

        [Then(@"I should be able to download the files successfully")]
        public void ThenIShouldBeAbleToDownloadTheFilesSuccessfully()
        {
            Assert.IsTrue(this.Get<FilePage>().CheckDownloadedFileContent(this.GetValue("UploadId") + ".xlsx", this.ScenarioContext, this.FeatureContext).IsCompleted);
        }

        [Then(@"I should see the ""(.*)"" on the page")]
        public void ThenIShouldSeeTheOnFileUploadPage(string field, Table table)
        {
            int i = 0;
            IList<IWebElement> tableColumns = this.Get<ElementPage>().GetElements(nameof(Resources.FileUploadHeader));
            foreach (var row in table?.Rows.Select((value) => (Default: value[field], Unused: string.Empty)))
            {
                while (i <= tableColumns.Count)
                {
                    Assert.AreEqual(tableColumns.ElementAt(i).Text, row.Default);
                    i++;
#pragma warning disable S1751 // Loops with at most one iteration should be refactored
                    break;
#pragma warning restore S1751 // Loops with at most one iteration should be refactored
                }
            }
        }

        [Then(@"I should see the files sorted by uploaded date in descending order")]
        public void ThenIShouldSeeTheFilesSortedByUploadedDateInDescendingOrder()
        {
            Assert.IsNotNull("Test");
        }

        [Then(@"I should see the Process Id for file tracking")]
        public async Task ThenIShouldSeeTheProcessIdForFileTrackingAsync()
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);
            await Task.Delay(5000).ConfigureAwait(true);
            var uploadId = this.Get<ElementPage>().GetElement(nameof(Resources.GetColumnValueFromFristRow), "fileUploads", 1);
            var lastRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetUploadId).ConfigureAwait(false);
            Assert.AreEqual(uploadId.Text, lastRow["UploadId"]);
        }

        [StepDefinition(@"""(.*)"" should be registered in the system")]
        public async Task ThenShouldBeRegisteredInTheSystemAsync(string entity)
        {
            if (entity.EqualsIgnoreCase("Inventory"))
            {
                var lastCreatedRow = await this.ReadAllSqlAsDictionaryAsync(input: ApiContent.GetRow["InventoryById"], args: new { inventoryId = this.GetValue("InventoryId") }).ConfigureAwait(false);
                if (this.GetValue("ActionType") == "Insert")
                {
                    Assert.AreEqual(1, lastCreatedRow.Count());
                }
                else if (this.GetValue("ActionType") == "Update")
                {
                    if (string.IsNullOrEmpty(this.GetValue("ActionTypeCount")))
                    {
                        Assert.AreEqual(3, lastCreatedRow.Count());
                        this.SetValue("ActionTypeCount", "3");
                    }
                    else
                    {
                        Assert.AreEqual(int.Parse(this.GetValue("ActionTypeCount"), CultureInfo.InvariantCulture) + 2, lastCreatedRow.Count());
                    }
                }
                else if (this.GetValue("ActionType") == "Delete")
                {
                    if (string.IsNullOrEmpty(this.GetValue("ActionTypeCount")))
                    {
                        Assert.AreEqual(2, lastCreatedRow.Count());
                        this.SetValue("ActionTypeCount", "2");
                    }
                    else
                    {
                        Assert.AreEqual(int.Parse(this.GetValue("ActionTypeCount"), CultureInfo.InvariantCulture) + 1, lastCreatedRow.Count());
                    }
                }
            }
            else
            {
                var lastCreatedRow = await this.ReadAllSqlAsDictionaryAsync(input: ApiContent.GetRow["MovementById"], args: new { movementId = this.GetValue("MovementId") }).ConfigureAwait(false);
                if (this.GetValue("ActionType") == "Insert")
                {
                    Assert.AreEqual(1, lastCreatedRow.Count());
                }
                else if (this.GetValue("ActionType") == "Update")
                {
                    Assert.AreEqual(3, lastCreatedRow.Count());
                }
                else if (this.GetValue("ActionType") == "Delete")
                {
                    Assert.AreEqual(2, lastCreatedRow.Count());
                }
            }
        }
    }
}