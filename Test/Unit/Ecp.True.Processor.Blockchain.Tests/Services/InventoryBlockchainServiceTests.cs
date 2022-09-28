// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryBlockchainServiceTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Blockchain.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Numerics;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Core.Dto;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Blockchain.Services;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Azure.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Nethereum.Hex.HexTypes;
    using Nethereum.RPC.Eth.DTOs;

    /// <summary>
    /// Blockchain InventoryProduct Service Tests.
    /// </summary>
    [TestClass]
    public class InventoryBlockchainServiceTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<InventoryProductBlockchainService>> mockLogger;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The azureclient factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureclientFactory;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The mock InventoryProduct repository.
        /// </summary>
        private Mock<IRepository<InventoryProduct>> mockInventoryProductRepository;

        /// <summary>
        /// The mock telemetry.
        /// </summary>
        private Mock<ITelemetry> mockTelemetry;

        /// <summary>
        /// The mock queue client.
        /// </summary>
        private Mock<IServiceBusQueueClient> mockQueueClient;

        /// <summary>
        /// The Blockchain Node Service.
        /// </summary>
        private InventoryProductBlockchainService blockchainInventoryService;

        /// <summary>
        /// The Blockchain Node Service.
        /// </summary>
        private Mock<IInventoryProductRepository> mockInventoryRepoMock;

        /// <summary>
        /// Initialize the method.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<InventoryProductBlockchainService>>();
            this.mockAzureclientFactory = new Mock<IAzureClientFactory>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockTelemetry = new Mock<ITelemetry>();
            this.mockQueueClient = new Mock<IServiceBusQueueClient>();

            this.mockTelemetry.Setup(t => t.TrackEvent(Constants.Critical, EventName.OffchainSyncFailed.ToString("G"), It.IsAny<IDictionary<string, string>>(), null, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()));

            this.mockAzureclientFactory.Setup(a => a.GetQueueClient(QueueConstants.OffchainQueue)).Returns(this.mockQueueClient.Object);
            this.mockQueueClient.Setup(m => m.QueueMessageAsync(It.IsAny<OffchainMessage>()));

            this.mockUnitOfWorkFactory.Setup(s => s.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockInventoryProductRepository = new Mock<IRepository<InventoryProduct>>();
            this.mockUnitOfWork.Setup(m => m.CreateRepository<InventoryProduct>()).Returns(this.mockInventoryProductRepository.Object);
            this.mockInventoryRepoMock = new Mock<IInventoryProductRepository>();
            this.mockUnitOfWork.Setup(m => m.InventoryProductRepository).Returns(this.mockInventoryRepoMock.Object);

            this.blockchainInventoryService = new InventoryProductBlockchainService(
               this.mockLogger.Object,
               this.mockAzureclientFactory.Object,
               this.mockUnitOfWorkFactory.Object,
               this.mockTelemetry.Object);
        }

        /// <summary>
        /// Blockchains the InventoryProduct service insert should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BlockchainInventoryService_Insert_Should_RegisterAsync()
        {
            var inventoryProductId = 1;
            var (inventoryProduct, owners) = this.GetInventoryProduct("Insert", false, Entities.Core.StatusType.PROCESSING);
            var contractMetadataStruct = new ContractMetadataStruct
            {
                ContractAddress = "ContractAddress",
                ContractAbi = "ContractAbi",
            };
            var transactionReceipt = new TransactionReceipt
            {
                TransactionHash = "TestHash",
                BlockNumber = new HexBigInteger(new BigInteger(10000)),
                GasUsed = new HexBigInteger(new BigInteger(70000)),
                CumulativeGasUsed = new HexBigInteger(new BigInteger(700000)),
            };

            this.mockInventoryProductRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(inventoryProduct);
            this.mockInventoryProductRepository.Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>())).ReturnsAsync(0);
            this.mockInventoryRepoMock.Setup(a => a.GetLatestBlockchainInventoryProductAsync(It.IsAny<string>())).ReturnsAsync((InventoryProduct)null);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), "GetLatest", It.IsAny<Dictionary<string, object>>())).ReturnsAsync(contractMetadataStruct);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);

            await this.blockchainInventoryService.RegisterAsync(inventoryProductId).ConfigureAwait(false);

            this.mockInventoryProductRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockInventoryProductRepository.Verify(r => r.GetCountAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>()), Times.Once);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), "GetLatest", It.IsAny<Dictionary<string, object>>()), Times.Exactly(3));
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Exactly(2));
        }

        /// <summary>
        /// Blockchains the InventoryProduct service insert should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BlockchainInventoryService_Insert_Should_Register_BridgeAsync()
        {
            var inventoryProductId = 1;
            var (inventoryProduct, owners) = this.GetInventoryProduct("Insert", false, Entities.Core.StatusType.PROCESSING);
            var contractMetadataStruct = new ContractMetadataStruct
            {
                ContractAddress = "ContractAddress",
                ContractAbi = "ContractAbi",
            };
            var transactionReceipt = new TransactionReceipt
            {
                TransactionHash = "TestHash",
                BlockNumber = new HexBigInteger(new BigInteger(10000)),
                GasUsed = new HexBigInteger(new BigInteger(70000)),
                CumulativeGasUsed = new HexBigInteger(new BigInteger(700000)),
            };

            this.mockInventoryProductRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(inventoryProduct);
            this.mockInventoryProductRepository.Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>())).ReturnsAsync(0);
            this.mockInventoryRepoMock.Setup(a => a.GetLatestBlockchainInventoryProductAsync(It.IsAny<string>())).ReturnsAsync(new InventoryProduct { EventType = EventType.Insert.ToString("G"), ProductVolume = 10.00M, BlockNumber = "0x908" });
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), "GetLatest", It.IsAny<Dictionary<string, object>>())).ReturnsAsync(contractMetadataStruct);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);

            await this.blockchainInventoryService.RegisterAsync(inventoryProductId).ConfigureAwait(false);

            this.mockInventoryProductRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockInventoryProductRepository.Verify(r => r.GetCountAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>()), Times.Once);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), "GetLatest", It.IsAny<Dictionary<string, object>>()), Times.Exactly(2));
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Exactly(2));
        }

        /// <summary>
        /// Blockchains the InventoryProduct service update should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BlockchainInventoryService_Update_Should_RegisterAsync()
        {
            var inventoryProductId = 1;
            var (inventoryProduct, owners) = this.GetInventoryProduct("Update", true, Entities.Core.StatusType.PROCESSING);
            var contractMetadataStruct = new ContractMetadataStruct
            {
                ContractAddress = "ContractAddress",
                ContractAbi = "ContractAbi",
            };
            var transactionReceipt = new TransactionReceipt
            {
                TransactionHash = "TestHash",
                BlockNumber = new HexBigInteger(new BigInteger(10000)),
                GasUsed = new HexBigInteger(new BigInteger(70000)),
                CumulativeGasUsed = new HexBigInteger(new BigInteger(700000)),
            };

            this.mockInventoryProductRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(inventoryProduct);
            this.mockInventoryProductRepository.Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>())).ReturnsAsync(0);
            this.mockInventoryRepoMock.Setup(a => a.GetLatestBlockchainInventoryProductAsync(It.IsAny<string>())).ReturnsAsync(new InventoryProduct { EventType = EventType.Insert.ToString("G"), ProductVolume = 10.00M, BlockNumber = "908" });
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), "GetLatest", It.IsAny<Dictionary<string, object>>())).ReturnsAsync(contractMetadataStruct);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);

            await this.blockchainInventoryService.RegisterAsync(inventoryProductId).ConfigureAwait(false);

            this.mockInventoryProductRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockInventoryProductRepository.Verify(r => r.GetCountAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>()), Times.Once);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), "GetLatest", It.IsAny<Dictionary<string, object>>()), Times.Exactly(3));
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Exactly(2));
        }

        /// <summary>
        /// Blockchains the InventoryProduct service update should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BlockchainInventoryService_Update_Should_Register_BridgeAsync()
        {
            var inventoryProductId = 1;
            var (inventoryProduct, owners) = this.GetInventoryProduct("Update", true, Entities.Core.StatusType.PROCESSING);
            var contractMetadataStruct = new ContractMetadataStruct
            {
                ContractAddress = "ContractAddress",
                ContractAbi = "ContractAbi",
            };
            var transactionReceipt = new TransactionReceipt
            {
                TransactionHash = "TestHash",
                BlockNumber = new HexBigInteger(new BigInteger(10000)),
                GasUsed = new HexBigInteger(new BigInteger(70000)),
                CumulativeGasUsed = new HexBigInteger(new BigInteger(700000)),
            };

            this.mockInventoryProductRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(inventoryProduct);
            this.mockInventoryProductRepository.Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>())).ReturnsAsync(0);
            this.mockInventoryRepoMock.Setup(a => a.GetLatestBlockchainInventoryProductAsync(It.IsAny<string>())).ReturnsAsync(new InventoryProduct { EventType = EventType.Insert.ToString("G"), ProductVolume = 10.00M, BlockNumber = "0x908" });
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), "GetLatest", It.IsAny<Dictionary<string, object>>())).ReturnsAsync(contractMetadataStruct);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);

            await this.blockchainInventoryService.RegisterAsync(inventoryProductId).ConfigureAwait(false);

            this.mockInventoryProductRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockInventoryProductRepository.Verify(r => r.GetCountAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>()), Times.Once);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), "GetLatest", It.IsAny<Dictionary<string, object>>()), Times.Exactly(2));
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Exactly(2));
        }

        /// <summary>
        /// Blockchains the InventoryProduct service delete should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BlockchainInventoryService_Delete_Should_RegisterAsync()
        {
            var inventoryProductId = 1;
            var (inventoryProduct, owners) = this.GetInventoryProduct("Delete", true, Entities.Core.StatusType.PROCESSING);
            var contractMetadataStruct = new ContractMetadataStruct
            {
                ContractAddress = "ContractAddress",
                ContractAbi = "ContractAbi",
            };
            var transactionReceipt = new TransactionReceipt
            {
                TransactionHash = "TestHash",
                BlockNumber = new HexBigInteger(new BigInteger(10000)),
                GasUsed = new HexBigInteger(new BigInteger(70000)),
                CumulativeGasUsed = new HexBigInteger(new BigInteger(700000)),
            };

            this.mockInventoryProductRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(inventoryProduct);
            this.mockInventoryProductRepository.Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>())).ReturnsAsync(0);
            this.mockInventoryRepoMock.Setup(a => a.GetLatestBlockchainInventoryProductAsync(It.IsAny<string>())).ReturnsAsync(new InventoryProduct { EventType = EventType.Insert.ToString("G"), ProductVolume = 10.00M, BlockNumber = "908" });
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), "GetLatest", It.IsAny<Dictionary<string, object>>())).ReturnsAsync(contractMetadataStruct);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallMethodAsync<bool>(It.IsAny<string>(), It.IsAny<string>(), "HasOwner", It.IsAny<Dictionary<string, object>>())).ReturnsAsync(true);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);

            await this.blockchainInventoryService.RegisterAsync(inventoryProductId).ConfigureAwait(false);

            this.mockInventoryProductRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockInventoryProductRepository.Verify(r => r.GetCountAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>()), Times.Once);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), "GetLatest", It.IsAny<Dictionary<string, object>>()), Times.Exactly(3));
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallMethodAsync<bool>(It.IsAny<string>(), It.IsAny<string>(), "HasOwner", It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Exactly(2));
        }

        /// <summary>
        /// Blockchains the InventoryProduct service delete should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BlockchainInventoryService_Delete_Should_Register_BridgeAsync()
        {
            var inventoryProductId = 1;
            var (inventoryProduct, owners) = this.GetInventoryProduct("Delete", true, Entities.Core.StatusType.PROCESSING);
            var contractMetadataStruct = new ContractMetadataStruct
            {
                ContractAddress = "ContractAddress",
                ContractAbi = "ContractAbi",
            };
            var transactionReceipt = new TransactionReceipt
            {
                TransactionHash = "TestHash",
                BlockNumber = new HexBigInteger(new BigInteger(10000)),
                GasUsed = new HexBigInteger(new BigInteger(70000)),
                CumulativeGasUsed = new HexBigInteger(new BigInteger(700000)),
            };

            this.mockInventoryProductRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(inventoryProduct);
            this.mockInventoryProductRepository.Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>())).ReturnsAsync(0);
            this.mockInventoryRepoMock.Setup(a => a.GetLatestBlockchainInventoryProductAsync(It.IsAny<string>())).ReturnsAsync(new InventoryProduct { EventType = EventType.Insert.ToString("G"), ProductVolume = 10.00M, BlockNumber = "908" });
            this.mockInventoryRepoMock.Setup(a => a.GetLatestBlockchainInventoryProductAsync(It.IsAny<string>())).ReturnsAsync(new InventoryProduct { EventType = EventType.Insert.ToString("G"), ProductVolume = 10.00M, BlockNumber = "0x908" });
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), "GetLatest", It.IsAny<Dictionary<string, object>>())).ReturnsAsync(contractMetadataStruct);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallMethodAsync<bool>(It.IsAny<string>(), It.IsAny<string>(), "HasOwner", It.IsAny<Dictionary<string, object>>())).ReturnsAsync(true);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);

            await this.blockchainInventoryService.RegisterAsync(inventoryProductId).ConfigureAwait(false);

            this.mockInventoryProductRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockInventoryProductRepository.Verify(r => r.GetCountAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>()), Times.Once);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), "GetLatest", It.IsAny<Dictionary<string, object>>()), Times.Exactly(2));
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Exactly(2));
        }

        /// <summary>
        /// Blockchains the InventoryProduct service if no InventoryProduct exits should not register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BlockchainInventoryService_If_No_InventoryProduct_Exits_Should_NotRegisterAsync()
        {
            var inventoryProductId = 1;
            var contractMetadataStruct = new ContractMetadataStruct
            {
                ContractAddress = "Address",
            };
            var transactionReceipt = new TransactionReceipt
            {
                TransactionHash = "TestHash",
                BlockNumber = new HexBigInteger(new BigInteger(10000)),
                GasUsed = new HexBigInteger(new BigInteger(70000)),
                CumulativeGasUsed = new HexBigInteger(new BigInteger(700000)),
            };

            this.mockInventoryProductRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(default(InventoryProduct));
            this.mockInventoryProductRepository.Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>())).ReturnsAsync(0);
            this.mockInventoryRepoMock.Setup(a => a.GetLatestBlockchainInventoryProductAsync(It.IsAny<string>())).ReturnsAsync((InventoryProduct)null);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), "GetLatest", It.IsAny<Dictionary<string, object>>())).ReturnsAsync(contractMetadataStruct);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);

            await this.blockchainInventoryService.RegisterAsync(inventoryProductId).ConfigureAwait(false);

            this.mockInventoryProductRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockInventoryProductRepository.Verify(r => r.GetCountAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>()), Times.Never);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), "GetLatest", It.IsAny<Dictionary<string, object>>()), Times.Never);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Never);
        }

        /// <summary>
        /// Blockchains the movement service if movement is already should not register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BlockchainMovementService_ShouldNotRegister_WhenEventTypeIsUnknownAsync()
        {
            var movementTransactionId = 1;
            var (inventoryProduct, owners) = this.GetInventoryProduct("Unknown", true, Entities.Core.StatusType.PROCESSING);
            var contractMetadataStruct = new ContractMetadataStruct
            {
                ContractAddress = "Address",
            };
            var transactionReceipt = new TransactionReceipt
            {
                TransactionHash = "TestHash",
                BlockNumber = new HexBigInteger(new BigInteger(10000)),
                GasUsed = new HexBigInteger(new BigInteger(70000)),
                CumulativeGasUsed = new HexBigInteger(new BigInteger(700000)),
            };

            this.mockInventoryProductRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(inventoryProduct);
            this.mockInventoryProductRepository.Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>())).ReturnsAsync(0);
            this.mockInventoryRepoMock.Setup(a => a.GetLatestBlockchainInventoryProductAsync(It.IsAny<string>())).ReturnsAsync((InventoryProduct)null);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), "GetLatest", It.IsAny<Dictionary<string, object>>())).ReturnsAsync(contractMetadataStruct);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);

            await this.blockchainInventoryService.RegisterAsync(movementTransactionId).ConfigureAwait(false);

            this.mockInventoryProductRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockInventoryProductRepository.Verify(r => r.GetCountAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>()), Times.Never);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), "GetLatest", It.IsAny<Dictionary<string, object>>()), Times.Never);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Never);
        }

        /// <summary>
        /// Blockchains the InventoryProduct service if number of unregistered InventoryProducts is not zero should not register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BlockchainInventoryService_If_NumOfUnregisteredInventoryProducts_Is_Not_Zero_Should_NotRegisterAsync()
        {
            var inventoryProductId = 1;
            var (inventoryProduct, owners) = this.GetInventoryProduct("Update", true, Entities.Core.StatusType.PROCESSING);
            var contractMetadataStruct = new ContractMetadataStruct
            {
                ContractAddress = "Address",
            };
            var transactionReceipt = new TransactionReceipt
            {
                TransactionHash = "TestHash",
                BlockNumber = new HexBigInteger(new BigInteger(10000)),
                GasUsed = new HexBigInteger(new BigInteger(70000)),
                CumulativeGasUsed = new HexBigInteger(new BigInteger(700000)),
            };

            this.mockInventoryProductRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(inventoryProduct);
            this.mockInventoryProductRepository.Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>())).ReturnsAsync(1);
            this.mockInventoryRepoMock.Setup(a => a.GetLatestBlockchainInventoryProductAsync(It.IsAny<string>())).ReturnsAsync((InventoryProduct)null);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), "GetLatest", It.IsAny<Dictionary<string, object>>())).ReturnsAsync(contractMetadataStruct);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);

            await this.blockchainInventoryService.RegisterAsync(inventoryProductId).ConfigureAwait(false);

            this.mockInventoryProductRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockInventoryProductRepository.Verify(r => r.GetCountAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>()), Times.Once);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), "GetLatest", It.IsAny<Dictionary<string, object>>()), Times.Never);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Never);
        }

        ///// <summary>
        ///// Blockchains the node service should register node asynchronous.
        ///// </summary>
        ///// <returns>The task.</returns>
        [TestMethod]
        public async Task BlockchainService_ShouldRegisterCriticalEvent_WhenSendToQueueFailsAsync()
        {
            var entityId = 1;
            var (inventoryProduct, owners) = this.GetInventoryProduct("Update", true, Entities.Core.StatusType.PROCESSING);
            var contractMetadataStruct = new ContractMetadataStruct
            {
                ContractAddress = "ContractAddress",
                ContractAbi = "ContractAbi",
            };
            var transactionReceipt = new TransactionReceipt
            {
                TransactionHash = "TestHash",
                BlockNumber = new HexBigInteger(new BigInteger(10000)),
                GasUsed = new HexBigInteger(new BigInteger(70000)),
                CumulativeGasUsed = new HexBigInteger(new BigInteger(700000)),
            };

            this.mockInventoryProductRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(inventoryProduct);
            this.mockInventoryProductRepository.Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>())).ReturnsAsync(0);
            this.mockInventoryRepoMock.Setup(a => a.GetLatestBlockchainInventoryProductAsync(It.IsAny<string>())).ReturnsAsync((InventoryProduct)null);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(contractMetadataStruct);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);
            this.mockQueueClient.Setup(m => m.QueueMessageAsync(It.IsAny<OffchainMessage>())).ThrowsAsync(new Exception());

            await this.blockchainInventoryService.RegisterAsync(entityId).ConfigureAwait(false);

            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Exactly(2));
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Exactly(3));

            this.mockInventoryProductRepository.VerifyAll();
            this.mockUnitOfWorkFactory.VerifyAll();
            this.mockUnitOfWork.VerifyAll();
            this.mockAzureclientFactory.VerifyAll();
            this.mockQueueClient.VerifyAll();
            this.mockTelemetry.VerifyAll();
        }

        /// <summary>
        /// Blockchains the service should queue with block status failed when transaction hash is null asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BlockchainService_ShouldQueueWithBlockStatusFailed_WhenTransactionHashIsNullAsync()
        {
            var entityId = 1;
            var (inventoryProduct, owners) = this.GetInventoryProduct("Update", true, Entities.Core.StatusType.PROCESSING);
            var contractMetadataStruct = new ContractMetadataStruct
            {
                ContractAddress = "ContractAddress",
                ContractAbi = "ContractAbi",
            };
            var transactionReceipt = new TransactionReceipt
            {
                BlockNumber = new HexBigInteger(new BigInteger(10000)),
                GasUsed = new HexBigInteger(new BigInteger(70000)),
                CumulativeGasUsed = new HexBigInteger(new BigInteger(700000)),
            };

            this.mockInventoryProductRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(inventoryProduct);
            this.mockInventoryProductRepository.Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>())).ReturnsAsync(0);
            this.mockInventoryRepoMock.Setup(a => a.GetLatestBlockchainInventoryProductAsync(It.IsAny<string>())).ReturnsAsync((InventoryProduct)null);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(contractMetadataStruct);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);
            this.mockQueueClient.Setup(m => m.QueueMessageAsync(new OffchainMessage { Status = StatusType.FAILED }));

            await this.blockchainInventoryService.RegisterAsync(entityId).ConfigureAwait(false);

            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Exactly(2));
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Exactly(3));

            this.mockInventoryProductRepository.VerifyAll();
            this.mockUnitOfWorkFactory.VerifyAll();
            this.mockUnitOfWork.VerifyAll();
            this.mockAzureclientFactory.Verify(m => m.GetQueueClient(QueueConstants.OffchainQueue), Times.Exactly(2));
            this.mockQueueClient.Verify(q => q.QueueMessageAsync(It.IsAny<OffchainMessage>()), Times.Exactly(2));
        }

        /// <summary>
        /// Blockchains the service should queue with block status failed when block number is null asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BlockchainService_ShouldQueueWithBlockStatusFailed_WhenBlockNumberIsNullAsync()
        {
            var entityId = 1;
            var (inventoryProduct, owners) = this.GetInventoryProduct("Update", true, Entities.Core.StatusType.PROCESSING);
            var contractMetadataStruct = new ContractMetadataStruct
            {
                ContractAddress = "ContractAddress",
                ContractAbi = "ContractAbi",
            };
            var transactionReceipt = new TransactionReceipt
            {
                TransactionHash = "Hash",
                BlockNumber = new HexBigInteger(null),
                GasUsed = new HexBigInteger(new BigInteger(70000)),
                CumulativeGasUsed = new HexBigInteger(new BigInteger(700000)),
            };

            this.mockInventoryProductRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(inventoryProduct);
            this.mockInventoryProductRepository.Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>())).ReturnsAsync(0);
            this.mockInventoryRepoMock.Setup(a => a.GetLatestBlockchainInventoryProductAsync(It.IsAny<string>())).ReturnsAsync((InventoryProduct)null);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(contractMetadataStruct);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);
            this.mockQueueClient.Setup(m => m.QueueMessageAsync(new OffchainMessage { Status = StatusType.FAILED }));

            await this.blockchainInventoryService.RegisterAsync(entityId).ConfigureAwait(false);

            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Exactly(2));
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Exactly(3));

            this.mockInventoryProductRepository.VerifyAll();
            this.mockUnitOfWorkFactory.VerifyAll();
            this.mockUnitOfWork.VerifyAll();
            this.mockAzureclientFactory.Verify(m => m.GetQueueClient(QueueConstants.OffchainQueue), Times.Exactly(2));
            this.mockQueueClient.Verify(q => q.QueueMessageAsync(It.IsAny<OffchainMessage>()), Times.Exactly(2));
        }

        /// <summary>
        /// Gets the InventoryProduct.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="hasPreviousBlockchainTransactionId">if set to <c>true</c> [has previous blockchain transaction identifier].</param>
        /// <param name="blockchainStatus">if set to <c>true</c> [blockchain status].</param>
        /// <returns>The InventoryProduct.</returns>
        private (InventoryProduct, IEnumerable<Owner>) GetInventoryProduct(string eventType, bool hasPreviousBlockchainTransactionId, StatusType blockchainStatus)
        {
            var inventoryProduct = new InventoryProduct
            {
                InventoryProductUniqueId = "1",
                InventoryProductId = 1,
                BlockchainStatus = blockchainStatus,
                Product = new Product { ProductId = "1", Name = "Crudo caño limón (CCL)" },
                ProductVolume = Convert.ToDecimal(-1),
                ProductType = 1,
                InventoryDate = new DateTime(2019, 9, 2, 8, 10, 0),
                FileRegistrationTransaction = new FileRegistrationTransaction
                {
                    FileRegistrationId = 1,
                },
                EventType = eventType,
            };

            inventoryProduct.Owners.Add(new Owner
            {
                OwnerId = 1,
                OwnershipValue = 1,
                OwnershipValueUnit = "BBL",
            });

            var owners = new List<Owner>
            {
                new Owner
                {
                    OwnerId = 1,
                    OwnershipValue = 1,
                    OwnershipValueUnit = "BBL",
                },
            };

            if (hasPreviousBlockchainTransactionId)
            {
                inventoryProduct.PreviousBlockchainInventoryProductTransactionId = Guid.NewGuid();
            }

            return (inventoryProduct, owners);
        }
    }
}