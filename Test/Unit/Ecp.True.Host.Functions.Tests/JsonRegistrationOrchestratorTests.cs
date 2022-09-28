// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonRegistrationOrchestratorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Chaos.Interfaces;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Host.Functions.Transform;
    using Ecp.True.Host.Functions.Transform.Orchestrator;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Transform.Homologate;
    using Ecp.True.Processors.Transform.Services.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json.Linq;
    using ChaosConstants = Ecp.True.Chaos.Constants;
    using CoreConstants = Ecp.True.Core.Constants;

    [TestClass]
    public class JsonRegistrationOrchestratorTests
    {
        /// <summary>
        /// The failure handler mock.
        /// </summary>
        private readonly Mock<ITelemetry> telemetryMock = new Mock<ITelemetry>();

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<ITrueLogger<JsonRegistrationOrchestrator>> mockLogger;

        /// <summary>
        /// The homologator.
        /// </summary>
        private Mock<IHomologator> mockHomologator;

        /// <summary>
        /// The json transform processor.
        /// </summary>
        private Mock<ITransformProcessor> mockJsonTransformProcessor;

        /// <summary>
        /// The service provider.
        /// </summary>
        private Mock<IServiceProvider> mockServiceProvider;

        /// <summary>
        /// The excel transform processor.
        /// </summary>
        private Mock<IDurableOrchestrationContext> mockDurableOrchestrationContext;

        /// <summary>
        /// The excel transform processor.
        /// </summary>
        private Mock<ISqlTokenProvider> mockSqlTokenProvider;

        /// <summary>
        /// The excel transform processor.
        /// </summary>
        private Mock<IConnectionFactory> mockConnectionFactory;

        /// <summary>
        /// The excel transform processor.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigurationHandler;

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureClientFactory;

        /// <summary>
        /// The mock homologation mapper.
        /// </summary>
        private Mock<IHomologationMapper> mockHomologationMapper;

        /// <summary>
        /// The mock transformation mapper.
        /// </summary>
        private Mock<ITransformationMapper> mockTransformationMapper;

        /// <summary>
        /// The mock chaos manager.
        /// </summary>
        private Mock<IChaosManager> mockChaosManager;

        /// <summary>
        /// The json transregistration orchestrator.
        /// </summary>
        private JsonRegistrationOrchestrator jsonRegistrationOrchestrator;

        /// <summary>
        /// Initialize the required set up.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<JsonRegistrationOrchestrator>>();
            this.mockHomologator = new Mock<IHomologator>();
            this.mockJsonTransformProcessor = new Mock<ITransformProcessor>();
            this.mockJsonTransformProcessor.SetupGet(x => x.InputType).Returns(InputType.JSON);
            this.mockServiceProvider = new Mock<IServiceProvider>();
            this.mockDurableOrchestrationContext = new Mock<IDurableOrchestrationContext>();
            var transformProcessorsList = new List<ITransformProcessor>() { this.mockJsonTransformProcessor.Object };
            this.mockSqlTokenProvider = new Mock<ISqlTokenProvider>();
            this.mockConnectionFactory = new Mock<IConnectionFactory>();
            this.mockConfigurationHandler = new Mock<IConfigurationHandler>();
            this.mockAzureClientFactory = new Mock<IAzureClientFactory>();
            this.mockHomologationMapper = new Mock<IHomologationMapper>();
            this.mockTransformationMapper = new Mock<ITransformationMapper>();
            this.mockChaosManager = new Mock<IChaosManager>();
            this.telemetryMock.Setup(t => t.TrackEvent(CoreConstants.Critical, EventName.RegistrationFailureEvent.ToString("G"), It.IsAny<IDictionary<string, string>>(), null, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()));
            this.mockServiceProvider.Setup(r => r.GetService(typeof(IChaosManager))).Returns(this.mockChaosManager.Object);
            this.mockServiceProvider.Setup(x => x.GetService(typeof(ISqlTokenProvider))).Returns(this.mockSqlTokenProvider.Object);
            this.mockServiceProvider.Setup(x => x.GetService(typeof(IConnectionFactory))).Returns(this.mockConnectionFactory.Object);
            this.mockServiceProvider.Setup(x => x.GetService(typeof(IConfigurationHandler))).Returns(this.mockConfigurationHandler.Object);
            this.mockConfigurationHandler.Setup(x => x.InitializeAsync());
            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<SqlConnectionSettings>(It.IsAny<string>())).ReturnsAsync(new SqlConnectionSettings());
            this.mockConfigurationHandler.Setup(m => m.GetConfigurationAsync(ConfigurationConstants.ConfigConnectionString)).ReturnsAsync(ConfigurationConstants.ConfigConnectionString);
            this.mockConfigurationHandler.Setup(m => m.GetConfigurationAsync<AnalysisSettings>(ConfigurationConstants.AnalysisSettings)).ReturnsAsync(new AnalysisSettings());
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IHomologationMapper))).Returns(this.mockHomologationMapper.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(ITransformationMapper))).Returns(this.mockTransformationMapper.Object);
            this.mockConnectionFactory.Setup(x => x.SetupSqlConfig(It.IsAny<SqlConnectionSettings>()));
            this.mockSqlTokenProvider.Setup(x => x.InitializeAsync());
            this.jsonRegistrationOrchestrator = new JsonRegistrationOrchestrator(
                this.mockConfigurationHandler.Object,
                this.mockAzureClientFactory.Object,
                this.mockServiceProvider.Object,
                this.mockLogger.Object,
                this.mockHomologator.Object,
                transformProcessorsList,
                this.telemetryMock.Object);
        }

        /// <summary>
        /// The DoRegisterAsync should register asynchronously.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task DoRegisterAsync_ShouldRegister_WhenTriggeredAsync()
        {
            // Arrange
            var registrationData = new RegistrationData()
            {
                InvocationId = Guid.NewGuid(),
                TrueMessage = new Entities.Dto.TrueMessage(),
            };

            this.mockDurableOrchestrationContext.Setup(x => x.GetInput<RegistrationData>()).Returns(registrationData);
            this.mockDurableOrchestrationContext.Setup(x => x.CallActivityAsync<RegistrationData>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(registrationData);

            // Act
            await this.jsonRegistrationOrchestrator.RegisterAsync(this.mockDurableOrchestrationContext.Object).ConfigureAwait(false);

            // Assert
            this.mockDurableOrchestrationContext.Verify(x => x.CallActivityAsync<RegistrationData>(It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(2));
        }

        /// <summary>
        /// The DoTransformAsync should transform asynchronously.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task DoTransformAsync_ShouldTransform_WhenTriggeredAsync()
        {
            // Arrange
            var trueMessage = new TrueMessage()
            {
                ActivityId = Guid.NewGuid(),
            };
            var registrationData = new RegistrationData()
            {
                InvocationId = Guid.NewGuid(),
                TrueMessage = trueMessage,
            };

            this.mockJsonTransformProcessor.Setup(x => x.TransformAsync(It.IsAny<TrueMessage>())).ReturnsAsync(registrationData);

            // Act
            var result = await this.jsonRegistrationOrchestrator.TransformAsync(registrationData).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(trueMessage, result.TrueMessage);
        }

        /// <summary>
        /// The DoHomologateAsync should homologate asynchronously.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task DoHomologateAsync_ShouldHomologate_WhenTriggeredAsync()
        {
            // Arrange
            var trueMessage = new TrueMessage()
            {
                ActivityId = Guid.NewGuid(),
            };
            var registrationData = new RegistrationData()
            {
                InvocationId = Guid.NewGuid(),
                TrueMessage = trueMessage,
                Data = new JArray(),
            };

            // Act
            var result = await this.jsonRegistrationOrchestrator.HomologateAsync(registrationData).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(trueMessage, result.TrueMessage);
        }

        /// <summary>
        /// The DoCompleteAsync should complete asynchronously.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task DoCompleteAsync_ShouldComplete_WhenTriggeredAsync()
        {
            // Arrange
            var trueMessage = new TrueMessage()
            {
                ActivityId = Guid.NewGuid(),
            };
            var registrationData = new RegistrationData()
            {
                InvocationId = Guid.NewGuid(),
                TrueMessage = trueMessage,
                Data = new JArray(),
            };
            this.mockJsonTransformProcessor.Setup(x => x.CompleteAsync(It.IsAny<JArray>(), It.IsAny<TrueMessage>())).Returns(Task.CompletedTask);

            // Act
            await this.jsonRegistrationOrchestrator.CompleteAsync(registrationData).ConfigureAwait(false);

            // Assert
            this.mockJsonTransformProcessor.Verify(x => x.CompleteAsync(It.IsAny<JArray>(), It.IsAny<TrueMessage>()), Times.Once);
        }

        /// <summary>
        /// The DoRegisterAsync should raise chaos when register asynchronously.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task DoRegisterAsync_ShouldRaiseChaos_WhenTriggeredAsync()
        {
            // Arrange
            var registrationData = new RegistrationData()
            {
                InvocationId = Guid.NewGuid(),
                TrueMessage = new Entities.Dto.TrueMessage(),
                ChaosValue = "Transform",
                Caller = FunctionNames.Transform,
            };

            this.mockChaosManager.Setup(x => x.TryTriggerChaos(It.IsAny<string>())).Returns(ChaosConstants.ErrorMessage);
            this.mockDurableOrchestrationContext.Setup(x => x.GetInput<RegistrationData>()).Returns(registrationData);

            // Act
            await this.jsonRegistrationOrchestrator.RegisterAsync(this.mockDurableOrchestrationContext.Object).ConfigureAwait(false);

            // Assert
            this.mockServiceProvider.Verify(r => r.GetService(typeof(IChaosManager)), Times.Exactly(2));
            this.mockChaosManager.Verify(x => x.TryTriggerChaos(It.IsAny<string>()), Times.Exactly(1));
            this.telemetryMock.VerifyAll();
        }
    }
}
