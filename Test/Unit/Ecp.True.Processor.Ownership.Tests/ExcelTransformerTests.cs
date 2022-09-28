// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelTransformerTests.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Ownership.Input.Interfaces;
    using Ecp.True.Processors.Ownership.Services.Excel.Interfaces;
    using Ecp.True.Processors.Ownership.Transform.Services.Excel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json.Linq;

    [TestClass]
    public class ExcelTransformerTests
    {
        /// <summary>
        /// The movement builder.
        /// </summary>
        private Mock<IExcelMovementTransformer> mockExcelMovementTransformer;

        /// <summary>
        /// The error movement builder.
        /// </summary>
        private Mock<IExcelInventoryTransformer> mockExcelInventoryTransformer;

        /// <summary>
        /// The error movement builder.
        /// </summary>
        private Mock<IOwnershipInputFactory> mockOwnershipInputFactory;

        /// <summary>
        /// The excel transformer.
        /// </summary>
        private ExcelTransformer excelTransformer;

        [TestInitialize]
        public void IntilizeMethod()
        {
            this.mockExcelMovementTransformer = new Mock<IExcelMovementTransformer>();
            this.mockExcelInventoryTransformer = new Mock<IExcelInventoryTransformer>();
            this.mockOwnershipInputFactory = new Mock<IOwnershipInputFactory>();
            this.excelTransformer = new ExcelTransformer(this.mockExcelMovementTransformer.Object, this.mockExcelInventoryTransformer.Object, this.mockOwnershipInputFactory.Object);
        }

        [TestMethod]
        public async Task TransformExcelAsync_ShouldTransformExcelAsync()
        {
            using (DataSet ds = new DataSet())
            {
                var args = new List<Tuple<DataRow, string, bool>>();

                DataTable dt = new DataTable("Movimientos");
                dt.Columns.Add("Tiquete");
                dt.Columns.Add("IdMovimiento");
                dt.Columns.Add("IdNodoOrigen");
                dt.Columns.Add("DescripcionError");
                dt.Columns.Add("FechaEjecucion");

                dt.Rows.Add(25116, 1029, 12727, "La regla 1 no existe", "04/11/2019 10:20:35");
                args.Add(new Tuple<DataRow, string, bool>(dt.Rows[0], "Insert", false));
                ds.Tables.Add(dt);

                DataTable dt2 = new DataTable("Inventarios");
                dt2.Columns.Add("Tiquete");
                dt2.Columns.Add("IdInventario");
                dt2.Columns.Add("IdNodo");
                dt2.Columns.Add("DescripcionError");
                dt2.Columns.Add("FechaEjecucion");

                dt2.Rows.Add(25116, 565, 12726, "La regla 1 no existe", "04/11/2019 10:20:35");
                args.Add(new Tuple<DataRow, string, bool>(dt2.Rows[0], "Insert", false));
                ds.Tables.Add(dt2);
                ExcelInput excelInput = new ExcelInput(args[0], ds);
                this.mockOwnershipInputFactory.Setup(o => o.GetExcelInput(It.IsAny<TrueMessage>(), It.IsAny<Stream>())).Returns(excelInput);
                this.mockExcelMovementTransformer.Setup(m => m.TransformMovementAsync(excelInput, OwnershipExcelType.ERROREXCEL)).ReturnsAsync(new JArray());
                this.mockExcelInventoryTransformer.Setup(i => i.TransformInventoryAsync(excelInput, OwnershipExcelType.ERROREXCEL)).ReturnsAsync(new JArray());

                var result = await this.excelTransformer.TransformExcelAsync(It.IsAny<TrueMessage>(), It.IsAny<Stream>(), OwnershipExcelType.ERROREXCEL).ConfigureAwait(false);
                Assert.IsNotNull(result);
            }
        }
    }
}
