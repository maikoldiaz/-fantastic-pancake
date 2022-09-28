// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryProductRegistrationStrategyTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Tests.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Registration;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class InventoryProductRegistrationStrategyTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private readonly Mock<ITrueLogger<InventoryProductRegistrationStrategy>> mockLogger = new Mock<ITrueLogger<InventoryProductRegistrationStrategy>>();

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private readonly Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();

        /// <summary>
        /// The mock contract repository.
        /// </summary>
        private readonly Mock<IRepository<InventoryProduct>> mockInventoryProductRepository = new Mock<IRepository<InventoryProduct>>();

        /// <summary>
        /// The mock file registration transaction repository.
        /// </summary>
        private readonly Mock<IRepository<FileRegistrationTransaction>> mockFileRegistrationTransactionRepository = new Mock<IRepository<FileRegistrationTransaction>>();

        /// <summary>
        /// The mock contract.
        /// </summary>
        private readonly Mock<InventoryProduct> mockInventoryProduct = new Mock<InventoryProduct>();

        /// <summary>
        /// The mock service bus queue client.
        /// </summary>
        private readonly Mock<IServiceBusQueueClient> mockServiceBusQueueClient = new Mock<IServiceBusQueueClient>();

        /// <summary>
        /// The mock inventory movement index repository.
        /// </summary>
        private readonly Mock<IRepository<InventoryMovementIndex>> mockInventoryMovementIndexRepository = new Mock<IRepository<InventoryMovementIndex>>();

        /// <summary>
        /// The mock owner repository.
        /// </summary>
        private readonly Mock<IRepository<Owner>> mockOwnerRepository = new Mock<IRepository<Owner>>();

        /// <summary>
        /// The mock attribute repository.
        /// </summary>
        private readonly Mock<IRepository<AttributeEntity>> mockAttributeRepository = new Mock<IRepository<AttributeEntity>>();

        /// <summary>
        /// The contract registration strategy.
        /// </summary>
        private InventoryProductRegistrationStrategy inventoryProductRegistrationStrategy;

        /// <summary>
        /// Initilizes this instance.
        /// </summary>
        [TestInitialize]
        public void Initilize()
        {
            var inventoryProduct = new InventoryProduct { ProductVolume = 1, CreatedDate = DateTime.Now };
            inventoryProduct.Owners.Add(new Owner { OwnerId = 1, OwnershipValue = 1 });
            this.mockInventoryProduct.Setup(m => m.CopyFrom(It.IsAny<IEntity>()));
            this.mockInventoryProductRepository.Setup(m => m.Insert(It.IsAny<InventoryProduct>()));
            this.mockInventoryProductRepository.Setup(m => m.Update(It.IsAny<InventoryProduct>()));
            this.mockInventoryProductRepository.Setup(m => m.Delete(It.IsAny<InventoryProduct>()));
            this.mockInventoryProductRepository.Setup(m => m.SingleOrDefaultAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>())).ReturnsAsync(new InventoryProduct { SystemTypeId = 2 });
            this.mockInventoryProductRepository.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<InventoryProduct> { inventoryProduct });
            this.mockUnitOfWork.Setup(m => m.CreateRepository<InventoryProduct>()).Returns(this.mockInventoryProductRepository.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Owner>()).Returns(this.mockOwnerRepository.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<AttributeEntity>()).Returns(this.mockAttributeRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<InventoryMovementIndex>()).Returns(this.mockInventoryMovementIndexRepository.Object);
            this.mockInventoryMovementIndexRepository.Setup(a => a.Insert(It.IsAny<InventoryMovementIndex>()));
            this.mockFileRegistrationTransactionRepository.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<FileRegistrationTransaction, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new FileRegistrationTransaction { FileRegistration = new Entities.Admin.FileRegistration { SystemTypeId = Entities.Dto.SystemType.SINOPER } });
            this.mockUnitOfWork.Setup(m => m.CreateRepository<FileRegistrationTransaction>()).Returns(this.mockFileRegistrationTransactionRepository.Object);

            this.mockServiceBusQueueClient.Setup(m => m.QueueSessionMessageAsync(It.IsAny<string>(), It.IsAny<string>()));
            this.mockAzureClientFactory.Setup(m => m.GetQueueClient(It.IsAny<string>())).Returns(this.mockServiceBusQueueClient.Object);

            this.inventoryProductRegistrationStrategy = new InventoryProductRegistrationStrategy(this.mockLogger.Object, this.mockAzureClientFactory.Object);
        }

        /// <summary>
        /// Registers the asynchronous should throw argument exception when entity is null asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task RegisterAsync_ShouldThrowArgumentException_WhenEntityIsNullAsync()
        {
            await this.inventoryProductRegistrationStrategy.RegisterAsync(null, this.mockUnitOfWork.Object).ConfigureAwait(false);
        }

        /// <summary>
        /// Registers the asynchronous should throw argument exception when unit of work is null asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task RegisterAsync_ShouldThrowArgumentException_WhenUnitOfWorkIsNullAsync()
        {
            await this.inventoryProductRegistrationStrategy.RegisterAsync(It.IsAny<object>(), null).ConfigureAwait(false);
        }

        /// <summary>
        /// Registers the asynchronous should register entity when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task RegisterAsync_ShouldInsertInventoryProductObject_WhenActionTypeInsertAsync()
        {
            // Arrange
            var entity = new InventoryProduct { EventType = "Insert", BlockchainInventoryProductTransactionId = Guid.NewGuid(), FileRegistrationTransactionId = 1, SystemTypeId = 2 };

            // Act
            await this.inventoryProductRegistrationStrategy.RegisterAsync(entity, this.mockUnitOfWork.Object).ConfigureAwait(false);

            // Assert
            this.mockUnitOfWork.Verify(m => m.CreateRepository<InventoryProduct>(), Times.Exactly(2));
            this.mockInventoryProductRepository.Verify(m => m.GetAllAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>()), Times.Exactly(1));
            this.mockInventoryProductRepository.Verify(m => m.Insert(It.IsAny<InventoryProduct>()), Times.Exactly(1));
            this.mockInventoryProductRepository.Verify(m => m.Update(It.IsAny<InventoryProduct>()), Times.Never);
            this.mockInventoryProductRepository.Verify(m => m.Delete(It.IsAny<InventoryProduct>()), Times.Never);
            this.mockUnitOfWork.Verify(m => m.SaveAsync(CancellationToken.None), Times.Exactly(1));
            this.mockUnitOfWork.Verify(a => a.CreateRepository<InventoryMovementIndex>(), Times.Exactly(1));
            this.mockInventoryMovementIndexRepository.Verify(a => a.Insert(It.IsAny<InventoryMovementIndex>()), Times.Exactly(1));
            this.mockServiceBusQueueClient.Verify(m => m.QueueSessionMessageAsync(It.IsAny<object>(), It.IsAny<string>()), Times.Exactly(1));
            this.mockAzureClientFactory.Verify(m => m.GetQueueClient(It.IsAny<string>()), Times.Exactly(1));
        }

        /// <summary>
        /// Registers the asynchronous should update entity when action type update asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task RegisterAsync_ShouldUpdateInventoryProductObject_WhenActionTypeUpdateAsync()
        {
            // Arrange
            var entity = new InventoryProduct { EventType = "Update", BlockchainInventoryProductTransactionId = Guid.NewGuid(), FileRegistrationTransactionId = 1 };
            entity.Owners.Add(new Owner());

            // Act
            await this.inventoryProductRegistrationStrategy.RegisterAsync(entity, this.mockUnitOfWork.Object).ConfigureAwait(false);

            // Assert
            this.mockUnitOfWork.Verify(m => m.CreateRepository<InventoryProduct>(), Times.Exactly(3));
            this.mockInventoryProductRepository.Verify(m => m.Insert(It.IsAny<InventoryProduct>()), Times.AtLeastOnce);
            this.mockInventoryProductRepository.Verify(m => m.Update(It.IsAny<InventoryProduct>()), Times.Never);
            this.mockInventoryProductRepository.Verify(m => m.Delete(It.IsAny<InventoryProduct>()), Times.Never);
            this.mockUnitOfWork.Verify(m => m.SaveAsync(CancellationToken.None), Times.Exactly(2));
            this.mockUnitOfWork.Verify(a => a.CreateRepository<InventoryMovementIndex>(), Times.Exactly(2));
            this.mockInventoryMovementIndexRepository.Verify(a => a.Insert(It.IsAny<InventoryMovementIndex>()), Times.Exactly(2));
            this.mockServiceBusQueueClient.Verify(m => m.QueueSessionMessageAsync(It.IsAny<object>(), It.IsAny<string>()), Times.Exactly(1));
            this.mockAzureClientFactory.Verify(m => m.GetQueueClient(It.IsAny<string>()), Times.Exactly(1));
        }

        /// <summary>
        /// Registers the asynchronous should delete entity when action type delete asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task RegisterAsync_ShouldDeleteInventoryProductObject_WhenActionTypeDeleteAsync()
        {
            // Arrange
            var entity = new InventoryProduct { EventType = "Delete", BlockchainInventoryProductTransactionId = Guid.NewGuid(), FileRegistrationTransactionId = 1 };
            entity.Owners.Add(new Owner());

            // Act
            await this.inventoryProductRegistrationStrategy.RegisterAsync(entity, this.mockUnitOfWork.Object).ConfigureAwait(false);

            // Assert
            this.mockUnitOfWork.Verify(m => m.CreateRepository<InventoryProduct>(), Times.Exactly(2));
            this.mockInventoryProductRepository.Verify(m => m.Insert(It.IsAny<InventoryProduct>()), Times.Exactly(1));
            this.mockInventoryProductRepository.Verify(m => m.Update(It.IsAny<InventoryProduct>()), Times.Never);
            this.mockInventoryProductRepository.Verify(m => m.Delete(It.IsAny<InventoryProduct>()), Times.Never);
            this.mockUnitOfWork.Verify(m => m.SaveAsync(CancellationToken.None), Times.Exactly(1));
            this.mockUnitOfWork.Verify(a => a.CreateRepository<InventoryMovementIndex>(), Times.Exactly(1));
            this.mockInventoryMovementIndexRepository.Verify(a => a.Insert(It.IsAny<InventoryMovementIndex>()), Times.Exactly(1));
            this.mockServiceBusQueueClient.Verify(m => m.QueueSessionMessageAsync(It.IsAny<object>(), It.IsAny<string>()), Times.Exactly(1));
            this.mockAzureClientFactory.Verify(m => m.GetQueueClient(It.IsAny<string>()), Times.Exactly(1));
        }

        /// <summary>
        /// Registers the asynchronous should delete entity when action type delete asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task RegisterAsync_ShouldDeleteInventoryProductObjectWithAbsoluteOwnerValue_WhenActionTypeDeleteAsync()
        {
            // Arrange
            var entity = new InventoryProduct { EventType = "Delete", BlockchainInventoryProductTransactionId = Guid.NewGuid(), FileRegistrationTransactionId = 1 };
            entity.Owners.Add(new Owner { OwnershipValue = 100, OwnershipValueUnit = Constants.OwnershipPercentageUnit });

            // Act
            await this.inventoryProductRegistrationStrategy.RegisterAsync(entity, this.mockUnitOfWork.Object).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(100.00M, entity.Owners.ElementAt(0).OwnershipValue);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<InventoryProduct>(), Times.Exactly(2));
            this.mockInventoryProductRepository.Verify(m => m.Insert(It.IsAny<InventoryProduct>()), Times.Exactly(1));
            this.mockInventoryProductRepository.Verify(m => m.Update(It.IsAny<InventoryProduct>()), Times.Never);
            this.mockInventoryProductRepository.Verify(m => m.Delete(It.IsAny<InventoryProduct>()), Times.Never);
            this.mockUnitOfWork.Verify(m => m.SaveAsync(CancellationToken.None), Times.Exactly(1));
            this.mockUnitOfWork.Verify(a => a.CreateRepository<InventoryMovementIndex>(), Times.Exactly(1));
            this.mockInventoryMovementIndexRepository.Verify(a => a.Insert(It.IsAny<InventoryMovementIndex>()), Times.Exactly(1));
            this.mockServiceBusQueueClient.Verify(m => m.QueueSessionMessageAsync(It.IsAny<object>(), It.IsAny<string>()), Times.Exactly(1));
            this.mockAzureClientFactory.Verify(m => m.GetQueueClient(It.IsAny<string>()), Times.Exactly(1));
        }
    }
}
