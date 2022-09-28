// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CalculateOwnershipTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Ownership.Tests.Calculation
{
    using System;
    using System.Collections.Generic;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Ownership;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The system calculator tests.
    /// </summary>
    [TestClass]
    public class CalculateOwnershipTests
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private CalculateOwnership calculateOwnership;

        /// <summary>
        /// The unit of work mock.
        /// </summary>
        private Mock<IUnitOfWorkFactory> unitOfWorkFactoryMock;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<IUnitOfWork> unitOfWorkMock;

        /// <summary>
        /// The system repository mock.
        /// </summary>
        private Mock<IRepository<OwnershipCalculation>> mockOwnershipCalculationRepository;

        /// <summary>
        /// The system repository mock.
        /// </summary>
        private Mock<IRepository<SegmentOwnershipCalculation>> mockSegmentOwnershipCalculationRepository;

        /// <summary>
        /// The system repository mock.
        /// </summary>
        private Mock<IRepository<SystemOwnershipCalculation>> mockSystemOwnershipCalculationRepository;

        /// <summary>
        /// The logger.
        /// </summary>
        private List<Movement> inputMovements;

        /// <summary>
        /// The logger.
        /// </summary>
        private List<Movement> outputMovements;

        /// <summary>
        /// The logger.
        /// </summary>
        private List<Movement> movements;

        /// <summary>
        /// The logger.
        /// </summary>
        private List<InventoryProduct> initialInventories;

        /// <summary>
        /// The logger.
        /// </summary>
        private List<InventoryProduct> finalInventories;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.unitOfWorkMock = new Mock<IUnitOfWork>();
            this.unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
            this.mockOwnershipCalculationRepository = new Mock<IRepository<OwnershipCalculation>>();
            this.mockSegmentOwnershipCalculationRepository = new Mock<IRepository<SegmentOwnershipCalculation>>();
            this.mockSystemOwnershipCalculationRepository = new Mock<IRepository<SystemOwnershipCalculation>>();
            this.unitOfWorkFactoryMock.Setup(x => x.GetUnitOfWork()).Returns(this.unitOfWorkMock.Object);
            this.unitOfWorkMock.Setup(s => s.CreateRepository<OwnershipCalculation>()).Returns(this.mockOwnershipCalculationRepository.Object);
            this.unitOfWorkMock.Setup(s => s.CreateRepository<SegmentOwnershipCalculation>()).Returns(this.mockSegmentOwnershipCalculationRepository.Object);
            this.unitOfWorkMock.Setup(s => s.CreateRepository<SystemOwnershipCalculation>()).Returns(this.mockSystemOwnershipCalculationRepository.Object);
            this.mockOwnershipCalculationRepository.Setup(s => s.Insert(It.IsAny<OwnershipCalculation>()));
            this.mockSegmentOwnershipCalculationRepository.Setup(s => s.Insert(It.IsAny<SegmentOwnershipCalculation>()));
            this.mockSystemOwnershipCalculationRepository.Setup(s => s.Insert(It.IsAny<SystemOwnershipCalculation>()));
            this.calculateOwnership = new CalculateOwnership();
            this.SetupData();
        }

        /// <summary>
        /// system calculator should calculate.
        /// </summary>
        [TestMethod]
        public void CalculateOwnership_ShouldCalculate_WithSuccess()
        {
            // Act
            var ownershipCalculationResults = new List<OwnershipCalculation>();
            var result = this.calculateOwnership.Calculate(1, "P1", 31, DateTime.Today.AddDays(-2), 27, 1, this.inputMovements, this.outputMovements, this.initialInventories, this.finalInventories, this.movements);
            ownershipCalculationResults.Add(result);
            this.calculateOwnership.CalculatePercentageAndRegister(ownershipCalculationResults);

            // Assert or Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(result.InitialInventoryVolume, new decimal(10921.56));
            Assert.AreEqual(result.FinalInventoryVolume, new decimal(3452.12));
            Assert.AreEqual(result.InputVolume, new decimal(4000.28));
            Assert.AreEqual(result.OutputVolume, new decimal(2333.17));
            Assert.AreEqual(result.InterfaceVolume, new decimal(-1549.83));
            Assert.AreEqual(result.ToleranceVolume, new decimal(5643.56));
            Assert.AreEqual(result.UnidentifiedLossesVolume, new decimal(0));
            Assert.AreEqual(result.UnbalanceVolume, new decimal(13230.28));
        }

        /// <summary>
        /// system calculator should calculate.
        /// </summary>
        [TestMethod]
        public void CalculateOwnership_ShouldCalculateForSegment_WithSuccess()
        {
            // Act
            var ownershipCalculationResults = new List<SegmentOwnershipCalculation>();
            var result = this.calculateOwnership.CalculateAndRegisterForSegment("P1", 31, DateTime.Today.AddDays(-2), 27, 1, 1, this.inputMovements, this.outputMovements, this.initialInventories, this.finalInventories, this.movements, new List<int> { 1 });
            ownershipCalculationResults.Add(result);
            this.calculateOwnership.CalculatePercentageAndRegisterForSegment(ownershipCalculationResults);

            // Assert or Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(result.InitialInventoryVolume, new decimal(10921.56));
            Assert.AreEqual(result.FinalInventoryVolume, new decimal(3452.12));
            Assert.AreEqual(result.InputVolume, new decimal(4000.28));
            Assert.AreEqual(result.OutputVolume, new decimal(2333.17));
        }

        /// <summary>
        /// system calculator should calculate .
        /// </summary>
        [TestMethod]
        public void CalculateOwnership_ShouldCalculateForSystem_WithSuccess()
        {
            // Act
            var ownershipCalculationResults = new List<SystemOwnershipCalculation>();
            var result = this.calculateOwnership.CalculateAndRegisterForSystem("P1", 31, DateTime.Today.AddDays(-2), 27, 1, 1, 1, this.inputMovements, this.outputMovements, this.initialInventories, this.finalInventories, this.movements, new List<int> { 1 });
            ownershipCalculationResults.Add(result);
            this.calculateOwnership.CalculatePercentageAndRegisterForSystem(ownershipCalculationResults);

            // Assert or Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(result.InitialInventoryVolume, new decimal(10921.56));
            Assert.AreEqual(result.FinalInventoryVolume, new decimal(3452.12));
            Assert.AreEqual(result.InputVolume, new decimal(4000.28));
            Assert.AreEqual(result.OutputVolume, new decimal(2333.17));
        }

        /// <summary>
        /// Setups the data.
        /// </summary>
        private void SetupData()
        {
            var movementOne = this.GetMovement(100, MessageType.Movement, null, -1, "1000");
            var movementSourceOne = this.GetMovementSource(100, 1, "P1");
            var movementDestinationOne = this.GetMovementDestination(100, 2, "P2");
            var movementOwnershipOne = this.GetOwnership(27, new decimal(2333.17), 100, null, -1);

            movementOne.MovementSource = movementSourceOne;
            movementOne.MovementDestination = movementDestinationOne;
            movementOne.Ownerships.Add(movementOwnershipOne);

            var movementTwo = this.GetMovement(101, MessageType.Movement, null, -1, "1000");
            var movementSourceTwo = this.GetMovementSource(101, 3, "P3");
            var movementDestinationTwo = this.GetMovementDestination(101, 1, "P1");
            var movementOwnershipTwo = this.GetOwnership(27, new decimal(4000.28), 100, null, -1);

            movementTwo.MovementSource = movementSourceTwo;
            movementTwo.MovementDestination = movementDestinationTwo;
            movementTwo.Ownerships.Add(movementOwnershipTwo);

            var movementThree = this.GetMovement(102, MessageType.SpecialMovement, VariableType.Interface, -1, "1001");
            var movementSourceThree = this.GetMovementSource(102, 1, "P1");
            var movementDestinationThree = this.GetMovementDestination(102, 4, "P1");
            var movementOwnershipThree = this.GetOwnership(27, new decimal(2987.5), 102, null, -1);

            movementThree.MovementSource = movementSourceThree;
            movementThree.MovementDestination = movementDestinationThree;
            movementThree.Ownerships.Add(movementOwnershipThree);

            var movementFour = this.GetMovement(103, MessageType.SpecialMovement, VariableType.BalanceTolerance, -1, "1002");
            var movementSourceFour = this.GetMovementSource(103, 5, "P1");
            var movementDestinationFour = this.GetMovementDestination(103, 1, "P1");
            var movementOwnershipFour = this.GetOwnership(27, new decimal(5643.56), 102, null, -1);

            movementFour.MovementSource = movementSourceFour;
            movementFour.MovementDestination = movementDestinationFour;
            movementFour.Ownerships.Add(movementOwnershipFour);

            var movementFive = this.GetMovement(105, MessageType.SpecialMovement, VariableType.Interface, -1, "1001");
            var movementSourceFive = this.GetMovementSource(105, 5, "P1");
            var movementDestinationFive = this.GetMovementDestination(105, 1, "P1");
            var movementOwnershipFive = this.GetOwnership(27, new decimal(1437.67), 105, null, -1);

            movementFive.MovementSource = movementSourceFive;
            movementFive.MovementDestination = movementDestinationFive;
            movementFive.Ownerships.Add(movementOwnershipFive);

            var inventoryProductOne = this.GetInventoryProduct(100, "P1", new decimal(3452.12), "INV1", -1, 1);
            var invOwnershipOne = this.GetOwnership(27, new decimal(3452.12), null, 100, -1);
            inventoryProductOne.Ownerships.Add(invOwnershipOne);

            var inventoryProductTwo = this.GetInventoryProduct(200, "P1", new decimal(300), "INV2", 0, 1);
            var invOwnershipTwo = this.GetOwnership(27, new decimal(6543.56), null, 200, 0);
            inventoryProductTwo.Ownerships.Add(invOwnershipTwo);

            var inventoryProductThree = this.GetInventoryProduct(300, "P1", new decimal(300), "INV2", 0, 1);
            var invOwnershipThree = this.GetOwnership(27, new decimal(4378), null, 300, 0);
            inventoryProductThree.Ownerships.Add(invOwnershipThree);

            this.inputMovements = new List<Movement> { movementTwo };
            this.outputMovements = new List<Movement> { movementOne };
            this.initialInventories = new List<InventoryProduct> { inventoryProductTwo, inventoryProductThree };
            this.finalInventories = new List<InventoryProduct> { inventoryProductOne };
            this.movements = new List<Movement> { movementThree, movementFour, movementFive };
        }

        /// <summary>
        /// Gets the movement.
        /// </summary>
        /// <param name="movementTransactionId">The movement transaction identifier.</param>
        /// <param name="messageType">The message type.</param>
        /// <param name="variableType">The variable type.</param>
        /// <param name="days">The days.</param>
        /// <param name="movementId">The movement identifier.</param>
        /// <returns>The movement.</returns>
        private Movement GetMovement(int movementTransactionId, MessageType messageType, VariableType? variableType, int days, string movementId)
        {
            var movement = new Movement();
            movement.MovementTransactionId = movementTransactionId;
            movement.MessageTypeId = (int)messageType;
            movement.MovementId = movementId;
            movement.VariableTypeId = variableType;
            movement.TicketId = 123;
            movement.OwnershipTicketId = 1;
            movement.SegmentId = 1;
            movement.OperationalDate = DateTime.Today.AddDays(days - 1);
            movement.NetStandardVolume = 100;
            movement.MeasurementUnit = 31;

            return movement;
        }

        /// <summary>
        /// Gets the ownership.
        /// </summary>
        /// <param name="ownerId">The owner identifier.</param>
        /// <param name="ownershipVolume">The ownership volume.</param>
        /// <param name="movementTransactionId">The movement transaction identifier.</param>
        /// <param name="inventoryProductId">The inventory product identifier.</param>
        /// <param name="days">The days.</param>
        /// <returns>The Ownership.</returns>
        private Ownership GetOwnership(int ownerId, decimal ownershipVolume, int? movementTransactionId, int? inventoryProductId, int days)
        {
            var ownership = new Ownership();
            ownership.OwnershipId = 100;
            ownership.TicketId = 1;
            ownership.MovementTransactionId = movementTransactionId;
            ownership.InventoryProductId = inventoryProductId;
            ownership.OwnerId = ownerId;
            ownership.OwnershipVolume = ownershipVolume;
            ownership.ExecutionDate = DateTime.Today.AddDays(days - 1);

            return ownership;
        }

        /// <summary>
        /// Gets the movement destination.
        /// </summary>
        /// <param name="movementTransactionId">The movement transaction identifier.</param>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <returns>The MovementDestination.</returns>
        private MovementDestination GetMovementDestination(int movementTransactionId, int nodeId, string productId)
        {
            var movementDestination = new MovementDestination();
            movementDestination.MovementDestinationId = 100;
            movementDestination.MovementTransactionId = movementTransactionId;
            movementDestination.DestinationNodeId = nodeId;
            movementDestination.DestinationProductId = productId;
            movementDestination.DestinationProduct = this.GetProduct(productId);

            return movementDestination;
        }

        /// <summary>
        /// Gets the movement source.
        /// </summary>
        /// <param name="movementTransactionId">The movement transaction identifier.</param>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <returns>The MovementSource.</returns>
        private MovementSource GetMovementSource(int movementTransactionId, int nodeId, string productId)
        {
            var movementSource = new MovementSource();
            movementSource.MovementSourceId = 200;
            movementSource.MovementTransactionId = movementTransactionId;
            movementSource.SourceNodeId = nodeId;
            movementSource.SourceProductId = productId;
            movementSource.SourceProduct = this.GetProduct(productId);

            return movementSource;
        }

        /// <summary>
        /// Gets the product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>The Product.</returns>
        private Product GetProduct(string productId)
        {
            return new Product { ProductId = productId };
        }

        /// <summary>
        /// Gets the inventory product.
        /// </summary>
        /// <param name="inventoryProductId">The inventory product identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="productVolume">The product volume.</param>
        /// <returns>The InventoryProduct.</returns>
        private InventoryProduct GetInventoryProduct(int inventoryProductId, string productId, decimal productVolume, string inventoryId, int days, int nodeId)
        {
            var inventoryProduct = new InventoryProduct();
            inventoryProduct.InventoryProductId = inventoryProductId;
            inventoryProduct.ProductId = productId;
            inventoryProduct.ProductVolume = productVolume;
            inventoryProduct.OwnershipTicketId = 1;
            inventoryProduct.InventoryId = inventoryId;
            inventoryProduct.SegmentId = 1;
            inventoryProduct.InventoryDate = DateTime.Today.AddDays(days - 1);
            inventoryProduct.NodeId = nodeId;
            inventoryProduct.MeasurementUnit = 31;

            return inventoryProduct;
        }
    }
}
