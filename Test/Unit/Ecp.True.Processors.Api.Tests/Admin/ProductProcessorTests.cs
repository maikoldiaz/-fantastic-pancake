// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductProcessorTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Tests.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Runtime.Serialization;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Api.Admin;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The productProcessorTests test class.
    /// </summary>
    [TestClass]
    public class ProductProcessorTests
    {
        private static readonly Product TestProduct = new Product()
        {
            ProductId = "123456",
            Name = "Crudo Boyacá",
            IsActive = true,
        };

        /// <summary>
        /// The movement repository.
        /// </summary>
        private readonly Mock<IRepository<Movement>> movementRepoMock = new Mock<IRepository<Movement>>();

        /// <summary>
        /// The storage location mapping repository.
        /// </summary>
        private readonly Mock<IRepository<StorageLocationProductMapping>> mappingRepoMock = new Mock<IRepository<StorageLocationProductMapping>>();

        /// <summary>
        /// The processor.
        /// </summary>
        private ProductProcessor processor;

        /// <summary>
        /// The unit of work repository.
        /// </summary>
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;

        /// <summary>
        /// The unit of work test factory.
        /// </summary>
        private TestUnitOfWorkFactory<Product> factory;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.factory = new TestUnitOfWorkFactory<Product>();
            this.unitOfWorkFactory = this.factory.GetUnitOfWorkFactoryMock();

            this.processor = new ProductProcessor(this.factory.UnitOfWorkFactory.Object);

            this.InitializeRepositoryMock(TestProduct);
        }

        /// <summary>
        /// ShouldThrowArgumentNullExceptionIfProductIsNullAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CreateProductAsync_ShouldThrowArgumentNullException_WhenProductIsNullAsync()
        {
            // Execute
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await this.processor
                .CreateProductAsync(null)
                .ConfigureAwait(false))
                    .ConfigureAwait(false);
        }

        /// <summary>
        /// ShouldSaveProductIfDoesn'tExist.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CreateProductAsync_ShouldSaveProduct_WhenProductIsNewAsync()
        {
            // Prepare
            this.InitializeRepositoryMock(default(Product));

            // Execute
            await this.processor
                .CreateProductAsync(TestProduct).ConfigureAwait(false);

            // Assert
            this.AssertQueryProductOnce();

            this.AssertSaveUnitOfWork(Times.Once);
        }

        /// <summary>
        /// ShouldSaveProductIfDoesn'tExist.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CreateProductAsync_ShouldReturnInfoWithMessage_WhenProductExistingAsync()
        {
            // Execute
            var result = await this.processor.CreateProductAsync(TestProduct)
                .ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result.HasError);
            this.AssertQueryProductOnce();
            this.AssertSaveUnitOfWork(Times.Never);
        }

        /// <summary>
        /// updateShouldThrowExceptionIfProductIsNullAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateProductAsync_ShouldThrowException_WhenProductIsNullAsync()
        {
            // Execute
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await this.processor
                    .UpdateProductAsync(null, TestProduct.ProductId)
                    .ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        /// <summary>
        /// ShouldSaveProductIfDoesn'tExist.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateProductAsync_ShouldThrowExceptionProduct_WhenDoesNotExistsAsync()
        {
            // Prepare
            this.InitializeRepositoryMock(default(Product));

            // Execute
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
                    async () => await this.processor
                        .UpdateProductAsync(TestProduct, TestProduct.ProductId)
                        .ConfigureAwait(false),
                    Constants.ProductDoesNotExist)
                .ConfigureAwait(false);

            // Assert
            this.AssertQueryProductOnce();

            this.AssertSaveUnitOfWork(Times.Never);
        }

        /// <summary>
        /// ShouldThrowExceptionIfProductHasMovements.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateProductAsync_ShouldNotUpdateIdAsync()
        {
            // Prepare
            var updatedProduct = GetTestUpdatedProduct();

            this.InitializeMovementRepository(default);
            this.InitializeMappingRepository(default);

            // Execute
            await this.processor
                .UpdateProductAsync(updatedProduct, TestProduct.ProductId)
                .ConfigureAwait(false);

            // Assert
            this.AssertQueryProductOnce();
            this.factory.RepositoryMock
                .Verify(
                    r => r.Update(TestProduct),
                    Times.Once);

            Assert.AreNotEqual(updatedProduct.ProductId, TestProduct.ProductId);
            Assert.AreEqual(updatedProduct.Name, TestProduct.Name);
            Assert.AreEqual(updatedProduct.IsActive, TestProduct.IsActive);

            this.AssertSaveUnitOfWork(Times.Once);
        }

        /// <summary>
        /// ShouldUpdateIfProductExistsAndHasNoMovementsAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateProductAsync_ShouldUpdateIfProductExistsAndHasNoMovementsAsync()
        {
            // Prepare
            this.InitializeMovementRepository(default);
            this.InitializeMappingRepository(default);
            var updatedProduct = GetTestUpdatedProduct();

            // Execute
            await this.processor.UpdateProductAsync(updatedProduct, TestProduct.ProductId)
                .ConfigureAwait(false);

            // Assert
            this.AssertQueryProductOnce();
            this.factory.RepositoryMock
                .Verify(
                    r => r.Update(TestProduct),
                    Times.Once);

            Assert.AreNotEqual(updatedProduct.ProductId, TestProduct.ProductId);
            Assert.AreEqual(updatedProduct.Name, TestProduct.Name);
            Assert.AreEqual(updatedProduct.IsActive, TestProduct.IsActive);

            this.AssertSaveUnitOfWork(Times.Once);
        }

        /// <summary>
        /// DeleteProductAsync_ShouldThrowExceptionIfProductDoesNotExistAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task DeleteProductAsync_ShouldThrowExceptionIfProductDoesNotExistAsync()
        {
            // Arrange
            this.InitializeMovementRepository(default);
            this.InitializeMappingRepository(default);

            var product = GetTestUpdatedProduct();

            this.factory.RepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(
                    It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(default(Product));

            // Act
            Func<Task> sut = async () => await this.processor.DeleteProductAsync(product.ProductId).ConfigureAwait(false);

            // Assert
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(sut, Constants.ProductDoesNotExist).ConfigureAwait(false);
        }

        /// <summary>
        /// DeleteProductAsync_ShouldThrowExceptionIfProductHasMovementsAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task DeleteProductAsync_ShouldThrowExceptionIfProductHasMovementsAsync()
        {
            // Arrange
            this.InitializeMovementRepository(this.GetTestMovements());
            this.InitializeMappingRepository(default);

            var product = GetTestUpdatedProduct();

            // Act
            Func<Task> sut = async () => await this.processor.DeleteProductAsync(product.ProductId).ConfigureAwait(false);

            // Assert
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(sut, Constants.ProductWithMovements).ConfigureAwait(false);
        }

        /// <summary>
        /// DeleteProductAsync_ShouldThrowExceptionIfProductHasMappingsAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task DeleteProductAsync_ShouldThrowExceptionIfProductHasMappingsAsync()
        {
            // Arrange
            this.InitializeMovementRepository(default);
            this.InitializeMappingRepository(this.GetTestMappings());

            var product = GetTestUpdatedProduct();

            // Act
            Func<Task> sut = async () => await this.processor.DeleteProductAsync(product.ProductId).ConfigureAwait(false);

            // Assert
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(sut, Constants.ProductWithMappings).ConfigureAwait(false);
        }

        /// <summary>
        /// DeleteProductAsync_ShouldThrowExceptionIfProductHasConfigurationsAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task DeleteProductAsync_ShouldThrowExceptionIfProductHasConfigurationsAsync()
        {
            // Arrange
            this.InitializeMovementRepository(default);
            this.InitializeMappingRepository(default);

            this.InitializeUnitOfWorkWithSqlException();

            var product = GetTestUpdatedProduct();

            // Act
            Func<Task> sut = async () => await this.processor.DeleteProductAsync(product.ProductId).ConfigureAwait(false);

            // Assert
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(sut, Constants.ProductWithConfigurations).ConfigureAwait(false);
        }

        /// <summary>
        /// DeleteProductAsync_ShouldThrowExceptionIfProductHasConfigurationsAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task DeleteProductAsync_ShouldDeleteProductWithoutMovementsMappingOrConfigurationsAsync()
        {
            // Arrange
            this.InitializeMovementRepository(default);
            this.InitializeMappingRepository(default);

            // Act
            await this.processor.DeleteProductAsync(TestProduct.ProductId).ConfigureAwait(false);

            // Assert
            this.factory.RepositoryMock.Verify(r => r.Delete(TestProduct), Times.Once);
            this.factory.UnitOfWork.Verify(r => r.SaveAsync(CancellationToken.None), Times.Once);
        }

        /// <summary>
        /// Gets the test updated product.
        /// </summary>
        /// <returns>The product.</returns>
        private static Product GetTestUpdatedProduct()
        {
            var updatedProduct = new Product
            {
                ProductId = "96325",
                Name = "Crudo Tunja",
                IsActive = false,
            };
            return updatedProduct;
        }

        /// <summary>
        /// Gets the test mappings.
        /// </summary>
        /// <returns>The test mapping.</returns>
        private IEnumerable<StorageLocationProductMapping> GetTestMappings()
        {
            yield return new StorageLocationProductMapping
            {
                StorageLocationProductMappingId = 1,
            };
        }

        /// <summary>
        /// Initialize unit of work with sql exception.
        /// </summary>
        private void InitializeUnitOfWorkWithSqlException()
        {
            var exception = FormatterServices.GetUninitializedObject(typeof(DbUpdateException))
                as DbUpdateException;

            this.factory.UnitOfWork
                .Setup(u => u.SaveAsync(CancellationToken.None))
                .ThrowsAsync(exception);
        }

        /// <summary>
        /// Get test movements.
        /// </summary>
        /// <returns>The test movements.</returns>
        private IEnumerable<Movement> GetTestMovements()
        {
            yield return new Movement
            {
                MovementSource = new MovementSource
                {
                    SourceProductId = TestProduct.ProductId,
                },
                MovementDestination = new MovementDestination
                {
                    DestinationProductId = TestProduct.ProductId,
                },
            };
        }

        /// <summary>
        /// Assert on the unit of work save method.
        /// </summary>
        /// <param name="times">The times this method should be called.</param>
        private void AssertSaveUnitOfWork(Func<Times> times)
        {
            this.factory.UnitOfWork.Verify(u => u.SaveAsync(CancellationToken.None), times);
        }

        /// <summary>
        /// Assert the query product once.
        /// </summary>
        private void AssertQueryProductOnce()
        {
            this.factory.RepositoryMock
                .Verify(
                    r => r.SingleOrDefaultAsync(
                        It.IsAny<Expression<Func<Product, bool>>>()),
                    Times.Once);
        }

        /// <summary>
        /// Initializes the repository mock.
        /// </summary>
        /// <param name="product">The product.</param>
        private void InitializeRepositoryMock(Product product)
        {
            this.factory.RepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(
                    It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(product);

            this.factory.UnitOfWork
                .Setup(u => u.CreateRepository<Movement>())
                .Returns(this.movementRepoMock.Object);

            this.factory.UnitOfWork
                .Setup(u => u.CreateRepository<StorageLocationProductMapping>())
                .Returns(this.mappingRepoMock.Object);
        }

        /// <summary>
        /// initializes the movement repository.
        /// </summary>
        /// <param name="movements">The movements.</param>
        private void InitializeMovementRepository(IEnumerable<Movement> movements)
        {
            this.movementRepoMock
                .Setup(m => m.QueryAllAsync(
                    It.IsAny<Expression<Func<Movement, bool>>>()))
                .ReturnsAsync(movements?.AsQueryable() ?? new List<Movement>().AsQueryable());
        }

        /// <summary>
        /// Initializes the mappings repository.
        /// </summary>
        /// <param name="mappings">The mappings.</param>
        private void InitializeMappingRepository(IEnumerable<StorageLocationProductMapping> mappings)
        {
            this.mappingRepoMock
                .Setup(m => m.QueryAllAsync(
                    It.IsAny<Expression<Func<StorageLocationProductMapping, bool>>>()))
                .ReturnsAsync(mappings?.AsQueryable() ?? new List<StorageLocationProductMapping>().AsQueryable());
        }
    }
}