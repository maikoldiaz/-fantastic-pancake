// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainRegistrarTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Chaos.Interfaces;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Functions.Blockchain;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Blockchain.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using ExecutionContext = Microsoft.Azure.WebJobs.ExecutionContext;

    [TestClass]
    public class BlockchainRegistrarTests
    {
        /// <summary>
        /// The blockchain registrar.
        /// </summary>
        private BlockchainRegistrar blockchainRegistrar;

        /// <summary>
        /// The mock contract service.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactoryService;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<ITrueLogger<BlockchainRegistrar>> mockLogger;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private Mock<ITelemetry> mockTelemetry;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private Mock<IServiceProvider> mockServiceProvider;

        /// <summary>
        /// The mock blockchain service provider.
        /// </summary>
        private Mock<IBlockchainServiceProvider> mockBlockchainServiceProvider;

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureClientFactory;

        /// <summary>
        /// The unit of work mock.
        /// </summary>
        private Mock<IUnitOfWork> unitOfWorkMock;

        /// <summary>
        /// The unbalance repository mock.
        /// </summary>
        private Mock<IRepository<Ticket>> ticketRepositoryMock;

        /// <summary>
        /// The unbalance repository mock.
        /// </summary>
        private Mock<IRepository<Unbalance>> unbalanceRepositoryMock;

        /// <summary>
        /// The unbalance repository mock.
        /// </summary>
        private Mock<IRepository<OwnershipCalculation>> ownershipCalculationRepositoryMock;

        /// <summary>
        /// The mock connection factory.
        /// </summary>
        private Mock<IConnectionFactory> mockConnectionFactory;

        /// <summary>
        /// The mock configuration handler.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigHandler;

        /// <summary>
        /// Gets the mock SQL token provider.
        /// </summary>
        /// <value>
        /// The mock SQL token provider.
        /// </value>
        private Mock<ISqlTokenProvider> mockSqlTokenProvider;

        /// <summary>
        /// The mock chaos manager.
        /// </summary>
        private Mock<IChaosManager> mockChaosManager;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockBlockchainServiceProvider = new Mock<IBlockchainServiceProvider>();
            this.unbalanceRepositoryMock = new Mock<IRepository<Unbalance>>();
            this.ticketRepositoryMock = new Mock<IRepository<Ticket>>();
            this.ownershipCalculationRepositoryMock = new Mock<IRepository<OwnershipCalculation>>();
            this.unitOfWorkMock = new Mock<IUnitOfWork>();
            this.mockUnitOfWorkFactoryService = new Mock<IUnitOfWorkFactory>();
            this.mockUnitOfWorkFactoryService.Setup(x => x.GetUnitOfWork()).Returns(this.unitOfWorkMock.Object);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<Ticket>()).Returns(this.ticketRepositoryMock.Object);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<Unbalance>()).Returns(this.unbalanceRepositoryMock.Object);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<OwnershipCalculation>()).Returns(this.ownershipCalculationRepositoryMock.Object);
            this.unitOfWorkMock.Setup(x => x.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));
            this.mockLogger = new Mock<ITrueLogger<BlockchainRegistrar>>();
            this.mockTelemetry = new Mock<ITelemetry>();
            this.mockServiceProvider = new Mock<IServiceProvider>();
            this.blockchainRegistrar = new BlockchainRegistrar(
                this.mockBlockchainServiceProvider.Object,
                this.mockLogger.Object,
                this.mockTelemetry.Object,
                this.mockServiceProvider.Object);
        }

        /// <summary>
        /// Blockchains the registrar register movement to blockchain asynchronous.
        /// </summary>
        /// <returns>Returns completed task.</returns>
        [TestMethod]
        public async Task BlockchainRegistrar_RegisterMovementToBlockchainAsync()
        {
            this.SetupMocks(true);

            var context = new ExecutionContext();
            var movementId = 1;

            var service = new Mock<IBlockchainService>();
            this.mockBlockchainServiceProvider.Setup(m => m.GetBlockchainServiceAsync(ServiceType.Movement)).ReturnsAsync(service.Object);
            service.Setup(x => x.RegisterAsync(It.IsAny<int>()));

            await this.blockchainRegistrar.RegisterMovementAsync(movementId, null, context).ConfigureAwait(false);

            this.mockBlockchainServiceProvider.Verify(m => m.GetBlockchainServiceAsync(ServiceType.Movement), Times.Once);
            service.Verify(m => m.RegisterAsync(movementId), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task BlockchainRegistrar_RegisterMovementToBlockchain_ShouldThrowException_And_ServiceBusRetryAsync()
        {
            this.SetupMocks(true);

            var context = new ExecutionContext();
            var movementId = 1;

            var service = new Mock<IBlockchainService>();
            this.mockBlockchainServiceProvider.Setup(m => m.GetBlockchainServiceAsync(ServiceType.Movement)).ReturnsAsync(service.Object);
            service.Setup(x => x.RegisterAsync(It.IsAny<int>())).ThrowsAsync(new Exception());

            await this.blockchainRegistrar.RegisterMovementAsync(movementId, null, context).ConfigureAwait(false);

            this.mockBlockchainServiceProvider.Verify(m => m.GetBlockchainServiceAsync(ServiceType.Movement), Times.Once);
            service.Verify(m => m.RegisterAsync(movementId), Times.Once);
        }

        /// <summary>
        /// Save movements related to the official balance by node on blockchainasynchronous.
        /// </summary>
        /// <returns>Returns completed task.</returns>
        [TestMethod]
        public async Task BlockchainRegistrar_DeltaNodeApprovalToBlockchainAsync()
        {
            this.SetupMocks(true);

            var context = new ExecutionContext();
            var deltaNodeId = 1;

            var service = new Mock<IBlockchainService>();
            this.mockBlockchainServiceProvider.Setup(m => m.GetBlockchainServiceAsync(ServiceType.OfficialMovement)).ReturnsAsync(service.Object);
            service.Setup(x => x.RegisterAsync(It.IsAny<int>()));

            await this.blockchainRegistrar.DeltaNodeApprovalAsync(deltaNodeId, null, context).ConfigureAwait(false);

            this.mockBlockchainServiceProvider.Verify(m => m.GetBlockchainServiceAsync(ServiceType.OfficialMovement), Times.Once);
            service.Verify(m => m.RegisterAsync(deltaNodeId), Times.Once);
        }

        /// <summary>
        /// Blockchains the registrar register inventory to blockchain asynchronous.
        /// </summary>
        /// <returns>Returns completed task.</returns>
        [TestMethod]
        public async Task BlockchainRegistrar_RegisterInventoryToBlockchainAsync()
        {
            this.SetupMocks(true);
            var context = new ExecutionContext();
            var inventoryId = 1;

            var service = new Mock<IBlockchainService>();
            this.mockBlockchainServiceProvider.Setup(m => m.GetBlockchainServiceAsync(ServiceType.InventoryProduct)).ReturnsAsync(service.Object);
            service.Setup(x => x.RegisterAsync(It.IsAny<int>()));

            await this.blockchainRegistrar.RegisterInventoryProductAsync(inventoryId, null, context).ConfigureAwait(false);

            this.mockBlockchainServiceProvider.Verify(m => m.GetBlockchainServiceAsync(ServiceType.InventoryProduct), Times.Once);
            service.Verify(m => m.RegisterAsync(inventoryId), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task BlockchainRegistrar_RegisterInventoryToBlockchain_ShouldThrowException_And_ServiceBusRetryAsync()
        {
            this.SetupMocks(true);
            var context = new ExecutionContext();
            var inventoryId = 1;

            var service = new Mock<IBlockchainService>();
            this.mockBlockchainServiceProvider.Setup(m => m.GetBlockchainServiceAsync(ServiceType.InventoryProduct)).ReturnsAsync(service.Object);
            service.Setup(x => x.RegisterAsync(It.IsAny<int>())).ThrowsAsync(new Exception());

            await this.blockchainRegistrar.RegisterInventoryProductAsync(inventoryId, null, context).ConfigureAwait(false);

            this.mockBlockchainServiceProvider.Verify(m => m.GetBlockchainServiceAsync(ServiceType.InventoryProduct), Times.Once);
            service.Verify(m => m.RegisterAsync(inventoryId), Times.Once);
        }

        /// <summary>
        /// Blockchains the registrar register ownership to blockchain asynchronous.
        /// </summary>
        /// <returns>Returns completed task.</returns>
        [TestMethod]
        public async Task BlockchainRegistrar_RegisterOwnershipToBlockchainAsync()
        {
            this.SetupMocks(true);
            var context = new ExecutionContext();

            var service = new Mock<IBlockchainService>();
            this.mockBlockchainServiceProvider.Setup(m => m.GetBlockchainServiceAsync(ServiceType.Ownership)).ReturnsAsync(service.Object);
            service.Setup(x => x.RegisterAsync(1));

            await this.blockchainRegistrar.RegisterMovementOwnershipAsync(1, null, context).ConfigureAwait(false);

            this.mockBlockchainServiceProvider.Verify(m => m.GetBlockchainServiceAsync(ServiceType.Ownership), Times.Once);
            service.Verify(m => m.RegisterAsync(1), Times.Once);
        }

        /// <summary>
        /// Blockchains the registrar register balance to blockchain asynchronous.
        /// </summary>
        /// <returns>Returns completed task.</returns>
        [TestMethod]
        public async Task BlockchainRegistrar_RegisterBalanceToBlockchainAsync()
        {
            this.SetupMocks(true);
            var context = new ExecutionContext();

            var service = new Mock<IBlockchainService>();
            this.mockBlockchainServiceProvider.Setup(m => m.GetBlockchainServiceAsync(ServiceType.Unbalance)).ReturnsAsync(service.Object);
            service.Setup(x => x.RegisterAsync(1));

            await this.blockchainRegistrar.RegisterBalanceAsync(1, null, context).ConfigureAwait(false);

            this.mockBlockchainServiceProvider.Verify(m => m.GetBlockchainServiceAsync(ServiceType.Unbalance), Times.Once);
            service.Verify(m => m.RegisterAsync(1), Times.Once);
        }

        /// <summary>
        /// Blockchains the registrar should initialize quorum profile asynchronous.
        /// </summary>
        /// <returns>Returns completed task.</returns>
        [TestMethod]
        public async Task BlockchainRegistrar_ShouldInitializeQuorumProfileAsync()
        {
            this.SetupMocks(true);
            this.mockAzureClientFactory.Setup(a => a.IsReady).Returns(false);
            this.mockConfigHandler.Setup(a => a.GetConfigurationAsync(It.IsAny<string>())).ReturnsAsync("ConnectionString");
            var blockchainConfiguration = new BlockchainSettings
            {
                EthereumAccountAddress = "EthereumAccountAddress",
                EthereumAccountKey = "EthereumAccountKey",
                RpcEndpoint = "RpcEndpoint",
            };

            this.mockConfigHandler.Setup(m => m.GetConfigurationAsync<BlockchainSettings>(ConfigurationConstants.BlockchainSettings)).ReturnsAsync(blockchainConfiguration);

            var context = new ExecutionContext();
            var inventoryId = 1;
            var service = new Mock<IBlockchainService>();
            this.mockBlockchainServiceProvider.Setup(m => m.GetBlockchainServiceAsync(ServiceType.InventoryProduct)).ReturnsAsync(service.Object);
            service.Setup(x => x.RegisterAsync(It.IsAny<int>()));

            await this.blockchainRegistrar.RegisterInventoryProductAsync(inventoryId, null, context).ConfigureAwait(false);

            this.mockBlockchainServiceProvider.Verify(m => m.GetBlockchainServiceAsync(ServiceType.InventoryProduct), Times.Once);
            service.Verify(m => m.RegisterAsync(inventoryId), Times.Once);
        }

        /// <summary>
        /// Blockchains the registrar register node to blockchain asynchronous.
        /// </summary>
        /// <returns>Returns completed task.</returns>
        [TestMethod]
        public async Task BlockchainRegistrar_RegisterNodeToBlockchainAsync()
        {
            this.SetupMocks(true);
            var context = new ExecutionContext();
            var adminMessage = 1;

            var service = new Mock<IBlockchainService>();
            this.mockBlockchainServiceProvider.Setup(m => m.GetBlockchainServiceAsync(ServiceType.Node)).ReturnsAsync(service.Object);
            service.Setup(x => x.RegisterAsync(It.IsAny<int>()));

            await this.blockchainRegistrar.RegisterNodeAsync(adminMessage, null, context).ConfigureAwait(false);

            this.mockBlockchainServiceProvider.Verify(m => m.GetBlockchainServiceAsync(ServiceType.Node), Times.Once);
            service.Verify(m => m.RegisterAsync(adminMessage), Times.Once);
        }

        /// <summary>
        /// Blockchains the registrar register node connection to blockchain asynchronous.
        /// </summary>
        /// <returns>Returns completed task.</returns>
        [TestMethod]
        public async Task BlockchainRegistrar_RegisterNodeConnectionToBlockchainAsync()
        {
            this.SetupMocks(true);
            var context = new ExecutionContext();
            var adminMessage = 1;

            var service = new Mock<IBlockchainService>();
            this.mockBlockchainServiceProvider.Setup(m => m.GetBlockchainServiceAsync(ServiceType.NodeConnection)).ReturnsAsync(service.Object);
            service.Setup(x => x.RegisterAsync(It.IsAny<int>()));

            await this.blockchainRegistrar.RegisterNodeConnectionAsync(adminMessage, null, context).ConfigureAwait(false);

            this.mockBlockchainServiceProvider.Verify(m => m.GetBlockchainServiceAsync(ServiceType.NodeConnection), Times.Once);
            service.Verify(m => m.RegisterAsync(adminMessage), Times.Once);
        }

        [TestMethod]
        public async Task BlockchainRegistrar_RegisterOwnerAsync()
        {
            this.SetupMocks(true);

            var context = new ExecutionContext();
            var ownerId = 1;

            var service = new Mock<IBlockchainService>();
            this.mockBlockchainServiceProvider.Setup(m => m.GetBlockchainServiceAsync(ServiceType.Owner)).ReturnsAsync(service.Object);
            service.Setup(x => x.RegisterAsync(It.IsAny<int>()));

            await this.blockchainRegistrar.RegisterOwnerAsync(ownerId, null, context).ConfigureAwait(false);

            this.mockBlockchainServiceProvider.Verify(m => m.GetBlockchainServiceAsync(ServiceType.Owner), Times.Once);
            service.Verify(m => m.RegisterAsync(ownerId), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task BlockchainRegistrar_RegisterOwner_ShouldThrowException_And_ServiceBusRetryAsync()
        {
            this.SetupMocks(true);

            var context = new ExecutionContext();
            var ownerId = 1;

            var service = new Mock<IBlockchainService>();
            this.mockBlockchainServiceProvider.Setup(m => m.GetBlockchainServiceAsync(ServiceType.Owner)).ReturnsAsync(service.Object);
            service.Setup(x => x.RegisterAsync(It.IsAny<int>())).ThrowsAsync(new Exception());

            await this.blockchainRegistrar.RegisterOwnerAsync(ownerId, null, context).ConfigureAwait(false);

            this.mockBlockchainServiceProvider.Verify(m => m.GetBlockchainServiceAsync(ServiceType.Owner), Times.Once);
            service.Verify(m => m.RegisterAsync(ownerId), Times.Once);
        }

        /// <summary>
        /// Setups the mocks.
        /// </summary>
        /// <param name="isReady">if set to <c>true</c> [is ready].</param>
        private void SetupMocks(bool isReady)
        {
            this.mockConnectionFactory = new Mock<IConnectionFactory>();
            this.mockConfigHandler = new Mock<IConfigurationHandler>();
            this.mockAzureClientFactory = new Mock<IAzureClientFactory>();
            this.mockSqlTokenProvider = new Mock<ISqlTokenProvider>();
            this.mockChaosManager = new Mock<IChaosManager>();

            this.mockConnectionFactory.Setup(m => m.SetupStorageConnection(It.IsAny<string>()));
            this.mockConnectionFactory.Setup(m => m.SetupSqlConfig(It.IsAny<SqlConnectionSettings>()));
            this.mockConnectionFactory.SetupGet(m => m.IsReady).Returns(isReady);

            this.mockAzureClientFactory.SetupGet(m => m.IsReady).Returns(isReady);
            this.mockAzureClientFactory.Setup(m => m.Initialize(It.IsAny<AzureConfiguration>()));

            var blockchainConfiguration = new BlockchainSettings();

            this.mockConfigHandler.Setup(m => m.GetConfigurationAsync<BlockchainSettings>(ConfigurationConstants.BlockchainSettings)).ReturnsAsync(blockchainConfiguration);

            this.mockServiceProvider.Setup(s => s.GetService(typeof(IConnectionFactory))).Returns(this.mockConnectionFactory.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(ITrueLogger<BlockchainRegistrar>))).Returns(this.mockLogger.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IConfigurationHandler))).Returns(this.mockConfigHandler.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IAzureClientFactory))).Returns(this.mockAzureClientFactory.Object);
            this.mockServiceProvider.Setup(r => r.GetService(typeof(ISqlTokenProvider))).Returns(this.mockSqlTokenProvider.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IChaosManager))).Returns(this.mockChaosManager.Object);
        }
    }
}
