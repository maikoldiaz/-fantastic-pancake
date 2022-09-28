// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveIntegrationDeltaFileTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
// cSpell:ignore FICO, Fico, fico

namespace Ecp.True.Processor.Delta.Tests
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Processors.Delta.Integration;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The Delta Integration Fico Tests.
    /// </summary>
    [TestClass]
    public class SaveIntegrationDeltaFileTests
    {
        /// <summary>
        /// The azure client factory mock.
        /// </summary>
        private Mock<IAzureClientFactory> azureClientFactory;

        /// <summary>
        /// The file registration transaction service mock.
        /// </summary>
        private Mock<IFileRegistrationTransactionService> fileRegistrationTransactionService;

        /// <summary>
        /// The delta integration fico.
        /// </summary>
        private ISaveIntegrationDeltaFile deltaIntegrationDeltaFico;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.azureClientFactory = new Mock<IAzureClientFactory>();
            this.fileRegistrationTransactionService = new Mock<IFileRegistrationTransactionService>();

            this.deltaIntegrationDeltaFico = new SaveIntegrationDeltaFile(
                this.azureClientFactory.Object,
                this.fileRegistrationTransactionService.Object);
        }

        [TestMethod]
        public async Task ShouldSaveIntegration_WithSuccessful_OperativeDelta_Request_RegisterIntegrationAsync()
        {
            var integrationData = new IntegrationData
            {
                Id = 0,
                Data = "{}",
                Type = SystemType.OPERATIVEDELTA,
                IntegrationType = IntegrationType.REQUEST,
            };

            this.azureClientFactory.Setup(x =>
                x.GetBlobStorageSaSClient(ContainerName.Delta, It.IsAny<string>()).CreateBlobAsync(It.IsAny<string>()));
            this.fileRegistrationTransactionService.Setup(x => x.InsertFileRegistrationAsync(It.IsAny<FileRegistration>()));

            await this.deltaIntegrationDeltaFico.RegisterIntegrationAsync(integrationData).ConfigureAwait(false);

            this.azureClientFactory.Verify(x => x.GetBlobStorageSaSClient(ContainerName.Delta, It.IsAny<string>()).CreateBlobAsync(It.IsAny<string>()), Times.Once);
            this.fileRegistrationTransactionService.Verify(x => x.InsertFileRegistrationAsync(It.IsAny<FileRegistration>()), Times.Once);
        }

        [TestMethod]
        public async Task ShouldSaveIntegration_WithSuccessfu_OfficialDelta_Requestl_RegisterIntegrationAsync()
        {
            var integrationData = new IntegrationData
            {
                Id = 0,
                Data = "{}",
                Type = SystemType.OFFICIALDELTA,
                IntegrationType = IntegrationType.RESPONSE,
                PreviousUploadId = Guid.NewGuid().ToString(),
            };

            this.azureClientFactory.Setup(x =>
                x.GetBlobStorageSaSClient(ContainerName.Delta, It.IsAny<string>()).CreateBlobAsync(It.IsAny<string>()));
            this.fileRegistrationTransactionService.Setup(x => x.InsertFileRegistrationAsync(It.IsAny<FileRegistration>()));

            await this.deltaIntegrationDeltaFico.RegisterIntegrationAsync(integrationData).ConfigureAwait(false);

            this.azureClientFactory.Verify(x => x.GetBlobStorageSaSClient(ContainerName.Delta, It.IsAny<string>()).CreateBlobAsync(It.IsAny<string>()), Times.Once);
            this.fileRegistrationTransactionService.Verify(x => x.InsertFileRegistrationAsync(It.IsAny<FileRegistration>()), Times.Once);
        }

        [TestMethod]
        public async Task ShouldNotSaveIntegration_WhenDataIsNull_WithError_RegisterIntegrationAsync()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => this.deltaIntegrationDeltaFico.RegisterIntegrationAsync(null)).ConfigureAwait(false);
        }
    }
}
