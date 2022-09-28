// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementRepositoryTests.cs" company="Microsoft">
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
    using Ecp.True.Core;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Sql.Interfaces;
    using Ecp.True.Entities;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Repositories.Specialized;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The movement repository tests.
    /// </summary>
    [TestClass]
    public class MovementRepositoryTests
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
        private SqlDataAccess<Movement> dataAccessMovement;

        /// <summary>
        /// The data access.
        /// </summary>
        private SqlDataAccess<MovementSource> dataAccessMovementSource;

        /// <summary>
        /// The data access.
        /// </summary>
        private SqlDataAccess<MovementDestination> dataAccessMovementDestination;

        /// <summary>
        /// The data access.
        /// </summary>
        private SqlDataAccess<MovementPeriod> dataAccessMovementPeriod;

        /// <summary>
        /// The movement repository.
        /// </summary>
        private MovementRepository movementRepository;

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

            this.dataAccessMovement = new SqlDataAccess<Movement>(this.dataContext);
            this.dataAccessMovementSource = new SqlDataAccess<MovementSource>(this.dataContext);
            this.dataAccessMovementDestination = new SqlDataAccess<MovementDestination>(this.dataContext);
            this.dataAccessMovementPeriod = new SqlDataAccess<MovementPeriod>(this.dataContext);

            this.movementRepository = new MovementRepository(this.dataAccessMovement);
        }

        /// <summary>
        /// Adds the category should add category to database.
        /// </summary>
        /// <returns>
        /// A Task.
        /// </returns>
        [TestMethod]
        public async Task HasMovementExistsForConnectionAsync_ShouldReturnTrueForMovemementForNodeConnectionAsync()
        {
            // Arrange
            var movement = this.GetNewMovments();
            var movementSource = this.GetNewMovementSource();
            var movementDestination = this.GetNewMovementDestination();

            // Act
            this.dataAccessMovement.Insert(movement);
            this.dataAccessMovementSource.Insert(movementSource);
            this.dataAccessMovementDestination.Insert(movementDestination);
            var rows = await this.dataContext.SaveAsync(CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(3, rows);

            var result = await this.movementRepository.HasActiveMovementForConnectionAsync(1, 1).ConfigureAwait(false);
            Assert.IsTrue(result, "Method must show movement exists for node connection.");

            this.mockAuditService.Verify(m => m.GetAuditLogs(It.IsAny<ChangeTracker>()), Times.Once);

            this.dataContext.RemoveRange(movement);
            this.dataContext.RemoveRange(movementSource);
            this.dataContext.RemoveRange(movementDestination);
            this.dataContext.SaveChanges();
        }

        /// <summary>
        /// Determines whether [has movement exists for connection asynchronous should return false for movemement for node connection asynchronous].
        /// </summary>
        /// <returns>
        /// A Task.
        /// </returns>
        [TestMethod]
        public async Task HasMovementExistsForConnectionAsync_ShouldReturnFalseForMovemementForNodeConnectionAsync()
        {
            // Arrange
            var movement = this.GetNewMovments();
            var movementSource = this.GetNewMovementSource();
            var movementDestination = this.GetNewMovementDestination();

            // Act
            this.dataAccessMovement.Insert(movement);
            this.dataAccessMovementSource.Insert(movementSource);
            this.dataAccessMovementDestination.Insert(movementDestination);
            var rows = await this.dataContext.SaveAsync(CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(3, rows);

            var result = await this.movementRepository.HasActiveMovementForConnectionAsync(8, 2).ConfigureAwait(false);
            Assert.IsFalse(result, "Method must show movement does not exists for node connection.");

            this.dataContext.RemoveRange(movement);
            this.dataContext.RemoveRange(movementSource);
            this.dataContext.RemoveRange(movementDestination);
            this.dataContext.SaveChanges();
        }

        /// <summary>
        /// Determines whether [has movement exists for connection asynchronous should return false for movemement for node connection asynchronous].
        /// </summary>
        /// <returns>
        /// A Task.
        /// </returns>
        [TestMethod]
        public async Task GetLatestMovementAsync_ShouldReturnNetStandardVolumeAndEventType_WhenMovementIsPresentAsync()
        {
            // Arrange
            var movement = this.GetNewMovments();
            var movementSource = this.GetNewMovementSource();
            var movementDestination = this.GetNewMovementDestination();

            // Act
            this.dataAccessMovement.Insert(movement);
            this.dataAccessMovementSource.Insert(movementSource);
            this.dataAccessMovementDestination.Insert(movementDestination);
            var rows = await this.dataContext.SaveAsync(CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(3, rows);

            var result = await this.movementRepository.GetLatestMovementAsync("1").ConfigureAwait(false);

            Assert.AreEqual(1000.00M, result.NetStandardVolume, "Invalid volume");
            Assert.AreEqual(EventType.Insert.ToString("G"), result.EventType, "Invalid event type");

            this.dataContext.RemoveRange(movement);
            this.dataContext.RemoveRange(movementSource);
            this.dataContext.RemoveRange(movementDestination);
            this.dataContext.SaveChanges();
        }

        /// <summary>
        /// Gets the latest movement asynchronous should return null when movement is not present asynchronous.
        /// </summary>
        /// <returns>
        /// A Task.
        /// </returns>
        [TestMethod]
        public async Task GetLatestMovementAsync_ShouldReturnNull_WhenMovementIsNotPresentAsync()
        {
            // Arrange
            var movement = this.GetNewMovments();
            var movementSource = this.GetNewMovementSource();
            var movementDestination = this.GetNewMovementDestination();

            // Act
            this.dataAccessMovement.Insert(movement);
            this.dataAccessMovementSource.Insert(movementSource);
            this.dataAccessMovementDestination.Insert(movementDestination);
            var rows = await this.dataContext.SaveAsync(CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(3, rows);

            var result = await this.movementRepository.GetLatestMovementAsync("123").ConfigureAwait(false);

            Assert.IsNull(result);

            this.dataContext.RemoveRange(movement);
            this.dataContext.RemoveRange(movementSource);
            this.dataContext.RemoveRange(movementDestination);
            this.dataContext.SaveChanges();
        }

        /// <summary>
        /// Determines whether [has movement exists for connection asynchronous should return false for movemement for node connection asynchronous].
        /// </summary>
        /// <returns>
        /// A Task.
        /// </returns>
        [TestMethod]
        public async Task GetMovementsForOfficialDeltaCalculationAsync_ShouldReturnMovementsAsync()
        {
            // Arrange
            var movement = this.GetNewMovments();
            var movementSource = this.GetNewMovementSource();
            var movementDestination = this.GetNewMovementDestination();
            var movementPeriod = this.GetNewMovementPeriod();

            // Act
            this.dataAccessMovement.Insert(movement);
            this.dataAccessMovementSource.Insert(movementSource);
            this.dataAccessMovementDestination.Insert(movementDestination);
            this.dataAccessMovementPeriod.Insert(movementPeriod);
            var rows = await this.dataContext.SaveAsync(CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(4, rows);

            var result = await this.movementRepository.GetMovementsForOfficialDeltaCalculationAsync(new List<int> { 1 }, new Entities.TransportBalance.Ticket { StartDate = DateTime.Today, EndDate = DateTime.Today, CategoryElementId = 1 }).ConfigureAwait(false);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());

            this.dataContext.RemoveRange(movement);
            this.dataContext.RemoveRange(movementSource);
            this.dataContext.RemoveRange(movementDestination);
            this.dataContext.RemoveRange(movementPeriod);
            this.dataContext.SaveChanges();
        }

        private MovementDestination GetNewMovementDestination()
        {
            return new MovementDestination
            {
                DestinationNodeId = 1,
                MovementTransactionId = 1,
            };
        }

        private MovementSource GetNewMovementSource()
        {
            return new MovementSource
            {
                SourceNodeId = 1,
                MovementTransactionId = 1,
            };
        }

        private MovementPeriod GetNewMovementPeriod()
        {
            return new MovementPeriod
            {
                MovementTransactionId = 1,
                StartTime = DateTime.Today,
                EndTime = DateTime.Today,
            };
        }

        private Movement GetNewMovments()
        {
            return new Movement
            {
                MovementId = "1",
                MovementTransactionId = 1,
                NetStandardVolume = 1000.00M,
                SegmentId = 1,
                OfficialDeltaTicketId = 1,
                EventType = EventType.Insert.ToString("G"),
                OfficialDeltaMessageTypeId = Entities.Enumeration.OfficialDeltaMessageType.OfficialMovementDelta,
            };
        }

        private IEnumerable<Movement> SetupMovements(int retryCount)
        {
            return new List<Movement>
            {
                new Movement
                {
                    MovementId = "1",
                    MovementTransactionId = 1,
                    NetStandardVolume = 1000.00M,
                    EventType = "Insert",
                    CreatedDate = DateTime.UtcNow.ToTrue().AddHours(-2),
                    RetryCount = retryCount,
                },
            };
        }
    }
}
