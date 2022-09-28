// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueueProcessorTest.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Tests.Admin
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Api.Admin;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The queue processor tests.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Api.Tests.ProcessorTestBase" />
    [TestClass]
    public class QueueProcessorTest : ProcessorTestBase
    {
        /// <summary>
        /// The QueueProcessor.
        /// </summary>
        private QueueProcessor queueProcessor;

        /// <summary>
        /// The mock cut Off processor.
        /// </summary>
        private Mock<IQueueProcessor> queuProcessorMock;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<ITrueLogger<TicketProcessor>> logger;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private Mock<IAzureClientFactory> azureClientFactory;

        /// <summary>
        /// The mock queue client.
        /// </summary>
        private Mock<IServiceBusQueueClient> mockQueueClient;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockQueueClient = new Mock<IServiceBusQueueClient>();
            this.queuProcessorMock = new Mock<IQueueProcessor>();
            this.logger = new Mock<ITrueLogger<TicketProcessor>>();
            this.azureClientFactory = new Mock<IAzureClientFactory>();
            this.queueProcessor = new QueueProcessor(this.logger.Object, this.azureClientFactory.Object);
        }

        /// <summary>
        /// Push Queue Session Message.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task PushQueueSessionMessage_ShouldCallQueueClient_WhenInvokedAsync()
        {
            // Arrange
            var serviceBusQueueClientMock = new Mock<IServiceBusQueueClient>();
            this.queuProcessorMock.Setup(m => m.PushQueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()));
            this.azureClientFactory.Setup(y => y.GetQueueClient(It.IsAny<string>())).Returns(serviceBusQueueClientMock.Object);

            // Act
            await this.queueProcessor.PushQueueSessionMessageAsync(1, QueueConstants.RecalculateOperationalCutoffBalanceQueue).ConfigureAwait(false);

            // Assert or verify
            this.azureClientFactory.Verify(c => c.GetQueueClient(It.IsAny<string>()), Times.Once());
        }

        /// <summary>
        /// Push Queue Session Message Error.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task PushQueueSessionMessage_ShouldNotSendQueueMessage_WhenAzureClientFactoryBeNullAsync()
        {
            // Arrange
            this.mockQueueClient.Setup(m => m.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()));
            this.queuProcessorMock.Setup(m => m.PushQueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()));
            this.azureClientFactory.Setup(y => y.GetQueueClient(It.IsAny<string>()));

            // Act
            await this.queueProcessor.PushQueueSessionMessageAsync(1, QueueConstants.RecalculateOperationalCutoffBalanceQueue).ConfigureAwait(false);

            // Assert
            this.mockQueueClient.Verify(c => c.QueueSessionMessageAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        /// <summary>
        /// Should Write Log Error When Azure ClientFactory Be Null.
        /// </summary>
        /// <returns>Write Log Error.</returns>
        [TestMethod]
        public async Task PushQueueSessionMessage_ShouldWriteLogError_WhenAzureClientFactoryBeNullAsync()
        {
            // Arrange
            this.logger.Setup(m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()));
            this.queuProcessorMock.Setup(m => m.PushQueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()));
            this.azureClientFactory.Setup(y => y.GetQueueClient(It.IsAny<string>()));

            // Act
            await this.queueProcessor.PushQueueSessionMessageAsync(1, QueueConstants.RecalculateOperationalCutoffBalanceQueue).ConfigureAwait(false);

            // Assert
            this.logger.Verify(
                m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()),
                Times.Once);
        }

        /// <summary>
        /// Push Queue Session Message.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task PushQueueMessage_ShouldCallQueueClient_WhenInvokedAsync()
        {
            // Arrange
            var serviceBusQueueClientMock = new Mock<IServiceBusQueueClient>();
            this.queuProcessorMock.Setup(m => m.PushQueueMessageAsync(It.IsAny<int>(), It.IsAny<string>()));
            this.azureClientFactory.Setup(y => y.GetQueueClient(It.IsAny<string>())).Returns(serviceBusQueueClientMock.Object);

            // Act
            await this.queueProcessor.PushQueueMessageAsync(1, QueueConstants.RecalculateOperationalCutoffBalanceQueue).ConfigureAwait(false);

            // Assert or verify
            this.azureClientFactory.Verify(c => c.GetQueueClient(It.IsAny<string>()), Times.Once());
        }

        /// <summary>
        /// Push Queue Session Message Error.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task PushQueueMessage_ShouldNotSendQueueMessage_WhenAzureClientFactoryBeNullAsync()
        {
            // Arrange
            this.mockQueueClient.Setup(m => m.QueueMessageAsync(It.IsAny<int>(), It.IsAny<string>()));
            this.queuProcessorMock.Setup(m => m.PushQueueMessageAsync(It.IsAny<int>(), It.IsAny<string>()));
            this.azureClientFactory.Setup(y => y.GetQueueClient(It.IsAny<string>()));

            // Act
            await this.queueProcessor.PushQueueMessageAsync(1, QueueConstants.RecalculateOperationalCutoffBalanceQueue).ConfigureAwait(false);

            // Assert
            this.mockQueueClient.Verify(c => c.QueueMessageAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        /// <summary>
        /// Should Write Log Error When AzureClientFactory Be Null.
        /// </summary>
        /// <returns>WriteLogError.</returns>
        [TestMethod]
        public async Task PushQueueSMessage_ShouldWriteLogError_WhenAzureClientFactoryBeNullAsync()
        {
            // Arrange
            this.logger.Setup(m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()));
            this.queuProcessorMock.Setup(m => m.PushQueueMessageAsync(It.IsAny<int>(), It.IsAny<string>()));
            this.azureClientFactory.Setup(y => y.GetQueueClient(It.IsAny<string>()));

            // Act
            await this.queueProcessor.PushQueueMessageAsync(1, QueueConstants.RecalculateOperationalCutoffBalanceQueue).ConfigureAwait(false);

            // Assert
            this.logger.Verify(
                m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()),
                Times.Once);
        }
    }
}
