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
    using global::Bdd.Core.Web.Utils;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using OpenQA.Selenium;

    using TechTalk.SpecFlow;

    [Binding]
    public class ValidateCreationOfTicketsAndTriggeringOfConsolidationProcessSteps : EcpWebStepDefinitionBase
    {
        [When(@"select a time period from the Processing Period dropdown")]
        public async Task WhenSelectATimePeriodFromTheProcessingPeriodDropdownAsync()
        {
            this.Get<ElementPage>().Click(nameof(Resources.ProcessingPeriodDorpdown));
            await Task.Delay(1000).ConfigureAwait(true);
            this.Get<ElementPage>().Click(nameof(Resources.ProcessingPeriodDropdownValue));
        }

        [When(@"I save the values of Period start and end dates from UI")]
        public void WhenISaveTheValuesOfPeriodStartAndEndDatesFromUI()
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            this.SetValue("OfficialDeltaStartDate", this.Get<ElementPage>().GetElementText(nameof(Resources.PeriodFromDateDuringOfficialDeltaCalculation)));
            this.SetValue("OfficialDeltaEndDate", this.Get<ElementPage>().GetElementText(nameof(Resources.PeriodEndDateDuringOfficialDeltaCalculation)));
        }

        [StepDefinition(@"I wait for Official Delta Calculation process to complete")]
        public async Task WhenIWaitForOfficialDeltaCalculationProcessToCompleteAsync()
        {
            await this.WaitForOfficialDeltaCalculationAsync().ConfigureAwait(true);
        }

        [Then(@"validate the following values for each chosen segment")]
        public async Task WhenValidateTheFollowingValuesForEachChosenSegmentAsync(Table table)
        {
            bool validationOfNonNullValues = false;
            bool validationOfStartAndEndDates = false;
            bool validationOfCurrentDate = false;
            bool validationOfState = false;

            var page = this.Get<ElementPage>();

            page.Get<ElementPage>().ClearText(nameof(Resources.SegmentFilterOnOfficialDeltaCalculationTable));
            page.Get<ElementPage>().SendEnterKey(nameof(Resources.SegmentFilterOnOfficialDeltaCalculationTable));
            page.Get<ElementPage>().SetValue(nameof(Resources.SegmentFilterOnOfficialDeltaCalculationTable), this.ScenarioContext["CategorySegment"].ToString());
            await Task.Delay(1500).ConfigureAwait(true);
            page.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));

            var dict = table.Rows.ToDictionary(r => r[0]);

            int currentColumnIndex = 0;

            Dictionary<string, string> columnNamesAndValuesFromUI = new Dictionary<string, string>();

            foreach (string columnName in dict.Keys)
            {
                currentColumnIndex = this.GetTheIndexOfColumn(columnName);
                switch (columnName)
                {
                    case "Tiquete":
                        currentColumnIndex += 1;
                        string ticketID = page.GetElementText(nameof(Resources.RowElementsOnOfficialDeltaCalculationTable1), formatArgs: currentColumnIndex);
                        this.SetValue("OfficialDeltaTicketForSegment", ticketID);
                        columnNamesAndValuesFromUI.Add("Tiquete", ticketID);
                        break;
                    case "Fecha Inicial":
                        columnNamesAndValuesFromUI.Add("Fecha Inicial", page.GetElementText(nameof(Resources.RowElementsOnOfficialDeltaCalculationTable1), formatArgs: currentColumnIndex));
                        break;
                    case "Fecha Final":
                        columnNamesAndValuesFromUI.Add("Fecha Final", page.GetElementText(nameof(Resources.RowElementsOnOfficialDeltaCalculationTable1), formatArgs: currentColumnIndex));
                        break;
                    case "Fecha Ejecucion":
                        columnNamesAndValuesFromUI.Add("Fecha Ejecución", page.GetElementText(nameof(Resources.RowElementsOnOfficialDeltaCalculationTable1), formatArgs: currentColumnIndex));
                        break;
                    case "Estado":
                        currentColumnIndex -= 1;
                        columnNamesAndValuesFromUI.Add("Estado", page.GetElementText(nameof(Resources.RowElementsOnOfficialDeltaCalculationTable1), formatArgs: currentColumnIndex));
                        break;
                    case "Usuario":
                        currentColumnIndex += 1;
                        columnNamesAndValuesFromUI.Add("Usuario", page.GetElementText(nameof(Resources.RowElementsOnOfficialDeltaCalculationTable2), formatArgs: currentColumnIndex));
                        break;
                }
            }

            if (columnNamesAndValuesFromUI["Tiquete"].Length != 0 && columnNamesAndValuesFromUI["Usuario"].Length != 0)
            {
                validationOfNonNullValues = true;
            }

            if (columnNamesAndValuesFromUI["Fecha Inicial"].EqualsIgnoreCase(this.GetValue("OfficialDeltaStartDate")) && columnNamesAndValuesFromUI["Fecha Final"].EqualsIgnoreCase(this.GetValue("OfficialDeltaEndDate")))
            {
                validationOfStartAndEndDates = true;
            }

            string date = DateTime.UtcNow.ToString("dd-MMM-yy", CultureInfo.InvariantCulture);
            if (columnNamesAndValuesFromUI["Fecha Ejecución"].ContainsIgnoreCase(date))
            {
                validationOfCurrentDate = true;
            }

            if (columnNamesAndValuesFromUI["Estado"].EqualsIgnoreCase("Deltas"))
            {
                validationOfState = true;
            }

            Assert.IsTrue(validationOfNonNullValues && validationOfStartAndEndDates && validationOfCurrentDate && validationOfState);
        }

        [Then(@"I validate that the segment ticket numbers are associated to the corresponding nodes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "will revert")]
        public async Task WhenIValidateThatTheSegmentTicketNumbersAreAssociatedToTheCorrespondingNodesAsync()
        {
            bool validationOfFirstNode = false;
            bool validationOfSecondNode = false;

            var resultantDeltaNodeTable = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetNodeIdsForAGivenTicketId, args: new { TicketId = this.GetValue("OfficialDeltaTicketForSegment").ToString() }).ConfigureAwait(false);
            var nodeIdsFromColumn = resultantDeltaNodeTable.ToDictionaryList();

            Console.WriteLine("Db:'{0}', Sc:'{1}'", nodeIdsFromColumn[0]["NodeId"], this.ScenarioContext["NodeId_1"].ToString());
            Console.WriteLine("Db:'{0}', Sc:'{1}'", nodeIdsFromColumn[1]["NodeId"], this.ScenarioContext["NodeId_2"].ToString());

            if (nodeIdsFromColumn[0]["NodeId"].ToString().EqualsIgnoreCase(this.ScenarioContext["NodeId_1"].ToString()))
            {
                validationOfFirstNode = true;
            }

            if (nodeIdsFromColumn[1]["NodeId"].ToString().EqualsIgnoreCase(this.ScenarioContext["NodeId_2"].ToString()))
            {
                validationOfSecondNode = true;
            }

            ////Assert.IsTrue(nodeIdsFromColumn[0]["NodeId"].ToString().Equals(this.ScenarioContext["NodeId_1"].ToString()) && nodeIdsFromColumn[1]["NodeId"].Equals(this.ScenarioContext["NodeId_2"].ToString()));
            Assert.IsTrue(validationOfFirstNode && validationOfSecondNode);
        }

        public int GetTheIndexOfColumn(string columnName)
        {
            int indexOfRequiredColumn = 0;

            if (columnName.EqualsIgnoreCase("Fecha Ejecucion"))
            {
                columnName = "Fecha Ejecución";
            }

            IList<IWebElement> columnNames = this.Get<ElementPage>().GetElements(nameof(Resources.ColumnNamesOnOfficialDeltaCalculationTable));
            for (int indexOfColumn = 0; indexOfColumn < columnNames.Count; indexOfColumn++)
            {
                if (columnNames[indexOfColumn].Text.Trim().EqualsIgnoreCase(columnName))
                {
                    indexOfRequiredColumn = indexOfColumn;
                    break;
                }
            }

            return indexOfRequiredColumn;
        }
    }
}
