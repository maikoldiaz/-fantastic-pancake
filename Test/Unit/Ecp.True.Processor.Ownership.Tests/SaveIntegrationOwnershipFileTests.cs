// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveIntegrationOwnershipFileTests.cs" company="Microsoft">
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

namespace Ecp.True.Processor.Ownership.Tests
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Processors.Ownership.Integration;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The Ownership Integration Fico Tests.
    /// </summary>
    [TestClass]
    public class SaveIntegrationOwnershipFileTests
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
        /// The Ownership integration fico.
        /// </summary>
        private ISaveIntegrationOwnershipFile ownershipIntegrationOwnershipFico;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.azureClientFactory = new Mock<IAzureClientFactory>();
            this.fileRegistrationTransactionService = new Mock<IFileRegistrationTransactionService>();

            this.ownershipIntegrationOwnershipFico = new SaveIntegrationOwnershipFile(
                this.azureClientFactory.Object,
                this.fileRegistrationTransactionService.Object);
        }

        [TestMethod]
        public async Task ShouldSaveIntegration_WithSuccessful_Ownership_Request_RegisterIntegrationAsync()
        {
            var integrationData = new IntegrationData
            {
                Id = 0,
                Data = "{}",
                Type = SystemType.OWNERSHIP,
                IntegrationType = IntegrationType.REQUEST,
            };

            this.azureClientFactory.Setup(x =>
                x.GetBlobStorageSaSClient(ContainerName.Ownership, It.IsAny<string>()).CreateBlobAsync(It.IsAny<string>()));
            this.fileRegistrationTransactionService.Setup(x => x.InsertFileRegistrationAsync(It.IsAny<FileRegistration>()));

            await this.ownershipIntegrationOwnershipFico.RegisterIntegrationAsync(integrationData).ConfigureAwait(false);

            this.azureClientFactory.Verify(x => x.GetBlobStorageSaSClient(ContainerName.Ownership, It.IsAny<string>()).CreateBlobAsync(It.IsAny<string>()), Times.Once);
            this.fileRegistrationTransactionService.Verify(x => x.InsertFileRegistrationAsync(It.IsAny<FileRegistration>()), Times.Once);
        }

        [TestMethod]
        public async Task ShouldNotSaveIntegration_WhenDataIsNull_WithError_RegisterIntegrationAsync()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => this.ownershipIntegrationOwnershipFico.RegisterIntegrationAsync(null)).ConfigureAwait(false);
        }
    }
}
