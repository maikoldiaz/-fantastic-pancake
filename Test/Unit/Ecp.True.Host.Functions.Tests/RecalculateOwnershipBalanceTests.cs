// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecalculateOwnershipBalanceTests.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Chaos.Interfaces;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Host.Functions.Ownership;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Processors.Ownership.Services.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The OwnershipGeneratorTests.
    /// </summary>
    [TestClass]
    public class RecalculateOwnershipBalanceTests
    {
        /// <summary>
        /// The ownership generator.
        /// </summary>
        private RecalculateOwnershipBalance ownershipGenerator;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<ITrueLogger<RecalculateOwnershipBalance>> mockLogger;

        private Mock<ITrueLogger<FunctionBase>> mockFunctionBaseLogger;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private Mock<IServiceProvider> mockServiceProvider;

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureClientFactory;

        /// <summary>
        /// The mock connection factory.
        /// </summary>
        private Mock<IConnectionFactory> mockConnectionFactory;

        /// <summary>
        /// The mock configuration handler.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigHandler;

        /// <summary>
        /// The mock data generator service.
        /// </summary>
        private Mock<IDataGeneratorService> mockDataGeneratorService;

        /// <summary>
        /// The mock ownership calculation service.
        /// </summary>
        private Mock<IOwnershipCalculationService> mockOwnershipCalculationService;

        /// <summary>
        /// The mock ownership calculation service.
        /// </summary>
        private Mock<IOwnershipRuleProcessor> mockOwnershipRuleProcessor;

        /// <summary>
        /// The mock ownership calculation service.
        /// </summary>
        private Mock<IExcelService> mockExcelService;

        /// <summary>
        /// Gets the mock SQL token provider.
        /// </summary>
        /// <value>
        /// The mock SQL token provider.
        /// </value>
        private Mock<ISqlTokenProvider> mockSqlTokenProvider;

        /// <summary>
        /// The mock chaos manager.
        /// </summary>
        private Mock<IChaosManager> mockChaosManager;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockDataGeneratorService = new Mock<IDataGeneratorService>();
            this.mockOwnershipCalculationService = new Mock<IOwnershipCalculationService>();
            this.mockLogger = new Mock<ITrueLogger<RecalculateOwnershipBalance>>();
            this.mockServiceProvider = new Mock<IServiceProvider>();
            this.mockExcelService = new Mock<IExcelService>();
            this.mockOwnershipRuleProcessor = new Mock<IOwnershipRuleProcessor>();
            this.mockFunctionBaseLogger = new Mock<ITrueLogger<FunctionBase>>();
            this.ownershipGenerator = new RecalculateOwnershipBalance(
                this.mockLogger.Object,
                this.mockServiceProvider.Object,
                this.mockOwnershipRuleProcessor.Object);
        }

        /// <summary>
        /// Recalculates ownership asynchronous should process ownership recalculation asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ReCalculateOwnershipTicketAsync_ShouldProcessOwnershipRecalculationAsync()
        {
            this.SetupMocks(true);
            this.mockOwnershipRuleProcessor.Setup(a => a.ProcessAsync(It.IsAny<OwnershipRuleData>(), It.IsAny<ChainType>()));
            this.mockOwnershipRuleProcessor.Setup(a => a.FinalizeProcessAsync(It.IsAny<OwnershipRuleData>()));

            await this.ownershipGenerator.RecalculateOwnershipBalanceAsync(1, "label", "replyTo", new ExecutionContext()).ConfigureAwait(false);

            this.mockOwnershipRuleProcessor.Verify(a => a.ProcessAsync(It.IsAny<OwnershipRuleData>(), It.IsAny<ChainType>()), Times.Once);
            this.mockOwnershipRuleProcessor.Verify(a => a.FinalizeProcessAsync(It.IsAny<OwnershipRuleData>()), Times.Once);
        }

        /// <summary>
        /// Recalculates ownership asynchronous should throw exception asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ReCalculateOwnershipTicketAsync_ShouldThrowExceptionAsync()
        {
            this.SetupMocks(true);
            this.mockOwnershipRuleProcessor.Setup(a => a.ProcessAsync(It.IsAny<OwnershipRuleData>(), It.IsAny<ChainType>()));
            this.mockOwnershipRuleProcessor.Setup(a => a.FinalizeProcessAsync(It.IsAny<OwnershipRuleData>()));

            await this.ownershipGenerator.RecalculateOwnershipBalanceAsync(null, "label", "replyTo", new ExecutionContext()).ConfigureAwait(false);
        }

        /// <summary>
        /// Setups the mocks.
        /// </summary>
        /// <param name="isReady">if set to <c>true</c> [is ready].</param>
        private void SetupMocks(bool isReady)
        {
            this.mockConnectionFactory = new Mock<IConnectionFactory>();
            this.mockConfigHandler = new Mock<IConfigurationHandler>();
            this.mockAzureClientFactory = new Mock<IAzureClientFactory>();
            this.mockSqlTokenProvider = new Mock<ISqlTokenProvider>();
            this.mockChaosManager = new Mock<IChaosManager>();

            this.mockConnectionFactory.Setup(m => m.SetupStorageConnection(It.IsAny<string>()));
            this.mockConnectionFactory.SetupGet(m => m.IsReady).Returns(isReady);

            this.mockAzureClientFactory.SetupGet(m => m.IsReady).Returns(isReady);
            this.mockAzureClientFactory.Setup(m => m.Initialize(It.IsAny<AzureConfiguration>()));

            this.mockServiceProvider.Setup(s => s.GetService(typeof(IConnectionFactory))).Returns(this.mockConnectionFactory.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(ITrueLogger<FunctionBase>))).Returns(this.mockFunctionBaseLogger.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(ITrueLogger<OwnershipGenerator>))).Returns(this.mockLogger.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IConfigurationHandler))).Returns(this.mockConfigHandler.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IAzureClientFactory))).Returns(this.mockAzureClientFactory.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IDataGeneratorService))).Returns(this.mockDataGeneratorService.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IOwnershipCalculationService))).Returns(this.mockOwnershipCalculationService.Object);
            this.mockServiceProvider.Setup(r => r.GetService(typeof(ISqlTokenProvider))).Returns(this.mockSqlTokenProvider.Object);
            this.mockServiceProvider.Setup(r => r.GetService(typeof(IChaosManager))).Returns(this.mockChaosManager.Object);
        }
    }
}
