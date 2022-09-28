// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipValidatorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Ownership.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Services;
    using Ecp.True.Proxies.OwnershipRules.Request;
    using Ecp.True.Proxies.OwnershipRules.Response;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The OwnershipValidatorTests.
    /// </summary>
    [TestClass]
    public class OwnershipValidatorTests
    {
        /// <summary>
        /// The mock repository factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockRepositoryFactory;

        /// <summary>
        /// The mock ticket repository.
        /// </summary>
        private Mock<IRepository<Ticket>> mockTicketRepository;

        /// <summary>
        /// The mock inventory product repository.
        /// </summary>
        private Mock<IRepository<InventoryProduct>> mockInventoryProductRepository;

        /// <summary>
        /// The mock movement repository.
        /// </summary>
        private Mock<IRepository<Movement>> mockMovementRepository;

        /// <summary>
        /// The ownership validator.
        /// </summary>
        private OwnershipValidator ownershipValidator;

        /// <summary>
        /// The ownership rule data.
        /// </summary>
        private OwnershipRuleData ownershipRuleData;

        private OwnershipRuleRequest ownershipRuleRequest;

        private OwnershipRuleResponse ownershipRuleResponse;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockRepositoryFactory = new Mock<IRepositoryFactory>();
            this.mockTicketRepository = new Mock<IRepository<Ticket>>();
            this.mockInventoryProductRepository = new Mock<IRepository<InventoryProduct>>();
            this.mockMovementRepository = new Mock<IRepository<Movement>>();
            this.ownershipRuleData = new OwnershipRuleData() { TicketId = 25281 };
            this.ownershipValidator = new OwnershipValidator(this.mockRepositoryFactory.Object);
            this.ownershipRuleRequest = new OwnershipRuleRequest();
            this.ownershipRuleResponse = new OwnershipRuleResponse();
        }

        /// <summary>
        /// Validates the error excel should process if both inventory and movement list are not empty asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ValidateErrorExcel_ShouldProcessIfBothInventoryAndMovementListAreNotEmptyAndMovementSourceHasValueAsync()
        {
            var ticketId = "12";
            var inventoryList = new List<OwnershipErrorInventory>();
            var inv = new OwnershipErrorInventory
            {
                Ticket = ticketId,
                ResponseInventoryId = "1",
                ResponseNodeId = "12",
                ErrorDescription = string.Empty,
            };
            inventoryList.Add(inv);

            var movementList = new List<OwnershipErrorMovement>();
            var mov = new OwnershipErrorMovement
            {
                Ticket = ticketId,
                ResponseMovementId = "1",
                ResponseSourceNodeId = "12",
                ErrorDescription = string.Empty,
            };
            movementList.Add(mov);

            var inventoryProduct = new InventoryProduct { NodeId = 12, InventoryProductId = 1 };
            var movement = new Movement { MovementDestination = new MovementDestination { DestinationNodeId = 12 }, MovementTransactionId = 1 };

            var trueInventoryProducts = new List<InventoryProduct> { inventoryProduct };
            var trueMovements = new List<Movement> { movement };

            this.mockInventoryProductRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>())).Returns(Task.FromResult(trueInventoryProducts.AsEnumerable()));
            this.mockMovementRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).Returns(Task.FromResult(trueMovements.AsEnumerable()));

            this.mockRepositoryFactory.Setup(a => a.CreateRepository<InventoryProduct>()).Returns(this.mockInventoryProductRepository.Object);
            this.mockRepositoryFactory.Setup(a => a.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);

            // Act
            var result = await this.ownershipValidator.ValidateOwnershipRuleErrorAsync(inventoryList, movementList).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Any());
            this.mockInventoryProductRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockMovementRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>()), Times.Once);

            this.mockRepositoryFactory.Verify(a => a.CreateRepository<InventoryProduct>(), Times.Once);
            this.mockRepositoryFactory.Verify(a => a.CreateRepository<Movement>(), Times.Once);
        }

        /// <summary>
        /// Validates the error excel should process if both inventory and movement list are not empty asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ValidateErrorExcel_ShouldProcessIfBothInventoryAndMovementListAreNotEmptyAndMovementDestinationHasValueAsync()
        {
            var ticketId = "12";
            var inventoryList = new List<OwnershipErrorInventory>();
            var inv = new OwnershipErrorInventory
            {
                Ticket = ticketId,
                ResponseInventoryId = "1",
                ResponseNodeId = "12",
                ErrorDescription = string.Empty,
            };
            inventoryList.Add(inv);

            var movementList = new List<OwnershipErrorMovement>();
            var mov = new OwnershipErrorMovement
            {
                Ticket = ticketId,
                ResponseMovementId = "1",
                ResponseSourceNodeId = "12",
                ErrorDescription = string.Empty,
            };
            movementList.Add(mov);

            var inventoryProduct = new InventoryProduct { NodeId = 12, InventoryProductId = 1 };
            var movement = new Movement { MovementSource = new MovementSource { SourceNodeId = 12 }, MovementTransactionId = 1 };

            var trueInventoryProducts = new List<InventoryProduct> { inventoryProduct };
            var trueMovements = new List<Movement> { movement };

            this.mockInventoryProductRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>())).Returns(Task.FromResult(trueInventoryProducts.AsEnumerable()));
            this.mockMovementRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).Returns(Task.FromResult(trueMovements.AsEnumerable()));

            this.mockRepositoryFactory.Setup(a => a.CreateRepository<InventoryProduct>()).Returns(this.mockInventoryProductRepository.Object);
            this.mockRepositoryFactory.Setup(a => a.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);

            // Act
            var result = await this.ownershipValidator.ValidateOwnershipRuleErrorAsync(inventoryList, movementList).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Any());
            this.mockInventoryProductRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockMovementRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>()), Times.Once);

            this.mockRepositoryFactory.Verify(a => a.CreateRepository<InventoryProduct>(), Times.Once);
            this.mockRepositoryFactory.Verify(a => a.CreateRepository<Movement>(), Times.Once);
        }

        /// <summary>
        /// Validates the error excel should process if only inventory list is empty asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ValidateErrorExcel_ShouldProcessIfOnlyInventoryListIsEmptyAsync()
        {
            var ticketId = "12";
            var inventoryList = new List<OwnershipErrorInventory>();

            var movementList = new List<OwnershipErrorMovement>();
            var mov = new OwnershipErrorMovement
            {
                Ticket = ticketId,
                ResponseMovementId = "1",
                ResponseSourceNodeId = "12",
                ErrorDescription = string.Empty,
            };
            movementList.Add(mov);

            var inventoryProduct = new InventoryProduct { NodeId = 12, InventoryProductId = 1 };
            var movement = new Movement { MovementSource = new MovementSource { SourceNodeId = 12 }, MovementTransactionId = 1 };

            var trueInventoryProducts = new List<InventoryProduct> { inventoryProduct };
            var trueMovements = new List<Movement> { movement };

            this.mockInventoryProductRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>())).Returns(Task.FromResult(trueInventoryProducts.AsEnumerable()));
            this.mockMovementRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).Returns(Task.FromResult(trueMovements.AsEnumerable()));

            this.mockRepositoryFactory.Setup(a => a.CreateRepository<InventoryProduct>()).Returns(this.mockInventoryProductRepository.Object);
            this.mockRepositoryFactory.Setup(a => a.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);

            // Act
            var result = await this.ownershipValidator.ValidateOwnershipRuleErrorAsync(inventoryList, movementList).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Any());
            this.mockMovementRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockInventoryProductRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>()), Times.Never);

            this.mockRepositoryFactory.Verify(a => a.CreateRepository<Movement>(), Times.Once);
            this.mockRepositoryFactory.Verify(a => a.CreateRepository<InventoryProduct>(), Times.Never);
        }

        /// <summary>
        /// Validates the error excel should process if only movement list is empty asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ValidateErrorExcel_ShouldProcessIfOnlyMovementListIsEmptyAsync()
        {
            var ticketId = "12";
            var inventoryList = new List<OwnershipErrorInventory>();
            var inv = new OwnershipErrorInventory
            {
                Ticket = ticketId,
                ResponseInventoryId = "1",
                ResponseNodeId = "12",
                ErrorDescription = string.Empty,
            };
            inventoryList.Add(inv);

            var movementList = new List<OwnershipErrorMovement>();

            var inventoryProduct = new InventoryProduct { NodeId = 12, InventoryProductId = 1 };
            var movement = new Movement { MovementSource = new MovementSource { SourceNodeId = 12 }, MovementTransactionId = 1 };

            var trueInventoryProducts = new List<InventoryProduct> { inventoryProduct };
            var trueMovements = new List<Movement> { movement };

            this.mockInventoryProductRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>())).Returns(Task.FromResult(trueInventoryProducts.AsEnumerable()));
            this.mockMovementRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).Returns(Task.FromResult(trueMovements.AsEnumerable()));

            this.mockRepositoryFactory.Setup(a => a.CreateRepository<InventoryProduct>()).Returns(this.mockInventoryProductRepository.Object);
            this.mockRepositoryFactory.Setup(a => a.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);

            // Act
            var result = await this.ownershipValidator.ValidateOwnershipRuleErrorAsync(inventoryList, movementList).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Any());
            this.mockInventoryProductRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockRepositoryFactory.Verify(a => a.CreateRepository<InventoryProduct>(), Times.Once);

            this.mockRepositoryFactory.Verify(a => a.CreateRepository<Movement>(), Times.Never);
            this.mockMovementRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>()), Times.Never);
        }

        /// <summary>
        /// Validates the error excel should return erros if inventory node identifier does not match asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ValidateErrorExcel_ShouldReturnErrosIfInventoryNodeIdDoesNotMatchAsync()
        {
            var ticketId = "12";
            var inventoryList = new List<OwnershipErrorInventory>();
            var inv = new OwnershipErrorInventory
            {
                Ticket = ticketId,
                ResponseInventoryId = "1",
                ResponseNodeId = "12",
                ErrorDescription = string.Empty,
            };
            inventoryList.Add(inv);

            var movementList = new List<OwnershipErrorMovement>();
            var mov = new OwnershipErrorMovement
            {
                Ticket = ticketId,
                ResponseMovementId = "1",
                ResponseSourceNodeId = "12",
                ErrorDescription = string.Empty,
            };
            movementList.Add(mov);

            var inventoryProduct = new InventoryProduct { NodeId = 11, InventoryProductId = 1 };
            var movement = new Movement { MovementSource = new MovementSource { SourceNodeId = 12 }, MovementTransactionId = 1 };
            var trueInventoryProducts = new List<InventoryProduct> { inventoryProduct };
            var trueMovements = new List<Movement> { movement };

            this.mockInventoryProductRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>())).Returns(Task.FromResult(trueInventoryProducts.AsEnumerable()));
            this.mockMovementRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).Returns(Task.FromResult(trueMovements.AsEnumerable()));

            this.mockRepositoryFactory.Setup(a => a.CreateRepository<InventoryProduct>()).Returns(this.mockInventoryProductRepository.Object);
            this.mockRepositoryFactory.Setup(a => a.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);

            // Act
            var result = await this.ownershipValidator.ValidateOwnershipRuleErrorAsync(inventoryList, movementList).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(string.Format(CultureInfo.InvariantCulture, Constants.ValidateInvalidNodeForInventoryProductFailureMessage, inventoryProduct.InventoryProductId), result.FirstOrDefault().Message);
            this.mockInventoryProductRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockMovementRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>()), Times.Once);

            this.mockRepositoryFactory.Verify(a => a.CreateRepository<InventoryProduct>(), Times.Once);
            this.mockRepositoryFactory.Verify(a => a.CreateRepository<Movement>(), Times.Once);
        }

        /// <summary>
        /// Validates the error excel should return erros if movement source node identifier does not match asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ValidateErrorExcel_ShouldReturnErrosIfMovementSourceNodeIdAndDestinationNodeIdDoesNotMatchAsync()
        {
            var ticketId = "12";
            var inventoryList = new List<OwnershipErrorInventory>();
            var inv = new OwnershipErrorInventory
            {
                Ticket = ticketId,
                ResponseInventoryId = "1",
                ResponseNodeId = "12",
                ErrorDescription = string.Empty,
            };
            inventoryList.Add(inv);

            var movementList = new List<OwnershipErrorMovement>();
            var mov = new OwnershipErrorMovement
            {
                Ticket = ticketId,
                ResponseMovementId = "1",
                ResponseSourceNodeId = "12",
                ErrorDescription = string.Empty,
            };
            movementList.Add(mov);

            var inventoryProduct = new InventoryProduct { NodeId = 12, InventoryProductId = 1 };
            var movement = new Movement
            {
                MovementSource = new MovementSource { SourceNodeId = 11 },
                MovementDestination = new MovementDestination { DestinationNodeId = 100 },
                MovementTransactionId = 1,
            };
            var trueInventoryProducts = new List<InventoryProduct> { inventoryProduct };
            var trueMovements = new List<Movement> { movement };

            this.mockInventoryProductRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>())).Returns(Task.FromResult(trueInventoryProducts.AsEnumerable()));
            this.mockMovementRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).Returns(Task.FromResult(trueMovements.AsEnumerable()));

            this.mockRepositoryFactory.Setup(a => a.CreateRepository<InventoryProduct>()).Returns(this.mockInventoryProductRepository.Object);
            this.mockRepositoryFactory.Setup(a => a.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);

            // Act
            var result = await this.ownershipValidator.ValidateOwnershipRuleErrorAsync(inventoryList, movementList).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(string.Format(CultureInfo.InvariantCulture, Constants.ValidateInvalidNodeForMovementFailureMessage, movement.MovementTransactionId), result.FirstOrDefault().Message);
            this.mockInventoryProductRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockMovementRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>()), Times.Once);

            this.mockRepositoryFactory.Verify(a => a.CreateRepository<InventoryProduct>(), Times.Once);
            this.mockRepositoryFactory.Verify(a => a.CreateRepository<Movement>(), Times.Once);
        }

        /// <summary>
        /// Ownerships the validator error messages return error.
        /// </summary>
        [TestMethod]
        public void OwnershipValidatorErrorMessages_ReturnError()
        {
            var movementOperationalData = new MovementOperationalData()
            {
                Ticket = 25281,
                MovementTransactionId = 5452,
                OwnershipUnit = "%",
                MovementTypeId = "43",
            };

            var inventoryOperationalData = new InventoryOperationalData()
            {
                Ticket = 25281,
                InventoryId = 5452,
                OwnershipUnit = "%",
            };

            var ownershipRuleResponseInventoryResults = new OwnershipResultInventory()
            {
                ResponseInventoryId = "5451",
            };

            var ownershipRuleResponseMovementyResults = new OwnershipResultMovement()
            {
                ResponseMovementId = "5451",
            };

            this.ownershipRuleRequest.MovementsOperationalData = new List<MovementOperationalData> { movementOperationalData };
            this.ownershipRuleRequest.InventoryOperationalData = new List<InventoryOperationalData> { inventoryOperationalData };
            this.ownershipRuleResponse.InventoryResults = new List<OwnershipResultInventory> { ownershipRuleResponseInventoryResults };
            this.ownershipRuleResponse.MovementResults = new List<OwnershipResultMovement> { ownershipRuleResponseMovementyResults };
            this.ownershipRuleData.OwnershipRuleRequest = this.ownershipRuleRequest;
            this.ownershipRuleData.OwnershipRuleResponse = this.ownershipRuleResponse;

            // Act
            var result = OwnershipValidator.ValidateOwnershipRuleResult(this.ownershipRuleData);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Constants.ValidationFailureMessage, result.ToList()[0].Message);
            Assert.AreEqual(Constants.ValidateInventoryOwnershipPercentageFailureMessage, result.ToList()[1].Message);
            Assert.AreEqual(Constants.ValidateMovementOwnershipPercentageFailureMessage, result.ToList()[2].Message);
        }
    }
}
