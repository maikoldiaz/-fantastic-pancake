// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaFinalizerTests.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Deltas;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class DeltaFinalizerTests
    {
        /// <summary>
        /// The factory.
        /// </summary>
        private readonly Mock<IUnitOfWorkFactory> mockFactory = new Mock<IUnitOfWorkFactory>();

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private readonly Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();

        /// <summary>
        /// The mock movement repository.
        /// </summary>
        private readonly Mock<IRepository<Movement>> mockMovementRepository = new Mock<IRepository<Movement>>();

        /// <summary>
        /// The client.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The mock queue client.
        /// </summary>
        private readonly Mock<IServiceBusQueueClient> mockQueueClient = new Mock<IServiceBusQueueClient>();

        /// <summary>
        /// The mock configuration handler.
        /// </summary>
        private readonly Mock<IConfigurationHandler> mockConfigurationHandler = new Mock<IConfigurationHandler>();

        /// <summary>
        /// The mock analysis service client.
        /// </summary>
        private readonly Mock<IAnalysisServiceClient> mockAnalysisServiceClient = new Mock<IAnalysisServiceClient>();

        /// <summary>
        /// The ownership finalizer.
        /// </summary>
        private DeltaFinalizer deltaFinalizer;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockConfigurationHandler.Setup(m => m.GetConfigurationAsync<ServiceBusSettings>(It.IsAny<string>())).ReturnsAsync(new ServiceBusSettings { ConnectionString = "UTString" });
            this.mockQueueClient.Setup(m => m.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()));
            this.mockAzureClientFactory.Setup(m => m.GetQueueClient(It.IsAny<string>())).Returns(this.mockQueueClient.Object);
            this.mockAnalysisServiceClient.Setup(m => m.RefreshCalculationAsync(1));
            this.mockAzureClientFactory.Setup(m => m.AnalysisServiceClient).Returns(this.mockAnalysisServiceClient.Object);
            this.mockFactory.Setup(m => m.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockMovementRepository.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Movement> { new Movement { MovementTransactionId = 1 } });
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);
            this.deltaFinalizer = new DeltaFinalizer(this.mockAzureClientFactory.Object, this.mockFactory.Object);
        }

        /// <summary>
        /// Types the should return ticket type delta when invoked.
        /// </summary>
        [TestMethod]
        public void Type_ShouldReturnTicketTypeDelta_WhenInvoked()
        {
            var result = this.deltaFinalizer.Type;
            Assert.AreEqual(TicketType.Delta, result);
        }

        [TestMethod]
        public async Task ProcessAsync_ShouldSendMessagesToQueues_WhenInvokedAsync()
        {
            // Act
            await this.deltaFinalizer.ProcessAsync(1).ConfigureAwait(false);

            // Assert
            this.mockQueueClient.Verify(m => m.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Exactly(1));
            this.mockAzureClientFactory.Verify(m => m.GetQueueClient(It.IsAny<string>()), Times.Exactly(1));
            this.mockMovementRepository.Verify(m => m.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Movement>(), Times.Once);
        }
    }
}
