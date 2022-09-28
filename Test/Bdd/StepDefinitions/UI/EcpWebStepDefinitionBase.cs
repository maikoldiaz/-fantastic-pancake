// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EcpWebStepDefinitionBase.cs" company="Microsoft">
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
    using System.Linq;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.StepDefinitions;
    using global::Bdd.Core.Web.Utils;

    using NUnit.Framework;

    using Ocaramba;
    using Ocaramba.Extensions;
    using Ocaramba.Types;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    using Keys = OpenQA.Selenium.Keys;

    public class EcpWebStepDefinitionBase : WebStepDefinitionBase
    {
        private const string LoaderId = "loader";
        private const string LoaderText = "One Moment...";
        private const string MenuSuffix = "--list";
        private const string DropdownSuffix = "--value";
        private const string ChildrenXPath = ".//*";

        public async Task IHaveOwnershipcalculatedSegmentAsync()
        {
            this.SetValue("OwnershipOperation", "Yes");
            await this.IHaveOwnershipCalculationDataGeneratedInTheSystemAsync().ConfigureAwait(false);
            this.IClickOn("Tickets\" \"viewSummary", "link");
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);
            page.CheckIfElementIsPresent(nameof(Resources.ElementByText), formatArgs: UIContent.Conversion["Volumetric Balance with ownership for node"]);
            var nodeName = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetNodeById, args: new { nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
            this.Get<ElementPage>().SetValue(nameof(Resources.NodeFilterInOwnershipPerNode), nodeName[ConstantValues.Name]);
            this.Get<ElementPage>().SendEnterKey(nameof(Resources.NodeFilterInOwnershipPerNode));
            this.SetValue("NodeName", nodeName[ConstantValues.Name]);
            this.SetValue("SegmentName", page.GetElement(nameof(Resources.FileStatus)).Text);
            this.SetValue("NodeStatus", page.GetElement(nameof(Resources.OwnershipGridInfo), formatArgs: 8).Text);
            this.SetValue("StartDateOfNode", page.GetElement(nameof(Resources.OwnershipGridInfo), formatArgs: 2).Text);
            this.SetValue("EndDateOfNode", page.GetElement(nameof(Resources.OwnershipGridInfo), formatArgs: 3).Text);
            this.SetValue("AcceptableBalancePercentage", nodeName[ConstantValues.AcceptableBalancePercentageTitle]);
        }

        public async Task IShouldSeeTheInformationOfExecutedOperationalCutoffsInTheGridAsync(string gridType)
        {
            IDictionary<string, string> lastCreated = null;
            IEnumerable<IDictionary<string, object>> totalRecords = null;
            switch (gridType)
            {
                case ConstantValues.OwnershipCalculationForSegments:
                    lastCreated = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLatestOwnershipCalculationTicket).ConfigureAwait(false);
                    totalRecords = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetOwnershipCalculationTickets).ConfigureAwait(false);
                    break;

                case ConstantValues.OwnershipCalculationForNodes:
                    lastCreated = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLatestOwnershipCalculationTicket).ConfigureAwait(false);
                    totalRecords = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetOwnershipCalculationTicketsForNode).ConfigureAwait(false);
                    break;

                case ConstantValues.OperationalCutoffs:
                    lastCreated = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastTicket).ConfigureAwait(false);
                    totalRecords = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetOperationalCutoffTickets).ConfigureAwait(false);
                    break;
                case ConstantValues.LogisticReportGeneration:
                    lastCreated = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLatestLogisticTicket).ConfigureAwait(false);
                    totalRecords = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetLogisticTickets).ConfigureAwait(false);
                    this.Get<UrlPage>().NavigateToUrl(this.DriverContext.Driver.Url);
                    this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                    break;

                case ConstantValues.Exceptions:
                    lastCreated = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastException).ConfigureAwait(false);
                    totalRecords = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetExceptions).ConfigureAwait(false);
                    break;

                case ConstantValues.PlanningProgrammingAndCollaborationAgreements:
                    lastCreated = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLatestUploadedEventRecordInFileUploadGrid).ConfigureAwait(false);
                    totalRecords = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetAllUploadedRecordsInFileUploadGrid).ConfigureAwait(false);
                    break;
                case ConstantValues.ExcelFileNameInFileRegistration:
                    lastCreated = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetExcelFileNameInFileRegistration).ConfigureAwait(false);
                    totalRecords = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetEAllExcelFileNamesInFileRegistration).ConfigureAwait(false);
                    break;

                default:
                    break;
            }

            if (gridType.EqualsIgnoreCase(ConstantValues.Exceptions))
            {
                Assert.AreEqual(lastCreated[ConstantValues.ErrorMessage], this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.Error]).Text);
            }
            else if (gridType.EqualsIgnoreCase(ConstantValues.PlanningProgrammingAndCollaborationAgreements))
            {
                Assert.AreEqual(lastCreated[ConstantValues.Name], this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.Segment]).Text);
            }
            else if (gridType.EqualsIgnoreCase(ConstantValues.LogisticReportGeneration))
            {
                var segmentOfLastCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryElementNameByCategoryElementId, args: new { elementId = lastCreated[ConstantValues.CategoryElementId] }).ConfigureAwait(false);
                Assert.AreEqual(segmentOfLastCreatedTicket[ConstantValues.Name], this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.TicketId]).Text);
            }
            else if (gridType.EqualsIgnoreCase(ConstantValues.ExcelFileNameInFileRegistration))
            {
                Assert.AreEqual(lastCreated[ConstantValues.Name], this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.ExcelFileNameInFileRegistration]).Text);
            }
            else
            {
                Assert.AreEqual(lastCreated[ConstantValues.TicketId], this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.TicketId]).Text);
            }

            var paginationCount = this.Get<ElementPage>().GetElement(nameof(Resources.PaginationCount)).Text;
            var count = paginationCount.Split(' ');
            Assert.AreEqual(totalRecords.Count().ToString(CultureInfo.InvariantCulture), count[4]);
        }

        public async Task ISelectedNodeFromEitherSourceNodeOrDestinationNodeBasedOnInputVariableAsync()
        {
            if (this.GetValue(ConstantValues.Variable) == "Input" || this.GetValue(ConstantValues.Variable) == "Tolerance" || this.GetValue(ConstantValues.Variable) == "UnidentifiedLoss")
            {
                ////this.When("I select node from \"CreateMovement\" \"SourceNode\" \"combobox\" on create movement interface");
                await this.ISelectedValueOnCreateMovementFormAsync("CreateMovement\" \"SourceNode", "combobox").ConfigureAwait(false);
            }
            else if (this.GetValue(ConstantValues.Variable) == "Output")
            {
                ////this.When("I selected node from \"CreateMovement\" \"destinationNode\" \"combobox\" on create movement interface");
                await this.ISelectedValueOnCreateMovementInterfaceAsync("CreateMovement\" \"destinationNode", "combobox").ConfigureAwait(false);
            }
        }

        public void IShouldSeeABelongsToInTheGrid(string field, string entity)
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            var connectionGridRow = page.GetElements(nameof(Resources.GridRow), formatArgs: UIContent.Conversion[entity]);
            foreach (var elementLocator in connectionGridRow)
            {
                var connectionGridColumn = elementLocator.FindElements(By.XPath("//div[@role='gridcell']"));
                foreach (var elementLocatorColumn in connectionGridColumn)
                {
                    if (elementLocatorColumn.Text.ContainsIgnoreCase(this.ScenarioContext[field].ToString()))
                    {
                        Assert.IsTrue(elementLocatorColumn.Text.ContainsIgnoreCase(this.ScenarioContext[field].ToString()));
                        break;
                    }
                }
            }
        }

        public void ISelectStartDateAndEndDateOnCreateFileInterface()
        {
            var page = this.Get<ElementPage>();
            var arguments = new object[] { ConstantValues.LogisticsPeriod.ToCamelCase(), ConstantValues.FinalDate.ToCamelCase() };

            page.Click(nameof(Resources.Date), formatArgs: arguments);
            ////Selecting Final date of ownership calculation so using directly Enter key
            page.GetElement(nameof(Resources.Date), formatArgs: arguments).SendKeys(OpenQA.Selenium.Keys.Enter);
            this.SetValue(ConstantValues.FinalDate, page.GetElement(nameof(Resources.Date), formatArgs: arguments).GetAttribute(ConstantValues.Value));
            arguments = new object[] { ConstantValues.LogisticsPeriod.ToCamelCase(), ConstantValues.InitialDate.ToCamelCase() };
            page.Click(nameof(Resources.Date), formatArgs: arguments);
            page.GetElement(nameof(Resources.Date), formatArgs: arguments).SendKeys(this.GetValue(ConstantValues.StartDate));
            page.GetElement(nameof(Resources.Date), formatArgs: arguments).SendKeys(OpenQA.Selenium.Keys.Enter);
            this.SetValue(ConstantValues.StartDate, page.GetElement(nameof(Resources.Date), formatArgs: arguments).GetAttribute(ConstantValues.Value));
        }

        public void ISelectFromNodeTagsDropdown(string value, string type, string number)
        {
            var page = this.Get<ElementPage>();
            if (type == ConstantValues.Category)
            {
                page.Click(nameof(Resources.NodeTagCategoryDropDown), number);
                page.Click(nameof(Resources.SelectBoxOptionByValue), UIContent.Conversion[value]);
            }
            else if (type == ConstantValues.Element && number == ConstantValues.FirstDropDown)
            {
                value = this.FeatureContext[ConstantValues.CategoryElementName].ToString();
                page.Click(nameof(Resources.NodeTagCategoryElementDropDown), number);
                page.Click(nameof(Resources.SelectBoxOptionByValue), value);
            }
            else if (type == ConstantValues.Element && number == ConstantValues.SecondDropDown)
            {
                value = this.ScenarioContext[ConstantValues.NameOfNodeType].ToString();
                page.Click(nameof(Resources.NodeTagCategoryElementDropDown), number);
                page.Click(nameof(Resources.SelectBoxOptionByValue), value);
            }
        }

        public void IEnterNewIntoNodeStorageLocationNameTextbox(string name)
        {
            this.Get<ElementPage>().GetElement(nameof(Resources.ProductNameTextBox)).SendKeys(UIContent.Conversion[name]);
            this.ScenarioContext[name] = UIContent.Conversion[name];
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            this.Get<ElementPage>().GetElement(nameof(Resources.ProductNameTextBox)).SendKeys(OpenQA.Selenium.Keys.Enter);
        }

        public void OwnersConfiguredAtTheConnectionProductLevelShouldBeDisplayed()
        {
            if (this.GetValue(ConstantValues.Variable) == "Input")
            {
                Assert.IsFalse(this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.OwnerhsipData), 2));
            }
            else if (this.GetValue(ConstantValues.Variable) == "Output")
            {
                Assert.IsTrue(this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 2).GetAttribute("value") == ConstantValues.ReficarOwnershipPercentage);
            }
        }

        public async Task IUploadTheExcelIntoTheSystemAsync(string fileName, string eventType = "Insert")
        {
            this.UiNavigation("FileUpload");
            this.IClickOn("FileUpload", "button");
            this.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            this.ISelectFileFromFileUploadDropdown(eventType);
            this.IClickOnUploadButton("Browse");
            await this.ISelectFileFromDirectoryAsync(fileName).ConfigureAwait(false);
            this.IClickOn("uploadFile\" \"Submit", "button");
            await this.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
        }

        protected void SelectAnItemAtIndexFromTheList(int index, ElementLocator locator)
        {
            var list = this.Get<ElementPage>().GetElement(locator);
            list.ClientClick();

            // menu
            if (locator?.Value?.EndsWith(MenuSuffix, StringComparison.OrdinalIgnoreCase) == true)
            {
                var children = list.FindElements(By.XPath(ChildrenXPath));
                children[index].ClientClick();
            }
            else
            {
                // drop-down
                if (locator?.Value?.EndsWith(DropdownSuffix, StringComparison.OrdinalIgnoreCase) == true)
                {
                    // Get the dynamic-menu that gets created and added to the parent drop-down at runtime
                    var dynamicChild = list.FindElements(By.XPath(ChildrenXPath)).LastOrDefault();
                    dynamicChild.ClientClick();

                    // Now, use the runtime-created menu to select an item
                    this.SelectItemAtIndex(dynamicChild, index);
                }
                else
                {
                    // combo-box
                    this.SelectItemAtIndex(list, index);
                }
            }

            // Wait until the progress-bar disappears before performing the next action
            this.WaitUntilProgressComplete();
        }

        protected void SelectItemAtIndex(IWebElement list, int index)
        {
            for (var i = 0; i <= index; i++)
            {
                list?.SendKeys(Keys.Down);
            }

            list?.SendKeys(Keys.Enter);
        }

        protected void WaitUntilProgressComplete()
        {
            var locator = new ElementLocator(Locator.Id, LoaderId);
            var wait = new WebDriverWait(this.DriverContext.Driver, TimeSpan.FromSeconds(this.ImplicitlyWaitMilliseconds));
#pragma warning disable CA1303 // Do not pass literals as localized parameters
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementWithText(locator.ToBy(), LoaderText));
#pragma warning restore CA1303 // Do not pass literals as localized parameters
        }

        protected async Task SelectValueFromComboBoxAsync(string value, string field, string grid)
        {
            this.ScenarioContext.Set<string>(value, field);
            this.Get<ElementPage>().GetElement(nameof(Resources.ComboBox), grid.ToCamelCase(), field.ToCamelCase()).Click();
            await Task.Delay(2000).ConfigureAwait(false);
            this.Get<ElementPage>().GetElement(nameof(Resources.ComboValue), value).Click();
        }

        protected void NavigateToPage(string link)
        {
            this.UiNavigation(link);
        }
    }
}