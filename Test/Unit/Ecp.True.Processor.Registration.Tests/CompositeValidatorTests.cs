// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompositeValidatorTests.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Registration.Interfaces;
    using Ecp.True.Processors.Registration.Validation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;

    [TestClass]
    public class CompositeValidatorTests
    {
        /// <summary>
        /// The respository factory.
        /// </summary>
        private Mock<IRepositoryFactory> respositoryFactory;

        /// <summary>
        /// The Composite movement validator.
        /// </summary>
        private CompositeValidator<Movement> compositeMovementValidator;

        /// <summary>
        /// The Composite inventory validator.
        /// </summary>
        private CompositeValidator<InventoryProduct> compositeInventoryValidator;

        /// <summary>
        /// The IValidator movement.
        /// </summary>
        private IValidator<Movement>[] validatorMovement;

        /// <summary>
        /// The Ivalidator inventory.
        /// </summary>
        private IValidator<InventoryProduct>[] validatorInventory;

        [TestInitialize]
        public void Initialize()
        {
            this.respositoryFactory = new Mock<IRepositoryFactory>();
            this.validatorMovement = new IValidator<Movement>[1] { new DataAnnotationValidator<Movement>() };
            this.validatorInventory = new IValidator<InventoryProduct>[2] { new DataAnnotationValidator<InventoryProduct>(), new NodeValidator<InventoryProduct>(this.respositoryFactory.Object) };
            this.compositeMovementValidator = new CompositeValidator<Movement>(this.validatorMovement);
            this.compositeInventoryValidator = new CompositeValidator<InventoryProduct>(this.validatorInventory);
        }

        /// <summary>
        /// Validates the children (valid dataannotaion validator and invalid othervalidator) with inventory should fail.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task CompositeValidator_ShouldFailWithValidDataAnnotationAndInvalidOtherValidationChildrenInventory_WithValidateAsync()
        {
            // Arrange
            var inventory = File.ReadAllText("InventoryJson/SingleInventory.json");
            var inventoryObject = JsonConvert.DeserializeObject<InventoryProduct>(inventory);
            var repository = new Mock<IRepository<Node>>();
            repository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Node() { NodeId = 1 });
            this.respositoryFactory.Setup(x => x.CreateRepository<Node>()).Returns(repository.Object);

            // Act
            var result = await this.compositeInventoryValidator.ValidateAsync(inventoryObject).ConfigureAwait(false);

            // Arrange
            Assert.IsTrue(!result.IsSuccess);
        }

        /// <summary>
        /// Validates the children (valid dataannotaion validator and valid othervalidator) with inventory should pass.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task CompositeValidator_ShouldPassWithValidDataAnnotationAndValidOtherValidationChildrenInventory_WithValidateAsync()
        {
            // Arrange
            var inventory = File.ReadAllText("InventoryJson/SingleInventory.json");
            var inventoryObject = JsonConvert.DeserializeObject<InventoryProduct>(inventory);
            var repository = new Mock<IRepository<Node>>();
            var nodeStorageLocations = new List<NodeStorageLocation>();
            var nodeStorageLocation = new NodeStorageLocation { NodeStorageLocationId = 0, IsActive = true, Name = "SL" };
            var storageLocationProduct1 = new StorageLocationProduct { ProductId = "1" };
            storageLocationProduct1.Product = new Product() { ProductId = "1" };
            var storageLocationProduct2 = new StorageLocationProduct { ProductId = "2" };
            storageLocationProduct2.Product = new Product() { ProductId = "2" };
            var storageLocationProduct3 = new StorageLocationProduct { ProductId = "3" };
            storageLocationProduct3.Product = new Product() { ProductId = "3" };
            nodeStorageLocation.Products.Add(storageLocationProduct1);
            nodeStorageLocation.Products.Add(storageLocationProduct2);
            nodeStorageLocation.Products.Add(storageLocationProduct3);
            nodeStorageLocations.Add(nodeStorageLocation);
            var nodeStorageLocationRepoMock = new Mock<IRepository<NodeStorageLocation>>();
            repository.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(1);
            this.respositoryFactory.Setup(x => x.CreateRepository<Node>()).Returns(repository.Object);
            nodeStorageLocationRepoMock.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<NodeStorageLocation, bool>>>())).ReturnsAsync(1);
            this.respositoryFactory.Setup(x => x.CreateRepository<NodeStorageLocation>()).Returns(nodeStorageLocationRepoMock.Object);

            // Act
            var result = await this.compositeInventoryValidator.ValidateAsync(inventoryObject).ConfigureAwait(false);

            // Arrange
            Assert.IsTrue(result.IsSuccess);
        }

        /// <summary>
        /// Validates the data annotation inventory children with null inventory should fail.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task CompositeValidator_ShouldFailWithNullInventoryDataAnnotationChildrenInventory_WithValidateAsync()
        {
            // Arrange
            InventoryProduct inventoryObject = new InventoryProduct();

            // Act
            var result = await this.compositeInventoryValidator.ValidateAsync(inventoryObject).ConfigureAwait(false);

            // Arrange
            Assert.IsTrue(!result.IsSuccess);
        }

        /// <summary>
        /// Validates the data annotation movement children with movement should pass.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task CompositeValidator_ShouldPassWithValidDataAnnotationChildrenMovement_WithValidateAsync()
        {
            // Arrange
            var movement = File.ReadAllText("MovementJson/SingleMovement.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            movementObject.SourceSystemId = 165;

            // Act
            var result = await this.compositeMovementValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            // Arrange
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
