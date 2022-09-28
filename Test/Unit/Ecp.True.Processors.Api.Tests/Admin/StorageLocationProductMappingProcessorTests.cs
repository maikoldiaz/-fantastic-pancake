// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageLocationProductMappingProcessorTests.cs" company="Microsoft">
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
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Processors.Api.Admin;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The storageLocationProductMappingProcessorTestsTests test class.
    /// </summary>
    [TestClass]
    public class StorageLocationProductMappingProcessorTests
    {
        /// <summary>
        /// The processor.
        /// </summary>
        /// <returns>The task.</returns>
        private StorageLocationProductMappingProcessor processor;

        private TestUnitOfWorkFactory<StorageLocationProductMapping> mockFactory;

        /// <summary>
        /// The Product Repository mock.
        /// </summary>
        private Mock<IRepository<Product>> productRepositoryMock;

        /// <summary>
        /// The StorageLocation IRepository mock.
        /// </summary>
        private Mock<IRepository<StorageLocation>> storageLocationRepositoryMock;

        private Mock<IRepository<LogisticCenter>> logisticCenterRepositoryMock;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockFactory = new TestUnitOfWorkFactory<StorageLocationProductMapping>();
            this.mockFactory.GetUnitOfWorkFactoryMock();

            this.productRepositoryMock = new Mock<IRepository<Product>>();
            this.storageLocationRepositoryMock = new Mock<IRepository<StorageLocation>>();
            this.logisticCenterRepositoryMock = new Mock<IRepository<LogisticCenter>>();

            this.mockFactory.UnitOfWork.Setup(u => u.CreateRepository<Product>())
                .Returns(this.productRepositoryMock.Object);
            this.mockFactory.UnitOfWork.Setup(u => u.CreateRepository<StorageLocation>())
                .Returns(this.storageLocationRepositoryMock.Object);
            this.mockFactory.UnitOfWork.Setup(u => u.CreateRepository<LogisticCenter>())
                .Returns(this.logisticCenterRepositoryMock.Object);

            this.processor = new StorageLocationProductMappingProcessor(this.mockFactory.UnitOfWorkFactory.Object);
        }

        /// <summary>
        /// Create_ShouldThrowException_WhenMappingIsNullAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Create_ShouldThrowException_WhenMappingIsNullAsync()
        {
            // Arrange
            var mappings = default(IEnumerable<StorageLocationProductMapping>);
            var sut = this.GetCreateMethodAsyncFunc(mappings);

            // Act

            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(sut, nameof(mappings)).ConfigureAwait(false);
        }

        /// <summary>
        /// Create_ShouldThrowException_WhenMappingListIsEmptyAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Create_ShouldThrowException_WhenMappingListIsEmptyAsync()
        {
            // Arrange
            var mappings = new List<StorageLocationProductMapping>();
            var sut = this.GetCreateMethodAsyncFunc(mappings);

            // Act

            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(sut, nameof(mappings))
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Create_ShouldReturnInfoWithDuplicatesAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Create_ShouldReturnInfoWithDuplicatesAsync()
        {
            // Arrange
            this.InitializeStorageAndProductRepos();
            var mappings = GetDuplicatedMappings();

            var sut = this.GetCreateMethodAsyncFunc(mappings);

            // Act
            var info = await sut.Invoke().ConfigureAwait(false);

            // Assert
            var duplicatedCount = info.Select(i => i.Status).Count(i => i == EntityInfoCreationStatus.Duplicated);
            Assert.AreEqual(2, duplicatedCount);
            var errorCount = info.Select(i => i.Errors?.ErrorCodes).Count(e => e?.FirstOrDefault()?.Message == Constants.MappingAlreadyExists);
            Assert.AreEqual(2, errorCount);
            var createdCount = info.Count(i => i.Status == EntityInfoCreationStatus.Created);
            Assert.AreEqual(1, createdCount);
            this.mockFactory.UnitOfWork.Verify(u => u.SaveAsync(CancellationToken.None), Times.Once);
        }

        /// <summary>
        /// Create_ShouldReturnInfoWithDuplicatesAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Create_ShouldReturnInfoWithExistingAsync()
        {
            // Arrange
            var mappings = this.InitializeRepoWithExistingMapping();
            var sut = this.GetCreateMethodAsyncFunc(mappings);

            // Act
            var info = await sut.Invoke().ConfigureAwait(false);

            // Assert
            var duplicatedCount = info.Select(i => i.Status).Count(i => i == EntityInfoCreationStatus.Duplicated);
            Assert.AreEqual(1, duplicatedCount);
            var errorCount = info.Select(i => i.Errors?.ErrorCodes).Count(e => e?.FirstOrDefault()?.Message == Constants.MappingAlreadyExists);
            Assert.AreEqual(1, errorCount);
            var createdCount = info.Count(i => i.Status == EntityInfoCreationStatus.Created);
            Assert.AreEqual(0, createdCount);
            this.mockFactory.UnitOfWork.Verify(u => u.SaveAsync(CancellationToken.None), Times.Never);
        }

        /// <summary>
        /// Create_ShouldReturnInfoWithInactiveProductAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Create_ShouldReturnInfoWithInactiveProductAsync()
        {
            // Arrange
            this.InitializeStorageAndProductRepos();
            var mappings = this.InitializeRepoWithInactiveProduct();

            // Act
            var info = await this.processor.CreateStorageLocationProductMappingAsync(mappings)
                    .ConfigureAwait(false);

            // Assert
            var errorMessagesCount = info.Select(i => i.Errors?.ErrorCodes).Count(e => e?.FirstOrDefault()?.Message == Constants.ProductIsInactive);
            Assert.AreEqual(mappings.Count(), errorMessagesCount);
            var errorCount = info.Count(i => i.Status == EntityInfoCreationStatus.Error);
            Assert.AreEqual(mappings.Count(), errorCount);
            this.mockFactory.UnitOfWork.Verify(u => u.SaveAsync(CancellationToken.None), Times.Never);
        }

        /// <summary>
        /// Create_ShouldReturnInfoWithInactiveProductAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Create_ShouldReturnInfoWithInactiveStorageLocationsAsync()
        {
            // Arrange
            this.InitializeStorageAndProductRepos();
            var inactiveStorageLocationMappings = this.InitializeRepoWithInactiveStorageLocation();
            var mappings = inactiveStorageLocationMappings;

            var sut = this.GetCreateMethodAsyncFunc(mappings);

            // Act
            var info = await sut.Invoke().ConfigureAwait(false);

            // Assert
            var errorMessagesCount = info.Select(i => i.Errors?.ErrorCodes).Count(e => e?.FirstOrDefault()?.Message == Constants.StorageLocationIsInactive);
            Assert.AreEqual(inactiveStorageLocationMappings.Count(), errorMessagesCount);
            var errorCount = info.Count(i => i.Status == EntityInfoCreationStatus.Error);
            Assert.AreEqual(mappings.Count(), errorCount);
            this.mockFactory.UnitOfWork.Verify(u => u.SaveAsync(CancellationToken.None), Times.Never);
        }

        /// <summary>
        /// Create_ShouldReturnInfoWithMissingProductAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Create_ShouldReturnInfoWithMissingProductAsync()
        {
            // Arrange
            var mappings = new List<StorageLocationProductMapping> { new StorageLocationProductMapping { ProductId = "1", StorageLocationId = "2" } };

            var info = await this.processor.CreateStorageLocationProductMappingAsync(mappings)
                .ConfigureAwait(false);

            // Act

            // Assert
            var productErrorMessagesCount = info.Select(i => i.Errors?.ErrorCodes).Count(e => e?.FirstOrDefault()?.Message == Constants.ProductDoesNotExist);
            Assert.AreEqual(mappings.Count, productErrorMessagesCount);
            var storageErorMessagesCount = info.Select(i => i.Errors?.ErrorCodes).Count(e => e?.FirstOrDefault()?.Message == Constants.ProductDoesNotExist);
            Assert.AreEqual(mappings.Count, storageErorMessagesCount);

            var errorCount = info.Count(i => i.Status == EntityInfoCreationStatus.Error);
            Assert.AreEqual(mappings.Count, errorCount);
            this.mockFactory.UnitOfWork.Verify(u => u.SaveAsync(CancellationToken.None), Times.Never);
        }

        private static IEnumerable<StorageLocationProductMapping> GetDuplicatedMappings()
        {
            yield return new StorageLocationProductMapping { StorageLocationId = "1", ProductId = "1" };
            yield return new StorageLocationProductMapping { StorageLocationId = "1", ProductId = "1" };
            yield return new StorageLocationProductMapping { StorageLocationId = "1", ProductId = "1" };
        }

        private static void AddEntityToRepo<TEntity>(TEntity entity, Mock<IRepository<TEntity>> repo)
            where TEntity : Entity
        {
            repo.Setup(r =>
                    r.SingleOrDefaultAsync(It.IsAny<Expression<Func<TEntity, bool>>>()))
                .ReturnsAsync(entity);

            repo.Setup(r =>
                    r.QueryAllAsync(It.IsAny<Expression<Func<TEntity, bool>>>()))
                .ReturnsAsync(new List<TEntity> { entity }.AsQueryable());

            repo.Setup(r =>
                    r.QueryAllAsync(It.IsAny<Expression<Func<TEntity, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(new List<TEntity> { entity }.AsQueryable());
        }

        private void InitializeStorageAndProductRepos()
        {
            AddEntityToRepo(new Product() { IsActive = true }, this.productRepositoryMock);

            var storageLocation = new StorageLocation()
            {
                IsActive = true,
                LogisticCenter = new LogisticCenter { IsActive = true },
            };
            AddEntityToRepo(storageLocation, this.storageLocationRepositoryMock);
            this.processor = new StorageLocationProductMappingProcessor(this.mockFactory.UnitOfWorkFactory.Object);
        }

        private Func<Task<IEnumerable<StorageLocationProductMappingInfo>>> GetCreateMethodAsyncFunc(IEnumerable<StorageLocationProductMapping> mappings)
        {
            Func<Task<IEnumerable<StorageLocationProductMappingInfo>>> sut =
                async () => await this.processor.CreateStorageLocationProductMappingAsync(mappings)
                    .ConfigureAwait(false);
            return sut;
        }

        private IEnumerable<StorageLocationProductMapping> InitializeRepoWithExistingMapping()
        {
            var existingMapping = new StorageLocationProductMapping { StorageLocationId = "3", ProductId = "4" };
            this.mockFactory.RepositoryMock.Setup(r =>
                    r.QueryAllAsync(It.IsAny<Expression<Func<StorageLocationProductMapping, bool>>>()))
                .ReturnsAsync(new List<StorageLocationProductMapping> { existingMapping }.AsQueryable());

            var mappings = new List<StorageLocationProductMapping> { existingMapping };
            return mappings;
        }

        private IEnumerable<StorageLocationProductMapping> InitializeRepoWithInactiveProduct()
        {
            var products = new List<Product>() { new Product { IsActive = false, ProductId = "1" } };
            var mappings = new List<StorageLocationProductMapping>();

            products.ForEach(product =>
            {
                var mapping = new StorageLocationProductMapping { ProductId = product.ProductId, StorageLocationId = "1" };
                mappings.Add(mapping);
            });
            products.ForEach(p => AddEntityToRepo(p, this.productRepositoryMock));

            return mappings;
        }

        private IEnumerable<StorageLocationProductMapping> InitializeRepoWithInactiveStorageLocation()
        {
            var storageLocations = new List<StorageLocation> { new StorageLocation() { IsActive = false, StorageLocationId = "1", LogisticCenter = new LogisticCenter { IsActive = true } } };
            var mappings = new List<StorageLocationProductMapping>();

            storageLocations.ForEach(storageLocation =>
            {
                var mapping = new StorageLocationProductMapping
                { ProductId = "10", StorageLocationId = storageLocation.StorageLocationId };

                mappings.Add(mapping);

                AddEntityToRepo(storageLocation, this.storageLocationRepositoryMock);
            });
            storageLocations.ForEach(s => AddEntityToRepo(s, this.storageLocationRepositoryMock));

            return mappings;
        }
    }
}