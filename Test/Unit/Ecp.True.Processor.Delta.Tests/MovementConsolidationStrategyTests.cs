// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementConsolidationStrategyTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Delta.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Query;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Delta.Consolidation;
    using Ecp.True.Processors.Delta.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The MovementConsolidationStrategyTests.
    /// </summary>
    [TestClass]
    public class MovementConsolidationStrategyTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger> mockLogger;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The movement consolidation strategy.
        /// </summary>
        private MovementConsolidationStrategy movementConsolidationStrategy;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();

            this.movementConsolidationStrategy = new MovementConsolidationStrategy(this.mockLogger.Object);
        }

        /// <summary>
        /// Consolidates the asynchronous should consolidate movements for no excluded movement case when invoke asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ConsolidateAsync_Should_ConsolidateMovements_ForNoExcludedMovementCase_WhenInvokeAsync()
        {
            var consolidationBatch = new ConsolidationBatch
            {
                Ticket = new Entities.TransportBalance.Ticket
                {
                    TicketId = 123,
                    CategoryElementId = 123,
                    StartDate = new DateTime(2020, 07, 01),
                    EndDate = new DateTime(2020, 07, 05),
                },
            };

            var consolidationMovementDatas = new List<ConsolidationMovementData>
            {
                this.GetConsolidationMovementData(120, 1, "SP1", 2, "DP1", "10", 1, 6300M, "Bbl", 10500M, 10501M, null, 10),
                this.GetConsolidationMovementData(120, 1, "SP1", 2, "DP1", "10", 2, 4200M, "Bbl", 10500M, 10501M, null, 10),
                this.GetConsolidationMovementData(121, 1, "SP1", 2, "DP1", "10", 1, 5100M, "Bbl", 8500M, 8502M, null, 10),
                this.GetConsolidationMovementData(121, 1, "SP1", 2, "DP1", "10", 2, 3400M, "Bbl", 8500M, 8502M, null, 10),
                this.GetConsolidationMovementData(122, 1, "SP1", 2, "DP1", "10", 1, 4488.74M, "Bbl", 6905.76M, 7420.88M, null, 10),
                this.GetConsolidationMovementData(122, 1, "SP1", 2, "DP1", "10", 2, 2417.02M, "Bbl", 6905.76M, 7420.88M, null, 10),
            };

            var consolidatedMovementRepository = new Mock<IRepository<ConsolidatedMovement>>();

            IEnumerable<ConsolidatedMovement> consolidatedMovement = new List<ConsolidatedMovement>();
            consolidatedMovementRepository.Setup(a => a.InsertAll(It.IsAny<IEnumerable<ConsolidatedMovement>>()))
                .Callback<IEnumerable<ConsolidatedMovement>>(list => consolidatedMovement = list);

            var consolidationMovementDataRepository = new Mock<IRepository<ConsolidationMovementData>>();
            consolidationMovementDataRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(consolidationMovementDatas);

            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidationMovementData>()).Returns(consolidationMovementDataRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidatedMovement>()).Returns(consolidatedMovementRepository.Object);

            await this.movementConsolidationStrategy.ConsolidateAsync(consolidationBatch, this.mockUnitOfWork.Object).ConfigureAwait(false);

            consolidationMovementDataRepository.Verify(a => a.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()), Times.AtLeastOnce);
            consolidatedMovementRepository.Verify(a => a.InsertAll(It.IsAny<IEnumerable<ConsolidatedMovement>>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<ConsolidationMovementData>(), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<ConsolidatedMovement>(), Times.Exactly(1));

            Assert.AreEqual(1, consolidatedMovement.Count());
            Assert.AreEqual(2, consolidatedMovement.First().ConsolidatedOwners.Count);
            Assert.AreEqual((int)SourceSystem.TRUE, consolidatedMovement.First().SourceSystemId);
            Assert.AreEqual(100.00M, consolidatedMovement.First().ConsolidatedOwners.Sum(a => a.OwnershipPercentage));

            Assert.AreEqual(25905.76M, consolidatedMovement.First().NetStandardVolume);
            Assert.AreEqual(26423.88M, consolidatedMovement.First().GrossStandardVolume);

            Assert.AreEqual(15888.74M, consolidatedMovement.First().ConsolidatedOwners.ElementAt(0).OwnershipVolume);
            Assert.AreEqual(10017.02M, consolidatedMovement.First().ConsolidatedOwners.ElementAt(1).OwnershipVolume);

            Assert.AreEqual(61.33M, consolidatedMovement.First().ConsolidatedOwners.ElementAt(0).OwnershipPercentage);
            Assert.AreEqual(38.67M, consolidatedMovement.First().ConsolidatedOwners.ElementAt(1).OwnershipPercentage);
        }

        /// <summary>
        /// Consolidates the asynchronous should consolidate movements for no excluded movement case for mixed geoss quantity when invoke asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ConsolidateAsync_Should_ConsolidateMovements_ForNoExcludedMovementCase__ForMixedGeossQuantityWhenInvokeAsync()
        {
            var consolidationBatch = new ConsolidationBatch
            {
                Ticket = new Entities.TransportBalance.Ticket
                {
                    TicketId = 123,
                    CategoryElementId = 123,
                    StartDate = new DateTime(2020, 07, 01),
                    EndDate = new DateTime(2020, 07, 05),
                },
            };

            var consolidationMovementDatas = new List<ConsolidationMovementData>
            {
                this.GetConsolidationMovementData(120, 1, "SP1", 2, "DP1", "10", 1, 6300M, "Bbl", 10500M, null, null, 10),
                this.GetConsolidationMovementData(120, 1, "SP1", 2, "DP1", "10", 2, 4200M, "Bbl", 10500M, 10501M, null, 10),
                this.GetConsolidationMovementData(121, 1, "SP1", 2, "DP1", "10", 1, 5100M, "Bbl", 8500M, 8502M, null, 10),
                this.GetConsolidationMovementData(121, 1, "SP1", 2, "DP1", "10", 2, 3400M, "Bbl", 8500M, 8502M, null, 10),
                this.GetConsolidationMovementData(122, 1, "SP1", 2, "DP1", "10", 1, 4488.74M, "Bbl", 6905.76M, 7420.88M, null, 10),
                this.GetConsolidationMovementData(122, 1, "SP1", 2, "DP1", "10", 2, 2417.02M, "Bbl", 6905.76M, 7420.88M, null, 10),
            };

            var consolidatedMovementRepository = new Mock<IRepository<ConsolidatedMovement>>();

            IEnumerable<ConsolidatedMovement> consolidatedMovement = new List<ConsolidatedMovement>();
            consolidatedMovementRepository.Setup(a => a.InsertAll(It.IsAny<IEnumerable<ConsolidatedMovement>>()))
                .Callback<IEnumerable<ConsolidatedMovement>>(list => consolidatedMovement = list);

            var consolidationMovementDataRepository = new Mock<IRepository<ConsolidationMovementData>>();
            consolidationMovementDataRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(consolidationMovementDatas);

            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidationMovementData>()).Returns(consolidationMovementDataRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidatedMovement>()).Returns(consolidatedMovementRepository.Object);

            await this.movementConsolidationStrategy.ConsolidateAsync(consolidationBatch, this.mockUnitOfWork.Object).ConfigureAwait(false);

            consolidationMovementDataRepository.Verify(a => a.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()), Times.AtLeastOnce);
            consolidatedMovementRepository.Verify(a => a.InsertAll(It.IsAny<IEnumerable<ConsolidatedMovement>>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<ConsolidationMovementData>(), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<ConsolidatedMovement>(), Times.Exactly(1));

            Assert.AreEqual(1, consolidatedMovement.Count());
            Assert.AreEqual(2, consolidatedMovement.First().ConsolidatedOwners.Count);
            Assert.AreEqual((int)SourceSystem.TRUE, consolidatedMovement.First().SourceSystemId);
            Assert.AreEqual(100.00M, consolidatedMovement.First().ConsolidatedOwners.Sum(a => a.OwnershipPercentage));

            Assert.AreEqual(25905.76M, consolidatedMovement.First().NetStandardVolume);
            Assert.AreEqual(15922.88M, consolidatedMovement.First().GrossStandardVolume);

            Assert.AreEqual(15888.74M, consolidatedMovement.First().ConsolidatedOwners.ElementAt(0).OwnershipVolume);
            Assert.AreEqual(10017.02M, consolidatedMovement.First().ConsolidatedOwners.ElementAt(1).OwnershipVolume);

            Assert.AreEqual(61.33M, consolidatedMovement.First().ConsolidatedOwners.ElementAt(0).OwnershipPercentage);
            Assert.AreEqual(38.67M, consolidatedMovement.First().ConsolidatedOwners.ElementAt(1).OwnershipPercentage);
        }

        /// <summary>
        /// Consolidates the asynchronous should consolidate movements for mix units when invoke asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ConsolidateAsync_Should_ConsolidateMovements_ForMixUnits_WhenInvokeAsync()
        {
            var consolidationBatch = new ConsolidationBatch
            {
                Ticket = new Entities.TransportBalance.Ticket
                {
                    TicketId = 123,
                    CategoryElementId = 123,
                    StartDate = new DateTime(2020, 07, 01),
                    EndDate = new DateTime(2020, 07, 05),
                },
            };

            var consolidationMovementDatas = new List<ConsolidationMovementData>
            {
                this.GetConsolidationMovementData(120, 1, "SP1", 2, "DP1", "10", 1, 61.33M, "%", 10500M, 10501M, null, 10),
                this.GetConsolidationMovementData(120, 1, "SP1", 2, "DP1", "10", 2, 38.67M, "%", 10500M, 10501M, null, 10),
                this.GetConsolidationMovementData(121, 1, "SP1", 2, "DP1", "10", 1, 5100M, "Bbl", 8500M, 8502M, null, 10),
                this.GetConsolidationMovementData(121, 1, "SP1", 2, "DP1", "10", 2, 3400M, "Bbl", 8500M, 8502M, null, 10),
                this.GetConsolidationMovementData(122, 1, "SP1", 2, "DP1", "10", 1, 4488.74M, "Bbl", 6905.76M, 7420.88M, null, 10),
                this.GetConsolidationMovementData(122, 1, "SP1", 2, "DP1", "10", 2, 2417.02M, "Bbl", 6905.76M, 7420.88M, null, 10),
            };

            var consolidatedMovementRepository = new Mock<IRepository<ConsolidatedMovement>>();

            IEnumerable<ConsolidatedMovement> consolidatedMovement = new List<ConsolidatedMovement>();
            consolidatedMovementRepository.Setup(a => a.InsertAll(It.IsAny<IEnumerable<ConsolidatedMovement>>()))
                .Callback<IEnumerable<ConsolidatedMovement>>(list => consolidatedMovement = list);

            var consolidationMovementDataRepository = new Mock<IRepository<ConsolidationMovementData>>();
            consolidationMovementDataRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(consolidationMovementDatas);

            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidationMovementData>()).Returns(consolidationMovementDataRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidatedMovement>()).Returns(consolidatedMovementRepository.Object);

            await this.movementConsolidationStrategy.ConsolidateAsync(consolidationBatch, this.mockUnitOfWork.Object).ConfigureAwait(false);

            consolidationMovementDataRepository.Verify(a => a.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()), Times.AtLeastOnce);
            consolidatedMovementRepository.Verify(a => a.InsertAll(It.IsAny<IEnumerable<ConsolidatedMovement>>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<ConsolidationMovementData>(), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<ConsolidatedMovement>(), Times.Exactly(1));

            Assert.AreEqual(1, consolidatedMovement.Count());
            Assert.AreEqual(2, consolidatedMovement.First().ConsolidatedOwners.Count);
            Assert.AreEqual((int)SourceSystem.TRUE, consolidatedMovement.First().SourceSystemId);
            Assert.AreEqual(100.00M, consolidatedMovement.First().ConsolidatedOwners.Sum(a => a.OwnershipPercentage));

            Assert.AreEqual(25905.76M, consolidatedMovement.First().NetStandardVolume);
            Assert.AreEqual(26423.88M, consolidatedMovement.First().GrossStandardVolume);

            Assert.AreEqual(16028.39M, consolidatedMovement.First().ConsolidatedOwners.ElementAt(0).OwnershipVolume);
            Assert.AreEqual(9877.37M, consolidatedMovement.First().ConsolidatedOwners.ElementAt(1).OwnershipVolume);

            Assert.AreEqual(61.87M, consolidatedMovement.First().ConsolidatedOwners.ElementAt(0).OwnershipPercentage);
            Assert.AreEqual(38.13M, consolidatedMovement.First().ConsolidatedOwners.ElementAt(1).OwnershipPercentage);
        }

        /// <summary>
        /// Consolidates the asynchronous should consolidate movements for no excluded movement case when unit is percentage when invoke asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ConsolidateAsync_Should_ConsolidateMovements_ForNoExcludedMovementCase_WhenUnitIsPercentage_WhenInvokeAsync()
        {
            var consolidationBatch = new ConsolidationBatch
            {
                Ticket = new Entities.TransportBalance.Ticket
                {
                    TicketId = 123,
                    CategoryElementId = 123,
                    StartDate = new DateTime(2020, 07, 01),
                    EndDate = new DateTime(2020, 07, 05),
                },
            };

            var consolidationMovementDatas = new List<ConsolidationMovementData>
            {
                this.GetConsolidationMovementData(120, 1, "SP1", 2, "DP1", "10", 1, 60M, "%", 10500M, 10501M, null, 10),
                this.GetConsolidationMovementData(120, 1, "SP1", 2, "DP1", "10", 2, 40M, "%", 10500M, 10501M, null, 10),
                this.GetConsolidationMovementData(121, 1, "SP1", 2, "DP1", "10", 1, 55M, "%", 8500M, 8502M, null, 10),
                this.GetConsolidationMovementData(121, 1, "SP1", 2, "DP1", "10", 2, 45M, "%", 8500M, 8502M, null, 10),
                this.GetConsolidationMovementData(122, 1, "SP1", 2, "DP1", "10", 1, 40.75M, "%", 6905.76M, 7420.88M, null, 10),
                this.GetConsolidationMovementData(122, 1, "SP1", 2, "DP1", "10", 2, 59.25M, "%", 6905.76M, 7420.88M, null, 10),
            };

            var consolidatedMovementRepository = new Mock<IRepository<ConsolidatedMovement>>();

            IEnumerable<ConsolidatedMovement> consolidatedMovement = new List<ConsolidatedMovement>();
            consolidatedMovementRepository.Setup(a => a.InsertAll(It.IsAny<IEnumerable<ConsolidatedMovement>>()))
                .Callback<IEnumerable<ConsolidatedMovement>>(list => consolidatedMovement = list);

            var consolidationMovementDataRepository = new Mock<IRepository<ConsolidationMovementData>>();
            consolidationMovementDataRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(consolidationMovementDatas);

            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidationMovementData>()).Returns(consolidationMovementDataRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidatedMovement>()).Returns(consolidatedMovementRepository.Object);

            await this.movementConsolidationStrategy.ConsolidateAsync(consolidationBatch, this.mockUnitOfWork.Object).ConfigureAwait(false);

            consolidationMovementDataRepository.Verify(a => a.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()), Times.AtLeastOnce);
            consolidatedMovementRepository.Verify(a => a.InsertAll(It.IsAny<IEnumerable<ConsolidatedMovement>>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<ConsolidationMovementData>(), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<ConsolidatedMovement>(), Times.Exactly(1));

            Assert.AreEqual(1, consolidatedMovement.Count());
            Assert.AreEqual(2, consolidatedMovement.First().ConsolidatedOwners.Count);
            Assert.AreEqual((int)SourceSystem.TRUE, consolidatedMovement.First().SourceSystemId);
            Assert.AreEqual(100.00M, consolidatedMovement.First().ConsolidatedOwners.Sum(a => a.OwnershipPercentage));

            Assert.AreEqual(25905.76M, consolidatedMovement.First().NetStandardVolume);
            Assert.AreEqual(26423.88M, consolidatedMovement.First().GrossStandardVolume);

            Assert.AreEqual(13789.10M, consolidatedMovement.First().ConsolidatedOwners.ElementAt(0).OwnershipVolume);
            Assert.AreEqual(12116.66M, consolidatedMovement.First().ConsolidatedOwners.ElementAt(1).OwnershipVolume);

            Assert.AreEqual(53.23M, consolidatedMovement.First().ConsolidatedOwners.ElementAt(0).OwnershipPercentage);
            Assert.AreEqual(46.77M, consolidatedMovement.First().ConsolidatedOwners.ElementAt(1).OwnershipPercentage);
        }

        /// <summary>
        /// Consolidates the asynchronous should throw invalid data exception for ownership not hundered percentage when grouped by movement transaction identifier when invoke asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task ConsolidateAsync_Should_ThrowInvalidDataException_ForOwnershipNotHunderedPercentage_when_GroupedByMovementTransactionId_WhenInvokeAsync()
        {
            var consolidationBatch = new ConsolidationBatch
            {
                Ticket = new Entities.TransportBalance.Ticket
                {
                    TicketId = 123,
                    CategoryElementId = 123,
                    StartDate = new DateTime(2020, 07, 01),
                    EndDate = new DateTime(2020, 07, 05),
                },
            };

            var consolidationMovementDatas = new List<ConsolidationMovementData>
            {
                this.GetConsolidationMovementData(120, 1, "SP1", 2, "DP1", "10", 1, 60M, "%", 10500M, 10501M, null, 10),
                this.GetConsolidationMovementData(120, 1, "SP1", 2, "DP1", "10", 2, 10M, "%", 10500M, 10501M, null, 10),
                this.GetConsolidationMovementData(121, 1, "SP1", 2, "DP1", "10", 1, 55M, "%", 8500M, 8502M, null, 10),
                this.GetConsolidationMovementData(121, 1, "SP1", 2, "DP1", "10", 2, 45M, "%", 8500M, 8502M, null, 10),
                this.GetConsolidationMovementData(122, 1, "SP1", 2, "DP1", "10", 1, 40.75M, "%", 6905.76M, 7420.88M, null, 10),
                this.GetConsolidationMovementData(122, 1, "SP1", 2, "DP1", "10", 2, 59.25M, "%", 6905.76M, 7420.88M, null, 10),
            };

            var consolidatedMovementRepository = new Mock<IRepository<ConsolidatedMovement>>();

            IEnumerable<ConsolidatedMovement> consolidatedMovement = new List<ConsolidatedMovement>();
            consolidatedMovementRepository.Setup(a => a.InsertAll(It.IsAny<IEnumerable<ConsolidatedMovement>>()))
                .Callback<IEnumerable<ConsolidatedMovement>>(list => consolidatedMovement = list);

            var consolidationMovementDataRepository = new Mock<IRepository<ConsolidationMovementData>>();
            consolidationMovementDataRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(consolidationMovementDatas);

            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidationMovementData>()).Returns(consolidationMovementDataRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidatedMovement>()).Returns(consolidatedMovementRepository.Object);

            await this.movementConsolidationStrategy.ConsolidateAsync(consolidationBatch, this.mockUnitOfWork.Object).ConfigureAwait(false);
        }

        /// <summary>
        /// Consolidates the asynchronous should throw invalid data exception for ownership not hundered percentage when unit is percentage and other when invoke asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task ConsolidateAsync_Should_ThrowInvalidDataException_ForOwnershipNotHunderedPercentage_when_UnitIsPercentageAndOther_WhenInvokeAsync()
        {
            var consolidationBatch = new ConsolidationBatch
            {
                Ticket = new Entities.TransportBalance.Ticket
                {
                    TicketId = 123,
                    CategoryElementId = 123,
                    StartDate = new DateTime(2020, 07, 01),
                    EndDate = new DateTime(2020, 07, 05),
                },
            };

            var consolidationMovementDatas = new List<ConsolidationMovementData>
            {
                this.GetConsolidationMovementData(120, 1, "SP1", 2, "DP1", "10", 1, 60M, "%", 10500M, 10501M, null, 10),
                this.GetConsolidationMovementData(120, 1, "SP1", 2, "DP1", "10", 2, 10M, "Bbl", 10500M, 10501M, null, 10),
                this.GetConsolidationMovementData(121, 1, "SP1", 2, "DP1", "10", 1, 55M, "%", 8500M, 8502M, null, 10),
                this.GetConsolidationMovementData(121, 1, "SP1", 2, "DP1", "10", 2, 45M, "%", 8500M, 8502M, null, 10),
                this.GetConsolidationMovementData(122, 1, "SP1", 2, "DP1", "10", 1, 40.75M, "%", 6905.76M, 7420.88M, null, 10),
                this.GetConsolidationMovementData(122, 1, "SP1", 2, "DP1", "10", 2, 59.25M, "%", 6905.76M, 7420.88M, null, 10),
            };

            var consolidatedMovementRepository = new Mock<IRepository<ConsolidatedMovement>>();

            IEnumerable<ConsolidatedMovement> consolidatedMovement = new List<ConsolidatedMovement>();
            consolidatedMovementRepository.Setup(a => a.InsertAll(It.IsAny<IEnumerable<ConsolidatedMovement>>()))
                .Callback<IEnumerable<ConsolidatedMovement>>(list => consolidatedMovement = list);

            var consolidationMovementDataRepository = new Mock<IRepository<ConsolidationMovementData>>();
            consolidationMovementDataRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(consolidationMovementDatas);

            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidationMovementData>()).Returns(consolidationMovementDataRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidatedMovement>()).Returns(consolidatedMovementRepository.Object);

            await this.movementConsolidationStrategy.ConsolidateAsync(consolidationBatch, this.mockUnitOfWork.Object).ConfigureAwait(false);
        }

        /// <summary>
        /// Consolidates the asynchronous should consolidate movements for excluded movement case when invoke asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ConsolidateAsync_Should_ConsolidateMovements_ForExcludedMovementCase_WhenInvokeAsync()
        {
            var consolidationBatch = new ConsolidationBatch
            {
                Ticket = new Entities.TransportBalance.Ticket
                {
                    TicketId = 123,
                    CategoryElementId = 123,
                    StartDate = new DateTime(2020, 07, 01),
                    EndDate = new DateTime(2020, 07, 05),
                },
            };

            var consolidationMovementDatas = new List<ConsolidationMovementData>
            {
                this.GetConsolidationMovementData(120, 1, "SP1", 2, "DP1", "10", 1, 6300M, "Bbl", 10500M, 10501M, null, 10),
                this.GetConsolidationMovementData(120, 1, "SP1", 2, "DP1", "10", 2, 4200M, "Bbl", 10500M, 10501M, null, 10),
                this.GetConsolidationMovementData(121, 1, "SP1", 2, "DP1", "10", 1, 5100M, "Bbl", 8500M, 8502M, 120, 10),
                this.GetConsolidationMovementData(121, 1, "SP1", 2, "DP1", "10", 2, 3400M, "Bbl", 8500M, 8502M, 120, 10),
                this.GetConsolidationMovementData(122, 1, "SP1", 2, "DP1", "10", 1, 4488.74M, "Bbl", 6905.76M, 7420.88M, 120, 10),
                this.GetConsolidationMovementData(122, 1, "SP1", 2, "DP1", "10", 2, 2417.02M, "Bbl", 6905.76M, 7420.88M, 120, 10),
            };

            var consolidatedMovementRepository = new Mock<IRepository<ConsolidatedMovement>>();

            IEnumerable<ConsolidatedMovement> consolidatedMovement = new List<ConsolidatedMovement>();
            consolidatedMovementRepository.Setup(a => a.InsertAll(It.IsAny<IEnumerable<ConsolidatedMovement>>()))
                .Callback<IEnumerable<ConsolidatedMovement>>(list => consolidatedMovement = list);

            var consolidationMovementDataRepository = new Mock<IRepository<ConsolidationMovementData>>();
            consolidationMovementDataRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(consolidationMovementDatas);

            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidationMovementData>()).Returns(consolidationMovementDataRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidatedMovement>()).Returns(consolidatedMovementRepository.Object);

            await this.movementConsolidationStrategy.ConsolidateAsync(consolidationBatch, this.mockUnitOfWork.Object).ConfigureAwait(false);

            consolidationMovementDataRepository.Verify(a => a.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()), Times.AtLeastOnce);
            consolidatedMovementRepository.Verify(a => a.InsertAll(It.IsAny<IEnumerable<ConsolidatedMovement>>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<ConsolidationMovementData>(), Times.AtLeastOnce);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<ConsolidatedMovement>(), Times.Exactly(1));

            Assert.AreEqual(1, consolidatedMovement.Count());
            Assert.AreEqual(2, consolidatedMovement.First().ConsolidatedOwners.Count);
            Assert.AreEqual((int)SourceSystem.TRUE, consolidatedMovement.First().SourceSystemId);
            Assert.AreEqual(100.00M, consolidatedMovement.First().ConsolidatedOwners.Sum(a => a.OwnershipPercentage));

            Assert.AreEqual(2000.00M, consolidatedMovement.First().NetStandardVolume);
            Assert.AreEqual(1999.00M, consolidatedMovement.First().GrossStandardVolume);

            Assert.AreEqual(1200.00M, consolidatedMovement.First().ConsolidatedOwners.ElementAt(0).OwnershipVolume);
            Assert.AreEqual(800.00M, consolidatedMovement.First().ConsolidatedOwners.ElementAt(1).OwnershipVolume);

            Assert.AreEqual(60.00M, consolidatedMovement.First().ConsolidatedOwners.ElementAt(0).OwnershipPercentage);
            Assert.AreEqual(40.00M, consolidatedMovement.First().ConsolidatedOwners.ElementAt(1).OwnershipPercentage);
        }

        /// <summary>
        /// Gets the consolidation movement data.
        /// </summary>
        /// <param name="movementTransactionId">The movement transaction identifier.</param>
        /// <param name="sourceNodeId">The source node identifier.</param>
        /// <param name="sourceProductId">The source product identifier.</param>
        /// <param name="destinationNodeId">The destination node identifier.</param>
        /// <param name="destinationProductId">The destination product identifier.</param>
        /// <param name="movementTypeId">The movement type identifier.</param>
        /// <param name="ownerId">The owner identifier.</param>
        /// <param name="ownershipVolume">The ownership volume.</param>
        /// <param name="ownershipValueUnit">The ownership value unit.</param>
        /// <param name="netQuantity">The net quantity.</param>
        /// <param name="grossQuantity">The gross quantity.</param>
        /// <param name="sourceMovementTransactionId">The source movement transaction identifier.</param>
        /// <param name="sourceMovementTypeId">The source movement type identifier.</param>
        /// <returns>The ConsolidationMovementData.</returns>
        private ConsolidationMovementData GetConsolidationMovementData(
            int movementTransactionId,
            int sourceNodeId,
            string sourceProductId,
            int destinationNodeId,
            string destinationProductId,
            string movementTypeId,
            int ownerId,
            decimal ownershipVolume,
            string ownershipValueUnit,
            decimal netQuantity,
            decimal? grossQuantity,
            int? sourceMovementTransactionId,
            int sourceMovementTypeId)
        {
            return new ConsolidationMovementData
            {
                MovementTransactionId = movementTransactionId,
                SourceNodeId = sourceNodeId,
                SourceProductId = sourceProductId,
                DestinationNodeId = destinationNodeId,
                DestinationProductId = destinationProductId,
                MovementTypeId = Convert.ToInt32(movementTypeId, CultureInfo.InvariantCulture),
                OwnerId = ownerId,
                OwnershipVolume = ownershipVolume,
                OwnershipValueUnit = ownershipValueUnit,
                GrossStandardVolume = grossQuantity,
                NetStandardVolume = netQuantity,
                OriginalMovementTransactionId = sourceMovementTransactionId,
                SourceMovementTypeId = sourceMovementTypeId,
            };
        }
    }
}
