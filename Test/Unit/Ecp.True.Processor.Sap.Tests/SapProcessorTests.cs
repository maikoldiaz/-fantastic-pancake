// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapProcessorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Sap.Tests
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
    using Ecp.True.Entities.Configuration.Entities;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Sap.Interfaces;
    using Ecp.True.Processors.Sap.Services;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Sap.Interfaces;
    using Ecp.True.Proxies.Sap.Request;
    using Ecp.True.Proxies.Sap.Response;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The Sap Processor tests.
    /// </summary>
    [TestClass]
    public class SapProcessorTests
    {
        /// <summary>
        /// The failure handler mock.
        /// </summary>
        private readonly Mock<ITelemetry> telemetryMock = new Mock<ITelemetry>();

        /// <summary>
        /// The Sap processor.
        /// </summary>
        private SapProcessor sapProcessor;

        /// <summary>
        /// The unit of work mock.
        /// </summary>
        private Mock<IUnitOfWorkFactory> unitOfWorkFactoryMock;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<IUnitOfWork> unitOfWorkMock;

        /// <summary>
        /// The mock sap proxy.
        /// </summary>
        private Mock<ISapProxy> mockSapProxy;

        /// <summary>
        /// The mock configuration handler.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigurationHandler;

        /// <summary>
        /// The system repository mock.
        /// </summary>
        private Mock<IRepository<Movement>> mockMovementRepository;

        /// <summary>
        /// The mock owner repository.
        /// </summary>
        private Mock<IRepository<Owner>> mockOwnerRepository;

        /// <summary>
        /// The mock attribute repository.
        /// </summary>
        private Mock<IRepository<AttributeEntity>> mockAttributeRepository;

        /// <summary>
        /// The system repository mock.
        /// </summary>
        private Mock<IRepository<SapTracking>> mockSapTrackingRepository;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<SapProcessor>> mockLogger;

        /// <summary>
        /// The sap repository.
        /// </summary>
        private Mock<IRepository<SapMapping>> sapRepository;

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The analysis service client.
        /// </summary>
        private Mock<IAnalysisServiceClient> analysisServiceClient;

        /// <summary>
        /// The BLOB storage client.
        /// </summary>
        private Mock<IBlobStorageClient> mockBlobStorageClient;

        /// <summary>
        /// The sap tracking.
        /// </summary>
        private Mock<ISapTrackingProcessor> mockSapTracking;

        /// <summary>
        /// The sap tracking.
        /// </summary>
        private Mock<IFileRegistrationTransactionService> fileRegistration;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.unitOfWorkMock = new Mock<IUnitOfWork>();
            this.unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
            this.unitOfWorkFactoryMock.Setup(x => x.GetUnitOfWork()).Returns(this.unitOfWorkMock.Object);
            this.mockSapProxy = new Mock<ISapProxy>();
            this.mockConfigurationHandler = new Mock<IConfigurationHandler>();
            this.mockMovementRepository = new Mock<IRepository<Movement>>();
            this.sapRepository = new Mock<IRepository<SapMapping>>();
            this.mockOwnerRepository = new Mock<IRepository<Owner>>();
            this.mockAttributeRepository = new Mock<IRepository<AttributeEntity>>();
            this.mockLogger = new Mock<ITrueLogger<SapProcessor>>();
            this.mockSapTrackingRepository = new Mock<IRepository<SapTracking>>();
            this.unitOfWorkMock.Setup(x => x.SaveAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            this.analysisServiceClient = new Mock<IAnalysisServiceClient>();
            this.mockBlobStorageClient = new Mock<IBlobStorageClient>();
            this.mockSapTracking = new Mock<ISapTrackingProcessor>();
            this.fileRegistration = new Mock<IFileRegistrationTransactionService>();
            this.azureClientFactory = new AzureClientFactory(
                null,
                this.analysisServiceClient.Object,
                null,
                null,
                null);
            this.sapProcessor = new SapProcessor(this.mockLogger.Object, this.unitOfWorkFactoryMock.Object, this.mockSapProxy.Object, this.azureClientFactory, this.telemetryMock.Object, this.mockSapTracking.Object, this.fileRegistration.Object);
        }

        /// <summary>
        /// the processor update transfer point asynchronous asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [Obsolete("This Method is Deprecated", false)]
        public async Task SapProcessor_UpdateTransferPoint_Async()
        {
            this.SetUpDependencies();
            await this.sapProcessor.UpdateTransferPointAsync(1, null).ConfigureAwait(false);
            this.mockSapProxy.Verify(a => a.UpdateMovementTransferPointAsync(It.IsAny<SapMovementRequest>(), It.IsAny<int>()), Times.Once);
        }

        /// <summary>
        /// Saps the processor update transfer point for previous movement asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        [Obsolete("This Method is Deprecated", false)]
        public async Task SapProcessor_UpdateTransferPointForPreviousMovement_Async()
        {
            this.SetUpDependencies();
            await this.sapProcessor.UpdateTransferPointAsync(1, 1).ConfigureAwait(false);
            this.mockSapProxy.Verify(a => a.UpdateMovementTransferPointAsync(It.IsAny<SapMovementRequest>(), It.IsAny<int>()), Times.Once);
        }

        /// <summary>
        /// Saps the processor update transfer point for previous movement error asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [Obsolete("This Method is Deprecated", false)]
        public async Task SapProcessor_UpdateTransferPointForPreviousMovement_ErrorAsync()
        {
            this.SetUpDependencies();
            this.mockSapProxy.Setup(x => x.UpdateMovementTransferPointAsync(It.IsAny<SapMovementRequest>(), It.IsAny<int>())).Throws(new Exception());
            await this.sapProcessor.UpdateTransferPointAsync(1, 1).ConfigureAwait(false);
            this.mockSapProxy.Verify(a => a.UpdateMovementTransferPointAsync(It.IsAny<SapMovementRequest>(), It.IsAny<int>()), Times.Once);
        }

        /// <summary>
        /// Saps the processor update transfer point for previous movement error asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [Obsolete("This Method is Deprecated", false)]
        public async Task SapProcessor_UpdateTransferPointWithCorrectPropertySerializationAsync()
        {
            this.SetUpDependencies();
            var movementObject = new Movement()
            {
                NetStandardVolume = 1,
                GrossStandardVolume = 1,
                SystemId = 1,
                Tolerance = 1,
                MovementId = "1",
                MovementTransactionId = 1,
                ScenarioId = ScenarioType.OPERATIONAL,
            };

            var sapMovement = new SapMovementRequest();
            this.mockMovementRepository.Setup(a => a.FirstOrDefaultAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(movementObject);
            this.mockSapProxy.Setup(x => x.UpdateMovementTransferPointAsync(It.IsAny<SapMovementRequest>(), It.IsAny<int>())).Callback<SapMovementRequest, int>((movement, backUpMovementId) =>
            {
                sapMovement = movement;
            }).ReturnsAsync(new SapTrackingStatus(string.Empty) { IsSuccess = true });

            await this.sapProcessor.UpdateTransferPointAsync(1, 1).ConfigureAwait(false);
            this.mockSapProxy.Verify(a => a.UpdateMovementTransferPointAsync(It.IsAny<SapMovementRequest>(), It.IsAny<int>()), Times.Once);
            Assert.AreEqual(movementObject.NetStandardVolume, sapMovement.NetStandardVolume);
            Assert.AreEqual(movementObject.GrossStandardVolume, sapMovement.GrossStandardVolume);
            Assert.AreEqual(movementObject.Tolerance, sapMovement.Tolerance);
            Assert.AreEqual(movementObject.SystemId.ToString(), sapMovement.SystemId);
            Assert.AreEqual(Convert.ToString((int)ScenarioType.OPERATIONAL, CultureInfo.InvariantCulture), sapMovement.ScenarioId);
        }

        [TestMethod]
        public async Task SapProcessor_ShouldSync_WithSuccessAsync()
        {
            var analysisSettings = new AnalysisSettings
            {
                ClientId = "ClientID",
                ClientSecret = "ClientSecret",
                ModelName = "ModelName",
                Region = "Region",
                ServerName = "ServerName",
                TenantId = "TenantID",
            };

            var storageSettings = new StorageSettings
            {
                ConnectionString = "ConnectionString",
                MaxAttempts = 5,
                DeltaBackOff = 2,
            };

            this.analysisServiceClient.Setup(x => x.Initialize(analysisSettings));

            this.azureClientFactory.Initialize(new AzureConfiguration(analysisSettings, storageSettings, new ServiceBusSettings()));

            // Act
            IEnumerable<SapMappingResponse> sapMappingsResponseFormAPI = this.GetSapMappingResponse();
            var sapMappingsFromDB = new List<SapMapping> { new SapMapping { SourceProductId = "456" } };
            this.unitOfWorkMock.Setup(x => x.CreateRepository<SapMapping>()).Returns(this.sapRepository.Object);
            this.sapRepository.Setup(x => x.GetAllAsync(null)).ReturnsAsync(sapMappingsFromDB);
            this.mockSapProxy.Setup(x => x.GetMappingsAsync()).ReturnsAsync(sapMappingsResponseFormAPI);
            await this.sapProcessor.SyncAsync().ConfigureAwait(false);
            this.unitOfWorkMock.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
            this.sapRepository.Verify(x => x.GetAllAsync(null), Times.Once);
            this.sapRepository.Verify(x => x.DeleteAll(sapMappingsFromDB), Times.Once);
            this.sapRepository.Verify(x => x.InsertAll(It.IsAny<IEnumerable<SapMapping>>()), Times.Once);
            this.analysisServiceClient.Verify(x => x.RefreshSapMappingDetailsAsync(), Times.Once);
        }

        private void SetUpDependencies()
        {
            var sapTracking = new SapTracking { MovementTransactionId = 1 };
            var movementObject = new Movement()
            {
                NetStandardVolume = 1,
                GrossStandardVolume = 1,
                SystemId = 1,
                Tolerance = 1,
                MovementId = "1",
                MovementTransactionId = 1,
            };
            var owners = new List<Owner> { new Owner() };
            var attributes = new List<AttributeEntity> { new AttributeEntity() };

            var uploadStatus = new SapTrackingStatus(string.Empty) { IsSuccess = true };
            this.mockLogger.Setup(x => x.LogInformation(It.IsAny<string>())).Verifiable();
            this.mockSapProxy.Setup(x => x.UpdateMovementTransferPointAsync(It.IsAny<SapMovementRequest>(), It.IsAny<int>())).ReturnsAsync(uploadStatus);
            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<SapSettings>(It.IsAny<string>())).ReturnsAsync(new SapSettings() { TransferPointPath = string.Empty });
            this.unitOfWorkMock.Setup(x => x.CreateRepository<SapTracking>()).Returns(this.mockSapTrackingRepository.Object);
            this.unitOfWorkMock.Setup(s => s.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<Owner>()).Returns(this.mockOwnerRepository.Object);
            this.unitOfWorkMock.Setup(s => s.CreateRepository<AttributeEntity>()).Returns(this.mockAttributeRepository.Object);
            this.mockSapTrackingRepository.Setup(a => a.FirstOrDefaultAsync(It.IsAny<Expression<Func<SapTracking, bool>>>())).ReturnsAsync(sapTracking);
            this.mockMovementRepository.Setup(a => a.FirstOrDefaultAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(movementObject);
            this.mockSapTrackingRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Owner, bool>>>())).ReturnsAsync(owners);
            this.mockMovementRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<AttributeEntity, bool>>>())).ReturnsAsync(attributes);
        }

        private List<SapMappingResponse> GetSapMappingResponse()
        {
            return new List<SapMappingResponse>()
            {
                new SapMappingResponse
                {
                    SourceProductId = "123",
                    DestinationMovementTypeId = 1,
                    DestinationProductId = "1",
                    DestinationSystemDestinationNodeId = 1,
                    DestinationSystemSourceNodeId = 1,
                    DestinationSystemId = 1,
                    OfficialSystem = 1,
                    SourceSystemDestinationNodeId = 1,
                    SourceMovementTypeId = 1,
                    SourceSystemId = 1,
                    SourceSystemSourceNodeId = 1,
                },
            };
        }
    }
}
