// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TicketInfoRepositoryTests.cs" company="Microsoft">
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
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Repositories.Specialized;
    using Ecp.True.Repositories.Tests.TestAsyncQueryHelpers;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;

    /// <summary>
    /// The TicketInfoRepositoryTests.
    /// </summary>
    [TestClass]
    public class TicketInfoRepositoryTests
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private TicketInfoRepository repository;

        /// <summary>
        /// The Ticket SQL data access.
        /// </summary>
        private Mock<ISqlDataAccess<Ticket>> ticketSqlDataAccess;

        /// <summary>
        /// The Movement SQL data access.
        /// </summary>
        private Mock<ISqlDataAccess<Movement>> movementSqlDataAccess;

        /// <summary>
        /// The Inventory SQL data access.
        /// </summary>
        private Mock<ISqlDataAccess<Inventory>> inventorySqlDataAccess;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.ticketSqlDataAccess = new Mock<ISqlDataAccess<Ticket>>();
            this.movementSqlDataAccess = new Mock<ISqlDataAccess<Movement>>();
            this.inventorySqlDataAccess = new Mock<ISqlDataAccess<Inventory>>();
            this.repository = new TicketInfoRepository(this.ticketSqlDataAccess.Object, this.movementSqlDataAccess.Object, this.inventorySqlDataAccess.Object);
        }

        /// <summary>
        /// Gets the ticket information asynchronous returns ticket information when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetTicketInfoAsync_ReturnsTicketInfo_WhenInvokedAsync()
        {
            var ticket = new Ticket { TicketId = 1, CategoryElementId = 2, StartDate = new DateTime(2019, 02, 12), EndDate = new DateTime(2019, 02, 16), Status = 0, CreatedBy = "System", CreatedDate = new DateTime(2019, 02, 18), LastModifiedBy = null, LastModifiedDate = null };

            var tickets = new List<Ticket>();
            tickets.Add(ticket);
            var inventory = new Dictionary<string, int>();
            inventory.Add("test", 6);
            var movement = new Dictionary<string, int>();
            movement.Add("SINOPER", 9);

            var movements = JsonConvert.DeserializeObject<IEnumerable<Movement>>(File.ReadAllText("MovementJson/Movements.json"));
            DbSet<Movement> movementDbSet = GetQueryableMockDbSet(movements.AsQueryable());

            var inventory1 = new Inventory { NodeId = 81, InventoryDate = new DateTime(2019, 9, 1, 8, 10, 0), InventoryId = "20190901", Node = new Node { NodeId = 81, Name = "AYACUCHO CRD-GALAN 14" }, TicketId = 1, SourceSystem = "test" };
            var inventory2 = new Inventory { NodeId = 81, InventoryDate = new DateTime(2019, 9, 1, 8, 10, 0), InventoryId = "20190901", Node = new Node { NodeId = 81, Name = "AYACUCHO CRD-GALAN 14" }, TicketId = 1, SourceSystem = "test" };
            var inventory3 = new Inventory { NodeId = 81, InventoryDate = new DateTime(2019, 9, 1, 8, 10, 0), InventoryId = "20190901", Node = new Node { NodeId = 81, Name = "AYACUCHO CRD-GALAN 14" }, TicketId = 1, SourceSystem = "test" };
            var inventory4 = new Inventory { NodeId = 81, InventoryDate = new DateTime(2019, 9, 1, 8, 10, 0), InventoryId = "20190901", Node = new Node { NodeId = 81, Name = "AYACUCHO CRD-GALAN 14" }, TicketId = 1, SourceSystem = "test" };
            var inventory5 = new Inventory { NodeId = 81, InventoryDate = new DateTime(2019, 9, 2, 8, 10, 0), InventoryId = "20190902", Node = new Node { NodeId = 81, Name = "AYACUCHO CRD-GALAN 14" }, TicketId = 1, SourceSystem = "test" };
            var inventory6 = new Inventory { NodeId = 81, InventoryDate = new DateTime(2019, 9, 2, 8, 10, 0), InventoryId = "20190902", Node = new Node { NodeId = 81, Name = "AYACUCHO CRD-GALAN 14" }, TicketId = 1, SourceSystem = "test" };

            var inventories = new List<Inventory> { inventory1, inventory2, inventory3, inventory4, inventory5, inventory6 }.AsQueryable();

            DbSet<Inventory> inventoryDbSet = GetQueryableMockDbSet(inventories.AsQueryable());

            DbSet<Ticket> ticketDbSet = GetQueryableMockDbSet(tickets.AsQueryable());

            this.ticketSqlDataAccess.Setup(a => a.Set<Ticket>()).Returns(ticketDbSet);
            this.movementSqlDataAccess.Setup(a => a.Set<Movement>()).Returns(movementDbSet);
            this.inventorySqlDataAccess.Setup(a => a.Set<Inventory>()).Returns(inventoryDbSet);

            var generatedMovements = new Dictionary<string, int>();
            var ticketInfo = new TicketInfo(ticket, inventory, movement, generatedMovements);

            this.ticketSqlDataAccess.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<Ticket, bool>>>(), "CategoryElement")).ReturnsAsync(ticket);

            // Act
            var result = await this.repository.GetTicketInfoAsync(1).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ticketInfo.Movements.Count, result.Movements.Count);
            Assert.AreEqual(ticketInfo.Inventories.Count, result.Inventories.Count);
            Assert.AreEqual(ticketInfo.Ticket.TicketId, result.Ticket.TicketId);

            this.ticketSqlDataAccess.Verify(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<Ticket, bool>>>(), "CategoryElement"), Times.Once);
        }

        /// <summary>
        /// Gets the ticket information asynchronous returns ticket information when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetTicketInfoAsync_ReturnsError_WhenInvokedAsync()
        {
            var ticket = new Ticket { TicketId = 1, CategoryElementId = 2, StartDate = new DateTime(2019, 02, 12), EndDate = new DateTime(2019, 02, 16), Status = 0, CreatedBy = "System", CreatedDate = new DateTime(2019, 02, 18), LastModifiedBy = null, LastModifiedDate = null };
            Ticket ticketReturnObject = null;
            var tickets = new List<Ticket>();
            tickets.Add(ticket);
            var inventory = new Dictionary<string, int>();
            inventory.Add("test", 6);
            var movement = new Dictionary<string, int>();
            movement.Add("SINOPER", 9);

            var movements = JsonConvert.DeserializeObject<IEnumerable<Movement>>(File.ReadAllText("MovementJson/Movements.json"));
            DbSet<Movement> movementDbSet = GetQueryableMockDbSet(movements.AsQueryable());

            var inventory1 = new Inventory { NodeId = 81, InventoryDate = new DateTime(2019, 9, 1, 8, 10, 0), InventoryId = "20190901", Node = new Node { NodeId = 81, Name = "AYACUCHO CRD-GALAN 14" }, TicketId = 1, SourceSystem = "test" };

            var inventory2 = new Inventory { NodeId = 81, InventoryDate = new DateTime(2019, 9, 1, 8, 10, 0), InventoryId = "20190901", Node = new Node { NodeId = 81, Name = "AYACUCHO CRD-GALAN 14" }, TicketId = 1, SourceSystem = "test" };

            var inventory3 = new Inventory { NodeId = 81, InventoryDate = new DateTime(2019, 9, 1, 8, 10, 0), InventoryId = "20190901", Node = new Node { NodeId = 81, Name = "AYACUCHO CRD-GALAN 14" }, TicketId = 1, SourceSystem = "test" };

            var inventory4 = new Inventory { NodeId = 81, InventoryDate = new DateTime(2019, 9, 1, 8, 10, 0), InventoryId = "20190901", Node = new Node { NodeId = 81, Name = "AYACUCHO CRD-GALAN 14" }, TicketId = 1, SourceSystem = "test" };

            var inventory5 = new Inventory { NodeId = 81, InventoryDate = new DateTime(2019, 9, 2, 8, 10, 0), InventoryId = "20190902", Node = new Node { NodeId = 81, Name = "AYACUCHO CRD-GALAN 14" }, TicketId = 1, SourceSystem = "test" };

            var inventory6 = new Inventory { NodeId = 81, InventoryDate = new DateTime(2019, 9, 2, 8, 10, 0), InventoryId = "20190902", Node = new Node { NodeId = 81, Name = "AYACUCHO CRD-GALAN 14" }, TicketId = 1, SourceSystem = "test" };

            var inventories = new List<Inventory> { inventory1, inventory2, inventory3, inventory4, inventory5, inventory6 }.AsQueryable();

            DbSet<Inventory> inventoryDbSet = GetQueryableMockDbSet(inventories.AsQueryable());

            DbSet<Ticket> ticketDbSet = GetQueryableMockDbSet(tickets.AsQueryable());

            this.ticketSqlDataAccess.Setup(a => a.Set<Ticket>()).Returns(ticketDbSet);
            this.movementSqlDataAccess.Setup(a => a.Set<Movement>()).Returns(movementDbSet);
            this.inventorySqlDataAccess.Setup(a => a.Set<Inventory>()).Returns(inventoryDbSet);

            this.ticketSqlDataAccess.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<Ticket, bool>>>(), "CategoryElement")).ReturnsAsync(ticketReturnObject);

            // Act
            var result = await this.repository.GetTicketInfoAsync(1).ConfigureAwait(false);

            // Assert
            Assert.IsNull(result);

            this.ticketSqlDataAccess.Verify(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<Ticket, bool>>>(), "CategoryElement"), Times.Once);
        }

        [TestMethod]
        public async Task GetLastTicketIdAsync_ReturnsTicketId_WhenInvokedAsync()
        {
            var ticketId = 10;

            var ticket1 = new Ticket { TicketId = 1, CategoryElementId = 2, StartDate = new DateTime(2019, 02, 12), EndDate = new DateTime(2019, 02, 16), Status = 0, CreatedBy = "System", CreatedDate = new DateTime(2019, 02, 18), LastModifiedBy = null, LastModifiedDate = null };
            var ticket2 = new Ticket { TicketId = 10, CategoryElementId = 2, StartDate = new DateTime(2019, 02, 15), EndDate = new DateTime(2019, 02, 25), Status = 0, CreatedBy = "System", CreatedDate = new DateTime(2019, 02, 18), LastModifiedBy = null, LastModifiedDate = null };

            var tickets = new List<Ticket> { ticket1, ticket2 };

            DbSet<Ticket> ticketDbSet = GetQueryableMockDbSet(tickets.AsQueryable());

            this.ticketSqlDataAccess.Setup(a => a.Set<Ticket>()).Returns(ticketDbSet);

            // Act
            var result = await this.repository.GetLastTicketIdAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ticketId, result);
        }

        /// <summary>
        /// Gets the ownership by segment when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task GetOwnershipBySegment_WhenInvokedAsync()
        {
            var ticket1 = new Ticket { TicketId = 1, CategoryElementId = 1, StartDate = new DateTime(2019, 02, 12), EndDate = new DateTime(2019, 02, 16), TicketTypeId = TicketType.Cutoff, Status = 0, CreatedBy = "System", CreatedDate = new DateTime(2019, 02, 18), LastModifiedBy = null, LastModifiedDate = null };
            var ticket2 = new Ticket { TicketId = 10, CategoryElementId = 1, StartDate = new DateTime(2019, 02, 12), EndDate = new DateTime(2019, 02, 15), Status = 0, TicketTypeId = TicketType.Ownership, CreatedBy = "System", CreatedDate = new DateTime(2019, 02, 18), LastModifiedBy = null, LastModifiedDate = null };

            var tickets = new List<Ticket> { ticket1, ticket2 };

            DbSet<Ticket> ticketDbSet = GetQueryableMockDbSet(tickets.AsQueryable());

            this.ticketSqlDataAccess.Setup(a => a.Set<Ticket>()).Returns(ticketDbSet);
            var result = await this.repository.GetOwnershipBySegmentAsync(1).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual(result[TicketType.Cutoff], ticket1.EndDate);
        }

        /// <summary>
        /// Gets the ownership by segment when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task GetOwnershipBySegment_WithDefaultStartDate_WhenInvokedAsync()
        {
            var ticket1 = new Ticket { TicketId = 1, CategoryElementId = 1, StartDate = new DateTime(2019, 02, 12), EndDate = new DateTime(2019, 02, 16), TicketTypeId = TicketType.Cutoff, Status = 0, CreatedBy = "System", CreatedDate = new DateTime(2019, 02, 18), LastModifiedBy = null, LastModifiedDate = null };
            var ticket2 = new Ticket { TicketId = 10, CategoryElementId = 1, StartDate = new DateTime(2019, 02, 12), EndDate = new DateTime(2019, 02, 15), Status = 0, TicketTypeId = TicketType.Ownership, CreatedBy = "System", CreatedDate = new DateTime(2019, 02, 18), LastModifiedBy = null, LastModifiedDate = null };

            var tickets = new List<Ticket> { ticket1, ticket2 };

            DbSet<Ticket> ticketDbSet = GetQueryableMockDbSet(tickets.AsQueryable());

            this.ticketSqlDataAccess.Setup(a => a.Set<Ticket>()).Returns(ticketDbSet);
            var result = await this.repository.GetOwnershipBySegmentAsync(10000).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.IsNull(result[TicketType.Cutoff]);
        }

        /// <summary>
        /// Gets the queryable mock database set.
        /// </summary>
        /// <typeparam name="T">The [T] entity.</typeparam>
        /// <param name="sourceList">The source list.</param>
        /// <returns>The Dbset of [T] entity.</returns>
        private static DbSet<T> GetQueryableMockDbSet<T>(IQueryable<T> sourceList)
            where T : class
        {
            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>()
                .Setup(x => x.Provider)
                .Returns(new TestAsyncQueryProvider<T>(sourceList.Provider));

            dbSet.As<IQueryable<T>>()
                .Setup(x => x.Expression)
                .Returns(sourceList.Expression);

            dbSet.As<IQueryable<T>>()
                .Setup(x => x.ElementType)
                .Returns(sourceList.ElementType);

            dbSet.As<IQueryable<T>>()
                .Setup(x => x.GetEnumerator())
                .Returns(sourceList.GetEnumerator());

            return dbSet.Object;
        }
    }
}
