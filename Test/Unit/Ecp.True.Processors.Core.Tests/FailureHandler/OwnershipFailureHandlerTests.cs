// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipFailureHandlerTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Tests.FailureHandler
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.HandleFailure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// OwnershipFailureHandler tests.
    /// </summary>
    [TestClass]
    public class OwnershipFailureHandlerTests
    {
        /// <summary>
        /// The failure handler mock.
        /// </summary>
        private readonly Mock<ITelemetry> telemetryMock = new Mock<ITelemetry>();

        /// <summary>
        /// The service provider mock.
        /// </summary>
        private readonly Mock<IServiceProvider> serviceProviderMock = new Mock<IServiceProvider>();

        /// <summary>
        /// The logger mock.
        /// </summary>
        private readonly Mock<ITrueLogger<OwnershipFailureHandler>> loggerMock = new Mock<ITrueLogger<OwnershipFailureHandler>>();

        /// <summary>
        /// The ownership failure handler.
        /// </summary>
        private OwnershipFailureHandler ownershipFailureHandler;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock ownership repository.
        /// </summary>
        private Mock<IRepository<Ownership>> mockOwnershipRepository;

        /// <summary>
        /// The mock ownership repository.
        /// </summary>
        private Mock<IRepository<Ticket>> mockTicketRepository;

        /// <summary>
        /// Initilizes this instance.
        /// </summary>
        [TestInitialize]
        public void Initilize()
        {
            this.mockOwnershipRepository = new Mock<IRepository<Ownership>>();
            this.mockTicketRepository = new Mock<IRepository<Ticket>>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockUnitOfWork.Setup(a => a.CreateRepository<Ownership>()).Returns(this.mockOwnershipRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);
            this.mockOwnershipRepository.Setup(a => a.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()));
            this.mockTicketRepository.Setup(a => a.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Ticket { StartDate = DateTime.Today });
            this.serviceProviderMock.Setup(s => s.GetService(typeof(ITrueLogger<OwnershipFailureHandler>))).Returns(this.loggerMock.Object);
            this.ownershipFailureHandler = new OwnershipFailureHandler(this.loggerMock.Object, this.telemetryMock.Object);
        }

        /// <summary>
        /// Types the should return ticket type ownership when invoked.
        /// </summary>
        [TestMethod]
        public void Type_ShouldReturnTicketTypeOwnership_WhenInvoked()
        {
            var result = this.ownershipFailureHandler.TicketType;

            Assert.AreEqual(TicketType.Ownership, result);
        }

        /// <summary>
        /// Gets the ownership failure hanler.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Ownership__HandleFailureAsync()
        {
            await this.ownershipFailureHandler.HandleFailureAsync(this.mockUnitOfWork.Object, new FailureInfo(1, string.Empty)).ConfigureAwait(false);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<Ownership>(), Times.Exactly(1));
            this.mockOwnershipRepository.Verify(a => a.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }
    }
}
