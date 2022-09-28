// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadSendLogisticMovementTest.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Sap.Tests.FactoryUpload
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Sap.Interfaces;
    using Ecp.True.Processors.Sap.Services;
    using Ecp.True.Processors.Sap.Services.FactoryUpload;
    using Ecp.True.Proxies.Sap.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class UploadSendLogisticMovementTest
    {
        /// <summary>
        /// The upload status contract.
        /// </summary>
        private SendLogisticMovement sendLogisticMovement;

        /// <summary>
        /// The sap tracking.
        /// </summary>
        private Mock<ISapTrackingProcessor> sapTrackingMock;

        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private Mock<IUnitOfWork> unitOfWorkMock;

        /// <summary>
        /// The sap proxy.
        /// </summary>
        private Mock<ISapProxy> sapProxyMock;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<ITrueLogger<SapProcessor>> loggerMock;

        /// <summary>
        /// The sap repository.
        /// </summary>
        private Mock<IRepository<LogisticMovement>> sapRepository;

        /// <summary>
        /// The ticket repository.
        /// </summary>
        private Mock<IRepository<Ticket>> ticketRepository;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private Mock<ITelemetry> telemetryMock;

        /// <summary>
        /// The file registration transaction service mock.
        /// </summary>
        private Mock<IFileRegistrationTransactionService> fileRegistrationTransactionServiceMock;

        [TestInitialize]
        public void Initialize()
        {
            this.unitOfWorkMock = new Mock<IUnitOfWork>();
            this.sapProxyMock = new Mock<ISapProxy>();
            this.sapTrackingMock = new Mock<ISapTrackingProcessor>();
            this.loggerMock = new Mock<ITrueLogger<SapProcessor>>();
            this.sapTrackingMock = new Mock<ISapTrackingProcessor>();
            this.sapRepository = new Mock<IRepository<LogisticMovement>>();
            this.ticketRepository = new Mock<IRepository<Ticket>>();
            this.fileRegistrationTransactionServiceMock = new Mock<IFileRegistrationTransactionService>();
            this.telemetryMock = new Mock<ITelemetry>();
            this.sendLogisticMovement = new SendLogisticMovement(
                this.unitOfWorkMock.Object,
                this.sapProxyMock.Object,
                this.sapTrackingMock.Object,
                this.telemetryMock.Object,
                this.loggerMock.Object,
                this.fileRegistrationTransactionServiceMock.Object);
        }

        /// <summary>
        /// Send Data to SAP Test.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task SendLogisticMovementTest_SendUploadProcessorSapAsync_ShouldSendToSapAsync()
        {
            LogisticMovement logisticMovement = new LogisticMovement() { LogisticMovementId = 1, StatusProcessId = StatusType.PROCESSED, MessageProcess = "prueba", SapSentDate = null };
            SapLogisticRequest sapLogisticRequest = new SapLogisticRequest
            {
                Movement = new SapLogisticRequestDto
                {
                    Movement = new SapLogisticDto
                    {
                        MovementId = "1",
                        Period = new SapLogisticPeriod
                        {
                            StartTime = DateTime.Now.ToShortDateString(),
                        },
                    },
                },
            };

            Ticket ticket = new Ticket
            {
                TicketId = 1,
            };

            this.sapTrackingMock.Setup(x => x.CreateBlobAsync(It.IsAny<string>(), It.IsAny<object>()));
            this.unitOfWorkMock.Setup(x => x.CreateRepository<LogisticMovement>()).Returns(this.sapRepository.Object);
            this.fileRegistrationTransactionServiceMock.Setup(x =>
                x.InsertFileRegistrationAsync(It.IsAny<FileRegistration>()));
            this.unitOfWorkMock.Setup(x => x.CreateRepository<Ticket>()).Returns(this.ticketRepository.Object);

            this.ticketRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ticket);

            await this.sendLogisticMovement.SendUploadProcessorSapAsync(sapLogisticRequest, logisticMovement).ConfigureAwait(false);
            this.sapTrackingMock.Verify(x => x.CreateBlobAsync(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
            this.sapRepository.Verify(x => x.Update(It.IsAny<LogisticMovement>()), Times.Once);
            this.fileRegistrationTransactionServiceMock.Verify(x => x.InsertFileRegistrationAsync(It.IsAny<FileRegistration>()), Times.Once);
        }
    }
}
