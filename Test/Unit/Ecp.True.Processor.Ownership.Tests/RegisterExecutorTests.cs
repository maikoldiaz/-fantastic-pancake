// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterExecutorTests.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Executors;
    using Ecp.True.Processors.Ownership.Services.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The RegisterExecutorTests.
    /// </summary>
    [TestClass]
    public class RegisterExecutorTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<RegisterExecutor>> mockLogger;

        /// <summary>
        /// The mock ownership service.
        /// </summary>
        private Mock<IOwnershipService> mockOwnershipService;

        /// <summary>
        /// The ownership rule data.
        /// </summary>
        private OwnershipRuleData ownershipRuleData;

        /// <summary>
        /// The register executor.
        /// </summary>
        private IExecutor registerExecutor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<RegisterExecutor>>();
            this.mockOwnershipService = new Mock<IOwnershipService>();
            this.ownershipRuleData = new OwnershipRuleData() { TicketId = 25281 };
            this.registerExecutor = new RegisterExecutor(this.mockOwnershipService.Object, this.mockLogger.Object);
        }

        /// <summary>
        /// Executes the asynchronous build executor asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExecuteAsync_BuildExecutorAsync()
        {
            this.mockOwnershipService.Setup(x => x.RegisterResultsAsync(It.IsAny<OwnershipRuleData>()));

            await this.registerExecutor.ExecuteAsync(this.ownershipRuleData).ConfigureAwait(false);

            this.mockOwnershipService.Verify(x => x.RegisterResultsAsync(It.IsAny<OwnershipRuleData>()), Times.Once);
        }

        /// <summary>
        /// Registers the executor should return the process type when invoked.
        /// </summary>
        [TestMethod]
        public void RegisterExecutor_ShouldReturnTheProcessType_WhenInvoked()
        {
            Assert.AreEqual(ProcessType.Ownership, this.registerExecutor.ProcessType);
        }

        /// <summary>
        /// Registers the executor should return the execution order when invoked.
        /// </summary>
        [TestMethod]
        public void RegisterExecutor_ShouldReturnTheExecutionOrder_WhenInvoked()
        {
            Assert.AreEqual(9, this.registerExecutor.Order);
        }
    }
}
