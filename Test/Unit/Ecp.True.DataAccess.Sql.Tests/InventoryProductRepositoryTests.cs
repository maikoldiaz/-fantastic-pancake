// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryProductRepositoryTests.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql.Interfaces;
    using Ecp.True.Entities;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Repositories.Specialized;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The inventory repository tests.
    /// </summary>
    [TestClass]
    public class InventoryProductRepositoryTests
    {
        /// <summary>
        /// The business context.
        /// </summary>
        private Mock<IBusinessContext> businessContext;

        /// <summary>
        /// The data context.
        /// </summary>
        private SqlDataContext dataContext;

        /// <summary>
        /// The data access.
        /// </summary>
        private SqlDataAccess<InventoryProduct> dataAccessInventoryProduct;

        /// <summary>
        /// The movement repository.
        /// </summary>
        private IInventoryProductRepository inventoryProductRepository;

        /// <summary>
        /// The mock audit service.
        /// </summary>
        private Mock<IAuditService> mockAuditService;

        /// <summary>
        /// The SQL token provider.
        /// </summary>
        private Mock<ISqlTokenProvider> sqlTokenProvider;

        [TestInitialize]
        public void Initialize()
        {
            this.mockAuditService = new Mock<IAuditService>();
            this.businessContext = new Mock<IBusinessContext>();
            this.sqlTokenProvider = new Mock<ISqlTokenProvider>();

            this.mockAuditService.Setup(m => m.GetAuditLogs(It.IsAny<ChangeTracker>())).Returns(new List<AuditLog>());
            var options = new DbContextOptionsBuilder<SqlDataContext>()
                                    .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                                    .Options;
            this.dataContext = new SqlDataContext(options, this.mockAuditService.Object, this.businessContext.Object, this.sqlTokenProvider.Object);

            this.dataAccessInventoryProduct = new SqlDataAccess<InventoryProduct>(this.dataContext);

            this.inventoryProductRepository = new InventoryProductRepository(this.dataAccessInventoryProduct);
        }

        /// <summary>
        /// Determines whether [has movement exists for connection asynchronous should return false for movemement for node connection asynchronous].
        /// </summary>
        /// <returns>
        /// A Task.
        /// </returns>
        [TestMethod]
        public async Task GetLatestProductVolumeAsync_ShouldReturnLatestInventoryProduct_WhenInventoryExistsAsync()
        {
            // Arrange
            var inventoryProduct = this.GetInventoryProduct();

            // Act
            this.dataAccessInventoryProduct.Insert(inventoryProduct);
            var rows = await this.dataContext.SaveAsync(CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(1, rows);

            var result = await this.inventoryProductRepository.GetLatestInventoryProductAsync("1").ConfigureAwait(false);
            Assert.AreEqual(1000.00M, result.ProductVolume, "Invalid volume");
            Assert.AreEqual(EventType.Insert.ToString("G"), result.EventType, "Invalid event type");

            this.dataContext.RemoveRange(inventoryProduct);
            this.dataContext.SaveChanges();
        }

        /// <summary>
        /// Determines whether [has movement exists for connection asynchronous should return false for movemement for node connection asynchronous].
        /// </summary>
        /// <returns>
        /// A Task.
        /// </returns>
        [TestMethod]
        public async Task GetLatestProductVolumeAsync_ShouldReturnNull_WhenInventoryDoesNotExistAsync()
        {
            // Arrange
            var inventoryProduct = this.GetInventoryProduct();

            // Act
            this.dataAccessInventoryProduct.Insert(inventoryProduct);
            var rows = await this.dataContext.SaveAsync(CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(1, rows);

            var result = await this.inventoryProductRepository.GetLatestInventoryProductAsync("23").ConfigureAwait(false);
            Assert.IsNull(result);

            this.dataContext.RemoveRange(inventoryProduct);
            this.dataContext.SaveChanges();
        }

        private InventoryProduct GetInventoryProduct()
        {
            return new InventoryProduct
            {
                InventoryProductUniqueId = "1",
                InventoryProductId = 1,
                ProductVolume = 1000.00M,
                EventType = EventType.Insert.ToString("G"),
            };
        }

        private IEnumerable<InventoryProduct> SetupInventoryProducts(int retryCount)
        {
            return new List<InventoryProduct>
            {
                new InventoryProduct
                {
                    InventoryProductUniqueId = "1",
                    InventoryProductId = 1,
                    ProductVolume = 1000.00M,
                    EventType = "Insert",
                    RetryCount = retryCount,
                },
            };
        }
    }
}
