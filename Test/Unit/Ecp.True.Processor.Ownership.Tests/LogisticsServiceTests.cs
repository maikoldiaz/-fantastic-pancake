// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogisticsServiceTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Ownership.Calculation.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.HandleFailure;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Ownership.Services;
    using Ecp.True.Processors.Ownership.Services.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The OwnershipServiceTests.
    /// </summary>
    [TestClass]
    public class LogisticsServiceTests
    {
        /// <summary>
        /// The mock failure handler.
        /// </summary>
        private readonly Mock<IFailureHandler> mockFailureHandler = new Mock<IFailureHandler>();

        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The repository factory.
        /// </summary>
        private Mock<IRepositoryFactory> repositoryFactory;

        /// <summary>
        /// The ownership service.
        /// </summary>
        private LogisticsService logisticsService;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<LogisticsService>> mockLogger;

        /// <summary>
        /// The mock ticket repository.
        /// </summary>
        private Mock<IRepository<NodesForSegmentResult>> mockNodesForSegmentResult;

        /// <summary>
        /// The mock LogisticMovement repository.
        /// </summary>
        private Mock<IRepository<LogisticMovement>> mockLogisticMovement;

        /// <summary>
        /// The mock failure handler factory.
        /// </summary>
        private Mock<IFailureHandlerFactory> mockFailureHandlerFactory;

        /// <summary>
        /// The mock unit of Excel.
        /// </summary>
        private Mock<IExcelService> mockExcelService;

        /// <summary>
        /// The mock LogisticsServic.
        /// </summary>
        private Mock<ILogisticsService> mockLogisticsService;

        /// <summary>
        /// The token.
        /// </summary>
        private CancellationToken token;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.unitOfWorkFactory.Setup(a => a.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockUnitOfWork.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));
            this.token = new CancellationToken(false);
            this.repositoryFactory = new Mock<IRepositoryFactory>();
            this.mockLogger = new Mock<ITrueLogger<LogisticsService>>();
            this.mockFailureHandlerFactory = new Mock<IFailureHandlerFactory>();
            this.mockExcelService = new Mock<IExcelService>();
            this.mockLogisticsService = new Mock<ILogisticsService>();
            this.mockFailureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockFailureHandler.Object);
            this.mockNodesForSegmentResult = new Mock<IRepository<NodesForSegmentResult>>();
            this.mockLogisticMovement = new Mock<IRepository<LogisticMovement>>();
            this.logisticsService = new LogisticsService(
                this.unitOfWorkFactory.Object,
                this.repositoryFactory.Object,
                this.mockLogger.Object,
                this.mockFailureHandlerFactory.Object,
                this.mockExcelService.Object);
        }

        /// <summary>
        /// TransformAsync ShouldTransformSuccessfullyWithTautologyAsync.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task TransformAsync_ShouldTransformSuccessfullyWithTautologyAsync()
        {
            var homologations = new List<Homologation>();
            var homologation = new Homologation();
            var group = new HomologationGroup();
            var dataMapping = new HomologationDataMapping { SourceValue = "2", DestinationValue = "Anul. Tr. Material a material" };
            group.HomologationDataMapping.Add(dataMapping);
            homologation.HomologationGroups.Add(group);
            homologations.Add(homologation);
            var annulations = new List<Annulation> { new Annulation() { SourceMovementTypeId = 1, AnnulationMovementTypeId = 1, IsActive = true } };
            var elements = new List<CategoryElement> { new CategoryElement() { ElementId = 2, Name = "Anul. Tr. Material a material" }, new CategoryElement() { ElementId = 31, Name = "Bbl" } };
            var movements = new List<GenericLogisticsMovement> { new GenericLogisticsMovement() { HasAnnulation = true, MovementTypeId = 1, SourceNodeId = 1, DestinationNodeId = 2, SourceProductId = "1", DestinationProductId = "2", MeasurementUnit = 31 } };
            var ticket = new Ticket
            {
                TicketTypeId = TicketType.OfficialLogistics,
                CategoryElementId = 2,
                StartDate = DateTime.Now.AddHours(-5),
                EndDate = DateTime.Now.AddHours(5),
                CategoryElement = new CategoryElement() { Name = "element" },
                ScenarioTypeId = ScenarioType.OFFICER,
                TicketId = 1,
                NodeId = 1,
            };

            this.repositoryFactory.Setup(m => m.HomologationRepository.GetAllAsync(
                It.IsAny<Expression<Func<Homologation, bool>>>(),
                "HomologationGroups",
                "HomologationGroups.Group",
                "HomologationGroups.HomologationObjects",
                "HomologationGroups.HomologationDataMapping")).ReturnsAsync(homologations);
            var repoMockAnnulation = new Mock<IRepository<Annulation>>();
            repoMockAnnulation.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Annulation, bool>>>())).ReturnsAsync(annulations);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Annulation>()).Returns(repoMockAnnulation.Object);
            var repoMockElement = new Mock<IRepository<CategoryElement>>();
            repoMockElement.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(elements);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<CategoryElement>()).Returns(repoMockElement.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<NodesForSegmentResult>()).Returns(this.mockNodesForSegmentResult.Object);
            this.mockFailureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.mockFailureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockFailureHandler.Object);

            var transformedMovement = await this.logisticsService.TransformAsync(movements, ticket, Entities.Dto.SystemType.SIV, ScenarioType.OPERATIONAL).ConfigureAwait(false);

            // Assert
            Assert.AreNotEqual(0, transformedMovement.Count());
            this.repositoryFactory.Verify(
                m => m.HomologationRepository.GetAllAsync(
                It.IsAny<Expression<Func<Homologation, bool>>>(),
                "HomologationGroups",
                "HomologationGroups.Group",
                "HomologationGroups.HomologationObjects",
                "HomologationGroups.HomologationDataMapping"), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<NodesForSegmentResult>(), Times.AtLeastOnce);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Annulation>(), Times.AtLeastOnce);
            repoMockAnnulation.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Annulation, bool>>>()), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<CategoryElement>(), Times.Once);
            repoMockElement.Verify(c => c.GetAllAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>()), Times.Once);
            this.mockFailureHandler.Verify(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Never);
        }

        /// <summary>
        /// TransformAsync ShouldTransformSuccessfullyWithTautologyAsync.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task TransformAsync_ShouldTransformSuccessfullyWithoutTautologyAsync()
        {
            var homologations = new List<Homologation>();
            var homologation = new Homologation();
            var group = new HomologationGroup();
            var dataMapping = new HomologationDataMapping { SourceValue = "1", DestinationValue = "movementtype" };
            group.HomologationDataMapping.Add(dataMapping);
            homologation.HomologationGroups.Add(group);
            homologations.Add(homologation);
            var annulations = new List<Annulation> { new Annulation() { SourceMovementTypeId = 1, AnnulationMovementTypeId = 1, IsActive = true } };
            var elements = new List<CategoryElement> { new CategoryElement() { ElementId = 1, Name = "movementtype" }, new CategoryElement() { ElementId = 31, Name = "Bbl" } };
            var movements = new List<GenericLogisticsMovement> { new GenericLogisticsMovement() { HasAnnulation = true, MovementTypeId = 1, SourceNodeId = 1, DestinationNodeId = 2, DestinationProductId = "2", MeasurementUnit = 31 } };
            var ticket = new Ticket
            {
                TicketTypeId = TicketType.OfficialLogistics,
                CategoryElementId = 2,
                StartDate = DateTime.Now.AddHours(-5),
                EndDate = DateTime.Now.AddHours(5),
                CategoryElement = new CategoryElement() { Name = "element" },
                ScenarioTypeId = ScenarioType.OFFICER,
                TicketId = 1,
                NodeId = 1,
            };

            this.repositoryFactory.Setup(m => m.HomologationRepository.GetAllAsync(
                It.IsAny<Expression<Func<Homologation, bool>>>(),
                "HomologationGroups",
                "HomologationGroups.Group",
                "HomologationGroups.HomologationObjects",
                "HomologationGroups.HomologationDataMapping")).ReturnsAsync(homologations);
            var repoMockAnnulation = new Mock<IRepository<Annulation>>();
            repoMockAnnulation.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Annulation, bool>>>())).ReturnsAsync(annulations);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Annulation>()).Returns(repoMockAnnulation.Object);
            var repoMockElement = new Mock<IRepository<CategoryElement>>();
            repoMockElement.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(elements);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<CategoryElement>()).Returns(repoMockElement.Object);
            this.mockFailureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.mockFailureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockFailureHandler.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<NodesForSegmentResult>()).Returns(this.mockNodesForSegmentResult.Object);

            var transformedMovement = await this.logisticsService.TransformAsync(movements, ticket,Entities.Dto.SystemType.SIV, ScenarioType.OFFICER).ConfigureAwait(false);

            // Assert
            Assert.AreNotEqual(0, transformedMovement.Count());
            this.repositoryFactory.Verify(
                m => m.HomologationRepository.GetAllAsync(
                It.IsAny<Expression<Func<Homologation, bool>>>(),
                "HomologationGroups",
                "HomologationGroups.Group",
                "HomologationGroups.HomologationObjects",
                "HomologationGroups.HomologationDataMapping"), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<NodesForSegmentResult>(), Times.AtLeastOnce);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Annulation>(), Times.AtLeastOnce);
            repoMockAnnulation.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Annulation, bool>>>()), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<CategoryElement>(), Times.Once);
            repoMockElement.Verify(c => c.GetAllAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>()), Times.Once);
            this.mockFailureHandler.Verify(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Never);
        }

        /// <summary>
        /// TransformAsync ShouldTransformSuccessfullyWithTautolog Error.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task TransformAsync_ShouldStopDueToTautologyErrorAsync()
        {
            var homologations = new List<Homologation>();
            var homologation = new Homologation();
            var group = new HomologationGroup();
            var dataMapping = new HomologationDataMapping { SourceValue = "Anul. Tr. Material a material", DestinationValue = "Anul. Tr. Material a material" };
            group.HomologationDataMapping.Add(dataMapping);
            homologation.HomologationGroups.Add(group);
            homologations.Add(homologation);
            var annulations = new List<Annulation> { new Annulation() { SourceMovementTypeId = 1, AnnulationMovementTypeId = 1, IsActive = true } };
            var elements = new List<CategoryElement> { new CategoryElement() { ElementId = 1, Name = "movementtype" }, new CategoryElement() { ElementId = 31, Name = "Bbl" } };
            var movements = new List<GenericLogisticsMovement> { new GenericLogisticsMovement() { HasAnnulation = true, MovementTypeId = 1, SourceNodeId = 1, DestinationNodeId = 2, SourceProductId = "1", DestinationProductId = "1", MeasurementUnit = 31 } };
            var ticket = new Ticket
            {
                TicketTypeId = TicketType.OfficialLogistics,
                CategoryElementId = 2,
                StartDate = DateTime.Now.AddHours(-5),
                EndDate = DateTime.Now.AddHours(5),
                CategoryElement = new CategoryElement() { Name = "element" },
                ScenarioTypeId = ScenarioType.OFFICER,
                TicketId = 1,
                NodeId = 1,
            };

            this.repositoryFactory.Setup(m => m.HomologationRepository.GetAllAsync(
                It.IsAny<Expression<Func<Homologation, bool>>>(),
                "HomologationGroups",
                "HomologationGroups.Group",
                "HomologationGroups.HomologationObjects",
                "HomologationGroups.HomologationDataMapping")).ReturnsAsync(homologations);
            var repoMockAnnulation = new Mock<IRepository<Annulation>>();
            repoMockAnnulation.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Annulation, bool>>>())).ReturnsAsync(annulations);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Annulation>()).Returns(repoMockAnnulation.Object);
            var repoMockElement = new Mock<IRepository<CategoryElement>>();
            repoMockElement.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(elements);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<CategoryElement>()).Returns(repoMockElement.Object);
            this.mockFailureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockFailureHandler.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<NodesForSegmentResult>()).Returns(this.mockNodesForSegmentResult.Object);

            var transformedMovement = await this.logisticsService.TransformAsync(movements, ticket, Entities.Dto.SystemType.SIV, ScenarioType.OFFICER).ConfigureAwait(false);

            // Assert
            Assert.AreNotEqual(0, transformedMovement.Count());
            this.repositoryFactory.Verify(
                m => m.HomologationRepository.GetAllAsync(
                It.IsAny<Expression<Func<Homologation, bool>>>(),
                "HomologationGroups",
                "HomologationGroups.Group",
                "HomologationGroups.HomologationObjects",
                "HomologationGroups.HomologationDataMapping"), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<NodesForSegmentResult>(), Times.AtLeastOnce);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Annulation>(), Times.AtLeastOnce);
            repoMockAnnulation.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Annulation, bool>>>()), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<CategoryElement>(), Times.Once);
            repoMockElement.Verify(c => c.GetAllAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>()), Times.Once);
        }

        /// <summary>
        /// TransformAsync Homologation Error.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task TransformAsync_ShouldStopDueToHomologationErrorAsync()
        {
            var homologations = new List<Homologation>();
            var homologation = new Homologation();
            var group = new HomologationGroup();
            var dataMapping = new HomologationDataMapping { SourceValue = "2", DestinationValue = "Tr. Material a material" };
            group.HomologationDataMapping.Add(dataMapping);
            homologation.HomologationGroups.Add(group);
            homologations.Add(homologation);
            var annulations = new List<Annulation> { new Annulation() { SourceMovementTypeId = 1, AnnulationMovementTypeId = 1, IsActive = true } };
            var elements = new List<CategoryElement> { new CategoryElement() { ElementId = 1, Name = "Anul. Tr. Material a material" }, new CategoryElement() { ElementId = 31, Name = "Bbl" } };
            var movements = new List<GenericLogisticsMovement> { new GenericLogisticsMovement() { HasAnnulation = false, MovementTypeId = 1, SourceNodeId = 1, DestinationNodeId = 2, SourceProductId = "2", DestinationProductId = "1", MeasurementUnit = 31 } };
            var ticket = new Ticket
            {
                TicketTypeId = TicketType.OfficialLogistics,
                CategoryElementId = 2,
                StartDate = DateTime.Now.AddHours(-5),
                EndDate = DateTime.Now.AddHours(5),
                CategoryElement = new CategoryElement() { Name = "element" },
                ScenarioTypeId = ScenarioType.OFFICER,
                TicketId = 1,
                NodeId = 1,
            };

            this.repositoryFactory.Setup(m => m.HomologationRepository.GetAllAsync(
                It.IsAny<Expression<Func<Homologation, bool>>>(),
                "HomologationGroups",
                "HomologationGroups.Group",
                "HomologationGroups.HomologationObjects",
                "HomologationGroups.HomologationDataMapping")).ReturnsAsync(homologations);
            var repoMockAnnulation = new Mock<IRepository<Annulation>>();
            repoMockAnnulation.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Annulation, bool>>>())).ReturnsAsync(annulations);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Annulation>()).Returns(repoMockAnnulation.Object);
            var repoMockElement = new Mock<IRepository<CategoryElement>>();
            repoMockElement.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(elements);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<CategoryElement>()).Returns(repoMockElement.Object);
            this.mockFailureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockFailureHandler.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<NodesForSegmentResult>()).Returns(this.mockNodesForSegmentResult.Object);

            var transformedMovement = await this.logisticsService.TransformAsync(movements, ticket, Entities.Dto.SystemType.SIV, ScenarioType.OPERATIONAL).ConfigureAwait(false);

            // Assert
            Assert.AreNotEqual(0, transformedMovement.Count());
            this.repositoryFactory.Verify(
                m => m.HomologationRepository.GetAllAsync(
                It.IsAny<Expression<Func<Homologation, bool>>>(),
                "HomologationGroups",
                "HomologationGroups.Group",
                "HomologationGroups.HomologationObjects",
                "HomologationGroups.HomologationDataMapping"), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<NodesForSegmentResult>(), Times.AtLeastOnce);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Annulation>(), Times.AtLeastOnce);
            repoMockAnnulation.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Annulation, bool>>>()), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<CategoryElement>(), Times.Once);
            repoMockElement.Verify(c => c.GetAllAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>()), Times.Once);
        }

        /// <summary>
        /// TransformAsync Check Nodes Approbed.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task TransformAsync_ShouldCheckNodesApprovedAsync()
        {
            var homologations = new List<Homologation>();
            var homologation = new Homologation();
            var group = new HomologationGroup();
            var dataMapping = new HomologationDataMapping { SourceValue = "2", DestinationValue = "Tr. Material a material" };
            group.HomologationDataMapping.Add(dataMapping);
            homologation.HomologationGroups.Add(group);
            homologations.Add(homologation);
            var annulations = new List<Annulation> { new Annulation() { SourceMovementTypeId = 1, AnnulationMovementTypeId = 1, IsActive = true } };
            var elements = new List<CategoryElement> { new CategoryElement() { ElementId = 1, Name = "Anul. Tr. Material a material" }, new CategoryElement() { ElementId = 31, Name = "Bbl" } };
            var movements = new List<GenericLogisticsMovement>
            {
                new GenericLogisticsMovement()
                {
                    HasAnnulation = true,
                    MovementTypeId = 1,
                    SourceNodeId = 1,
                    DestinationNodeId = 2,
                    SourceProductId = "2",
                    DestinationProductId = "1",
                    MeasurementUnit = 31,
                    StartDate = Convert.ToDateTime("05/05/2021", CultureInfo.InvariantCulture),
                    EndDate = Convert.ToDateTime("06/05/2021", CultureInfo.InvariantCulture),
                    Classification = Constants.LossClassification,
                    SourceNodeSendToSap = false,
                    DestinationNodeSendToSap = false,
                },
            };
            var nodesForSegment = new List<NodesForSegmentResult>
            {
                new NodesForSegmentResult()
                {
                    IsApproved = true,
                    NodeId = 1,
                    OperationDate = Convert.ToDateTime("05/05/2021", CultureInfo.InvariantCulture),
                },
                new NodesForSegmentResult()
                {
                    IsApproved = true,
                    NodeId = 2,
                    OperationDate = Convert.ToDateTime("05/05/2021", CultureInfo.InvariantCulture),
                },
            };
            var ticket = new Ticket
            {
                TicketTypeId = TicketType.OfficialLogistics,
                CategoryElementId = 2,
                StartDate = DateTime.Now.AddHours(-5),
                EndDate = DateTime.Now.AddHours(5),
                CategoryElement = new CategoryElement() { Name = "element" },
                ScenarioTypeId = ScenarioType.OFFICER,
                TicketId = 1,
                NodeId = 1,
            };

            this.repositoryFactory.Setup(m => m.HomologationRepository.GetAllAsync(
                It.IsAny<Expression<Func<Homologation, bool>>>(),
                "HomologationGroups",
                "HomologationGroups.Group",
                "HomologationGroups.HomologationObjects",
                "HomologationGroups.HomologationDataMapping")).ReturnsAsync(homologations);
            var repoMockAnnulation = new Mock<IRepository<Annulation>>();
            repoMockAnnulation.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Annulation, bool>>>())).ReturnsAsync(annulations);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Annulation>()).Returns(repoMockAnnulation.Object);
            var repoMockElement = new Mock<IRepository<CategoryElement>>();
            repoMockElement.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(elements);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<CategoryElement>()).Returns(repoMockElement.Object);
            this.mockFailureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockFailureHandler.Object);

            var repoMockNodesForSegmentResult = new Mock<IRepository<NodesForSegmentResult>>();
            repoMockNodesForSegmentResult.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(nodesForSegment);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<NodesForSegmentResult>()).Returns(repoMockNodesForSegmentResult.Object);

            var transformedMovement = await this.logisticsService.TransformAsync(movements, ticket, Entities.Dto.SystemType.SIV, ScenarioType.OPERATIONAL).ConfigureAwait(false);

            // Assert
            Assert.AreNotEqual(0, transformedMovement.Count());
            this.repositoryFactory.Verify(
                m => m.HomologationRepository.GetAllAsync(
                It.IsAny<Expression<Func<Homologation, bool>>>(),
                "HomologationGroups",
                "HomologationGroups.Group",
                "HomologationGroups.HomologationObjects",
                "HomologationGroups.HomologationDataMapping"), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<NodesForSegmentResult>(), Times.AtLeastOnce);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Annulation>(), Times.AtLeastOnce);
            repoMockAnnulation.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Annulation, bool>>>()), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<CategoryElement>(), Times.Once);
            repoMockElement.Verify(c => c.GetAllAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>()), Times.Once);
        }

        /// <summary>
        /// TransformAsync check Nodes Send To Sap.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task TransformAsync_ShouldCheckNodeSendToSapAsync()
        {
            var homologations = new List<Homologation>();
            var homologation = new Homologation();
            var group = new HomologationGroup();
            var dataMapping = new HomologationDataMapping { SourceValue = "2", DestinationValue = "Tr. Material a material" };
            group.HomologationDataMapping.Add(dataMapping);
            homologation.HomologationGroups.Add(group);
            homologations.Add(homologation);
            var annulations = new List<Annulation> { new Annulation() { SourceMovementTypeId = 1, AnnulationMovementTypeId = 1, IsActive = true } };
            var elements = new List<CategoryElement> { new CategoryElement() { ElementId = 1, Name = "Anul. Tr. Material a material" }, new CategoryElement() { ElementId = 31, Name = "Bbl" } };
            var movements = new List<GenericLogisticsMovement>
            {
                new GenericLogisticsMovement()
                {
                    HasAnnulation = true,
                    MovementTypeId = 1,
                    SourceNodeId = 1,
                    DestinationNodeId = 2,
                    SourceProductId = "2",
                    DestinationProductId = "1",
                    MeasurementUnit = 31,
                    StartDate = Convert.ToDateTime("05/05/2021", CultureInfo.InvariantCulture),
                    EndDate = Convert.ToDateTime("06/05/2021", CultureInfo.InvariantCulture),
                    Classification = Constants.LossClassification,
                    DestinationNodeSendToSap = true,
                    SourceNodeSendToSap = true,
                },
            };
            var nodesForSegment = new List<NodesForSegmentResult>
            {
                new NodesForSegmentResult()
                {
                    IsApproved = true,
                    NodeId = 1,
                    OperationDate = Convert.ToDateTime("05/05/2021", CultureInfo.InvariantCulture),
                },
                new NodesForSegmentResult()
                {
                    IsApproved = true,
                    NodeId = 2,
                    OperationDate = Convert.ToDateTime("05/05/2021", CultureInfo.InvariantCulture),
                },
            };
            var ticket = new Ticket
            {
                TicketTypeId = TicketType.OfficialLogistics,
                CategoryElementId = 2,
                StartDate = DateTime.Now.AddHours(-5),
                EndDate = DateTime.Now.AddHours(5),
                CategoryElement = new CategoryElement() { Name = "element" },
                ScenarioTypeId = ScenarioType.OFFICER,
                TicketId = 1,
                NodeId = 1,
            };

            this.repositoryFactory.Setup(m => m.HomologationRepository.GetAllAsync(
                It.IsAny<Expression<Func<Homologation, bool>>>(),
                "HomologationGroups",
                "HomologationGroups.Group",
                "HomologationGroups.HomologationObjects",
                "HomologationGroups.HomologationDataMapping")).ReturnsAsync(homologations);
            var repoMockAnnulation = new Mock<IRepository<Annulation>>();
            repoMockAnnulation.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Annulation, bool>>>())).ReturnsAsync(annulations);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Annulation>()).Returns(repoMockAnnulation.Object);
            var repoMockElement = new Mock<IRepository<CategoryElement>>();
            repoMockElement.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(elements);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<CategoryElement>()).Returns(repoMockElement.Object);
            this.mockFailureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockFailureHandler.Object);

            var repoMockNodesForSegmentResult = new Mock<IRepository<NodesForSegmentResult>>();
            repoMockNodesForSegmentResult.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(nodesForSegment);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<NodesForSegmentResult>()).Returns(repoMockNodesForSegmentResult.Object);

            var transformedMovement = await this.logisticsService.TransformAsync(movements, ticket, Entities.Dto.SystemType.SIV, ScenarioType.OPERATIONAL).ConfigureAwait(false);

            // Assert
            Assert.AreNotEqual(0, transformedMovement.Count());
            this.repositoryFactory.Verify(
                m => m.HomologationRepository.GetAllAsync(
                It.IsAny<Expression<Func<Homologation, bool>>>(),
                "HomologationGroups",
                "HomologationGroups.Group",
                "HomologationGroups.HomologationObjects",
                "HomologationGroups.HomologationDataMapping"), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<NodesForSegmentResult>(), Times.AtLeastOnce);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Annulation>(), Times.AtLeastOnce);
            repoMockAnnulation.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Annulation, bool>>>()), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<CategoryElement>(), Times.Once);
            repoMockElement.Verify(c => c.GetAllAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>()), Times.Once);
        }

        /// <summary>
        /// TransformAsync tautology key Error.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task TransformAsync_ShouldLogisticsTautologyKeyErrorAsync()
        {
            var homologations = new List<Homologation>();
            var homologation = new Homologation();
            var group = new HomologationGroup();
            var dataMapping = new HomologationDataMapping { SourceValue = "2", DestinationValue = "Tr. Material a material" };
            group.HomologationDataMapping.Add(dataMapping);
            homologation.HomologationGroups.Add(group);
            homologations.Add(homologation);
            var annulations = new List<Annulation> { new Annulation() { SourceMovementTypeId = 1, AnnulationMovementTypeId = 1, IsActive = true } };
            var elements = new List<CategoryElement> { new CategoryElement() { ElementId = 1, Name = "Anul. Tr. Material a material" }, new CategoryElement() { ElementId = 31, Name = "Bbl" } };
            var movements = new List<GenericLogisticsMovement>
            {
                new GenericLogisticsMovement()
                {
                    HasAnnulation = true,
                    MovementTypeId = 1,
                    SourceNodeId = 1,
                    DestinationNodeId = 1,
                    SourceProductId = "1",
                    DestinationProductId = "1",
                    MeasurementUnit = 31,
                    StartDate = Convert.ToDateTime("05/05/2021", CultureInfo.InvariantCulture),
                    EndDate = Convert.ToDateTime("06/05/2021", CultureInfo.InvariantCulture),
                    Classification = Constants.LossClassification,
                    DestinationNodeSendToSap = true,
                    SourceNodeSendToSap = true,
                },
            };
            var nodesForSegment = new List<NodesForSegmentResult>
            {
                new NodesForSegmentResult()
                {
                    IsApproved = true,
                    NodeId = 1,
                    OperationDate = Convert.ToDateTime("05/05/2021", CultureInfo.InvariantCulture),
                },
                new NodesForSegmentResult()
                {
                    IsApproved = true,
                    NodeId = 2,
                    OperationDate = Convert.ToDateTime("05/05/2021", CultureInfo.InvariantCulture),
                },
            };
            var ticket = new Ticket
            {
                TicketTypeId = TicketType.OfficialLogistics,
                CategoryElementId = 2,
                StartDate = DateTime.Now.AddHours(-5),
                EndDate = DateTime.Now.AddHours(5),
                CategoryElement = new CategoryElement() { Name = "element" },
                ScenarioTypeId = ScenarioType.OFFICER,
                TicketId = 1,
                NodeId = 1,
            };

            this.repositoryFactory.Setup(m => m.HomologationRepository.GetAllAsync(
                It.IsAny<Expression<Func<Homologation, bool>>>(),
                "HomologationGroups",
                "HomologationGroups.Group",
                "HomologationGroups.HomologationObjects",
                "HomologationGroups.HomologationDataMapping")).ReturnsAsync(homologations);
            var repoMockAnnulation = new Mock<IRepository<Annulation>>();
            repoMockAnnulation.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Annulation, bool>>>())).ReturnsAsync(annulations);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Annulation>()).Returns(repoMockAnnulation.Object);
            var repoMockElement = new Mock<IRepository<CategoryElement>>();
            repoMockElement.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(elements);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<CategoryElement>()).Returns(repoMockElement.Object);
            this.mockFailureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockFailureHandler.Object);

            var repoMockNodesForSegmentResult = new Mock<IRepository<NodesForSegmentResult>>();
            repoMockNodesForSegmentResult.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(nodesForSegment);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<NodesForSegmentResult>()).Returns(repoMockNodesForSegmentResult.Object);

            var transformedMovement = await this.logisticsService.TransformAsync(movements, ticket, Entities.Dto.SystemType.SIV, ScenarioType.OPERATIONAL).ConfigureAwait(false);

            // Assert
            Assert.AreNotEqual(0, transformedMovement.Count());
            this.repositoryFactory.Verify(
                m => m.HomologationRepository.GetAllAsync(
                It.IsAny<Expression<Func<Homologation, bool>>>(),
                "HomologationGroups",
                "HomologationGroups.Group",
                "HomologationGroups.HomologationObjects",
                "HomologationGroups.HomologationDataMapping"), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<NodesForSegmentResult>(), Times.AtLeastOnce);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Annulation>(), Times.AtLeastOnce);
            repoMockAnnulation.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Annulation, bool>>>()), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<CategoryElement>(), Times.Once);
            repoMockElement.Verify(c => c.GetAllAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>()), Times.Once);
        }

        /// <summary>
        /// DoFinalizeAsyn Apply order ok Error.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task DoFinalizeAsync_ShouldApplyOrderAsync()
        {
            var repoMockLogisticMovement = new Mock<IRepository<LogisticMovement>>();
            this.mockUnitOfWork.Setup(m => m.CreateRepository<LogisticMovement>()).Returns(repoMockLogisticMovement.Object);
            var movements = new List<GenericLogisticsMovement> { new GenericLogisticsMovement() { HasAnnulation = false, MovementTypeId = 1, SourceNodeId = 1, DestinationNodeId = 2, SourceProductId = "2", DestinationProductId = "1", MeasurementUnit = 31, NodeApproved = true } };
            var ticket = new Ticket
            {
                TicketTypeId = TicketType.OfficialLogistics,
                CategoryElementId = 2,
                StartDate = DateTime.Now.AddHours(-5),
                EndDate = DateTime.Now.AddHours(5),
                CategoryElement = new CategoryElement() { Name = "element" },
                ScenarioTypeId = ScenarioType.OFFICER,
                TicketId = 1,
                NodeId = 1,
            };
            await this.logisticsService.DoFinalizeAsync(movements, ticket).ConfigureAwait(false);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<LogisticMovement>(), Times.AtLeastOnce);
            repoMockLogisticMovement.Verify(x => x.InsertAll(It.IsAny<List<LogisticMovement>>()), Times.Once);
        }

        /// <summary>
        /// DoFinalizeAsyn Apply order Destination Node Error.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task DoFinalizeAsync_ShouldApplyOrderDestinationNodeNullAsync()
        {
            var repoMockLogisticMovement = new Mock<IRepository<LogisticMovement>>();
            this.mockUnitOfWork.Setup(m => m.CreateRepository<LogisticMovement>()).Returns(repoMockLogisticMovement.Object);
            var movements = new List<GenericLogisticsMovement>
            {
                new GenericLogisticsMovement()
                {
                    HasAnnulation = false,
                    MovementTypeId = 1,
                    SourceNodeId = 1,
                    DestinationNodeId = null,
                    SourceProductId = "2",
                    DestinationProductId = "1",
                    MeasurementUnit = 31,
                    NodeApproved = true,
                },
            };
            var ticket = new Ticket
            {
                TicketTypeId = TicketType.OfficialLogistics,
                CategoryElementId = 2,
                StartDate = DateTime.Now.AddHours(-5),
                EndDate = DateTime.Now.AddHours(5),
                CategoryElement = new CategoryElement() { Name = "element" },
                ScenarioTypeId = ScenarioType.OFFICER,
                TicketId = 1,
                NodeId = 1,
            };
            await this.logisticsService.DoFinalizeAsync(movements, ticket).ConfigureAwait(false);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<LogisticMovement>(), Times.AtLeastOnce);
            repoMockLogisticMovement.Verify(x => x.InsertAll(It.IsAny<List<LogisticMovement>>()), Times.Once);
        }

        /// <summary>
        /// DoFinalizeAsyn Apply order Source Node Error.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task DoFinalizeAsync_ShouldApplyOrderSourceNodeNullAsync()
        {
            var repoMockLogisticMovement = new Mock<IRepository<LogisticMovement>>();
            this.mockUnitOfWork.Setup(m => m.CreateRepository<LogisticMovement>()).Returns(repoMockLogisticMovement.Object);
            var movements = new List<GenericLogisticsMovement>
            {
                new GenericLogisticsMovement()
                {
                    HasAnnulation = false,
                    MovementTypeId = 1,
                    SourceNodeId = null,
                    DestinationNodeId = 1,
                    SourceProductId = "2",
                    DestinationProductId = "1",
                    MeasurementUnit = 31,
                    NodeApproved = true,
                },
            };
            var ticket = new Ticket
            {
                TicketTypeId = TicketType.OfficialLogistics,
                CategoryElementId = 2,
                StartDate = DateTime.Now.AddHours(-5),
                EndDate = DateTime.Now.AddHours(5),
                CategoryElement = new CategoryElement() { Name = "element" },
                ScenarioTypeId = ScenarioType.OFFICER,
                TicketId = 1,
                NodeId = 1,
            };
            await this.logisticsService.DoFinalizeAsync(movements, ticket).ConfigureAwait(false);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<LogisticMovement>(), Times.AtLeastOnce);
            repoMockLogisticMovement.Verify(x => x.InsertAll(It.IsAny<List<LogisticMovement>>()), Times.Once);
        }

        /// <summary>
        /// DoFinalizeOfficialProcessAsyn SIV.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task DoFinalizeOfficialProcessAsync_ShouldApplyOrderSivAsync()
        {
            var dataSet = new DataSet() { Locale = CultureInfo.InvariantCulture };
            var repoMockLogisticMovement = new Mock<IRepository<LogisticMovement>>();
            this.mockUnitOfWork.Setup(m => m.CreateRepository<LogisticMovement>()).Returns(repoMockLogisticMovement.Object);
            var movements = new List<GenericLogisticsMovement>
            {
                new GenericLogisticsMovement()
                {
                    HasAnnulation = false,
                    MovementTypeId = 1,
                    SourceNodeId = 2,
                    DestinationNodeId = 1,
                    SourceProductId = "2",
                    DestinationProductId = "1",
                    MeasurementUnit = 31,
                    NodeApproved = true,
                    Status = Entities.Core.StatusType.VISUALIZATION,
                },
            };
            var ticket = new Ticket
                {
                    TicketTypeId = TicketType.OfficialLogistics,
                    CategoryElementId = 2,
                    StartDate = DateTime.Now.AddHours(-5),
                    EndDate = DateTime.Now.AddHours(5),
                    CategoryElement = new CategoryElement() { Name = "element" },
                    ScenarioTypeId = ScenarioType.OFFICER,
                    TicketId = 1,
                    NodeId = 1,
                    Owner = new CategoryElement() { Name = "owner" },
                };
            this.mockLogisticsService.Setup(a => a.Generate(It.IsAny<List<OfficialLogisticsDetails>>())).Returns(dataSet);
            this.mockExcelService.Setup(a => a.ExportAndUploadLogisticsExcelAsync(It.IsAny<DataSet>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            await this.logisticsService.DoFinalizeOfficialProcessAsync(movements, ticket, SystemType.SIV).ConfigureAwait(false);
            dataSet.Dispose();
            this.mockExcelService.Verify(a => a.ExportAndUploadLogisticsExcelAsync(It.IsAny<DataSet>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// DoFinalizeOfficialProcessAsyn SAP.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task DoFinalizeOfficialProcessAsync_ShouldApplyOrderSapAsync()
        {
            var repoMockLogisticMovement = new Mock<IRepository<LogisticMovement>>();
            this.mockUnitOfWork.Setup(m => m.CreateRepository<LogisticMovement>()).Returns(repoMockLogisticMovement.Object);
            var movements = new List<GenericLogisticsMovement>
            {
                new GenericLogisticsMovement()
                {
                    HasAnnulation = false,
                    MovementTypeId = 1,
                    SourceNodeId = 2,
                    DestinationNodeId = 1,
                    SourceProductId = "2",
                    DestinationProductId = "1",
                    MeasurementUnit = 31,
                    NodeApproved = true,
                    Status = Entities.Core.StatusType.VISUALIZATION,
                },
            };
            var ticket = new Ticket
            {
                TicketTypeId = TicketType.OfficialLogistics,
                CategoryElementId = 2,
                StartDate = DateTime.Now.AddHours(-5),
                EndDate = DateTime.Now.AddHours(5),
                CategoryElement = new CategoryElement() { Name = "element" },
                ScenarioTypeId = ScenarioType.OFFICER,
                TicketId = 1,
                NodeId = 1,
                Owner = new CategoryElement() { Name = "owner" },
            };
            await this.logisticsService.DoFinalizeOfficialProcessAsync(movements, ticket, SystemType.SAP).ConfigureAwait(false);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<LogisticMovement>(), Times.AtLeastOnce);
        }

        /// <summary>
        /// DoFinalizeOfficialProcessAsyn Sap apply Order.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task DoFinalizeAsync_ShouldApplyOrder_SourceNodeOK_DestinationNodeOkAsync()
        {
            var repoMockLogisticMovement = new Mock<IRepository<LogisticMovement>>();
            this.mockUnitOfWork.Setup(m => m.CreateRepository<LogisticMovement>()).Returns(repoMockLogisticMovement.Object);
            var movements = new List<GenericLogisticsMovement>
            {
                new GenericLogisticsMovement()
                {
                    HasAnnulation = false,
                    MovementTypeId = 1,
                    SourceNodeId = 1,
                    DestinationNodeId = 1,
                    SourceProductId = "2",
                    DestinationProductId = "1",
                    MeasurementUnit = 31,
                    NodeApproved = true,
                },
            };
            var ticket = new Ticket
            {
                TicketTypeId = TicketType.OfficialLogistics,
                CategoryElementId = 2,
                StartDate = DateTime.Now.AddHours(-5),
                EndDate = DateTime.Now.AddHours(5),
                CategoryElement = new CategoryElement() { Name = "element" },
                ScenarioTypeId = ScenarioType.OFFICER,
                TicketId = 1,
                NodeId = 1,
            };
            await this.logisticsService.DoFinalizeAsync(movements, ticket).ConfigureAwait(false);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<LogisticMovement>(), Times.AtLeastOnce);
            repoMockLogisticMovement.Verify(x => x.InsertAll(It.IsAny<List<LogisticMovement>>()), Times.Once);
        }

        /// <summary>
        /// GetLogisticMovementByMovementIdAsync Sap.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task GetLogisticMovementByMovementIdAsync()
        {
            var repoMockLogisticMovement = new Mock<IRepository<LogisticMovement>>();
            this.mockUnitOfWork.Setup(m => m.CreateRepository<LogisticMovement>()).Returns(repoMockLogisticMovement.Object);
            repoMockLogisticMovement.Setup(s => s.FirstOrDefaultAsync(It.IsAny<Expression<Func<LogisticMovement, bool>>>(), It.IsAny<string[]>()));
            await this.logisticsService.GetLogisticMovementByMovementIdAsync("1").ConfigureAwait(false);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<LogisticMovement>(), Times.AtLeastOnce);
        }

        /// <summary>
        /// ExistLogisticMovementByMovementIdAsync Sap.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task ExistLogisticMovementByMovementIdAsync()
        {
            var repoMockLogisticMovement = new Mock<IRepository<LogisticMovement>>();
            this.mockUnitOfWork.Setup(m => m.CreateRepository<LogisticMovement>()).Returns(repoMockLogisticMovement.Object);
            repoMockLogisticMovement.Setup(s => s.FirstOrDefaultAsync(It.IsAny<Expression<Func<LogisticMovement, bool>>>(), It.IsAny<string[]>()));
            await this.logisticsService.ExistLogisticMovementByMovementIdAsync("1").ConfigureAwait(false);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<LogisticMovement>(), Times.AtLeastOnce);
        }

        /// <summary>
        /// ProcessLogisticMovementAsync Sap.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task ProcessLogisticMovementAsync()
        {
            var request = new LogisticMovementResponse()
            {
                MovementId = "1",
                StatusMessage = Constants.SapSuccessProcess,
                Information = "Ok",
            };
            var logisticMovement = new LogisticMovement()
            {
                MovementTransaction = new Movement() { MovementId = "1", MovementTransactionId = 1 },
                StatusProcessId = StatusType.SENT,
                IsCheck = 1,
                LogisticMovementId = 1,
                MovementTransactionId = 1,
            };
            var ticket = new Ticket() { TicketId = 1, };

            var mockTicketRepository = new Mock<IRepository<Ticket>>();
            mockTicketRepository.Setup(x => x.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(ticket);
            this.mockUnitOfWork.Setup(s => s.CreateRepository<Ticket>()).Returns(mockTicketRepository.Object);

            var repoMockLogisticMovement = new Mock<IRepository<LogisticMovement>>();
            repoMockLogisticMovement.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<LogisticMovement, bool>>>())).ReturnsAsync(logisticMovement);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<LogisticMovement>()).Returns(repoMockLogisticMovement.Object);
            await this.logisticsService.ProcessLogisticMovementAsync(request).ConfigureAwait(false);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<LogisticMovement>(), Times.AtLeastOnce);
            mockTicketRepository.Verify(m => m.Update(It.IsAny<Ticket>()), Times.Once);
        }

        /// <summary>
        /// ProcessLogisticMovementAsync Sap fail.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task ProcessLogisticMovementFailAsync()
        {
            var request = new LogisticMovementResponse()
            {
                MovementId = "1",
                StatusMessage = Constants.SapFailedProcess,
                Information = "Ok",
            };
            var logisticMovement = new LogisticMovement()
            {
                MovementTransaction = new Movement() { MovementId = "1", MovementTransactionId = 1 },
                StatusProcessId = StatusType.FAILED,
                IsCheck = 1,
                LogisticMovementId = 1,
                MovementTransactionId = 1,
            };
            var ticket = new Ticket() { TicketId = 1, };

            var mockTicketRepository = new Mock<IRepository<Ticket>>();
            mockTicketRepository.Setup(x => x.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(ticket);
            this.mockUnitOfWork.Setup(s => s.CreateRepository<Ticket>()).Returns(mockTicketRepository.Object);

            var repoMockLogisticMovement = new Mock<IRepository<LogisticMovement>>();
            repoMockLogisticMovement.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<LogisticMovement, bool>>>())).ReturnsAsync(logisticMovement);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<LogisticMovement>()).Returns(repoMockLogisticMovement.Object);
            await this.logisticsService.ProcessLogisticMovementAsync(request).ConfigureAwait(false);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<LogisticMovement>(), Times.AtLeastOnce);
            mockTicketRepository.Verify(m => m.Update(It.IsAny<Ticket>()), Times.Once);
        }
    }
}