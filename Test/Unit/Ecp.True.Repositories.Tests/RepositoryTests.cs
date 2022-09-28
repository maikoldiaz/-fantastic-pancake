// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryTests.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The repository tests.
    /// </summary>
    [TestClass]
    public class RepositoryTests
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private Repository<Category> repository;

        /// <summary>
        /// The mock data access.
        /// </summary>
        private Mock<IDataAccess<Category>> mockDataAccess;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockDataAccess = new Mock<IDataAccess<Category>>();
            this.repository = new Repository<Category>(this.mockDataAccess.Object);
        }

        /// <summary>
        /// Inserts the should insert using data access.
        /// </summary>
        [TestMethod]
        public void Insert_ShouldInsertUsingDataAccess()
        {
            // Arrange
            var category = new Category();
            this.mockDataAccess.Setup(m => m.Insert(It.IsAny<Category>()));

            // Act
            this.repository.Insert(category);

            // Assert
            this.mockDataAccess.Verify(m => m.Insert(It.Is<Category>(c => c == category)), Times.Once);
        }

        /// <summary>
        /// Updates the should update using data access.
        /// </summary>
        [TestMethod]
        public void Update_ShouldUpdateUsingDataAccess()
        {
            // Arrange
            var category = new Category();
            this.mockDataAccess.Setup(m => m.Update(It.IsAny<Category>()));

            // Act
            this.repository.Update(category);

            // Assert
            this.mockDataAccess.Verify(m => m.Update(It.Is<Category>(c => c == category)), Times.Once);
        }

        /// <summary>
        /// Deletes the should delete using data access.
        /// </summary>
        [TestMethod]
        public void Delete_ShouldDeleteUsingDataAccess()
        {
            // Arrange
            var category = new Category();
            this.mockDataAccess.Setup(m => m.Delete(It.IsAny<Category>()));

            // Act
            this.repository.Delete(category);

            // Assert
            this.mockDataAccess.Verify(m => m.Delete(It.Is<Category>(c => c == category)), Times.Once);
        }

        /// <summary>
        /// Deletes the should delete using data access.
        /// </summary>
        [TestMethod]
        public void DeleteAll_ShouldDeleteAllUsingDataAccess()
        {
            // Arrange
            var category = new Category();
            this.mockDataAccess.Setup(m => m.DeleteAll(It.IsAny<IEnumerable<Category>>()));

            // Act
            this.repository.DeleteAll(new List<Category> { category });

            // Assert
            this.mockDataAccess.Verify(m => m.DeleteAll(It.IsAny<IEnumerable<Category>>()), Times.Once);
        }

        /// <summary>
        /// Gets the by identifier asynchronous should get entity from data access.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetByIdAsync_ShouldGetEntityFromDataAccessAsync()
        {
            // Arrange
            var category = new Category();
            this.mockDataAccess.Setup(m => m.GetByIdAsync(1)).ReturnsAsync(category);

            // Act
            var result = await this.repository.GetByIdAsync(1).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(result, category);
            this.mockDataAccess.Verify(m => m.GetByIdAsync(1), Times.Once);
        }

        /// <summary>
        /// Gets the count asynchronous should get entity count from data access asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetCountAsync_ShouldGetEntityCountFromDataAccessAsync()
        {
            // Arrange
            this.mockDataAccess.Setup(m => m.GetCountAsync(x => x.IsActive.GetValueOrDefault())).ReturnsAsync(1);

            // Act
            var result = await this.repository.GetCountAsync(x => x.IsActive.GetValueOrDefault()).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(1, result);
            this.mockDataAccess.Verify(m => m.GetCountAsync(x => x.IsActive.GetValueOrDefault()), Times.Once);
        }

        /// <summary>
        /// Gets the by identifier asynchronous should get entity from data access.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task FirstOrDefaultAsync_ShouldFirstOrDefaultFromDataAccessAsync()
        {
            // Arrange
            var category = new Category();
            this.mockDataAccess.Setup(m => m.FirstOrDefaultAsync(null)).ReturnsAsync(category);

            // Act
            var result = await this.repository.FirstOrDefaultAsync(null).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(category, result);
            this.mockDataAccess.Verify(m => m.FirstOrDefaultAsync(null), Times.Once);
        }

        /// <summary>
        /// Firsts the or default asynchronous with included properties should first or default from data access.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task FirstOrDefaultAsyncWithIncludedProperties_ShouldFirstOrDefaultFromDataAccessAsync()
        {
            // Arrange
            var category = new Category();
            this.mockDataAccess.Setup(m => m.FirstOrDefaultAsync(null, "Elements")).ReturnsAsync(category);

            // Act
            var result = await this.repository.FirstOrDefaultAsync(null, "Elements").ConfigureAwait(false);

            // Assert
            Assert.AreEqual(category, result);
            this.mockDataAccess.Verify(m => m.FirstOrDefaultAsync(null, "Elements"), Times.Once);
        }

        /// <summary>
        /// Gets the by identifier asynchronous should get entity from data access.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task FirstOrDefaultAsync_WithSelector_ShouldReturnEntityFromDataAccessAsync()
        {
            // Arrange
            this.mockDataAccess.Setup(m => m.FirstOrDefaultAsync(x => x.CategoryId == 10, x => x.CategoryId, "Elements")).ReturnsAsync(10);

            // Act
            var result = await this.repository.FirstOrDefaultAsync(x => x.CategoryId == 10, x => x.CategoryId, "Elements").ConfigureAwait(false);

            // Assert
            Assert.AreEqual(10, result);
            this.mockDataAccess.Verify(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<Expression<Func<Category, int>>>(), "Elements"), Times.Once);
        }

        /// <summary>
        /// Gets the by identifier asynchronous should get entity from data access.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task SingleOrDefaultAsync_ShouldSingleOrDefaultFromDataAccessAsync()
        {
            // Arrange
            var category = new Category();
            this.mockDataAccess.Setup(m => m.SingleOrDefaultAsync(null)).ReturnsAsync(category);

            // Act
            var result = await this.repository.SingleOrDefaultAsync(null).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(category, result);
            this.mockDataAccess.Verify(m => m.SingleOrDefaultAsync(null), Times.Once);
        }

        /// <summary>
        /// Gets the by identifier asynchronous should get entity from data access.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task SingleOrDefaultAsyncWithIncludedProperties_ShouldSingleOrDefaultFromDataAccessAsync()
        {
            // Arrange
            var category = new Category();
            this.mockDataAccess.Setup(m => m.SingleOrDefaultAsync(null, "Elements")).ReturnsAsync(category);

            // Act
            var result = await this.repository.SingleOrDefaultAsync(null, "Elements").ConfigureAwait(false);

            // Assert
            Assert.AreEqual(category, result);
            this.mockDataAccess.Verify(m => m.SingleOrDefaultAsync(null, "Elements"), Times.Once);
        }

        /// <summary>
        /// Gets the by identifier asynchronous should get entity from data access.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task SingleOrDefaultAsync_WithSelector_ShouldReturnEntityFromDataAccessAsync()
        {
            // Arrange
            this.mockDataAccess.Setup(m => m.SingleOrDefaultAsync(x => x.CategoryId == 10, x => x.CategoryId, "Elements")).ReturnsAsync(10);

            // Act
            var result = await this.repository.SingleOrDefaultAsync(x => x.CategoryId == 10, x => x.CategoryId, "Elements").ConfigureAwait(false);

            // Assert
            Assert.AreEqual(10, result);
            this.mockDataAccess.Verify(m => m.SingleOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<Expression<Func<Category, int>>>(), "Elements"), Times.Once);
        }

        /// <summary>
        /// Gets the by identifier asynchronous should get entity from data access.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetAllAsync_ShouldReturnAllCategoriesFromDataAccessAsync()
        {
            // Arrange
            var categories = new[] { new Category() };
            this.mockDataAccess.Setup(m => m.GetAllAsync(null, "Elements")).ReturnsAsync(categories);

            // Act
            var result = await this.repository.GetAllAsync(null, "Elements").ConfigureAwait(false);

            // Assert
            Assert.AreEqual(categories, result);
            this.mockDataAccess.Verify(m => m.GetAllAsync(null, "Elements"), Times.Once);
        }

        /// <summary>
        /// Gets the by identifier asynchronous should get entity from data access.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetAllEntitiesAsync_ShouldReturnAllEntitiesFromDataAccessAsync()
        {
            // Arrange
            var elements = new[] { new CategoryElement() };
            this.mockDataAccess.Setup(m => m.GetAllAsync<CategoryElement>(null, "Category")).ReturnsAsync(elements);

            // Act
            var result = await this.repository.GetAllAsync<CategoryElement>(null, "Category").ConfigureAwait(false);

            // Assert
            Assert.AreEqual(elements, result);
            this.mockDataAccess.Verify(m => m.GetAllAsync<CategoryElement>(null, "Category"), Times.Once);
        }

        /// <summary>
        /// Gets the by identifier asynchronous should get entity from data access.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetAllEntitiesAsync_WithSelector_ShouldReturnAllEntitiesFromDataAccessAsync()
        {
            // Arrange
            this.mockDataAccess.Setup(m => m.GetAllAsync(x => x.CategoryId == 10, x => x.CategoryId, "Elements")).ReturnsAsync(new[] { 10 });

            // Act
            var result = await this.repository.GetAllAsync(x => x.CategoryId == 10, x => x.CategoryId, "Elements").ConfigureAwait(false);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(10, result.First());
            this.mockDataAccess.Verify(m => m.GetAllAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<Expression<Func<Category, int>>>(), "Elements"), Times.Once);
        }

        /// <summary>
        /// GetAllSpecificAsync_ShouldThrowExceptionIf.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetAllSpecificAsync_ShouldThrowExceptionIfSpecificationIsNullAsync()
        {
            // Arrange
            this.mockDataAccess.Setup(m => m.GetAllAsync(x => x.CategoryId == 10, x => x.CategoryId, "Elements")).ReturnsAsync(new[] { 10 });

            // Act
            Func<Task<IEnumerable<Category>>> execution = async () => await this.repository.GetAllSpecificAsync(null).ConfigureAwait(false);

            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(execution).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the by identifier asynchronous should get entity from data access.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task OrderByAsync_ShouldOrderBy_FromDataAccessAsync()
        {
            // Arrange
            var elements = new[] { new Category() };
            this.mockDataAccess.Setup(m => m.OrderByAsync<Category>(null, null, null)).ReturnsAsync(elements);

            // Act
            var result = await this.repository.OrderByAsync<Category>(null, null, null).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(elements, result);
            this.mockDataAccess.Verify(m => m.OrderByAsync<Category>(null, null, null), Times.Once);
        }

        /// <summary>
        /// Gets the by identifier asynchronous should get entity from data access.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task OrderByDescendingAsync_ShouldOrderBy_FromDataAccessAsync()
        {
            // Arrange
            var elements = new[] { new Category() };
            this.mockDataAccess.Setup(m => m.OrderByDescendingAsync<Category>(null, null, null)).ReturnsAsync(elements);

            // Act
            var result = await this.repository.OrderByDescendingAsync<Category>(null, null, null).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(elements, result);
            this.mockDataAccess.Verify(m => m.OrderByDescendingAsync<Category>(null, null, null), Times.Once);
        }

        /// <summary>
        /// Queries all asynchronous should query using data access when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task QueryAllAsync_ShouldQueryUsingDataAccess_WhenInvokedAsync()
        {
            // Arrange
            var elements = new[] { new Category() }.AsQueryable();
            this.mockDataAccess.Setup(m => m.QueryAllAsync(null, "Category")).ReturnsAsync(elements);

            // Act
            var result = await this.repository.QueryAllAsync(null, "Category").ConfigureAwait(false);

            // Assert
            Assert.AreEqual(elements, result);
            this.mockDataAccess.Verify(m => m.QueryAllAsync(null, "Category"), Times.Once);
        }

        /// <summary>
        /// Gets the by identifier asynchronous should get entity from data access.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task ExecuteAsync_ShouldExecuteUsingDataAccessAsync()
        {
            // Arrange
            var parameters = new Dictionary<string, object>();
            this.mockDataAccess.Setup(m => m.ExecuteAsync("spName", parameters));

            // Act
            await this.repository.ExecuteAsync("spName", parameters).ConfigureAwait(false);

            // Assert
            this.mockDataAccess.Verify(m => m.ExecuteAsync("spName", It.Is<IDictionary<string, object>>(p => p == parameters)), Times.Once);
        }
    }
}
