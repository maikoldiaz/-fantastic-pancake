// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaControllerTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Tests.Admin
{
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Api.Controllers;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Delta.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The delta file controller tests.
    /// </summary>
    [TestClass]
    public class DeltaControllerTests
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private DeltaController controller;

        /// <summary>
        /// The mock processor.
        /// </summary>
        private Mock<IDeltaProcessor> mockProcessor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockProcessor = new Mock<IDeltaProcessor>();
            this.controller = new DeltaController(this.mockProcessor.Object);
        }

        /// <summary>
        /// Queries the delta nodes asynchronous should invoke processor to return nodes asynchronous.
        /// </summary>
        /// <returns>The tasks.</returns>
        [TestMethod]
        public async Task QueryDeltaNodesAsync_ShouldInvokeProcessor_ToReturnNodesAsync()
        {
            var nodeInfos = new[] { new DeltaNodeInfo() }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryViewAsync<DeltaNodeInfo>()).ReturnsAsync(nodeInfos);

            var result = await this.controller.QueryDeltaNodesAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.AreEqual(result, nodeInfos);

            this.mockProcessor.Verify(c => c.QueryViewAsync<DeltaNodeInfo>(), Times.Once());
        }

        /// <summary>
        /// Gets the official delta period asynchronous should return period list when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetOfficialDeltaPeriodAsync_ShouldReturnPeriodList_WhenInvokedAsync()
        {
            int segmentid = 10;
            int years = 5;
            this.mockProcessor.Setup(m => m.GetOfficialDeltaPeriodAsync(segmentid, years, true));
            var result = await this.controller.GetOfficialDeltaPeriodAsync(segmentid, years, true).ConfigureAwait(false);
            var entityResult = result as EntityResult;

            // Assert
            Assert.IsInstanceOfType(entityResult, typeof(EntityResult));
            Assert.IsNotNull(entityResult);
            this.mockProcessor.Verify(c => c.GetOfficialDeltaPeriodAsync(segmentid, years, true), Times.Once());
        }

        /// <summary>
        /// Validates the previous official period asynchronous should return period list when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ValidatePreviousOfficialPeriodAsync_ShouldReturnPeriodList_WhenInvokedAsync()
        {
            var ticket = new Ticket();
            this.mockProcessor.Setup(m => m.ValidatePreviousOfficialPeriodAsync(ticket));
            var result = await this.controller.ValidatePreviousPeriodNodesAsync(ticket).ConfigureAwait(false);
            var entityResult = result as EntityResult;

            // Assert
            Assert.IsInstanceOfType(entityResult, typeof(EntityResult));
            Assert.IsNotNull(entityResult);
            this.mockProcessor.Verify(c => c.ValidatePreviousOfficialPeriodAsync(ticket), Times.Once());
        }

        /// <summary>
        /// Gets the official delta ticket processing status asynchronous should invoke processor to return success asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetOfficialDeltaTicketProcessingStatusAsync_ShouldInvokeProcessor_ToReturnSuccessAsync()
        {
            this.mockProcessor.Setup(m => m.GetOfficialDeltaTicketProcessingStatusAsync(It.IsAny<int>()));
            var result = await this.controller.GetOfficialDeltaTicketProcessingStatusAsync(It.IsAny<int>()).ConfigureAwait(false);
            var entityResult = result as EntityResult;

            // Assert
            Assert.IsInstanceOfType(entityResult, typeof(EntityResult));
            Assert.IsNotNull(entityResult);
            this.mockProcessor.Verify(c => c.GetOfficialDeltaTicketProcessingStatusAsync(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task GetUnapprovedOfficialNodesAsync_ToReturnUnapprovedOfficialNodesAsync()
        {
            this.mockProcessor.Setup(m => m.GetUnapprovedOfficialNodesAsync(It.IsAny<Ticket>()));
            var result = await this.controller.GetUnapprovedOfficialNodesAsync(It.IsAny<Ticket>()).ConfigureAwait(false);
            var entityResult = result as EntityResult;

            // Assert
            Assert.IsInstanceOfType(entityResult, typeof(EntityResult));
            Assert.IsNotNull(entityResult);
            this.mockProcessor.Verify(c => c.GetUnapprovedOfficialNodesAsync(It.IsAny<Ticket>()), Times.Once());
        }
    }
}
