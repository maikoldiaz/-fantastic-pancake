namespace Ecp.True.Bdd.Tests.StepDefinitions.UI
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Ocaramba;

    using OpenQA.Selenium;

    using TechTalk.SpecFlow;

    [Binding]
    public class ValidateFicoErrorMessagesDuringOfficialDeltaCalculationSteps : EcpWebStepDefinitionBase
    {
        [Given(@"I set the prerequisite to have a ""(.*)"" error generated")]
        public async Task GivenISetThePrerequisiteToHaveAErrorGeneratedAsync(string errorType)
        {
            string errorMessage = string.Empty;

            switch (errorType)
            {
                case "consolidation":
                    errorMessage = "Se presentó un error técnico inesperado en la consolidación del escenario operativo. Por favor ejecute nuevamente el proceso.";
                    break;
                case "delta":
                    errorMessage = "Se presentó un error técnico inesperado al enviar la información al motor de reglas para el cálculo de deltas oficiales. Por favor ejecute nuevamente el proceso.";
                    break;
                case "backend validations":
                    errorMessage = "El segmento no tiene información oficial pendiente en el período de fechas.";
                    break;
                case "business":
                    errorMessage = "Error deliberado debido al encabezado del caos.";
                    break;
            }

            string startDate = DateTime.UtcNow.AddDays(-15).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 00:00:00.000";
            string endDate = DateTime.UtcNow.AddDays(-13).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 00:00:00.000";

            switch (errorType)
            {
                case "consolidation":
                case "delta":
                case "backend validations":
                    string segmentId = "10";

                    var resultFromCategoryElementTable = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetSegmentNameUsingSegmentId, args: new { CategoryElementId = segmentId }).ConfigureAwait(false);
                    var categoryElementTableAsDict = resultFromCategoryElementTable.ToDictionaryList();

                    this.SetValue("CurrentSegmentName", categoryElementTableAsDict[0]["Name"].ToString());

                    var resultFromTicketTable = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.CreateTechnicalErrorOfGivenType, args: new { CategoryElementId = segmentId, StartDate = startDate, EndDate = endDate, TechnicalError = errorMessage }).ConfigureAwait(false);
                    var ticketTableAsDict = resultFromTicketTable.ToDictionaryList();

                    this.SetValue("OfficialDeltaTicketIdForError", ticketTableAsDict[0]["TicketId"].ToString());
                    this.SetValue("StartDateOfOfficialDelta", DateTime.UtcNow.AddDays(-15).ToString("dd-MMM-yy", CultureInfo.InvariantCulture));
                    this.SetValue("EndDateOfOfficialDelta", DateTime.UtcNow.AddDays(-13).ToString("dd-MMM-yy", CultureInfo.InvariantCulture));

                    break;
                case "business":
                    ////////var resultFromInventoryProductTable = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetInventoryProductIdUsingSegmentId, args: new { SegmentId = this.GetValue("SegmentId") }).ConfigureAwait(false);
                    ////var resultFromInventoryProductTable = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetInventoryProductIdUsingSegmentId, args: new { SegmentId = "166549" }).ConfigureAwait(false);
                    ////var inventoryProductTableAsDict = resultFromInventoryProductTable.ToDictionaryList();

                    ////////var resultFromMovementTable = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetMovementTransactionIdUsingSegmentId, args: new { SegmentId = this.GetValue("SegmentId") }).ConfigureAwait(false);
                    ////var resultFromMovementTable = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetMovementTransactionIdUsingSegmentId, args: new { SegmentId = "166549" }).ConfigureAwait(false);
                    ////var movementTableAsDict = resultFromMovementTable.ToDictionaryList();

                    ////var table1 = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.Query1, args: new { SegmentId = "166549", StartDate = startDate, EndDate = endDate }).ConfigureAwait(false);
                    ////var table1AsDict = table1.ToDictionaryList();

                    ////var table2 = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.Query2, args: new { TicketId = table1AsDict[0]["TicketId"].ToString(), NodeId = "39720" }).ConfigureAwait(false);
                    ////var table2AsDict = table2.ToDictionaryList();

                    ////var resultFromInventoryProductTable = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetInventoryProductIdUsingSegmentId, args: new { SegmentId = this.GetValue("SegmentId") }).ConfigureAwait(false);
                    var resultFromInventoryProductTable = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetInventoryProductIdUsingSegmentId, args: new { SegmentId = this.GetValue("SegmentId") }).ConfigureAwait(false);
                    var inventoryProductTableAsDict = resultFromInventoryProductTable.ToDictionaryList();

                    ////var resultFromMovementTable = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetMovementTransactionIdUsingSegmentId, args: new { SegmentId = this.GetValue("SegmentId") }).ConfigureAwait(false);
                    var resultFromMovementTable = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetMovementTransactionIdUsingSegmentId, args: new { SegmentId = this.GetValue("SegmentId") }).ConfigureAwait(false);
                    var movementTableAsDict = resultFromMovementTable.ToDictionaryList();

                    var table1 = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.Query1, args: new { SegmentId = this.GetValue("SegmentId"), StartDate = startDate, EndDate = endDate }).ConfigureAwait(false);
                    var table1AsDict = table1.ToDictionaryList();

                    var table2 = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.Query2, args: new { TicketId = table1AsDict[0]["TicketId"].ToString(), NodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
                    var table2AsDict = table2.ToDictionaryList();

                    ////////var resultFromTicketTableBusiness = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.CreateBusinessErrorForOfficialDelta, args: new { SegmentId = this.GetValue("SegmentId"), StartDate = startDate, EndDate = endDate, NodeId = this.GetValue("NodeId_1"), InventoryProductId = inventoryProductTableAsDict[0]["InventoryProductId"].ToString(), MovementTransactionId = movementTableAsDict[0]["MovementTransactionId"].ToString(), BusinessError = errorMessage }).ConfigureAwait(false);
                    ////var resultFromTicketTableBusiness = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.CreateBusinessErrorForOfficialDelta, args: new { DeltaNodeId = table2AsDict[0]["DeltaNodeId"].ToString(), InventoryProductId = inventoryProductTableAsDict[0]["InventoryProductId"].ToString(), MovementTransactionId = movementTableAsDict[0]["MovementTransactionId"].ToString(), BusinessError = errorMessage }).ConfigureAwait(false);
                    ////var ticketTableAsDictBusiness = resultFromTicketTableBusiness.ToDictionaryList();

                    var resultFromTicketTableBusiness = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.Query3, args: new { DeltaNodeId = table2AsDict[0]["DeltaNodeId"].ToString(), InventoryProductId = inventoryProductTableAsDict[0]["InventoryProductId"].ToString(), MovementTransactionId = movementTableAsDict[0]["MovementTransactionId"].ToString(), BusinessError = errorMessage }).ConfigureAwait(false);
                    var ticketTableAsDictBusiness = resultFromTicketTableBusiness.ToDictionaryList();

                    this.SetValue("OfficialDeltaTicketIdForError", table1AsDict[0]["TicketId"].ToString());
                    this.SetValue("ExecutionDateOfOfficialDelta", DateTime.UtcNow.ToString("dd-MMM-yy", CultureInfo.InvariantCulture));
                    this.SetValue("CurrentBusinessError", errorMessage);
                    break;
            }
        }

        [When(@"the user clicks on the error icon of the respective segment or node")]
        public async Task WhenTheUserClicksOnTheErrorIconOfTheRespectiveSegmentOrNodeAsync()
        {
            var page = this.Get<ElementPage>();
            await this.SerachForSegmentOrNodeUsingTicketIdAsync().ConfigureAwait(true);
            page.WaitUntilElementIsVisible(nameof(Resources.ErrorEyeIconOnTable));
            page.Click(nameof(Resources.ErrorEyeIconOnTable));
        }

        [When(@"the user clicks on the error icon of the business error related segment")]
        public async Task WhenTheUserClicksOnTheErrorIconOfTheBusinessErrorRelatedSegmentAsync()
        {
            var page = this.Get<ElementPage>();
            await this.SerachForSegmentOrNodeUsingTicketIdAsync().ConfigureAwait(true);
            page.WaitUntilElementIsVisible(nameof(Resources.EyeIconOfBusinessError));
            page.Click(nameof(Resources.EyeIconOfBusinessError));
        }

        [Then(@"a popup must appear with title ""(.*)""")]
        public void ThenAPopupMustAppearWithTitle(string errorOnPopup)
        {
            var page = this.Get<ElementPage>();

            page.WaitUntilElementIsVisible(nameof(Resources.CategoryHeader));
            string errorFromUI = page.GetElementText(nameof(Resources.CategoryHeader));

            Assert.IsTrue(errorOnPopup.EqualsIgnoreCase(errorFromUI));
        }

        [Then(@"I validate the error message on popup ""(.*)""")]
        public void ThenIValidateTheErrorMessageOnPopup(string expectedErrorMessage)
        {
            Verify.Equals(true, this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.CategoryHeader), formatArgs: expectedErrorMessage));
        }

        [Then(@"I validate the value ""(.*)"" on the popup")]
        public void ThenIValidateTheValueOnThePopup(string valueToBeValidated)
        {
            bool validationSuccessful = false;

            switch (valueToBeValidated)
            {
                case "segment":
                    validationSuccessful = this.Get<ElementPage>().GetElement(nameof(Resources.SegmentElementOnErrorPopup)).Text.Trim().EqualsIgnoreCase(this.GetValue("CurrentSegmentName"));
                    Assert.IsTrue(validationSuccessful);
                    break;
                case "period start date":
                    validationSuccessful = this.Get<ElementPage>().GetElement(nameof(Resources.PeroiodStartDateOnErrorPopup)).Text.Trim().EqualsIgnoreCase(this.GetValue("StartDateOfOfficialDelta"));
                    Assert.IsTrue(validationSuccessful);
                    break;
                case "period end date":
                    validationSuccessful = this.Get<ElementPage>().GetElement(nameof(Resources.PeroiodEndDateOnErrorPopup)).Text.Trim().EqualsIgnoreCase(this.GetValue("EndDateOfOfficialDelta"));
                    Assert.IsTrue(validationSuccessful);
                    break;
                case "Node":
                    validationSuccessful = this.Get<ElementPage>().GetElement(nameof(Resources.NodeNameFromBusinessErrorPopup)).Text.Trim().EqualsIgnoreCase(this.GetValue("NodeName_1"));
                    Assert.IsTrue(validationSuccessful);
                    break;
                case "Execution Date":
                    validationSuccessful = this.Get<ElementPage>().GetElement(nameof(Resources.ExecutionDateFromBusinessErrorPopup)).Text.Trim().EqualsIgnoreCase(this.GetValue("ExecutionDateOfOfficialDelta"));
                    Assert.IsTrue(validationSuccessful);
                    break;
            }
        }

        [When(@"the user clicks on the Aceptar button")]
        public void WhenTheUserClicksOnTheAceptarButton()
        {
            this.Get<ElementPage>().Click(nameof(Resources.DeltaCalAcceptBtn));
        }

        [Then(@"the error popup must disappear")]
        public void ThenTheErrorPopupMustDisappear()
        {
            Assert.IsFalse(this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.DeltaCalAcceptBtn)));
        }

        [Then(@"I validate that the table on the popup has following columns displayed with respective error messages")]
        public void ThenIValidateThatTheTableOnThePopupHasFollowingColumnsDisplayedWithRespectiveErrorMessages(Table table)
        {
            var dict = table?.Rows?.ToDictionary(r => r[0]);

            List<string> expectedColumnNames = dict.Keys.ToList();
            IList<IWebElement> columnNameElementsFromUI = this.Get<ElementPage>().GetElements(nameof(Resources.ColumnNamesOnBusinessErrorTable));

            List<string> actualColumnNamesOnUI = new List<string>();
            foreach (IWebElement columnElement in columnNameElementsFromUI)
            {
                actualColumnNamesOnUI.Add(columnElement.Text.Trim());
            }

            Assert.IsTrue(expectedColumnNames.SequenceEqual(actualColumnNamesOnUI, StringComparer.OrdinalIgnoreCase));
        }

        [Then(@"I validate that the errors are sorted by type ascending")]
        public void ThenIValidateThatTheErrorsAreSortedByTypeAscending()
        {
            IList<IWebElement> typoNames = this.Get<ElementPage>().GetElements(nameof(Resources.TypoColumnValuesFromBusinessErrorPopup));

            List<string> typoNamesInOrderDisplayedOnUI = new List<string>();
            foreach (IWebElement typoName in typoNames)
            {
                typoNamesInOrderDisplayedOnUI.Add(typoName.Text.Trim());
            }

            List<string> typoNamesAfterSort = typoNamesInOrderDisplayedOnUI;
            typoNamesAfterSort.Sort();

            Assert.IsTrue(typoNamesInOrderDisplayedOnUI.SequenceEqual(typoNamesAfterSort));
        }

        [Then(@"I validate the business error messages on the popup")]
        public void ThenIValidateTheBusinessErrorMessagesOnThePopup()
        {
            bool validationFailed = false;
            string expectedBusinessError = this.GetValue("CurrentBusinessError");
            IList<IWebElement> busniessErrors = this.Get<ElementPage>().GetElements(nameof(Resources.BusinessErrorsOnThePopup));

            foreach (IWebElement busniessError in busniessErrors)
            {
                if (!busniessError.Text.Trim().EqualsIgnoreCase(expectedBusinessError))
                {
                    validationFailed = true;
                    break;
                }
            }

            Assert.IsFalse(validationFailed);
        }

        public async Task SerachForSegmentOrNodeUsingTicketIdAsync()
        {
            var page = this.Get<ElementPage>();
            page.ClearText(nameof(Resources.TicketIdSearchBox));

            //// Search the nodes with the TicketId
            page.EnterText(nameof(Resources.TicketIdSearchBox), this.GetValue("OfficialDeltaTicketIdForError"));
            page.SendEnterKey(nameof(Resources.TicketIdSearchBox));
            await Task.Delay(1500).ConfigureAwait(false);
        }
    }
}
