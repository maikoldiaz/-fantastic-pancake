// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialCalculateExecutorTests.cs" company="Microsoft">
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
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Delta.Calculation;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Processors.Delta.OfficialDeltaExecutors;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Calculate Executor Tests.
    /// </summary>
    [TestClass]
    public class OfficialCalculateExecutorTests
    {
        /// <summary>
        /// The mock registration strategy factory.
        /// </summary>
        private readonly Mock<IDeltaBalanceCalculator> deltaBalanceCalculator = new Mock<IDeltaBalanceCalculator>();

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<CalculateExecutor>> mockLogger;

        /// <summary>
        /// The delta data.
        /// </summary>
        private OfficialDeltaData deltaData;

        /// <summary>
        /// The build executor.
        /// </summary>
        private IExecutor calculateExecutor;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock inventory repository.
        /// </summary>
        private Mock<IRepository<DeltaBalance>> mockDeltaBalanceRepository;

        /// <summary>
        /// The mock i unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockIUnitOfWorkFactory;

        /// <summary>
        /// The mock inventory product repository.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigurationHandler;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockIUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockDeltaBalanceRepository = new Mock<IRepository<DeltaBalance>>();
            this.mockLogger = new Mock<ITrueLogger<CalculateExecutor>>();
            this.mockConfigurationHandler = new Mock<IConfigurationHandler>();
            this.mockIUnitOfWorkFactory.Setup(x => x.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<ServiceBusSettings>(It.IsAny<string>())).ReturnsAsync(new ServiceBusSettings());
            this.mockUnitOfWork.Setup(x => x.CreateRepository<DeltaBalance>()).Returns(this.mockDeltaBalanceRepository.Object);
            this.mockDeltaBalanceRepository.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<DeltaBalance, bool>>>())).ReturnsAsync(new List<DeltaBalance>());

            this.deltaData = new OfficialDeltaData()
            {
                Ticket = new Ticket { TicketId = 123 },
                PendingOfficialMovements = new List<PendingOfficialMovement>
                {
                    new PendingOfficialMovement
                    {
                        SourceNodeId = 1,
                        SegmentId = 1,
                        SourceNodeSegmentId = 1,
                        DestinationNodeId = 2,
                        DestinationNodeSegmentID = 1,
                    },
                },
                PendingOfficialInventories = new List<PendingOfficialInventory>
                {
                    new PendingOfficialInventory
                    {
                        NodeId = 3,
                    },
                },
                ConsolidationInventories = new List<ConsolidatedInventoryProduct>
                {
                    new ConsolidatedInventoryProduct
                    {
                        NodeId = 3,
                        ProductId = "3",
                        MeasurementUnit = "31",
                    },
                },
                ConsolidationMovements = new List<ConsolidatedMovement>
                {
                    new ConsolidatedMovement
                    {
                        SourceNodeId = 1,
                        SourceProductId = "1",
                        DestinationNodeId = 2,
                        DestinationProductId = "2",
                    },
                },
            };

            this.deltaData.ConsolidationMovements.ElementAt(0).ConsolidatedOwners.Add(
                    new ConsolidatedOwner
                    {
                        OwnerId = 1,
                        OwnershipVolume = 100,
                    });
            this.deltaData.ConsolidationMovements.ElementAt(0).ConsolidatedOwners.Add(
                    new ConsolidatedOwner
                    {
                        OwnerId = 2,
                        OwnershipVolume = 200,
                    });
            this.deltaData.ConsolidationInventories.ElementAt(0).ConsolidatedOwners.Add(
                    new ConsolidatedOwner
                    {
                        OwnerId = 1,
                        OwnershipVolume = 300,
                    });

            this.deltaData.ConsolidationInventories.ElementAt(0).ConsolidatedOwners.Add(
                    new ConsolidatedOwner
                    {
                        OwnerId = 2,
                        OwnershipVolume = 400,
                    });

            this.calculateExecutor = new CalculateExecutor(
                this.deltaBalanceCalculator.Object,
                this.mockIUnitOfWorkFactory.Object,
                this.mockLogger.Object);
        }

        /// <summary>
        /// Executes the asynchronous build executor asynchronous.
        /// </summary>
        /// <returns>Delta data.</returns>
        [TestMethod]
        public async Task ExecuteAsync_CalculateExecutorAsync()
        {
            var movements = new List<Movement>
            {
                new Movement
                {
                    MovementTransactionId = 1,
                    MeasurementUnit = 31,
                    MovementSource = new MovementSource
                    {
                        SourceNodeId = 1,
                        SourceProductId = "1",
                    },
                    MovementDestination = new MovementDestination
                    {
                        DestinationNodeId = 2,
                        DestinationProductId = "2",
                    },
                    Period = new MovementPeriod
                    {
                        StartTime = new DateTime(2020, 6, 1),
                        EndTime = new DateTime(2020, 6, 30),
                    },
                },
                new Movement
                {
                    MovementTransactionId = 2,
                    MeasurementUnit = 31,
                    MovementSource = new MovementSource
                    {
                        SourceNodeId = 3,
                        SourceProductId = "3",
                    },
                    MovementDestination = new MovementDestination
                    {
                        DestinationNodeId = 4,
                        DestinationProductId = "4",
                    },
                    Period = new MovementPeriod
                    {
                        StartTime = new DateTime(2020, 6, 1),
                        EndTime = new DateTime(2020, 6, 30),
                    },
                },
                new Movement
                {
                    MovementTransactionId = 3,
                    MeasurementUnit = 31,
                    MovementSource = new MovementSource
                    {
                        SourceNodeId = 3,
                        SourceProductId = "3",
                    },
                    MovementDestination = new MovementDestination
                    {
                        DestinationNodeId = 3,
                        DestinationProductId = "3",
                    },
                    Period = new MovementPeriod
                    {
                        StartTime = new DateTime(2020, 6, 1),
                        EndTime = new DateTime(2020, 6, 30),
                    },
                },
            };

            movements[0].Owners.Add(
            new Owner
            {
                OwnerId = 1,
                OwnershipValue = 100,
            });

            movements[0].Owners.Add(
            new Owner
            {
                OwnerId = 2,
                OwnershipValue = 200,
            });

            movements[1].Owners.Add(
            new Owner
            {
                OwnerId = 1,
                OwnershipValue = 300,
            });

            movements[1].Owners.Add(
            new Owner
            {
                OwnerId = 2,
                OwnershipValue = 400,
            });

            movements[2].Owners.Add(
            new Owner
            {
                OwnerId = 1,
                OwnershipValue = 300,
            });

            movements[2].Owners.Add(
            new Owner
            {
                OwnerId = 2,
                OwnershipValue = 400,
            });

            this.mockUnitOfWork.Setup(m => m.MovementRepository.GetMovementsForOfficialDeltaCalculationAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<Ticket>())).ReturnsAsync(movements);

            this.mockUnitOfWork.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));
            this.deltaBalanceCalculator.Setup(m => m.Calculate(It.IsAny<Ticket>(), It.IsAny<ConcurrentDictionary<string, List<Movement>>>(), It.IsAny<ConcurrentDictionary<string, List<ConsolidatedMovement>>>(), It.IsAny<ConcurrentDictionary<string, List<ConsolidatedInventoryProduct>>>()))
                .Returns(new List<DeltaBalance>());

            await this.calculateExecutor.ExecuteAsync(this.deltaData).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Builds the result executor should return the execution order when invoked.
        /// </summary>
        [TestMethod]
        public void CalculateExecutor_ShouldReturnTheExecutionOrder_WhenInvoked()
        {
            Assert.AreEqual(6, this.calculateExecutor.Order);
        }

        /// <summary>
        /// Builds the result executor should return the process type when invoked.
        /// </summary>
        [TestMethod]
        public void CalculateExecutor_ShouldReturnTheProcessType_WhenInvoked()
        {
            Assert.AreEqual(ProcessType.OfficialDelta, this.calculateExecutor.ProcessType);
        }
    }
}
