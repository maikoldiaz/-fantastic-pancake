// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelAttributeBuilderTests.cs" company="Microsoft">
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
    public class ExcelAttributeBuilderTests
    {
        private ExcelAttributeBuilder excelAttributeBuilder;

        [TestInitialize]
        public void IntilizeMethod()
        {
            this.excelAttributeBuilder = new ExcelAttributeBuilder();
        }

        [TestMethod]
        public async Task ExcelAtrributeBuilder_ShouldBuild_MovementAttributeAsync()
        {
            // Act
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("CALIDADMOV"))
                {
                    dt.Columns.Add("IdMovimiento");
                    dt.Columns.Add("IdAtributo");

                    DataColumn volume = new DataColumn("ValorAtributo");
                    volume.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(volume);

                    dt.Columns.Add("UnidadValorAtributo");
                    dt.Columns.Add("DescripcionAtributo");
                    dt.Columns.Add("TipoAtributo");

                    dt.Rows.Add(1, "AttributeId", 1, "ValueAttributeUnit", "AttributeDescription");

                    var args = Tuple.Create(dt.Rows[0], "Insert", true);

                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);
                    var data = await this.excelAttributeBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                    Assert.IsNotNull(data);
                    Assert.AreEqual(JTokenType.Array, JObject.FromObject(data)["Attributes"].Type);
                    Assert.AreEqual(dt.Rows.Count, ((JArray)data["Attributes"]).Count);
                    Assert.AreEqual(JTokenType.String, JObject.FromObject(data)["Attributes"][0]["AttributeId"].Type);
                    Assert.AreEqual(JTokenType.String, JObject.FromObject(data)["Attributes"][0]["ValueAttributeUnit"].Type);
                    Assert.AreEqual(JTokenType.String, JObject.FromObject(data)["Attributes"][0]["ValueAttributeUnit"].Type);
                }
            }
        }

        [TestMethod]
        public async Task ExcelAtrributeBuilder_ShouldBuild_InventoryAttributeAsync()
        {
            // Act
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("CALIDADINV"))
                {
                    dt.Columns.Add("IdInventario");
                    dt.Columns.Add("IdAtributo");
                    DataColumn inventoryDate = new DataColumn("FechaInventario");
                    inventoryDate.DataType = Type.GetType("System.DateTime");
                    dt.Columns.Add(inventoryDate);

                    DataColumn volume = new DataColumn("ValorAtributo");
                    volume.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(volume);

                    dt.Columns.Add("UnidadValorAtributo");
                    dt.Columns.Add("DescripcionAtributo");
                    dt.Columns.Add("IdNodo");
                    dt.Columns.Add("Producto");
                    dt.Columns.Add("BatchId");
                    dt.Columns.Add("Tanque");
                    dt.Columns.Add("TipoAtributo");

                    dt.Rows.Add(1, "AttributeId", DateTime.Today.Date, 1, "ValueAttributeUnit", "AttributeDescription", 1,"PROD1","123","Tank1");

                    var args = Tuple.Create(dt.Rows[0], "Insert", false);

                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);
                    var data = await this.excelAttributeBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                    Assert.IsNotNull(data);
                    Assert.AreEqual(JTokenType.Array, JObject.FromObject(data)["Attributes"].Type);
                    Assert.AreEqual(dt.Rows.Count, ((JArray)data["Attributes"]).Count);
                    Assert.AreEqual(JTokenType.String, JObject.FromObject(data)["Attributes"][0]["AttributeId"].Type);
                    Assert.AreEqual(JTokenType.String, JObject.FromObject(data)["Attributes"][0]["ValueAttributeUnit"].Type);
                    Assert.AreEqual(JTokenType.String, JObject.FromObject(data)["Attributes"][0]["ValueAttributeUnit"].Type);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExcelAtrributeBuilder_ShouldThrowArgumentException_ForIncorrectValorAtributoDatatypeAsync()
        {
            // Act
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("CALIDADINV"))
                {
                    dt.Columns.Add("IdInventario");
                    dt.Columns.Add("IdAtributo");
                    dt.Columns.Add("ValorAtributo");
                    dt.Columns.Add("UnidadValorAtributo");
                    dt.Columns.Add("DescripcionAtributo");
                    dt.Columns.Add("IdNodo");
                    dt.Columns.Add("Producto");

                    dt.Rows.Add(1, "AttributeId", 1, "ValueAttributeUnit", "AttributeDescription", 1, "PROD1");

                    var args = Tuple.Create(dt.Rows[0], "Insert", false);

                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);
                    await this.excelAttributeBuilder.BuildAsync(excelInput).ConfigureAwait(false);
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
                using (DataTable dt = new DataTable("CALIDADINV"))
                {
                    dt.Columns.Add("IdInventario");
                    dt.Columns.Add("IdAtributo");
                    dt.Columns.Add("FechaInventario");
                    DataColumn volume = new DataColumn("ValorAtributo");
                    volume.DataType = Type.GetType("System.Double");
                    dt.Columns.Add(volume);
                    dt.Columns.Add("UnidadValorAtributo");
                    dt.Columns.Add("DescripcionAtributo");
                    dt.Columns.Add("IdNodo");
                    dt.Columns.Add("Producto");

                    dt.Rows.Add(1, "AttributeId", DateTime.Today, 1, "ValueAttributeUnit", "AttributeDescription", 1, "PROD1");

                    var args = Tuple.Create(dt.Rows[0], "Insert", false);

                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);
                    await this.excelAttributeBuilder.BuildAsync(excelInput).ConfigureAwait(false);
                }
            }
        }
    }
}