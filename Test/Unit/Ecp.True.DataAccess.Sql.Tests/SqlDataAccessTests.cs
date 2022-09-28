// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlDataAccessTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Sql.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Sql.Interfaces;
    using Ecp.True.Entities;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Host.Core;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The SQL data access tests.
    /// </summary>
    [TestClass]
    public class SqlDataAccessTests
    {
        /// <summary>
        /// The category name.
        /// </summary>
        private const string CategoryName = "Segments";

        /// <summary>
        /// The data context.
        /// </summary>
        private SqlDataContext dataContext;

        /// <summary>
        /// The data access.
        /// </summary>
        private SqlDataAccess<Category> dataAccess;

        /// <summary>
        /// The business context.
        /// </summary>
        private IBusinessContext businessContext;

        /// <summary>
        /// The SQL token provider.
        /// </summary>
        private Mock<ISqlTokenProvider> sqlTokenProvider;

        /// <summary>
        /// The mock audit service.
        /// </summary>
        private Mock<IAuditService> mockAuditService;

        /// <summary>
        /// The sqldatacontext.
        /// </summary>
        private Mock<ISqlDataContext> mockSqlDataContext;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockAuditService = new Mock<IAuditService>();
            this.businessContext = new BusinessContext();
            this.sqlTokenProvider = new Mock<ISqlTokenProvider>();
            this.mockSqlDataContext = new Mock<ISqlDataContext>();
            this.mockSqlDataContext.Setup(m => m.SetAccessToken()).Verifiable();
            this.mockAuditService.Setup(m => m.GetAuditLogs(It.IsAny<ChangeTracker>())).Returns(new List<AuditLog>());
            var options = new DbContextOptionsBuilder<SqlDataContext>()
                                    .UseInMemoryDatabase(databaseName: $"InMemoryDatabase_{Guid.NewGuid()}")
                                    .Options;
            this.dataContext = new SqlDataContext(options, this.mockAuditService.Object, this.businessContext, this.sqlTokenProvider.Object);
            this.dataAccess = new SqlDataAccess<Category>(this.dataContext);
        }

        /// <summary>
        /// Adds the category should add category to database.
        /// </summary>
        /// <returns>
        /// A Task.
        /// </returns>
        [TestMethod]
        public async Task AddCategory_ShouldAddCategory_ToDatabaseAsync()
        {
            // Arrange
            var category = this.GetNewCategory();

            // Act
            this.dataAccess.Insert(category);
            var rows = await this.dataContext.SaveAsync(CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(1, rows);

            var result = await this.dataAccess.GetByIdAsync(category.CategoryId).ConfigureAwait(false);
            Assert.AreEqual(result.Name, category.Name);
            Assert.AreEqual("System", result.CreatedBy);
            Assert.IsNotNull(result.CreatedDate);

            this.mockAuditService.Verify(m => m.GetAuditLogs(It.IsAny<ChangeTracker>()), Times.Once);

            await this.CleanupEntitiesAsync(category).ConfigureAwait(false);
        }

        /// <summary>
        /// Update category should update category in database.
        /// </summary>
        /// <returns>
        /// A Task.
        /// </returns>
        [TestMethod]
        public async Task UpdateCategory_ShouldUpdateCategory_InDatabaseAsync()
        {
            // Arrange
            var category = this.GetNewCategory();
            this.dataAccess.Insert(category);
            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);
            var result = await this.dataAccess.GetByIdAsync(category.CategoryId).ConfigureAwait(false);
            result.Name = "New Category";

            // Act
            this.dataAccess.Update(result);
            var rows = await this.dataContext.SaveAsync(CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(1, rows);

            result = await this.dataAccess.GetByIdAsync(category.CategoryId).ConfigureAwait(false);
            Assert.AreEqual("New Category", result.Name);
            Assert.AreEqual("System", result.LastModifiedBy);
            Assert.IsNotNull(result.LastModifiedDate);

            await this.CleanupEntitiesAsync(category).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete the category should delete category from database.
        /// </summary>
        /// <returns>
        /// A Task.
        /// </returns>
        [TestMethod]
        public async Task DeleteCategory_ShouldDeleteCategory_FromDatabaseAsync()
        {
            // Arrange
            var category = this.GetNewCategory();
            this.dataAccess.Insert(category);
            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            this.dataAccess.Delete(category);
            var rows = await this.dataContext.SaveAsync(CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(1, rows);
            Assert.AreEqual("System", category.LastModifiedBy);
            Assert.IsNotNull(category.LastModifiedDate);

            var result = await this.dataAccess.GetByIdAsync(category.CategoryId).ConfigureAwait(false);
            Assert.IsNull(result);
        }

        /// <summary>
        /// Delete the category should delete category from database.
        /// </summary>
        /// <returns>
        /// A Task.
        /// </returns>
        [TestMethod]
        public async Task DeleteAllCategory_ShouldDeleteAllCategory_FromDatabaseAsync()
        {
            // Arrange
            var category = this.GetNewCategory();
            this.dataAccess.Insert(category);
            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            this.dataAccess.DeleteAll(new List<Category> { category });
            var rows = await this.dataContext.SaveAsync(CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(1, rows);
            Assert.AreEqual("System", category.LastModifiedBy);
            Assert.IsNotNull(category.LastModifiedDate);

            var result = await this.dataAccess.GetByIdAsync(category.CategoryId).ConfigureAwait(false);
            Assert.IsNull(result);
        }

        /// <summary>
        /// Gets the category by identifier should get category from database.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetCategoryById_ShouldGetCategory_FromDatabaseAsync()
        {
            // Arrange
            var category = this.GetNewCategory();
            this.dataAccess.Insert(category);
            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            var result = await this.dataAccess.GetByIdAsync(category.CategoryId).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Name, CategoryName);

            await this.CleanupEntitiesAsync(category).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the category by identifier should get category from database.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetAllCategories_ShouldGetAllCategories_FromDatabaseAsync()
        {
            // Arrange
            var segments = this.GetNewCategory("Segments", true);
            var operators = this.GetNewCategory("Operators", false);
            var nodetypes = this.GetNewCategory("NodeTypes", true);

            this.dataAccess.Insert(segments);
            this.dataAccess.Insert(operators);
            this.dataAccess.Insert(nodetypes);
            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);
            this.dataAccess = new SqlDataAccess<Category>(this.dataContext);

            // Act
            var result = await this.dataAccess.GetAllAsync(x => x.IsActive == true).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());

            await this.CleanupEntitiesAsync(segments, operators, nodetypes).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the category by identifier should get category from database.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetCountAsync_ShouldGetCount_FromDatabaseAsync()
        {
            // Arrange
            var segments = this.GetNewCategory("Segments", true);
            var operators = this.GetNewCategory("Operators", false);
            var nodetypes = this.GetNewCategory("NodeTypes", true);

            this.dataAccess.Insert(segments);
            this.dataAccess.Insert(operators);
            this.dataAccess.Insert(nodetypes);
            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);
            this.dataAccess = new SqlDataAccess<Category>(this.dataContext);

            // Act
            var result = await this.dataAccess.GetCountAsync(x => x.IsActive == true).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(2, result);

            await this.CleanupEntitiesAsync(segments, operators, nodetypes).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the count asynchronous should get count without predicates asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetCountAsync_ShouldGetCount_WithoutPredicatesAsync()
        {
            // Arrange
            var segments = this.GetNewCategory("Segments", true);
            var operators = this.GetNewCategory("Operators", false);
            var nodetypes = this.GetNewCategory("NodeTypes", true);

            this.dataAccess.Insert(segments);
            this.dataAccess.Insert(operators);
            this.dataAccess.Insert(nodetypes);
            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);
            this.dataAccess = new SqlDataAccess<Category>(this.dataContext);

            // Act
            var result = await this.dataAccess.GetCountAsync(null).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(3, result);

            await this.CleanupEntitiesAsync(segments, operators, nodetypes).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the category by identifier should get category from database.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task FirstOrDefaultAsync_ShouldGetFirstCategory_FromDatabaseAsync()
        {
            // Arrange
            var segments = this.GetNewCategory("Segments", true);
            var operators = this.GetNewCategory("Operators", false);
            var nodetypes = this.GetNewCategory("NodeTypes", true);

            this.dataAccess.Insert(segments);
            this.dataAccess.Insert(operators);
            this.dataAccess.Insert(nodetypes);
            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            var result = await this.dataAccess.FirstOrDefaultAsync(x => x.IsActive == true && x.Name == "Segments").ConfigureAwait(false);

            // Assert
            Assert.AreEqual("Segments", result.Name);
            await this.CleanupEntitiesAsync(segments, operators, nodetypes).ConfigureAwait(false);
        }

        /// <summary>
        /// Firsts the or default asynchronous should get first category include child items from database asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task FirstOrDefaultAsync_ShouldGetFirstCategoryIncludeChildItems_FromDatabaseAsync()
        {
            // Arrange
            var segments = this.GetNewCategory("Segments", true);
            var operators = this.GetNewCategory("Operators", false);
            var nodetypes = this.GetNewCategory("NodeTypes", true);
            var categoryElement = new CategoryElement { CategoryId = 1, Name = "Segments1" };
            segments.Elements.Add(categoryElement);

            this.dataAccess.Insert(segments);
            this.dataAccess.Insert(operators);
            this.dataAccess.Insert(nodetypes);
            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            var result = await this.dataAccess.FirstOrDefaultAsync(x => x.IsActive == true, "Elements").ConfigureAwait(false);

            // Assert
            Assert.AreEqual("Segments", result.Name);
            Assert.IsNotNull(result.Elements);
            Assert.IsTrue(result.Elements.Count > 0);
            Assert.IsTrue(result.Elements.First().Name == categoryElement.Name);
            await this.CleanupEntitiesAsync(segments, operators, nodetypes).ConfigureAwait(false);
        }

        /// <summary>
        /// Firsts the or default asynchronous should get first without predicates from database asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task FirstOrDefaultAsync_ShouldGetFirstWithoutPredicates_FromDatabaseAsync()
        {
            // Arrange
            var segments = this.GetNewCategory("Segments", true);
            this.dataAccess.Insert(segments);

            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            var result = await this.dataAccess.FirstOrDefaultAsync(null).ConfigureAwait(false);

            // Assert
            Assert.AreEqual("Segments", result.Name);
            Assert.IsNotNull(result.Elements);

            await this.CleanupEntitiesAsync(segments).ConfigureAwait(false);
        }

        /// <summary>
        /// Firsts the or default asynchronous should get first without predicates from database asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task FirstOrDefaultAsync_ShouldGetFirstWithSelector_FromDatabaseAsync()
        {
            // Arrange
            var segments = this.GetNewCategory("Segments", true);
            this.dataAccess.Insert(segments);

            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            var result = await this.dataAccess.FirstOrDefaultAsync(null, c => c.Name).ConfigureAwait(false);

            // Assert
            Assert.AreEqual("Segments", result);

            await this.CleanupEntitiesAsync(segments).ConfigureAwait(false);
        }

        /// <summary>
        /// Queries all asynchronous should get first category include child items from database asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task QueryAllAsync_ShouldGetFirstCategoryIncludeChildItems_FromDatabaseAsync()
        {
            // Arrange
            var segments = this.GetNewCategory("Segments", true);
            var operators = this.GetNewCategory("Operators", false);
            var nodetypes = this.GetNewCategory("NodeTypes", true);
            var categoryElement = new CategoryElement { CategoryId = 1, Name = "Segments1" };
            segments.Elements.Add(categoryElement);

            this.dataAccess.Insert(segments);
            this.dataAccess.Insert(operators);
            this.dataAccess.Insert(nodetypes);
            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);
            this.dataAccess = new SqlDataAccess<Category>(this.dataContext);

            // Act
            var result = await this.dataAccess.QueryAllAsync(x => x.IsActive == true).ConfigureAwait(false);
            var segmentCategory = result.FirstOrDefault(x => x.Name == "Segments");

            // Assert
            Assert.IsNotNull(segmentCategory);
            Assert.IsNotNull(segmentCategory.Elements);
            Assert.IsTrue(segmentCategory.Elements.Count > 0);
            Assert.IsTrue(segmentCategory.Elements.First().Name == categoryElement.Name);
            await this.CleanupEntitiesAsync(segments, operators, nodetypes).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets all asynchronous by entity should get first category include child items from database asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetAllAsync_ByEntity_ShouldGetFirstCategoryIncludeChildItems_FromDatabaseAsync()
        {
            // Arrange
            var segments = this.GetNewCategory("Segments", true);
            var operators = this.GetNewCategory("Operators", false);
            var nodetypes = this.GetNewCategory("NodeTypes", true);
            var categoryElement = new CategoryElement { CategoryId = 1, Name = "Segments1" };
            segments.Elements.Add(categoryElement);

            this.dataAccess.Insert(segments);
            this.dataAccess.Insert(operators);
            this.dataAccess.Insert(nodetypes);
            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);
            this.dataAccess = new SqlDataAccess<Category>(this.dataContext);

            // Act
            var result = await this.dataAccess.GetAllAsync<Category>(x => x.IsActive == true, "Elements").ConfigureAwait(false);
            var segmentCategory = result.FirstOrDefault(x => x.Name == "Segments");

            // Assert
            Assert.IsNotNull(segmentCategory);
            Assert.IsNotNull(segmentCategory.Elements);
            Assert.IsTrue(segmentCategory.Elements.Count > 0);
            Assert.IsTrue(segmentCategory.Elements.First().Name == categoryElement.Name);
            await this.CleanupEntitiesAsync(segments, operators, nodetypes).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets all asynchronous by entity should get first category include child items from database asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetAllAsync_ByEntity_ShouldGetFirstCategoryIncludeChildItems_WithSelector_FromDatabaseAsync()
        {
            // Arrange
            var segments = this.GetNewCategory("Segments", true);
            var operators = this.GetNewCategory("Operators", false);
            var nodetypes = this.GetNewCategory("NodeTypes", true);
            var categoryElement = new CategoryElement { CategoryId = 1, Name = "Segments1" };
            segments.Elements.Add(categoryElement);

            this.dataAccess.Insert(segments);
            this.dataAccess.Insert(operators);
            this.dataAccess.Insert(nodetypes);
            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);
            this.dataAccess = new SqlDataAccess<Category>(this.dataContext);

            // Act
            var result = await this.dataAccess.GetAllAsync(x => x.IsActive == true, x => new { x.Name, x.Elements }, "Elements").ConfigureAwait(false);
            var segmentCategory = result.FirstOrDefault(x => x.Name == "Segments");

            // Assert
            Assert.IsNotNull(segmentCategory);
            Assert.IsNotNull(segmentCategory.Elements);
            Assert.IsTrue(segmentCategory.Elements.Count > 0);
            Assert.IsTrue(segmentCategory.Elements.First().Name == categoryElement.Name);
            await this.CleanupEntitiesAsync(segments, operators, nodetypes).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets all asynchronous by entity should get first category include child items from database asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OrderByAsync_ByEntity_ShouldOrderedCategory_FromDatabaseAsync()
        {
            // Arrange
            var transport = this.GetNewCategory("transport", true);
            var production = this.GetNewCategory("production", true);
            var commercial = this.GetNewCategory("commercial", false);

            this.dataAccess.Insert(transport);
            this.dataAccess.Insert(production);
            this.dataAccess.Insert(commercial);
            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);
            this.dataAccess = new SqlDataAccess<Category>(this.dataContext);

            // Act
            var result = await this.dataAccess.OrderByAsync(x => x.IsActive == true, n => n.Name, 1).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result.Count() == 1);
            Assert.AreSame(production, result.First());
            await this.CleanupEntitiesAsync(transport, production, commercial).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets all asynchronous by entity should get first category include child items from database asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OrderByDescAsync_ByEntity_ShouldOrderedCategory_FromDatabaseAsync()
        {
            // Arrange
            var transport = this.GetNewCategory("transport", true);
            var production = this.GetNewCategory("production", true);
            var commercial = this.GetNewCategory("commercial", false);

            this.dataAccess.Insert(transport);
            this.dataAccess.Insert(production);
            this.dataAccess.Insert(commercial);
            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);
            this.dataAccess = new SqlDataAccess<Category>(this.dataContext);

            // Act
            var result = await this.dataAccess.OrderByDescendingAsync(x => x.IsActive == true, n => n.Name, 1).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result.Count() == 1);
            Assert.AreSame(transport, result.First());
            await this.CleanupEntitiesAsync(transport, production, commercial).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the category by identifier should get category from database.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SingleOrDefaultAsync_ShouldGetSingleCategory_FromDatabaseAsync()
        {
            // Arrange
            var segments = this.GetNewCategory("Segments", true);
            var operators = this.GetNewCategory("Operators", false);
            var nodetypes = this.GetNewCategory("NodeTypes", true);

            this.dataAccess.Insert(segments);
            this.dataAccess.Insert(operators);
            this.dataAccess.Insert(nodetypes);
            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            var result = await this.dataAccess.SingleOrDefaultAsync(x => x.IsActive == false).ConfigureAwait(false);

            // Assert
            Assert.AreEqual("Operators", result.Name);

            await this.CleanupEntitiesAsync(segments, operators, nodetypes).ConfigureAwait(false);
        }

        /// <summary>
        /// Singles the or default asynchronous should get single category include child data from database asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SingleOrDefaultAsync_ShouldGetSingleCategoryIncludeChildData_FromDatabaseAsync()
        {
            // Arrange
            var segments = this.GetNewCategory("Segments", false);
            var operators = this.GetNewCategory("Operators", true);
            var nodetypes = this.GetNewCategory("NodeTypes", true);

            var categoryElement = new CategoryElement { CategoryId = 1, Name = "Segments1" };
            segments.Elements.Add(categoryElement);

            this.dataAccess.Insert(segments);
            this.dataAccess.Insert(operators);
            this.dataAccess.Insert(nodetypes);

            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            var result = await this.dataAccess.SingleOrDefaultAsync(x => x.IsActive == false, "Elements").ConfigureAwait(false);

            // Assert
            Assert.AreEqual("Segments", result.Name);
            Assert.IsNotNull(result.Elements);
            Assert.IsTrue(result.Elements.Count > 0);
            Assert.IsTrue(result.Elements.First().Name == categoryElement.Name);
            await this.CleanupEntitiesAsync(segments, operators, nodetypes).ConfigureAwait(false);
        }

        /// <summary>
        /// Singles the or default asynchronous should get single category without predicate asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SingleOrDefaultAsync_ShouldGetSingleCategory_WithoutPredicateAsync()
        {
            // Arrange
            var segments = this.GetNewCategory("Segments", false);

            this.dataAccess.Insert(segments);

            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            var result = await this.dataAccess.SingleOrDefaultAsync(null).ConfigureAwait(false);

            // Assert
            Assert.AreEqual("Segments", result.Name);
            await this.CleanupEntitiesAsync(segments).ConfigureAwait(false);
        }

        /// <summary>
        /// Singles the or default asynchronous should get single category include child data from database asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SingleOrDefaultAsync_ShouldGetSingleCategoryIncludeChildDataWithSelector_FromDatabaseAsync()
        {
            // Arrange
            var segments = this.GetNewCategory("Segments", false);
            var operators = this.GetNewCategory("Operators", true);
            var nodetypes = this.GetNewCategory("NodeTypes", true);

            var categoryElement = new CategoryElement { CategoryId = 1, Name = "Segments1" };
            segments.Elements.Add(categoryElement);

            this.dataAccess.Insert(segments);
            this.dataAccess.Insert(operators);
            this.dataAccess.Insert(nodetypes);

            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            var result = await this.dataAccess.SingleOrDefaultAsync(x => x.IsActive == false, x => new { x.Name, x.Elements }, "Elements").ConfigureAwait(false);

            // Assert
            Assert.AreEqual("Segments", result.Name);
            Assert.IsNotNull(result.Elements);
            Assert.IsTrue(result.Elements.Count > 0);
            Assert.IsTrue(result.Elements.First().Name == categoryElement.Name);
            await this.CleanupEntitiesAsync(segments, operators, nodetypes).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes the asynchronous should execute from database asynchronous.
        /// </summary>
        /// <returns>The tasks.</returns>
        [TestMethod]
        public async Task ExecuteAsync_ShouldExecute_FromDatabaseAsync()
        {
            // Arrange
            var expectedErrorMessage = "Relational-specific methods can only be used when the context is using a relational database provider.";
            var arg = new object();
            var data = new Dictionary<string, object>();
            data.Add("Key1", "Value1");
            data.Add("key2", "Value2");
            this.dataAccess = new SqlDataAccess<Category>(this.dataContext);

            // Act
            var ex = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await this.dataAccess.ExecuteAsync(arg, data).ConfigureAwait(false), "Argument null exception must be thrown if no connection is passed in argument.").ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(ex);
            Assert.IsNotNull(ex.Message);
            Assert.AreEqual(expectedErrorMessage, ex.Message, true, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Cleans up this instance.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            this.dataContext.Dispose();
        }

        /// <summary>
        /// Sets the access token.
        /// </summary>
        public void SetAccessToken()
        {
            (this.dataContext.Database.GetDbConnection() as System.Data.SqlClient.SqlConnection).AccessToken = "test";
        }

        /// <summary>
        /// Gets the new category.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The category.</returns>
        private Category GetNewCategory(string name = CategoryName, bool isActive = true)
        {
            var category = new Category
            {
                Name = name,
                IsActive = isActive,
            };

            category.Initialize();

            return category;
        }

        private Task CleanupEntitiesAsync(params Category[] entities)
        {
            this.dataContext.RemoveRange(entities);
            return this.dataContext.SaveChangesAsync();
        }
    }
}
