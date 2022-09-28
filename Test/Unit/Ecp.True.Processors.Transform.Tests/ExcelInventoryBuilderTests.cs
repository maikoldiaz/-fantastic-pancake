// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelInventoryBuilderTests.cs" company="Microsoft">
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
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Transform.Builders.Excel.Interfaces;
    using Ecp.True.Processors.Transform.Builders.Excel.Movement;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The ExcelInventoryBuilderTests.
    /// </summary>
    [TestClass]
    public class ExcelInventoryBuilderTests
    {
        /// <summary>
        /// The mock owner builder.
        /// </summary>
        private Mock<IExcelOwnerBuilder> mockOwnerBuilder;

        /// <summary>
        /// The mock attribute builder.
        /// </summary>
        private Mock<IExcelAttributeBuilder> mockAttributeBuilder;

        /// <summary>
        /// The excel product builder.
        /// </summary>
        private ExcelInventoryBuilder excelInventoryBuilder;

        /// <summary>
        /// Intilizes the method.
        /// </summary>
        [TestInitialize]
        public void IntilizeMethod()
        {
            this.mockOwnerBuilder = new Mock<IExcelOwnerBuilder>();
            this.mockAttributeBuilder = new Mock<IExcelAttributeBuilder>();
            this.excelInventoryBuilder = new ExcelInventoryBuilder(this.mockOwnerBuilder.Object, this.mockAttributeBuilder.Object);
        }

        [TestMethod]
        public async Task ExcelInventoryBuilder_ShouldBuildAsync()
        {
            this.mockAttributeBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Attributes\":[{\"AttributeId\":\"AttributeId\",\"AttributeValue\":\"1\",\"ValueAttributeUnit\":\"ValueAttributeUnit\",\"AttributeDescription\":\"AttributeDescription\"}]}"));
            this.mockOwnerBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Owners\":[{\"OwnerId\":\"OwnerId\",\"OwnershipValue\":\"1\",\"OwnershipValueUnit\":\"OwnershipValueUnit\"}]}"));

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

                    DataColumn date = new DataColumn("FechaInventario");
                    date.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(date);

                    dt.Columns.Add("NodeId");
                    dt.Columns.Add("BatchId");
                    dt.Columns.Add("Tanque");
                    DataColumn scenarioId = new DataColumn("IdEscenario");
                    scenarioId.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(scenarioId);
                    dt.Columns.Add("Observaciones");

                    DataColumn percentage = new DataColumn("Tolerancia");
                    percentage.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(percentage);

                    dt.Columns.Add("SistemaOrigen");

                    dt.Columns.Add("Producto");
                    dt.Columns.Add("TipoProducto");
                    DataColumn netVolume = new DataColumn("VolumenProductoNSV");
                    netVolume.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(netVolume);
                    DataColumn grossVolume = new DataColumn("VolumenProductoGSV");
                    grossVolume.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(grossVolume);
                    dt.Columns.Add("UnidadMedida");
                    DataColumn grossVolume1 = new DataColumn("CantidadNeta");
                    grossVolume1.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(grossVolume1);
                    DataColumn grossVolume2 = new DataColumn("CantidadBruta");
                    grossVolume2.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(grossVolume2);
                    dt.Columns.Add("Incertidumbre");
                    dt.Columns.Add("Version");
                    dt.Columns.Add("Sistema");
                    dt.Columns.Add("Operador");

                    var currentDate = DateTime.UtcNow;
                    dt.Rows.Add("1234", 1, "SourceSystem", "DestinationSystem", "EventType", "InventoryId", currentDate, "NodeId", "BatchId", "Tank", 1, "Observaciones", 20, "EXCEL", "CRUDO CAMPO MAMBO", "CRUDO", 5, 10, "bbl",5,10,20,"version", "Sistema", "Operador");
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    var result = await this.excelInventoryBuilder.BuildAsync(excelInput).ConfigureAwait(false);

                    Assert.IsNotNull(result);
                    Assert.AreEqual("EXCEL", result["SourceSystemId"].Value<string>());
                    Assert.AreEqual("TRUE", result["DestinationSystem"].Value<string>());
                    Assert.AreEqual("Insert", result["EventType"].Value<string>());
                    Assert.AreEqual("1234", result["InventoryId"].Value<string>());
                    Assert.AreEqual(currentDate, result["InventoryDate"].Value<DateTime>());
                    Assert.AreEqual(1, result["NodeId"].Value<int>());
                    Assert.AreEqual("Observaciones", result["Observations"].Value<string>());
                    Assert.AreEqual(20, result["UncertaintyPercentage"].Value<int>());
                    Assert.AreEqual("CRUDO CAMPO MAMBO", result["ProductId"].Value<string>());
                    Assert.AreEqual("CRUDO", result["ProductType"].Value<string>());
                    Assert.AreEqual(5, result["ProductVolume"].Value<int>());
                    Assert.AreEqual(10, result["GrossStandardQuantity"].Value<int>());
                    Assert.AreEqual("bbl", result["MeasurementUnit"].Value<string>());
                    Assert.AreEqual(1, result["Owners"].Children().Count());
                    Assert.AreEqual("OwnerId", result["Owners"].Children().First()["OwnerId"].Value<string>());
                    Assert.AreEqual("1", result["Owners"].Children().First()["OwnershipValue"].Value<string>());
                    Assert.AreEqual("OwnershipValueUnit", result["Owners"].Children().First()["OwnershipValueUnit"].Value<string>());
                    Assert.AreEqual(1, result["Attributes"].Children().Count());
                    Assert.AreEqual("AttributeId", result["Attributes"].Children().First()["AttributeId"].Value<string>());
                    Assert.AreEqual("1", result["Attributes"].Children().First()["AttributeValue"].Value<string>());
                    Assert.AreEqual("ValueAttributeUnit", result["Attributes"].Children().First()["ValueAttributeUnit"].Value<string>());
                    Assert.AreEqual("AttributeDescription", result["Attributes"].Children().First()["AttributeDescription"].Value<string>());
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelInventoryBuilder_ShouldThrowArgumentException_ForInvalid_FechaInventarioAsync()
        {
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
                    dt.Columns.Add("FechaInventario");
                    dt.Columns.Add("NodeId");
                    dt.Columns.Add("Escenario");
                    dt.Columns.Add("Observaciones");

                    DataColumn percentage = new DataColumn("Tolerancia");
                    percentage.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(percentage);

                    dt.Columns.Add("SistemaOrigen");

                    dt.Rows.Add("1234", 1, "SourceSystem", "DestinationSystem", "EventType", "InventoryId", DateTime.UtcNow, "NodeId", "Escenario", "Observaciones", 20, "Excel-Ocenca");
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    await this.excelInventoryBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Excels inventory builder should throw argument exception for invalid volumen producto NSV datatype asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelInventoryBuilder_ShouldThrowArgumentException_ForInvalidVolumenProductoNSVDatatype_Async()
        {
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Test"))
                {
                    dt.Columns.Add("IdMovimiento");
                    dt.Columns.Add("IdPropietario");
                    dt.Columns.Add("ValorPropiedad");
                    dt.Columns.Add("UnidadValorPropiedad");
                    dt.Columns.Add("IdNodo");
                    dt.Columns.Add("Producto");
                    dt.Columns.Add("TipoProducto");
                    dt.Columns.Add("VolumenProductoNSV");
                    DataColumn productVolumeGsv = new DataColumn("VolumenProductoGSV");
                    productVolumeGsv.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(productVolumeGsv);

                    dt.Columns.Add("UnidadMedida");
                    dt.Columns.Add("IdInventario");
                    dt.Rows.Add(1, "OwnerId", 1, "OwnershipValueUnit", 1, "PROD1", "TipoProducto", 10, 10, "UnidadMedida", "INV12");
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);
                    await this.excelInventoryBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Excels the inventory builder should throw argument exception for invalid volumen producto GSV datatype asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelInventoryBuilder_ShouldThrowArgumentException_ForInvalidVolumenProductoGSVDatatype_Async()
        {
            this.mockAttributeBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Attributes\":[{\"AttributeId\":\"AttributeId\",\"AttributeValue\":\"1\",\"ValueAttributeUnit\":\"ValueAttributeUnit\",\"AttributeDescription\":\"AttributeDescription\"}]}"));
            this.mockOwnerBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Owners\":[{\"OwnerId\":\"OwnerId\",\"OwnershipValue\":\"1\",\"OwnershipValueUnit\":\"OwnershipValueUnit\"}]}"));
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Test"))
                {
                    dt.Columns.Add("IdMovimiento");
                    dt.Columns.Add("IdPropietario");
                    dt.Columns.Add("ValorPropiedad");
                    dt.Columns.Add("UnidadValorPropiedad");
                    dt.Columns.Add("IdNodo");
                    dt.Columns.Add("Producto");
                    dt.Columns.Add("TipoProducto");
                    DataColumn productVolumeNsv = new DataColumn("VolumenProductoNSV");
                    productVolumeNsv.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(productVolumeNsv);
                    dt.Columns.Add("VolumenProductoGSV");
                    dt.Columns.Add("UnidadMedida");
                    dt.Columns.Add("IdInventario");
                    dt.Rows.Add(1, "OwnerId", 1, "OwnershipValueUnit", 1, "PROD1", "TipoProducto", 10, 10, "UnidadMedida", "INV12");

                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);
                    await this.excelInventoryBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelInventoryBuilder_ShouldThrowArgumentException_ForInvalidScenarioIdDatatype_Async()
        {
            this.mockAttributeBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Attributes\":[{\"AttributeId\":\"AttributeId\",\"AttributeValue\":\"1\",\"ValueAttributeUnit\":\"ValueAttributeUnit\",\"AttributeDescription\":\"AttributeDescription\"}]}"));
            this.mockOwnerBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Owners\":[{\"OwnerId\":\"OwnerId\",\"OwnershipValue\":\"1\",\"OwnershipValueUnit\":\"OwnershipValueUnit\"}]}"));

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

                    DataColumn date = new DataColumn("FechaInventario");
                    date.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(date);

                    dt.Columns.Add("NodeId");
                    dt.Columns.Add("BatchId");
                    dt.Columns.Add("Tanque");
                    dt.Columns.Add("Escenario");
                    dt.Columns.Add("Observaciones");

                    DataColumn percentage = new DataColumn("Tolerancia");
                    percentage.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(percentage);

                    dt.Columns.Add("SistemaOrigen");

                    dt.Columns.Add("Producto");
                    dt.Columns.Add("TipoProducto");
                    DataColumn netVolume = new DataColumn("VolumenProductoNSV");
                    netVolume.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(netVolume);
                    DataColumn grossVolume = new DataColumn("VolumenProductoGSV");
                    grossVolume.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(grossVolume);
                    dt.Columns.Add("UnidadMedida");
                    DataColumn grossVolume1 = new DataColumn("CantidadNeta");
                    grossVolume1.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(grossVolume1);
                    DataColumn grossVolume2 = new DataColumn("CantidadBruta");
                    grossVolume2.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(grossVolume2);
                    dt.Columns.Add("IdEscenario");
                    dt.Columns.Add("Incertidumbre");
                    dt.Columns.Add("Version");
                    dt.Columns.Add("Sistema");
                    dt.Columns.Add("Operador");

                    var currentDate = DateTime.UtcNow;
                    dt.Rows.Add("1234", 1, "SourceSystem", "DestinationSystem", "EventType", "InventoryId", currentDate, "NodeId", "BatchId", "Tank", "Escenario", "Observaciones", 20, "Excel-Ocenca", "CRUDO CAMPO MAMBO", "CRUDO", 5, 10, "bbl", 5, 10, "id", 20, "version", "Sistema", "Operador");
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    var result = await this.excelInventoryBuilder.BuildAsync(excelInput).ConfigureAwait(false);

                    Assert.IsNotNull(result);
                    Assert.AreEqual("Excel-Ocenca", result["SourceSystem"].Value<string>());
                    Assert.AreEqual("TRUE", result["DestinationSystem"].Value<string>());
                    Assert.AreEqual("Insert", result["EventType"].Value<string>());
                    Assert.AreEqual("1234", result["InventoryId"].Value<string>());
                    Assert.AreEqual(currentDate, result["InventoryDate"].Value<DateTime>());
                    Assert.AreEqual(1, result["NodeId"].Value<int>());
                    Assert.AreEqual("Observaciones", result["Observations"].Value<string>());
                    Assert.AreEqual(20, result["UncertaintyPercentage"].Value<int>());
                    Assert.AreEqual("CRUDO CAMPO MAMBO", result["ProductId"].Value<string>());
                    Assert.AreEqual("CRUDO", result["ProductType"].Value<string>());
                    Assert.AreEqual(5, result["ProductVolume"].Value<int>());
                    Assert.AreEqual(10, result["GrossStandardQuantity"].Value<int>());
                    Assert.AreEqual("bbl", result["MeasurementUnit"].Value<string>());
                    Assert.AreEqual(1, result["Owners"].Children().Count());
                    Assert.AreEqual("OwnerId", result["Owners"].Children().First()["OwnerId"].Value<string>());
                    Assert.AreEqual("1", result["Owners"].Children().First()["OwnershipValue"].Value<string>());
                    Assert.AreEqual("OwnershipValueUnit", result["Owners"].Children().First()["OwnershipValueUnit"].Value<string>());
                    Assert.AreEqual(1, result["Attributes"].Children().Count());
                    Assert.AreEqual("AttributeId", result["Attributes"].Children().First()["AttributeId"].Value<string>());
                    Assert.AreEqual("1", result["Attributes"].Children().First()["AttributeValue"].Value<string>());
                    Assert.AreEqual("ValueAttributeUnit", result["Attributes"].Children().First()["ValueAttributeUnit"].Value<string>());
                    Assert.AreEqual("AttributeDescription", result["Attributes"].Children().First()["AttributeDescription"].Value<string>());
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public async Task ExcelInventoryBuilder_ShouldThrowArgumentException_ForInvalidInventoryId_Async()
        {
            this.mockAttributeBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Attributes\":[{\"AttributeId\":\"AttributeId\",\"AttributeValue\":\"1\",\"ValueAttributeUnit\":\"ValueAttributeUnit\",\"AttributeDescription\":\"AttributeDescription\"}]}"));
            this.mockOwnerBuilder.Setup(x => x.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(JObject.Parse("{\"Owners\":[{\"OwnerId\":\"OwnerId\",\"OwnershipValue\":\"1\",\"OwnershipValueUnit\":\"OwnershipValueUnit\"}]}"));

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

                    DataColumn date = new DataColumn("FechaInventario");
                    date.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(date);

                    dt.Columns.Add("NodeId");
                    dt.Columns.Add("BatchId");
                    dt.Columns.Add("Tanque");
                    DataColumn scenarioId = new DataColumn("IdEscenario");
                    scenarioId.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(scenarioId);
                    dt.Columns.Add("Observaciones");

                    DataColumn percentage = new DataColumn("Tolerancia");
                    percentage.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(percentage);

                    dt.Columns.Add("SistemaOrigen");

                    dt.Columns.Add("Producto");
                    dt.Columns.Add("TipoProducto");
                    DataColumn netVolume = new DataColumn("VolumenProductoNSV");
                    netVolume.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(netVolume);
                    DataColumn grossVolume = new DataColumn("VolumenProductoGSV");
                    grossVolume.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(grossVolume);
                    dt.Columns.Add("UnidadMedida");
                    DataColumn grossVolume1 = new DataColumn("CantidadNeta");
                    grossVolume1.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(grossVolume1);
                    DataColumn grossVolume2 = new DataColumn("CantidadBruta");
                    grossVolume2.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(grossVolume2);
                    dt.Columns.Add("Incertidumbre");
                    dt.Columns.Add("Version");
                    dt.Columns.Add("Sistema");
                    dt.Columns.Add("Operador");

                    var currentDate = DateTime.UtcNow;
                    dt.Rows.Add("123413241234123423412342341241234124124242412424214241241234", 1, "SourceSystem", "DestinationSystem", "EventType", "InventoryId", currentDate, "NodeId", "BatchId", "Tank", 1, "Observaciones", 20, "Excel-Ocenca", "CRUDO CAMPO MAMBO", "CRUDO", 5, 10, "bbl", 5, 10, 20, "version", "Sistema", "Operador");
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    await this.excelInventoryBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }
    }
}
