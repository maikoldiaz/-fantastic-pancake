// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CalculateExecutorTests.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Executors;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The CalculateExecutorTests.
    /// </summary>
    [TestClass]
    public class CalculateExecutorTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<CalculateExecutor>> mockLogger;

        /// <summary>
        /// The mock ownership service.
        /// </summary>
        private Mock<IOwnershipProcessor> mockOwnershipProcessor;

        /// <summary>
        /// The ownership rule data.
        /// </summary>
        private OwnershipRuleData ownershipRuleData;

        /// <summary>
        /// The complete executor.
        /// </summary>
        private IExecutor completeExecutor;

        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<CalculateExecutor>>();
            this.mockOwnershipProcessor = new Mock<IOwnershipProcessor>();
            this.ownershipRuleData = new OwnershipRuleData() { TicketId = 25281 };
            this.completeExecutor = new CalculateExecutor(this.mockOwnershipProcessor.Object, this.mockLogger.Object);
        }

        [TestMethod]
        public async Task ExecuteAsync_BuildExecutorAsync()
        {
            var response = new Tuple<IEnumerable<OwnershipCalculation>, IEnumerable<SegmentOwnershipCalculation>, IEnumerable<SystemOwnershipCalculation>>(new List<OwnershipCalculation>(), new List<SegmentOwnershipCalculation>(), new List<SystemOwnershipCalculation>());
            this.mockOwnershipProcessor.Setup(x => x.CalculateOwnershipAsync(It.IsAny<int>())).ReturnsAsync(response);

            await this.completeExecutor.ExecuteAsync(this.ownershipRuleData).ConfigureAwait(false);

            this.mockOwnershipProcessor.Verify(x => x.CalculateOwnershipAsync(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void CompleteExecutor_ShouldReturnTheProcessType_WhenInvoked()
        {
            Assert.AreEqual(ProcessType.Ownership, this.completeExecutor.ProcessType);
        }

        [TestMethod]
        public void CompleteExecutor_ShouldReturnTheExecutionOrder_WhenInvoked()
        {
            Assert.AreEqual(10, this.completeExecutor.Order);
        }
    }
}
