// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelContractBuilderTests.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Transform.Builders.Excel.Movement;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The ExcelContractBuilderTests.
    /// </summary>
    [TestClass]
    public class ExcelContractBuilderTests
    {
        /// <summary>
        /// The excel contract builder.
        /// </summary>
        private ExcelContractBuilder excelContractBuilder;

        /// <summary>
        /// Intilizes the method.
        /// </summary>
        [TestInitialize]
        public void IntilizeMethod()
        {
            this.excelContractBuilder = new ExcelContractBuilder();
        }

        /// <summary>
        /// Excels the contract builder should build asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExcelContractBuilder_ShouldBuild_WhenInvokeAsync()
        {
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Pedidos"))
                {
                    dt.Columns.Add("NumeroDocumento");
                    dt.Columns.Add("Posicion");
                    dt.Columns.Add("Tipo");
                    dt.Columns.Add("NodoOrigen");
                    dt.Columns.Add("NodoDestino");
                    dt.Columns.Add("Producto");

                    DataColumn intialDate = new DataColumn("FechaInicio");
                    intialDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(intialDate);

                    DataColumn finalDate = new DataColumn("FechaFin");
                    finalDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(finalDate);

                    dt.Columns.Add("Propietario1");

                    DataColumn volume = new DataColumn("Valor");
                    volume.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(volume);

                    dt.Columns.Add("Unidad");
                    dt.Columns.Add("Propietario2");

                    dt.Columns.Add("TipoOrden");
                    dt.Columns.Add("Estado");

                    DataColumn estimatedVolume = new DataColumn("VolumenPresupuestado");
                    estimatedVolume.DataType = Type.GetType("System.Decimal");
                    dt.Columns.Add(estimatedVolume);

                    DataColumn tolerance = new DataColumn("Porcentajetolerancia");
                    tolerance.DataType = Type.GetType("System.Decimal");
                    dt.Columns.Add(tolerance);

                    dt.Columns.Add("Frecuencia");

                    var currentDate = DateTime.UtcNow;
                    dt.Rows.Add("1764388", 1, "Compra", "REBOMDEOS", "AYACUCHO", "CRUDO CAMPO MAMBO", currentDate, currentDate, "ECOPETROL", 8000, "Bbl", "EQUION", "ORDENES SPOT", "Activa", 145, 95, "Diario");
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    var result = await this.excelContractBuilder.BuildAsync(excelInput).ConfigureAwait(false);

                    Assert.IsNotNull(result);
                    Assert.AreEqual("EXCEL", result["SystemName"].Value<string>());
                    Assert.AreEqual("1764388", result["DocumentNumber"].Value<string>());
                    Assert.AreEqual("1", result["Position"].Value<string>());
                    Assert.AreEqual("Compra", result["MovementTypeId"].Value<string>());
                    Assert.AreEqual("REBOMDEOS", result["SourceNodeId"].Value<string>());
                    Assert.AreEqual("AYACUCHO", result["DestinationNodeId"].Value<string>());
                    Assert.AreEqual("CRUDO CAMPO MAMBO", result["ProductId"].Value<string>());
                    Assert.AreEqual(currentDate, result["StartDate"].Value<DateTime>());
                    Assert.AreEqual(currentDate, result["EndDate"].Value<DateTime>());
                    Assert.AreEqual("ECOPETROL", result["Owner1Id"].Value<string>());
                    Assert.AreEqual("EQUION", result["Owner2Id"].Value<string>());
                    Assert.AreEqual(8000, result["Volume"].Value<int>());
                    Assert.AreEqual("Bbl", result["MeasurementUnit"].Value<string>());
                    Assert.AreEqual("ORDENES SPOT", result["PurchaseOrderType"].Value<string>());
                    Assert.AreEqual("Activa", result["Status"].Value<string>());
                    Assert.AreEqual(145, result["EstimatedVolume"].Value<int>());
                    Assert.AreEqual(95, result["Tolerance"].Value<int>());
                    Assert.AreEqual("Diario", result["Frequency"].Value<string>());
                }
            }
        }

        /// <summary>
        /// Excels the contract builder should throw argument exception for invalid volume datatype when invoke build asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelContractBuilder_ShouldThrowArgumentException_ForInvalid_Volume_Datatype_WhenInvokeBuildAsync()
        {
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Pedidos"))
                {
                    dt.Columns.Add("NumeroDocumento");
                    dt.Columns.Add("Posicion");
                    dt.Columns.Add("Tipo");
                    dt.Columns.Add("NodoOrigen");
                    dt.Columns.Add("NodoDestino");
                    dt.Columns.Add("Producto");

                    DataColumn intialDate = new DataColumn("FechaInicio");
                    intialDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(intialDate);

                    DataColumn finalDate = new DataColumn("FechaFin");
                    finalDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(finalDate);

                    dt.Columns.Add("Propietario1");

                    dt.Columns.Add("Valor");

                    dt.Columns.Add("Unidad");
                    dt.Columns.Add("Propietario2");

                    dt.Columns.Add("TipoOrden");
                    dt.Columns.Add("Estado");

                    DataColumn estimatedVolume = new DataColumn("VolumenPresupuestado");
                    estimatedVolume.DataType = Type.GetType("System.Decimal");
                    dt.Columns.Add(estimatedVolume);

                    DataColumn tolerance = new DataColumn("Porcentajetolerancia");
                    tolerance.DataType = Type.GetType("System.Decimal");
                    dt.Columns.Add(tolerance);

                    dt.Columns.Add("Frecuencia");

                    var currentDate = DateTime.UtcNow;
                    dt.Rows.Add("1764388", 1, "Compra", "REBOMDEOS", "AYACUCHO", "CRUDO CAMPO MAMBO", currentDate, currentDate, "ECOPETROL", 8000, "Bbl", "EQUION", "ORDENES SPOT", "Activa", 145, 95, "Diario");
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    await this.excelContractBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Excels the contract builder should throw argument exception for invalid end date datatype when invoke build asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelContractBuilder_ShouldThrowArgumentException_ForInvalid_EndDate_Datatype_WhenInvokeBuildAsync()
        {
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Pedidos"))
                {
                    dt.Columns.Add("NumeroDocumento");
                    dt.Columns.Add("Posicion");
                    dt.Columns.Add("Tipo");
                    dt.Columns.Add("NodoOrigen");
                    dt.Columns.Add("NodoDestino");
                    dt.Columns.Add("Producto");

                    DataColumn intialDate = new DataColumn("FechaInicio");
                    intialDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(intialDate);

                    dt.Columns.Add("FechaFin");

                    dt.Columns.Add("Propietario1");

                    DataColumn value = new DataColumn("Valor");
                    value.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(value);

                    dt.Columns.Add("Unidad");
                    dt.Columns.Add("Propietario2");

                    dt.Columns.Add("TipoOrden");
                    dt.Columns.Add("Estado");

                    DataColumn estimatedVolume = new DataColumn("VolumenPresupuestado");
                    estimatedVolume.DataType = Type.GetType("System.Decimal");
                    dt.Columns.Add(estimatedVolume);

                    DataColumn tolerance = new DataColumn("Porcentajetolerancia");
                    tolerance.DataType = Type.GetType("System.Decimal");
                    dt.Columns.Add(tolerance);

                    dt.Columns.Add("Frecuencia");

                    var currentDate = DateTime.UtcNow;
                    dt.Rows.Add("1764388", 1, "Compra", "REBOMDEOS", "AYACUCHO", "CRUDO CAMPO MAMBO", currentDate, currentDate, "ECOPETROL", 8000, "Bbl", "EQUION", "ORDENES SPOT", "Activa", 145, 95, "Diario");
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    await this.excelContractBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Excels the contract builder should throw argument exception for invalid start date datatype when invoke build asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelContractBuilder_ShouldThrowArgumentException_ForInvalid_StartDate_Datatype_WhenInvokeBuildAsync()
        {
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Pedidos"))
                {
                    dt.Columns.Add("NumeroDocumento");
                    dt.Columns.Add("Posicion");
                    dt.Columns.Add("Tipo");
                    dt.Columns.Add("NodoOrigen");
                    dt.Columns.Add("NodoDestino");
                    dt.Columns.Add("Producto");

                    dt.Columns.Add("FechaInicio");

                    DataColumn finalDate = new DataColumn("FechaFin");
                    finalDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(finalDate);

                    dt.Columns.Add("Propietario1");

                    DataColumn value = new DataColumn("Valor");
                    value.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(value);

                    dt.Columns.Add("Unidad");
                    dt.Columns.Add("Propietario2");

                    dt.Columns.Add("TipoOrden");
                    dt.Columns.Add("Estado");

                    DataColumn estimatedVolume = new DataColumn("VolumenPresupuestado");
                    estimatedVolume.DataType = Type.GetType("System.Decimal");
                    dt.Columns.Add(estimatedVolume);

                    DataColumn tolerance = new DataColumn("Porcentajetolerancia");
                    tolerance.DataType = Type.GetType("System.Decimal");
                    dt.Columns.Add(tolerance);

                    dt.Columns.Add("Frecuencia");

                    var currentDate = DateTime.UtcNow;
                    dt.Rows.Add("1764388", 1, "Compra", "REBOMDEOS", "AYACUCHO", "CRUDO CAMPO MAMBO", currentDate, currentDate, "ECOPETROL", 8000, "Bbl", "EQUION", "ORDENES SPOT", "Activa", 145, 95, "Diario");
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    await this.excelContractBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Excels the contract builder should throw argument exception for owner1 null or empty when invoke build asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelContractBuilder_ShouldThrowArgumentException_ForOwner1_NullOrEmpty_WhenInvokeBuildAsync()
        {
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Pedidos"))
                {
                    dt.Columns.Add("NumeroDocumento");
                    dt.Columns.Add("Posicion");
                    dt.Columns.Add("Tipo");
                    dt.Columns.Add("NodoOrigen");
                    dt.Columns.Add("NodoDestino");
                    dt.Columns.Add("Producto");

                    dt.Columns.Add("FechaInicio");

                    DataColumn finalDate = new DataColumn("FechaFin");
                    finalDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(finalDate);

                    dt.Columns.Add("Propietario1");

                    DataColumn value = new DataColumn("Valor");
                    value.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(value);

                    dt.Columns.Add("Unidad");
                    dt.Columns.Add("Propietario2");

                    dt.Columns.Add("TipoOrden");
                    dt.Columns.Add("Estado");

                    DataColumn estimatedVolume = new DataColumn("VolumenPresupuestado");
                    estimatedVolume.DataType = Type.GetType("System.Decimal");
                    dt.Columns.Add(estimatedVolume);

                    DataColumn tolerance = new DataColumn("Porcentajetolerancia");
                    tolerance.DataType = Type.GetType("System.Decimal");
                    dt.Columns.Add(tolerance);

                    dt.Columns.Add("Frecuencia");

                    var currentDate = DateTime.UtcNow;
                    dt.Rows.Add("1764388", 1, "Compra", "REBOMDEOS", "AYACUCHO", "CRUDO CAMPO MAMBO", currentDate, currentDate, string.Empty, 8000, "Bbl", "EQUION", "ORDENES SPOT", "Activa", 145, 95, "Diario");
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    await this.excelContractBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Excels the contract builder should throw argument exception for owner2 null or empty when invoke build asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelContractBuilder_ShouldThrowArgumentException_ForOwner2_NullOrEmpty_WhenInvokeBuildAsync()
        {
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Pedidos"))
                {
                    dt.Columns.Add("NumeroDocumento");
                    dt.Columns.Add("Posicion");
                    dt.Columns.Add("Tipo");
                    dt.Columns.Add("NodoOrigen");
                    dt.Columns.Add("NodoDestino");
                    dt.Columns.Add("Producto");

                    dt.Columns.Add("FechaInicio");

                    DataColumn finalDate = new DataColumn("FechaFin");
                    finalDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(finalDate);

                    dt.Columns.Add("Propietario1");

                    DataColumn value = new DataColumn("Valor");
                    value.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(value);

                    dt.Columns.Add("Unidad");
                    dt.Columns.Add("Propietario2");

                    dt.Columns.Add("TipoOrden");
                    dt.Columns.Add("Estado");

                    DataColumn estimatedVolume = new DataColumn("VolumenPresupuestado");
                    estimatedVolume.DataType = Type.GetType("System.Decimal");
                    dt.Columns.Add(estimatedVolume);

                    DataColumn tolerance = new DataColumn("Porcentajetolerancia");
                    tolerance.DataType = Type.GetType("System.Decimal");
                    dt.Columns.Add(tolerance);

                    dt.Columns.Add("Frecuencia");

                    var currentDate = DateTime.UtcNow;
                    dt.Rows.Add("1764388", 1, "Compra", "REBOMDEOS", "AYACUCHO", "CRUDO CAMPO MAMBO", currentDate, currentDate, "Ecopetrol", 8000, "Bbl", string.Empty, "ORDENES SPOT", "Activa", 145, 95, "Diario");
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    await this.excelContractBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Excels the contract builder should throw argument exception for purchase order type null or empty when invoke build asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelContractBuilder_ShouldThrowArgumentException_ForPurchaseOrderType_NullOrEmpty_WhenInvokeBuildAsync()
        {
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Pedidos"))
                {
                    dt.Columns.Add("NumeroDocumento");
                    dt.Columns.Add("Posicion");
                    dt.Columns.Add("Tipo");
                    dt.Columns.Add("NodoOrigen");
                    dt.Columns.Add("NodoDestino");
                    dt.Columns.Add("Producto");

                    dt.Columns.Add("FechaInicio");

                    DataColumn finalDate = new DataColumn("FechaFin");
                    finalDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(finalDate);

                    dt.Columns.Add("Propietario1");

                    DataColumn value = new DataColumn("Valor");
                    value.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(value);

                    dt.Columns.Add("Unidad");
                    dt.Columns.Add("Propietario2");

                    dt.Columns.Add("TipoOrden");
                    dt.Columns.Add("Estado");

                    DataColumn estimatedVolume = new DataColumn("VolumenPresupuestado");
                    estimatedVolume.DataType = Type.GetType("System.Decimal");
                    dt.Columns.Add(estimatedVolume);

                    DataColumn tolerance = new DataColumn("Porcentajetolerancia");
                    tolerance.DataType = Type.GetType("System.Decimal");
                    dt.Columns.Add(tolerance);

                    dt.Columns.Add("Frecuencia");

                    var currentDate = DateTime.UtcNow;
                    dt.Rows.Add("1764388", 1, "Compra", "REBOMDEOS", "AYACUCHO", "CRUDO CAMPO MAMBO", currentDate, currentDate, "Ecopetrol", 8000, "Bbl", "EQUION", string.Empty, "Activa", 145, 95, "Diario");
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    await this.excelContractBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Excels the contract builder should throw argument exception for status null or empty when invoke build asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelContractBuilder_ShouldThrowArgumentException_ForStatus_NullOrEmpty_WhenInvokeBuildAsync()
        {
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Pedidos"))
                {
                    dt.Columns.Add("NumeroDocumento");
                    dt.Columns.Add("Posicion");
                    dt.Columns.Add("Tipo");
                    dt.Columns.Add("NodoOrigen");
                    dt.Columns.Add("NodoDestino");
                    dt.Columns.Add("Producto");

                    dt.Columns.Add("FechaInicio");

                    DataColumn finalDate = new DataColumn("FechaFin");
                    finalDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(finalDate);

                    dt.Columns.Add("Propietario1");

                    DataColumn value = new DataColumn("Valor");
                    value.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(value);

                    dt.Columns.Add("Unidad");
                    dt.Columns.Add("Propietario2");

                    dt.Columns.Add("TipoOrden");
                    dt.Columns.Add("Estado");

                    DataColumn estimatedVolume = new DataColumn("VolumenPresupuestado");
                    estimatedVolume.DataType = Type.GetType("System.Decimal");
                    dt.Columns.Add(estimatedVolume);

                    DataColumn tolerance = new DataColumn("Porcentajetolerancia");
                    tolerance.DataType = Type.GetType("System.Decimal");
                    dt.Columns.Add(tolerance);

                    dt.Columns.Add("Frecuencia");

                    var currentDate = DateTime.UtcNow;
                    dt.Rows.Add("1764388", 1, "Compra", "REBOMDEOS", "AYACUCHO", "CRUDO CAMPO MAMBO", currentDate, currentDate, "Ecopetrol", 8000, "Bbl", "EQUION", "ORDENES SPOT", string.Empty, 145, 95, "Diario");
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    await this.excelContractBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Excels the contract builder should throw argument exception for frequency null or empty when invoke build asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelContractBuilder_ShouldThrowArgumentException_ForFrequency_NullOrEmpty_WhenInvokeBuildAsync()
        {
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Pedidos"))
                {
                    dt.Columns.Add("NumeroDocumento");
                    dt.Columns.Add("Posicion");
                    dt.Columns.Add("Tipo");
                    dt.Columns.Add("NodoOrigen");
                    dt.Columns.Add("NodoDestino");
                    dt.Columns.Add("Producto");

                    dt.Columns.Add("FechaInicio");

                    DataColumn finalDate = new DataColumn("FechaFin");
                    finalDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(finalDate);

                    dt.Columns.Add("Propietario1");

                    DataColumn value = new DataColumn("Valor");
                    value.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(value);

                    dt.Columns.Add("Unidad");
                    dt.Columns.Add("Propietario2");

                    dt.Columns.Add("TipoOrden");
                    dt.Columns.Add("Estado");

                    DataColumn estimatedVolume = new DataColumn("VolumenPresupuestado");
                    estimatedVolume.DataType = Type.GetType("System.Decimal");
                    dt.Columns.Add(estimatedVolume);

                    DataColumn tolerance = new DataColumn("Porcentajetolerancia");
                    tolerance.DataType = Type.GetType("System.Decimal");
                    dt.Columns.Add(tolerance);

                    dt.Columns.Add("Frecuencia");

                    var currentDate = DateTime.UtcNow;
                    dt.Rows.Add("1764388", 1, "Compra", "REBOMDEOS", "AYACUCHO", "CRUDO CAMPO MAMBO", currentDate, currentDate, "Ecopetrol", 8000, "Bbl", "EQUION", "ORDENES SPOT", "Activa", 145, 95, string.Empty);
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    await this.excelContractBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }
    }
}
