// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelErrorInventoryBuilderTests.cs" company="Microsoft">
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
    public class ExcelErrorInventoryBuilderTests
    {
        /// <summary>
        ///  The builder.
        /// </summary>
        private ExcelErrorInventoryBuilder builder;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.builder = new ExcelErrorInventoryBuilder();
        }

        /// <summary>
        /// Excels the error inventory builder should build asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExcelErrorInventoryBuilder_ShouldBuildAsync()
        {
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Inventarios"))
                {
                    dt.Columns.Add("Tiquete");
                    dt.Columns.Add("IdInventario");
                    dt.Columns.Add("IdNodo");
                    dt.Columns.Add("DescripcionError");

                    DataColumn date = new DataColumn("FechaEjecucion");
                    date.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(date);

                    var currentDate = DateTime.UtcNow;

                    dt.Rows.Add(25116, 565, 12726, "La regla 1 no existe", currentDate);
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    var result = await this.builder.BuildAsync(excelInput).ConfigureAwait(false);
                    Assert.IsNotNull(result);
                    Assert.AreEqual(25116, result[0]["Ticket"]);
                    Assert.AreEqual(565, result[0]["InventoryId"]);
                    Assert.AreEqual(12726, result[0]["NodeId"]);
                    Assert.AreEqual("La regla 1 no existe", result[0]["ErrorDescription"]);
                    Assert.AreEqual(currentDate, result[0]["ExecutionDate"]);
                }
            }
        }

        /// <summary>
        /// Excels the error inventory builder should return argument exception for incorrect datatype of fecha ejecucion asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelErrorInventoryBuilder_ShouldReturnArgumentExceptionForIncorrectDatatypeOf_FechaEjecucion_Async()
        {
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Inventarios"))
                {
                    dt.Columns.Add("Tiquete");
                    dt.Columns.Add("IdInventario");
                    dt.Columns.Add("IdNodo");
                    dt.Columns.Add("DescripcionError");
                    dt.Columns.Add("FechaEjecucion");

                    dt.Rows.Add(23914, 153, 27, "Ecopetrol", 50, 1000, "Regla 1", "1", DateTime.UtcNow);
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    await this.builder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }
    }
}
