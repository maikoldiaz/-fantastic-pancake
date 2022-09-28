// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConciliationControllerTest.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Api.Controllers;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Conciliation.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// The node controller tests.
    /// </summary>
    [TestClass]
    public class ConciliationControllerTest : ControllerTestBase
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private ConciliationController controller;

        /// <summary>
        /// The mock processor.
        /// </summary>
        private Mock<IConciliationProcessor> ownershipConciliationMock;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.ownershipConciliationMock = new Mock<IConciliationProcessor>();
            this.controller = new ConciliationController(this.ownershipConciliationMock.Object);
        }

        /// <summary>
        /// Initialize conciliation.
        /// </summary>
        /// <returns>The Mask.</returns>
        [TestMethod]
        public async Task InitializeConciliationAsync_ShouldInvokeProcessor_ExecuteManualConciliationOwnershipAsync()
        {
            var conciliationRequest = new ConciliationNodesResquest();
            this.ownershipConciliationMock.Setup(m => m.InitializeConciliationAsync(conciliationRequest));
            var result = await this.controller.InitializeConciliationAsync(new ConciliationNodesResquest()).ConfigureAwait(false);
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.ownershipConciliationMock.Verify(c => c.InitializeConciliationAsync(It.IsAny<ConciliationNodesResquest>()), Times.Once());
        }
    }
}
