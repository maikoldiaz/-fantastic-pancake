// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeOwnershipRepositoryTests.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Sql.Interfaces;
    using Ecp.True.Entities;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Repositories.Specialized;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The Node Ownership Repository Tests.
    /// </summary>
    [TestClass]
    public class NodeOwnershipRepositoryTests
    {
        /// <summary>
        /// The node ownership repository.
        /// </summary>
        private NodeOwnershipRepository nodeOwnershipRepository;

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
        private SqlDataAccess<NodeConnectionProductOwner> dataAccessNodeConnectionProductOwner;

        /// <summary>
        /// The data access.
        /// </summary>
        private SqlDataAccess<NodeConnectionProduct> dataAccessNodeConnectionProduct;

        /// <summary>
        /// The data access.
        /// </summary>
        private SqlDataAccess<NodeConnection> dataAccessNodeConnection;

        /// <summary>
        /// The data access.
        /// </summary>
        private SqlDataAccess<Node> dataAccessNode;

        /// <summary>
        /// The data access.
        /// </summary>
        private SqlDataAccess<NodeStorageLocation> dataAccessNodeStorageLocation;

        /// <summary>
        /// The data access.
        /// </summary>
        private SqlDataAccess<StorageLocationProduct> dataAccessStorageLocationProduct;

        /// <summary>
        /// The data access.
        /// </summary>
        private SqlDataAccess<StorageLocationProductOwner> dataAccessStorageLocationProductOwner;

        /// <summary>
        /// The data access.
        /// </summary>
        private SqlDataAccess<Product> dataAccessProduct;

        /// <summary>
        /// The mock audit service.
        /// </summary>
        private Mock<IAuditService> mockAuditService;

        /// <summary>
        /// The SQL token provider.
        /// </summary>
        private Mock<ISqlTokenProvider> sqlTokenProvider;

        /// <summary>
        /// Initialize Method for the test class.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockAuditService = new Mock<IAuditService>();
            this.businessContext = new Mock<IBusinessContext>();
            this.sqlTokenProvider = new Mock<ISqlTokenProvider>();
            var options = new DbContextOptionsBuilder<SqlDataContext>()
                                    .UseInMemoryDatabase(databaseName: $"this.InMemoryDatabase_{Guid.NewGuid()}")
                                    .Options;
            this.mockAuditService.Setup(m => m.GetAuditLogs(It.IsAny<ChangeTracker>())).Returns(new List<AuditLog>());
            this.dataContext = new SqlDataContext(options, this.mockAuditService.Object, this.businessContext.Object, this.sqlTokenProvider.Object);
            this.dataAccessNode = new SqlDataAccess<Node>(this.dataContext);
            this.dataAccessNodeConnectionProductOwner = new SqlDataAccess<NodeConnectionProductOwner>(this.dataContext);
            this.dataAccessNodeConnectionProduct = new SqlDataAccess<NodeConnectionProduct>(this.dataContext);
            this.dataAccessNodeConnection = new SqlDataAccess<NodeConnection>(this.dataContext);
            this.dataAccessNodeStorageLocation = new SqlDataAccess<NodeStorageLocation>(this.dataContext);
            this.dataAccessStorageLocationProduct = new SqlDataAccess<StorageLocationProduct>(this.dataContext);
            this.dataAccessStorageLocationProductOwner = new SqlDataAccess<StorageLocationProductOwner>(this.dataContext);
            this.dataAccessProduct = new SqlDataAccess<Product>(this.dataContext);

            this.nodeOwnershipRepository = new NodeOwnershipRepository(
                this.dataAccessNodeConnectionProductOwner,
                this.dataAccessNodeConnectionProduct,
                this.dataAccessNodeConnection,
                this.dataAccessNode,
                this.dataAccessNodeStorageLocation,
                this.dataAccessStorageLocationProduct,
                this.dataAccessStorageLocationProductOwner,
                this.dataAccessProduct);
        }

        [TestMethod]
        public async Task GetOwnersForMovementAsync_ShouldReturnOwnersAsync()
        {
            // Arrange
            var nodeConnectionProductOwner = new NodeConnectionProductOwner
            {
                NodeConnectionProductOwnerId = 1738,
                NodeConnectionProductId = 464,
                OwnerId = 27,
                OwnershipPercentage = 100,
                IsDeleted = false,
            };

            var nodeConnectionProduct = new NodeConnectionProduct
            {
                NodeConnectionProductId = 464,
                NodeConnectionId = 126,
                ProductId = "10000003006",
                IsDeleted = false,
                Priority = 10,
            };

            var nodeConnection = new NodeConnection
            {
                NodeConnectionId = 126,
                SourceNodeId = 130,
                DestinationNodeId = 132,
                Description = "NodeConnectionAutomation",
                IsDeleted = false,
            };
            this.dataAccessNodeConnectionProductOwner.Insert(nodeConnectionProductOwner);
            this.dataAccessNodeConnectionProduct.Insert(nodeConnectionProduct);
            this.dataAccessNodeConnection.Insert(nodeConnection);

            var rows = await this.dataContext.SaveChangesAsync().ConfigureAwait(false);
            List<NodeConnectionProductOwner> nodeConnectionProductOwnerList = new List<NodeConnectionProductOwner>();
            nodeConnectionProductOwnerList.Add(nodeConnectionProductOwner);

            // Act
            var result = await this.nodeOwnershipRepository.GetOwnersForMovementAsync(130, 132, "10000003006").ConfigureAwait(false);

            // Assert
            Assert.AreEqual(3, rows);
            Assert.IsNotNull(result);
            ////Assert.AreEqual(nodeConnectionProductOwnerList.First().OwnerId, result.First().OwnerId);
        }
    }
}
