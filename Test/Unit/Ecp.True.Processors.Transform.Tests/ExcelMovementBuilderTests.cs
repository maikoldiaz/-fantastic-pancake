// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelMovementBuilderTests.cs" company="Microsoft">
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
    using System.IO;
    using System.Threading.Tasks;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Transform.Builders.Excel.Interfaces;
    using Ecp.True.Processors.Transform.Builders.Excel.Movement;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json.Linq;

    [TestClass]
    public class ExcelMovementBuilderTests
    {
        /// <summary>
        /// The owner builder.
        /// </summary>
        private Mock<IExcelOwnerBuilder> mockOwnerBuilder;

        /// <summary>
        /// The attribute builder.
        /// </summary>
        private Mock<IExcelAttributeBuilder> mockAttributeBuilder;

        /// <summary>
        /// The excel movement builder.
        /// </summary>
        private ExcelMovementBuilder excelMovementBuilder;

        [TestInitialize]
        public void IntilizeMethod()
        {
            this.mockAttributeBuilder = new Mock<IExcelAttributeBuilder>();
            this.mockOwnerBuilder = new Mock<IExcelOwnerBuilder>();
            this.excelMovementBuilder = new ExcelMovementBuilder(this.mockOwnerBuilder.Object, this.mockAttributeBuilder.Object);
        }

        [TestMethod]
        public async Task ExcelMovementBuilder_ShouldBuildAsync()
        {
            this.mockAttributeBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Attributes\":[{\"AttributeId\":\"AttributeId\",\"AttributeValue\":\"1\",\"ValueAttributeUnit\":\"ValueAttributeUnit\",\"AttributeDescription\":\"AttributeDescription\"}]}"));
            this.mockOwnerBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Owners\":[{\"OwnerId\":\"OwnerId\",\"OwnershipValue\":\"1\",\"OwnershipValueUnit\":\"OwnershipValueUnit\"}]}"));
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Test"))
                {
                    dt.Columns.Add("SistemaOrigen");
                    dt.Columns.Add("IdMovimiento");
                    dt.Columns.Add("IdTipoMovimiento");

                    DataColumn initialDate = new DataColumn("FechaHoraInicial");
                    initialDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(initialDate);

                    DataColumn endDate = new DataColumn("FechaHoraFinal");
                    endDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(endDate);

                    dt.Columns.Add("OrigenMovimiento");
                    dt.Columns.Add("IdProductoOrigen");
                    dt.Columns.Add("IdTipoProductoOrigen");
                    dt.Columns.Add("DestinoMovimiento");
                    dt.Columns.Add("IdProductoDestino");
                    dt.Columns.Add("IdTipoProductoDestino");

                    DataColumn grossVolume1 = new DataColumn("CantidadNeta");
                    grossVolume1.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(grossVolume1);
                    DataColumn grossVolume2 = new DataColumn("CantidadBruta");
                    grossVolume2.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(grossVolume2);

                    dt.Columns.Add("UnidadMedida");
                    DataColumn scenarioId = new DataColumn("IdEscenario");
                    scenarioId.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(scenarioId);
                    dt.Columns.Add("Observaciones");
                    dt.Columns.Add("Clasificacion");

                    DataColumn tolerance = new DataColumn("Incertidumbre");
                    tolerance.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(tolerance);

                    dt.Columns.Add("SourceSystemId");
                    dt.Columns.Add("EventType");
                    dt.Columns.Add("MovementId");
                    dt.Columns.Add("BatchId");
                    dt.Columns.Add("Version");
                    dt.Columns.Add("Sistema");
                    dt.Columns.Add("Operador");

                    var currentDate = DateTime.UtcNow;

                    dt.Rows.Add("EXCEL", "142705", "DESPA", currentDate, currentDate, "REBOMDEOS", "CRUDO CAMPO MAMBO", "DILUYENTE", "AYACUCHO", "CRUDO CAMPO MAMBO", "CRUDOR", 200, 442558.55, "Bbl", 1, "Reporte Operativo Cusiana -Fecha", "cls", 0.22);
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    var result = await this.excelMovementBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                    Assert.IsNotNull(result);
                    Assert.AreEqual("EXCEL", result["SourceSystemId"].Value<string>());
                    Assert.AreEqual("Insert", result["EventType"].Value<string>());
                    Assert.AreEqual("142705", result["MovementId"].Value<string>());
                    Assert.AreEqual("DESPA", result["MovementTypeId"].Value<string>());
                    Assert.AreEqual(currentDate, result["OperationalDate"].Value<DateTime>());
                    Assert.AreEqual(200, result["NetStandardVolume"].Value<int>());
                    Assert.AreEqual(442558.56, Math.Round(result["GrossStandardVolume"].Value<float>(), 2));
                    Assert.AreEqual("Bbl", result["MeasurementUnit"].Value<string>());
                    Assert.AreEqual(1, result["ScenarioId"].Value<int>());
                    Assert.AreEqual("Reporte Operativo Cusiana -Fecha", result["Observations"].Value<string>());
                    Assert.AreEqual("cls", result["Classification"].Value<string>());
                    Assert.AreEqual(0.22, Math.Round(result["UncertaintyPercentage"].Value<float>(), 2));
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelMovementBuilder_ShouldThrowArgumentException_ForInvalidFechaHoraInicialAsync()
        {
            this.mockAttributeBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Attributes\":[{\"AttributeId\":\"AttributeId\",\"AttributeValue\":\"1\",\"ValueAttributeUnit\":\"ValueAttributeUnit\",\"AttributeDescription\":\"AttributeDescription\"}]}"));
            this.mockOwnerBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Owners\":[{\"OwnerId\":\"OwnerId\",\"OwnershipValue\":\"1\",\"OwnershipValueUnit\":\"OwnershipValueUnit\"}]}"));
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Test"))
                {
                    dt.Columns.Add("SistemaOrigen");
                    dt.Columns.Add("IdMovimiento");
                    dt.Columns.Add("IdTipoMovimiento");
                    dt.Columns.Add("FechaHoraInicial");

                    DataColumn endDate = new DataColumn("FechaHoraFinal");
                    endDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(endDate);

                    dt.Columns.Add("OrigenMovimiento");
                    dt.Columns.Add("IdProductoOrigen");
                    dt.Columns.Add("IdTipoProductoOrigen");
                    dt.Columns.Add("DestinoMovimiento");
                    dt.Columns.Add("IdProductoDestino");
                    dt.Columns.Add("IdTipoProductoDestino");

                    DataColumn grossVolume = new DataColumn("VolumenBruto");
                    grossVolume.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(grossVolume);

                    DataColumn netVolume = new DataColumn("VolumenNeto");
                    netVolume.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(netVolume);

                    dt.Columns.Add("UnidadMedida");
                    dt.Columns.Add("Escenario");
                    dt.Columns.Add("Observaciones");
                    dt.Columns.Add("Clasificacion");

                    DataColumn tolerance = new DataColumn("Tolerancia");
                    tolerance.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(tolerance);

                    dt.Columns.Add("SourceSystemId");
                    dt.Columns.Add("EventType");
                    dt.Columns.Add("MovementId");

                    dt.Rows.Add("EXCEL-OCENSA", "142705", "DESPA", DateTime.UtcNow, DateTime.UtcNow, "REBOMDEOS", "CRUDO CAMPO MAMBO", "DILUYENTE", "AYACUCHO", "CRUDO CAMPO MAMBO", "CRUDOR", 200, 442558.55, "Bbl", 1, "Reporte Operativo Cusiana -Fecha", "cls", 0.22);
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    await this.excelMovementBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelMovementBuilder_ShouldThrowArgumentException_ForInvalidFechaHoraFinalAsync()
        {
            this.mockAttributeBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Attributes\":[{\"AttributeId\":\"AttributeId\",\"AttributeValue\":\"1\",\"ValueAttributeUnit\":\"ValueAttributeUnit\",\"AttributeDescription\":\"AttributeDescription\"}]}"));
            this.mockOwnerBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Owners\":[{\"OwnerId\":\"OwnerId\",\"OwnershipValue\":\"1\",\"OwnershipValueUnit\":\"OwnershipValueUnit\"}]}"));
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Test"))
                {
                    dt.Columns.Add("SistemaOrigen");
                    dt.Columns.Add("IdMovimiento");
                    dt.Columns.Add("IdTipoMovimiento");

                    DataColumn initialDate = new DataColumn("FechaHoraInicial");
                    initialDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(initialDate);

                    dt.Columns.Add("FechaHoraFinal");
                    dt.Columns.Add("OrigenMovimiento");
                    dt.Columns.Add("IdProductoOrigen");
                    dt.Columns.Add("IdTipoProductoOrigen");
                    dt.Columns.Add("DestinoMovimiento");
                    dt.Columns.Add("IdProductoDestino");
                    dt.Columns.Add("IdTipoProductoDestino");

                    DataColumn grossVolume = new DataColumn("VolumenBruto");
                    grossVolume.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(grossVolume);

                    DataColumn netVolume = new DataColumn("VolumenNeto");
                    netVolume.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(netVolume);

                    dt.Columns.Add("UnidadMedida");
                    dt.Columns.Add("Escenario");
                    dt.Columns.Add("Observaciones");
                    dt.Columns.Add("Clasificacion");

                    DataColumn tolerance = new DataColumn("Tolerancia");
                    tolerance.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(tolerance);

                    dt.Columns.Add("SourceSystemId");
                    dt.Columns.Add("EventType");
                    dt.Columns.Add("MovementId");

                    dt.Rows.Add("EXCEL-OCENSA", "142705", "DESPA", DateTime.UtcNow, DateTime.UtcNow, "REBOMDEOS", "CRUDO CAMPO MAMBO", "DILUYENTE", "AYACUCHO", "CRUDO CAMPO MAMBO", "CRUDOR", 200, 442558.55, "Bbl", "Operativ", "Reporte Operativo Cusiana -Fecha", "cls", 0.22);
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    await this.excelMovementBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelMovementBuilder_ShouldThrowArgumentException_ForInvalidVolumenBrutoAsync()
        {
            this.mockAttributeBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Attributes\":[{\"AttributeId\":\"AttributeId\",\"AttributeValue\":\"1\",\"ValueAttributeUnit\":\"ValueAttributeUnit\",\"AttributeDescription\":\"AttributeDescription\"}]}"));
            this.mockOwnerBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Owners\":[{\"OwnerId\":\"OwnerId\",\"OwnershipValue\":\"1\",\"OwnershipValueUnit\":\"OwnershipValueUnit\"}]}"));
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Test"))
                {
                    dt.Columns.Add("SistemaOrigen");
                    dt.Columns.Add("IdMovimiento");
                    dt.Columns.Add("IdTipoMovimiento");

                    DataColumn initialDate = new DataColumn("FechaHoraInicial");
                    initialDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(initialDate);

                    DataColumn endDate = new DataColumn("FechaHoraFinal");
                    endDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(endDate);

                    dt.Columns.Add("OrigenMovimiento");
                    dt.Columns.Add("IdProductoOrigen");
                    dt.Columns.Add("IdTipoProductoOrigen");
                    dt.Columns.Add("DestinoMovimiento");
                    dt.Columns.Add("IdProductoDestino");
                    dt.Columns.Add("IdTipoProductoDestino");
                    dt.Columns.Add("VolumenBruto");

                    DataColumn netVolume = new DataColumn("VolumenNeto");
                    netVolume.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(netVolume);

                    dt.Columns.Add("UnidadMedida");
                    dt.Columns.Add("Escenario");
                    dt.Columns.Add("Observaciones");
                    dt.Columns.Add("Clasificacion");

                    DataColumn tolerance = new DataColumn("Tolerancia");
                    tolerance.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(tolerance);

                    dt.Columns.Add("SourceSystemId");
                    dt.Columns.Add("EventType");
                    dt.Columns.Add("MovementId");

                    dt.Rows.Add("EXCEL-OCENSA", "142705", "DESPA", DateTime.UtcNow, DateTime.UtcNow, "REBOMDEOS", "CRUDO CAMPO MAMBO", "DILUYENTE", "AYACUCHO", "CRUDO CAMPO MAMBO", "CRUDOR", 200, 442558.55, "Bbl", "Operativ", "Reporte Operativo Cusiana -Fecha", "cls", 0.22);
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    await this.excelMovementBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelMovementBuilder_ShouldThrowArgumentException_ForInvalidVolumenNetoAsync()
        {
            this.mockAttributeBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Attributes\":[{\"AttributeId\":\"AttributeId\",\"AttributeValue\":\"1\",\"ValueAttributeUnit\":\"ValueAttributeUnit\",\"AttributeDescription\":\"AttributeDescription\"}]}"));
            this.mockOwnerBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Owners\":[{\"OwnerId\":\"OwnerId\",\"OwnershipValue\":\"1\",\"OwnershipValueUnit\":\"OwnershipValueUnit\"}]}"));
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Test"))
                {
                    dt.Columns.Add("SistemaOrigen");
                    dt.Columns.Add("IdMovimiento");
                    dt.Columns.Add("IdTipoMovimiento");

                    DataColumn initialDate = new DataColumn("FechaHoraInicial");
                    initialDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(initialDate);

                    DataColumn endDate = new DataColumn("FechaHoraFinal");
                    endDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(endDate);

                    dt.Columns.Add("OrigenMovimiento");
                    dt.Columns.Add("IdProductoOrigen");
                    dt.Columns.Add("IdTipoProductoOrigen");
                    dt.Columns.Add("DestinoMovimiento");
                    dt.Columns.Add("IdProductoDestino");
                    dt.Columns.Add("IdTipoProductoDestino");

                    DataColumn grossVolume = new DataColumn("VolumenBruto");
                    grossVolume.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(grossVolume);

                    dt.Columns.Add("VolumenNeto");

                    dt.Columns.Add("UnidadMedida");
                    dt.Columns.Add("Escenario");
                    dt.Columns.Add("Observaciones");
                    dt.Columns.Add("Clasificacion");

                    DataColumn tolerance = new DataColumn("Tolerancia");
                    tolerance.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(tolerance);

                    dt.Columns.Add("SourceSystemId");
                    dt.Columns.Add("EventType");
                    dt.Columns.Add("MovementId");

                    dt.Rows.Add("EXCEL-OCENSA", "142705", "DESPA", DateTime.UtcNow, DateTime.UtcNow, "REBOMDEOS", "CRUDO CAMPO MAMBO", "DILUYENTE", "AYACUCHO", "CRUDO CAMPO MAMBO", "CRUDOR", 200, 442558.55, "Bbl", "Operativ", "Reporte Operativo Cusiana -Fecha", "cls", 0.22);
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    await this.excelMovementBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelMovementBuilder_ShouldThrowArgumentException_ForInvalidScenarioIdAsync()
        {
            this.mockAttributeBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Attributes\":[{\"AttributeId\":\"AttributeId\",\"AttributeValue\":\"1\",\"ValueAttributeUnit\":\"ValueAttributeUnit\",\"AttributeDescription\":\"AttributeDescription\"}]}"));
            this.mockOwnerBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Owners\":[{\"OwnerId\":\"OwnerId\",\"OwnershipValue\":\"1\",\"OwnershipValueUnit\":\"OwnershipValueUnit\"}]}"));
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Test"))
                {
                    dt.Columns.Add("SistemaOrigen");
                    dt.Columns.Add("IdMovimiento");
                    dt.Columns.Add("IdTipoMovimiento");

                    DataColumn initialDate = new DataColumn("FechaHoraInicial");
                    initialDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(initialDate);

                    DataColumn endDate = new DataColumn("FechaHoraFinal");
                    endDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(endDate);

                    dt.Columns.Add("OrigenMovimiento");
                    dt.Columns.Add("IdProductoOrigen");
                    dt.Columns.Add("IdTipoProductoOrigen");
                    dt.Columns.Add("DestinoMovimiento");
                    dt.Columns.Add("IdProductoDestino");
                    dt.Columns.Add("IdTipoProductoDestino");

                    DataColumn grossVolume1 = new DataColumn("CantidadNeta");
                    grossVolume1.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(grossVolume1);
                    DataColumn grossVolume2 = new DataColumn("CantidadBruta");
                    grossVolume2.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(grossVolume2);

                    dt.Columns.Add("UnidadMedida");
                    dt.Columns.Add("IdEscenario");
                    dt.Columns.Add("Observaciones");
                    dt.Columns.Add("Clasificacion");

                    DataColumn tolerance = new DataColumn("Incertidumbre");
                    tolerance.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(tolerance);

                    dt.Columns.Add("SourceSystemId");
                    dt.Columns.Add("EventType");
                    dt.Columns.Add("MovementId");
                    dt.Columns.Add("BatchId");
                    dt.Columns.Add("Version");
                    dt.Columns.Add("Sistema");
                    dt.Columns.Add("Operador");

                    var currentDate = DateTime.UtcNow;

                    dt.Rows.Add("EXCEL-OCENSA", "142705", "DESPA", currentDate, currentDate, "REBOMDEOS", "CRUDO CAMPO MAMBO", "DILUYENTE", "AYACUCHO", "CRUDO CAMPO MAMBO", "CRUDOR", 200, 442558.55, "Bbl", "Operativo", "Reporte Operativo Cusiana -Fecha", "cls", 0.22);
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    await this.excelMovementBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public async Task ExcelMovementBuilder_ShouldThrowArgumentException_ForInvalidMovementIdAsync()
        {
            this.mockAttributeBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Attributes\":[{\"AttributeId\":\"AttributeId\",\"AttributeValue\":\"1\",\"ValueAttributeUnit\":\"ValueAttributeUnit\",\"AttributeDescription\":\"AttributeDescription\"}]}"));
            this.mockOwnerBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Owners\":[{\"OwnerId\":\"OwnerId\",\"OwnershipValue\":\"1\",\"OwnershipValueUnit\":\"OwnershipValueUnit\"}]}"));
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Test"))
                {
                    dt.Columns.Add("SistemaOrigen");
                    dt.Columns.Add("IdMovimiento");
                    dt.Columns.Add("IdTipoMovimiento");

                    DataColumn initialDate = new DataColumn("FechaHoraInicial");
                    initialDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(initialDate);

                    DataColumn endDate = new DataColumn("FechaHoraFinal");
                    endDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(endDate);

                    dt.Columns.Add("OrigenMovimiento");
                    dt.Columns.Add("IdProductoOrigen");
                    dt.Columns.Add("IdTipoProductoOrigen");
                    dt.Columns.Add("DestinoMovimiento");
                    dt.Columns.Add("IdProductoDestino");
                    dt.Columns.Add("IdTipoProductoDestino");

                    DataColumn grossVolume1 = new DataColumn("CantidadNeta");
                    grossVolume1.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(grossVolume1);
                    DataColumn grossVolume2 = new DataColumn("CantidadBruta");
                    grossVolume2.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(grossVolume2);

                    dt.Columns.Add("UnidadMedida");

                    DataColumn scenarioId = new DataColumn("IdEscenario");
                    scenarioId.DataType = Type.GetType("System.Int32");
                    dt.Columns.Add(scenarioId);

                    dt.Columns.Add("Observaciones");
                    dt.Columns.Add("Clasificacion");

                    DataColumn tolerance = new DataColumn("Incertidumbre");
                    tolerance.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(tolerance);

                    dt.Columns.Add("SourceSystem");
                    dt.Columns.Add("EventType");
                    dt.Columns.Add("MovementId");
                    dt.Columns.Add("BatchId");
                    dt.Columns.Add("Version");
                    dt.Columns.Add("Sistema");
                    dt.Columns.Add("Operador");

                    var currentDate = DateTime.UtcNow;

                    dt.Rows.Add("EXCEL-OCENSA", "14212313131313131313123131313131334242411241242144142411424705", "DESPA", currentDate, currentDate, "REBOMDEOS", "CRUDO CAMPO MAMBO", "DILUYENTE", "AYACUCHO", "CRUDO CAMPO MAMBO", "CRUDOR", 200, 442558.55, "Bbl", 1, "Reporte Operativo Cusiana -Fecha", "cls", 0.22);
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    await this.excelMovementBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }
    }
}
