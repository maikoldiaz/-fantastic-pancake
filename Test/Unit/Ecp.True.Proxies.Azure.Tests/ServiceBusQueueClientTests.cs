// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceBusQueueClientTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.Azure.Tests
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Chaos.Interfaces;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The azure client factory tests.
    /// </summary>
    [TestClass]
    public class ServiceBusQueueClientTests
    {
        /// <summary>
        /// The Queue client.
        /// </summary>
        private Mock<IQueueClient> mockQueueClient;

        /// <summary>
        /// The mock chaos manager.
        /// </summary>
        private Mock<IChaosManager> mockChaosManager;

        /// <summary>
        /// The mock dead letter manager.
        /// </summary>
        private Mock<IDeadLetterManager> mockDeadLetterManager;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private IServiceBusQueueClient serviceBusQueueClient;

        /// <summary>
        /// Initilizes this instance.
        /// </summary>
        [TestInitialize]
        public void Initilize()
        {
            this.mockQueueClient = new Mock<IQueueClient>();
            this.mockChaosManager = new Mock<IChaosManager>();
            this.mockDeadLetterManager = new Mock<IDeadLetterManager>();

            this.mockChaosManager.Setup(m => m.HasChaos).Returns(true);
            this.mockChaosManager.Setup(m => m.ChaosValue).Returns("Label");
            this.serviceBusQueueClient = new ServiceBusQueueClient(this.mockQueueClient.Object, this.mockChaosManager.Object, this.mockDeadLetterManager.Object);
        }

        /// <summary>
        /// Queue message async should call send on queue client when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task QueueMessageAsync_ShouldCallSendOnQueueClient_WhenInvokedAsync()
        {
            this.mockQueueClient.Setup(x => x.SendAsync(It.IsAny<Message>()));

            await this.serviceBusQueueClient.QueueMessageAsync(DateTime.UtcNow).ConfigureAwait(false);

            this.mockQueueClient.Verify(x => x.SendAsync(It.Is<Message>(m => string.IsNullOrWhiteSpace(m.MessageId) && m.Label == "Label" && m.Body != null)), Times.Once);
            this.mockChaosManager.VerifyAll();
        }

        /// <summary>
        /// Queue message async should call send on queue client when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task QueueMessageAsync_WithMessageId_ShouldCallSendOnQueueClient_WhenInvokedAsync()
        {
            this.mockQueueClient.Setup(x => x.SendAsync(It.IsAny<Message>()));

            await this.serviceBusQueueClient.QueueMessageAsync(DateTime.UtcNow, "testmessageid").ConfigureAwait(false);

            this.mockQueueClient.Verify(x => x.SendAsync(It.Is<Message>(m => m.MessageId == "testmessageid" && m.Label == "Label" && m.Body != null)), Times.Once);
            this.mockChaosManager.VerifyAll();
        }

        /// <summary>
        /// Queue message async should call send on queue client when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task QueueSessionMessageAsync_ShouldCallSendOnQueueClientWithSessionId_WhenInvokedAsync()
        {
            this.mockQueueClient.Setup(x => x.SendAsync(It.IsAny<Message>()));

            await this.serviceBusQueueClient.QueueSessionMessageAsync(DateTime.UtcNow, "sessionid").ConfigureAwait(false);

            this.mockQueueClient.Verify(x => x.SendAsync(It.Is<Message>(m => string.IsNullOrWhiteSpace(m.MessageId) && m.SessionId == "sessionid" && m.Label == "Label" && m.Body != null)), Times.Once);
            this.mockChaosManager.VerifyAll();
        }

        /// <summary>
        /// Queue message async should call send on queue client when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task QueueSessionMessageAsync_ShouldCallSendOnQueueClient_WhenInvokedAsync()
        {
            this.mockQueueClient.Setup(x => x.SendAsync(It.IsAny<Message>()));

            await this.serviceBusQueueClient.QueueSessionMessageAsync("test", "sessionid").ConfigureAwait(false);

            this.mockQueueClient.Verify(x => x.SendAsync(It.Is<Message>(m => string.IsNullOrWhiteSpace(m.MessageId) && m.SessionId == "sessionid" && m.Label == "Label" && m.Body != null)), Times.Once);
            this.mockChaosManager.VerifyAll();
        }

        /// <summary>
        /// Queues the schedule message asynchronous should call send on queue client when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task QueueScheduleMessageAsync_ShouldCallSendOnQueueClient_WhenInvokedAsync()
        {
            this.mockQueueClient.Setup(x => x.SendAsync(It.IsAny<Message>()));
            this.mockDeadLetterManager.Setup(m => m.IsDeadLettered).Returns(true);
            await this.serviceBusQueueClient.QueueScheduleMessageAsync("test", "sessionid", 10).ConfigureAwait(false);

            this.mockQueueClient.Verify(x => x.SendAsync(It.Is<Message>(m => string.IsNullOrWhiteSpace(m.MessageId) && m.Label == "Label" && m.Body != null)), Times.Once);
            this.mockChaosManager.VerifyAll();
        }
    }
}
