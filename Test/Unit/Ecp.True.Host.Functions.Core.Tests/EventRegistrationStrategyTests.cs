// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventRegistrationStrategyTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Core.Tests
{
    using System;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Registration;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The EventRegistrationStrategyTests.
    /// </summary>
    [TestClass]
    public class EventRegistrationStrategyTests
    {
        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<EventRegistrationStrategy>> mockLogger;

        /// <summary>
        /// The configuration handler mock.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigurationHandler;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock event repository.
        /// </summary>
        private Mock<IRepository<Event>> mockEventRepository;

        /// <summary>
        /// The event registration strategy.
        /// </summary>
        private EventRegistrationStrategy eventRegistrationStrategy;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<EventRegistrationStrategy>>();
            this.mockConfigurationHandler = new Mock<IConfigurationHandler>();
            this.mockEventRepository = new Mock<IRepository<Event>>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();

            this.eventRegistrationStrategy =
                new EventRegistrationStrategy(
                    this.mockLogger.Object,
                    this.mockAzureClientFactory.Object);
        }

        /// <summary>
        /// Events the registration strategy insert should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task EventRegistrationStrategy_Insert_Should_RegisterAsync()
        {
            var eventObject = this.GetEvent("Insert");

            this.mockUnitOfWork.Setup(a => a.CreateRepository<Event>()).Returns(this.mockEventRepository.Object);
            this.mockEventRepository.Setup(a => a.Insert(It.IsAny<Event>()));
            this.mockUnitOfWork.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));

            await this.eventRegistrationStrategy.RegisterAsync(eventObject, this.mockUnitOfWork.Object).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(a => a.CreateRepository<Event>(), Times.Once);
            this.mockEventRepository.Verify(a => a.Insert(It.IsAny<Event>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Events the registration strategy update should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task EventRegistrationStrategy_Update_Should_RegisterAsync()
        {
            var eventObject = this.GetEvent("Update");

            this.mockUnitOfWork.Setup(a => a.CreateRepository<Event>()).Returns(this.mockEventRepository.Object);
            this.mockEventRepository.Setup(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<Event, bool>>>())).ReturnsAsync(eventObject);
            this.mockEventRepository.Setup(a => a.Update(It.IsAny<Event>()));
            this.mockUnitOfWork.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));

            await this.eventRegistrationStrategy.RegisterAsync(eventObject, this.mockUnitOfWork.Object).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(a => a.CreateRepository<Event>(), Times.Once);
            this.mockEventRepository.Verify(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<Event, bool>>>()), Times.Once);
            this.mockEventRepository.Verify(a => a.Update(It.IsAny<Event>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Contracts the registration strategy delete should register asynchronous.
        /// </summary>'<returns>The task.</returns>
        [TestMethod]
        public async Task ContractRegistrationStrategy_Delete_Should_RegisterAsync()
        {
            var contract = this.GetEvent("Delete");

            this.mockUnitOfWork.Setup(a => a.CreateRepository<Event>()).Returns(this.mockEventRepository.Object);
            this.mockEventRepository.Setup(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<Event, bool>>>())).ReturnsAsync(contract);
            this.mockEventRepository.Setup(a => a.Update(It.IsAny<Event>()));
            this.mockUnitOfWork.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));

            await this.eventRegistrationStrategy.RegisterAsync(contract, this.mockUnitOfWork.Object).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(a => a.CreateRepository<Event>(), Times.Once);
            this.mockEventRepository.Verify(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<Event, bool>>>()), Times.Once);
            this.mockEventRepository.Verify(a => a.Update(It.IsAny<Event>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Gets the event.
        /// </summary>
        /// <param name="actionType">Type of the action.</param>
        /// <returns>The event.</returns>
        private Event GetEvent(string actionType)
        {
            var eventObject = new Event
            {
                ActionType = actionType,
            };

            return eventObject;
        }
    }
}
