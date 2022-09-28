// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaExecutionManagerTests.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Processors.Delta.OfficialDeltaExecutors;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The ExecutionManagerTests.
    /// </summary>
    [TestClass]
    public class OfficialDeltaExecutionManagerTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<OfficialDeltaExecutionManager>> mockLogger;

        /// <summary>
        /// The execution manager.
        /// </summary>
        private IExecutionManager executionManager;

        /// <summary>
        /// The mock executor.
        /// </summary>
        private Mock<IExecutor> mockExecutor;

        /// <summary>
        /// The delta data.
        /// </summary>
        private OfficialDeltaData deltaData;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<OfficialDeltaExecutionManager>>();
            this.mockExecutor = new Mock<IExecutor>();
            this.deltaData = new OfficialDeltaData
            {
                Ticket = new Ticket { TicketId = 123 },
            };
            this.executionManager = new OfficialDeltaExecutionManager(this.mockLogger.Object);
            this.executionManager.Initialize(this.mockExecutor.Object);
        }

        /// <summary>
        /// Executes the asynchronous build executor asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExecuteAsync_BuildExecutorAsync()
        {
            this.mockExecutor.Setup(x => x.ExecuteAsync(It.IsAny<object>()));

            await this.executionManager.ExecuteChainAsync(this.deltaData).ConfigureAwait(false);

            this.mockExecutor.Verify(x => x.ExecuteAsync(It.IsAny<object>()), Times.Once);
        }
    }
}
