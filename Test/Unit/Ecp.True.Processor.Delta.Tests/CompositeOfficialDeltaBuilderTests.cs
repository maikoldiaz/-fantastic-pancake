// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompositeOfficialDeltaBuilderTests.cs" company="Microsoft">
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
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Delta.Builders;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Processors.Delta.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The CompositeOfficialDeltaBuilderTests.
    /// </summary>
    [TestClass]
    public class CompositeOfficialDeltaBuilderTests
    {
        /// <summary>
        /// The failure handler mock.
        /// </summary>
        private readonly Mock<IOfficialDeltaBuilder> officialDeltaBuilderMock = new Mock<IOfficialDeltaBuilder>();

        /// <summary>
        /// The failure handler mock.
        /// </summary>
        private readonly Mock<IOfficialDeltaBuilder> officialDeltaBuilderExecutionMock = new Mock<IOfficialDeltaBuilder>();

        /// <summary>
        /// The failure handler factory.
        /// </summary>
        private CompositeOfficialDeltaBuilder compositeOfficialDeltaBuilder;

        /// <summary>
        /// The delta data.
        /// </summary>
        private OfficialDeltaData deltaData;

        /// <summary>
        /// Initilizes this instance.
        /// </summary>
        [TestInitialize]
        public void Initilize()
        {
            this.deltaData = new OfficialDeltaData() { Ticket = new Ticket { TicketId = 123 } };
            var deltaNodeError = new List<DeltaNodeError> { new DeltaNodeError { DeltaNodeId = 1 } };
            var movements = new List<Movement> { new Movement { MovementId = "test" } };
            this.officialDeltaBuilderMock.Setup(x => x.BuildErrorsAsync(this.deltaData)).ReturnsAsync(deltaNodeError);
            this.officialDeltaBuilderExecutionMock.Setup(x => x.BuildMovementsAsync(this.deltaData)).ReturnsAsync(movements);
            var executionManagerFactoryList = new List<IOfficialDeltaBuilder>() { this.officialDeltaBuilderMock.Object, this.officialDeltaBuilderExecutionMock.Object };
            this.compositeOfficialDeltaBuilder = new CompositeOfficialDeltaBuilder(executionManagerFactoryList);
        }

        /// <summary>
        /// Gets the ownership failure hanler.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BuildErrors_GetBuildErrorsListOnSuccessAsync()
        {
            var buildErrorsResult = await this.compositeOfficialDeltaBuilder.BuildErrorsAsync(this.deltaData).ConfigureAwait(false);

            Assert.IsNotNull(buildErrorsResult);
            Assert.AreEqual(1, buildErrorsResult.FirstOrDefault().DeltaNodeId);
        }

        /// <summary>
        /// Builds the movements get build movements list on success asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BuildMovements_GetBuildMovementsListOnSuccessAsync()
        {
            var buildErrorMovements = await this.compositeOfficialDeltaBuilder.BuildMovementsAsync(this.deltaData).ConfigureAwait(false);

            Assert.IsNotNull(buildErrorMovements);
            Assert.AreEqual("test", buildErrorMovements.FirstOrDefault().MovementId);
        }
    }
}
