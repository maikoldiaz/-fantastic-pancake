// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelOwnerBuilderTests.cs" company="Microsoft">
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

    [TestClass]
    public class ExcelOwnerBuilderTests
    {
        private ExcelOwnerBuilder excelOwnerBuilder;

        [TestInitialize]
        public void IntilizeMethod()
        {
            this.excelOwnerBuilder = new ExcelOwnerBuilder();
        }

        [TestMethod]
        public async Task ExcelAtrributeBuilder_ShouldBuild_MovementAttributeAsync()
        {
            // Act
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("PROPIETARIOSMOV"))
                {
                    dt.Columns.Add("IdMovimiento");
                    dt.Columns.Add("IdPropietario");

                    DataColumn valueProperty = new DataColumn("ValorPropiedad");
                    valueProperty.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(valueProperty);

                    dt.Columns.Add("UnidadValorPropiedad");

                    dt.Rows.Add(1, "OwnerId", 1, "OwnershipValueUnit");

                    var args = Tuple.Create(dt.Rows[0], "Insert", true);

                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);
                    var data = await this.excelOwnerBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                    Assert.IsNotNull(data);
                    Assert.AreEqual(JTokenType.Array, JObject.FromObject(data)["Owners"].Type);
                    Assert.AreEqual(dt.Rows.Count, ((JArray)data["Owners"]).Count);
                    Assert.AreEqual(JTokenType.String, JObject.FromObject(data)["Owners"][0]["OwnerId"].Type);
                    Assert.AreEqual(JTokenType.String, JObject.FromObject(data)["Owners"][0]["OwnershipValueUnit"].Type);
                }
            }
        }

        [TestMethod]
        public async Task ExcelAtrributeBuilder_ShouldBuild_InventoryAttributeAsync()
        {
            // Act
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("PROPIETARIOSINV"))
                {
                    dt.Columns.Add("IdInventario");
                    dt.Columns.Add("IdPropietario");

                    DataColumn inventoryDate = new DataColumn("FechaInventario");
                    inventoryDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(inventoryDate);

                    DataColumn valueProperty = new DataColumn("ValorPropiedad");
                    valueProperty.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(valueProperty);

                    dt.Columns.Add("UnidadValorPropiedad");
                    dt.Columns.Add("IdNodo");
                    dt.Columns.Add("Producto");
                    dt.Columns.Add("BatchId");
                    dt.Columns.Add("Tanque");

                    dt.Rows.Add(1, "OwnerId", DateTime.Today, 1, "OwnershipValueUnit", 1, "PROD1", "123", "Tank1");

                    var args = Tuple.Create(dt.Rows[0], "Insert", false);

                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);
                    var data = await this.excelOwnerBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                    Assert.IsNotNull(data);
                    Assert.AreEqual(JTokenType.Array, JObject.FromObject(data)["Owners"].Type);
                    Assert.AreEqual(dt.Rows.Count, ((JArray)data["Owners"]).Count);
                    Assert.AreEqual(JTokenType.String, JObject.FromObject(data)["Owners"][0]["OwnerId"].Type);
                    Assert.AreEqual(JTokenType.String, JObject.FromObject(data)["Owners"][0]["OwnershipValueUnit"].Type);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelAtrributeBuilder_ShouldThrowArgumentException_ForIncorrectValorPropiedadDatatypeAsync()
        {
            // Act
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("PROPIETARIOSINV"))
                {
                    dt.Columns.Add("IdInventario");
                    dt.Columns.Add("IdPropietario");
                    dt.Columns.Add("ValorPropiedad");
                    dt.Columns.Add("UnidadValorPropiedad");
                    dt.Columns.Add("IdNodo");
                    dt.Columns.Add("Producto");

                    dt.Rows.Add(1, "OwnerId", 1, "OwnershipValueUnit", 1, "PROD1");

                    var args = Tuple.Create(dt.Rows[0], "Insert", false);

                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);
                    await this.excelOwnerBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelAtrributeBuilder_ShouldThrowArgumentException_ForIncorrectInventoryDateDatatypeAsync()
        {
            // Act
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("PROPIETARIOSINV"))
                {
                    dt.Columns.Add("IdInventario");
                    dt.Columns.Add("IdPropietario");
                    dt.Columns.Add("FechaInventario");
                    DataColumn valueProperty = new DataColumn("ValorPropiedad");
                    valueProperty.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(valueProperty);
                    dt.Columns.Add("UnidadValorPropiedad");
                    dt.Columns.Add("IdNodo");
                    dt.Columns.Add("Producto");

                    dt.Rows.Add(1, "OwnerId", DateTime.Today, 1, "OwnershipValueUnit", 1, "PROD1");

                    var args = Tuple.Create(dt.Rows[0], "Insert", false);

                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);
                    await this.excelOwnerBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }
    }
}