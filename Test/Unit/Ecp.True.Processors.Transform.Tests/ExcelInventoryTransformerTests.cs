// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelInventoryTransformerTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Transform.Builders.Excel.Interfaces;
    using Ecp.True.Processors.Transform.Services.Excel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json.Linq;

    [TestClass]
    public class ExcelInventoryTransformerTests
    {
        /// <summary>
        /// The inventory builder.
        /// </summary>
        private Mock<IExcelInventoryBuilder> mockInventoryBuilder;

        /// <summary>
        /// The node inventories.
        /// </summary>
        private Mock<IDictionary<string, DataRow>> mockNodeInventories;

        /// <summary>
        /// The excel inventory transformer.
        /// </summary>
        private ExcelInventoryTransformer excelInventoryTransformer;

        /// <summary>
        /// Intilizes the method.
        /// </summary>
        [TestInitialize]
        public void IntilizeMethod()
        {
            this.mockInventoryBuilder = new Mock<IExcelInventoryBuilder>();
            this.mockNodeInventories = new Mock<IDictionary<string, DataRow>>();
            this.excelInventoryTransformer = new ExcelInventoryTransformer(this.mockInventoryBuilder.Object);
        }

        /// <summary>
        /// Excels the inventory transformer should transform inventory when invoke asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExcelInventoryTransformer_ShouldTransformInventory_WhenInvokeAsync()
        {
            this.mockInventoryBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Inventories\":{\"SourceSystem\": \"EXCEL\",\"DestinationSystem\": \"TRUE\",\"EventType\": \"Insert\",\"InventoryId\": \"1\",\"InventoryDate\": \"NodeId\",\"NodeId\": \"SourceSystem\",\"Scenario\": \"Observaciones\",\"Observations\": \"Tolerancia\",\"Tolerance\": null,\"Products\": [{\"Products\": {\"ProductId\": \"PROD1\",\"ProductType\": \"\",\"ProductVolume\": null,\"GrossProductVolume\": null,\"MeasurementUnit\": \"\",\"Owners\": [{\"OwnerId\": \"OwnerId\",\"OwnershipValue\": \"1\",\"OwnershipValueUnit\": \"OwnershipValueUnit\"}],\"Attributes\": [{\"AttributeId\": \"AttributeId\",\"AttributeValue\": \"1\",\"ValueAttributeUnit\": \"ValueAttributeUnit\",\"AttributeDescription\": \"AttributeDescription\"}]}}]}}"));
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("INVENTARIOS"))
                {
                    dt.Columns.Add("IdInventario");
                    dt.Columns.Add("IdNodo");
                    dt.Columns.Add("SourceSystem");
                    dt.Columns.Add("DestinationSystem");
                    dt.Columns.Add("EventType");
                    dt.Columns.Add("InventoryId");
                    dt.Columns.Add("MovementId");
                    dt.Columns.Add("IdTipoMovimiento");
                    dt.Columns.Add("FechaHoraInicial");
                    dt.Columns.Add("VolumenBruto");
                    dt.Columns.Add("VolumenNeto");
                    dt.Columns.Add("UnidadMedida");
                    dt.Columns.Add("FechaInventario");
                    dt.Columns.Add("NodeId");
                    dt.Columns.Add("Escenario");
                    dt.Columns.Add("Observaciones");
                    dt.Columns.Add("Tolerancia");

                    dt.Rows.Add(1, "IdInventario", "IdNodo", "SourceSystem", "DestinationSystem", "EventType", "InventoryId", "MovementId", "IdTipoMovimiento", "FechaHoraInicial", "VolumenBruto", "VolumenNeto", "UnidadMedida", "FechaInventario", "NodeId", "Escenario", "Observaciones");
                    ds.Tables.Add(dt);
                    var message = new TrueMessage();
                    ExcelInput excelInput = new ExcelInput(ds, message);

                    var result = await this.excelInventoryTransformer.TransformInventoryAsync(excelInput).ConfigureAwait(false);
                    Assert.IsNotNull(result);
                }
            }
        }

        /// <summary>
        /// Excels the inventory transformer should populate pending transactions for exceptions when invoke asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExcelInventoryTransformer_shouldPopulatePendingTransactions_For_Exceptions_WhenInvokeAsync()
        {
            this.mockInventoryBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ThrowsAsync(new Exception());
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("INVENTARIOS"))
                {
                    dt.Columns.Add("IdInventario");
                    dt.Columns.Add("IdNodo");
                    dt.Columns.Add("SourceSystem");
                    dt.Columns.Add("DestinationSystem");
                    dt.Columns.Add("EventType");
                    dt.Columns.Add("InventoryId");
                    dt.Columns.Add("MovementId");
                    dt.Columns.Add("IdTipoMovimiento");
                    dt.Columns.Add("FechaHoraInicial");
                    dt.Columns.Add("VolumenBruto");
                    dt.Columns.Add("VolumenNeto");
                    dt.Columns.Add("UnidadMedida");
                    dt.Columns.Add("FechaInventario");
                    dt.Columns.Add("NodeId");
                    dt.Columns.Add("Escenario");
                    dt.Columns.Add("Observaciones");
                    dt.Columns.Add("Tolerancia");

                    dt.Rows.Add(1, "IdInventario", "IdNodo", "SourceSystem", "DestinationSystem", "EventType", "InventoryId", "MovementId", "IdTipoMovimiento", "FechaHoraInicial", "VolumenBruto", "VolumenNeto", "UnidadMedida", "FechaInventario", "NodeId", "Escenario", "Observaciones");
                    ds.Tables.Add(dt);
                    var message = new TrueMessage();
                    ExcelInput excelInput = new ExcelInput(ds, message);

                    var output = await this.excelInventoryTransformer.TransformInventoryAsync(excelInput).ConfigureAwait(false);

                    Assert.IsNotNull(output);
                    Assert.IsTrue(excelInput.Message.PendingTransactions.Any());
                }
            }
        }

        /// <summary>
        /// Excels the inventory transformer should throw missing field exception when invoke asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(MissingFieldException))]
        public async Task ExcelInventoryTransformer_ShouldThrowMissingFieldException_WhenInvokeAsync()
        {
            this.mockInventoryBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Inventories\":{\"SourceSystem\": \"EXCEL\",\"DestinationSystem\": \"TRUE\",\"EventType\": \"Insert\",\"InventoryId\": \"1\",\"InventoryDate\": \"NodeId\",\"NodeId\": \"SourceSystem\",\"Scenario\": \"Observaciones\",\"Observations\": \"Tolerancia\",\"Tolerance\": null,\"Products\": [{\"Products\": {\"ProductId\": \"PROD1\",\"ProductType\": \"\",\"ProductVolume\": null,\"GrossProductVolume\": null,\"MeasurementUnit\": \"\",\"Owners\": [{\"OwnerId\": \"OwnerId\",\"OwnershipValue\": \"1\",\"OwnershipValueUnit\": \"OwnershipValueUnit\"}],\"Attributes\": [{\"AttributeId\": \"AttributeId\",\"AttributeValue\": \"1\",\"ValueAttributeUnit\": \"ValueAttributeUnit\",\"AttributeDescription\": \"AttributeDescription\"}]}}]}}"));
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable())
                {
                    dt.Columns.Add("IdInventario");
                    dt.Columns.Add("IdNodo");
                    dt.Columns.Add("SourceSystem");
                    dt.Columns.Add("DestinationSystem");
                    dt.Columns.Add("EventType");
                    dt.Columns.Add("InventoryId");
                    dt.Columns.Add("MovementId");
                    dt.Columns.Add("IdTipoMovimiento");
                    dt.Columns.Add("FechaHoraInicial");
                    dt.Columns.Add("VolumenBruto");
                    dt.Columns.Add("VolumenNeto");
                    dt.Columns.Add("UnidadMedida");
                    dt.Columns.Add("FechaInventario");
                    dt.Columns.Add("NodeId");
                    dt.Columns.Add("Escenario");
                    dt.Columns.Add("Observaciones");
                    dt.Columns.Add("Tolerancia");

                    dt.Rows.Add(1, "IdInventario", "IdNodo", "SourceSystem", "DestinationSystem", "EventType", "InventoryId", "MovementId", "IdTipoMovimiento", "FechaHoraInicial", "VolumenBruto", "VolumenNeto", "UnidadMedida", "FechaInventario", "NodeId", "Escenario", "Observaciones");
                    ds.Tables.Add(dt);
                    var message = new TrueMessage();
                    ExcelInput excelInput = new ExcelInput(ds, message);

                    await this.excelInventoryTransformer.TransformInventoryAsync(excelInput).ConfigureAwait(false);
                }
            }
        }
    }
}
