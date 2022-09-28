// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipProcessorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Ownership.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core.Entities;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.DataAccess.Sql.Interfaces;
    using Ecp.True.Entities;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Conciliation.Interfaces;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Core.Registration;
    using Ecp.True.Processors.Ownership;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Processors.Ownership.Services;
    using Ecp.True.Processors.Ownership.Services.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.OwnershipRules.Request;
    using Ecp.True.Proxies.OwnershipRules.Response;
    using Ecp.True.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The OwnershipProcessorTests.
    /// </summary>
    [TestClass]
    public class OwnershipProcessorTests
    {
        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The mock repository factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockRepositoryFactory;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The mock ownership service.
        /// </summary>
        private Mock<IOwnershipService> mockOwnershipService;

        /// <summary>
        /// The mock calculate ownership.
        /// </summary>
        private Mock<ICalculateOwnership> mockCalculateOwnership;

        /// <summary>
        /// The mock calculate ownership.
        /// </summary>
        private Mock<ISegmentOwnershipCalculationService> mockSegmentOwnershipCalculationService;

        /// <summary>
        /// The mock calculate ownership.
        /// </summary>
        private Mock<ISystemOwnershipCalculationService> mockSystemOwnershipCalculationService;

        /// <summary>
        /// The mock ownership calculation repository.
        /// </summary>
        private Mock<IRepository<OwnershipCalculation>> mockOwnershipCalculationRepository;

        /// <summary>
        /// The mock ownership calculation repository.
        /// </summary>
        private Mock<IRepository<SegmentOwnershipCalculation>> mockSegmentOwnershipCalculationRepository;

        /// <summary>
        /// The mock ownership calculation repository.
        /// </summary>
        private Mock<IRepository<SystemOwnershipCalculation>> mockSystemOwnershipCalculationRepository;

        /// <summary>
        /// The mock ownership calculation result repository.
        /// </summary>
        private Mock<IRepository<OwnershipCalculationResult>> mockOwnershipCalculationResultRepository;

        /// <summary>
        /// The mock movement repository.
        /// </summary>
        private Mock<IRepository<Movement>> mockMovementRepository;

        /// <summary>
        /// The mock ticket repository.
        /// </summary>
        private Mock<IRepository<Ticket>> mockTicketRepository;

        /// <summary>
        /// The mock inventory product repository.
        /// </summary>
        private Mock<IRepository<InventoryProduct>> mockInventoryProductRepository;

        /// <summary>
        /// The mock Conciliation Nodes Result.
        /// </summary>
        private Mock<IRepository<ConciliationNodesResult>> mockConciliationNodesResult;

        /// <summary>
        /// The unit of ticket repository factory.
        /// </summary>
        private Mock<IRepository<OwnershipNode>> mockOwnershipNodeRepository;

        /// <summary>
        /// The mock inventory product repository.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigurationHandler;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The ownership processor.
        /// </summary>
        private OwnershipProcessor ownershipProcessor;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<ITrueLogger<OwnershipProcessor>> mockLogger;

        /// <summary>
        /// The ownership rule data.
        /// </summary>
        private OwnershipRuleData ownershipRuleData;

        /// <summary>
        /// The ownership rule request.
        /// </summary>
        private OwnershipRuleRequest ownershipRuleRequest;

        /// <summary>
        /// The ownership rule response.
        /// </summary>
        private OwnershipRuleResponse ownershipRuleResponse;

        /// <summary>
        /// The mock movement registration service.
        /// </summary>
        private Mock<IMovementRegistrationService> mockMovementRegistrationService;

        /// <summary>
        /// The mock service bus queue client.
        /// </summary>
        private Mock<IServiceBusQueueClient> mockServiceBusQueueClient;

        /// <summary>
        /// The mock ownership conciliation.
        /// </summary>
        private Mock<IConciliationProcessor> ownershipConciliation;

        /// <summary>
        /// Intilizes the method.
        /// </summary>
        [TestInitialize]
        public void IntilizeMethod()
        {
            this.mockRepositoryFactory = new Mock<IRepositoryFactory>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockOwnershipService = new Mock<IOwnershipService>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockMovementRepository = new Mock<IRepository<Movement>>();
            this.mockInventoryProductRepository = new Mock<IRepository<InventoryProduct>>();
            this.mockTicketRepository = new Mock<IRepository<Ticket>>();
            this.mockCalculateOwnership = new Mock<ICalculateOwnership>();
            this.mockSegmentOwnershipCalculationService = new Mock<ISegmentOwnershipCalculationService>();
            this.mockSystemOwnershipCalculationService = new Mock<ISystemOwnershipCalculationService>();
            this.mockConfigurationHandler = new Mock<IConfigurationHandler>();
            this.mockLogger = new Mock<ITrueLogger<OwnershipProcessor>>();
            this.mockOwnershipNodeRepository = new Mock<IRepository<OwnershipNode>>();
            this.mockMovementRegistrationService = new Mock<IMovementRegistrationService>();
            this.mockServiceBusQueueClient = new Mock<IServiceBusQueueClient>();
            this.mockConciliationNodesResult = new Mock<IRepository<ConciliationNodesResult>>();
            this.ownershipConciliation = new Mock<IConciliationProcessor>();
            this.mockAzureClientFactory.Setup(x => x.GetQueueClient(It.IsAny<string>())).Returns(this.mockServiceBusQueueClient.Object);
            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<ServiceBusSettings>(It.IsAny<string>())).ReturnsAsync(new ServiceBusSettings());
            this.mockOwnershipCalculationRepository = new Mock<IRepository<OwnershipCalculation>>();
            this.mockSegmentOwnershipCalculationRepository = new Mock<IRepository<SegmentOwnershipCalculation>>();
            this.mockSystemOwnershipCalculationRepository = new Mock<IRepository<SystemOwnershipCalculation>>();
            this.mockOwnershipCalculationResultRepository = new Mock<IRepository<OwnershipCalculationResult>>();

            this.mockOwnershipCalculationRepository.Setup(a => a.Insert(It.IsAny<OwnershipCalculation>()));
            this.mockSegmentOwnershipCalculationRepository.Setup(a => a.Insert(It.IsAny<SegmentOwnershipCalculation>()));
            this.mockSystemOwnershipCalculationRepository.Setup(a => a.Insert(It.IsAny<SystemOwnershipCalculation>()));
            this.mockOwnershipCalculationResultRepository.Setup(a => a.Insert(It.IsAny<OwnershipCalculationResult>()));

            this.mockUnitOfWork.Setup(a => a.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<InventoryProduct>()).Returns(this.mockInventoryProductRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConciliationNodesResult>()).Returns(this.mockConciliationNodesResult.Object);
            this.mockUnitOfWorkFactory.Setup(a => a.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<OwnershipCalculation>()).Returns(this.mockOwnershipCalculationRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<SegmentOwnershipCalculation>()).Returns(this.mockSegmentOwnershipCalculationRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<SystemOwnershipCalculation>()).Returns(this.mockSystemOwnershipCalculationRepository.Object);

            this.mockUnitOfWork.Setup(a => a.CreateRepository<OwnershipCalculationResult>()).Returns(this.mockOwnershipCalculationResultRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<OwnershipNode>()).Returns(this.mockOwnershipNodeRepository.Object);
            this.mockUnitOfWork.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));
            this.ownershipProcessor = new OwnershipProcessor(this.mockUnitOfWorkFactory.Object, this.mockOwnershipService.Object, this.mockCalculateOwnership.Object, this.mockSegmentOwnershipCalculationService.Object, this.mockSystemOwnershipCalculationService.Object, this.mockAzureClientFactory.Object, this.mockRepositoryFactory.Object, this.mockLogger.Object);
        }

        /// <summary>
        /// Calculate the ownership asynchronous should calculate ownership.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CalculateOwnershipAsync_ShouldBeSuccesfullAsync()
        {
            var ownershipTicketId = 1000;

            this.SetupData(ownershipTicketId);

            this.mockTicketRepository.Setup(a => a.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(new Ticket { TicketId = ownershipTicketId, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(1) });
            this.mockTicketRepository.Setup(a => a.Update(It.IsAny<Ticket>()));

            this.mockOwnershipService.Setup(a => a.RegisterResultsAsync(It.IsAny<OwnershipRuleData>()));

            await this.ownershipProcessor.CalculateOwnershipAsync(ownershipTicketId).ConfigureAwait(false);

            this.mockMovementRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockInventoryProductRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>()), Times.Once);
        }

        [TestMethod]
        public async Task CalculateOwnershipAsync_ShouldBeSuccesfulAsync()
        {
            var ownershipTicketId = 1000;

            var mockAuditService = new Mock<IAuditService>();
            var businessContext = new Mock<IBusinessContext>();
            var sqlTokenProvider = new Mock<ISqlTokenProvider>();

            mockAuditService.Setup(m => m.GetAuditLogs(It.IsAny<ChangeTracker>())).Returns(new List<AuditLog>());

            var options = new DbContextOptionsBuilder<SqlDataContext>()
                                   .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                                   .Options;

            using (var dataContext = new SqlDataContext(options, mockAuditService.Object, businessContext.Object, sqlTokenProvider.Object))
            {
                var repositoryFactory = new RepositoryFactory(dataContext);

                await this.SetUpMockDataAsync(ownershipTicketId, repositoryFactory, dataContext).ConfigureAwait(false);

                var unitOfWorkFactory = new UnitOfWorkFactory(dataContext, repositoryFactory);
                var inventoryOwnershipService = new InventoryOwnershipService();
                var movementOwnershipService = new MovementOwnershipService();
                var mockRegistrationStrategyFactoryLogger = new Mock<ITrueLogger<RegistrationStrategyFactory>>();
                var registrationStrategyFactory = new RegistrationStrategyFactory(mockRegistrationStrategyFactoryLogger.Object, this.mockAzureClientFactory.Object, this.mockMovementRegistrationService.Object);
                var ownershipResultService = new OwnershipResultService();

                var mockOwnershipServiceLogger = new Mock<ITrueLogger<OwnershipService>>();
                var ownershipService = new OwnershipService(mockOwnershipServiceLogger.Object, unitOfWorkFactory, inventoryOwnershipService, movementOwnershipService, registrationStrategyFactory, ownershipResultService);
                var calculateOwnership = new CalculateOwnership();
                var mockOwnershipProcessorLogger = new Mock<ITrueLogger<OwnershipProcessor>>();

                var ownershipProcessorObject = new OwnershipProcessor(
                    unitOfWorkFactory,
                    ownershipService,
                    calculateOwnership,
                    this.mockSegmentOwnershipCalculationService.Object,
                    this.mockSystemOwnershipCalculationService.Object,
                    this.mockAzureClientFactory.Object,
                    this.mockRepositoryFactory.Object,
                    mockOwnershipProcessorLogger.Object);

                var result = await ownershipProcessorObject.CalculateOwnershipAsync(ownershipTicketId).ConfigureAwait(false);

                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Item1);
                Assert.IsNotNull(result.Item2);
                Assert.IsNotNull(result.Item2);
            }
        }

        /// <summary>
        /// Complete the process.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CompleteAsync_ShouldBeSuccesfullAsync()
        {
            var ownershipTicketId = 1000;

            this.SetupData(ownershipTicketId);

            this.mockTicketRepository.Setup(a => a.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(new Ticket { TicketId = ownershipTicketId });
            this.mockTicketRepository.Setup(a => a.Update(It.IsAny<Ticket>()));
            this.mockOwnershipService.Setup(a => a.RegisterResultsAsync(It.IsAny<OwnershipRuleData>()));

            this.ownershipRuleRequest = new OwnershipRuleRequest();
            this.ownershipRuleResponse = new OwnershipRuleResponse();
            this.ownershipRuleData = new OwnershipRuleData() { TicketId = 25281 };
            this.ownershipRuleData.OwnershipRuleRequest = this.ownershipRuleRequest;
            this.ownershipRuleData.OwnershipRuleResponse = new OwnershipRuleResponse
            {
                InventoryResults = new List<OwnershipResultInventory>(),
                MovementResults = new List<OwnershipResultMovement>(),
            };
            this.ownershipRuleData.OwnershipCalculations = new List<OwnershipCalculation>
            {
                new OwnershipCalculation
                {
                    OwnershipCalculationId = 1,
                },
            };

            this.ownershipRuleData.SegmentOwnershipCalculations = new List<SegmentOwnershipCalculation>
            {
                new SegmentOwnershipCalculation
                {
                    SegmentOwnershipCalculationId = 1,
                },
            };

            this.ownershipRuleData.SystemOwnershipCalculations = new List<SystemOwnershipCalculation>
            {
                new SystemOwnershipCalculation
                {
                    SystemOwnershipCalculationId = 1,
                },
            };

            await this.ownershipProcessor.CompleteAsync(this.ownershipRuleData).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<OwnershipCalculation>(), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<SegmentOwnershipCalculation>(), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<SystemOwnershipCalculation>(), Times.Once);
        }

        /// <summary>
        /// Completes the asynchronous should fail subsequent current and tickets for push message to service bus fails asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CompleteAsync_ShouldFailSubsequentCurrentAndTickets_For_PushMessageToServiceBusFailsAsync()
        {
            var ownershipTicketId = 1000;

            this.SetupData(ownershipTicketId);

            this.mockTicketRepository.Setup(a => a.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(new Ticket { TicketId = ownershipTicketId });
            this.mockTicketRepository.Setup(a => a.Update(It.IsAny<Ticket>()));
            this.mockOwnershipService.Setup(a => a.RegisterResultsAsync(It.IsAny<OwnershipRuleData>()));
            var unprocessedTickets = new List<Ticket>
            {
                new Ticket { TicketId = 1, StartDate = DateTime.UtcNow.AddMinutes(1) },
                new Ticket { TicketId = 2, StartDate = DateTime.UtcNow.AddMinutes(2) },
                new Ticket { TicketId = 3, StartDate = DateTime.UtcNow.AddMinutes(3) },
            };
            this.mockOwnershipService.Setup(a => a.GetUnprocessedTicketsAsync(It.IsAny<int>())).ReturnsAsync(unprocessedTickets);
            this.mockOwnershipService.Setup(a => a.HandleFailureAsync(It.IsAny<int>(), It.IsAny<IEnumerable<ErrorInfo>>(), It.IsAny<IEnumerable<OwnershipErrorMovement>>(), It.IsAny<IEnumerable<OwnershipErrorInventory>>(), It.IsAny<bool>()));
            this.mockServiceBusQueueClient.Setup(a => a.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>())).ThrowsAsync(new Exception());

            this.ownershipRuleRequest = new OwnershipRuleRequest();
            this.ownershipRuleResponse = new OwnershipRuleResponse();
            this.ownershipRuleData = new OwnershipRuleData() { TicketId = 25281 };
            this.ownershipRuleData.OwnershipRuleRequest = this.ownershipRuleRequest;
            this.ownershipRuleData.OwnershipRuleResponse = new OwnershipRuleResponse
            {
                InventoryResults = new List<OwnershipResultInventory>(),
                MovementResults = new List<OwnershipResultMovement>(),
            };
            this.ownershipRuleData.OwnershipCalculations = new List<OwnershipCalculation>
            {
                new OwnershipCalculation
                {
                    OwnershipCalculationId = 1,
                },
            };

            this.ownershipRuleData.SegmentOwnershipCalculations = new List<SegmentOwnershipCalculation>
            {
                new SegmentOwnershipCalculation
                {
                    SegmentOwnershipCalculationId = 1,
                },
            };

            this.ownershipRuleData.SystemOwnershipCalculations = new List<SystemOwnershipCalculation>
            {
                new SystemOwnershipCalculation
                {
                    SystemOwnershipCalculationId = 1,
                },
            };

            await this.ownershipProcessor.CompleteAsync(this.ownershipRuleData).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<OwnershipCalculation>(), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<SegmentOwnershipCalculation>(), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<SystemOwnershipCalculation>(), Times.Once);

            this.mockOwnershipService.Verify(a => a.GetUnprocessedTicketsAsync(It.IsAny<int>()), Times.Once);
            this.mockOwnershipService.Verify(a => a.HandleFailureAsync(It.IsAny<int>(), It.IsAny<IEnumerable<ErrorInfo>>(), It.IsAny<IEnumerable<OwnershipErrorMovement>>(), It.IsAny<IEnumerable<OwnershipErrorInventory>>(), It.IsAny<bool>()), Times.Once);
            this.mockServiceBusQueueClient.Verify(a => a.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Complete the process.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CompleteAsync_ShouldUpdateOwnershipNodeAsync()
        {
            var ownershipTicketId = 1000;

            this.SetupData(ownershipTicketId);

            this.mockOwnershipNodeRepository.Setup(a => a.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new OwnershipNode());
            this.mockOwnershipNodeRepository.Setup(a => a.Update(It.IsAny<OwnershipNode>()));
            this.mockOwnershipService.Setup(a => a.RegisterResultsAsync(It.IsAny<OwnershipRuleData>()));

            this.ownershipRuleRequest = new OwnershipRuleRequest();
            this.ownershipRuleResponse = new OwnershipRuleResponse();
            this.ownershipRuleData = new OwnershipRuleData() { TicketId = 25281, OwnershipNodeId = 1 };
            this.ownershipRuleData.OwnershipRuleRequest = this.ownershipRuleRequest;
            this.ownershipRuleData.OwnershipRuleResponse = new OwnershipRuleResponse
            {
                InventoryResults = new List<OwnershipResultInventory>(),
                MovementResults = new List<OwnershipResultMovement>(),
            };
            this.ownershipRuleData.OwnershipCalculations = new List<OwnershipCalculation>
            {
                new OwnershipCalculation
                {
                    OwnershipCalculationId = 1,
                },
            };

            this.ownershipRuleData.SegmentOwnershipCalculations = new List<SegmentOwnershipCalculation>
            {
                new SegmentOwnershipCalculation
                {
                    SegmentOwnershipCalculationId = 1,
                },
            };

            this.ownershipRuleData.SystemOwnershipCalculations = new List<SystemOwnershipCalculation>
            {
                new SystemOwnershipCalculation
                {
                    SystemOwnershipCalculationId = 1,
                },
            };

            await this.ownershipProcessor.CompleteAsync(this.ownershipRuleData).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<OwnershipCalculation>(), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<SegmentOwnershipCalculation>(), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<SystemOwnershipCalculation>(), Times.Once);
        }

        /// <summary>
        /// Sets up mock data.
        /// </summary>
        /// <param name="ownershipTicketId">The ownership ticket identifier.</param>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <param name="dataContext">The data context.</param>
        private async Task SetUpMockDataAsync(int ownershipTicketId, IRepositoryFactory repositoryFactory, ISqlDataContext dataContext)
        {
            var movementOne = this.GetMovement(287786, ownershipTicketId, 100, "1000", 30);
            var movementSourceOne = this.GetMovementSource(100, 100, 100, "P1");
            var movementDestinationOne = this.GetMovementDestination(100, 100, 100, "P1");
            var movementOwnershipOne = this.GetOwnership(100, 100, MessageType.MovementOwnership, ownershipTicketId, 100, null, 0);

            movementOne.MovementSource = movementSourceOne;
            movementOne.MovementDestination = movementDestinationOne;
            movementOne.Ownerships.Add(movementOwnershipOne);

            var movementTwo = this.GetMovement(200, ownershipTicketId, 200, "2000", 31);
            var movementSourceTwo = this.GetMovementSource(200, 200, 200, "P1");
            var movementDestinationTwo = this.GetMovementDestination(200, 200, 200, "P2");
            var movementOwnershipTwo = this.GetOwnership(200, 200, MessageType.MovementOwnership, ownershipTicketId, 200, null, 0);

            movementTwo.MovementSource = movementSourceTwo;
            movementTwo.MovementDestination = movementDestinationTwo;
            movementTwo.Ownerships.Add(movementOwnershipTwo);

            var inventoryProductOne = this.GetInventoryProduct(100, "P1", ownershipTicketId, "INV1", 100, 100);
            inventoryProductOne.InventoryDate = DateTime.UtcNow;

            var inventoryProductTwo = this.GetInventoryProduct(200, "P2", ownershipTicketId, "INV2", 200, 200);
            inventoryProductOne.InventoryDate = DateTime.UtcNow;

            var movementRepository = repositoryFactory.CreateRepository<Movement>();
            movementRepository.InsertAll(new List<Movement> { movementOne, movementTwo });

            var inventoryProductRepository = repositoryFactory.CreateRepository<InventoryProduct>();
            inventoryProductRepository.InsertAll(new List<InventoryProduct> { inventoryProductOne, inventoryProductTwo });

            var ticketEntity = new Ticket
            {
                TicketId = ownershipTicketId,
                CategoryElementId = 2,
                StartDate = new DateTime(2019, 02, 12),
                EndDate = new DateTime(2019, 02, 16),
                Status = 0,
                CreatedBy = "System",
                CreatedDate = new DateTime(2019, 02, 18),
                LastModifiedBy = null,
                LastModifiedDate = null,
            };

            var ticketRepository = repositoryFactory.CreateRepository<Ticket>();
            ticketRepository.Insert(ticketEntity);

            await dataContext.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Setups the data.
        /// </summary>
        /// <param name="ownershipTicketId">The ownership ticket identifier.</param>
        private void SetupData(int ownershipTicketId)
        {
            var movementOne = this.GetMovement(100, ownershipTicketId, 0, "1000",30);
            var movementSourceOne = this.GetMovementSource(287786, 100, 100, "P1");
            var movementDestinationOne = this.GetMovementDestination(100, 100, 100, "P1");
            var movementOwnershipOne = this.GetOwnership(100, 100, MessageType.MovementOwnership, ownershipTicketId, 100, null, 0);
            var movementOwnershipOnesecond = this.GetOwnership(100, 101, MessageType.MovementOwnership, ownershipTicketId, 100, null, 0);

            movementOne.MovementSource = movementSourceOne;
            movementOne.MovementDestination = movementDestinationOne;
            movementOne.Ownerships.Add(movementOwnershipOne);
            movementOne.Ownerships.Add(movementOwnershipOnesecond);

            var movementTwo = this.GetMovement(200, ownershipTicketId, 0, "2000", 31);
            var movementSourceTwo = this.GetMovementSource(200, 200, 100, "P1");
            var movementDestinationTwo = this.GetMovementDestination(200, 200, 200, "P2");
            var movementOwnershipTwo = this.GetOwnership(200, 200, MessageType.MovementOwnership, ownershipTicketId, 200, null, 0);

            movementTwo.MovementSource = movementSourceTwo;
            movementTwo.MovementDestination = movementDestinationTwo;
            movementTwo.Ownerships.Add(movementOwnershipTwo);

            var inventoryProductOne = this.GetInventoryProduct(100, "P1", ownershipTicketId, "INV1", 100, 100);
            inventoryProductOne.InventoryDate = DateTime.UtcNow;

            var inventoryProductTwo = this.GetInventoryProduct(200, "P2", ownershipTicketId, "INV2", 200, 200);
            inventoryProductOne.InventoryDate = DateTime.UtcNow;

            this.mockMovementRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Movement> { movementOne, movementTwo });
            this.mockInventoryProductRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<InventoryProduct> { inventoryProductOne, inventoryProductTwo });
        }

        /// <summary>
        /// Gets the movement.
        /// </summary>
        /// <param name="movementTransactionId">The movement transaction identifier.</param>
        /// <param name="ownershipTicketId">The ownership ticket identifier.</param>
        /// <param name="days">The days.</param>
        /// <param name="movementId">The movement identifier.</param>
        /// <param name="measurementunit">The measurement unit identifier.</param>
        /// <returns>The movement.</returns>
        private Movement GetMovement(int movementTransactionId, int ownershipTicketId, int days, string movementId,int measurementunit)
        {
            var movement = new Movement
            {
                MovementTransactionId = movementTransactionId,
                MessageTypeId = (int)MessageType.Movement,
                SystemTypeId = 1,
                EventType = "INSERT",
                MovementId = movementId,
                MovementTypeId = 1,
                TicketId = 123,
                OwnershipTicketId = ownershipTicketId,
                SegmentId = 12,
                OperationalDate = DateTime.UtcNow.AddDays(days),
                GrossStandardVolume = 1200,
                NetStandardVolume = 100,
                UncertaintyPercentage = 25,
                MeasurementUnit = measurementunit,
                ScenarioId = ScenarioType.OPERATIONAL,
                Observations = "Observations",
                Classification = "Classification",
                IsDeleted = false,
            };

            return movement;
        }

        /// <summary>
        /// Gets the ownership.
        /// </summary>
        /// <param name="ownershipId">The ownership identifier.</param>
        /// <param name="ownerId">The owner identifier.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="ownershipTicketId">The ownership ticket identifier.</param>
        /// <param name="movementTransactionId">The movement transaction identifier.</param>
        /// <param name="inventoryProductId">The inventory product identifier.</param>
        /// <param name="days">The days.</param>
        /// <returns>
        /// The Ownership.
        /// </returns>
        private Ownership GetOwnership(int ownershipId, int ownerId, MessageType messageType, int ownershipTicketId, int? movementTransactionId, int? inventoryProductId, int days)
        {
            var ownership = new Ownership
            {
                OwnershipId = ownershipId,
                MessageTypeId = messageType,
                TicketId = ownershipTicketId,
                MovementTransactionId = movementTransactionId,
                InventoryProductId = inventoryProductId,
                OwnerId = ownerId,
                OwnershipPercentage = 25,
                OwnershipVolume = 123,
                AppliedRule = "Rule One",
                RuleVersion = "Version 1",
                ExecutionDate = DateTime.UtcNow.AddDays(days + 1),
            };

            return ownership;
        }

        /// <summary>
        /// Gets the movement destination.
        /// </summary>
        /// <param name="movementDestinationId">The movement destination identifier.</param>
        /// <param name="movementTransactionId">The movement transaction identifier.</param>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <returns>The MovementDestination.</returns>
        private MovementDestination GetMovementDestination(int movementDestinationId, int movementTransactionId, int nodeId, string productId)
        {
            var movementDestination = new MovementDestination();
            movementDestination.MovementDestinationId = movementDestinationId;
            movementDestination.MovementTransactionId = movementTransactionId;
            movementDestination.DestinationNodeId = nodeId;
            movementDestination.DestinationStorageLocationId = 123;
            movementDestination.DestinationProductId = productId;
            movementDestination.DestinationProductTypeId = 1;

            return movementDestination;
        }

        /// <summary>
        /// Gets the movement source.
        /// </summary>
        /// <param name="movementSourceId">The movement source identifier.</param>
        /// <param name="movementTransactionId">The movement transaction identifier.</param>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <returns>The MovementSource.</returns>
        private MovementSource GetMovementSource(int movementSourceId, int movementTransactionId, int nodeId, string productId)
        {
            var movementSource = new MovementSource
            {
                MovementSourceId = movementSourceId,
                MovementTransactionId = movementTransactionId,
                SourceNodeId = nodeId,
                SourceStorageLocationId = 123,
                SourceProductId = productId,
                SourceProductTypeId = 1,
            };

            return movementSource;
        }

        /// <summary>
        /// Gets the inventory product.
        /// </summary>
        /// <param name="inventoryProductId">The inventory product identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="ownershipTicketId">The ownership ticket identifier.</param>
        /// <returns>The InventoryProduct.</returns>
        private InventoryProduct GetInventoryProduct(int inventoryProductId, string productId, int ownershipTicketId, string inventoryId, int days, int nodeId)
        {
            var inventoryProduct = new InventoryProduct
            {
                InventoryProductId = inventoryProductId,
                ProductId = productId,
                ProductType = 1,
                ProductVolume = 122,
                MeasurementUnit = 30,
                UncertaintyPercentage = 23,
                OwnershipTicketId = ownershipTicketId,
                SystemTypeId = 1,
                DestinationSystem = "SINOPER",
                EventType = "INSERT",
                TankName = "TANQUE",
                InventoryId = inventoryId,
                TicketId = 1,
                InventoryDate = DateTime.UtcNow.AddDays(days + 1),
                NodeId = nodeId,
                Observations = "Barrel",
                ScenarioId = ScenarioType.OPERATIONAL,
                IsDeleted = false,
                FileRegistrationTransactionId = 122,
                SegmentId = 12,
            };

            return inventoryProduct;
        }
    }
}