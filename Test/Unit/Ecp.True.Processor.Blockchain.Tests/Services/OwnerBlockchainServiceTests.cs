// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnerBlockchainServiceTests.cs" company="Microsoft">
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

    [TestClass]
    public class OwnerBlockchainServiceTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<OwnerBlockchainService>> mockLogger;

        /// <summary>
        /// The azureclient factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureclientFactory;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The mock owner repository.
        /// </summary>
        private Mock<IRepository<Owner>> mockOwnerRepository;

        /// <summary>
        /// The mock movement repository.
        /// </summary>
        private Mock<IRepository<Movement>> mockMovementRepository;

        /// <summary>
        /// The mock inventory product repository.
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
        /// The owner blockchain service.
        /// </summary>
        private OwnerBlockchainService ownerBlockchainService;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<OwnerBlockchainService>>();
            this.mockAzureclientFactory = new Mock<IAzureClientFactory>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockTelemetry = new Mock<ITelemetry>();
            this.mockQueueClient = new Mock<IServiceBusQueueClient>();
            this.mockMovementRepository = new Mock<IRepository<Movement>>();
            this.mockInventoryProductRepository = new Mock<IRepository<InventoryProduct>>();

            this.mockTelemetry.Setup(t => t.TrackEvent(Constants.Critical, EventName.OffchainSyncFailed.ToString("G"), It.IsAny<IDictionary<string, string>>(), null, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()));

            this.mockAzureclientFactory.Setup(a => a.GetQueueClient(QueueConstants.OffchainQueue)).Returns(this.mockQueueClient.Object);
            this.mockQueueClient.Setup(m => m.QueueMessageAsync(It.IsAny<OffchainMessage>()));

            this.mockUnitOfWorkFactory.Setup(s => s.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockOwnerRepository = new Mock<IRepository<Owner>>();
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Owner>()).Returns(this.mockOwnerRepository.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<InventoryProduct>()).Returns(this.mockInventoryProductRepository.Object);

            this.ownerBlockchainService = new OwnerBlockchainService(
               this.mockLogger.Object,
               this.mockAzureclientFactory.Object,
               this.mockUnitOfWorkFactory.Object,
               this.mockTelemetry.Object);
        }

        /// <summary>
        /// Owners the blockchain service should register movement owner asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OwnerBlockchainService_Should_RegisterMovementOwnerAsync()
        {
            var inventoryProductId = 1;
            var (inventoryProduct, movement, owner) = this.GetOwner("Insert", 123, null);
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

            this.mockOwnerRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Owner, bool>>>())).ReturnsAsync(owner);
            this.mockMovementRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Movement, bool>>>())).ReturnsAsync(movement);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), "GetLatest", It.IsAny<Dictionary<string, object>>())).ReturnsAsync(contractMetadataStruct);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);

            await this.ownerBlockchainService.RegisterAsync(inventoryProductId).ConfigureAwait(false);

            this.mockOwnerRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Owner, bool>>>()), Times.Once);
            this.mockMovementRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Movement, bool>>>()), Times.Once);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), "GetLatest", It.IsAny<Dictionary<string, object>>()), Times.Exactly(2));
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Exactly(1));
        }

        /// <summary>
        /// Owners the blockchain service should register inventory product owner asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OwnerBlockchainService_Should_RegisterInventoryProductOwnerAsync()
        {
            var inventoryProductId = 1;
            var (inventoryProduct, movement, owner) = this.GetOwner("Insert", null, 123);
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

            this.mockOwnerRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Owner, bool>>>())).ReturnsAsync(owner);
            this.mockInventoryProductRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>())).ReturnsAsync(inventoryProduct);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), "GetLatest", It.IsAny<Dictionary<string, object>>())).ReturnsAsync(contractMetadataStruct);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);

            await this.ownerBlockchainService.RegisterAsync(inventoryProductId).ConfigureAwait(false);

            this.mockOwnerRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Owner, bool>>>()), Times.Once);
            this.mockInventoryProductRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>()), Times.Once);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), "GetLatest", It.IsAny<Dictionary<string, object>>()), Times.Exactly(2));
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Exactly(1));
        }

        /// <summary>
        /// Owners the blockchain service should not register if owner does not exists asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OwnerBlockchainService_Should_Not_Register_If_Owner_Does_Not_Exists_Async()
        {
            var inventoryProductId = 1;
            var (inventoryProduct, movement, owner) = this.GetOwner("Insert", null, 123);
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

            this.mockOwnerRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Owner, bool>>>())).ReturnsAsync(() => null);
            this.mockInventoryProductRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>())).ReturnsAsync(inventoryProduct);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), "GetLatest", It.IsAny<Dictionary<string, object>>())).ReturnsAsync(contractMetadataStruct);
            this.mockAzureclientFactory.Setup(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(transactionReceipt);

            await this.ownerBlockchainService.RegisterAsync(inventoryProductId).ConfigureAwait(false);

            this.mockOwnerRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Owner, bool>>>()), Times.Once);
            this.mockInventoryProductRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>()), Times.Never);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(It.IsAny<string>(), It.IsAny<string>(), "GetLatest", It.IsAny<Dictionary<string, object>>()), Times.Never);
            this.mockAzureclientFactory.Verify(x => x.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Never);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnerBlockchainServiceTests" /> class.
        /// </summary>
        [TestMethod]
        public void OwnerBlockchainService_ShouldReturnType()
        {
            Assert.AreEqual(ServiceType.Owner, this.ownerBlockchainService.Type);
        }

        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="movementTransactionId">The movement transaction identifier.</param>
        /// <param name="inventoryProductId">The inventory product identifier.</param>
        /// <returns>InventoryProduct, Movement and Owner stub.</returns>
        private (InventoryProduct, Movement, Owner) GetOwner(string eventType, int? movementTransactionId, int? inventoryProductId)
        {
            var owners = new Owner
            {
                OwnerId = 1,
                OwnershipValue = 1,
                OwnershipValueUnit = "BBL",
                MovementTransactionId = movementTransactionId,
                InventoryProductId = inventoryProductId,
            };

            var inventoryProduct = new InventoryProduct
            {
                InventoryProductUniqueId = "1",
                InventoryProductId = 1,
                BlockchainStatus = Entities.Core.StatusType.PROCESSED,
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

            var movement = new Movement
            {
                MovementId = "1",
                MovementTransactionId = 1,
                BlockchainStatus = Entities.Core.StatusType.PROCESSED,
                MovementSource = new MovementSource
                {
                    SourceNodeId = 300,
                    SourceNode = new Node { NodeId = 300, Name = "Rebombeos de caño limon-ayacucho GCX" },
                    SourceProductId = "1",
                    SourceProduct = new Product { ProductId = "1", Name = "Crudo caño limón (CCL)" },
                },
                MovementDestination = new MovementDestination
                {
                    DestinationNodeId = 100,
                    DestinationNode = new Node { NodeId = 100, Name = "AYACUCHO CRD-GALAN 14" },
                    DestinationProductId = "1",
                    DestinationProduct = new Product { ProductId = "1", Name = "Crudo caño limón (CCL)" },
                },
                NetStandardVolume = Convert.ToDecimal(-1),
                MovementTypeId = 123,
                OperationalDate = new DateTime(2019, 9, 2, 8, 10, 0),
                FileRegistrationTransaction = new FileRegistrationTransaction
                {
                    FileRegistrationId = 1,
                },
                EventType = eventType,
            };

            return (inventoryProduct, movement, owners);
        }
    }
}
