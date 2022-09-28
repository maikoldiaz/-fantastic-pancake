// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelTransformerTests.cs" company="Microsoft">
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
    using System.Data;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Transform.Services.Excel;
    using Ecp.True.Processors.Transform.Services.Excel.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json.Linq;

    [TestClass]
    public class ExcelTransformerTests
    {
        /// <summary>
        /// The movement transformer.
        /// </summary>
        private Mock<IExcelMovementTransformer> mockMovementTransformer;

        /// <summary>
        /// The inventory transformer.
        /// </summary>
        private Mock<IExcelInventoryTransformer> mockInventoryTransformer;

        /// <summary>
        /// The event transformer.
        /// </summary>
        private Mock<IExcelEventTransformer> mockIExcelEventTransformer;

        /// <summary>
        /// The event transformer.
        /// </summary>
        private Mock<IExcelContractTransformer> mockIExcelContractTransformer;

        /// <summary>
        /// The excel transformer.
        /// </summary>
        private ExcelTransformer excelTransformer;

        [TestInitialize]
        public void IntilizeMethod()
        {
            this.mockMovementTransformer = new Mock<IExcelMovementTransformer>();
            this.mockInventoryTransformer = new Mock<IExcelInventoryTransformer>();
            this.mockIExcelEventTransformer = new Mock<IExcelEventTransformer>();
            this.mockIExcelContractTransformer = new Mock<IExcelContractTransformer>();
            this.excelTransformer = new ExcelTransformer(this.mockMovementTransformer.Object, this.mockInventoryTransformer.Object, this.mockIExcelEventTransformer.Object, this.mockIExcelContractTransformer.Object);
        }

        /// <summary>
        /// Excels the transformer should transform inventory and movements asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExcelTransformer_ShouldTransformInventoryAndMovementsAsync()
        {
            this.mockInventoryTransformer.Setup(x => x.TransformInventoryAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JArray.Parse("[{\"SourceSystem\": \"EXCEL\",\"DestinationSystem\": \"TRUE\",\"EventType\": \"Insert\",\"InventoryId\": \"1\",\"InventoryDate\": \"NodeId\",\"NodeId\": \"SourceSystem\",\"Scenario\": \"Observaciones\",\"Observations\": \"Tolerancia\",\"Tolerance\": null,\"Products\": [{\"Products\": {\"ProductId\": \"PROD1\",\"ProductType\": \"\",\"ProductVolume\": null,\"GrossProductVolume\": null,\"MeasurementUnit\": \"\",\"Owners\": [{\"OwnerId\": \"OwnerId\",\"OwnershipValue\": \"1\",\"OwnershipValueUnit\": \"OwnershipValueUnit\"}],\"Attributes\": [{\"AttributeId\": \"AttributeId\",\"AttributeValue\": \"1\",\"ValueAttributeUnit\": \"ValueAttributeUnit\",\"AttributeDescription\": \"AttributeDescription\"}]}}]}]"));
            this.mockMovementTransformer.Setup(x => x.TransformMovementAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JArray.Parse("[{\"Movements\": {\"SourceSystem\": \"EXCEL\",\"EventType\": \"Insert\",\"MovementId\": \"1\",\"MovementTypeId\": \"MovementId\",\"OperationalDate\": \"IdTipoMovimiento\",\"GrossStandardVolume\": \"FechaHoraInicial\",\"NetStandardVolume\": \"VolumenBruto\",\"MeasurementUnit\": \"UnidadMedida\",\"Scenario\": \"Escenario\",\"Observations\": \"Observaciones\",\"Classification\": \"Clasificacion\",\"Tolerance\": \"Tolerancia\",\"Period\": {\"StartTime\": \"IdTipoMovimiento\",\"EndTime\": \"OrigenMovimiento\"},\"MovementSource\": {\"SourceNodeId\": \"IdProductoOrigen\",\"SourceProductId\": \"IdTipoProductoOrigen\",\"SourceProductTypeId\": \"DestinoMovimiento\"},\"MovementDestination\": {\"DestinationNodeId\": \"IdProductoDestino\",\"DestinationProductId\": \"IdTipoProductoDestino\",\"DestinationProductTypeId\": null},\"Owners\": [{\"OwnerId\": \"OwnerId\",\"OwnershipValue\": \"1\",\"OwnershipValueUnit\": \"OwnershipValueUnit\"}],\"Attributes\": [{\"AttributeId\": \"AttributeId\",\"AttributeValue\": \"1\",\"ValueAttributeUnit\": \"ValueAttributeUnit\",\"AttributeDescription\": \"AttributeDescription\"}]}}]"));
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Test"))
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
                    dt.Columns.Add("Clasificacion");
                    dt.Columns.Add("Tolerancia");

                    dt.Rows.Add(1, "IdInventario", "IdNodo", "SourceSystem", "DestinationSystem", "EventType", "InventoryId", "MovementId", "IdTipoMovimiento", "FechaHoraInicial", "VolumenBruto", "VolumenNeto", "UnidadMedida", "FechaInventario", "NodeId", "Escenario", "Observaciones", "Clasificacion");
                    ds.Tables.Add(dt);
                    var message = new TrueMessage();
                    ExcelInput excelInput = new ExcelInput(ds, message);
                    var result = await this.excelTransformer.TransformExcelAsync(excelInput).ConfigureAwait(false);
                    Assert.IsNotNull(result);
                }
            }
        }

        /// <summary>
        /// Excels the transformer should transform events asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExcelTransformer_ShouldTransformEventsAsync()
        {
            this.mockIExcelEventTransformer.Setup(x => x.TransformEventAsync(It.IsAny<ExcelInput>())).ReturnsAsync(new JArray());
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Eventos"))
                {
                    dt.Columns.Add("Column1");
                    dt.Columns.Add("Column2");
                    dt.Rows.Add("Event1", 1);
                    dt.Rows.Add("Event2", 2);

                    ds.Tables.Add(dt);
                    var message = new TrueMessage
                    {
                        SourceSystem = SystemType.EVENTS,
                    };
                    ExcelInput excelInput = new ExcelInput(ds, message);
                    var result = await this.excelTransformer.TransformExcelAsync(excelInput).ConfigureAwait(false);

                    Assert.IsNotNull(result);
                    Assert.IsInstanceOfType(result, typeof(JArray));
                }
            }
        }

        /// <summary>
        /// Excels the transformer should transform contracts asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExcelTransformer_ShouldTransformContractsAsync()
        {
            this.mockIExcelContractTransformer.Setup(x => x.TransformContractAsync(It.IsAny<ExcelInput>())).ReturnsAsync(new JArray());
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Contratos"))
                {
                    dt.Columns.Add("Column1");
                    dt.Columns.Add("Column2");
                    dt.Rows.Add("Contract1", 1);
                    dt.Rows.Add("Contract2", 2);

                    ds.Tables.Add(dt);
                    var message = new TrueMessage
                    {
                        SourceSystem = SystemType.CONTRACT,
                    };
                    ExcelInput excelInput = new ExcelInput(ds, message);
                    var result = await this.excelTransformer.TransformExcelAsync(excelInput).ConfigureAwait(false);

                    Assert.IsNotNull(result);
                    Assert.IsInstanceOfType(result, typeof(JArray));
                }
            }
        }
    }
}
