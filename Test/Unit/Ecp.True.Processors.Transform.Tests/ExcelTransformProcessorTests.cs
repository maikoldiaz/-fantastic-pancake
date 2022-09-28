// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelTransformProcessorTests.cs" company="Microsoft">
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
    using System.Data;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Transform.Input.Interfaces;
    using Ecp.True.Processors.Transform.Services.Excel;
    using Ecp.True.Processors.Transform.Services.Excel.Interfaces;
    using Ecp.True.Processors.Transform.Services.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The ExcelTransformProcessorTests.
    /// </summary>
    [TestClass]
    public class ExcelTransformProcessorTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<ExcelTransformProcessor>> mockLogger;

        /// <summary>
        /// The mock input factory.
        /// </summary>
        private Mock<IInputFactory> mockInputFactory;

        /// <summary>
        /// The mock excel transformer.
        /// </summary>
        private Mock<IExcelTransformer> mockExcelTransformer;

        /// <summary>
        /// The mock data service.
        /// </summary>
        private Mock<IDataService> mockDataService;

        /// <summary>
        /// The excel transform processor.
        /// </summary>
        private ExcelTransformProcessor excelTransformProcessor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<ExcelTransformProcessor>>();
            this.mockInputFactory = new Mock<IInputFactory>();
            this.mockExcelTransformer = new Mock<IExcelTransformer>();
            this.mockDataService = new Mock<IDataService>();

            this.excelTransformProcessor = new ExcelTransformProcessor(
                this.mockLogger.Object,
                this.mockInputFactory.Object,
                this.mockExcelTransformer.Object,
                this.mockDataService.Object);
        }

        /// <summary>
        /// Transforms the excel asynchronous should transform excel when transform excel invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task TransformExcelAsync_ShouldTransformExcel_WhenTransformExcelInvokedAsync()
        {
            using (var dataSet = new DataSet())
            {
                var trueMessage = new TrueMessage
                {
                    Message = MessageType.MovementAndInventory,
                    SourceSystem = SystemType.EXCEL,
                    TargetSystem = SystemType.TRUE,
                };

                this.mockInputFactory.Setup(m => m.GetFileRegistrationAsync(It.IsAny<string>())).ReturnsAsync(new FileRegistration());
                this.mockInputFactory.Setup(m => m.GetExcelInputAsync(It.IsAny<TrueMessage>())).ReturnsAsync(new ExcelInput(dataSet));
                this.mockExcelTransformer.Setup(m => m.TransformExcelAsync(It.IsAny<ExcelInput>())).ReturnsAsync(new JArray());

                await this.excelTransformProcessor.TransformAsync(trueMessage).ConfigureAwait(false);

                this.mockInputFactory.Verify(m => m.GetFileRegistrationAsync(It.IsAny<string>()), Times.Once);
                this.mockInputFactory.Verify(m => m.GetExcelInputAsync(It.IsAny<TrueMessage>()), Times.Once);
                this.mockExcelTransformer.Verify(m => m.TransformExcelAsync(It.IsAny<ExcelInput>()), Times.Once);
            }
        }

        /// <summary>
        /// Transforms the excel asynchronous should complete asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task TransformExcelAsync_ShouldCompleteAsync()
        {
            var array = new JArray();
            var message = new TrueMessage();

            this.mockDataService.Setup(a => a.SaveExcelAsync(It.IsAny<JArray>(), It.IsAny<JArray>(), It.IsAny<JArray>(), It.IsAny<JArray>(), It.IsAny<TrueMessage>()));
            await this.excelTransformProcessor.CompleteAsync(array, message).ConfigureAwait(false);

            this.mockDataService.Verify(a => a.SaveExcelAsync(It.IsAny<JArray>(), It.IsAny<JArray>(), It.IsAny<JArray>(), It.IsAny<JArray>(), It.IsAny<TrueMessage>()), Times.Once);
        }

        /// <summary>
        /// Transforms the excel asynchronous system type should return system type sinoper when invoked.
        /// </summary>
        [TestMethod]
        public void TransformExcelAsync_SystemType_ShouldReturnSystemTypeExcel_WhenInvoked()
        {
            var result = this.excelTransformProcessor.InputType;

            Assert.AreEqual(InputType.EXCEL, result);
        }

        /// <summary>
        /// Transforms the excel asynchronous complete asynchronous when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task TransformExcelAsync_CompleteAsync_WhenInvokedAsync()
        {
            var homologatedArray = new JArray();
            homologatedArray.Add("Inventory");
            homologatedArray.Add("Movements");
            homologatedArray.Add("Events");
            homologatedArray.Add("Contracts");

            this.mockDataService.Setup(a => a.SaveExcelAsync(It.IsAny<JArray>(), It.IsAny<JArray>(), It.IsAny<JArray>(), It.IsAny<JArray>(), It.IsAny<TrueMessage>()));
            await this.excelTransformProcessor.CompleteAsync(homologatedArray, new TrueMessage()).ConfigureAwait(false);

            this.mockDataService.Verify(a => a.SaveExcelAsync(It.IsAny<JArray>(), It.IsAny<JArray>(), It.IsAny<JArray>(), It.IsAny<JArray>(), It.IsAny<TrueMessage>()), Times.Once);
        }
    }
}