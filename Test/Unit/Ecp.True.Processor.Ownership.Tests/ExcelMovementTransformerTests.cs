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

namespace Ecp.True.Processor.Ownership.Tests
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Ownership.Builders.Excel.Interfaces;
    using Ecp.True.Processors.Ownership.Services.Excel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json.Linq;

    [TestClass]
    public class ExcelMovementTransformerTests
    {
        /// <summary>
        /// The movement builder.
        /// </summary>
        private Mock<IExcelMovementBuilder> mockExcelMovementBuilder;

        /// <summary>
        /// The error movement builder.
        /// </summary>
        private Mock<IExcelErrorMovementBuilder> mockExcelErrorMovementBuilder;

        /// <summary>
        /// The excel transformer.
        /// </summary>
        private ExcelMovementTransformer excelMovementTransformer;

        [TestInitialize]
        public void IntilizeMethod()
        {
            this.mockExcelMovementBuilder = new Mock<IExcelMovementBuilder>();
            this.mockExcelErrorMovementBuilder = new Mock<IExcelErrorMovementBuilder>();
            this.excelMovementTransformer = new ExcelMovementTransformer(this.mockExcelMovementBuilder.Object, this.mockExcelErrorMovementBuilder.Object);
        }

        [TestMethod]
        public async Task TransformMovementAsync_ShouldTransformResultMovementAsync()
        {
            this.mockExcelMovementBuilder.Setup(i => i.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(new JArray());
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Movimientos"))
                {
                    dt.Columns.Add("Tiquete");
                    dt.Columns.Add("IdMovimiento");
                    dt.Columns.Add("IdPropietario");
                    dt.Columns.Add("Propietario");
                    dt.Columns.Add("PorcentajePropiedad");
                    dt.Columns.Add("VolumenPropiedad");
                    dt.Columns.Add("ReglaAplicada");
                    dt.Columns.Add("VersionRegla");
                    dt.Columns.Add("FechaEjecucion");

                    dt.Rows.Add(23914, 153, 27, "Ecopetrol", 50, 1000, "Regla 1", "1", "04/11/2019 10:20:35");
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    var result = await this.excelMovementTransformer.TransformMovementAsync(excelInput, OwnershipExcelType.RESULTEXCEL).ConfigureAwait(false);
                    Assert.IsNotNull(result);
                }
            }
        }

        [TestMethod]
        public async Task TransformMovementAsync_ShouldTransformErrorMovementAsync()
        {
            this.mockExcelErrorMovementBuilder.Setup(i => i.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(new JArray());
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("Movimientos"))
                {
                    dt.Columns.Add("Tiquete");
                    dt.Columns.Add("IdMovimiento");
                    dt.Columns.Add("IdNodoOrigen");
                    dt.Columns.Add("DescripcionError");
                    dt.Columns.Add("FechaEjecucion");

                    dt.Rows.Add(25116, 1029, 12727, "La regla 1 no existe", "04/11/2019 10:20:35");
                    var args = Tuple.Create(dt.Rows[0], "Insert", false);
                    ds.Tables.Add(dt);
                    ExcelInput excelInput = new ExcelInput(args, ds);

                    var result = await this.excelMovementTransformer.TransformMovementAsync(excelInput, OwnershipExcelType.ERROREXCEL).ConfigureAwait(false);
                    Assert.IsNotNull(result);
                }
            }
        }
    }
}
