// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelContractTransformerTests.cs" company="Microsoft">
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
    using Ecp.True.Entities.Dto;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Transform.Builders.Excel.Interfaces;
    using Ecp.True.Processors.Transform.Services.Excel;
    using Microsoft.EntityFrameworkCore.Internal;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The ExcelContractTransformerTests.
    /// </summary>
    [TestClass]
    public class ExcelContractTransformerTests
    {
        /// <summary>
        /// The mock excel contract builder.
        /// </summary>
        private Mock<IExcelContractBuilder> mockExcelContractBuilder;

        /// <summary>
        /// The excel contract transformer.
        /// </summary>
        private ExcelContractTransformer excelContractTransformer;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockExcelContractBuilder = new Mock<IExcelContractBuilder>();

            this.excelContractTransformer = new ExcelContractTransformer(this.mockExcelContractBuilder.Object);
        }

        /// <summary>
        /// Transforms the contract asynchronous should transform when invoke asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task TransformContractAsync_ShouldTransform_WhenInvokeAsync()
        {
            DataTable table = new DataTable("Pedidos");
            table.Columns.Add("Column1");
            table.Columns.Add("Column2");
            table.Rows.Add("Contract1", 1);
            table.Rows.Add("Contract2", 2);
            using (var dataSet = new DataSet())
            {
                dataSet.Tables.Add(table);
                var trueMessage = new TrueMessage
                {
                    ActionType = "Insert",
                };

                var excelInput = new ExcelInput(dataSet, trueMessage);

                this.mockExcelContractBuilder.Setup(a => a.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(new JObject());

                var output = await this.excelContractTransformer.TransformContractAsync(excelInput).ConfigureAwait(false);

                Assert.IsInstanceOfType(output, typeof(JArray));
                Assert.IsFalse(excelInput.Message.PendingTransactions.Any());
            }
        }

        /// <summary>
        /// Transforms the contract asynchronous should populate pending transactions for exceptions when invoke asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task TransformContractAsync_ShouldPopulatePendingTransactions_For_Exceptions_WhenInvokeAsync()
        {
            DataTable table = new DataTable("Pedidos");
            table.Columns.Add("name");
            table.Columns.Add("id");
            table.Rows.Add("sam", 1);
            table.Rows.Add("mark", 2);
            using (var dataSet = new DataSet())
            {
                dataSet.Tables.Add(table);
                var trueMessage = new TrueMessage
                {
                    ActionType = "Insert",
                };

                var excelInput = new ExcelInput(dataSet, trueMessage);

                this.mockExcelContractBuilder.Setup(a => a.BuildAsync(It.IsAny<ExcelInput>())).ThrowsAsync(new Exception());

                var output = await this.excelContractTransformer.TransformContractAsync(excelInput).ConfigureAwait(false);

                Assert.IsNotNull(output);
                Assert.IsTrue(excelInput.Message.PendingTransactions.Any());
            }
        }

        /// <summary>
        /// Transforms the contract asynchronous should throw missing field exception when invoke asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(MissingFieldException))]
        public async Task TransformContractAsync_ShouldThrowMissingFieldException_WhenInvokeAsync()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Column1");
            table.Columns.Add("Column2");
            table.Rows.Add("Contract1", 1);
            table.Rows.Add("Contract2", 2);
            using (var dataSet = new DataSet())
            {
                dataSet.Tables.Add(table);
                var trueMessage = new TrueMessage
                {
                    ActionType = "Insert",
                };

                var excelInput = new ExcelInput(dataSet, trueMessage);

                this.mockExcelContractBuilder.Setup(a => a.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(new JObject());

                await this.excelContractTransformer.TransformContractAsync(excelInput).ConfigureAwait(false);
            }
        }
    }
}
