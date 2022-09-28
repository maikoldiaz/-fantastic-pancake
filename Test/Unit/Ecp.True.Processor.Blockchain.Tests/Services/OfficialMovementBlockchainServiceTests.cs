// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialMovementBlockchainServiceTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Blockchain.Tests.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Query;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Blockchain.Services;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Blockchain Movement Service Tests.
    /// </summary>
    [TestClass]
    public class OfficialMovementBlockchainServiceTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<OfficialMovementBlockchainService>> mockLogger;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The azureclient factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureclientFactory;

        /// <summary>
        /// The Blockchain Node Service.
        /// </summary>
        private OfficialMovementBlockchainService blockchainMovementService;

        /// <summary>
        /// The mock telemetry.
        /// </summary>
        private Mock<ITelemetry> mockTelemetry;

        /// <summary>
        /// The mock queue client.
        /// </summary>
        private Mock<IServiceBusQueueClient> mockQueueClient;

        /// <summary>
        /// The owner repository.
        /// </summary>
        private Mock<IRepository<OfficialBlockChainMovement>> movementRepository;

        /// <summary>
        /// Intilizes the method.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<OfficialMovementBlockchainService>>();
            this.mockAzureclientFactory = new Mock<IAzureClientFactory>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockTelemetry = new Mock<ITelemetry>();
            this.mockQueueClient = new Mock<IServiceBusQueueClient>();
            this.movementRepository = new Mock<IRepository<OfficialBlockChainMovement>>();
            this.mockAzureclientFactory.Setup(a => a.GetQueueClient(QueueConstants.OffchainQueue)).Returns(this.mockQueueClient.Object);
            this.mockUnitOfWorkFactory.Setup(s => s.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<OfficialBlockChainMovement>()).Returns(this.movementRepository.Object);

            this.blockchainMovementService = new OfficialMovementBlockchainService(
               this.mockLogger.Object,
               this.mockAzureclientFactory.Object,
               this.mockUnitOfWorkFactory.Object,
               this.mockTelemetry.Object);
        }

        [TestMethod]
        public async Task BlockchainOfficailMovementService_Insert_Should_RegisterAsync()
        {
            var deltaNodeId = 1;
            IEnumerable<OfficialBlockChainMovement> movements = new List<OfficialBlockChainMovement>();
            this.movementRepository.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(movements);
            await this.blockchainMovementService.RegisterAsync(deltaNodeId).ConfigureAwait(false);
            this.movementRepository.Verify(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }
    }
}