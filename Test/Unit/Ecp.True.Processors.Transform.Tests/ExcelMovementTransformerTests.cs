// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelMovementTransformerTests.cs" company="Microsoft">
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
    public class ExcelMovementTransformerTests
    {
        /// <summary>
        /// The movement builder.
        /// </summary>
        private Mock<IExcelMovementBuilder> mockMovementBuilder;

        /// <summary>
        /// The excel inventory transformer.
        /// </summary>
        private ExcelMovementTransformer excelMovementTransformer;

        [TestInitialize]
        public void IntilizeMethod()
        {
            this.mockMovementBuilder = new Mock<IExcelMovementBuilder>();
            this.excelMovementTransformer = new ExcelMovementTransformer(this.mockMovementBuilder.Object);
        }

        /// <summary>
        /// Excels the movement transformer should transform movement when invoke asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExcelMovementTransformer_ShouldTransformMovement__WhenInvokeAsync()
        {
            this.mockMovementBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Movements\":{\"SourceSystem\": \"EXCEL\",\"EventType\": \"Insert\",\"MovementId\": \"1\",\"MovementTypeId\": \"MovementId\",\"OperationalDate\": \"IdTipoMovimiento\",\"GrossStandardVolume\": \"FechaHoraInicial\",\"NetStandardVolume\": \"VolumenBruto\",\"MeasurementUnit\": \"UnidadMedida\",\"Scenario\": \"Escenario\",\"Observations\": \"Observaciones\",\"Classification\": \"Clasificacion\",\"Tolerance\": \"Tolerancia\",\"Period\": {\"StartTime\": \"IdTipoMovimiento\",\"EndTime\": \"OrigenMovimiento\"},\"MovementSource\": {\"SourceNodeId\": \"IdProductoOrigen\",\"SourceProductId\": \"IdTipoProductoOrigen\",\"SourceProductTypeId\": \"DestinoMovimiento\"},\"MovementDestination\": {\"DestinationNodeId\": \"IdProductoDestino\",\"DestinationProductId\": \"IdTipoProductoDestino\",\"DestinationProductTypeId\": null},\"Owners\": [{\"OwnerId\": \"OwnerId\",\"OwnershipValue\": \"1\",\"OwnershipValueUnit\": \"OwnershipValueUnit\"}],\"Attributes\": [{\"AttributeId\": \"AttributeId\",\"AttributeValue\": \"1\",\"ValueAttributeUnit\": \"ValueAttributeUnit\",\"AttributeDescription\": \"AttributeDescription\"}]}}"));
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("MOVIMIENTOS"))
                {
                    dt.Columns.Add("SourceSystem");
                    dt.Columns.Add("EventType");
                    dt.Columns.Add("MovementId");
                    dt.Columns.Add("IdTipoMovimiento");
                    dt.Columns.Add("FechaHoraInicial");
                    dt.Columns.Add("VolumenBruto");
                    dt.Columns.Add("VolumenNeto");
                    dt.Columns.Add("UnidadMedida");
                    dt.Columns.Add("Escenario");
                    dt.Columns.Add("Observaciones");
                    dt.Columns.Add("Clasificacion");
                    dt.Columns.Add("Tolerancia");

                    dt.Rows.Add(1, "SourceSystem", "EventType", "MovementId", "IdTipoMovimiento", "FechaHoraInicial", "VolumenBruto", "UnidadMedida", "Escenario", "Observaciones", "Clasificacion", "Tolerancia");
                    ds.Tables.Add(dt);
                    var message = new TrueMessage();
                    ExcelInput excelInput = new ExcelInput(ds, message);

                    var result = await this.excelMovementTransformer.TransformMovementAsync(excelInput).ConfigureAwait(false);
                    Assert.IsNotNull(result);
                }
            }
        }

        /// <summary>
        /// Excels the movement transformer should populate pending transactions when invoke asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExcelMovementTransformer_ShouldPopulatePendingTransactions_WhenInvokeAsync()
        {
            this.mockMovementBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ThrowsAsync(new Exception());
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("MOVIMIENTOS"))
                {
                    dt.Columns.Add("SourceSystem");
                    dt.Columns.Add("EventType");
                    dt.Columns.Add("MovementId");
                    dt.Columns.Add("IdTipoMovimiento");
                    dt.Columns.Add("FechaHoraInicial");
                    dt.Columns.Add("VolumenBruto");
                    dt.Columns.Add("VolumenNeto");
                    dt.Columns.Add("UnidadMedida");
                    dt.Columns.Add("Escenario");
                    dt.Columns.Add("Observaciones");
                    dt.Columns.Add("Clasificacion");
                    dt.Columns.Add("Tolerancia");

                    dt.Rows.Add(1, "SourceSystem", "EventType", "MovementId", "IdTipoMovimiento", "FechaHoraInicial", "VolumenBruto", "UnidadMedida", "Escenario", "Observaciones", "Clasificacion", "Tolerancia");
                    ds.Tables.Add(dt);
                    var message = new TrueMessage();
                    ExcelInput excelInput = new ExcelInput(ds, message);

                    var result = await this.excelMovementTransformer.TransformMovementAsync(excelInput).ConfigureAwait(false);

                    Assert.IsNotNull(result);
                    Assert.IsTrue(excelInput.Message.PendingTransactions.Any());
                }
            }
        }

        /// <summary>
        /// Excels the movement transformer should throw missing field exception when invoke asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(MissingFieldException))]
        public async Task ExcelMovementTransformer_shouldThrowMissingFieldException_whenInvokeAsync()
        {
            this.mockMovementBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Movements\":{\"SourceSystem\": \"EXCEL\",\"EventType\": \"Insert\",\"MovementId\": \"1\",\"MovementTypeId\": \"MovementId\",\"OperationalDate\": \"IdTipoMovimiento\",\"GrossStandardVolume\": \"FechaHoraInicial\",\"NetStandardVolume\": \"VolumenBruto\",\"MeasurementUnit\": \"UnidadMedida\",\"Scenario\": \"Escenario\",\"Observations\": \"Observaciones\",\"Classification\": \"Clasificacion\",\"Tolerance\": \"Tolerancia\",\"Period\": {\"StartTime\": \"IdTipoMovimiento\",\"EndTime\": \"OrigenMovimiento\"},\"MovementSource\": {\"SourceNodeId\": \"IdProductoOrigen\",\"SourceProductId\": \"IdTipoProductoOrigen\",\"SourceProductTypeId\": \"DestinoMovimiento\"},\"MovementDestination\": {\"DestinationNodeId\": \"IdProductoDestino\",\"DestinationProductId\": \"IdTipoProductoDestino\",\"DestinationProductTypeId\": null},\"Owners\": [{\"OwnerId\": \"OwnerId\",\"OwnershipValue\": \"1\",\"OwnershipValueUnit\": \"OwnershipValueUnit\"}],\"Attributes\": [{\"AttributeId\": \"AttributeId\",\"AttributeValue\": \"1\",\"ValueAttributeUnit\": \"ValueAttributeUnit\",\"AttributeDescription\": \"AttributeDescription\"}]}}"));
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable())
                {
                    dt.Columns.Add("SourceSystem");
                    dt.Columns.Add("EventType");
                    dt.Columns.Add("MovementId");
                    dt.Columns.Add("IdTipoMovimiento");
                    dt.Columns.Add("FechaHoraInicial");
                    dt.Columns.Add("VolumenBruto");
                    dt.Columns.Add("VolumenNeto");
                    dt.Columns.Add("UnidadMedida");
                    dt.Columns.Add("Escenario");
                    dt.Columns.Add("Observaciones");
                    dt.Columns.Add("Clasificacion");
                    dt.Columns.Add("Tolerancia");

                    dt.Rows.Add(1, "SourceSystem", "EventType", "MovementId", "IdTipoMovimiento", "FechaHoraInicial", "VolumenBruto", "UnidadMedida", "Escenario", "Observaciones", "Clasificacion", "Tolerancia");
                    ds.Tables.Add(dt);
                    var message = new TrueMessage();
                    ExcelInput excelInput = new ExcelInput(ds, message);

                    await this.excelMovementTransformer.TransformMovementAsync(excelInput).ConfigureAwait(false);
                }
            }
        }
    }
}