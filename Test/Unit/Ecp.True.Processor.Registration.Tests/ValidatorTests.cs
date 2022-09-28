// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidatorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Repositories.Tests
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
    using Constant = Ecp.True.Entities.Constants;

    /// <summary>
    /// The Validator tests.
    /// </summary>
    [TestClass]
    public class ValidatorTests
    {
        /// <summary>
        /// The data annotation validator.
        /// </summary>
        private readonly DataAnnotationValidator<InventoryProduct> dataAnnotationValidator = new DataAnnotationValidator<InventoryProduct>();

        /// <summary>
        /// The data annotation validator movement.
        /// </summary>
        private readonly DataAnnotationValidator<Movement> dataAnnotationMovementValidator = new DataAnnotationValidator<Movement>();

        /// <summary>
        /// The data annotation validator.
        /// </summary>
        private ProductValidator<InventoryProduct> productValidator;

        /// <summary>
        /// The movement validator.
        /// </summary>
        private ProductValidator<Movement> movementValidator;

        /// <summary>
        /// The respository factory.
        /// </summary>
        private Mock<IRepositoryFactory> respositoryFactory;

        /// <summary>
        /// The repository mock.
        /// </summary>
        private Mock<IRepository<Product>> repositoryMock;

        /// <summary>
        /// The node validator.
        /// </summary>
        private NodeValidator<InventoryProduct> nodeValidator;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.respositoryFactory = new Mock<IRepositoryFactory>();
            this.repositoryMock = new Mock<IRepository<Product>>();
            this.repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(new Product());
            this.respositoryFactory.Setup(x => x.CreateRepository<Product>()).Returns(this.repositoryMock.Object);
            this.nodeValidator = new NodeValidator<InventoryProduct>(this.respositoryFactory.Object);
            this.productValidator = new ProductValidator<InventoryProduct>(this.respositoryFactory.Object);
            this.movementValidator = new ProductValidator<Movement>(this.respositoryFactory.Object);
        }

        /// <summary>
        /// Validators the should validate with inventory asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DataAnnotationValidator_ShouldValidate_WithInventoryAsync()
        {
            //// Arrange.
            var inventory = File.ReadAllText(@"InventoryJson/SingleInventory.json");

            //// Act.
            var result = await this.dataAnnotationValidator.ValidateAsync(JsonConvert.DeserializeObject<InventoryProduct>(inventory)).ConfigureAwait(false);

            //// Assert.
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Products the validator should validate with inventory asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProductValidator_ShouldValidateWithoutError_WithInventoryAsync()
        {
            //// Arrange.
            var inventory = File.ReadAllText("InventoryJson/SingleInventory.json");

            //// Act
            var result = await this.productValidator.ValidateAsync(JsonConvert.DeserializeObject<InventoryProduct>(inventory)).ConfigureAwait(false);

            //// Assert.
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Products the validator should validate with inventory asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProductValidator_ShouldValidateWhenNotHaveNode_WithInventoryAsync()
        {
            //// Arrange.
            this.repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(default(Product));
            var inventory = File.ReadAllText("InventoryJson/SingleInventory.json");

            //// Act
            var result = await this.productValidator.ValidateAsync(JsonConvert.DeserializeObject<InventoryProduct>(inventory)).ConfigureAwait(false);

            //// Assert.
            var errorMessage = string.Format(CultureInfo.InvariantCulture, Constants.InvalidProductForNodeStorageLocation, 1);
            var errorInfo = new ErrorInfo(errorMessage);

            Assert.AreEqual(errorInfo.Message, result.ErrorInfo[0].Message);
            Assert.AreEqual(errorInfo.Code, result.ErrorInfo[0].Code);
        }

        /// <summary>
        /// Products the validator should validate with movement asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProductValidator_ShouldValidateWhenInvalidProductId_WithMovementAsync()
        {
            //// Arrange.
            this.repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(default(Product));
            var movement = File.ReadAllText("MovementJson/SingleMovement.json");

            //// Act
            var result = await this.movementValidator.ValidateAsync(JsonConvert.DeserializeObject<Movement>(movement)).ConfigureAwait(false);

            //// Assert.
            var errorMessage = string.Format(CultureInfo.InvariantCulture, Constants.InvalidProductForNodeStorageLocation, 2);
            var errorInfo = new ErrorInfo(errorMessage);

            Assert.AreEqual(errorInfo.Message, result.ErrorInfo[0].Message);
            Assert.AreEqual(errorInfo.Code, result.ErrorInfo[0].Code);
        }

        /// <summary>
        /// Products the validator should validate with movement asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProductValidator_ShouldNotThrowError_ForValidProductAsync()
        {
            //// Arrange.
            this.repositoryMock.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(1);
            var movement = File.ReadAllText("MovementJson/SingleMovement.json");
            var entity = JsonConvert.DeserializeObject<Movement>(movement);
            entity.MovementSource.SourceProductId = "some id";
            entity.MovementDestination.DestinationProductId = "some Id";

            //// Act
            this.respositoryFactory.Setup(m => m.CreateRepository<Product>().GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new Product { ProductId = "some Id1" });
            var result = await this.movementValidator.ValidateAsync(entity).ConfigureAwait(false);

            //// Assert.
            Assert.AreEqual(0, result.ErrorInfo.Count);
            this.respositoryFactory.Verify(m => m.CreateRepository<Product>().GetCountAsync(It.IsAny<Expression<Func<Product, bool>>>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// Should not throw error with null source and destination product IDs.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ProductValidator_ShouldThrowError_ForNullSourceAndProductIdAsync()
        {
            //// Arrange.
            this.repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(default(Product));
            var movement = File.ReadAllText("MovementJson/SingleMovement.json");
            var entity = JsonConvert.DeserializeObject<Movement>(movement);
            entity.MovementSource.SourceProductId = null;
            entity.MovementDestination.DestinationProductId = null;

            //// Act
            var result = await this.movementValidator.ValidateAsync(entity).ConfigureAwait(false);

            //// Assert.
            Assert.AreEqual(false, result.IsSuccess);
        }

        /// <summary>
        /// Should throw error for invalid destination product.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ProductValidator_ShouldThrowError_ForInvalidDestinationProductAsync()
        {
            //// Arrange.
            this.repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(default(Product));
            var movement = File.ReadAllText("MovementJson/SingleMovement.json");
            var entity = JsonConvert.DeserializeObject<Movement>(movement);
            entity.MovementSource.SourceProductId = "some id";
            entity.MovementDestination.DestinationProductId = "some Id";

            //// Act
            var result = await this.movementValidator.ValidateAsync(entity).ConfigureAwait(false);

            //// Assert.
            var errorMessage = string.Format(CultureInfo.InvariantCulture, Constants.InvalidProductForNodeStorageLocation, entity.MovementSource.SourceProductId);
            var errorInfo = new ErrorInfo(errorMessage);

            Assert.AreEqual(errorInfo.Message, result.ErrorInfo[0].Message);
            Assert.AreEqual(errorInfo.Code, result.ErrorInfo[0].Code);
        }

        /// <summary>
        /// Products the validator should validate with inventory asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProductValidator_ShouldValidateWithErrorForInvalidValueUnit_WithInventoryAsync()
        {
            //// Arrange.
            var inventory = File.ReadAllText("InventoryJson/InvalidInventory.json");
            var inventoryObject = JsonConvert.DeserializeObject<InventoryProduct>(inventory);

            //// Act.
            var result = await this.productValidator.ValidateAsync(inventoryObject).ConfigureAwait(false);

            //// Assert.
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Products the validator should validate with inventory asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProductValidator_ShouldValidateWithErrorForUnit_WithInventoryAsync()
        {
            var inventory = File.ReadAllText("InventoryJson/InvalidInventoryPercentage.json");
            var inventoryObject = JsonConvert.DeserializeObject<InventoryProduct>(inventory);
            var result = await this.productValidator.ValidateAsync(inventoryObject).ConfigureAwait(false);

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Products the validator should validate with inventory asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProductValidator_ShouldValidateWithErrorForVolumeUnit_WithInventoryAsync()
        {
            //// Arrange.
            var inventory = File.ReadAllText("InventoryJson/InvalidInventoryVolume.json");
            var inventoryObject = JsonConvert.DeserializeObject<InventoryProduct>(inventory);

            //// Act.
            var result = await this.productValidator.ValidateAsync(inventoryObject).ConfigureAwait(false);

            //// Assert.
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Products the validator should validate with inventory asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task NodeValidator_ShouldValidateWithSuccess_WithInventoryAsync()
        {
            //// Arrange.
            var repository = new Mock<IRepository<Node>>();
            repository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Node());
            this.respositoryFactory.Setup(x => x.CreateRepository<Node>()).Returns(repository.Object);

            //// Act.
            var result = await this.nodeValidator.ValidateAsync(new InventoryProduct()).ConfigureAwait(false);

            //// Assert.
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Validators the should validate with inventory asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProductValidator_ShouldValidate_WithInventoryAsync()
        {
            //// Arrange.
            var inventory = File.ReadAllText(@"InventoryJson/InventoryWithoutProducts.json");

            //// Act.
            var result = await this.dataAnnotationValidator.ValidateAsync(JsonConvert.DeserializeObject<InventoryProduct>(inventory)).ConfigureAwait(false);

            //// Assert.
            Assert.IsNotNull(result.ErrorInfo[0].Message, "Los productos del inventario son  obligatorios");
        }

        /// <summary>
        /// Validators the should validate with inventory asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task Validate_ElementId_InventoryAsync()
        {
            //// Arrange.
            var inventory = File.ReadAllText(@"InventoryJson/InventoryWithoutSourceId.json");

            //// Act.
            var result = await this.dataAnnotationValidator.ValidateAsync(JsonConvert.DeserializeObject<InventoryProduct>(inventory)).ConfigureAwait(false);

            //// Assert.
            Assert.IsNotNull(result.ErrorInfo[0].Message,  Constant.SourceSystemNameRequired);
        }

        /// <summary>
        /// Validates the movement type object should pass.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task Validate_ElementId_MovimentAsync()
        {
            // Arrange
            var movement = File.ReadAllText("MovementJson/MovementSourceSystemid.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);

            // Act
            var result = await this.dataAnnotationMovementValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result.ErrorInfo[0].Message, Constant.SourceSystemNameRequired);
        }
    }
}
