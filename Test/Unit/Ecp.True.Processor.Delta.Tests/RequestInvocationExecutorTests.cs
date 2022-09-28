// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestInvocationExecutorTests.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Delta.Entities;
    using Ecp.True.Processors.Delta.Executors;
    using Ecp.True.Processors.Delta.Integration;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.OwnershipRules.Interfaces;
    using Ecp.True.Proxies.OwnershipRules.Request;
    using Ecp.True.Proxies.OwnershipRules.Response;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The RequestInvocationExecutorTests.
    /// </summary>
    [TestClass]
    public class RequestInvocationExecutorTests
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
        /// The mockblob client.
        /// </summary>
        private Mock<IBlobStorageSasClient> mockBlobSasClient;

        /// <summary>
        /// The request invocation executor.
        /// </summary>
        private IExecutor requestInvocationExecutor;

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
            this.mockBlobSasClient = new Mock<IBlobStorageSasClient>();

            this.requestInvocationExecutor = new RequestInvocationExecutor(
                this.mockDeltaProxy.Object,
                this.mockUnitOfWorkFactory.Object,
                this.mockFailureHandlerFactory.Object,
                this.mockConfigurationHandler.Object,
                this.mockSaveIntegrationFile.Object,
                this.mockLogger.Object);
        }

        /// <summary>
        /// Requests the invocation executor should return order.
        /// </summary>
        [TestMethod]
        public void RequestInvocationExecutor_ShouldReturnOrder()
        {
            Assert.AreEqual(3, this.requestInvocationExecutor.Order);
        }

        /// <summary>
        /// Executes the asynchronous should return reponse asynchronous.
        /// </summary>
        /// <returns>The task.</returns>~
        [TestMethod]
        public async Task ExecuteAsync_ShouldReturnReponseAsync()
        {
            (DeltaData deltaData, DeltaResponse response) = this.SetUpData();

            this.mockDeltaProxy.Setup(a => a.ProcessDeltaAsync(It.IsAny<DeltaRequest>(), It.IsAny<int>())).ReturnsAsync(response);
            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<OwnershipRuleSettings>(ConfigurationConstants.OwnershipRuleSettings)).ReturnsAsync(new OwnershipRuleSettings { ShouldStoreResponse = true });
            this.mockSaveIntegrationFile.Setup(x => x.RegisterIntegrationAsync(It.IsAny<IntegrationData>())).ReturnsAsync(Guid.NewGuid().ToString);

            await this.requestInvocationExecutor.ExecuteAsync(deltaData).ConfigureAwait(false);

            Assert.IsNotNull(deltaData);
            Assert.AreEqual(1, deltaData.ResultMovements.Count());
            Assert.AreEqual(1, deltaData.ResultInventories.Count());
            Assert.AreEqual(1, deltaData.ErrorMovements.Count());
            Assert.AreEqual(1, deltaData.ErrorInventories.Count());

            Assert.AreEqual(response.ResultMovements.First().MovementId, deltaData.ResultMovements.First().MovementId);
            Assert.AreEqual(response.ResultMovements.First().MovementTransactionId, deltaData.ResultMovements.First().MovementTransactionId);
            Assert.AreEqual(true, deltaData.ResultMovements.First().Sign);
            Assert.AreEqual(response.ResultMovements.First().Delta, deltaData.ResultMovements.First().Delta);

            Assert.AreEqual(response.ResultInventories.First().InventoryProductUniqueId, deltaData.ResultInventories.First().InventoryProductUniqueId);
            Assert.AreEqual(response.ResultInventories.First().InventoryTransactionId, deltaData.ResultInventories.First().InventoryProductId);
            Assert.AreEqual(true, deltaData.ResultInventories.First().Sign);
            Assert.AreEqual(response.ResultInventories.First().Delta, deltaData.ResultInventories.First().Delta);

            Assert.AreEqual(response.ErrorMovements.First().MovementId, deltaData.ErrorMovements.First().MovementId);
            Assert.AreEqual(response.ErrorMovements.First().MovementTransactionId, deltaData.ErrorMovements.First().MovementTransactionId);
            Assert.AreEqual(response.ErrorMovements.First().Description, deltaData.ErrorMovements.First().Description);

            Assert.AreEqual(response.ErrorInventories.First().InventoryProductUniqueId, deltaData.ErrorInventories.First().InventoryId);
            Assert.AreEqual(response.ErrorInventories.First().InventoryTransactionId, deltaData.ErrorInventories.First().InventoryProductId);
            Assert.AreEqual(response.ErrorInventories.First().Description, deltaData.ErrorInventories.First().Description);

            this.mockDeltaProxy.Verify(a => a.ProcessDeltaAsync(It.IsAny<DeltaRequest>(), It.IsAny<int>()), Times.Once);
            this.mockSaveIntegrationFile.Verify(a => a.RegisterIntegrationAsync(It.IsAny<IntegrationData>()), Times.Exactly(2));
        }

        /// <summary>
        /// Requests the invocation executor should return the process type when invoked.
        /// </summary>
        [TestMethod]
        public void RequestInvocationExecutor_ShouldReturnTheProcessType_WhenInvoked()
        {
            Assert.AreEqual(ProcessType.Delta, this.requestInvocationExecutor.ProcessType);
        }

        /// <summary>
        /// Sets up data.
        /// </summary>
        /// <returns>The tuple[DeltaData, DeltaResponse].</returns>
        private (DeltaData deltaData, DeltaResponse response) SetUpData()
        {
            var deltaData = new DeltaData { Ticket = new Ticket { TicketId = 123 } };
            deltaData.OriginalMovements = new List<Entities.Query.OriginalMovement>
            {
                new Entities.Query.OriginalMovement { MovementId = "Mov1", MovementTransactionId = 1, NetStandardVolume = 123.33M },
            };

            deltaData.UpdatedMovements = new List<UpdatedMovement>
            {
                new UpdatedMovement { MovementId = "Mov1", MovementTransactionId = 1, NetStandardVolume = 123.33M, EventType = Entities.Registration.EventType.Insert.ToString() },
            };

            deltaData.OriginalInventories = new List<Entities.Query.OriginalInventory>
            {
                new Entities.Query.OriginalInventory { InventoryProductUniqueId = "Inv", InventoryProductId = 12, ProductVolume = 123.33M },
            };

            deltaData.UpdatedInventories = new List<UpdatedInventory>
            {
                new UpdatedInventory { InventoryProductUniqueId = "Inv", InventoryProductId = 12, ProductVolume = 123.33M, EventType = Entities.Registration.EventType.Insert.ToString() },
            };

            var deltaResultMovement = new DeltaResultMovement()
            {
                MovementId = "Mov1",
                MovementTransactionId = 1,
                Sign = Constants.Positive,
                Delta = 10,
            };

            var deltaResultInventory = new DeltaResultInventory()
            {
                InventoryProductUniqueId = "Inv",
                InventoryTransactionId = 1,
                Sign = Constants.Positive,
                Delta = 10,
            };

            var deltaErrorMovement = new DeltaErrorMovement()
            {
                MovementId = "Mov2",
                MovementTransactionId = 3,
                Description = "SomeError",
            };

            var deltaErrorInventory = new DeltaErrorInventory()
            {
                InventoryProductUniqueId = "Inv2",
                InventoryTransactionId = 1,
                Description = "Some Error",
            };

            var deltaResponse = new DeltaResponse
            {
                ResultMovements = new List<DeltaResultMovement> { deltaResultMovement },
                ResultInventories = new List<DeltaResultInventory> { deltaResultInventory },
                ErrorMovements = new List<DeltaErrorMovement> { deltaErrorMovement },
                ErrorInventories = new List<DeltaErrorInventory> { deltaErrorInventory },
            };

            return (deltaData, deltaResponse);
        }
    }
}
