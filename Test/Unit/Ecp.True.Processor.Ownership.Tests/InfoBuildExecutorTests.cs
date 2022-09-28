// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InfoBuildExecutorTests.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Query;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Executors;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Proxies.OwnershipRules.Request;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The InfoBuildExecutorTests.
    /// </summary>
    [TestClass]
    public class InfoBuildExecutorTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<InfoBuildExecutor>> mockLogger;

        /// <summary>
        /// The mock ownership calculation service.
        /// </summary>
        private Mock<IOwnershipCalculationService> mockOwnershipCalculationService;

        /// <summary>
        /// The information build executor.
        /// </summary>
        private IExecutor infoBuildExecutor;

        /// <summary>
        /// The ownership rule data.
        /// </summary>
        private OwnershipRuleData ownershipRuleData;

        /// <summary>
        /// The ownership rule request.
        /// </summary>
        private OwnershipRuleRequest ownershipRuleRequest;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<InfoBuildExecutor>>();
            this.mockOwnershipCalculationService = new Mock<IOwnershipCalculationService>();
            this.infoBuildExecutor = new InfoBuildExecutor(this.mockLogger.Object, this.mockOwnershipCalculationService.Object);
            this.ownershipRuleData = new OwnershipRuleData() { TicketId = 25281 };
            this.ownershipRuleRequest = new OwnershipRuleRequest();
        }

        /// <summary>
        /// Executes the asynchronous should format data when request formed asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExecuteAsync_ShouldFormatData_WhenRequestFormedAsync()
        {
            var movementOperationalData = new MovementOperationalData()
            {
                Ticket = 25281,
                MovementTransactionId = 5452,
                OwnershipUnit = "%",
                MovementTypeId = "43",
            };

            var inventoryOperationalData = new InventoryOperationalData()
            {
                Ticket = 25281,
                InventoryId = 5452,
                OwnershipUnit = "%",
            };

            this.ownershipRuleRequest.MovementsOperationalData = new List<MovementOperationalData> { movementOperationalData };
            this.ownershipRuleRequest.InventoryOperationalData = new List<InventoryOperationalData> { inventoryOperationalData };
            this.ownershipRuleData.OwnershipRuleRequest = this.ownershipRuleRequest;
            this.ownershipRuleData.TransferPointMovements = new List<OwnershipAnalytics>();
            this.mockOwnershipCalculationService.Setup(x => x.PopulateOwnershipRuleRequestDataAsync(this.ownershipRuleData));

            await this.infoBuildExecutor.ExecuteAsync(this.ownershipRuleData).ConfigureAwait(false);

            Assert.AreEqual("PORCENTAJE", movementOperationalData.OwnershipUnit);
            Assert.AreEqual("PORCENTAJE", inventoryOperationalData.OwnershipUnit);
        }

        /// <summary>
        /// Executes the asynchronous should format data when unit is BBL asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExecuteAsync_ShouldFormatData_WhenUnitIsBblAsync()
        {
            var movementOperationalData = new MovementOperationalData()
            {
                Ticket = 25281,
                MovementTransactionId = 5452,
                OwnershipUnit = "Bbl",
            };

            var inventoryOperationalData = new InventoryOperationalData()
            {
                Ticket = 25281,
                InventoryId = 5452,
                OwnershipUnit = "Bbl",
            };

            this.ownershipRuleRequest.MovementsOperationalData = new List<MovementOperationalData> { movementOperationalData };
            this.ownershipRuleRequest.InventoryOperationalData = new List<InventoryOperationalData>() { inventoryOperationalData };
            this.ownershipRuleData.OwnershipRuleRequest = this.ownershipRuleRequest;
            this.ownershipRuleData.TransferPointMovements = new List<OwnershipAnalytics>();
            this.mockOwnershipCalculationService.Setup(x => x.PopulateOwnershipRuleRequestDataAsync(this.ownershipRuleData));

            await this.infoBuildExecutor.ExecuteAsync(this.ownershipRuleData).ConfigureAwait(false);

            Assert.AreEqual("VOLUMEN", movementOperationalData.OwnershipUnit);
            Assert.AreEqual("VOLUMEN", inventoryOperationalData.OwnershipUnit);
        }

        /// <summary>
        /// Informations the build executor should return the process type when invoked.
        /// </summary>
        [TestMethod]
        public void InfoBuildExecutor_ShouldReturnTheProcessType_WhenInvoked()
        {
            Assert.AreEqual(ProcessType.Ownership, this.infoBuildExecutor.ProcessType);
        }

        /// <summary>
        /// Informations the build executor should return the execution order when invoked.
        /// </summary>
        [TestMethod]
        public void InfoBuildExecutor_ShouldReturnTheExecutionOrder_WhenInvoked()
        {
            Assert.AreEqual(2, this.infoBuildExecutor.Order);
        }
    }
}
