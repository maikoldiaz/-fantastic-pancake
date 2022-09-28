// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaProcessorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Approval.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Approval;
    using Ecp.True.Processors.Approval.Interfaces;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Core.Registration;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The delta processor tests.
    /// </summary>
    [TestClass]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Test Class")]
    public class DeltaProcessorTests
    {
        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The repository factory.
        /// </summary>
        private Mock<IRepositoryFactory> repositoryFactory;

        /// <summary>
        /// The repository factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;

        /// <summary>
        /// The unit of work mock.
        /// </summary>
        private Mock<IUnitOfWork> unitOfWorkMock;

        /// <summary>
        /// The configuration handler.
        /// </summary>
        private Mock<IConfigurationHandler> configurationHandler;

        /// <summary>
        /// The delta node repository mock.
        /// </summary>
        private Mock<IRepository<DeltaNode>> deltaNodeRepositoryMock;

        /// <summary>
        /// The delta node approval repository mock.
        /// </summary>
        private Mock<IRepository<DeltaNodeApproval>> deltaNodeApprovalRepositoryMock;

        /// <summary>
        /// The user repository mock.
        /// </summary>
        private Mock<IRepository<User>> userRepositoryMock;

        /// <summary>
        /// The balance calculator.
        /// </summary>
        private DeltaProcessor deltaProcessor;

        /// <summary>
        /// The mock processor.
        /// </summary>
        private Mock<IDeltaProcessor> mockProcessor;

        /// <summary>
        /// The Transformation Official Delta Node.
        /// </summary>
        private Mock<ITransformationOfficialDeltaNode> mockTransformationOfficialDeltaNode;

        /// <summary>
        /// The registration strategy factory.
        /// </summary>
        private RegistrationStrategyFactory registrationStrategyFactory;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<RegistrationStrategyFactory>> mockLogger;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<DeltaProcessor>> mockLoggerDeltaProcessor;

        /// <summary>
        /// The mock movement registration service.
        /// </summary>
        private Mock<IMovementRegistrationService> mockMovementRegistrationService;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.repositoryFactory = new Mock<IRepositoryFactory>();
            this.unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.configurationHandler = new Mock<IConfigurationHandler>();
            this.mockProcessor = new Mock<IDeltaProcessor>();
            this.mockTransformationOfficialDeltaNode = new Mock<ITransformationOfficialDeltaNode>();
            var serviceBusQueueClient = new Mock<IServiceBusQueueClient>();
            this.mockAzureClientFactory.Setup(x => x.GetQueueClient(It.IsAny<string>())).Returns(serviceBusQueueClient.Object);
            this.mockLogger = new Mock<ITrueLogger<RegistrationStrategyFactory>>();
            this.mockLoggerDeltaProcessor = new Mock<ITrueLogger<DeltaProcessor>>();
            this.mockMovementRegistrationService = new Mock<IMovementRegistrationService>();
            this.registrationStrategyFactory = new RegistrationStrategyFactory(
                    this.mockLogger.Object,
                    this.mockAzureClientFactory.Object,
                    this.mockMovementRegistrationService.Object);

            this.deltaNodeRepositoryMock = new Mock<IRepository<DeltaNode>>();
            this.deltaNodeApprovalRepositoryMock = new Mock<IRepository<DeltaNodeApproval>>();
            this.userRepositoryMock = new Mock<IRepository<User>>();
            this.repositoryFactory.Setup(x => x.CreateRepository<DeltaNode>()).Returns(this.deltaNodeRepositoryMock.Object);
            this.repositoryFactory.Setup(x => x.CreateRepository<DeltaNodeApproval>()).Returns(this.deltaNodeApprovalRepositoryMock.Object);
            this.unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.unitOfWorkMock = new Mock<IUnitOfWork>();

            this.repositoryFactory.Setup(x => x.CreateRepository<User>()).Returns(this.userRepositoryMock.Object);

            this.unitOfWorkFactory.Setup(x => x.GetUnitOfWork()).Returns(this.unitOfWorkMock.Object);
            this.unitOfWorkMock.Setup(x => x.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            this.deltaProcessor = new DeltaProcessor(
                this.repositoryFactory.Object,
                this.unitOfWorkFactory.Object,
                this.configurationHandler.Object,
                this.mockAzureClientFactory.Object,
                this.mockTransformationOfficialDeltaNode.Object,
                this.mockLoggerDeltaProcessor.Object);
        }

        /// <summary>
        /// Gets the delta node balance details asynchronous should invoke repos when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetDeltaByDeltaNodeIdAsync_ShouldInvokeRepos_WhenInvokedAsync()
        {
            // Arrange
            var settings = new SystemSettings { BasePath = "https://dev-true.ecopetrol.com.co" };
            this.configurationHandler.Setup(a => a.GetConfigurationAsync<SystemSettings>(It.IsAny<string>())).ReturnsAsync(settings);
            this.userRepositoryMock.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(new User { Email = "TestEmail" });
            this.deltaNodeRepositoryMock.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<DeltaNode, bool>>>(), "Ticket", "Node")).ReturnsAsync(new DeltaNode { Editor = "TestUser", Node = new Node { Name = "TestNode" }, Ticket = new Ticket { StartDate = DateTime.Parse("1/1/2020", CultureInfo.InvariantCulture) } });
            this.deltaNodeApprovalRepositoryMock.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<DeltaNodeApproval, bool>>>())).ReturnsAsync(new DeltaNodeApproval { Approvers = "TestUser", Node = new Node { Name = "TestNode" } });

            // Act
            var details = await this.deltaProcessor.GetDeltaByDeltaNodeIdAsync(1).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(details);
            Assert.AreEqual($"{settings.BasePath}/officialdeltanode/manage/1", details.ReportPath);
            this.deltaNodeRepositoryMock.Verify(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<DeltaNode, bool>>>(), "Ticket", "Node"), Times.Once);
            this.deltaNodeApprovalRepositoryMock.Verify(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<DeltaNodeApproval, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Gets the delta node  details asynchronous should throw key not found exception when node not found asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task GetDeltaByDeltaNodeIdAsync_ShouldThrowKeyNotFoundException_WhenNodeNotFoundAsync()
        {
            // Arrange
            // Arrange
            DeltaNode deltanode = null;
            this.deltaNodeRepositoryMock.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<DeltaNode, bool>>>(), "Ticket", "Node")).ReturnsAsync(deltanode);

            // Act
            await this.deltaProcessor.GetDeltaByDeltaNodeIdAsync(01).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the delta node status asynchronous should return sucess messege.
        /// </summary>
        /// <returns> The Task.</returns>
        [TestMethod]
        public async Task Save_ShouldBeInvoked_WhenApprovalStatusIsApprovedAsync()
        {
            var deltaRequest = new DeltaNodeApprovalRequest
            {
                DeltaNodeId = 2,
                ApproverAlias = "approveralias",
                Comment = string.Empty,
                Status = OwnershipNodeStatusType.APPROVED.ToString(),
            };
            var deltaNode = new DeltaNode
            {
                DeltaNodeId = 2,
                Approvers = string.Empty,
                Comment = string.Empty,
                Status = Entities.Enumeration.OwnershipNodeStatusType.SUBMITFORAPPROVAL,
            };

            var repoMock = new Mock<IRepository<DeltaNode>>();
            repoMock.Setup(r => r.Update(It.IsAny<DeltaNode>()));
            repoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(deltaNode);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<DeltaNode>()).Returns(repoMock.Object);
            this.unitOfWorkMock.Setup(x => x.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));
            this.mockProcessor.Setup(x => x.UpdateDeltaApprovalStateAsync(deltaRequest));

            await this.deltaProcessor.UpdateDeltaApprovalStateAsync(deltaRequest).ConfigureAwait(false);
            this.unitOfWorkMock.Verify(x => x.CreateRepository<DeltaNode>(), Times.Exactly(1));
            this.unitOfWorkMock.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        ///  Generate Delta Movements.
        /// </summary>
        /// <returns> The Task.</returns>
        [TestMethod]
        public async Task GenerateDeltaMovementsAsync_ShouldBeGenerateDeltasAsync()
        {
            DateTime operatinalData = DateTime.Now;
            var repoMock = new Mock<IRepository<DeltaNode>>();
            DeltaNode deltaNode = new DeltaNode
            {
                DeltaNodeId = 1,
                NodeId = 1,
                TicketId = 1,
                Ticket = new Ticket() { CategoryElementId = 1, TicketId = 1 },
            };

            List<Movement> existingMovement = new List<Movement>();
            existingMovement.Add(new Movement()
            {
                MovementTransactionId = 1,
                MovementTypeId = (int)MovementType.DeltaInventory,
                IsDeleted = false,
                MovementId = "DO-1234",
                SourceSystemId = (int)SourceSystem.TRUE,
                SegmentId = 1,
                OperationalDate = operatinalData,
                BackupMovementId = "TEST",
                MovementSource = new MovementSource() { SourceNodeId = 1 },
                MovementDestination = new MovementDestination() { DestinationNodeId = 1 },
            });

            existingMovement.Add(new Movement()
            {
                MovementTransactionId = 2,
                MovementTypeId = (int)MovementType.DeltaInventory,
                IsDeleted = false,
                MovementId = "DO-1234",
                SourceSystemId = (int)SourceSystem.FICO,
                SegmentId = 1,
                OperationalDate = operatinalData,
                BackupMovementId = null,
                OfficialDeltaTicketId = 1,
                MovementSource = new MovementSource() { SourceNodeId = 1 },
                MovementDestination = new MovementDestination() { DestinationNodeId = 1 },
            });

            this.repositoryFactory.Setup(x => x.CreateRepository<DeltaNode>()).Returns(repoMock.Object);
            repoMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<DeltaNode, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(deltaNode);

            var repoMockOfficialDeltaNodeMovement = new Mock<IRepository<OfficialDeltaNodeMovement>>();
            this.repositoryFactory.Setup(x => x.CreateRepository<OfficialDeltaNodeMovement>()).Returns(repoMockOfficialDeltaNodeMovement.Object);

            IEnumerable<OfficialDeltaNodeMovement> officialDeltaMovement = new List<OfficialDeltaNodeMovement>
            {
                new OfficialDeltaNodeMovement()
                {
                MovementTransactionId = 2,
                MovementTypeId = (int)MovementType.DeltaInventory,
                MovementId = "DO-1234",
                SourceSystemId = (int)SourceSystem.FICO,
                SegmentId = 1,
                OperationalDate = operatinalData,
                OfficialDeltaTicketId = 1,
                },
                new OfficialDeltaNodeMovement() { MovementTypeId = 153 },
                new OfficialDeltaNodeMovement() { MovementTypeId = 154 } ,
            };
            repoMockOfficialDeltaNodeMovement.Setup(x => x.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(officialDeltaMovement);

            var repoMockTicket = new Mock<IRepository<DateDeltaMovement>>();
            this.repositoryFactory.Setup(x => x.CreateRepository<DateDeltaMovement>()).Returns(repoMockTicket.Object);

            IEnumerable<DateDeltaMovement> listTicketsDate = new List<DateDeltaMovement> { new DateDeltaMovement() { OperationDate = operatinalData } };
            repoMockTicket.Setup(x => x.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(listTicketsDate);

            var repoMockMovement = new Mock<IRepository<Movement>>();
            this.unitOfWorkMock.Setup(x => x.CreateRepository<Movement>()).Returns(repoMockMovement.Object);
            repoMockMovement.Setup(x => x.InsertAll(It.IsAny<IEnumerable<Movement>>()));
            repoMockMovement.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(existingMovement);

            await this.deltaProcessor.GenerateDeltaMovementsAsync(1).ConfigureAwait(false);

            // Assert
            this.unitOfWorkMock.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// GenerateDeltaMovementsAsync Should save Log Error When InvokedAsync.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task GenerateDeltaMovementsAsync_ShouldLogError_WhenExceptionIsRaisedAsync()
        {
            // Arrange
            this.mockLoggerDeltaProcessor.Setup(m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>()));
            var repoMock = new Mock<IRepository<DeltaNode>>();
            DeltaNode deltaNode = new DeltaNode
            {
                DeltaNodeId = 1,
                NodeId = 1,
                TicketId = 1,
                Ticket = new Ticket() { CategoryElementId = 1, TicketId = 1 },
            };

            this.repositoryFactory.Setup(x => x.CreateRepository<DeltaNode>()).Returns(repoMock.Object);
            repoMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<DeltaNode, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(deltaNode);

            // Act
            await this.deltaProcessor.GenerateDeltaMovementsAsync(1).ConfigureAwait(false);

            // Assert
            this.mockLoggerDeltaProcessor.Verify(
                m => m.LogError(
                It.IsAny<Exception>(),
                It.IsAny<string>(),
                It.IsAny<object[]>()), Times.Once);
        }
    }
}
