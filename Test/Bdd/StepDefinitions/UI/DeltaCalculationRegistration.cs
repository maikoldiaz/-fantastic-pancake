// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaCalculationRegistration.cs" company="Microsoft">
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
    using System.IO;
    using System.Threading.Tasks;
    using AventStack.ExtentReports.Utils;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Utils;
    using global::Bdd.Core.Utils;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;
    using OfficeOpenXml;
    using TechTalk.SpecFlow;

    [Binding]
    public class DeltaCalculationRegistration : EcpWebStepDefinitionBase
    {
        [Given(@"I have movement with only source node is a ""(.*)""")]
        public void GivenIHaveMovementWithOnlySourceNodeIsA(string value)
        {
            this.SetValue("OnlyWithSourceNodeValue", "True");
            this.SetValue("EventProcessing", value);
        }

        [Given(@"I update the excel ""(.*)"" with only ""(.*)"" node and to get negative delta")]
        public void GivenIUpdateTheExcelWithOnlySourceNodeAndToGetNegativeDelta(string fileName, string node)
        {
            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));
            //// For Inventories
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                int net = 10000;
                worksheet = package.Workbook.Worksheets[0];
                var id = worksheet.Cells[i, 12].Value;
                worksheet.Cells[i, 12].Value = id.ToInt() + net;
            }

            //// For Movements
            worksheet = package.Workbook.Worksheets[3];
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                int inc = 10000;
                worksheet = package.Workbook.Worksheets[3];
                var oldvale = worksheet.Cells[i, 15].Value;
                worksheet.Cells[i, 15].Value = oldvale.ToInt() + inc;
            }

            if (node.ContainsIgnoreCase("destination"))
            {
                worksheet = package.Workbook.Worksheets[3];
                for (int i = 2; i <= worksheet.Dimension.Rows; i++)
                {
                    worksheet = package.Workbook.Worksheets[3];
                    worksheet.Cells[i, 8].Value = null;
                    worksheet.Cells[i, 9].Value = null;
                }
            }
            else
            {
                worksheet = package.Workbook.Worksheets[3];
                for (int i = 2; i <= worksheet.Dimension.Rows; i++)
                {
                    worksheet = package.Workbook.Worksheets[3];
                    worksheet.Cells[i, 11].Value = null;
                    worksheet.Cells[i, 12].Value = null;
                }
            }

            package.Save();
            package.Dispose();
        }

        [Then(@"verify new movement ""(.*)"" is created with destination node and destination product")]
        public async Task ThenVerifyNewMovementIsCreatedWithDestinationNodeAndDestinationProductAsync(string response)
        {
            if (response.EndsWithIgnoreCase("resultadoMovimientos"))
            {
                string fileNameValue;
                var ticketData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetTicketIdwithTicketDesc).ConfigureAwait(false);
                fileNameValue = string.Concat(ticketData["TicketId"].ToString(), "_response");
                this.ScenarioContext["json"] = await fileNameValue.DeltafromBlobAsync().ConfigureAwait(false);
                var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
                int ficoMovementValue = (int)ficoRequestData[response]["idMovimientoTRUE"];

                var newMovementId = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetNewMovementTransactionId, args: new { movementTransactionId = ficoMovementValue }).ConfigureAwait(false);

                var newDestinationNodeAndProduct = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetMovementDestinationNodeAndProduct, args: new { movementTransactionId = newMovementId }).ConfigureAwait(false);
                var oldSourceProductAndNode = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetMovementSourceNodeAndProduct, args: new { movementTransactionId = ficoMovementValue }).ConfigureAwait(false);

                if (newDestinationNodeAndProduct.Equals(oldSourceProductAndNode))
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.IsTrue(false);
                }
            }
            else
            {
                string fileNameValue;
                var ticketData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetTicketIdwithTicketDesc).ConfigureAwait(false);
                fileNameValue = string.Concat(ticketData["TicketId"].ToString(), "_response");
                this.ScenarioContext["json"] = await fileNameValue.DeltafromBlobAsync().ConfigureAwait(false);
                var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
                int ficoMovementValue = (int)ficoRequestData[response]["idInventarioTRUE"];
                var newMovementId = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetNewMovementTransactionId, args: new { movementTransactionId = ficoMovementValue }).ConfigureAwait(false);
                var newDestinationNodeAndProduct = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetMovementDestinationNodeAndProduct, args: new { movementTransactionId = newMovementId }).ConfigureAwait(false);
                var oldSourceProductAndNode = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetMovementSourceNodeAndProduct, args: new { movementTransactionId = ficoMovementValue }).ConfigureAwait(false);

                if (newDestinationNodeAndProduct.Equals(oldSourceProductAndNode))
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.IsTrue(false);
                }
            }
        }

        [Then(@"verify ""(.*)"" node and product should be null for new movement ""(.*)""")]
        public async Task ThenVerifySourceNodeAndProductShouldBeNullForNewMovementAsync(string response, string node)
        {
            if (node.EndsWithIgnoreCase("node"))
            {
                string fileNameValue;
                var ticketData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetTicketIdwithTicketDesc).ConfigureAwait(false);
                fileNameValue = string.Concat(ticketData["TicketId"].ToString(), "_response");
                this.ScenarioContext["json"] = await fileNameValue.DeltafromBlobAsync().ConfigureAwait(false);
                var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
                int ficoMovementValue = (int)ficoRequestData[response]["idMovimientoTRUE"];
                var newMovementId = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetNewMovementTransactionId, args: new { movementTransactionId = ficoMovementValue }).ConfigureAwait(false);

                ////var newDestinationNodeAndProduct = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetNewMovementDestinationNodeAndProduct, args: new { movementTransactionId = newMovementId }).ConfigureAwait(false);
                var newSourceProductAndNode = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetMovementSourceNodeAndProduct, args: new { movementTransactionId = newMovementId }).ConfigureAwait(false);
                if (newSourceProductAndNode.IsNullOrEmpty())
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.IsTrue(false);
                }
            }
            else
            {
                string fileNameValue;
                var ticketData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetTicketIdwithTicketDesc).ConfigureAwait(false);
                fileNameValue = string.Concat(ticketData["TicketId"].ToString(), "_response");
                this.ScenarioContext["json"] = await fileNameValue.DeltafromBlobAsync().ConfigureAwait(false);
                var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
                int ficoMovementValue = (int)ficoRequestData[response]["idMovimientoTRUE"];
                var newMovementId = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetNewMovementTransactionId, args: new { movementTransactionId = ficoMovementValue }).ConfigureAwait(false);

                var newDestinationNodeAndProduct = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetMovementDestinationNodeAndProduct, args: new { movementTransactionId = newMovementId }).ConfigureAwait(false);
                if (newDestinationNodeAndProduct.IsNullOrEmpty())
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.IsTrue(false);
                }
            }
        }

        [Then(@"I verify newly created movement sourceNode, destinationNode and movementType must be the same type of operational movement with the ""(.*)"" which has a positive sign")]
        public async Task ThenIVerifyNewlyCreatedMovementValuesWithTheWhichHasAPositiveSignAsync(string response)
        {
            if (response.EndsWithIgnoreCase("resultadoMovimientos"))
            {
                string fileNameValue;
                var ticketData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetTicketIdwithTicketDesc).ConfigureAwait(false);
                fileNameValue = string.Concat(ticketData["TicketId"].ToString(), "_response");
                this.ScenarioContext["json"] = await fileNameValue.DeltafromBlobAsync().ConfigureAwait(false);
                var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
                int ficoMovementValue = (int)ficoRequestData[response][1]["idMovimientoTRUE"];

                var sourceNode = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetSourceNodeOfOperationalMovement, args: new { movementTransactionId = ficoMovementValue }).ConfigureAwait(false);
                var destinationNode = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetDestinationNodeOfOperationalMovement, args: new { movementTransactionId = ficoMovementValue }).ConfigureAwait(false);

                var movementTypeIdOperational = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetMovementTypeIdOperational, args: new { movementTransactionId = ficoMovementValue }).ConfigureAwait(false);
                var movementTypeIdFICO = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetMovementTypeIdFICO, args: new { sourceMovementTransactionId = ficoMovementValue }).ConfigureAwait(false);

                if (sourceNode.Equals(destinationNode) && movementTypeIdOperational.Equals(movementTypeIdFICO))
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.IsTrue(false);
                }
            }
            else
            {
                string fileNameValue;
                var ticketData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetTicketIdwithTicketDesc).ConfigureAwait(false);
                fileNameValue = string.Concat(ticketData["TicketId"].ToString(), "_response");
                this.ScenarioContext["json"] = await fileNameValue.DeltafromBlobAsync().ConfigureAwait(false);
                var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
                int ficoInventoryValue = (int)ficoRequestData[response][1]["idInventarioTRUE"];

                var sourceNode = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetSourceNodeOfOperationalMovement, args: new { movementTransactionId = ficoInventoryValue }).ConfigureAwait(false);
                var destinationNode = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetDestinationNodeOfOperationalMovement, args: new { movementTransactionId = ficoInventoryValue }).ConfigureAwait(false);

                var movementTypeIdOperational = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetMovementTypeIdOperational, args: new { movementTransactionId = ficoInventoryValue }).ConfigureAwait(false);
                var movementTypeIdFICO = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetMovementTypeIdFICO, args: new { sourceMovementTransactionId = ficoInventoryValue }).ConfigureAwait(false);

                if (sourceNode.Equals(destinationNode) && movementTypeIdOperational.Equals(movementTypeIdFICO))
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.IsTrue(false);
                }
            }
        }

        [Then(@"Verify the product type must be the same as the operational movement of ""(.*)""")]
        public async Task ThenVerifyTheProductTypeMustBeTheSameAsTheOperationalMovementOfAsync(string response)
        {
            if (response.EndsWithIgnoreCase("resultadoMovimientos"))
            {
                string fileNameValue;
                var ticketData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetTicketIdwithTicketDesc).ConfigureAwait(false);
                fileNameValue = string.Concat(ticketData["TicketId"].ToString(), "_response");
                this.ScenarioContext["json"] = await fileNameValue.DeltafromBlobAsync().ConfigureAwait(false);
                var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
                int ficoMovementValue = (int)ficoRequestData[response][1]["idMovimientoTRUE"];

                var sourceProduct = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetSourceProductId, args: new { movementTransactionId = ficoMovementValue }).ConfigureAwait(false);
                var destinationProduct = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetDestinationProductId, args: new { movementTransactionId = ficoMovementValue }).ConfigureAwait(false);

                if (sourceProduct.Equals(destinationProduct))
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.IsTrue(false);
                }
            }
            else
            {
                string fileNameValue;
                var ticketData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetTicketIdwithTicketDesc).ConfigureAwait(false);
                fileNameValue = string.Concat(ticketData["TicketId"].ToString(), "_response");
                this.ScenarioContext["json"] = await fileNameValue.DeltafromBlobAsync().ConfigureAwait(false);
                var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
                int ficoInventoryValue = (int)ficoRequestData[response][1]["idInventarioTRUE"];

                var sourceProduct = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetSourceProductId, args: new { movementTransactionId = ficoInventoryValue }).ConfigureAwait(false);
                var destinationProduct = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetDestinationProductId, args: new { movementTransactionId = ficoInventoryValue }).ConfigureAwait(false);

                if (sourceProduct.Equals(destinationProduct))
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.IsTrue(false);
                }
            }
        }

        [Then(@"Verify the unit, segment, scenario must be the same as the operational movement of ""(.*)""")]
        public async Task ThenVerifyTheUnitSegmentScenarioMustBeTheSameAsTheOperationalMovementOfAsync(string response)
        {
            if (response.EndsWithIgnoreCase("resultadoMovimientos"))
            {
                string fileNameValue;
                var ticketData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetTicketIdwithTicketDesc).ConfigureAwait(false);
                fileNameValue = string.Concat(ticketData["TicketId"].ToString(), "_response");
                this.ScenarioContext["json"] = await fileNameValue.DeltafromBlobAsync().ConfigureAwait(false);
                var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
                int ficoMovementValue = (int)ficoRequestData[response][1]["idMovimientoTRUE"];

                List<string> systemOperationalvalues = new List<string>();
                List<string> ficoGeneratedvalues = new List<string>();

                var operationalvalues = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetSegSceUnitOperational, args: new { movementTransactionId = ficoMovementValue }).ConfigureAwait(false);
                var dboperationalvalues = operationalvalues.ToDictionaryList();
                int dboperationalvaluesCount = dboperationalvalues.Count;

                for (int currentIndex = 0; currentIndex < dboperationalvaluesCount; currentIndex++)
                {
                    systemOperationalvalues.Add(dboperationalvalues[currentIndex]["SegmentId"].ToString());
                    systemOperationalvalues.Add(dboperationalvalues[currentIndex]["MeasurementUnit"].ToString());
                    systemOperationalvalues.Add(dboperationalvalues[currentIndex]["ScenarioId"].ToString());
                }

                var dbficoGeneratedvalues = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetSegSceUnitFICO, args: new { sourceMovementTransactionId = ficoMovementValue }).ConfigureAwait(false);
                var dbficoGeneratedvaluesdict = dbficoGeneratedvalues.ToDictionaryList();
                int dbficoGeneratedvaluesCount = dbficoGeneratedvaluesdict.Count;

                for (int currentIndex = 0; currentIndex < dbficoGeneratedvaluesCount; currentIndex++)
                {
                    ficoGeneratedvalues.Add(dbficoGeneratedvaluesdict[currentIndex]["SegmentId"].ToString());
                    ficoGeneratedvalues.Add(dbficoGeneratedvaluesdict[currentIndex]["MeasurementUnit"].ToString());
                    ficoGeneratedvalues.Add(dbficoGeneratedvaluesdict[currentIndex]["ScenarioId"].ToString());
                }

                if (ficoGeneratedvalues.Equals(systemOperationalvalues))
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.IsTrue(false);
                }
            }
            else
            {
                string fileNameValue;
                var ticketData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetTicketIdwithTicketDesc).ConfigureAwait(false);
                fileNameValue = string.Concat(ticketData["TicketId"].ToString(), "_response");
                this.ScenarioContext["json"] = await fileNameValue.DeltafromBlobAsync().ConfigureAwait(false);
                var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
                int ficoMovementValue = (int)ficoRequestData[response][1]["idInventarioTRUE"];

                List<string> systemOperationalvalues = new List<string>();
                List<string> ficoGeneratedvalues = new List<string>();

                var inventoryvalues = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetSegSceUnitInventory, args: new { inventoryProductId = ficoMovementValue }).ConfigureAwait(false);
                var dboperationalvalues = inventoryvalues.ToDictionaryList();
                int dboperationalvaluesCount = dboperationalvalues.Count;

                for (int currentIndex = 0; currentIndex < dboperationalvaluesCount; currentIndex++)
                {
                    systemOperationalvalues.Add(dboperationalvalues[currentIndex]["SegmentId"].ToString());
                    systemOperationalvalues.Add(dboperationalvalues[currentIndex]["MeasurementUnit"].ToString());
                    systemOperationalvalues.Add(dboperationalvalues[currentIndex]["ScenarioId"].ToString());
                }

                var dbficoGeneratedvalues = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetSegSceUnitFICO, args: new { sourceMovementTransactionId = ficoMovementValue }).ConfigureAwait(false);
                var dbficoGeneratedvaluesdict = dbficoGeneratedvalues.ToDictionaryList();
                int dbficoGeneratedvaluesCount = dbficoGeneratedvaluesdict.Count;

                for (int currentIndex = 0; currentIndex < dbficoGeneratedvaluesCount; currentIndex++)
                {
                    ficoGeneratedvalues.Add(dbficoGeneratedvaluesdict[currentIndex]["SegmentId"].ToString());
                    ficoGeneratedvalues.Add(dbficoGeneratedvaluesdict[currentIndex]["MeasurementUnit"].ToString());
                    ficoGeneratedvalues.Add(dbficoGeneratedvaluesdict[currentIndex]["ScenarioId"].ToString());
                }

                if (ficoGeneratedvalues.Equals(systemOperationalvalues))
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.IsTrue(false);
                }
            }
        }

        [Then(@"verify the net quantity value must be the ""(.*)"" value and source movement or inventory identifier must be the ""(.*)"" returned by FICO ""(.*)""")]
        public async Task ThenVerifyTheNetQuantityValueMustBeTheValueAndSourceMovementIdentifierMustBeTheReturnedByFICOAsync(string delta, string source, string result)
        {
            if (result.ContainsIgnoreCase("resultadoMovimientos"))
            {
                string fileNameValue;
                var ticketData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetTicketIdwithTicketDesc).ConfigureAwait(false);
                fileNameValue = string.Concat(ticketData["TicketId"].ToString(), "_response");
                this.ScenarioContext["json"] = await fileNameValue.DeltafromBlobAsync().ConfigureAwait(false);
                var ficoResponseData = JObject.Parse(this.ScenarioContext["json"].ToString());
                List<string> deltaResponsevalues = new List<string>();
                List<string> dbmovementValues = new List<string>();

                string ficoMovementValue = (string)ficoResponseData[result][1][source];
                string ficoDeltaValue = (string)ficoResponseData[result][1][delta];
                deltaResponsevalues.Add(ficoMovementValue);
                deltaResponsevalues.Add(ficoDeltaValue);

                var newMovement = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetDeltaAndSourceMovementTrans, args: new { sourceMovementTransactionId = ficoMovementValue }).ConfigureAwait(false);
                var dbvalues = newMovement.ToDictionaryList();
                foreach (var value in dbvalues)
                {
                    dbmovementValues.Add(value.ToString());
                }

                var sourceSystem = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetSourceSystemFromNewMovement, args: new { sourceMovementTransactionId = ficoMovementValue }).ConfigureAwait(false);

                if (deltaResponsevalues.Equals(dbmovementValues) && sourceSystem.ToString().EqualsIgnoreCase(ConstantValues.FICO))
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.IsTrue(false);
                }
            }
            else
            {
                string fileNameValue;
                var ticketData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetTicketIdwithTicketDesc).ConfigureAwait(false);
                fileNameValue = string.Concat(ticketData["TicketId"].ToString(), "_response");
                this.ScenarioContext["json"] = await fileNameValue.DeltafromBlobAsync().ConfigureAwait(false);
                var ficoResponseData = JObject.Parse(this.ScenarioContext["json"].ToString());
                List<string> deltaResponsevalues = new List<string>();
                List<string> dbmovementValues = new List<string>();

                string ficoMovementValue = (string)ficoResponseData[result][1][source];
                string ficoDeltaValue = (string)ficoResponseData[result][1][delta];
                deltaResponsevalues.Add(ficoMovementValue);
                deltaResponsevalues.Add(ficoDeltaValue);

                var newMovement = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetDeltaAndSourceInventoryId, args: new { sourceInventoryProductId = ficoMovementValue }).ConfigureAwait(false);
                var dbvalues = newMovement.ToDictionaryList();
                foreach (var value in dbvalues)
                {
                    dbmovementValues.Add(value.ToString());
                }

                var sourceSystem = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetSourceSystemFromNewInventoryProdId, args: new { sourceInventoryProductId = ficoMovementValue }).ConfigureAwait(false);

                if (deltaResponsevalues.Equals(dbmovementValues) && sourceSystem.ToString().EqualsIgnoreCase(ConstantValues.FICO))
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.IsTrue(false);
                }
            }
        }

        [Then(@"verify the ""(.*)"" has new movements in system with delta ticket and their owners")]
        public async Task ThenVerifyTheHasNewMovementsInSystemWithDeltaTicketAndTheirOwnersAsync(string movememntID)
        {
            if (movememntID.EndsWithIgnoreCase("resultadoMovimientos"))
            {
                string fileNameValue;
                var ticketData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetTicketIdwithTicketDesc).ConfigureAwait(false);
                fileNameValue = string.Concat(ticketData["TicketId"].ToString(), "_response");
                this.ScenarioContext["json"] = await fileNameValue.DeltafromBlobAsync().ConfigureAwait(false);
                var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
                int ficoMovementValue = (int)ficoRequestData[movememntID][1]["idMovimientoTRUE"];

                var newMovement = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetFicoSourceMovement, args: new { SourceMovementTransactionId = ficoMovementValue }).ConfigureAwait(false);
                var newMovementDeltaTicket = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetDeltaTicketIdFromMovement, args: new { SourceMovementTransactionId = ficoMovementValue }).ConfigureAwait(false);
                Assert.IsNotNull(newMovement);
                Assert.IsTrue(newMovementDeltaTicket.Equals(ticketData));
            }
            else
            {
                string fileNameValue;
                var ticketData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetTicketIdwithTicketDesc).ConfigureAwait(false);
                ////fileNameValue = "24649_response";
                fileNameValue = string.Concat(ticketData["TicketId"].ToString(), "_response");
                this.ScenarioContext["json"] = await fileNameValue.DeltafromBlobAsync().ConfigureAwait(false);
                var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
                int ficoInventoryValue = (int)ficoRequestData[movememntID][1]["idInventarioTRUE"];

                var newMovement = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetFicoSourceMovement, args: new { SourceMovementTransactionId = ficoInventoryValue }).ConfigureAwait(false);
                var newMovementDeltaTicket = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetDeltaTicketIdFromMovement, args: new { SourceMovementTransactionId = ficoInventoryValue }).ConfigureAwait(false);
                Assert.IsNotNull(newMovement);
                Assert.IsTrue(newMovementDeltaTicket.Equals(ticketData));
            }
        }
    }
}
