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

namespace Ecp.True.Processors.Core.Tests.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Registration;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The event registration strategy tests.
    /// </summary>
    [TestClass]
    public class EventRegistrationStrategyTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private readonly Mock<ITrueLogger<EventRegistrationStrategy>> mockLogger = new Mock<ITrueLogger<EventRegistrationStrategy>>();

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private readonly Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();

        /// <summary>
        /// The mock contract repository.
        /// </summary>
        private readonly Mock<IRepository<Event>> mockEventRepository = new Mock<IRepository<Event>>();

        /// <summary>
        /// The mock contract.
        /// </summary>
        private readonly Mock<Event> mockEvent = new Mock<Event>();

        /// <summary>
        /// The contract registration strategy.
        /// </summary>
        private EventRegistrationStrategy eventRegistrationStrategy;

        /// <summary>
        /// Initilizes this instance.
        /// </summary>
        [TestInitialize]
        public void Initilize()
        {
            this.mockEvent.Setup(m => m.CopyFrom(It.IsAny<IEntity>()));
            this.mockEventRepository.Setup(m => m.Insert(It.IsAny<Event>()));
            this.mockEventRepository.Setup(m => m.Update(It.IsAny<Event>()));
            this.mockEventRepository.Setup(m => m.Delete(It.IsAny<Event>()));
            this.mockEventRepository.Setup(m => m.SingleOrDefaultAsync(It.IsAny<Expression<Func<Event, bool>>>())).ReturnsAsync(this.mockEvent.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Event>()).Returns(this.mockEventRepository.Object);
            this.eventRegistrationStrategy = new EventRegistrationStrategy(this.mockLogger.Object, this.mockAzureClientFactory.Object);
        }

        /// <summary>
        /// Registers the asynchronous should throw argument exception when entity is null asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task RegisterAsync_ShouldThrowArgumentException_WhenEntityIsNullAsync()
        {
            await this.eventRegistrationStrategy.RegisterAsync(null, this.mockUnitOfWork.Object).ConfigureAwait(false);
        }

        /// <summary>
        /// Registers the asynchronous should throw argument exception when unit of work is null asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task RegisterAsync_ShouldThrowArgumentException_WhenUnitOfWorkIsNullAsync()
        {
            await this.eventRegistrationStrategy.RegisterAsync(It.IsAny<object>(), null).ConfigureAwait(false);
        }

        /// <summary>
        /// Registers the asynchronous should register entity when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task RegisterAsync_ShouldInsertEventObject_WhenActionTypeInsertAsync()
        {
            // Arrange
            var entity = new Event { ActionType = "Insert" };

            // Act
            await this.eventRegistrationStrategy.RegisterAsync(entity, this.mockUnitOfWork.Object).ConfigureAwait(false);

            // Assert
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Event>(), Times.Exactly(1));
            this.mockEventRepository.Verify(m => m.Insert(It.IsAny<Event>()), Times.Exactly(1));
            this.mockEventRepository.Verify(m => m.Update(It.IsAny<Event>()), Times.Never);
            this.mockEventRepository.Verify(m => m.Delete(It.IsAny<Event>()), Times.Never);
            this.mockUnitOfWork.Verify(m => m.SaveAsync(CancellationToken.None), Times.Exactly(1));
        }

        /// <summary>
        /// Registers the asynchronous should update entity when action type update asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task RegisterAsync_ShouldUpdateEventObject_WhenActionTypeUpdateAsync()
        {
            // Arrange
            var entity = new Event { ActionType = "Update" };

            // Act
            await this.eventRegistrationStrategy.RegisterAsync(entity, this.mockUnitOfWork.Object).ConfigureAwait(false);

            // Assert
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Event>(), Times.Exactly(1));
            this.mockEventRepository.Verify(m => m.Insert(It.IsAny<Event>()), Times.Never);
            this.mockEventRepository.Verify(m => m.Update(It.IsAny<Event>()), Times.Exactly(1));
            this.mockEventRepository.Verify(m => m.Delete(It.IsAny<Event>()), Times.Never);
            this.mockEventRepository.Verify(m => m.SingleOrDefaultAsync(It.IsAny<Expression<Func<Event, bool>>>()), Times.Exactly(1));
            this.mockEvent.Verify(m => m.CopyFrom(It.IsAny<IEntity>()), Times.Exactly(1));
            this.mockUnitOfWork.Verify(m => m.SaveAsync(CancellationToken.None), Times.Exactly(1));
        }

        /// <summary>
        /// Registers the asynchronous should delete entity when action type delete asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task RegisterAsync_ShouldDeleteEventObject_WhenActionTypeDeleteAsync()
        {
            // Arrange
            var entity = new Event { ActionType = "Delete" };

            // Act
            await this.eventRegistrationStrategy.RegisterAsync(entity, this.mockUnitOfWork.Object).ConfigureAwait(false);

            // Assert
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Event>(), Times.Exactly(1));
            this.mockEventRepository.Verify(m => m.Insert(It.IsAny<Event>()), Times.Never);
            this.mockEventRepository.Verify(m => m.Update(It.IsAny<Event>()), Times.Exactly(1));
            this.mockEventRepository.Verify(m => m.Delete(It.IsAny<Event>()), Times.Never);
            this.mockEventRepository.Verify(m => m.SingleOrDefaultAsync(It.IsAny<Expression<Func<Event, bool>>>()), Times.Exactly(1));
            this.mockEvent.Verify(m => m.CopyFrom(It.IsAny<IEntity>()), Times.Never);
            this.mockUnitOfWork.Verify(m => m.SaveAsync(CancellationToken.None), Times.Exactly(1));
        }

        /// <summary>
        /// Inserts the should log information message when invoke.
        /// </summary>
        [TestMethod]
        public void Insert_ShouldLogInformationMessage_WhenInvoke()
        {
            // Arrange
            var entity = new Event { ActionType = "Delete" };

            // Act
            this.eventRegistrationStrategy.Insert(new List<Event> { entity }, this.mockUnitOfWork.Object);

            // Assert
            this.mockLogger.Verify(a => a.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }
    }
}
