// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeRepositoryTests.cs" company="Microsoft">
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
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Sql.Interfaces;
    using Ecp.True.Entities;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Repositories.Specialized;
    using EfCore.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The Homologation Repository Tests.
    /// </summary>
    [TestClass]
    public class NodeRepositoryTests
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
        /// The node segment repository.
        /// </summary>
        private NodeRepository nodeRepository;

        /// <summary>
        /// The data access.
        /// </summary>
        private SqlDataAccess<Node> dataAccessNode;

        /// <summary>
        /// The category element data access.
        /// </summary>
        private SqlDataAccess<CategoryElement> categoryElement;

        /// <summary>
        /// The category data access.
        /// </summary>
        private SqlDataAccess<Category> category;

        /// <summary>
        /// The data access.
        /// </summary>
        private SqlDataAccess<NodeTag> dataAccessNodeTag;

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
                                    .UseInMemoryDatabase(databaseName: $"InMemoryDatabase_{Guid.NewGuid()}")
                                    .Options;
            this.mockAuditService.Setup(m => m.GetAuditLogs(It.IsAny<ChangeTracker>())).Returns(new List<AuditLog>());
            this.dataContext = new SqlDataContext(options, this.mockAuditService.Object, this.businessContext.Object, this.sqlTokenProvider.Object);

            this.dataAccessNode = new SqlDataAccess<Node>(this.dataContext);
            this.dataAccessNodeTag = new SqlDataAccess<NodeTag>(this.dataContext);
            this.categoryElement = new SqlDataAccess<CategoryElement>(this.dataContext);
            this.category = new SqlDataAccess<Category>(this.dataContext);

            this.nodeRepository = new NodeRepository(this.dataAccessNode, this.dataAccessNodeTag);
        }

        /// <summary>
        /// Test Method which validates if a node exists in the segment, with the same order.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task ExistsNodeWithSameOrderAsync_ShouldReturnNode_WhenDuplicateOrderExistsInSameSegmentAsync()
        {
            // Arrange
            var node = new Node
            {
                NodeId = 2,
                Segment = new CategoryElement
                {
                    ElementId = 1,
                },
                SegmentId = 1,
                Order = 2,
                Name = "Node2",
            };

            var nodeTag = new NodeTag
            {
                Node = node,
                NodeTagId = 2,
                StartDate = this.ToTrue(DateTime.UtcNow.AddMinutes(-10)),
                EndDate = this.ToTrue(DateTime.UtcNow.AddMinutes(20)),
                ElementId = 1,
            };

            var node1 = new Node
            {
                NodeId = 3,
                Segment = new CategoryElement
                {
                    ElementId = 1,
                },
                SegmentId = 1,
                Order = 2,
                Name = "Node1",
            };

            var nodeTag1 = new NodeTag
            {
                Node = node1,
                NodeTagId = 3,
                StartDate = this.ToTrue(DateTime.UtcNow.AddMinutes(-10)),
                EndDate = this.ToTrue(DateTime.UtcNow.AddMinutes(20)),
                ElementId = 1,
            };

            this.dataAccessNode.Insert(node);
            this.dataAccessNode.Insert(node1);
            this.dataAccessNodeTag.Insert(nodeTag);
            this.dataAccessNodeTag.Insert(nodeTag1);
            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            var result = await this.nodeRepository.GetNodeWithSameOrderAsync(3, 1, 2).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(node, result);
        }

        [TestMethod]
        public async Task ExistsNodeWithSameOrderAsync_ShouldReturnNodesWithSameOrHigherOrderAsync()
        {
            // Arrange
            var node1 = new Node
            {
                NodeId = 1,
                Segment = new CategoryElement
                {
                    ElementId = 1,
                },
                SegmentId = 1,
                Order = 2,
            };

            var node2 = new Node
            {
                NodeId = 2,
                Segment = new CategoryElement
                {
                    ElementId = 1,
                },
                SegmentId = 1,
                Order = 2,
            };

            var node3 = new Node
            {
                NodeId = 3,
                Segment = new CategoryElement
                {
                    ElementId = 1,
                },
                SegmentId = 1,
                Order = 1,
            };

            var nodeTag1 = new NodeTag
            {
                Node = node1,
                NodeTagId = 1,
                StartDate = this.ToTrue(DateTime.UtcNow.AddMinutes(-10)),
                EndDate = this.ToTrue(DateTime.UtcNow.AddMinutes(20)),
                ElementId = 1,
            };

            var nodeTag2 = new NodeTag
            {
                Node = node2,
                NodeTagId = 2,
                StartDate = this.ToTrue(DateTime.UtcNow.AddMinutes(-10)),
                EndDate = this.ToTrue(DateTime.UtcNow.AddMinutes(20)),
                ElementId = 1,
            };

            var nodeTag3 = new NodeTag
            {
                Node = node3,
                NodeTagId = 3,
                StartDate = this.ToTrue(DateTime.UtcNow.AddMinutes(-10)),
                EndDate = this.ToTrue(DateTime.UtcNow.AddMinutes(20)),
                ElementId = 1,
            };

            // Act
            this.dataAccessNode.Insert(node1);
            this.dataAccessNode.Insert(node2);
            this.dataAccessNode.Insert(node3);
            this.dataAccessNodeTag.Insert(nodeTag1);
            this.dataAccessNodeTag.Insert(nodeTag2);
            this.dataAccessNodeTag.Insert(nodeTag3);
            var rows = await this.dataContext.SaveChangesAsync().ConfigureAwait(false);

            // Assert
            Assert.AreEqual(6, rows);

            var result = await this.nodeRepository.GetNodesWithSameOrHigherOrderAsync(1, 2).ConfigureAwait(false);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public async Task GetSegmentDetailForNode_ShouldReturnCategoryElement_WhenInvokedAsync()
        {
            // Arrange
            var node1 = new Node
            {
                NodeId = 1,
                Segment = new CategoryElement
                {
                    ElementId = 1,
                },
                SegmentId = 1,
                Order = 2,
            };

            var nodeTag1 = new NodeTag
            {
                Node = node1,
                NodeTagId = 1,
                StartDate = this.ToTrue(DateTime.UtcNow.AddMinutes(-10)),
                EndDate = this.ToTrue(DateTime.UtcNow.AddMinutes(20)),
                ElementId = 1,
            };

            var categoryDetail = new Category
            {
                CategoryId = 1,
                Name = "Segmento",
            };

            var categoryElement1 = new CategoryElement
            {
                ElementId = 1,
                Name = "element 1",
                CategoryId = 1,
            };

            // Act
            this.dataAccessNode.Insert(node1);
            this.dataAccessNodeTag.Insert(nodeTag1);
            this.category.Insert(categoryDetail);
            this.categoryElement.Insert(categoryElement1);

            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);

            var row = await this.nodeRepository.GetSegmentDetailForNodeAsync(1).ConfigureAwait(false);
            Assert.IsNotNull(row);
            Assert.AreEqual(row, categoryElement1);
        }

        /// <summary>
        /// Gets the ticks.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>Returns ticks.</returns>
        private DateTime ToTrue(DateTime dateTime)
        {
            var timezones = TimeZoneInfo.GetSystemTimeZones();
            var trueTimeZone = timezones.FirstOrDefault(x => x.Id == "SA Pacific Standard Time" || x.Id == "America/Bogota");
            if (trueTimeZone == null)
            {
                return dateTime;
            }

            return dateTime.Kind == DateTimeKind.Utc ? TimeZoneInfo.ConvertTimeFromUtc(dateTime, trueTimeZone) : TimeZoneInfo.ConvertTime(dateTime, trueTimeZone);
        }
    }
}
