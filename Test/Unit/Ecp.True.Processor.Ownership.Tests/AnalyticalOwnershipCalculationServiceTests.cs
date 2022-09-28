// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnalyticalOwnershipCalculationServiceTests.cs" company="Microsoft">
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
    using Ecp.True.Logging;
    using Ecp.True.Processors.Ownership;
    using Ecp.True.Processors.Ownership.Calculation.Request;
    using Ecp.True.Processors.Ownership.Calculation.Response;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The Analytical Ownership Calculation Service Tests class.
    /// </summary>
    [TestClass]
    public class AnalyticalOwnershipCalculationServiceTests
    {
        /// <summary>
        /// The mock repository factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockRepositoryFactory;

        /// <summary>
        /// The mock communicator.
        /// </summary>
        private Mock<IAnalyticsClient> mockAnalyticsClient;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<AnalyticalOwnershipCalculationService>> mockLogger;

        /// <summary>
        /// The mock ticket repository.
        /// </summary>
        private Mock<IRepository<TransferPointMovement>> mockTransferPointMovementRepository;

        /// <summary>
        /// The analytical ownership calculation service.
        /// </summary>
        private AnalyticalOwnershipCalculationService analyticalOwnershipCalculationService;

        /// <summary>
        /// Intilizes the method.
        /// </summary>
        [TestInitialize]
        public void IntilizeMethod()
        {
            this.mockRepositoryFactory = new Mock<IRepositoryFactory>();
            this.mockAnalyticsClient = new Mock<IAnalyticsClient>();
            this.mockLogger = new Mock<ITrueLogger<AnalyticalOwnershipCalculationService>>();
            this.mockTransferPointMovementRepository = new Mock<IRepository<TransferPointMovement>>();

            this.analyticalOwnershipCalculationService = new AnalyticalOwnershipCalculationService(this.mockRepositoryFactory.Object, this.mockAnalyticsClient.Object, this.mockLogger.Object);
        }

        /// <summary>
        /// Gets transfer point movements asynchronous.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task GetTransferPointMovementsAsync_TestAsync()
        {
            this.mockRepositoryFactory.Setup(s => s.CreateRepository<TransferPointMovement>()).Returns(this.mockTransferPointMovementRepository.Object);
            this.mockTransferPointMovementRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()));
            var result = await this.analyticalOwnershipCalculationService.GetTransferPointMovementsAsync(10).ConfigureAwait(false);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Gets ownership analytical data asynchronous.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task GetOwnershipAnalyticalDataAsync_TestAsync()
        {
            var transferPointMovements = new List<TransferPointMovement>
            { new TransferPointMovement { AlgorithmId = 10, TicketId = 20, MovementId = "30", MovementType = "Test" } };
            this.mockAnalyticsClient.Setup(s => s.GetOwnershipAnalyticsAsync(It.IsAny<AnalyticalServiceRequestData>())).ReturnsAsync(new List<AnalyticalServiceResponseData>());

            var result = await this.analyticalOwnershipCalculationService.GetOwnershipAnalyticalDataAsync(transferPointMovements).ConfigureAwait(false);
            Assert.IsNotNull(result);
        }
    }
}
