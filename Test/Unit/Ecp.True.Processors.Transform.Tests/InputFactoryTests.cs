// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputFactoryTests.cs" company="Microsoft">
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
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Transform.Input;
    using Ecp.True.Processors.Transform.Services.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Input factory tests.
    /// </summary>
    [TestClass]
    public class InputFactoryTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private readonly Mock<ITrueLogger<InputFactory>> mockLogger = new Mock<ITrueLogger<InputFactory>>();

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The BLOB client.
        /// </summary>
        private readonly Mock<IBlobStorageSasClient> mockBlobClient = new Mock<IBlobStorageSasClient>();

        /// <summary>
        /// The mock data service.
        /// </summary>
        private readonly Mock<IDataService> mockDataService = new Mock<IDataService>();

        /// <summary>
        /// The mock file registration transaction service.
        /// </summary>
        private readonly Mock<IFileRegistrationTransactionService> mockFileRegistrationTransactionService = new Mock<IFileRegistrationTransactionService>();

        /// <summary>
        /// The input factory.
        /// </summary>
        private InputFactory inputFactory;

        /// <summary>
        /// Initilizes this instance.
        /// </summary>
        [TestInitialize]
        public void Initilize()
        {
            this.mockAzureClientFactory.Setup(m => m.GetBlobStorageSaSClient(It.IsAny<string>(), It.IsAny<string>())).Returns(this.mockBlobClient.Object);
            this.inputFactory = new InputFactory(this.mockLogger.Object, this.mockAzureClientFactory.Object, this.mockDataService.Object, this.mockFileRegistrationTransactionService.Object);
        }

        /// <summary>
        /// Gets the json input asynchronous should throw argument exception when BLOB path null asynchronous.
        /// </summary>
        /// <returns>Representing the asynchronous unit test.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetJsonInputAsync_ShouldThrowArgumentException_WhenBlobPathNullAsync()
        {
            await this.inputFactory.GetJsonInputAsync(string.Empty).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the json input asynchronous return stream when invoked asynchronous.
        /// </summary>
        /// <returns>Representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task GetJsonInputAsync_ReturnDeserializeStream_WhenInvokedAsync()
        {
            // Arrange
            this.mockBlobClient.Setup(m => m.ParseAsync<JToken>()).ReturnsAsync(JToken.Parse("{ InventoryId : 2 }"));
            this.mockAzureClientFactory.Setup(m => m.GetBlobStorageSaSClient(It.IsAny<string>(), It.IsAny<string>())).Returns(this.mockBlobClient.Object);

            // Act
            var result = await this.inputFactory.GetJsonInputAsync("BlobPath").ConfigureAwait(false);

            // Assert
            Assert.AreEqual(true, result.First.HasValues);
        }

        /// <summary>
        /// Gets the excel input asynchronous should throw argument exception when message is null asynchronous.
        /// </summary>
        /// <returns>Representing the asynchronous unit test.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetExcelInputAsync_ShouldThrowArgumentException_WhenMessageIsNullAsync()
        {
            await this.inputFactory.GetExcelInputAsync(null).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the excel input asynchronous should return excel input when invoke asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetExcelInputAsync_ShouldReturnExcelInput_WhenInvokeAsync()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var filePath = @"Excel/ValidMovementInventoryExcel.xlsx";
            var byteArray = File.ReadAllBytes(filePath);
            using (Stream stream = new MemoryStream(byteArray))
            {
                // Arrange
                this.mockBlobClient.Setup(m => m.GetCloudBlobStreamAsync()).ReturnsAsync(stream);
                this.mockAzureClientFactory.Setup(m => m.GetBlobStorageSaSClient(It.IsAny<string>(), It.IsAny<string>())).Returns(this.mockBlobClient.Object);

                // Act
                var result = await this.inputFactory.GetExcelInputAsync(new TrueMessage()).ConfigureAwait(false);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Input);
                Assert.AreEqual(6, result.Input.Tables.Count);
                Assert.AreEqual("INVENTARIOS", result.Input.Tables[0].TableName);
                Assert.AreEqual("PROPIETARIOSINV", result.Input.Tables[1].TableName);
                Assert.AreEqual("CALIDADINV", result.Input.Tables[2].TableName);
                Assert.AreEqual("MOVIMIENTOS", result.Input.Tables[3].TableName);
                Assert.AreEqual("PROPIETARIOSMOV", result.Input.Tables[4].TableName);
                Assert.AreEqual("CALIDADMOV", result.Input.Tables[5].TableName);
            }
        }

        /// <summary>
        /// Gets the excel input asynchronous should throw invalid data exception when invoke asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public async Task GetExcelInputAsync_ShouldThrowInvalidDataException_WhenInvokeAsync()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var filePath = @"Excel/InValidMovementInventoryExcel.xlsx";
            var byteArray = File.ReadAllBytes(filePath);
            using (Stream stream = new MemoryStream(byteArray))
            {
                // Arrange
                this.mockBlobClient.Setup(m => m.GetCloudBlobStreamAsync()).ReturnsAsync(stream);
                this.mockAzureClientFactory.Setup(m => m.GetBlobStorageSaSClient(It.IsAny<string>(), It.IsAny<string>())).Returns(this.mockBlobClient.Object);

                // Act
                await this.inputFactory.GetExcelInputAsync(new TrueMessage()).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the file registration asynchronous return file registration when inoked asynchronous.
        /// </summary>
        /// <returns>Representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task GetFileRegistrationAsync_ReturnFileRegistration_WhenInokedAsync()
        {
            // Arrange
            this.mockFileRegistrationTransactionService.Setup(m => m.GetFileRegistrationAsync(It.IsAny<string>())).ReturnsAsync(new FileRegistration());

            // Act
            var result = await this.inputFactory.GetFileRegistrationAsync(It.IsAny<string>()).ConfigureAwait(false);

            // Assert
            Assert.IsInstanceOfType(result, typeof(FileRegistration));
        }

        /// <summary>
        /// Gets the file registration transaction asynchronous return file trgistration when inoked asynchronous.
        /// </summary>
        /// <returns>Representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task GetFileRegistrationTransactionAsync_ReturnFileTrgistration_WhenInokedAsync()
        {
            // Arrange
            this.mockFileRegistrationTransactionService.Setup(m => m.GetFileRegistrationTransactionAsync(It.IsAny<int>())).ReturnsAsync(new FileRegistrationTransaction());

            // Act
            var result = await this.inputFactory.GetFileRegistrationTransactionAsync(It.IsAny<int>()).ConfigureAwait(false);

            // Assert
            Assert.IsInstanceOfType(result, typeof(FileRegistrationTransaction));
        }

        /// <summary>
        /// Saves the sap json array asynchronous save external source entity asynchronous when invoked asynchronous.
        /// </summary>
        /// <returns>Representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task SaveSapJsonArrayAsync_SaveExternalSourceEntityAsync_WhenInvokedAsync()
        {
            // Arrange
            this.mockDataService.Setup(m => m.SaveExternalSourceEntityArrayAsync(It.IsAny<JArray>(), It.IsAny<TrueMessage>()));
            var message = new TrueMessage() { SourceSystem = SystemType.SINOPER };

            // Act
            await this.inputFactory.SaveSapJsonArrayAsync(new JArray(), message).ConfigureAwait(false);

            // Assert
            this.mockDataService.Verify(m => m.SaveExternalSourceEntityArrayAsync(It.IsAny<JArray>(), It.IsAny<TrueMessage>()), Times.Exactly(1));
        }

        /// <summary>
        /// Saves the sap json asynchronous save external source entity asynchronous when invoked asynchronous.
        /// </summary>
        /// <returns>Representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task SaveSapJsonAsync_SaveExternalSourceEntityAsync_WhenInvokedAsync()
        {
            // Arrange
            this.mockDataService.Setup(m => m.SaveExternalSourceEntityAsync(It.IsAny<JObject>(), It.IsAny<TrueMessage>()));
            var message = new TrueMessage() { SourceSystem = SystemType.LOGISTIC };

            // Act
            await this.inputFactory.SaveSapJsonAsync(new JObject(), message).ConfigureAwait(false);

            // Assert
            this.mockDataService.Verify(m => m.SaveExternalSourceEntityAsync(It.IsAny<JObject>(), It.IsAny<TrueMessage>()), Times.Exactly(1));
        }

        /// <summary>
        /// Generates the stream from string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>The stream.</returns>
        private Stream GenerateStreamFromString(string s)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(s));
        }
    }
}
