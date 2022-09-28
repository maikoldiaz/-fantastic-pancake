// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnbalanceBlockchainServiceTests.cs" company="Microsoft">
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
    using Ecp.True.Logging;
    using Ecp.True.Processors.Blockchain.Services;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Azure.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Nethereum.Hex.HexTypes;
    using Nethereum.RPC.Eth.DTOs;

    [TestClass]
    public class UnbalanceBlockchainServiceTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<UnbalanceBlockchainService>> mockLogger;

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
        /// The unbalance repository.
        /// </summary>
        private Mock<IRepository<Unbalance>> unbalanceRepositoryMock;

        /// <summary>
        /// The azureclient factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureClientFactory;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The Blockchain Node Service.
        /// </summary>
        private UnbalanceBlockchainService service;

        /// <summary>
        /// Intilizes the method.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<UnbalanceBlockchainService>>();
            this.mockAzureClientFactory = new Mock<IAzureClientFactory>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockTelemetry = new Mock<ITelemetry>();
            this.mockQueueClient = new Mock<IServiceBusQueueClient>();

            this.mockTelemetry.Setup(t => t.TrackEvent(Constants.Critical, EventName.OffchainSyncFailed.ToString("G"), It.IsAny<IDictionary<string, string>>(), null, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()));

            this.mockAzureClientFactory.Setup(a => a.GetQueueClient(QueueConstants.OffchainQueue)).Returns(this.mockQueueClient.Object);
            this.mockQueueClient.Setup(m => m.QueueMessageAsync(It.IsAny<OffchainMessage>()));
            this.mockUnitOfWorkFactory.Setup(s => s.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);

            var unbalance = new Unbalance
            {
                NodeId = 1,
                ProductId = "1",
                BlockchainStatus = Entities.Core.StatusType.PROCESSING,
            };

            this.unbalanceRepositoryMock = new Mock<IRepository<Unbalance>>();
            this.unbalanceRepositoryMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Unbalance, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(unbalance);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Unbalance>()).Returns(this.unbalanceRepositoryMock.Object);

            this.service = new UnbalanceBlockchainService(this.mockLogger.Object, this.mockAzureClientFactory.Object, this.mockUnitOfWorkFactory.Object, this.mockTelemetry.Object);
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

            this.mockAzureClientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new ContractMetadataStruct());
            this.mockAzureClientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);
            this.mockQueueClient.Setup(m => m.QueueMessageAsync(It.IsAny<OffchainMessage>())).ThrowsAsync(new Exception());

            await this.service.RegisterAsync(entityId).ConfigureAwait(false);

            this.mockAzureClientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockAzureClientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.AtLeastOnce);

            this.unbalanceRepositoryMock.VerifyAll();
            this.mockUnitOfWorkFactory.VerifyAll();
            this.mockUnitOfWork.VerifyAll();
            this.mockAzureClientFactory.VerifyAll();
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

            this.mockAzureClientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new ContractMetadataStruct());
            this.mockAzureClientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);
            this.mockQueueClient.Setup(m => m.QueueMessageAsync(new OffchainMessage { Status = StatusType.FAILED }));

            await this.service.RegisterAsync(entityId).ConfigureAwait(false);

            this.mockAzureClientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockAzureClientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.AtLeastOnce);

            this.unbalanceRepositoryMock.VerifyAll();
            this.mockUnitOfWorkFactory.VerifyAll();
            this.mockUnitOfWork.VerifyAll();
            this.mockAzureClientFactory.Verify(m => m.GetQueueClient(QueueConstants.OffchainQueue), Times.Exactly(1));
            this.mockQueueClient.Verify(q => q.QueueMessageAsync(It.IsAny<OffchainMessage>()), Times.Exactly(1));
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

            this.mockAzureClientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new ContractMetadataStruct());
            this.mockAzureClientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);
            this.mockQueueClient.Setup(m => m.QueueMessageAsync(new OffchainMessage { Status = StatusType.FAILED }));

            await this.service.RegisterAsync(entityId).ConfigureAwait(false);

            this.mockAzureClientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockAzureClientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.AtLeastOnce);

            this.unbalanceRepositoryMock.VerifyAll();
            this.mockUnitOfWorkFactory.VerifyAll();
            this.mockUnitOfWork.VerifyAll();
            this.mockAzureClientFactory.Verify(m => m.GetQueueClient(QueueConstants.OffchainQueue), Times.Once);
            this.mockQueueClient.Verify(q => q.QueueMessageAsync(It.IsAny<OffchainMessage>()), Times.Once);
        }

        /// <summary>
        /// Blockchains the node product calculation service should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BlockchainNodeProductCalculationService_ShouldRegisterAsync()
        {
            var entityId = 1;

            var transactionReceipt = new TransactionReceipt
            {
                TransactionHash = "TestHash",
                BlockNumber = new HexBigInteger(new BigInteger(10000)),
                GasUsed = new HexBigInteger(new BigInteger(70000)),
                CumulativeGasUsed = new HexBigInteger(new BigInteger(700000)),
            };

            var unbalance = new Unbalance
            {
                NodeId = 1,
                ProductId = "1",
                BlockchainStatus = Entities.Core.StatusType.PROCESSING,
            };

            this.unbalanceRepositoryMock = new Mock<IRepository<Unbalance>>();
            this.unbalanceRepositoryMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Unbalance, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(unbalance);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Unbalance>()).Returns(this.unbalanceRepositoryMock.Object);

            this.mockAzureClientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new ContractMetadataStruct());
            this.mockAzureClientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);

            await this.service.RegisterAsync(entityId).ConfigureAwait(false);

            this.mockAzureClientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), "GetLatest", It.IsAny<Dictionary<string, object>>()), Times.Exactly(1));
            this.mockAzureClientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Exactly(1));
        }
    }
}
