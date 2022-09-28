// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipControllerTests.cs" company="Microsoft">
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
    using Ecp.True.Host.Api.Controllers;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// The node controller tests.
    /// </summary>
    [TestClass]
    public class OwnershipControllerTests : ControllerTestBase
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private OwnershipController controller;

        /// <summary>
        /// The mock processor.
        /// </summary>
        private Mock<IOwnershipProcessor> ownershipProcessorMock;

        /// <summary>
        /// The mock queueProcessor.
        /// </summary>
        private Mock<IQueueProcessor> queueProcessor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.ownershipProcessorMock = new Mock<IOwnershipProcessor>();
            this.queueProcessor = new Mock<IQueueProcessor>();
            this.controller = new OwnershipController(this.ownershipProcessorMock.Object, this.queueProcessor.Object);
        }

        /// <summary>
        /// Queries the systemOwnershipCalculations returns systemOwnershipCalculations when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task QueryTickets_ReturnsSystemOwnershipCalculation_WhenInvokedAsync()
        {
            var systemOwnershipCalculations = new[] { new SystemOwnershipCalculation() }.AsQueryable();
            this.ownershipProcessorMock.Setup(m => m.QueryAllAsync<SystemOwnershipCalculation>(null)).ReturnsAsync(systemOwnershipCalculations);

            var result = await this.controller.QuerySystemOwnershipCalculationAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.AreEqual(result, systemOwnershipCalculations);

            this.ownershipProcessorMock.Verify(c => c.QueryAllAsync<SystemOwnershipCalculation>(null), Times.Once());
        }

        /// <summary>
        /// Gets the sap tracking errors asynchronous returns sap tracking errors when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task RecalculateOwnershipBalanc_WhenInvokedAsync()
        {
            this.queueProcessor.Setup(m => m.PushQueueMessageAsync(It.IsAny<int>(), It.IsAny<string>()));

            var result = await this.controller.RecalculateOwnershipAsync(It.IsAny<int>()).ConfigureAwait(false);

            // Assert or verify
            this.queueProcessor.Verify(c => c.PushQueueMessageAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once());
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }
    }
}
