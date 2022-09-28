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

namespace Ecp.True.Host.Flow.Api.Tests
{
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Host.Flow.Api.Controllers;
    using Ecp.True.Processors.Approval.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The delta controller tests.
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

        // Initializes this instance.
        // </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockProcessor = new Mock<IDeltaProcessor>();
            this.controller = new DeltaController(this.mockProcessor.Object);
        }

        /// <summary>
        /// Updates the delta node status asynchronous should invoke processor to update delta node status asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task UpdateDeltaApprovalStateAsync_ShouldInvokeProcessor_ToUpdateOwnershipNodeStatusAsync()
        {
            var deltaNodeRequest = new DeltaNodeApprovalRequest { Status = "APPROVED" };
            this.mockProcessor.Setup(m => m.UpdateDeltaApprovalStateAsync(It.IsAny<DeltaNodeApprovalRequest>()));

            var result = await this.controller.UpdateDeltaApprovalStateAsync(deltaNodeRequest).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.UpdateDeltaApprovalStateAsync(deltaNodeRequest), Times.Once());
        }

        /// <summary>
        /// Gets the node delta data by identifier asynchronous should invoke processor to get node delta data by identifier asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task GetDeltaByDeltaNodeIdAsync_ShouldInvokeProcessor_ToGetNodeOwnershipDataByIdAsync()
        {
            var deltaNodeDetails = new DeltaNodeDetails();
            this.mockProcessor.Setup(m => m.GetDeltaByDeltaNodeIdAsync(It.IsAny<int>())).ReturnsAsync(deltaNodeDetails);

            var result = await this.controller.GetDeltaByDeltaNodeIdAsync(1).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.GetDeltaByDeltaNodeIdAsync(1), Times.Once());
        }

        /// <summary>
        /// Generate Delta Movements Asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task GenerateDeltaMovementsAsync_ShouldInvoqueProcessor_GenerateDeltaMovementsAsyncByNodeIdAsync()
        {
            // Arrange

            // Act
            var result = await this.controller.GenerateDeltaMovementsAsync(1).ConfigureAwait(false);

            // Assert
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.GenerateDeltaMovementsAsync(1), Times.Once());
        }
    }
}
