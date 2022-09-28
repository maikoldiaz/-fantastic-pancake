// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductValidatorTests.cs" company="Microsoft">
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
    using System.Globalization;
    using System.IO;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.Core.Entities;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Registration;
    using Ecp.True.Processors.Registration.Validation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;

    [TestClass]
    public class ProductValidatorTests
    {
        /// <summary>
        /// The respository factory.
        /// </summary>
        private Mock<IRepositoryFactory> respositoryFactory;

        /// <summary>
        /// The Product movement validator.
        /// </summary>
        private ProductValidator<Movement> productMovementValidator;

        /// <summary>
        /// The Product inventory validator.
        /// </summary>
        private ProductValidator<InventoryProduct> productInventoryValidator;

        /// <summary>
        /// The Product event validator.
        /// </summary>
        private ProductValidator<Event> productEventValidator;

        [TestInitialize]
        public void Initialize()
        {
            this.respositoryFactory = new Mock<IRepositoryFactory>();
            this.productMovementValidator = new ProductValidator<Movement>(this.respositoryFactory.Object);
            this.productInventoryValidator = new ProductValidator<InventoryProduct>(this.respositoryFactory.Object);
            this.productEventValidator = new ProductValidator<Event>(this.respositoryFactory.Object);
        }

        /// <summary>
        /// Validates the null inventory.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "An inventory Object of null was inappropriately allowed.")]
        public async Task InventoryProductValidator_ShouldFailWithNullInventoryUnit_WithInventoryAsync()
        {
            InventoryProduct inventoryObject = null;
            await this.productInventoryValidator.ValidateAsync(inventoryObject).ConfigureAwait(false);
        }

        /// <summary>
        /// Validate the inventory with no products in db should fail.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task InventoryProductValidator_ShouldFailWithNullProductIdinProductRepository_WithInventoryAsync()
        {
            // Arrange
            var inventory = File.ReadAllText("InventoryJson/SingleInventory.json");
            var inventoryObject = JsonConvert.DeserializeObject<InventoryProduct>(inventory);

            // Act
            var result = await this.ValidateInventoryProductIdAsync(0, inventoryObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(!result);
        }

        /// <summary>
        /// Validate the inventory with correct productid should pass.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task InventoryProductValidator_ShouldPassWithCorrectProductId_WithInventoryAsync()
        {
            // Arrange
            var inventory = File.ReadAllText("InventoryJson/SingleInventory.json");
            var inventoryObject = JsonConvert.DeserializeObject<InventoryProduct>(inventory);

            // Act
            var result = await this.ValidateInventoryProductIdAsync(1, inventoryObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Validates the null movement.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "An movement Object of null was inappropriately allowed.")]
        public async Task MovementProductValidator_ShouldFailWithNullMovementUnit_WithMovementAsync()
        {
            Movement movementObject = null;
            await this.productMovementValidator.ValidateAsync(movementObject).ConfigureAwait(false);
        }

        /// <summary>
        /// Validate the movement null sourceId and null destinationid should pass.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task MovementProductValidator_ShouldThrowErrorWithNullSourceIdandDestinationId_WithMovementAsync()
        {
            // Arrange
            var movement = File.ReadAllText("MovementJson/SingleMovement.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            movementObject.MovementSource.SourceProductId = null;
            movementObject.MovementDestination.DestinationProductId = null;

            // Act
            var result = await this.productMovementValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            //// Assert.
            Assert.AreEqual(false, result.IsSuccess);
        }

        /// <summary>
        /// Validate the movement null source and with correct destinationid should pass.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task MovementProductValidator_ShouldFailWithNullSourceIdandCorrectDestinationId_WithMovementAsync()
        {
            // Arrange
            var movement = File.ReadAllText("MovementJson/MovementNullMovementSource.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            Mock<IRepository<Product>> repos = new Mock<IRepository<Product>>();
            this.respositoryFactory.Setup(x => x.CreateRepository<Product>()).Returns(repos.Object);

            // Act
            var result = await this.productMovementValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            //// Assert.
            var errorMessage = string.Format(CultureInfo.InvariantCulture, Constants.InvalidProductForNodeStorageLocation, 3);
            var errorInfo = new ErrorInfo(errorMessage);

            Assert.AreEqual(errorInfo.Message, result.ErrorInfo[0].Message);
            Assert.AreEqual(errorInfo.Code, result.ErrorInfo[0].Code);
        }

        /// <summary>
        /// Validate the movement null source and with wrong destinationid should fail.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task MovementProductValidator_ShouldFailWithNullSourceIdandWrongDestinationId_WithMovementAsync()
        {
            // Arrange
            var movement = File.ReadAllText("MovementJson/MovementNullMovementSource.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);

            // Act
            var result = await this.ValidateMovementProductIdAsync(0, movementObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(!result);
        }

        /// <summary>
        /// Validate the movement with null destination and correct sourceId should pass.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task MovementProductValidator_ShouldPassWithNullDestinationandCorrectSourceId_WithMovementAsync()
        {
            // Arrange
            var movement = File.ReadAllText("MovementJson/MovementDestinationNodeIdNull.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);

            // Act
            var result = await this.ValidateMovementProductIdAsync(1, movementObject).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(false, result);
        }

        /// <summary>
        /// Validate the movement with null destination and wrong sourceId should fail.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task MovementProductValidator_ShouldFailWithNullDestinationandWrongSourceId_WithMovementAsync()
        {
            // Arrange
            var movement = File.ReadAllText("MovementJson/MovementDestinationNodeIdNull.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);

            // Act
            var result = await this.ValidateMovementProductIdAsync(0, movementObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(!result);
        }

        /// <summary>
        /// Validates the null event.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "An Event Object of null was inappropriately allowed.")]
        public async Task EventProductValidator_ShouldFailWithNullEventObject_WithInventoryAsync()
        {
            Event eventObject = null;
            await this.productEventValidator.ValidateAsync(eventObject).ConfigureAwait(false);
        }

        /// <summary>
        /// Validate the Product with invalid sourceProductId should fail.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task EventProductValidator_ShouldFailWithInvalidSourceProductId_WithEventAsync()
        {
            // Arrange
            var eventProduct = File.ReadAllText("EventJson/ValidEvent.json");
            var eventObject = JsonConvert.DeserializeObject<Event>(eventProduct);
            eventObject.SourceProductId = null;

            // Act
            var result = await this.ValidateEventProductAsync(0, eventObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(!result);
        }

        /// <summary>
        /// Validate the Product with valid destinationProductId and valid  sourceProductId should pass.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task EventProductValidator_ShouldPassWithValidDestinationProductIdAndValidSourceProductId_WithEventAsync()
        {
            // Arrange
            var eventProduct = File.ReadAllText("EventJson/ValidEvent.json");
            var eventObject = JsonConvert.DeserializeObject<Event>(eventProduct);

            // Act
            var result = await this.ValidateEventProductAsync(1, eventObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result);
        }

        private async Task<bool> ValidateInventoryProductIdAsync(long productsCount, InventoryProduct inventoryObject)
        {
            Mock<IRepository<Product>> repos = new Mock<IRepository<Product>>();
            repos.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(productsCount);

            this.respositoryFactory.Setup(x => x.CreateRepository<Product>()).Returns(repos.Object);
            var result = await this.productInventoryValidator.ValidateAsync(inventoryObject).ConfigureAwait(false);
            return result.IsSuccess;
        }

        private async Task<bool> ValidateMovementProductIdAsync(long productsCount, Movement movementObject)
        {
            Mock<IRepository<Product>> repos = new Mock<IRepository<Product>>();
            repos.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(productsCount);

            this.respositoryFactory.Setup(x => x.CreateRepository<Product>()).Returns(repos.Object);
            var result = await this.productMovementValidator.ValidateAsync(movementObject).ConfigureAwait(false);
            return result.IsSuccess;
        }

        private async Task<bool> ValidateEventProductAsync(long productsCount, Event eventObject)
        {
            Mock<IRepository<Product>> repos = new Mock<IRepository<Product>>();
            repos.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(productsCount);

            this.respositoryFactory.Setup(x => x.CreateRepository<Product>()).Returns(repos.Object);
            var result = await this.productEventValidator.ValidateAsync(eventObject).ConfigureAwait(false);
            return result.IsSuccess;
        }
    }
}
