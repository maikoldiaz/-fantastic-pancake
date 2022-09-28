// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompleteExecutorTests.cs" company="Microsoft">
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
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Executors;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class CompleteExecutorTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<CompleteExecutor>> mockLogger;

        /// <summary>
        /// The mock ownership service.
        /// </summary>
        private Mock<IOwnershipProcessor> mockOwnershipProcessor;

        private OwnershipRuleData ownershipRuleData;

        private IExecutor buildExecutor;

        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<CompleteExecutor>>();
            this.mockOwnershipProcessor = new Mock<IOwnershipProcessor>();
            this.ownershipRuleData = new OwnershipRuleData() { TicketId = 25281 };
            this.buildExecutor = new CompleteExecutor(this.mockOwnershipProcessor.Object, this.mockLogger.Object);
        }

        [TestMethod]
        public async Task ExecuteAsync_BuildExecutorAsync()
        {
            this.mockOwnershipProcessor.Setup(x => x.CompleteAsync(It.IsAny<OwnershipRuleData>()));

            await this.buildExecutor.ExecuteAsync(this.ownershipRuleData).ConfigureAwait(false);

            this.mockOwnershipProcessor.Verify(x => x.CompleteAsync(It.IsAny<OwnershipRuleData>()), Times.Once);
        }
    }
}
