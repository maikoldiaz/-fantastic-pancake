// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManageNodesSteps.cs" company="Microsoft">
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
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using NUnit.Framework;

    using Ocaramba.Types;

    using TechTalk.SpecFlow;

    [Binding]
    public class ManageNodesSteps : EcpWebStepDefinitionBase
    {
        [Then(@"I should see the ""(.*)""")]
        public void ThenIShouldSeeThe(string interfaceName)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            Assert.AreEqual(UIContent.Conversion[interfaceName], this.Get<ElementPage>().GetElementText(nameof(Resources.CreateNodeInterface)));
        }

        [StepDefinition(@"I should see the ""(.*)"" interface")]
        public void ThenIShouldSeeTheInterface(string interfaceName)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            Assert.AreEqual(UIContent.Conversion[interfaceName], this.Get<ElementPage>().GetElementText(nameof(Resources.NodeInformationInterface)));
        }

        [When(@"I select Segment (from "".*"" "".*"" "".*"")")]
        public void WhenISelectSegementFrom(ElementLocator elementLocator)
        {
            this.Get<UrlPage>().NavigateToUrl(this.DriverContext.Driver.Url);
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            ////this.When("I click on \"Search\" \"Button\"");
            this.IClickOn("Search", "button");
            this.Get<ElementPage>().Click(elementLocator);
            this.Get<ElementPage>().GetElement(nameof(Resources.SelectBoxOptionByValue), this.ScenarioContext["SegmentName"]).Click();
        }

        [When(@"I select Type (from "".*"" "".*"" "".*"")")]
        public void WhenISelectTypeFrom(ElementLocator elementLocator)
        {
            this.Get<ElementPage>().Click(elementLocator);
            this.Get<ElementPage>().GetElement(nameof(Resources.SelectBoxOptionByValue), this.ScenarioContext["NodeTypeName"]).Click();
        }

        [When(@"I select Operator (from "".*"" "".*"" "".*"")")]
        public void WhenISelectOperatorFrom(ElementLocator elementLocator)
        {
            this.Get<ElementPage>().Click(elementLocator);
            this.Get<ElementPage>().GetElement(nameof(Resources.SelectBoxOptionByValue), this.ScenarioContext["OperatorName"]).Click();
        }

        [Then(@"I should see the records filtered as per the search criteria")]
        public async Task ThenIShouldSeeTheRecordsFilteredAsPerTheSearchCriteriaAsync()
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            ////var expectedNodeInformation = ((IDictionary<string, object>)await this.Output.ReadAsync<dynamic>(args: new { segmentElementId = this.ScenarioContext["SegmentId"], operatorElementId = this.ScenarioContext["OperatorId"], nodeTypeElementId = this.ScenarioContext["NodeTypeId"] }).ConfigureAwait(false)).ToStringDictionary();
            var expectedNodeInformation = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetNodeInformation"], args: new { segmentElementId = this.ScenarioContext["SegmentId"], operatorElementId = this.ScenarioContext["OperatorId"], nodeTypeElementId = this.ScenarioContext["NodeTypeId"] }).ConfigureAwait(false);
            Dictionary<string, string> actualNodeInformation = new Dictionary<string, string>
            {
                { "Segment", this.Get<ElementPage>().GetElement(nameof(Resources.NodeInformationAfterFiltered), UIContent.GridPosition["NodeSegment"]).Text },
                { "NodeName", this.Get<ElementPage>().GetElement(nameof(Resources.NodeInformationAfterFiltered), UIContent.GridPosition["NodeName"]).Text },
                { "NodeType", this.Get<ElementPage>().GetElement(nameof(Resources.NodeInformationAfterFiltered), UIContent.GridPosition["NodeTypeName"]).Text },
                { "Operator", this.Get<ElementPage>().GetElement(nameof(Resources.NodeInformationAfterFiltered), UIContent.GridPosition["Operator"]).Text },
            };

            Assert.IsTrue(this.VerifyDiffs(expectedNodeInformation, actualNodeInformation));

            // verifying filter when user is trying to filter data which is not found in the grid
            this.Get<ElementPage>().GetElement(nameof(Resources.FilterForNodeName)).SendKeys(ConstantValues.SpecialCharacters + OpenQA.Selenium.Keys.Enter);
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.NoRecordsFoundMessage));
            Assert.AreEqual(ConstantValues.NoRecordsFoundMessage, this.Get<ElementPage>().GetElement(nameof(Resources.NoRecordsFoundMessage)).Text);
        }

        [Then(@"I should see the results based on the selected filter")]
        public async Task ThenIShouldSeeTheResultsBasedOnTehSelectedFilterAsync()
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            ////var expectedNodeInformation = ((IDictionary<string, object>)await this.Output.ReadAsync<dynamic>(args: new { segmentElementId = this.ScenarioContext["SegmentId"], operatorElementId = this.ScenarioContext["OperatorId"], nodeTypeElementId = this.ScenarioContext["NodeTypeId"] }).ConfigureAwait(false)).ToStringDictionary();
            var expectedNodeInformation = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetNodeInformation"], args: new { segmentElementId = this.ScenarioContext["SegmentId"], operatorElementId = this.ScenarioContext["OperatorId"], nodeTypeElementId = this.ScenarioContext["NodeTypeId"] }).ConfigureAwait(false);
            Dictionary<string, string> actualNodeInformation = new Dictionary<string, string>
            {
                { "Segment", this.Get<ElementPage>().GetElement(nameof(Resources.NodeInformationAfterFiltered), UIContent.GridPosition["NodeSegment"]).Text },
                { "NodeName", this.Get<ElementPage>().GetElement(nameof(Resources.NodeInformationAfterFiltered), UIContent.GridPosition["NodeName"]).Text },
                { "NodeType", this.Get<ElementPage>().GetElement(nameof(Resources.NodeInformationAfterFiltered), UIContent.GridPosition["NodeTypeName"]).Text },
                { "Operator", this.Get<ElementPage>().GetElement(nameof(Resources.NodeInformationAfterFiltered), UIContent.GridPosition["Operator"]).Text },
            };

            Assert.IsTrue(this.VerifyDiffs(expectedNodeInformation, actualNodeInformation));
        }

        [When(@"I click on the ""(.*)"" in Grid")]
        public void WhenIClickOnTheInGrid(string field)
        {
            switch (field)
            {
                case ConstantValues.Segmento:
                    this.Get<ElementPage>().GetElement(nameof(Resources.ColumnsInNodeGrid), UIContent.GridPosition["NodeSegment"]).Click();
                    break;
                case ConstantValues.Nombre:
                    this.Get<ElementPage>().GetElement(nameof(Resources.ColumnsInNodeGrid), UIContent.GridPosition["NodeName"]).Click();
                    break;
                case ConstantValues.Tipo:
                    this.Get<ElementPage>().GetElement(nameof(Resources.ColumnsInNodeGrid), UIContent.GridPosition["NodeTypeName"]).Click();
                    break;
                case ConstantValues.Operador:
                    this.Get<ElementPage>().GetElement(nameof(Resources.ColumnsInNodeGrid), UIContent.GridPosition["Operator"]).Click();
                    break;
                default:
                    break;
            }
        }

        [Then(@"the results should be sorted based on ""(.*)"" in ""(.*)"" Grid")]
        public async Task ThenTheResultsShouldBeSortedBasedOnAsync(string columnName, string gridType)
        {
            Dictionary<string, string> actualGridInformation = null;
            switch (gridType)
            {
                case ConstantValues.Nodes:
                    actualGridInformation = new Dictionary<string, string>
                    {
                        { "Segment", this.Get<ElementPage>().GetElement(nameof(Resources.NodeInformationAfterFiltered), UIContent.GridPosition["NodeSegment"]).Text },
                        { "NodeName", this.Get<ElementPage>().GetElement(nameof(Resources.NodeInformationAfterFiltered), UIContent.GridPosition["NodeName"]).Text },
                        { "NodeType", this.Get<ElementPage>().GetElement(nameof(Resources.NodeInformationAfterFiltered), UIContent.GridPosition["NodeTypeName"]).Text },
                        { "Operator", this.Get<ElementPage>().GetElement(nameof(Resources.NodeInformationAfterFiltered), UIContent.GridPosition["Operator"]).Text },
                    };
                    break;

                case ConstantValues.OwnershipCalculationForSegments:
                    Dictionary<string, string> actualGridInformationForDate = new Dictionary<string, string>()
                    {
                         { "StartDate", this.Get<ElementPage>().GetElement(nameof(Resources.OwnershipCalculationForSegmentsTicketGrid), UIContent.GridPosition["StartDateOfOwnershipCalculationForSegments"]).Text },
                         { "EndDate", this.Get<ElementPage>().GetElement(nameof(Resources.OwnershipCalculationForSegmentsTicketGrid), UIContent.GridPosition["EndDateOfOwnershipCalculationForSegments"]).Text },
                         { "ExecutionDate", this.Get<ElementPage>().GetElement(nameof(Resources.OwnershipCalculationForSegmentsTicketGrid), UIContent.GridPosition["ExecutionDateOfOwnershipCalculationForSegments"]).Text },
                    };
                    actualGridInformationForDate["StartDate"] = actualGridInformationForDate["StartDate"].Replace(actualGridInformationForDate["StartDate"].Split('-')[1], UIContent.OwnershipCalculationGrid[actualGridInformationForDate["StartDate"].Split('-')[1]]);
                    actualGridInformationForDate["EndDate"] = actualGridInformationForDate["EndDate"].Replace(actualGridInformationForDate["EndDate"].Split('-')[1], UIContent.OwnershipCalculationGrid[actualGridInformationForDate["EndDate"].Split('-')[1]]);
                    actualGridInformationForDate["ExecutionDate"] = actualGridInformationForDate["ExecutionDate"].Replace(actualGridInformationForDate["ExecutionDate"].Split('-')[1], UIContent.OwnershipCalculationGrid[actualGridInformationForDate["ExecutionDate"].Split('-')[1]]);
                    actualGridInformation = new Dictionary<string, string>
                    {
                        { "TicketId", this.Get<ElementPage>().GetElement(nameof(Resources.OwnershipCalculationForSegmentsTicketGrid), UIContent.GridPosition["TicketId"]).Text },
                        { "CreatedBy", this.Get<ElementPage>().GetElement(nameof(Resources.OwnershipCalculationForSegmentsTicketGrid), UIContent.GridPosition["UsernameOfOwnershipCalculationForSegments"]).Text },
                        { "Name", this.Get<ElementPage>().GetElement(nameof(Resources.OwnershipCalculationForSegmentsTicketGrid), UIContent.GridPosition["SegmentNameOfOwnershipCalculationForSegments"]).Text },
                        { "Status", this.Get<ElementPage>().GetElement(nameof(Resources.OwnershipCalculationForSegmentsTicketGrid), UIContent.GridPosition["StatusOfOwnershipCalculationForSegments"]).Text },
                    };
                    actualGridInformation.Add("StartDate", actualGridInformationForDate["StartDate"]);
                    actualGridInformation.Add("EndDate", actualGridInformationForDate["EndDate"]);
                    actualGridInformation.Add("CreatedDate", actualGridInformationForDate["ExecutionDate"]);
                    break;

                case ConstantValues.OwnershipCalculationForNodes:
                    actualGridInformationForDate = new Dictionary<string, string>()
                    {
                         { "StartDate", this.Get<ElementPage>().GetElement(nameof(Resources.OwnershipCalculationForNodesGrid), UIContent.OwnershipCalculationGrid["StartDate"]).Text },
                         { "EndDate", this.Get<ElementPage>().GetElement(nameof(Resources.OwnershipCalculationForNodesGrid), UIContent.OwnershipCalculationGrid["EndDate"]).Text },
                         { "ExecutionDate", this.Get<ElementPage>().GetElement(nameof(Resources.OwnershipCalculationForNodesGrid), UIContent.OwnershipCalculationGrid["TicketCreatedDate"]).Text },
                    };
                    actualGridInformationForDate["StartDate"] = actualGridInformationForDate["StartDate"].Replace(actualGridInformationForDate["StartDate"].Split('-')[1], UIContent.OwnershipCalculationGrid[actualGridInformationForDate["StartDate"].Split('-')[1]]);
                    actualGridInformationForDate["EndDate"] = actualGridInformationForDate["EndDate"].Replace(actualGridInformationForDate["EndDate"].Split('-')[1], UIContent.OwnershipCalculationGrid[actualGridInformationForDate["EndDate"].Split('-')[1]]);
                    actualGridInformationForDate["ExecutionDate"] = actualGridInformationForDate["ExecutionDate"].Replace(actualGridInformationForDate["ExecutionDate"].Split('-')[1], UIContent.OwnershipCalculationGrid[actualGridInformationForDate["ExecutionDate"].Split('-')[1]]);
                    actualGridInformation = new Dictionary<string, string>
                    {
                        { "TicketId", this.Get<ElementPage>().GetElement(nameof(Resources.OwnershipCalculationForNodesGrid), UIContent.OwnershipCalculationGrid["TicketId"]).Text },
                        { "CreatedBy", this.Get<ElementPage>().GetElement(nameof(Resources.OwnershipCalculationForNodesGrid), UIContent.OwnershipCalculationGrid["CreatedBy"]).Text },
                        { "Node", this.Get<ElementPage>().GetElement(nameof(Resources.OwnershipCalculationForNodesGrid), UIContent.OwnershipCalculationGrid["Node"]).Text },
                        { "Segment", this.Get<ElementPage>().GetElement(nameof(Resources.OwnershipCalculationForNodesGrid), UIContent.OwnershipCalculationGrid["Segment"]).Text },
                        { "Status", this.Get<ElementPage>().GetElement(nameof(Resources.OwnershipCalculationForNodesGrid), UIContent.OwnershipCalculationGrid["Status"]).Text },
                    };
                    actualGridInformation.Add("StartDate", actualGridInformationForDate["StartDate"]);
                    actualGridInformation.Add("EndDate", actualGridInformationForDate["EndDate"]);
                    actualGridInformation.Add("CreatedDate", actualGridInformationForDate["ExecutionDate"]);
                    break;

                case ConstantValues.LogisticReportGeneration:
                    actualGridInformationForDate = new Dictionary<string, string>()
                    {
                         { "StartDate", this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), UIContent.ReportLogisticGrid["StartDate"]).Text },
                         { "EndDate", this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), UIContent.ReportLogisticGrid["EndDate"]).Text },
                         { "ExecutionDate", this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), UIContent.ReportLogisticGrid["TicketCreatedDate"]).Text },
                    };
                    actualGridInformationForDate["StartDate"] = actualGridInformationForDate["StartDate"].Replace(actualGridInformationForDate["StartDate"].Split('-')[1], UIContent.ReportLogisticGrid[actualGridInformationForDate["StartDate"].Split('-')[1]]);
                    actualGridInformationForDate["EndDate"] = actualGridInformationForDate["EndDate"].Replace(actualGridInformationForDate["EndDate"].Split('-')[1], UIContent.ReportLogisticGrid[actualGridInformationForDate["EndDate"].Split('-')[1]]);
                    actualGridInformationForDate["ExecutionDate"] = actualGridInformationForDate["ExecutionDate"].Replace(actualGridInformationForDate["ExecutionDate"].Split('-')[1], UIContent.ReportLogisticGrid[actualGridInformationForDate["ExecutionDate"].Split('-')[1]]);
                    actualGridInformation = new Dictionary<string, string>
                    {
                        { "CreatedBy", this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), UIContent.ReportLogisticGrid["CreatedBy"]).Text },
                        { "Owner", this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), UIContent.ReportLogisticGrid["Owner"]).Text },
                        { "Segment", this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), UIContent.ReportLogisticGrid["Segment"]).Text },
                        { "Status", this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), UIContent.ReportLogisticGrid["Status"]).Text },
                    };
                    actualGridInformation.Add("StartDate", actualGridInformationForDate["StartDate"].Replace("-", "/"));
                    actualGridInformation.Add("EndDate", actualGridInformationForDate["EndDate"].Replace("-", "/"));
                    actualGridInformation.Add("CreatedDate", actualGridInformationForDate["ExecutionDate"].Replace("-", "/"));
                    break;

                default:
                    break;
            }

            IDictionary<string, string> sortedRecords = null;

            switch (columnName + "_" + gridType)
            {
                case ConstantValues.SegmentoOfNodesGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetSortedSegmentNodeInformation"]).ConfigureAwait(false);
                    break;
                case ConstantValues.NombreOfNodesGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetSortedNodeInformation"]).ConfigureAwait(false);
                    break;
                case ConstantValues.TipoOfNodesGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetSortedNodeTypeInformation"]).ConfigureAwait(false);
                    break;
                case ConstantValues.OperadorOfNodesGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetSortedOperatorNodeInformation"]).ConfigureAwait(false);
                    break;
                case ConstantValues.TicketOfOwnershipCalculationForSegmentsGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetTicketSortedByTicketIdForOwnershipForSegmentsGrid"]).ConfigureAwait(false);
                    break;
                case ConstantValues.StartDateOfOwnershipCalculationForSegmentsGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetTicketSortedByStartDateForOwnershipForSegmentsGrid"]).ConfigureAwait(false);
                    break;
                case ConstantValues.EndDateOfOwnershipCalculationForSegmentsGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetTicketSortedByEndDateForOwnershipForSegmentsGrid"]).ConfigureAwait(false);
                    break;
                case ConstantValues.ExecutionDateOfOwnershipCalculationForSegmentsGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetTicketSortedByCreatedTicketDateForOwnershipForSegmentsGrid"]).ConfigureAwait(false);
                    break;
                case ConstantValues.UsernameOfOwnershipCalculationForSegmentsGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetTicketSortedByUsernameForOwnershipForSegmentsGrid"]).ConfigureAwait(false);
                    break;
                case ConstantValues.SegmentOfOwnershipCalculationForSegmentsGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetTicketSortedBySegmentNameForOwnershipForSegmentsGrid"]).ConfigureAwait(false);
                    break;
                case ConstantValues.StatusOfOwnershipCalculationForSegmentsGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetTicketSortedByStatusForOwnershipForSegmentsGrid"]).ConfigureAwait(false);
                    break;
                case ConstantValues.TicketOfOwnershipCalculationForNodesGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetTicketSortedByTicketIdForOwnershipForNodesGrid"]).ConfigureAwait(false);
                    break;
                case ConstantValues.StartDateOfOwnershipCalculationForNodesGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetTicketSortedByStartDateForOwnershipForNodesGrid"]).ConfigureAwait(false);
                    break;
                case ConstantValues.EndDateOfOwnershipCalculationForNodesGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetTicketSortedByEndDateForOwnershipForNodesGrid"]).ConfigureAwait(false);
                    break;
                case ConstantValues.ExecutionDateOfOwnershipCalculationForNodesGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetTicketSortedByCreatedTicketDateForOwnershipForNodesGrid"]).ConfigureAwait(false);
                    break;
                case ConstantValues.UsernameOfOwnershipCalculationForNodesGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetTicketSortedByUsernameForOwnershipForNodesGrid"]).ConfigureAwait(false);
                    break;
                case ConstantValues.SegmentOfOwnershipCalculationForNodesGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetTicketSortedBySegmentNameForOwnershipForNodesGrid"]).ConfigureAwait(false);
                    break;
                case ConstantValues.NodeOfOwnershipCalculationForNodesGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetTicketSortedByNodeNameForOwnershipForNodesGrid"]).ConfigureAwait(false);
                    break;
                case ConstantValues.StatusOfOwnershipCalculationForNodesGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetTicketSortedByStatusForOwnershipForNodesGrid"]).ConfigureAwait(false);
                    break;
                case ConstantValues.SegmentOfReportLogisticsGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetTicketSortedBySegmentForReportLogisticGrid"]).ConfigureAwait(false);
                    break;
                case ConstantValues.OwnerOfReportLogisticsGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetTicketSortedByOwnerForReportLogisticGrid"]).ConfigureAwait(false);
                    break;
                case ConstantValues.StartDateOfReportLogisticsGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetTicketSortedByStartDateForReportLogisticGrid"]).ConfigureAwait(false);
                    break;
                case ConstantValues.EndDateOfReportLogisticsGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetTicketSortedByEndDateForReportLogisticGrid"]).ConfigureAwait(false);
                    break;
                case ConstantValues.ExecutionDateOfReportLogisticsGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetTicketSortedByTicketCreatedDateForReportLogisticGrid"]).ConfigureAwait(false);
                    break;
                case ConstantValues.UsernameOfReportLogisticsGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetTicketSortedByUsernameForReportLogisticGrid"]).ConfigureAwait(false);
                    break;
                case ConstantValues.StatusOfReportLogisticsGrid:
                    sortedRecords = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["GetTicketSortedByStatusForReportLogisticGrid"]).ConfigureAwait(false);
                    break;
                default:
                    break;
            }

            Assert.IsTrue(this.VerifyDiffs(sortedRecords, actualGridInformation));
        }

        [When(@"I select a Segment (from "".*"" "".*"" "".*"")")]
        public void WhenISelectASegmentFrom(ElementLocator elementLocator)
        {
            this.Get<ElementPage>().Click(elementLocator);
            this.Get<ElementPage>().GetElement(nameof(Resources.SelectBoxOptionByValue), UIContent.Conversion["SegmentName"]).Click();
        }

        [When(@"I provided value (for "".*"" "".*"" "".*"" "".*"")")]
        public void WhenIProvidedValueFor(ElementLocator elementLocator)
        {
            try
            {
                if (elementLocator == null)
                {
                    throw new ArgumentNullException(nameof(elementLocator));
                }
                else
                {
                    switch (elementLocator.Value)
                    {
                        case ConstantValues.SegmentName:
                            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(this.ScenarioContext["SegmentName"].ToString() + OpenQA.Selenium.Keys.Enter);
                            break;
                        case ConstantValues.NodeName:
                            this.Get<ElementPage>().GetElement(nameof(Resources.FilterForNodeName)).SendKeys(this.ScenarioContext["NodeName"].ToString() + OpenQA.Selenium.Keys.Enter);
                            break;
                        case ConstantValues.NodeTypeName:
                            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(this.ScenarioContext["NodeTypeName"].ToString() + OpenQA.Selenium.Keys.Enter);
                            break;
                        case ConstantValues.OperatorName:
                            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(this.ScenarioContext["OperatorName"].ToString() + OpenQA.Selenium.Keys.Enter);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (ArgumentNullException e)
            {
                Logger.Log(NLog.LogLevel.Error, e.Message, e);
            }
        }
    }
}