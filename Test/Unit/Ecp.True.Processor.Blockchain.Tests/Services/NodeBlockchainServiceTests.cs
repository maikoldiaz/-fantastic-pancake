// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeBlockchainServiceTests.cs" company="Microsoft">
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
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Blockchain.Services;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Azure.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Nethereum.Hex.HexTypes;
    using Nethereum.RPC.Eth.DTOs;

    [TestClass]
    public class NodeBlockchainServiceTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<NodeBlockchainService>> mockLogger;

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
        private Mock<IRepository<OffchainNode>> nodeRepositoryMock;

        /// <summary>
        /// The node.
        /// </summary>
        private OffchainNode node;

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
        private NodeBlockchainService blockchainNodeService;

        /// <summary>
        /// Intilizes the method.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<NodeBlockchainService>>();
            this.mockAzureclientFactory = new Mock<IAzureClientFactory>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockTelemetry = new Mock<ITelemetry>();
            this.mockQueueClient = new Mock<IServiceBusQueueClient>();

            this.mockTelemetry.Setup(t => t.TrackEvent(Constants.Critical, EventName.OffchainSyncFailed.ToString("G"), It.IsAny<IDictionary<string, string>>(), null, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()));

            this.mockAzureclientFactory.Setup(a => a.GetQueueClient(QueueConstants.OffchainQueue)).Returns(this.mockQueueClient.Object);
            this.mockQueueClient.Setup(m => m.QueueMessageAsync(It.IsAny<OffchainMessage>()));
            this.mockUnitOfWorkFactory.Setup(s => s.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);

            this.nodeRepositoryMock = new Mock<IRepository<OffchainNode>>();

            this.node = new OffchainNode
            {
                NodeId = 1,
                Name = "Node One",
                NodeStateTypeId = (int)NodeState.CreatedNode,
                IsActive = true,
                Id = 1,
                LastUpdateDate = DateTime.UtcNow,
            };

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<OffchainNode>()).Returns(this.nodeRepositoryMock.Object);

            this.blockchainNodeService = new NodeBlockchainService(this.mockLogger.Object, this.mockAzureclientFactory.Object, this.mockUnitOfWorkFactory.Object, this.mockTelemetry.Object);
        }

        ///// <summary>
        ///// Blockchains the node service should register node asynchronous.
        ///// </summary>
        ///// <returns>The task.</returns>
        [TestMethod]
        public async Task NodeBlockchainService_ShouldRegisterOffchainNode_WhenInvokedAsync()
        {
            this.nodeRepositoryMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<OffchainNode, bool>>>())).ReturnsAsync(this.node);
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

            await this.blockchainNodeService.RegisterAsync(entityId).ConfigureAwait(false);

            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.AtLeastOnce);

            this.nodeRepositoryMock.VerifyAll();
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
        public async Task NodeBlockchainService_ShouldRegisterCriticalEvent_WhenSendToQueueFailsAsync()
        {
            this.nodeRepositoryMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<OffchainNode, bool>>>())).ReturnsAsync(this.node);
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

            await this.blockchainNodeService.RegisterAsync(entityId).ConfigureAwait(false);

            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.AtLeastOnce);

            this.nodeRepositoryMock.VerifyAll();
            this.mockUnitOfWorkFactory.VerifyAll();
            this.mockUnitOfWork.VerifyAll();
            this.mockAzureclientFactory.VerifyAll();
            this.mockQueueClient.VerifyAll();
            this.mockTelemetry.VerifyAll();
        }

        /// <summary>
        /// Nodes the blockchain service should queue with block status failed when transaction hash is null asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task NodeBlockchainService_ShouldQueueWithBlockStatusFailed_WhenTransactionHashIsNullAsync()
        {
            this.nodeRepositoryMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<OffchainNode, bool>>>())).ReturnsAsync(this.node);
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

            await this.blockchainNodeService.RegisterAsync(entityId).ConfigureAwait(false);

            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.AtLeastOnce);

            this.nodeRepositoryMock.VerifyAll();
            this.mockUnitOfWorkFactory.VerifyAll();
            this.mockUnitOfWork.VerifyAll();
            this.mockAzureclientFactory.Verify(m => m.GetQueueClient(QueueConstants.OffchainQueue), Times.Exactly(1));
            this.mockQueueClient.Verify(q => q.QueueMessageAsync(It.IsAny<OffchainMessage>()), Times.Exactly(1));
        }

        /// <summary>
        /// Nodes the blockchain service should queue with block status failed when block number is null asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task NodeBlockchainService_ShouldQueueWithBlockStatusFailed_WhenBlockNumberIsNullAsync()
        {
            this.nodeRepositoryMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<OffchainNode, bool>>>())).ReturnsAsync(this.node);
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

            await this.blockchainNodeService.RegisterAsync(entityId).ConfigureAwait(false);

            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.AtLeastOnce);

            this.nodeRepositoryMock.VerifyAll();
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
        public async Task NodeBlockchainService_ShouldNotRegisterNode_WhenNodeIsNotFoundAsync()
        {
            this.nodeRepositoryMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<OffchainNode, bool>>>())).ReturnsAsync(default(OffchainNode));
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new ContractMetadataStruct());
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(It.IsAny<TransactionReceipt>());

            await this.blockchainNodeService.RegisterAsync(1).ConfigureAwait(false);

            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Never);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Never);

            this.nodeRepositoryMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<OffchainNode, bool>>>()), Times.Once);
            this.nodeRepositoryMock.Verify(r => r.Update(It.IsAny<OffchainNode>()), Times.Never);
        }
    }
}
