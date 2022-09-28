// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CutOffFailureHandlerTests.cs" company="Microsoft">
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
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.HandleFailure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// CutOffFailureHandler tests.
    /// </summary>
    [TestClass]
    public class CutOffFailureHandlerTests
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
        private readonly Mock<ITrueLogger<CutOffFailureHandler>> loggerMock = new Mock<ITrueLogger<CutOffFailureHandler>>();

        /// <summary>
        /// The cut off failure handler.
        /// </summary>
        private CutOffFailureHandler cutOffFailureHandler;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock ticket repository.
        /// </summary>
        private Mock<IRepository<Unbalance>> mockUnbalanceRepository;

        /// <summary>
        /// Initilizes this instance.
        /// </summary>
        [TestInitialize]
        public void Initilize()
        {
            this.mockUnbalanceRepository = new Mock<IRepository<Unbalance>>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockUnitOfWork.Setup(a => a.CreateRepository<Unbalance>()).Returns(this.mockUnbalanceRepository.Object);
            this.serviceProviderMock.Setup(s => s.GetService(typeof(ITrueLogger<CutOffFailureHandler>))).Returns(this.loggerMock.Object);
            this.cutOffFailureHandler = new CutOffFailureHandler(this.loggerMock.Object, this.telemetryMock.Object);
        }

        /// <summary>
        /// Types the should return ticket type cut off when invoked.
        /// </summary>
        [TestMethod]
        public void Type_ShouldReturnTicketTypeCutOff_WhenInvoked()
        {
            var result = this.cutOffFailureHandler.TicketType;

            Assert.AreEqual(TicketType.Cutoff, result);
        }

        /// <summary>
        /// Gets the ownership failure hanler.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CutOff_HandleFailureAsync()
        {
            await this.cutOffFailureHandler.HandleFailureAsync(this.mockUnitOfWork.Object, new FailureInfo(1, string.Empty)).ConfigureAwait(false);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<Unbalance>(), Times.Exactly(1));
            this.mockUnbalanceRepository.Verify(a => a.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }
    }
}
