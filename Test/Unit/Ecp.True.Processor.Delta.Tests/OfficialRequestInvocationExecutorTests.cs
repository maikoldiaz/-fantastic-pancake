// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialRequestInvocationExecutorTests.cs" company="Microsoft">
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
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Delta.Entities;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Processors.Delta.Integration;
    using Ecp.True.Processors.Delta.OfficialDeltaExecutors;
    using Ecp.True.Processors.Delta.Services;
    using Ecp.True.Proxies.OwnershipRules.Interfaces;
    using Ecp.True.Proxies.OwnershipRules.OfficialDeltaDataRequest;
    using Ecp.True.Proxies.OwnershipRules.Response;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The RequestInvocationExecutorTests.
    /// </summary>
    [TestClass]
    public class OfficialRequestInvocationExecutorTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<RequestInvocationExecutor>> mockLogger;

        /// <summary>
        /// The mock delta proxy.
        /// </summary>
        private Mock<IDeltaProxy> mockDeltaProxy;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The mock failure handler factory.
        /// </summary>
        private Mock<IFailureHandlerFactory> mockFailureHandlerFactory;

        /// <summary>
        /// The mock configuration handler.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigurationHandler;

        /// <summary>
        /// The Save Integration File mock.
        /// </summary>
        private Mock<ISaveIntegrationDeltaFile> mockSaveIntegrationFile;

        /// <summary>
        /// The request invocation executor.
        /// </summary>
        private IExecutor requestInvocationExecutor;

        /// <summary>
        /// The movement aggregation strategy.
        /// </summary>
        private Mock<IMovementAggregationService> mockMovementAggregationStrategy;

        private Mock<IOfficialDeltaResponseConvertService> mockOfficialDeltaResponseConvertService;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<RequestInvocationExecutor>>();
            this.mockDeltaProxy = new Mock<IDeltaProxy>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockFailureHandlerFactory = new Mock<IFailureHandlerFactory>();
            this.mockConfigurationHandler = new Mock<IConfigurationHandler>();
            this.mockSaveIntegrationFile = new Mock<ISaveIntegrationDeltaFile>();
            this.mockMovementAggregationStrategy = new Mock<IMovementAggregationService>();
            this.mockOfficialDeltaResponseConvertService = new Mock<IOfficialDeltaResponseConvertService>();

            this.requestInvocationExecutor = new RequestInvocationExecutor(
                this.mockDeltaProxy.Object,
                this.mockUnitOfWorkFactory.Object,
                this.mockFailureHandlerFactory.Object,
                this.mockConfigurationHandler.Object,
                this.mockSaveIntegrationFile.Object,
                this.mockLogger.Object,
                this.mockMovementAggregationStrategy.Object,
                this.mockOfficialDeltaResponseConvertService.Object);
        }

        /// <summary>
        /// Requests the invocation executor should return order.
        /// </summary>
        [TestMethod]
        public void RequestInvocationExecutor_ShouldReturnOrder()
        {
            Assert.AreEqual(2, this.requestInvocationExecutor.Order);
        }

        /// <summary>
        /// Executes the asynchronous should return reponse asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExecuteAsync_ShouldReturnReponseAsync()
        {
            (OfficialDeltaData deltaData, OfficialDeltaResponse response) = this.SetUpData();

            this.mockDeltaProxy.Setup(a => a.ProcessOfficialDeltaAsync(It.IsAny<OfficialDeltaRequest>())).ReturnsAsync(response);
            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<OwnershipRuleSettings>(ConfigurationConstants.OwnershipRuleSettings)).ReturnsAsync(new OwnershipRuleSettings() { ShouldStoreResponse = true });
            this.mockSaveIntegrationFile.Setup(x => x.RegisterIntegrationAsync(It.IsAny<IntegrationData>())).ReturnsAsync(Guid.NewGuid().ToString);

            await this.requestInvocationExecutor.ExecuteAsync(deltaData).ConfigureAwait(false);

            Assert.IsNotNull(deltaData);
            Assert.AreEqual(1, deltaData.OfficialResultInventories.Count());
            Assert.AreEqual(1, deltaData.MovementErrors.Count());
            Assert.AreEqual(1, deltaData.InventoryErrors.Count());

            Assert.AreEqual(response.ResultInventories.First().InventoryTransactionId, deltaData.OfficialResultInventories.First().TransactionId.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(true, deltaData.OfficialResultInventories.First().Sign);
            Assert.AreEqual(response.ResultInventories.First().DeltaOfficial, deltaData.OfficialResultInventories.First().OfficialDelta);

            Assert.AreEqual(response.ErrorMovements.First().MovementTransactionId, deltaData.MovementErrors.First().MovementTransactionId.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(response.ErrorMovements.First().Description, deltaData.MovementErrors.First().Description);

            Assert.AreEqual(response.ErrorInventories.First().InventoryTransactionId, deltaData.InventoryErrors.First().InventoryProductId.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(response.ErrorInventories.First().Description, deltaData.InventoryErrors.First().Description);

            this.mockDeltaProxy.Verify(a => a.ProcessOfficialDeltaAsync(It.IsAny<OfficialDeltaRequest>()), Times.Once);
            this.mockMovementAggregationStrategy.Verify(a => a.AggregateTolerancesAndUnidentifiedLosses(It.IsAny<IEnumerable<OfficialDeltaConsolidatedMovement>>(), It.IsAny<OfficialDeltaData>()), Times.Once);
            this.mockOfficialDeltaResponseConvertService.Verify(a => a.ConvertOfficialDeltaResponse(It.IsAny<IEnumerable<OfficialDeltaResultMovement>>(), It.IsAny<OfficialDeltaData>()), Times.Once);
            this.mockSaveIntegrationFile.Verify(a => a.RegisterIntegrationAsync(It.IsAny<IntegrationData>()), Times.Exactly(2));
        }

        /// <summary>
        /// Sets up data.
        /// </summary>
        /// <returns>The tuple[DeltaData, DeltaResponse].</returns>
        private (OfficialDeltaData deltaData, OfficialDeltaResponse response) SetUpData()
        {
            var deltaData = new OfficialDeltaData { Ticket = new Ticket { TicketId = 123 } };
            deltaData.OfficialResultMovements = new List<OfficialResultMovement>
            {
                new OfficialResultMovement
                {
                   MovementTransactionId = 1,
                   Sign = true,
                   OfficialDelta = 10,
                },
            };

            deltaData.OfficialResultInventories = new List<OfficialResultInventory>
            {
                new OfficialResultInventory { TransactionId = 1, Sign = true, OfficialDelta = 10 },
            };

            deltaData.MovementErrors = new List<OfficialErrorMovement>
            {
                new OfficialErrorMovement { MovementTransactionId = 1, Description = "SomeError", },
            };

            deltaData.InventoryErrors = new List<OfficialErrorInventory>
            {
                new OfficialErrorInventory { InventoryProductId = 1, Description = "Some Error", },
            };

            var deltaResultMovement = new OfficialDeltaResultMovement()
            {
                MovementTransactionId = "1",
                Sign = Constants.Positive,
                DeltaOfficial = 10,
            };

            var deltaResultInventory = new OfficialDeltaResultInventory()
            {
                InventoryTransactionId = "1",
                Sign = Constants.Positive,
                DeltaOfficial = 10,
            };

            var deltaErrorMovement = new OfficialDeltaErrorMovement()
            {
                MovementTransactionId = "3",
                Description = "SomeError",
            };

            var deltaErrorInventory = new OfficialDeltaErrorInventory()
            {
                InventoryTransactionId = "1",
                Description = "Some Error",
            };

            var deltaResponse = new OfficialDeltaResponse
            {
                ResultMovements = new List<OfficialDeltaResultMovement> { deltaResultMovement },
                ResultInventories = new List<OfficialDeltaResultInventory> { deltaResultInventory },
                ErrorMovements = new List<OfficialDeltaErrorMovement> { deltaErrorMovement },
                ErrorInventories = new List<OfficialDeltaErrorInventory> { deltaErrorInventory },
            };

            return (deltaData, deltaResponse);
        }
    }
}
