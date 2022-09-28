// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeadletterControllerTests.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Api.Controllers;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Deadletter.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The dead letter controller tests.
    /// </summary>
    [TestClass]
    public class DeadletterControllerTests : ControllerTestBase
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private DeadletterController controller;

        /// <summary>
        /// The mock processor.
        /// </summary>
        private Mock<IDeadletterProcessor> mockProcessor;

        /// <summary>
        /// The mock processor.
        /// </summary>
        private Mock<IReconciler> reconcileService;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockProcessor = new Mock<IDeadletterProcessor>();
            this.reconcileService = new Mock<IReconciler>();
            this.controller = new DeadletterController(this.mockProcessor.Object);
        }

        /// <summary>
        /// Gets all the deadlettred message with status processing.
        /// This method supports ODATA query.
        /// </summary>
        /// <returns>
        /// The deadlettred message.
        /// </returns>
        [TestMethod]
        public async Task GetDeadletteredMessageAsync_ShouldInvokeProcessor_ToReturnDeadlettredMessagesAsync()
        {
            var deadletteredMessages = new[] { new DeadletteredMessage() }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryAllAsync<DeadletteredMessage>(x => x.Status)).ReturnsAsync(deadletteredMessages);

            var result = await this.controller.QueryDeadletteredMessagesAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.AreEqual(result, deadletteredMessages);

            this.mockProcessor.Verify(c => c.QueryAllAsync<DeadletteredMessage>(x => x.Status), Times.Once());
        }

        /// <summary>
        /// The retrigger processo.
        /// </summary>
        /// <returns>
        /// the result.
        /// </returns>
        [TestMethod]
        public async Task ReTriggerAsync_WasSuccessfullAsync()
        {
            IEnumerable<int> deadlettredMessageIds = new List<int>() { 1 };
            this.mockProcessor.Setup(m => m.RetriggerAsync(deadlettredMessageIds)).ReturnsAsync(true);

            var result = await this.controller.ReTriggerAsync(deadlettredMessageIds).ConfigureAwait(false);
            var objectResult = result as EntityResult;
            var returnMessage = objectResult.Messagekey;

            Assert.AreEqual(Entities.Constants.ReTriggerSuccessful, returnMessage);
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.RetriggerAsync(deadlettredMessageIds), Times.Once());
        }

        [TestMethod]
        public async Task GetReconciliationInfoAsync_WasSuccessfulAsync()
        {
            var parameter = new BlockchainFailuresRequest();
            parameter.IsCritical = false;
            parameter.TakeRecords = 50000;
            this.mockProcessor.Setup(m => m.GetFailuresAsync(It.IsAny<BlockchainFailuresRequest>())).ReturnsAsync(new BlockchainFailures());
            await this.controller.GetReconciliationInfoAsync(parameter.IsCritical, parameter.TakeRecords).ConfigureAwait(false);
            this.mockProcessor.Verify(c => c.GetFailuresAsync(It.IsAny<BlockchainFailuresRequest>()), Times.Once());
        }

        [TestMethod]
        public async Task ResetCriticalRecordsAsync_WasSuccessfulAsync()
        {
            this.mockProcessor.Setup(m => m.ResetAsync(It.IsAny<BlockchainFailures>())).Returns(Task.CompletedTask);
            await this.controller.ResetAsync(new BlockchainFailures()).ConfigureAwait(false);
            this.mockProcessor.Verify(m => m.ResetAsync(It.IsAny<BlockchainFailures>()), Times.Once());
        }
    }
}