// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildMovementExecutorTests.cs" company="Microsoft">
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
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Executors;
    using Ecp.True.Processors.Ownership.Services.Interfaces;
    using Ecp.True.Proxies.OwnershipRules.Request;
    using Ecp.True.Proxies.OwnershipRules.Response;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The BuildMovementExecutorTests.
    /// </summary>
    [TestClass]
    public class BuildMovementExecutorTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<BuildMovementExecutor>> mockLogger;

        /// <summary>
        /// The mock ownership service.
        /// </summary>
        private Mock<IOwnershipService> mockOwnershipService;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockunitOfWork;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockunitOfWorkFactory;

        /// <summary>
        /// The ownership rule data.
        /// </summary>
        private OwnershipRuleData ownershipRuleData;

        /// <summary>
        /// The ownership rule request.
        /// </summary>
        private OwnershipRuleRequest ownershipRuleRequest;

        /// <summary>
        /// The build movement executor.
        /// </summary>
        private IExecutor buildMovementExecutor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<BuildMovementExecutor>>();
            this.mockOwnershipService = new Mock<IOwnershipService>();
            this.ownershipRuleData = new OwnershipRuleData() { TicketId = 25281 };
            this.ownershipRuleRequest = new OwnershipRuleRequest();
            this.mockunitOfWork = new Mock<IUnitOfWork>();
            this.mockunitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.buildMovementExecutor = new BuildMovementExecutor(this.mockLogger.Object, this.mockOwnershipService.Object, this.mockunitOfWorkFactory.Object);
        }

        /// <summary>
        /// Executes the asynchronous should return movements asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExecuteAsync_ShouldReturnMovementsAsync()
        {
            var movement = new Movement
            {
                TicketId = 25281,
            };

            var commercialMovementsResult = new CommercialMovementsResult
            {
                MovementId = 100,
                MovementType = "COMPRA",
                RuleVersion = "5",
            };

            var newMovement = new NewMovement
            {
                AgreementType = "COLABORACION",
                RuleVersion = "5",
            };

            this.ownershipRuleData.OwnershipRuleResponse = new OwnershipRuleResponse
            {
                CommercialMovementsResults = new List<CommercialMovementsResult> { commercialMovementsResult },
                NewMovements = new List<NewMovement> { newMovement },
            };

            this.mockOwnershipService.Setup(x => x.BuildOwnershipMovementResultsAsync(It.IsAny<List<CommercialMovementsResult>>(), It.IsAny<List<NewMovement>>(), It.IsAny<List<CancellationMovementDetail>>(), 25281, this.mockunitOfWork.Object)).ReturnsAsync(new List<Movement> { movement });

            await this.buildMovementExecutor.ExecuteAsync(this.ownershipRuleData).ConfigureAwait(false);

            Assert.IsNotNull(this.ownershipRuleData.Movements);
            this.mockOwnershipService.Verify(x => x.BuildOwnershipMovementResultsAsync(It.IsAny<List<CommercialMovementsResult>>(), It.IsAny<List<NewMovement>>(), It.IsAny<List<CancellationMovementDetail>>(), 25281, this.mockunitOfWork.Object), Times.Never);
        }

        /// <summary>
        /// Builds the movement executor should return the process type when invoked.
        /// </summary>
        [TestMethod]
        public void BuildMovementExecutor_ShouldReturnTheProcessType_WhenInvoked()
        {
            Assert.AreEqual(ProcessType.Ownership, this.buildMovementExecutor.ProcessType);
        }

        /// <summary>
        /// Builds the movement executor should return the execution order when invoked.
        /// </summary>
        [TestMethod]
        public void BuildMovementExecutor_ShouldReturnTheExecutionOrder_WhenInvoked()
        {
            Assert.AreEqual(8, this.buildMovementExecutor.Order);
        }
    }
}
