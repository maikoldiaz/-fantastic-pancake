// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElementValidatorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Registration.Tests
{
    using System;
    using System.IO;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Registration.Validation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;

    [TestClass]
    public class ElementValidatorTests
    {
        /// <summary>
        /// The respository factory.
        /// </summary>
        private Mock<IRepositoryFactory> respositoryFactory;

        /// <summary>
        /// The Product movement validator.
        /// </summary>
        private ElementValidator<Movement> movementElementValidator;

        /// <summary>
        /// The Product inventory validator.
        /// </summary>
        private ElementValidator<InventoryProduct> inventoryElementValidator;

        /// <summary>
        /// The Event element validator.
        /// </summary>
        private ElementValidator<Event> eventElementValidator;

        [TestInitialize]
        public void Initialize()
        {
            this.respositoryFactory = new Mock<IRepositoryFactory>();
            this.movementElementValidator = new ElementValidator<Movement>(this.respositoryFactory.Object);
            this.inventoryElementValidator = new ElementValidator<InventoryProduct>(this.respositoryFactory.Object);
            this.eventElementValidator = new ElementValidator<Event>(this.respositoryFactory.Object);
        }

        /// <summary>
        /// Validates the null inventory.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "An inventory Object of null was inappropriately allowed.")]
        public async Task InventoryElementValidator_ShouldFailWithNullInventoryUnit_WithInventoryAsync()
        {
            InventoryProduct inventoryObject = null;
            await this.inventoryElementValidator.ValidateAsync(inventoryObject).ConfigureAwait(false);
        }

        /// <summary>
        /// Validates the wrong product element in inventory should fail.
        /// </summary>
        /// <returns>The Tasks.</returns>
        [TestMethod]
        public async Task ProductInventoryElementValidator_ShouldFailWithWorngProduct_WithInventoryAsync()
        {
            // Arrange
            var inventory = File.ReadAllText("InventoryJson/InventoryWithWrongProduct.json");
            var inventoryObject = JsonConvert.DeserializeObject<InventoryProduct>(inventory);

            // Act
            var result = await this.ValidateInventoryElementCountAsync(inventoryObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Validates the wrong unit element in inventory should fail.
        /// </summary>
        /// <returns>The Tasks.</returns>
        [TestMethod]
        public async Task UnitInventoryElementValidator_ShouldFailWithWorngUnit_WithInventoryAsync()
        {
            // Arrange
            var inventory = File.ReadAllText("InventoryJson/InventoryWithWrongUnit.json");
            var inventoryObject = JsonConvert.DeserializeObject<InventoryProduct>(inventory);

            // Act
            var result = await this.ValidateInventoryElementCountAsync(inventoryObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(!result);
        }

        /// <summary>
        /// Validates the wrong owner element in inventory should fail.
        /// </summary>
        /// <returns>The Tasks.</returns>
        [TestMethod]
        public async Task OwnerInventoryElementValidator_ShouldFailWithWorngOwner_WithInventoryAsync()
        {
            // Arrange
            var inventory = File.ReadAllText("InventoryJson/InventoryWithWrongOwner.json");
            var inventoryObject = JsonConvert.DeserializeObject<InventoryProduct>(inventory);

            // Act
            var result = await this.ValidateInventoryElementCountAsync(inventoryObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task ProductInventoryElementValidator_ShouldPassWithValidInventoryAsync()
        {
            // Arrange
            var inventory = File.ReadAllText("InventoryJson/SingleInventory.json");
            var inventoryObject = JsonConvert.DeserializeObject<InventoryProduct>(inventory);

            // Act
            var result = await this.ValidateInventoryElementCountAsync(inventoryObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Validates the null movement.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "An movement Object of null was inappropriately allowed.")]
        public async Task MovementElementValidator_ShouldFailWithNullMovementUnit_WithMovementAsync()
        {
            Movement movementObject = null;
            await this.movementElementValidator.ValidateAsync(movementObject).ConfigureAwait(false);
        }

        /// <summary>
        /// Validates the wrong product element in movement should fail.
        /// </summary>
        /// <returns>The Tasks.</returns>
        [TestMethod]
        public async Task ProductMovementElementValidator_ShouldFailWithWorngProduct_WithMOvementAsync()
        {
            // Arrange
            var movement = File.ReadAllText("MovementJson/MovementWithWrongProductTypeId.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);

            // Act
            var result = await this.ValidateMovementElementCountAsync(movementObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Validates the wrong unit element in movement should fail.
        /// </summary>
        /// <returns>The Tasks.</returns>
        [TestMethod]
        public async Task UnitMovementElementValidator_ShouldFailWithWorngUnit_WithMovementAsync()
        {
            // Arrange
            var movement = File.ReadAllText("MovementJson/MovementWithWrongUnit.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);

            // Act
            var result = await this.ValidateMovementElementCountAsync(movementObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(!result);
        }

        /// <summary>
        /// Validates the wrong owner element in movement should fail.
        /// </summary>
        /// <returns>The Tasks.</returns>
        [TestMethod]
        public async Task OwnerMovementElementValidator_ShouldFailWithWorngOwner_WithMovementAsync()
        {
            // Arrange
            var movement = File.ReadAllText("MovementJson/MovementWithWrongOwner.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);

            // Act
            var result = await this.ValidateMovementElementCountAsync(movementObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Validates the wrong movement typeid element in movement should fail.
        /// </summary>
        /// <returns>The Tasks.</returns>
        [TestMethod]
        public async Task TypeIdMovementElementValidator_ShouldFailWithWorngTypeId_WithMovementAsync()
        {
            // Arrange
            var movement = File.ReadAllText("MovementJson/MovementWithWrongMovementTypeId.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);

            // Act
            var result = await this.ValidateMovementElementCountAsync(movementObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Validates the null event should fail with exception.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "An event Object of null was inappropriately allowed.")]
        public async Task EventElementValidator_ShouldFailWithNullEvent_WithEventAsync()
        {
            Event eventObject = null;
            await this.eventElementValidator.ValidateAsync(eventObject).ConfigureAwait(false);
        }

        /// <summary>
        /// Validate the event with invalid units should fail.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task EventElementValidator_ShouldFailWithInvalidUnit_WithEventAsync()
        {
            // Arrange
            var evenetElement = File.ReadAllText("EventJson/EventWithInvalidUnits.json");
            var eventObject = JsonConvert.DeserializeObject<Event>(evenetElement);

            // Act
            var result = await this.ValidateEventElementCountAsync(eventObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(!result);
        }

        /// <summary>
        /// Validate the event with invalid eventtype should fail.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task EventElementValidator_ShouldFailWithInvalidEventType_WithEventAsync()
        {
            // Arrange
            var evenetElement = File.ReadAllText("EventJson/EventWithInvalidEventType.json");
            var eventObject = JsonConvert.DeserializeObject<Event>(evenetElement);

            // Act
            var result = await this.ValidateEventElementCountAsync(eventObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(!result);
        }

        /// <summary>
        /// Validate the event with invalid owner should fail.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task EventElementValidator_ShouldFailWithInvalidOwner_WithEventAsync()
        {
            // Arrange
            var evenetElement = File.ReadAllText("EventJson/EventWithInvalidOwner.json");
            var eventObject = JsonConvert.DeserializeObject<Event>(evenetElement);

            // Act
            var result = await this.ValidateEventElementCountAsync(eventObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(!result);
        }

        /// <summary>
        /// Validate the valid event element should pass.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task EventElementValidator_ShouldPassWithValidEventDetails_WithEventAsync()
        {
            // Arrange
            var evenetElement = File.ReadAllText("EventJson/ValidEvent.json");
            var eventObject = JsonConvert.DeserializeObject<Event>(evenetElement);

            // Act
            var result = await this.ValidateEventElementCountAsync(eventObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Validates the null movement.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task MovementElementValidator_ValidateDestinationStorageLocationId_WithMovementAsync()
        {
            var newDebtorMovement = new Movement
            {
                MovementDestination = new MovementDestination
                {
                    DestinationNodeId = 14421,
                    DestinationStorageLocationId = 14525,
                    DestinationProductId = "10000002318",
                    DestinationProductTypeId = 64488,
                },
                MovementTypeId = 64486,
                MeasurementUnit = 31,
            };
            Mock<IRepository<CategoryElement>> repos = new Mock<IRepository<CategoryElement>>();
            repos.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(1);
            this.respositoryFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(repos.Object);
            Mock<IRepository<NodeStorageLocation>> repo = new Mock<IRepository<NodeStorageLocation>>();
            repo.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<NodeStorageLocation, bool>>>())).ReturnsAsync(1);
            this.respositoryFactory.Setup(x => x.CreateRepository<NodeStorageLocation>()).Returns(repo.Object);

            var result = await this.movementElementValidator.ValidateAsync(newDebtorMovement).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.IsSuccess);
        }

        /// <summary>
        /// Validates the null movement.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task MovementElementValidator_ValidateDestinationStorageLocationIdWithInvalidStorageLocationId_WithMovementAsync()
        {
            var newDebtorMovement = new Movement
            {
                MovementDestination = new MovementDestination
                {
                    DestinationNodeId = 14421,
                    DestinationStorageLocationId = 1,
                    DestinationProductId = "10000002318",
                    DestinationProductTypeId = 64488,
                },
                MovementTypeId = 64486,
                MeasurementUnit = 31,
            };
            Mock<IRepository<CategoryElement>> repos = new Mock<IRepository<CategoryElement>>();
            repos.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(1);
            this.respositoryFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(repos.Object);
            Mock<IRepository<NodeStorageLocation>> repo = new Mock<IRepository<NodeStorageLocation>>();
            repo.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<NodeStorageLocation, bool>>>())).ReturnsAsync(0);
            this.respositoryFactory.Setup(x => x.CreateRepository<NodeStorageLocation>()).Returns(repo.Object);

            var result = await this.movementElementValidator.ValidateAsync(newDebtorMovement).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.IsSuccess);
        }

        /// <summary>
        /// Validates the null movement.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task MovementElementValidator_ValidateSourceStorageLocationId_WithMovementAsync()
        {
            var newDebtorMovement = new Movement
            {
                MovementSource = new MovementSource
                {
                    SourceNodeId = 14422,
                    SourceStorageLocationId = 14526,
                    SourceProductId = "10000002318",
                    SourceProductTypeId = 64488,
                },
                MovementTypeId = 64486,
                MeasurementUnit = 31,
            };
            Mock<IRepository<CategoryElement>> repos = new Mock<IRepository<CategoryElement>>();
            repos.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(1);
            this.respositoryFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(repos.Object);
            Mock<IRepository<NodeStorageLocation>> repo = new Mock<IRepository<NodeStorageLocation>>();
            repo.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<NodeStorageLocation, bool>>>())).ReturnsAsync(1);
            this.respositoryFactory.Setup(x => x.CreateRepository<NodeStorageLocation>()).Returns(repo.Object);

            var result = await this.movementElementValidator.ValidateAsync(newDebtorMovement).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.IsSuccess);
        }

        /// <summary>
        /// Validates the null movement.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task MovementElementValidator_ValidateSourceStorageLocationIdWithInvalidStorageLocationId_WithMovementAsync()
        {
            var newDebtorMovement = new Movement
            {
                MovementSource = new MovementSource
                {
                    SourceNodeId = 14422,
                    SourceStorageLocationId = 1,
                    SourceProductId = "10000002318",
                    SourceProductTypeId = 64488,
                },
                MovementTypeId = 64486,
                MeasurementUnit = 31,
            };
            Mock<IRepository<CategoryElement>> repos = new Mock<IRepository<CategoryElement>>();
            repos.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(1);
            this.respositoryFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(repos.Object);
            Mock<IRepository<NodeStorageLocation>> repo = new Mock<IRepository<NodeStorageLocation>>();
            repo.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<NodeStorageLocation, bool>>>())).ReturnsAsync(0);
            this.respositoryFactory.Setup(x => x.CreateRepository<NodeStorageLocation>()).Returns(repo.Object);

            var result = await this.movementElementValidator.ValidateAsync(newDebtorMovement).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.IsSuccess);
        }

        private async Task<bool> ValidateInventoryElementCountAsync(InventoryProduct inventoryObject)
        {
            Mock<IRepository<CategoryElement>> repos = new Mock<IRepository<CategoryElement>>();
            repos.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(1);
            this.respositoryFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(repos.Object);
            var result = await this.inventoryElementValidator.ValidateAsync(inventoryObject).ConfigureAwait(false);
            return result.IsSuccess;
        }

        private async Task<bool> ValidateMovementElementCountAsync(Movement movementObject)
        {
            Mock<IRepository<CategoryElement>> repos = new Mock<IRepository<CategoryElement>>();
            repos.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(1);
            this.respositoryFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(repos.Object);
            Mock<IRepository<NodeStorageLocation>> nodeStorageLocationrepos = new Mock<IRepository<NodeStorageLocation>>();
            nodeStorageLocationrepos.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<NodeStorageLocation, bool>>>())).ReturnsAsync(1);
            this.respositoryFactory.Setup(x => x.CreateRepository<NodeStorageLocation>()).Returns(nodeStorageLocationrepos.Object);
            var result = await this.movementElementValidator.ValidateAsync(movementObject).ConfigureAwait(false);
            return result.IsSuccess;
        }

        private async Task<bool> ValidateEventElementCountAsync(Event eventObject)
        {
            Mock<IRepository<CategoryElement>> repos = new Mock<IRepository<CategoryElement>>();
            if (eventObject.Owner1Id > 10000 || eventObject.EventTypeId > 10000)
            {
                repos.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(0);
            }
            else
            {
                repos.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(1);
            }

            this.respositoryFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(repos.Object);
            var result = await this.eventElementValidator.ValidateAsync(eventObject).ConfigureAwait(false);
            return result.IsSuccess;
        }
    }
}
