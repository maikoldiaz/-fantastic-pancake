// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeOrderManagerTests.cs" company="Microsoft">
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
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql.Interfaces;
    using Ecp.True.Entities;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Host.Core;
    using Ecp.True.Processors.Api.Admin;
    using Ecp.True.Repositories;
    using Ecp.True.Repositories.Specialized;
    using EfCore.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The node manager tests.
    /// </summary>
    /// <seealso cref="True.Processors.Api.Tests.ProcessorTestBase" />
    [TestClass]
    public class NodeOrderManagerTests
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
        /// The data access.
        /// </summary>
        private SqlDataAccess<NodeTag> dataAccessNodeTag;

        /// <summary>
        /// The mock audit service.
        /// </summary>
        private Mock<IAuditService> mockAuditService;

        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockFactory;

        /// <summary>
        /// The mock node repository.
        /// </summary>
        private Mock<IRepository<Node>> mockNodeRepository;

        /// <summary>
        /// The node order manager.
        /// </summary>
        private NodeOrderManager nodeOrderManager;

        /// <summary>
        /// The SQL token provider.
        /// </summary>
        private Mock<ISqlTokenProvider> sqlTokenProvider;

        /// <summary>
        /// Initializes this instance.
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

            this.nodeRepository = new NodeRepository(this.dataAccessNode, this.dataAccessNodeTag);
            this.mockFactory = new Mock<IRepositoryFactory>();
            this.mockNodeRepository = new Mock<IRepository<Node>>();
            this.nodeOrderManager = new NodeOrderManager(new RepositoryFactory(this.dataContext));
        }

        /// <summary>
        /// Checks the node name exists should return true for node name exists in repository when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task NodeOrderManager_ShouldReorderNodesAsync()
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
                AutoOrder = true,
            };

            var node2 = new Node
            {
                NodeId = 2,
                Segment = new CategoryElement
                {
                    ElementId = 1,
                },
                SegmentId = 1,
                Order = 3,
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

            // Act
            this.dataAccessNode.Insert(node1);
            this.dataAccessNode.Insert(node2);
            this.dataAccessNodeTag.Insert(nodeTag1);
            this.dataAccessNodeTag.Insert(nodeTag2);
            var rows = await this.dataContext.SaveChangesAsync().ConfigureAwait(false);
            var repositoryFactory = new RepositoryFactory(this.dataContext);
            var unitOfWorkFactory = new UnitOfWorkFactory(this.dataContext, repositoryFactory);
            using (var unitOfWork = unitOfWorkFactory.GetUnitOfWork())
            {
                var repository = unitOfWork.CreateRepository<Node>();
                await this.nodeOrderManager.TryReorderAsync(node1, repository).ConfigureAwait(false);
                await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
                var orderedNodes = await repositoryFactory.NodeRepository.GetNodesWithSameOrHigherOrderAsync(1, 3).ConfigureAwait(false);
                Assert.AreEqual(2, orderedNodes.Count());
                Assert.AreEqual(3, orderedNodes.FirstOrDefault().Order);
                Assert.AreEqual(4, orderedNodes.ToArray()[1].Order);
            }

            Assert.AreEqual(4, rows);
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
