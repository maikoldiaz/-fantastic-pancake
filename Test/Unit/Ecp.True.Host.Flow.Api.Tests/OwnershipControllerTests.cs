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
    /// The category controller tests.
    /// </summary>
    [TestClass]
    public class OwnershipControllerTests
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private OwnershipController controller;

        /// <summary>
        /// The mock processor.
        /// </summary>
        private Mock<IApprovalProcessor> mockProcessor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockProcessor = new Mock<IApprovalProcessor>();
            this.controller = new OwnershipController(this.mockProcessor.Object);
        }

        /// <summary>
        /// Updates the ownership node status asynchronous should invoke processor to update ownership node status asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task UpdateOwnershipNodeStatusAsync_ShouldInvokeProcessor_ToUpdateOwnershipNodeStatusAsync()
        {
            var nodeOwnershipApprovalRequest = new NodeOwnershipApprovalRequest { Status = "APPROVED" };
            this.mockProcessor.Setup(m => m.UpdateOwnershipNodeStatusAsync(It.IsAny<NodeOwnershipApprovalRequest>()));

            var result = await this.controller.UpdateNodeOwnershipAsync(nodeOwnershipApprovalRequest).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.UpdateOwnershipNodeStateAsync(nodeOwnershipApprovalRequest), Times.Once());
        }

        /// <summary>
        /// Gets the node ownership data by identifier asynchronous should invoke processor to get node ownership data by identifier asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task GetNodeOwnershipDataByIdAsync_ShouldInvokeProcessor_ToGetNodeOwnershipDataByIdAsync()
        {
            var ownershipNodeBalanceDetails = new OwnershipNodeBalanceDetails();
            this.mockProcessor.Setup(m => m.GetOwnershipNodeBalanceDetailsAsync(It.IsAny<int>())).ReturnsAsync(ownershipNodeBalanceDetails);

            var result = await this.controller.GetNodeOwnershipByIdAsync(1).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.GetOwnershipNodeBalanceDetailsAsync(1), Times.Once());
        }
    }
}
