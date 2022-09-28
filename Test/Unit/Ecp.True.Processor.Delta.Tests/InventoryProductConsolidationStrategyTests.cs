// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryProductConsolidationStrategyTests.cs" company="Microsoft">
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
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Delta.Consolidation;
    using Ecp.True.Processors.Delta.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Test class.
    /// </summary>
    [TestClass]
    public class InventoryProductConsolidationStrategyTests
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
        private InventoryProductConsolidationStrategy inventoryConsolidationStrategy;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.inventoryConsolidationStrategy = new InventoryProductConsolidationStrategy(this.mockLogger.Object);
        }

        /// <summary>
        /// Consolidates the asynchronous should consolidate movements asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ConsolidateAsync_Should_ConsolidateInventoryWithMatchingStartDateAsync()
        {
            var consolidationBatch = new ConsolidationBatch
            {
                Ticket = new Ticket { TicketId = 123, CategoryElementId = 123, StartDate = new DateTime(2020, 07, 01), EndDate = new DateTime(2020, 07, 06) },
                ShouldProcessInventory = true,
            };

            var consolidatedInventoriesData = new List<ConsolidationInventoryProductData>();
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2100, 1, "P1", 1, 13250M, 13251M, 7950M, "Bbl"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2100, 1, "P1", 2, 13250M, 13251M, 5300M, "Bbl"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2101, 1, "P1", 1, 15265M, 15266M, 10685.5M, "Bbl"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2101, 1, "P1", 2, 15265M, 15266M, 4579.5M, "Bbl"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2102, 1, "P1", 1, 9630M, 9640M, 5778M, "Bbl"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2102, 1, "P1", 2, 9630M, 9640M, 3852M, "Bbl"));

            var consolidatedInventoryProductRepository = new Mock<IRepository<ConsolidatedInventoryProduct>>();

            IEnumerable<ConsolidatedInventoryProduct> consolidatedInventory = new List<ConsolidatedInventoryProduct>();
            consolidatedInventoryProductRepository.Setup(a => a.InsertAll(It.IsAny<IEnumerable<ConsolidatedInventoryProduct>>()))
                .Callback<IEnumerable<ConsolidatedInventoryProduct>>(list => consolidatedInventory = list);

            var consolidationInventoryDataRepository = new Mock<IRepository<ConsolidationInventoryProductData>>();
            var parameters1 = new Dictionary<string, object>
            {
                { "@SegmentId", consolidationBatch.Ticket.CategoryElementId },
                { "@InventoryDate", consolidationBatch.Ticket.StartDate.AddDays(-1) },
            };
            consolidationInventoryDataRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<string>(), parameters1)).ReturnsAsync(consolidatedInventoriesData);
            var parameters2 = new Dictionary<string, object>
            {
                { "@SegmentId", consolidationBatch.Ticket.CategoryElementId },
                { "@InventoryDate", consolidationBatch.Ticket.EndDate },
            };
            consolidationInventoryDataRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<string>(), parameters2)).ReturnsAsync(new List<ConsolidationInventoryProductData>());

            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidatedInventoryProduct>()).Returns(consolidatedInventoryProductRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidationInventoryProductData>()).Returns(consolidationInventoryDataRepository.Object);

            await this.inventoryConsolidationStrategy.ConsolidateAsync(consolidationBatch, this.mockUnitOfWork.Object).ConfigureAwait(false);

            consolidationInventoryDataRepository.Verify(a => a.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()), Times.Exactly(2));
            consolidatedInventoryProductRepository.Verify(a => a.InsertAll(It.IsAny<IEnumerable<ConsolidatedInventoryProduct>>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<ConsolidationInventoryProductData>(), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<ConsolidatedInventoryProduct>(), Times.Exactly(2));

            Assert.AreEqual(1, consolidatedInventory.Count());
            Assert.AreEqual(2, consolidatedInventory.First().ConsolidatedOwners.Count);
            Assert.AreEqual((int)SourceSystem.TRUE, consolidatedInventory.First().SourceSystemId);

            Assert.AreEqual(38157M, consolidatedInventory.First().GrossStandardQuantity);
            Assert.AreEqual(38145M, consolidatedInventory.First().ProductVolume);

            Assert.AreEqual(100.00M, consolidatedInventory.First().ConsolidatedOwners.Sum(a => a.OwnershipPercentage));

            Assert.AreEqual(24413.50M, consolidatedInventory.First().ConsolidatedOwners.ElementAt(0).OwnershipVolume);
            Assert.AreEqual(13731.50M, consolidatedInventory.First().ConsolidatedOwners.ElementAt(1).OwnershipVolume);

            Assert.AreEqual(64.00M, consolidatedInventory.First().ConsolidatedOwners.ElementAt(0).OwnershipPercentage);
            Assert.AreEqual(36.00M, consolidatedInventory.First().ConsolidatedOwners.ElementAt(1).OwnershipPercentage);
        }

        /// <summary>
        /// Consolidates the asynchronous should consolidate inventory with matching start date mixed gross standard volume asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ConsolidateAsync_Should_ConsolidateInventoryWithMatchingStartDate_MixedGrossStandardVolumeAsync()
        {
            var consolidationBatch = new ConsolidationBatch
            {
                Ticket = new Ticket { TicketId = 123, CategoryElementId = 123, StartDate = new DateTime(2020, 07, 01), EndDate = new DateTime(2020, 07, 06) },
                ShouldProcessInventory = true,
            };

            var consolidatedInventoriesData = new List<ConsolidationInventoryProductData>();
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2100, 1, "P1", 1, 13250M, 15266M, 7950M, "Bbl"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2100, 1, "P1", 2, 13250M, null, 5300M, "Bbl"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2101, 1, "P1", 1, 15265M, null, 10685.5M, "Bbl"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2101, 1, "P1", 2, 15265M, 15266M, 4579.5M, "Bbl"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2102, 1, "P1", 1, 9630M, null, 5778M, "Bbl"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2102, 1, "P1", 2, 9630M, 9640M, 3852M, "Bbl"));

            var consolidatedInventoryProductRepository = new Mock<IRepository<ConsolidatedInventoryProduct>>();

            IEnumerable<ConsolidatedInventoryProduct> consolidatedInventory = new List<ConsolidatedInventoryProduct>();
            consolidatedInventoryProductRepository.Setup(a => a.InsertAll(It.IsAny<IEnumerable<ConsolidatedInventoryProduct>>()))
                .Callback<IEnumerable<ConsolidatedInventoryProduct>>(list => consolidatedInventory = list);

            var consolidationInventoryDataRepository = new Mock<IRepository<ConsolidationInventoryProductData>>();
            var parameters1 = new Dictionary<string, object>
            {
                { "@SegmentId", consolidationBatch.Ticket.CategoryElementId },
                { "@InventoryDate", consolidationBatch.Ticket.StartDate.AddDays(-1) },
            };
            consolidationInventoryDataRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<string>(), parameters1)).ReturnsAsync(consolidatedInventoriesData);
            var parameters2 = new Dictionary<string, object>
            {
                { "@SegmentId", consolidationBatch.Ticket.CategoryElementId },
                { "@InventoryDate", consolidationBatch.Ticket.EndDate },
            };
            consolidationInventoryDataRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<string>(), parameters2)).ReturnsAsync(new List<ConsolidationInventoryProductData>());

            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidatedInventoryProduct>()).Returns(consolidatedInventoryProductRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidationInventoryProductData>()).Returns(consolidationInventoryDataRepository.Object);

            await this.inventoryConsolidationStrategy.ConsolidateAsync(consolidationBatch, this.mockUnitOfWork.Object).ConfigureAwait(false);

            consolidationInventoryDataRepository.Verify(a => a.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()), Times.Exactly(2));
            consolidatedInventoryProductRepository.Verify(a => a.InsertAll(It.IsAny<IEnumerable<ConsolidatedInventoryProduct>>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<ConsolidationInventoryProductData>(), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<ConsolidatedInventoryProduct>(), Times.Exactly(2));

            Assert.AreEqual(1, consolidatedInventory.Count());
            Assert.AreEqual(2, consolidatedInventory.First().ConsolidatedOwners.Count);
            Assert.AreEqual((int)SourceSystem.TRUE, consolidatedInventory.First().SourceSystemId);

            Assert.AreEqual(15266M, consolidatedInventory.First().GrossStandardQuantity);
            Assert.AreEqual(38145M, consolidatedInventory.First().ProductVolume);

            Assert.AreEqual(100.00M, consolidatedInventory.First().ConsolidatedOwners.Sum(a => a.OwnershipPercentage));

            Assert.AreEqual(24413.50M, consolidatedInventory.First().ConsolidatedOwners.ElementAt(0).OwnershipVolume);
            Assert.AreEqual(13731.50M, consolidatedInventory.First().ConsolidatedOwners.ElementAt(1).OwnershipVolume);

            Assert.AreEqual(64.00M, consolidatedInventory.First().ConsolidatedOwners.ElementAt(0).OwnershipPercentage);
            Assert.AreEqual(36.00M, consolidatedInventory.First().ConsolidatedOwners.ElementAt(1).OwnershipPercentage);
        }

        /// <summary>
        /// Consolidates the asynchronous should consolidate inventory product with unit percentage for non son segment when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ConsolidateAsync_Should_ConsolidateInventoryProduct_WithUnitPercentage__For_NonSon_Segment_WhenInvokedAsync()
        {
            var consolidationBatch = new ConsolidationBatch
            {
                Ticket = new Ticket { TicketId = 123, CategoryElementId = 123, StartDate = new DateTime(2020, 07, 01), EndDate = new DateTime(2020, 07, 06) },
                ShouldProcessInventory = true,
            };

            var consolidatedInventoriesData = new List<ConsolidationInventoryProductData>();
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2100, 1, "P1", 1, 13250M, 13251M, 50M, "%"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2100, 1, "P1", 2, 13250M, 13251M, 50M, "%"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2101, 1, "P1", 1, 15265M, 15266M, 55M, "%"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2101, 1, "P1", 2, 15265M, 15266M, 45M, "%"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2102, 1, "P1", 1, 9630M, 9640M, 63M, "%"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2102, 1, "P1", 2, 9630M, 9640M, 37M, "%"));

            var consolidatedInventoryProductRepository = new Mock<IRepository<ConsolidatedInventoryProduct>>();

            IEnumerable<ConsolidatedInventoryProduct> consolidatedInventory = new List<ConsolidatedInventoryProduct>();
            consolidatedInventoryProductRepository.Setup(a => a.InsertAll(It.IsAny<IEnumerable<ConsolidatedInventoryProduct>>()))
                .Callback<IEnumerable<ConsolidatedInventoryProduct>>(list => consolidatedInventory = list);

            var consolidationInventoryDataRepository = new Mock<IRepository<ConsolidationInventoryProductData>>();
            var parameters1 = new Dictionary<string, object>
            {
                { "@SegmentId", consolidationBatch.Ticket.CategoryElementId },
                { "@InventoryDate", consolidationBatch.Ticket.StartDate.AddDays(-1) },
            };
            consolidationInventoryDataRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<string>(), parameters1)).ReturnsAsync(consolidatedInventoriesData);
            var parameters2 = new Dictionary<string, object>
            {
                { "@SegmentId", consolidationBatch.Ticket.CategoryElementId },
                { "@InventoryDate", consolidationBatch.Ticket.EndDate },
            };
            consolidationInventoryDataRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<string>(), parameters2)).ReturnsAsync(new List<ConsolidationInventoryProductData>());

            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidatedInventoryProduct>()).Returns(consolidatedInventoryProductRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidationInventoryProductData>()).Returns(consolidationInventoryDataRepository.Object);

            await this.inventoryConsolidationStrategy.ConsolidateAsync(consolidationBatch, this.mockUnitOfWork.Object).ConfigureAwait(false);

            consolidationInventoryDataRepository.Verify(a => a.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()), Times.Exactly(2));
            consolidatedInventoryProductRepository.Verify(a => a.InsertAll(It.IsAny<IEnumerable<ConsolidatedInventoryProduct>>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<ConsolidationInventoryProductData>(), Times.Exactly(1));
            this.mockUnitOfWork.Verify(a => a.CreateRepository<ConsolidatedInventoryProduct>(), Times.Exactly(2));

            Assert.AreEqual(1, consolidatedInventory.Count());
            Assert.AreEqual(38157M, consolidatedInventory.First().GrossStandardQuantity);
            Assert.AreEqual((int)SourceSystem.TRUE, consolidatedInventory.First().SourceSystemId);

            Assert.AreEqual(2, consolidatedInventory.First().ConsolidatedOwners.Count);
            Assert.AreEqual(100.00M, consolidatedInventory.First().ConsolidatedOwners.Sum(a => a.OwnershipPercentage));

            Assert.AreEqual(38145, consolidatedInventory.First().ProductVolume);

            Assert.AreEqual(21087.65M, consolidatedInventory.First().ConsolidatedOwners.ElementAt(0).OwnershipVolume);
            Assert.AreEqual(17057.35M, consolidatedInventory.First().ConsolidatedOwners.ElementAt(1).OwnershipVolume);

            Assert.AreEqual(55.28M, consolidatedInventory.First().ConsolidatedOwners.ElementAt(0).OwnershipPercentage);
            Assert.AreEqual(44.72M, consolidatedInventory.First().ConsolidatedOwners.ElementAt(1).OwnershipPercentage);
        }

        /// <summary>
        /// Consolidates the asynchronous should consolidate inventory product with unit percentage for son segment when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ConsolidateAsync_Should_ConsolidateInventoryProduct_WithUnitPercentage_For_Son_Segment_WhenInvokedAsync()
        {
            var consolidationBatch = new ConsolidationBatch
            {
                Ticket = new Ticket { TicketId = 123, CategoryElementId = 123, StartDate = new DateTime(2020, 07, 01), EndDate = new DateTime(2020, 07, 06) },
                ShouldProcessInventory = true,
            };

            var consolidatedInventoriesData = new List<ConsolidationInventoryProductData>();
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2100, 1, "P1", 1, 13250M, 13251M, 50M, "%"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2100, 1, "P1", 2, 13250M, 13251M, 50M, "%"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2101, 1, "P1", 1, 15265M, 15266M, 55M, "%"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2101, 1, "P1", 2, 15265M, 15266M, 45M, "%"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2102, 1, "P1", 1, 9630M, 9640M, 63M, "%"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2102, 1, "P1", 2, 9630M, 9640M, 37M, "%"));

            var mockConsolidatedInventoryProductRepository = new Mock<IRepository<ConsolidatedInventoryProduct>>();

            IEnumerable<ConsolidatedInventoryProduct> consolidatedInventory = new List<ConsolidatedInventoryProduct>();
            mockConsolidatedInventoryProductRepository.Setup(a => a.InsertAll(It.IsAny<IEnumerable<ConsolidatedInventoryProduct>>()))
                .Callback<IEnumerable<ConsolidatedInventoryProduct>>(list => consolidatedInventory = list);

            var consolidationInventoryDataRepository = new Mock<IRepository<ConsolidationInventoryProductData>>();
            var parameters1 = new Dictionary<string, object>
            {
                { "@SegmentId", consolidationBatch.Ticket.CategoryElementId },
                { "@InventoryDate", consolidationBatch.Ticket.StartDate.AddDays(-1) },
            };
            consolidationInventoryDataRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<string>(), parameters1)).ReturnsAsync(consolidatedInventoriesData);
            var parameters2 = new Dictionary<string, object>
            {
                { "@SegmentId", consolidationBatch.Ticket.CategoryElementId },
                { "@InventoryDate", consolidationBatch.Ticket.EndDate },
            };
            consolidationInventoryDataRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<string>(), parameters2)).ReturnsAsync(new List<ConsolidationInventoryProductData>());

            mockConsolidatedInventoryProductRepository.Setup(a => a.GetCountAsync(It.IsAny<Expression<Func<ConsolidatedInventoryProduct, bool>>>())).ReturnsAsync(0);

            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidatedInventoryProduct>()).Returns(mockConsolidatedInventoryProductRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidationInventoryProductData>()).Returns(consolidationInventoryDataRepository.Object);

            await this.inventoryConsolidationStrategy.ConsolidateAsync(consolidationBatch, this.mockUnitOfWork.Object).ConfigureAwait(false);

            consolidationInventoryDataRepository.Verify(a => a.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()), Times.Exactly(2));
            mockConsolidatedInventoryProductRepository.Verify(a => a.InsertAll(It.IsAny<IEnumerable<ConsolidatedInventoryProduct>>()), Times.Once);
            mockConsolidatedInventoryProductRepository.Verify(a => a.GetCountAsync(It.IsAny<Expression<Func<ConsolidatedInventoryProduct, bool>>>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<ConsolidationInventoryProductData>(), Times.Exactly(1));
            this.mockUnitOfWork.Verify(a => a.CreateRepository<ConsolidatedInventoryProduct>(), Times.Exactly(2));

            Assert.AreEqual(1, consolidatedInventory.Count());
            Assert.AreEqual(38157M, consolidatedInventory.First().GrossStandardQuantity);
            Assert.AreEqual((int)SourceSystem.TRUE, consolidatedInventory.First().SourceSystemId);

            Assert.AreEqual(2, consolidatedInventory.First().ConsolidatedOwners.Count);
            Assert.AreEqual(100.00M, consolidatedInventory.First().ConsolidatedOwners.Sum(a => a.OwnershipPercentage));

            Assert.AreEqual(38145, consolidatedInventory.First().ProductVolume);

            Assert.AreEqual(21087.65M, consolidatedInventory.First().ConsolidatedOwners.ElementAt(0).OwnershipVolume);
            Assert.AreEqual(17057.35M, consolidatedInventory.First().ConsolidatedOwners.ElementAt(1).OwnershipVolume);

            Assert.AreEqual(55.28M, consolidatedInventory.First().ConsolidatedOwners.ElementAt(0).OwnershipPercentage);
            Assert.AreEqual(44.72M, consolidatedInventory.First().ConsolidatedOwners.ElementAt(1).OwnershipPercentage);
        }

        /// <summary>
        /// Consolidates the asynchronous should consolidate movements asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task ConsolidateAsync__Should_ThrowInvalidDataException__ConsolidateInventoryProduct_WhenNotHundredPercentage_WhenInvokedAsync()
        {
            var consolidationBatch = new ConsolidationBatch
            {
                Ticket = new Entities.TransportBalance.Ticket { TicketId = 123, CategoryElementId = 123, StartDate = new DateTime(2020, 07, 01), EndDate = new DateTime(2020, 07, 06) },
                ShouldProcessInventory = true,
            };

            var consolidatedInventoriesData = new List<ConsolidationInventoryProductData>();
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2100, 1, "P1", 1, 13250M, 13251M, 50M, "%"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2100, 1, "P1", 2, 13250M, 13251M, 30M, "%"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2101, 1, "P1", 1, 15265M, 15266M, 55M, "%"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2101, 1, "P1", 2, 15265M, 15266M, 20M, "%"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2102, 1, "P1", 1, 9630M, 9640M, 63M, "%"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2102, 1, "P1", 2, 9630M, 9640M, 12M, "%"));

            var consolidatedInventoryProductRepository = new Mock<IRepository<ConsolidatedInventoryProduct>>();

            IEnumerable<ConsolidatedInventoryProduct> consolidatedInventory = new List<ConsolidatedInventoryProduct>();
            consolidatedInventoryProductRepository.Setup(a => a.InsertAll(It.IsAny<IEnumerable<ConsolidatedInventoryProduct>>()))
                .Callback<IEnumerable<ConsolidatedInventoryProduct>>(list => consolidatedInventory = list);

            var consolidationInventoryDataRepository = new Mock<IRepository<ConsolidationInventoryProductData>>();
            var parameters1 = new Dictionary<string, object>
            {
                { "@SegmentId", consolidationBatch.Ticket.CategoryElementId },
                { "@InventoryDate", consolidationBatch.Ticket.StartDate.AddDays(-1) },
            };
            consolidationInventoryDataRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<string>(), parameters1)).ReturnsAsync(consolidatedInventoriesData);
            var parameters2 = new Dictionary<string, object>
            {
                { "@SegmentId", consolidationBatch.Ticket.CategoryElementId },
                { "@InventoryDate", consolidationBatch.Ticket.EndDate },
            };
            consolidationInventoryDataRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<string>(), parameters2)).ReturnsAsync(new List<ConsolidationInventoryProductData>());

            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidatedInventoryProduct>()).Returns(consolidatedInventoryProductRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidationInventoryProductData>()).Returns(consolidationInventoryDataRepository.Object);

            await this.inventoryConsolidationStrategy.ConsolidateAsync(consolidationBatch, this.mockUnitOfWork.Object).ConfigureAwait(false);
        }

        /// <summary>
        /// Consolidates the asynchronous should consolidate movements asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task ConsolidateAsync__Should_ThrowInvalidDataException__ConsolidateInventoryProduct_WhenAllUnitsNotPercentage_WhenInvokedAsync()
        {
            var consolidationBatch = new ConsolidationBatch
            {
                Ticket = new Entities.TransportBalance.Ticket { TicketId = 123, CategoryElementId = 123, StartDate = new DateTime(2020, 07, 01), EndDate = new DateTime(2020, 07, 06) },
                ShouldProcessInventory = true,
            };

            var consolidatedInventoriesData = new List<ConsolidationInventoryProductData>();
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2100, 1, "P1", 1, 13250M, 13251M, 50M, "%"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2100, 1, "P1", 2, 13250M, 13251M, 30M, "Bbl"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2101, 1, "P1", 1, 15265M, 15266M, 55M, "%"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2101, 1, "P1", 2, 15265M, 15266M, 20M, "%"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2102, 1, "P1", 1, 9630M, 9640M, 63M, "Bbl"));
            consolidatedInventoriesData.Add(this.GetConsolidatedInventoryProductData(2102, 1, "P1", 2, 9630M, 9640M, 12M, "%"));

            var consolidatedInventoryProductRepository = new Mock<IRepository<ConsolidatedInventoryProduct>>();

            IEnumerable<ConsolidatedInventoryProduct> consolidatedInventory = new List<ConsolidatedInventoryProduct>();
            consolidatedInventoryProductRepository.Setup(a => a.InsertAll(It.IsAny<IEnumerable<ConsolidatedInventoryProduct>>()))
                .Callback<IEnumerable<ConsolidatedInventoryProduct>>(list => consolidatedInventory = list);

            var consolidationInventoryDataRepository = new Mock<IRepository<ConsolidationInventoryProductData>>();
            var parameters1 = new Dictionary<string, object>
            {
                { "@SegmentId", consolidationBatch.Ticket.CategoryElementId },
                { "@InventoryDate", consolidationBatch.Ticket.StartDate.AddDays(-1) },
            };
            consolidationInventoryDataRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<string>(), parameters1)).ReturnsAsync(consolidatedInventoriesData);
            var parameters2 = new Dictionary<string, object>
            {
                { "@SegmentId", consolidationBatch.Ticket.CategoryElementId },
                { "@InventoryDate", consolidationBatch.Ticket.EndDate },
            };
            consolidationInventoryDataRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<string>(), parameters2)).ReturnsAsync(new List<ConsolidationInventoryProductData>());

            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidatedInventoryProduct>()).Returns(consolidatedInventoryProductRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidationInventoryProductData>()).Returns(consolidationInventoryDataRepository.Object);

            await this.inventoryConsolidationStrategy.ConsolidateAsync(consolidationBatch, this.mockUnitOfWork.Object).ConfigureAwait(false);
        }

        /// <summary>
        /// Consolidates the asynchronous should when matching end date and inventory does not exist exist asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ConsolidateAsync_Should_WhenMatchingEndDateAndInventoryDoesNotExistExistAsync()
        {
            var consolidationBatch = new ConsolidationBatch
            {
                Ticket = new Ticket { TicketId = 123, CategoryElementId = 123, StartDate = new DateTime(2020, 07, 02), EndDate = new DateTime(2020, 07, 05) },
                ShouldProcessInventory = true,
            };

            var consolidationInventoryDataRepository = new Mock<IRepository<ConsolidationInventoryProductData>>();
            consolidationInventoryDataRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(new List<ConsolidationInventoryProductData>());
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidationInventoryProductData>()).Returns(consolidationInventoryDataRepository.Object);

            var mockConsolidatedInventoryProductRepository = new Mock<IRepository<ConsolidatedInventoryProduct>>();
            mockConsolidatedInventoryProductRepository.Setup(a => a.GetCountAsync(It.IsAny<Expression<Func<ConsolidatedInventoryProduct, bool>>>())).ReturnsAsync(1);

            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidatedInventoryProduct>()).Returns(mockConsolidatedInventoryProductRepository.Object);

            await this.inventoryConsolidationStrategy.ConsolidateAsync(consolidationBatch, this.mockUnitOfWork.Object).ConfigureAwait(false);

            consolidationInventoryDataRepository.Verify(a => a.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
            mockConsolidatedInventoryProductRepository.Verify(a => a.InsertAll(It.IsAny<IEnumerable<ConsolidatedInventoryProduct>>()), Times.Once);
            mockConsolidatedInventoryProductRepository.Verify(a => a.GetCountAsync(It.IsAny<Expression<Func<ConsolidatedInventoryProduct, bool>>>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<ConsolidationInventoryProductData>(), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<ConsolidatedInventoryProduct>(), Times.Exactly(2));
        }

        /// <summary>
        /// Gets the consolidated inventory product data.
        /// </summary>
        /// <param name="inventoryProductId">The inventory product identifier.</param>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="ownerId">The owner identifier.</param>
        /// <param name="productVolume">The product volume.</param>
        /// <param name="grossStandardQuantity">The gross standard quantity.</param>
        /// <param name="ownershipVolume">The ownership volume.</param>
        /// <param name="ownershipValueUnit">The ownership value unit.</param>
        /// <returns>The ConsolidationInventoryProductData.</returns>
        private ConsolidationInventoryProductData GetConsolidatedInventoryProductData(
        int inventoryProductId,
        int nodeId,
        string productId,
        int ownerId,
        decimal productVolume,
        decimal? grossStandardQuantity,
        decimal ownershipVolume,
        string ownershipValueUnit)
        {
            return new ConsolidationInventoryProductData
            {
                InventoryProductId = inventoryProductId,
                NodeId = nodeId,
                ProductId = productId,
                OwnerId = ownerId,
                ProductVolume = productVolume,
                GrossStandardQuantity = grossStandardQuantity,
                OwnershipVolume = ownershipVolume,
                OwnershipValueUnit = ownershipValueUnit,
            };
        }
    }
}
