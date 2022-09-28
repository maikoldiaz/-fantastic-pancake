// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelEventBuilderTests.cs" company="Microsoft">
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
    /// The ExcelEventBuilderTests.
    /// </summary>
    [TestClass]
    public class ExcelEventBuilderTests
    {
        /// <summary>
        /// The excel event builder.
        /// </summary>
        private ExcelEventBuilder excelEventBuilder;

        /// <summary>
        /// Intilizes the method.
        /// </summary>
        [TestInitialize]
        public void IntilizeMethod()
        {
            this.excelEventBuilder = new ExcelEventBuilder();
        }

        /// <summary>
        /// Excels the event builder should build when invoke asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExcelEventBuilder_ShouldBuild_WhenInvokeAsync()
        {
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Eventos"))
                {
                    dt.Columns.Add("EventoPropiedad");
                    dt.Columns.Add("NodoOrigen");
                    dt.Columns.Add("NodoDestino");
                    dt.Columns.Add("ProductoOrigen");
                    dt.Columns.Add("ProductoDestino");
                    dt.Columns.Add("Propietario1");

                    DataColumn dailyValue = new DataColumn("ValorDiario");
                    dailyValue.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(dailyValue);

                    dt.Columns.Add("Unidad");

                    DataColumn initialDate = new DataColumn("Fecha Inicio");
                    initialDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(initialDate);

                    DataColumn finalDate = new DataColumn("FechaFin");
                    finalDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(finalDate);

                    dt.Columns.Add("Propietario2");

                    var currentDate = DateTime.UtcNow;
                    dt.Rows.Add("EventoPlaneacion", "AYACUCHO", "REBOMDEOS", "CRUDO CAMPO MAMBO", "CRUDO CAMPO CUSUCO", "ECOPETROL", 2000, "Bbl", currentDate, currentDate, "EQUION");
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    var result = await this.excelEventBuilder.BuildAsync(excelInput).ConfigureAwait(false);

                    Assert.IsNotNull(result);
                    Assert.AreEqual("EXCEL", result["SystemName"].Value<string>());
                    Assert.AreEqual("EventoPlaneacion", result["EventTypeId"].Value<string>());
                    Assert.AreEqual("AYACUCHO", result["SourceNodeId"].Value<string>());
                    Assert.AreEqual("REBOMDEOS", result["DestinationNodeId"].Value<string>());
                    Assert.AreEqual("CRUDO CAMPO MAMBO", result["SourceProductId"].Value<string>());
                    Assert.AreEqual("CRUDO CAMPO CUSUCO", result["DestinationProductId"].Value<string>());
                    Assert.AreEqual(currentDate, result["StartDate"].Value<DateTime>());
                    Assert.AreEqual(currentDate, result["EndDate"].Value<DateTime>());
                    Assert.AreEqual("ECOPETROL", result["Owner1Id"].Value<string>());
                    Assert.AreEqual("EQUION", result["Owner2Id"].Value<string>());
                    Assert.AreEqual(2000, result["Volume"].Value<int>());
                    Assert.AreEqual("Bbl", result["MeasurementUnit"].Value<string>());
                }
            }
        }

        /// <summary>
        /// Excels the event builder should throw argument exception for invalid daily value datatype when invoke build asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelEventBuilder_ShouldThrowArgumentException_ForInvalid_DailyValue_Datatype_WhenInvokeBuildAsync()
        {
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Eventos"))
                {
                    dt.Columns.Add("EventoPropiedad");
                    dt.Columns.Add("NodoOrigen");
                    dt.Columns.Add("NodoDestino");
                    dt.Columns.Add("ProductoOrigen");
                    dt.Columns.Add("ProductoDestino");
                    dt.Columns.Add("Propietario1");

                    dt.Columns.Add("ValorDiario");

                    dt.Columns.Add("Unidad");

                    DataColumn initialDate = new DataColumn("Fecha Inicio");
                    initialDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(initialDate);

                    DataColumn finalDate = new DataColumn("FechaFin");
                    finalDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(finalDate);

                    dt.Columns.Add("Propietario2");

                    var currentDate = DateTime.UtcNow;
                    dt.Rows.Add("EventoPlaneacion", "AYACUCHO", "REBOMDEOS", "CRUDO CAMPO MAMBO", "CRUDO CAMPO CUSUCO", "ECOPETROL", 2000, "Bbl", currentDate, currentDate, "EQUION");
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    await this.excelEventBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Excels the event builder should throw argument exception for invalid end date datatype when invoke build asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelEventBuilder_ShouldThrowArgumentException_ForInvalid_EndDate_Datatype_WhenInvokeBuildAsync()
        {
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Eventos"))
                {
                    dt.Columns.Add("EventoPropiedad");
                    dt.Columns.Add("NodoOrigen");
                    dt.Columns.Add("NodoDestino");
                    dt.Columns.Add("ProductoOrigen");
                    dt.Columns.Add("ProductoDestino");
                    dt.Columns.Add("Propietario1");

                    DataColumn dailyValue = new DataColumn("ValorDiario");
                    dailyValue.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(dailyValue);

                    dt.Columns.Add("Unidad");

                    DataColumn initialDate = new DataColumn("Fecha Inicio");
                    initialDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(initialDate);

                    dt.Columns.Add("FechaFin");

                    dt.Columns.Add("Propietario2");

                    var currentDate = DateTime.UtcNow;
                    dt.Rows.Add("EventoPlaneacion", "AYACUCHO", "REBOMDEOS", "CRUDO CAMPO MAMBO", "CRUDO CAMPO CUSUCO", "ECOPETROL", 2000, "Bbl", currentDate, currentDate, "EQUION");
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    await this.excelEventBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Excels the event builder should throw argument exception for invalid start date datatype when invoke build asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelEventBuilder_ShouldThrowArgumentException_ForInvalid_StartDate_Datatype_WhenInvokeBuildAsync()
        {
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Eventos"))
                {
                    dt.Columns.Add("EventoPropiedad");
                    dt.Columns.Add("NodoOrigen");
                    dt.Columns.Add("NodoDestino");
                    dt.Columns.Add("ProductoOrigen");
                    dt.Columns.Add("ProductoDestino");
                    dt.Columns.Add("Propietario1");

                    DataColumn dailyValue = new DataColumn("ValorDiario");
                    dailyValue.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(dailyValue);

                    dt.Columns.Add("Unidad");
                    dt.Columns.Add("Fecha Inicio");

                    DataColumn finalDate = new DataColumn("FechaFin");
                    finalDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(finalDate);

                    dt.Columns.Add("Propietario2");

                    var currentDate = DateTime.UtcNow;
                    dt.Rows.Add("EventoPlaneacion", "AYACUCHO", "REBOMDEOS", "CRUDO CAMPO MAMBO", "CRUDO CAMPO CUSUCO", "ECOPETROL", 2000, "Bbl", currentDate, currentDate, "EQUION");
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    await this.excelEventBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Excels the event builder should throw argument exception for invalid for owner1 null or empty when invoke build asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelEventBuilder_ShouldThrowArgumentException_ForInvalid_ForOwner1_NullOrEmpty_WhenInvokeBuildAsync()
        {
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Eventos"))
                {
                    dt.Columns.Add("EventoPropiedad");
                    dt.Columns.Add("NodoOrigen");
                    dt.Columns.Add("NodoDestino");
                    dt.Columns.Add("ProductoOrigen");
                    dt.Columns.Add("ProductoDestino");
                    dt.Columns.Add("Propietario1");

                    DataColumn dailyValue = new DataColumn("ValorDiario");
                    dailyValue.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(dailyValue);

                    dt.Columns.Add("Unidad");
                    dt.Columns.Add("Fecha Inicio");

                    DataColumn finalDate = new DataColumn("FechaFin");
                    finalDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(finalDate);

                    dt.Columns.Add("Propietario2");

                    var currentDate = DateTime.UtcNow;
                    dt.Rows.Add("EventoPlaneacion", "AYACUCHO", "REBOMDEOS", "CRUDO CAMPO MAMBO", "CRUDO CAMPO CUSUCO", string.Empty, 2000, "Bbl", currentDate, currentDate, "EQUION");
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    await this.excelEventBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Excels the event builder should throw argument exception for invalid for owner2 null or empty when invoke build asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelEventBuilder_ShouldThrowArgumentException_ForInvalid_ForOwner2_NullOrEmpty_WhenInvokeBuildAsync()
        {
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Eventos"))
                {
                    dt.Columns.Add("EventoPropiedad");
                    dt.Columns.Add("NodoOrigen");
                    dt.Columns.Add("NodoDestino");
                    dt.Columns.Add("ProductoOrigen");
                    dt.Columns.Add("ProductoDestino");
                    dt.Columns.Add("Propietario1");

                    DataColumn dailyValue = new DataColumn("ValorDiario");
                    dailyValue.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(dailyValue);

                    dt.Columns.Add("Unidad");
                    dt.Columns.Add("Fecha Inicio");

                    DataColumn finalDate = new DataColumn("FechaFin");
                    finalDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(finalDate);

                    dt.Columns.Add("Propietario2");

                    var currentDate = DateTime.UtcNow;
                    dt.Rows.Add("EventoPlaneacion", "AYACUCHO", "REBOMDEOS", "CRUDO CAMPO MAMBO", "CRUDO CAMPO CUSUCO", "ECOPETROL", 2000, "Bbl", currentDate, currentDate, string.Empty);
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    await this.excelEventBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }
    }
}
