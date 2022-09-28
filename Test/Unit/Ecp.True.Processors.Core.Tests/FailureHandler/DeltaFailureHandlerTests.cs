// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaFailureHandlerTests.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.HandleFailure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class DeltaFailureHandlerTests
    {
        /// <summary>
        /// The failure handler mock.
        /// </summary>
        private readonly Mock<ITelemetry> telemetryMock = new Mock<ITelemetry>();

        /// <summary>
        /// The logger mock.
        /// </summary>
        private readonly Mock<ITrueLogger<DeltaFailureHandler>> loggerMock = new Mock<ITrueLogger<DeltaFailureHandler>>();

        /// <summary>
        /// The delta failure handler.
        /// </summary>
        private DeltaFailureHandler deltaFailureHandler;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock ticket repository.
        /// </summary>
        private Mock<IRepository<Ticket>> mockTicketRepository;

        /// <summary>
        /// Initilizes this instance.
        /// </summary>
        [TestInitialize]
        public void Initilize()
        {
            this.mockTicketRepository = new Mock<IRepository<Ticket>>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.deltaFailureHandler = new DeltaFailureHandler(this.loggerMock.Object, this.telemetryMock.Object);
        }

        /// <summary>
        /// Types the should return ticket type cut off when invoked.
        /// </summary>
        [TestMethod]
        public void Type_ShouldReturnTicketTypeDelta_WhenInvoked()
        {
            var result = this.deltaFailureHandler.TicketType;

            Assert.AreEqual(TicketType.Delta, result);
        }

        /// <summary>
        /// Gets the ownership failure hanler.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Delta_HandleFailureAsync()
        {
            this.mockUnitOfWork.Setup(a => a.CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);
            this.mockTicketRepository.Setup(a => a.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(new Ticket { TicketId = 1 });
            await this.deltaFailureHandler.HandleFailureAsync(this.mockUnitOfWork.Object, new FailureInfo(1, string.Empty)).ConfigureAwait(false);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<Ticket>(), Times.Once);
            this.mockTicketRepository.Verify(a => a.GetByIdAsync(It.IsAny<object>()), Times.Once);
        }
    }
}
