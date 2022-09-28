// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelEventTransformerTests.cs" company="Microsoft">
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
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Transform.Builders.Excel.Interfaces;
    using Ecp.True.Processors.Transform.Services.Excel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The ExcelEventTransformerTests.
    /// </summary>
    [TestClass]
    public class ExcelEventTransformerTests
    {
        /// <summary>
        /// The mock i excel event builder.
        /// </summary>
        private Mock<IExcelEventBuilder> mockIExcelEventBuilder;

        /// <summary>
        /// The excel event transformer.
        /// </summary>
        private ExcelEventTransformer excelEventTransformer;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockIExcelEventBuilder = new Mock<IExcelEventBuilder>();
            this.excelEventTransformer = new ExcelEventTransformer(this.mockIExcelEventBuilder.Object);
        }

        /// <summary>
        /// Transforms the event asynchronous should transform when invoke asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task TransformEventAsync_ShouldTransform_WhenInvokeAsync()
        {
            DataTable table = new DataTable("Eventos");
            table.Columns.Add("Column1");
            table.Columns.Add("Column2");
            table.Rows.Add("Event1", 1);
            table.Rows.Add("Event2", 2);
            using (var dataSet = new DataSet())
            {
                dataSet.Tables.Add(table);
                var trueMessage = new TrueMessage
                {
                    ActionType = "Insert",
                };

                var excelInput = new ExcelInput(dataSet, trueMessage);

                this.mockIExcelEventBuilder.Setup(a => a.BuildAsync(It.IsAny<ExcelInput>())).ReturnsAsync(new JObject());

                var output = await this.excelEventTransformer.TransformEventAsync(excelInput).ConfigureAwait(false);

                Assert.IsInstanceOfType(output, typeof(JArray));
                Assert.IsFalse(excelInput.Message.PendingTransactions.Any());
            }
        }

        /// <summary>
        /// Transforms the event asynchronous should populate pending transactions for exceptions when invoke asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task TransformEventAsync_ShouldPopulatePendingTransactions_For_Exceptions_WhenInvokeAsync()
        {
            DataTable table = new DataTable("Eventos");
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

                this.mockIExcelEventBuilder.Setup(a => a.BuildAsync(It.IsAny<ExcelInput>())).ThrowsAsync(new Exception());

                var output = await this.excelEventTransformer.TransformEventAsync(excelInput).ConfigureAwait(false);

                Assert.IsNotNull(output);
                Assert.IsTrue(excelInput.Message.PendingTransactions.Any());
            }
        }

        /// <summary>
        /// Transforms the event asynchronous should throw missing field exception when invoke asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [ExpectedException(typeof(MissingFieldException))]
        public async Task TransformEventAsync_ShouldThrowMissingFieldException_WhenInvokeAsync()
        {
            DataTable table = new DataTable();
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

                this.mockIExcelEventBuilder.Setup(a => a.BuildAsync(It.IsAny<ExcelInput>())).ThrowsAsync(new Exception());

                await this.excelEventTransformer.TransformEventAsync(excelInput).ConfigureAwait(false);
            }
        }
    }
}
