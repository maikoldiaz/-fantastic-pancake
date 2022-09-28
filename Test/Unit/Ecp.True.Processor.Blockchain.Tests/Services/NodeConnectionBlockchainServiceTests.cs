// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeConnectionBlockchainServiceTests.cs" company="Microsoft">
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
    /// The BlockchainNodeConnectionServiceTests.
    /// </summary>
    [TestClass]
    public class NodeConnectionBlockchainServiceTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<NodeConnectionBlockchainService>> mockLogger;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock telemetry.
        /// </summary>
        private Mock<ITelemetry> mockTelemetry;

        /// <summary>
        /// The mock queue client.
        /// </summary>
        private Mock<IServiceBusQueueClient> mockQueueClient;

        /// <summary>
        /// The node repository.
        /// </summary>
        private Mock<IRepository<OffchainNodeConnection>> repositoryMock;

        /// <summary>
        /// The node.
        /// </summary>
        private OffchainNodeConnection connection;

        /// <summary>
        /// The azureclient factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureclientFactory;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The Blockchain Node Service.
        /// </summary>
        private NodeConnectionBlockchainService blockchainService;

        /// <summary>
        /// Intilizes the method.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<NodeConnectionBlockchainService>>();
            this.mockAzureclientFactory = new Mock<IAzureClientFactory>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockTelemetry = new Mock<ITelemetry>();
            this.mockQueueClient = new Mock<IServiceBusQueueClient>();

            this.mockTelemetry.Setup(t => t.TrackEvent(Constants.Critical, EventName.OffchainSyncFailed.ToString("G"), It.IsAny<IDictionary<string, string>>(), null, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()));

            this.mockAzureclientFactory.Setup(a => a.GetQueueClient(QueueConstants.OffchainQueue)).Returns(this.mockQueueClient.Object);
            this.mockQueueClient.Setup(m => m.QueueMessageAsync(It.IsAny<OffchainMessage>()));

            this.mockUnitOfWorkFactory.Setup(s => s.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);

            this.repositoryMock = new Mock<IRepository<OffchainNodeConnection>>();

            this.connection = new OffchainNodeConnection
            {
                NodeConnectionId = 1,
                SourceNodeId = 1,
                DestinationNodeId = 2,
                IsActive = true,
                Id = 1,
            };

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<OffchainNodeConnection>()).Returns(this.repositoryMock.Object);

            this.blockchainService = new NodeConnectionBlockchainService(this.mockLogger.Object, this.mockAzureclientFactory.Object, this.mockUnitOfWorkFactory.Object, this.mockTelemetry.Object);
        }

        ///// <summary>
        ///// Blockchains the node service should register node asynchronous.
        ///// </summary>
        ///// <returns>The task.</returns>
        [TestMethod]
        public async Task NodeConnectionBlockchainService_ShouldRegisterNodeAsync()
        {
            this.repositoryMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<OffchainNodeConnection, bool>>>())).ReturnsAsync(this.connection);

            var transactionReceipt = new TransactionReceipt
            {
                TransactionHash = "TestHash",
                BlockNumber = new HexBigInteger(new BigInteger(10000)),
                GasUsed = new HexBigInteger(new BigInteger(70000)),
                CumulativeGasUsed = new HexBigInteger(new BigInteger(700000)),
            };

            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new ContractMetadataStruct());
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);

            await this.blockchainService.RegisterAsync(1).ConfigureAwait(false);

            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.AtLeastOnce);

            this.repositoryMock.VerifyAll();
            this.mockUnitOfWorkFactory.VerifyAll();
            this.mockUnitOfWork.VerifyAll();
            this.mockAzureclientFactory.VerifyAll();
            this.mockQueueClient.VerifyAll();
        }

        ///// <summary>
        ///// Blockchains the node service should register node asynchronous.
        ///// </summary>
        ///// <returns>The task.</returns>
        [TestMethod]
        public async Task NodeConnectionBlockchainService_ShouldRegisterCriticalEvent_WhenSendToQueueFailsAsync()
        {
            this.repositoryMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<OffchainNodeConnection, bool>>>())).ReturnsAsync(this.connection);
            var entityId = 1;

            var transactionReceipt = new TransactionReceipt
            {
                TransactionHash = "TestHash",
                BlockNumber = new HexBigInteger(new BigInteger(10000)),
                GasUsed = new HexBigInteger(new BigInteger(70000)),
                CumulativeGasUsed = new HexBigInteger(new BigInteger(700000)),
            };

            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new ContractMetadataStruct());
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);
            this.mockQueueClient.Setup(m => m.QueueMessageAsync(It.IsAny<OffchainMessage>())).ThrowsAsync(new Exception());

            await this.blockchainService.RegisterAsync(entityId).ConfigureAwait(false);

            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.AtLeastOnce);

            this.repositoryMock.VerifyAll();
            this.mockUnitOfWorkFactory.VerifyAll();
            this.mockUnitOfWork.VerifyAll();
            this.mockAzureclientFactory.VerifyAll();
            this.mockQueueClient.VerifyAll();
            this.mockTelemetry.VerifyAll();
        }

        /// <summary>
        /// Nodes the connection blockchain service should queue with block status failed when transaction hash is null asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task NodeConnectionBlockchainService_ShouldQueueWithBlockStatusFailed_WhenTransactionHashIsNullAsync()
        {
            this.repositoryMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<OffchainNodeConnection, bool>>>())).ReturnsAsync(this.connection);
            var entityId = 1;

            var transactionReceipt = new TransactionReceipt
            {
                BlockNumber = new HexBigInteger(new BigInteger(10000)),
                GasUsed = new HexBigInteger(new BigInteger(70000)),
                CumulativeGasUsed = new HexBigInteger(new BigInteger(700000)),
            };

            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new ContractMetadataStruct());
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);
            this.mockQueueClient.Setup(m => m.QueueMessageAsync(new OffchainMessage { Status = StatusType.FAILED }));

            await this.blockchainService.RegisterAsync(entityId).ConfigureAwait(false);

            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.AtLeastOnce);

            this.repositoryMock.VerifyAll();
            this.mockUnitOfWorkFactory.VerifyAll();
            this.mockUnitOfWork.VerifyAll();
            this.mockAzureclientFactory.Verify(m => m.GetQueueClient(QueueConstants.OffchainQueue), Times.Exactly(1));
            this.mockQueueClient.Verify(q => q.QueueMessageAsync(It.IsAny<OffchainMessage>()), Times.Exactly(1));
        }

        /// <summary>
        /// Nodes the connection blockchain service should queue with block status failed when block number is null asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task NodeConnectionBlockchainService_ShouldQueueWithBlockStatusFailed_WhenBlockNumberIsNullAsync()
        {
            this.repositoryMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<OffchainNodeConnection, bool>>>())).ReturnsAsync(this.connection);
            var entityId = 1;

            var transactionReceipt = new TransactionReceipt
            {
                TransactionHash = "Hash",
                BlockNumber = new HexBigInteger(null),
                GasUsed = new HexBigInteger(new BigInteger(70000)),
                CumulativeGasUsed = new HexBigInteger(new BigInteger(700000)),
            };

            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new ContractMetadataStruct());
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);
            this.mockQueueClient.Setup(m => m.QueueMessageAsync(new OffchainMessage { Status = StatusType.FAILED }));

            await this.blockchainService.RegisterAsync(entityId).ConfigureAwait(false);

            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.AtLeastOnce);

            this.repositoryMock.VerifyAll();
            this.mockUnitOfWorkFactory.VerifyAll();
            this.mockUnitOfWork.VerifyAll();
            this.mockAzureclientFactory.Verify(m => m.GetQueueClient(QueueConstants.OffchainQueue), Times.Exactly(1));
            this.mockQueueClient.Verify(q => q.QueueMessageAsync(It.IsAny<OffchainMessage>()), Times.Exactly(1));
        }

        ///// <summary>
        ///// Blockchains the node service should register node asynchronous.
        ///// </summary>
        ///// <returns>The task.</returns>
        [TestMethod]
        public async Task NodeConnectionBlockchainService_ShouldNotRegisterNodeConnection_WhenNodeConnectionIsNotFoundAsync()
        {
            this.repositoryMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<OffchainNodeConnection, bool>>>())).Returns(Task.FromResult<OffchainNodeConnection>(null));
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new ContractMetadataStruct());
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(It.IsAny<TransactionReceipt>());

            await this.blockchainService.RegisterAsync(1).ConfigureAwait(false);

            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Never);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Never);

            this.repositoryMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<OffchainNodeConnection, bool>>>()), Times.Once);
            this.repositoryMock.Verify(r => r.Update(It.IsAny<OffchainNodeConnection>()), Times.Never);
        }
    }
}
