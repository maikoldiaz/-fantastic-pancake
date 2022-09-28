// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialBuildResultExecutorTests.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Processors.Delta.Interfaces;
    using Ecp.True.Processors.Delta.OfficialDeltaExecutors;
    using Ecp.True.Proxies.OwnershipRules.OfficialDeltaDataRequest;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The OfficialBuildResultExecutorTests.
    /// </summary>
    [TestClass]
    public class OfficialBuildResultExecutorTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<BuildResultExecutor>> mockLogger;

        /// <summary>
        /// The result build executor.
        /// </summary>
        private IExecutor buildResultExecutor;

        /// <summary>
        /// The delta data.
        /// </summary>
        private OfficialDeltaData deltaData;

        /// <summary>
        /// The delta request.
        /// </summary>
        private OfficialDeltaRequest deltaRequest;

        /// <summary>
        /// The mock repository factory.
        /// </summary>
        private Mock<ICompositeOfficialDeltaBuilder> compositeOfficialDeltaBuilder;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.compositeOfficialDeltaBuilder = new Mock<ICompositeOfficialDeltaBuilder>();
            this.mockLogger = new Mock<ITrueLogger<BuildResultExecutor>>();
            this.buildResultExecutor = new BuildResultExecutor(this.mockLogger.Object, this.compositeOfficialDeltaBuilder.Object);
            this.deltaData = new OfficialDeltaData() { Ticket = new Ticket { TicketId = 123 } };
            this.deltaRequest = new OfficialDeltaRequest();
        }

        /// <summary>
        /// Informations the build executor should return order.
        /// </summary>
        [TestMethod]
        public void BuildResultExecutor_ShouldReturnOrder()
        {
            Assert.AreEqual(4, this.buildResultExecutor.Order);
        }

        /// <summary>
        /// Executes the asynchronous should format data when request formed asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExecuteAsync_ShouldFormatData_WhenRequestFormedAsync()
        {
            var movements = new List<Movement> { new Movement { MovementDestination = new MovementDestination { DestinationNodeId = 6 }, MovementSource = new MovementSource { SourceNodeId = 5 } } };
            this.compositeOfficialDeltaBuilder.Setup(a => a.BuildMovementsAsync(It.IsAny<OfficialDeltaData>())).ReturnsAsync(movements);

            await this.buildResultExecutor.ExecuteAsync(this.deltaData).ConfigureAwait(false);
            this.compositeOfficialDeltaBuilder.Verify(a => a.BuildMovementsAsync(It.IsAny<OfficialDeltaData>()), Times.Exactly(1));
            Assert.IsNotNull(this.deltaData.GeneratedMovements);
            Assert.IsTrue(this.deltaData.GeneratedMovements.Any());
        }
    }
}
