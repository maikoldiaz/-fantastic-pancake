// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipBlockchainServiceTests.cs" company="Microsoft">
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
    using Ecp.True.Logging;
    using Ecp.True.Processors.Blockchain.Services;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Azure.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Nethereum.Hex.HexTypes;
    using Nethereum.RPC.Eth.DTOs;

    /// <summary>
    /// The BlockchainOwnershipServiceTests.
    /// </summary>
    [TestClass]
    public class OwnershipBlockchainServiceTests
    {
        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<OwnershipBlockchainService>> mockLogger;

        /// <summary>
        /// The mock telemetry.
        /// </summary>
        private Mock<ITelemetry> mockTelemetry;

        /// <summary>
        /// The mock queue client.
        /// </summary>
        private Mock<IServiceBusQueueClient> mockQueueClient;

        /// <summary>
        /// The azureclient factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureclientFactory;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The blockchain ownership service.
        /// </summary>
        private OwnershipBlockchainService blockchainOwnershipService;

        /// <summary>
        /// The ownership.
        /// </summary>
        private Ownership ownership;

        /// <summary>
        /// The ownership repository.
        /// </summary>
        private Mock<IRepository<Ownership>> ownershipRepositoryMock;

        /// <summary>
        /// Intilizes the method.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockAzureclientFactory = new Mock<IAzureClientFactory>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();

            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockLogger = new Mock<ITrueLogger<OwnershipBlockchainService>>();
            this.mockTelemetry = new Mock<ITelemetry>();
            this.mockQueueClient = new Mock<IServiceBusQueueClient>();

            this.mockTelemetry.Setup(t => t.TrackEvent(Constants.Critical, EventName.OffchainSyncFailed.ToString("G"), It.IsAny<IDictionary<string, string>>(), null, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()));

            this.mockAzureclientFactory.Setup(a => a.GetQueueClient(QueueConstants.OffchainQueue)).Returns(this.mockQueueClient.Object);
            this.mockQueueClient.Setup(m => m.QueueMessageAsync(It.IsAny<OffchainMessage>()));
            this.mockUnitOfWorkFactory.Setup(s => s.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);

            this.ownership = new Ownership
            {
                OwnerId = 1,
                OwnershipPercentage = 200,
                OwnershipVolume = 200,
                AppliedRule = "Rule",
                RuleVersion = "1",
                ExecutionDate = DateTime.UtcNow,
                EventType = "Update",
                PreviousBlockchainOwnershipId = Guid.NewGuid(),
                InventoryProduct = new InventoryProduct(),
            };

            this.ownershipRepositoryMock = new Mock<IRepository<Ownership>>();
            this.ownershipRepositoryMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.ownership);
            this.ownershipRepositoryMock.Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<Ownership, bool>>>())).ReturnsAsync(0);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Ownership>()).Returns(this.ownershipRepositoryMock.Object);

            this.blockchainOwnershipService = new OwnershipBlockchainService(this.mockLogger.Object, this.mockAzureclientFactory.Object, this.mockUnitOfWorkFactory.Object, this.mockTelemetry.Object);
        }

        /// <summary>
        /// Blockchains the ownership service tests should not register owner ship insert asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BlockchainOwnershipService_ShouldNotRegisterOwnership_WhenEventTypeIsUnknownAsync()
        {
            var entityId = 1;
            this.ownership.EventType = "Unknown";
            var transactionReceipt = new TransactionReceipt
            {
                TransactionHash = "TestHash",
                BlockNumber = new HexBigInteger(new BigInteger(10000)),
                GasUsed = new HexBigInteger(new BigInteger(70000)),
                CumulativeGasUsed = new HexBigInteger(new BigInteger(700000)),
            };
            this.ownershipRepositoryMock.Setup(r => r.OrderByAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), It.IsAny<Expression<Func<Ownership, string>>>(), It.IsAny<int?>())).ReturnsAsync(new List<Ownership>());
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new ContractMetadataStruct());
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);
            this.ownershipRepositoryMock.Setup(r => r.OrderByAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), It.IsAny<Expression<Func<Ownership, string>>>(), It.IsAny<int?>())).ReturnsAsync((List<Ownership>)null);

            await this.blockchainOwnershipService.RegisterAsync(entityId).ConfigureAwait(false);

            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Never);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Never);
            this.mockAzureclientFactory.Verify(m => m.GetQueueClient(QueueConstants.OffchainQueue), Times.Never);
            this.mockQueueClient.Verify(q => q.QueueMessageAsync(It.IsAny<OffchainMessage>()), Times.Never);
        }

        /// <summary>
        /// Blockchains the ownership service tests should not register owner ship insert asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BlockchainOwnershipService_ShouldNotRegisterOwnership_WhenEventTypeIsUnknown_BridgeAsync()
        {
            var entityId = 1;
            this.ownership.EventType = "Unknown";
            var transactionReceipt = new TransactionReceipt
            {
                TransactionHash = "TestHash",
                BlockNumber = new HexBigInteger(new BigInteger(10000)),
                GasUsed = new HexBigInteger(new BigInteger(70000)),
                CumulativeGasUsed = new HexBigInteger(new BigInteger(700000)),
            };
            this.ownershipRepositoryMock.Setup(r => r.OrderByAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), It.IsAny<Expression<Func<Ownership, string>>>(), It.IsAny<int?>())).ReturnsAsync(new List<Ownership> { new Ownership { EventType = EventType.Insert.ToString("G"), OwnershipVolume = 10.00M, BlockNumber = "0x890" } });
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new ContractMetadataStruct());
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);
            this.ownershipRepositoryMock.Setup(r => r.OrderByAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), It.IsAny<Expression<Func<Ownership, string>>>(), It.IsAny<int?>())).ReturnsAsync(new List<Ownership> { new Ownership { EventType = EventType.Insert.ToString("G"), OwnershipVolume = 10.00M, BlockNumber = "0x890" } });

            await this.blockchainOwnershipService.RegisterAsync(entityId).ConfigureAwait(false);

            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Never);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Never);
            this.mockAzureclientFactory.Verify(m => m.GetQueueClient(QueueConstants.OffchainQueue), Times.Never);
            this.mockQueueClient.Verify(q => q.QueueMessageAsync(It.IsAny<OffchainMessage>()), Times.Never);
        }

        /// <summary>
        /// Blockchains the ownership service tests should not register owner ship insert asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BlockchainOwnershipServiceTests_ShouldNotRegisterOwnerShip_WhenRecordDoesNotExistAsync()
        {
            this.ownershipRepositoryMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(default(Ownership));
            var entityId = 1;
            var transactionReceipt = new TransactionReceipt
            {
                TransactionHash = "TestHash",
                BlockNumber = new HexBigInteger(new BigInteger(10000)),
                GasUsed = new HexBigInteger(new BigInteger(70000)),
                CumulativeGasUsed = new HexBigInteger(new BigInteger(700000)),
            };
            this.ownershipRepositoryMock.Setup(r => r.OrderByAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), It.IsAny<Expression<Func<Ownership, string>>>(), It.IsAny<int?>())).ReturnsAsync(new List<Ownership>());
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new ContractMetadataStruct());
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);

            await this.blockchainOwnershipService.RegisterAsync(entityId).ConfigureAwait(false);

            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Never);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Never);
            this.mockAzureclientFactory.Verify(m => m.GetQueueClient(QueueConstants.OffchainQueue), Times.Never);
            this.mockQueueClient.Verify(q => q.QueueMessageAsync(It.IsAny<OffchainMessage>()), Times.Never);
        }

        /// <summary>
        /// Blockchains the ownership service tests should not register owner ship insert asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BlockchainOwnershipServiceTests_ShouldNotRegisterOwnerShip_WhenPreviousRecordIsPendingRegistrationAsync()
        {
            this.ownershipRepositoryMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(default(Ownership));
            this.ownershipRepositoryMock.Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<Ownership, bool>>>())).ReturnsAsync(1);
            var entityId = 1;
            var transactionReceipt = new TransactionReceipt
            {
                TransactionHash = "TestHash",
                BlockNumber = new HexBigInteger(new BigInteger(10000)),
                GasUsed = new HexBigInteger(new BigInteger(70000)),
                CumulativeGasUsed = new HexBigInteger(new BigInteger(700000)),
            };
            this.ownershipRepositoryMock.Setup(r => r.OrderByAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), It.IsAny<Expression<Func<Ownership, string>>>(), It.IsAny<int?>())).ReturnsAsync(new List<Ownership>());
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new ContractMetadataStruct());
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);

            await this.blockchainOwnershipService.RegisterAsync(entityId).ConfigureAwait(false);

            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Never);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Never);
            this.mockAzureclientFactory.Verify(m => m.GetQueueClient(QueueConstants.OffchainQueue), Times.Never);
            this.mockQueueClient.Verify(q => q.QueueMessageAsync(It.IsAny<OffchainMessage>()), Times.Never);
        }

        ///// <summary>
        ///// Blockchains the node service should register node asynchronous.
        ///// </summary>
        ///// <returns>The task.</returns>
        [TestMethod]
        public async Task BlockchainService_ShouldRegisterCriticalEvent_WhenSendToQueueFailsAsync()
        {
            var entityId = 1;

            var transactionReceipt = new TransactionReceipt
            {
                TransactionHash = "TestHash",
                BlockNumber = new HexBigInteger(new BigInteger(10000)),
                GasUsed = new HexBigInteger(new BigInteger(70000)),
                CumulativeGasUsed = new HexBigInteger(new BigInteger(700000)),
            };

            this.ownershipRepositoryMock.Setup(r => r.OrderByAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), It.IsAny<Expression<Func<Ownership, string>>>(), It.IsAny<int?>())).ReturnsAsync(new List<Ownership>());
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new ContractMetadataStruct());
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);
            this.mockQueueClient.Setup(m => m.QueueMessageAsync(It.IsAny<OffchainMessage>())).ThrowsAsync(new Exception());

            await this.blockchainOwnershipService.RegisterAsync(entityId).ConfigureAwait(false);

            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);

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

            var transactionReceipt = new TransactionReceipt
            {
                BlockNumber = new HexBigInteger(new BigInteger(10000)),
                GasUsed = new HexBigInteger(new BigInteger(70000)),
                CumulativeGasUsed = new HexBigInteger(new BigInteger(700000)),
            };

            this.ownershipRepositoryMock.Setup(r => r.OrderByAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), It.IsAny<Expression<Func<Ownership, string>>>(), It.IsAny<int?>())).ReturnsAsync(new List<Ownership>());
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new ContractMetadataStruct());
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);
            this.mockQueueClient.Setup(m => m.QueueMessageAsync(new OffchainMessage { Status = StatusType.FAILED }));

            await this.blockchainOwnershipService.RegisterAsync(entityId).ConfigureAwait(false);

            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);

            this.mockAzureclientFactory.Verify(m => m.GetQueueClient(QueueConstants.OffchainQueue), Times.Once);
            this.mockQueueClient.Verify(q => q.QueueMessageAsync(It.IsAny<OffchainMessage>()), Times.Once);
        }

        /// <summary>
        /// Blockchains the service should queue with block status failed when block number is null asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BlockchainService_ShouldQueueWithBlockStatusFailed_WhenBlockNumberIsNullAsync()
        {
            var entityId = 1;

            var transactionReceipt = new TransactionReceipt
            {
                TransactionHash = "Hash",
                BlockNumber = new HexBigInteger(null),
                GasUsed = new HexBigInteger(new BigInteger(70000)),
                CumulativeGasUsed = new HexBigInteger(new BigInteger(700000)),
            };

            this.ownershipRepositoryMock.Setup(r => r.OrderByAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), It.IsAny<Expression<Func<Ownership, string>>>(), It.IsAny<int?>())).ReturnsAsync(new List<Ownership>());
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new ContractMetadataStruct());
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);
            this.mockQueueClient.Setup(m => m.QueueMessageAsync(new OffchainMessage { Status = StatusType.FAILED }));

            await this.blockchainOwnershipService.RegisterAsync(entityId).ConfigureAwait(false);

            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);

            this.mockAzureclientFactory.Verify(m => m.GetQueueClient(QueueConstants.OffchainQueue), Times.Exactly(1));
            this.mockQueueClient.Verify(q => q.QueueMessageAsync(It.IsAny<OffchainMessage>()), Times.Exactly(1));
        }
    }
}
