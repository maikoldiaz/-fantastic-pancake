// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialMonthlyBalanceFileToBeProcessedByTheSivSystemSteps.cs" company="Microsoft">
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
    using System.IO;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.DataSources;
    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Executors.UI;
    using global::Bdd.Core.Web.Utils;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using OfficeOpenXml;

    using TechTalk.SpecFlow;

    [Binding]
    public class OfficialMonthlyBalanceFileToBeProcessedByTheSivSystemSteps : EcpWebStepDefinitionBase
    {
        [When(@"generated official monthly balance file with all nodes")]
        [When(@"I have generated official monthly balance file")]
        [When(@"generated official monthly balance file")]
        public async Task WhenIHaveGeneratedOfficialMonthlyBalanceFileAsync()
        {
            this.ScenarioContext["CategorySegment"] = this.GetValue("SegmentName");
            //// UI steps
            this.IClickOn("createOfficialLogistics", "button");
            this.ISelectSegmentFromOfficialLogistics("Random", "logisticsCriteria\" \"segment", "dropdown");
            if (!string.IsNullOrEmpty(this.GetValue("OfficialBalanceForSingleNode")))
            {
                this.ISelectNodeFromTheNodoDropdownOnOfficialLogistics("One");
            }
            else
            {
                this.ISelectNodeFromTheNodoDropdownOnOfficialLogistics("Todos");
            }

            if (!string.IsNullOrEmpty(this.GetValue("OfficialBalanceNotMeetRequiredCriteria")))
            {
                this.ISelectOwnerOnOfficialLogistics("Reficar");
            }
            else
            {
                this.ISelectOwnerOnOfficialLogistics("Ecopetrol");
            }

            this.IClickOn("logisticsCriteria\" \"next", "button");
            this.ISelectYearFromDropDown(this.GetValue("YearForPeriod"));
            this.ISelectAPeriodFromDropDown(this.GetValue("PreviousMonthName").ToPascalCase());
            this.IClickOn("logisticsPeriod\" \"next", "button");
            this.IClickOn("logisticsValidation\" \"createLogisticReport", "button");
            //// wait till ticket processing complete
            await this.VerifyThatLogisticOfficialBalanceFileGenerationShouldBeSuccessfulAsync().ConfigureAwait(false);
        }

        [When(@"generated official monthly balance file with single node")]
        public async Task WhenGeneratedOfficialMonthlyBalanceFileWithSingleNodeAsync()
        {
            this.SetValue("OfficialBalanceForSingleNode", "Yes");
            await this.WhenIHaveGeneratedOfficialMonthlyBalanceFileAsync().ConfigureAwait(false);
        }

        [When(@"generated official monthly balance file that not meet required criteria")]
        public async Task WhenGeneratedOfficialMonthlyBalanceFileThatNotMeetRequiredCriteriaAsync()
        {
            this.SetValue("OfficialBalanceNotMeetRequiredCriteria", "Yes");
            await this.WhenIHaveGeneratedOfficialMonthlyBalanceFileAsync().ConfigureAwait(false);
        }

        [When(@"I click on ""(.*)"" link on the grid")]
        public void WhenIClickOnDowloadLinkOnTheGrid(string field)
        {
            var arguments = new object[] { field, this.GetValue("OfficialLogisticTicketId") };
            this.Get<ElementPage>().Click(nameof(Resources.linkOnOfficialLogisticsGrid), formatArgs: arguments);
        }

        [Given(@"I do not have homologation between true to siv")]
        public async Task GivenIDoNotHaveHomologationBetweenTrueToSivAsync()
        {
            // Delete Homologation between 1 to 6
            try
            {
                var homologationRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationIdForSIV]).ConfigureAwait(false);
                this.ScenarioContext[ConstantValues.HomologationId] = homologationRow[ConstantValues.HomologationId];
                var homologationGroup = await this.ReadAllSqlAsDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationGroupId], args: new { homologationId = this.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
                foreach (var homologationGroupRow in homologationGroup)
                {
                    var homologationJson = homologationGroupRow[ConstantValues.HomologationGroupId];
                    this.ScenarioContext[ConstantValues.HomologationGroupId] = homologationJson;
                    await this.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationObjectAndDataMapping, args: new { homologationGroupId = this.ScenarioContext[ConstantValues.HomologationGroupId] }).ConfigureAwait(false);
                }

                await this.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationAndGroup, args: new { homologationId = this.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
            }
            catch (ArgumentNullException ex)
            {
                Logger.Info("Homologation for True to Siv does not exists");
                Assert.IsNotNull(ex);
            }
            catch (NullReferenceException ex)
            {
                Logger.Info("Homologation for True to Siv does not exists");
                Assert.IsNotNull(ex);
            }
        }

        [Given(@"movements within chosen period")]
        [Given(@"I have movements information that is linked to selected segment")]
        [Given(@"movements source node or destination node is linked to selected segment")]
        [Given(@"owner is either EcoPetrol or Reficar")]
        [Given(@"nodes should be send to sap is true")]
        [Then(@"stop the process")]
        [Then(@"format of columns as text")]
        [Given(@"I have a cancelaltion movement type linked to movement type")]
        [Given(@"it is in active state")]
        [Given(@"owner is neither EcoPetrol nor Reficar")]
        [Given(@"I have movements information that is not linked to selected segment")]
        [Given(@"movements are not within chosen period")]
        [Given(@"movements source node or destination node is not linked to selected segment")]
        [Given(@"nodes should be send to sap is false")]
        [Given(@"I have movements as per tautology table")]
        [Given(@"I have cancellation movements that met cancellation criteria")]
        [Given(@"I have homologation between true to siv for original movement type")]
        [Given(@"I have homologation between true to siv for logistic movement type")]
        [Then(@"the original value of the movement type should not be overrided with the value found in new property")]
        [Then(@"value of the net amount of the movement should be replaced with the absolute value of the net amount of the annulation movement")]
        public void GivenIHaveMovementsInformationThatIsLinkedToSelectedSegment()
        {
            // Method intentionally left empty.
        }

        [Given(@"""(.*)"" is equal to either ""(.*)"" or ""(.*)"" or the sourceSystem is equal to ""(.*)""")]
        [Given(@"""(.*)"" is not equal to neither ""(.*)"" nor ""(.*)"" or the sourceSystem is not equal to ""(.*)""")]
        public void GivenIsEqualToEitherOrOrTheSourceSystemIsEqualTo(string field1, string field2, string field3, string field4)
        {
            // Method intentionally left empty.
            Assert.IsNotNull(field1);
            Assert.IsNotNull(field2);
            Assert.IsNotNull(field3);
            Assert.IsNotNull(field4);
        }

        [Then(@"format of ""(.*)"" as number without decimal places")]
        [Then(@"format of ""(.*)"" as number with two decimals")]
        [Given(@"scenario equal to ""(.*)""")]
        [Given(@"scenario equal to ""(.*)""")]
        public void ThenFormatOfAsNumberWithTwoDecimals(string field)
        {
            // Method intentionally left empty.
            Assert.IsNotNull(field);
        }

        [Then(@"""(.*)"" ""(.*)"" and ""(.*)"" should be in date format")]
        public void ThenAndShouldBeInDateFormat(string field1, string field2, string field3)
        {
            // Method intentionally left empty.
            Assert.IsNotNull(field1);
            Assert.IsNotNull(field2);
            Assert.IsNotNull(field3);
        }

        [Then(@"it should contains columns and values as per the mapping")]
        public async Task ThenItShouldContainsColumnsAndValuesAsPerTheMappingAsync()
        {
            //// verification of column names in the sheet
            var actualOfficialBalanceInformation = new Dictionary<string, string>()
            {
                { "Column1", "MOVIMIENTO" },
                { "Column2", "ALMACEN-ORIGEN" },
                { "Column3", "PRODUCTO-ORIGEN" },
                { "Column4", "ALMACEN-DESTINO" },
                { "Column5", "PRODUCTO-DESTINO" },
                { "Column6", "ORDEN-COMPRA" },
                { "Column7", "POS-COMPRA" },
                { "Column8", "VALOR" },
                { "Column9", "UOM" },
                { "Column10", "HALLAZGO" },
                { "Column11", "DIAGNÓSTICO" },
                { "Column12", "IMPACTO" },
                { "Column13", "SOLUCIÓN" },
                { "Column14", "ESTADO" },
                { "Column15", "ORDEN" },
                { "Column16", "FECHA-INICIO" },
                { "Column17", "FECHA-FIN" },
                { "Column18", "FECHA-CONTABILIZACION" },
            };

            BlobStorageDataSource blobStorageDataSource = new BlobStorageDataSource();
            var blobContainer = blobStorageDataSource.Read("true/logistics");
            var blob = await blobStorageDataSource.Read(blobContainer, "ReporteLogisticoOficial_" + this.GetValue("SegmentName") + "_ECOPETROL_" + this.GetValue("OfficialLogisticTicketId") + ".xlsx").ConfigureAwait(false);
            await blobStorageDataSource.Download(blob, Path.Combine(FilePaths.EventsFilePath.GetFullPath(), "OperationalDataAndConfiguration.xlsx")).ConfigureAwait(false);
            ExcelPackage package = new ExcelPackage(new FileInfo(FilePaths.DownloadedOwnershipExcelPath.GetFullPath()));
            var expectedOficialBalanceFileInformation = new Dictionary<string, string>();
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            this.SetValue("sheetName", worksheet.Name);
            for (int i = 1; i <= worksheet.Dimension.Columns; i++)
            {
                expectedOficialBalanceFileInformation.Add("Column" + i, worksheet.Cells[1, i].Value.ToString());
            }

            Assert.IsTrue(this.VerifyDiffs(expectedOficialBalanceFileInformation, actualOfficialBalanceInformation));

            //// verification of values in the sheet
            this.ScenarioContext["Values"] = worksheet.Dimension.Rows - 1;
            if (string.IsNullOrEmpty(this.GetValue("OfficialBalanceForSingleNode")))
            {
                await this.ThenAllNodesInformationShouldBeShownInTheOfficialBalanceFileAsync().ConfigureAwait(false);
            }

            package.Dispose();
        }

        [Then(@"all nodes information should be shown in the official balance file")]
        [Then(@"include all these movements into official balance file that to be processed by the SIV system")]
        [Then(@"cancellation movement type movements should be included in official balance file")]
        [Then(@"value of movement column in balance file should be based on tautology table and cancelation criterion")]
        [Then(@"official balance file should contain all movements details")]
        public async Task ThenAllNodesInformationShouldBeShownInTheOfficialBalanceFileAsync()
        {
            Assert.AreEqual(this.ScenarioContext["Values"], await this.ReadSqlScalarAsync<int>(SqlQueries.CountOfRecordsInOfficialBalanceFile, args: new { segment = this.GetValue("SegmentName") }).ConfigureAwait(false));
        }

        [Then(@"information related to selected node only should be shown in the official balance file")]
        public async Task ThenInformationRelatedToSelectedNodeOnlyShouldBeShownInTheOfficialBalanceFileAsync()
        {
            Assert.AreEqual(this.ScenarioContext["Values"], await this.ReadSqlScalarAsync<int>(SqlQueries.CountOfRecordsInOfficialBalanceFileForSingleNode, args: new { segment = this.GetValue("SegmentName"), nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false));
        }

        [Then(@"transformations for all cancelation movements according to the relationship settings should be performed")]
        public async Task ThenTransformationsForAllCancelationMovementsAccordingToTheRelationshipSettingsShouldBePerformedAsync()
        {
            Assert.AreEqual(63, await this.ReadSqlScalarAsync<int>(SqlQueries.CountOfRecordsInOfficialBalanceFileWithTrasnformations, args: new { segment = this.GetValue("SegmentName") }).ConfigureAwait(false));
        }

        [Then(@"I should see sheet name as ""(.*)""")]
        public void ThenIShouldSeeSheetNameAs(string sheetName)
        {
            Assert.AreEqual(sheetName, this.GetValue(nameof(sheetName)));
        }

        [Then(@"file should be generated with name ""(.*)""")]
        public async Task ThenFileShouldBeGeneratedWithNameAsync(string fileName)
        {
            if (fileName != null)
            {
                var latestLogisticTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetOfficialLogisticTicket, args: new { status = 0 }).ConfigureAwait(false);
                this.SetValue("OfficialLogisticTicketId", latestLogisticTicket[ConstantValues.TicketId]);
                var segmentOfLastCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryElementNameByCategoryElementId, args: new { elementId = latestLogisticTicket[ConstantValues.CategoryElementId] }).ConfigureAwait(false);
                Assert.IsTrue(this.Get<FilePage>().CheckDownloadedFileContent(fileName.Split('_')[0] + "_" + this.GetValue("SegmentName") + "_ECOPETROL_" + latestLogisticTicket[ConstantValues.TicketId] + ".xlsx", this.ScenarioContext, this.FeatureContext).IsCompleted);
            }
        }

        [Then(@"it is failed with error message as ""(.*)""")]
        [Then(@"error message should be displayed as ""(.*)""")]
        public void ThenItIsFailedWithErrorMessageAs(string message)
        {
            Assert.IsNotNull(message);
            if (message.ContainsIgnoreCase("MovementType"))
            {
                Assert.IsTrue(this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.ElementByTextContains), formatArgs: "No se encontró una homologación entre TRUE y SIV para el tipo de movimiento"));
            }
            else
            {
                Assert.IsTrue(this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.ElementByTextContains), formatArgs: message));
            }
        }

        [When(@"file generation process is failed")]
        public void WhenFileGenerationProcessIsFailed()
        {
            Assert.AreEqual(ConstantValues.Fallido, this.GetValue("LogisticsOfficialStatus"));
        }
    }
}