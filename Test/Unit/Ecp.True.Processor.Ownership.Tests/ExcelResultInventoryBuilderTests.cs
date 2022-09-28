// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelResultInventoryBuilderTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Ownership.Tests
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Ownership.Builders.Excel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The Excel Error Inventory Tests.
    /// </summary>
    [TestClass]
    public class ExcelResultInventoryBuilderTests
    {
        /// <summary>
        ///  The builder.
        /// </summary>
        private ExcelInventoryBuilder builder;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.builder = new ExcelInventoryBuilder();
        }

        /// <summary>
        /// Excels the result inventory builder should build asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExcelResultInventoryBuilder_ShouldBuildAsync()
        {
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Inventarios"))
                {
                    dt.Columns.Add("Tiquete");
                    dt.Columns.Add("IdInventario");
                    dt.Columns.Add("IdPropietario");
                    dt.Columns.Add("Propietario");

                    DataColumn percentage = new DataColumn("PorcentajePropiedad");
                    percentage.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(percentage);

                    DataColumn volume = new DataColumn("VolumenPropiedad");
                    volume.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(volume);

                    dt.Columns.Add("ReglaAplicada");
                    dt.Columns.Add("VersionRegla");

                    DataColumn date = new DataColumn("FechaEjecucion");
                    date.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(date);

                    var currentDate = DateTime.UtcNow;

                    dt.Rows.Add(23914, 153, 27, "Ecopetrol", 50, 1000, "Regla 1", "1", currentDate);
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    var result = await this.builder.BuildAsync(excelInput).ConfigureAwait(false);
                    Assert.IsNotNull(result);
                    Assert.AreEqual(23914, result[0]["Ticket"]);
                    Assert.AreEqual(153, result[0]["InventoryId"]);
                    Assert.AreEqual(27, result[0]["OwnerId"]);
                    Assert.AreEqual("Ecopetrol", result[0]["Owner"]);
                    Assert.AreEqual(50.0, result[0]["OwnershipPercentage"]);
                    Assert.AreEqual(1000.0, result[0]["OwnershipVolume"]);
                    Assert.AreEqual("Regla 1", result[0]["AppliedRule"]);
                    Assert.AreEqual("1", result[0]["RuleVersion"]);
                    Assert.AreEqual(currentDate, result[0]["ExecutionDate"]);
                }
            }
        }

        /// <summary>
        /// Excels the result inventory builder should return argument exception for incorrect datatype of porcentaje propiedad asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelResultInventoryBuilder_ShouldReturnArgumentExceptionForIncorrectDatatypeOf_PorcentajePropiedad_Async()
        {
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Inventarios"))
                {
                    dt.Columns.Add("Tiquete");
                    dt.Columns.Add("IdInventario");
                    dt.Columns.Add("IdPropietario");
                    dt.Columns.Add("Propietario");
                    dt.Columns.Add("PorcentajePropiedad");

                    DataColumn volume = new DataColumn("VolumenPropiedad");
                    volume.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(volume);

                    dt.Columns.Add("ReglaAplicada");
                    dt.Columns.Add("VersionRegla");

                    DataColumn date = new DataColumn("FechaEjecucion");
                    date.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(date);

                    dt.Rows.Add(23914, 153, 27, "Ecopetrol", 50, 1000, "Regla 1", "1", "04/11/2019 10:20:35");
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    await this.builder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Excels the result inventory builder should return argument exception for incorrect datatype of volumen propiedad asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelResultInventoryBuilder_ShouldReturnArgumentExceptionForIncorrectDatatypeOf_VolumenPropiedad_Async()
        {
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Inventarios"))
                {
                    dt.Columns.Add("Tiquete");
                    dt.Columns.Add("IdInventario");
                    dt.Columns.Add("IdPropietario");
                    dt.Columns.Add("Propietario");

                    DataColumn percentage = new DataColumn("PorcentajePropiedad");
                    percentage.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(percentage);

                    dt.Columns.Add("VolumenPropiedad");
                    dt.Columns.Add("ReglaAplicada");
                    dt.Columns.Add("VersionRegla");

                    DataColumn date = new DataColumn("FechaEjecucion");
                    date.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(date);

                    dt.Rows.Add(23914, 153, 27, "Ecopetrol", 50, 99999999999999999.99, "Regla 1", "1", "04/11/2019 10:20:35");
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    await this.builder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelResultInventoryBuilder_ShouldReturnArgumentExceptionForIncorrectDatatypeOf_FechaEjecucion_Async()
        {
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Inventarios"))
                {
                    dt.Columns.Add("Tiquete");
                    dt.Columns.Add("IdInventario");
                    dt.Columns.Add("IdPropietario");
                    dt.Columns.Add("Propietario");

                    DataColumn percentage = new DataColumn("PorcentajePropiedad");
                    percentage.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(percentage);

                    DataColumn volume = new DataColumn("VolumenPropiedad");
                    volume.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(volume);

                    dt.Columns.Add("ReglaAplicada");
                    dt.Columns.Add("VersionRegla");
                    dt.Columns.Add("FechaEjecucion");

                    dt.Rows.Add(23914, 153, 27, "Ecopetrol", 50, 1000, "Regla 1", "1", "04/11/2019 10:20:35");
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    await this.builder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }
    }
}
