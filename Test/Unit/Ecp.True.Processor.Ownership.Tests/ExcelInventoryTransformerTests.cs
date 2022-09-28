// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelInventoryTransformerTests.cs" company="Microsoft">
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
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Ownership.Builders.Excel.Interfaces;
    using Ecp.True.Processors.Ownership.Services.Excel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json.Linq;

    [TestClass]
    public class ExcelInventoryTransformerTests
    {
        /// <summary>
        /// The inventory builder.
        /// </summary>
        private Mock<IExcelInventoryBuilder> mockExcelInventoryBuilder;

        /// <summary>
        /// The excel error inventory builder.
        /// </summary>
        private Mock<IExcelErrorInventoryBuilder> mockExcelErrorInventoryBuilder;

        /// <summary>
        /// The inventory transformer.
        /// </summary>
        private ExcelInventoryTransformer excelInventoryTransformer;

        [TestInitialize]
        public void IntilizeMethod()
        {
            this.mockExcelInventoryBuilder = new Mock<IExcelInventoryBuilder>();
            this.mockExcelErrorInventoryBuilder = new Mock<IExcelErrorInventoryBuilder>();
            this.excelInventoryTransformer = new ExcelInventoryTransformer(this.mockExcelInventoryBuilder.Object, this.mockExcelErrorInventoryBuilder.Object);
        }

        [TestMethod]
        public async Task TransformInventoryAsync_ShouldTransformErrorInventory_WhenInvokeAsync()
        {
            this.mockExcelErrorInventoryBuilder.Setup(i => i.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(new JArray());
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Inventarios"))
                {
                    dt.Columns.Add("Tiquete");
                    dt.Columns.Add("IdInventario");
                    dt.Columns.Add("IdNodo");
                    dt.Columns.Add("DescripcionError");
                    dt.Columns.Add("FechaEjecucion");

                    dt.Rows.Add(25116, 565, 12726, "La regla 1 no existe", "04/11/2019 10:20:35");
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(ds, new TrueMessage());

                    var result = await this.excelInventoryTransformer.TransformInventoryAsync(excelInput, OwnershipExcelType.ERROREXCEL).ConfigureAwait(false);
                    Assert.IsNotNull(result);
                }
            }
        }

        [TestMethod]
        public async Task TransformInventoryAsync_ShouldTransformResultInventory_WhenInvokeAsync()
        {
            this.mockExcelInventoryBuilder.Setup(i => i.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(new JArray());

            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Inventarios"))
                {
                    dt.Columns.Add("Tiquete");
                    dt.Columns.Add("IdInventario");
                    dt.Columns.Add("IdPropietario");
                    dt.Columns.Add("Propietario");
                    dt.Columns.Add("PorcentajePropiedad");
                    dt.Columns.Add("VolumenPropiedad");
                    dt.Columns.Add("ReglaAplicada");
                    dt.Columns.Add("VersionRegla");
                    dt.Columns.Add("FechaEjecucion");

                    dt.Rows.Add(23914, 153, 27, "Ecopetrol", 50, 1000, "Regla 1", "1", "04/11/2019 10:20:35");
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(ds, new TrueMessage());

                    var result = await this.excelInventoryTransformer.TransformInventoryAsync(excelInput, OwnershipExcelType.RESULTEXCEL).ConfigureAwait(false);
                    Assert.IsNotNull(result);
                }
            }
        }

        /// <summary>
        /// Transforms the inventory asynchronous should return empty j array when invoke asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task TransformInventoryAsync_ShouldReturnEmptyJArray_WhenInvokeAsync()
        {
            this.mockExcelErrorInventoryBuilder.Setup(i => i.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(new JArray());
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable())
                {
                    dt.Columns.Add("Tiquete");
                    dt.Columns.Add("IdInventario");
                    dt.Columns.Add("IdNodo");
                    dt.Columns.Add("DescripcionError");
                    dt.Columns.Add("FechaEjecucion");

                    dt.Rows.Add(25116, 565, 12726, "La regla 1 no existe", "04/11/2019 10:20:35");
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(ds, new TrueMessage());
                    var result = await this.excelInventoryTransformer.TransformInventoryAsync(excelInput, OwnershipExcelType.ERROREXCEL).ConfigureAwait(false);

                    Assert.IsInstanceOfType(result, typeof(JArray));
                    Assert.IsNull(result.First);
                }
            }
        }

        /// <summary>
        /// Transforms the inventory asynchronous should throw error when invoke asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task TransformInventoryAsync_ShouldThrowError_WhenInvokeAsync()
        {
            this.mockExcelErrorInventoryBuilder.Setup(i => i.BuildAsync(It.IsAny<ExcelInput>())).ThrowsAsync(new Exception());
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Inventarios"))
                {
                    dt.Columns.Add("Tiquete");
                    dt.Columns.Add("IdInventario");
                    dt.Columns.Add("IdNodo");
                    dt.Columns.Add("DescripcionError");
                    dt.Columns.Add("FechaEjecucion");

                    dt.Rows.Add(25116, 565, 12726, "La regla 1 no existe", "04/11/2019 10:20:35");
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(ds, new TrueMessage());
                    await this.excelInventoryTransformer.TransformInventoryAsync(excelInput, OwnershipExcelType.ERROREXCEL).ConfigureAwait(false);
                }
            }
        }
    }
}
