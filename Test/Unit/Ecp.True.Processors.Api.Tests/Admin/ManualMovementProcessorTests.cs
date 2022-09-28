// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManualMovementProcessorTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Tests.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Builders;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Interfaces;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Api.Admin;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    ///     The movementProcessorTests test class.
    /// </summary>
    [TestClass]
    public class ManualMovementProcessorTests
    {
        /// <summary>
        ///     The Movement Repository mock.
        /// </summary>
        private Mock<IRepository<Movement>> movementRepositoryMock;

        /// <summary>
        ///     The movement processor.
        /// </summary>
        private ManualManualMovementProcessor processor;

        /// <summary>
        /// The finalizer mock.
        /// </summary>
        private Mock<IFinalizer> finalizerMock;

        /// <summary>
        /// The logger mock.
        /// </summary>
        private Mock<ITrueLogger<ManualManualMovementProcessor>> loggerMock;

        private Mock<IUnitOfWorkFactory> unitOfWorkFactoryMock;
        private Mock<IUnitOfWork> unitOfWorkMock;
        private Mock<IRepository<DeltaNode>> deltaNodeRepositoryMock;
        private Mock<IFinalizerFactory> finalizerFactory;
        private DeltaNode deltaNode = new DeltaNode();
        private IEnumerable<IExecutor> executors;
        private Mock<IInfoBuildExecutor> infoExecutor;

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.InitializeUnitOfWorkFactory();

            this.InitializeFinalizerFactory();

            this.InitializeExecutors();

            this.processor = new ManualManualMovementProcessor(
                this.unitOfWorkFactoryMock.Object,
                this.finalizerFactory.Object,
                this.infoExecutor.Object,
                this.executors,
                this.loggerMock.Object);
        }

        /// <summary>
        ///     ShouldInvokeRepositoryAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ShouldInvokeRepositoryAsync()
        {
            // Prepare
            this.movementRepositoryMock.Setup(m => m.QueryAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>()));
            this.InitializeTestDeltaNode();

            // Execute
            await this.processor.GetAssignableMovementsAsync(1, DateTime.Now, DateTime.Now).ConfigureAwait(false);

            // Assert
            this.unitOfWorkFactoryMock.Verify(f => f.GetUnitOfWork(), Times.Once);
            this.unitOfWorkMock.Verify(f => f.CreateRepository<Movement>(), Times.Once);
            this.movementRepositoryMock.Verify(f => f.QueryAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>()), Times.Once);
        }

        /// <summary>
        /// ShouldQueryManualMovementsAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ShouldQueryManualMovementsAsync()
        {
            // Prepare
            var movements = GetTestManualMovements();
            this.InitializeTestDeltaNode();

            var movementIds = this.InitializeMovementRepository(movements);

            // Execute
            await this.processor.UpdateTicketManualMovementsAsync(this.deltaNode.DeltaNodeId, movementIds)
                .ConfigureAwait(false);

            // Assert
            this.movementRepositoryMock.Verify(r => r.UpdateAll(It.IsAny<IEnumerable<Movement>>()), Times.Once);
            this.movementRepositoryMock.Verify(r => r.QueryAllAsync(It.IsAny<Expression<Func<Movement,bool>>>(), It.IsAny<string[]>()), Times.Once);
        }

        /// <summary>
        /// ShouldQueryDeltaNodeManualMovementsAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ShouldQueryDeltaNodeManualMovementsAsync()
        {
            // Prepare
            this.InitializeTestDeltaNode();
            var movements = GetTestManualMovements();
            var movementIds = this.InitializeMovementRepository(movements);

            // Execute
            await this.processor.UpdateTicketManualMovementsAsync(this.deltaNode.DeltaNodeId, movementIds)
                    .ConfigureAwait(false);

            // Assert
            this.deltaNodeRepositoryMock
                .Verify(
                    r => r.FirstOrDefaultAsync(
                        It.IsAny<Expression<Func<DeltaNode, bool>>>(), It.IsAny<string[]>()),
                    Times.Once);
        }

        /// <summary>
        /// ShouldQueryDeltaNodeManualMovementsAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ShouldThrowException_WhenDeltaNodeInApprovalOrSentStateAsync()
        {
            // Prepare
            var movements = GetTestManualMovements();
            var movementIds = this.InitializeMovementRepository(movements);

            // Execute
            var exception = await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
                    async () => await
                        this.processor.UpdateTicketManualMovementsAsync(this.deltaNode.DeltaNodeId, movementIds)
                            .ConfigureAwait(false))
                .ConfigureAwait(false);

            // Assert
            this.deltaNodeRepositoryMock
                .Verify(
                    r => r.FirstOrDefaultAsync(
                        It.IsAny<Expression<Func<DeltaNode, bool>>>(), It.IsAny<string[]>()),
                    Times.Once);
            Assert.AreEqual(Constants.DeltaNodeNotFound, exception.Message);
        }

        private static List<Movement> GetTestManualMovements()
        {
            var builder = new MovementBuilder();
            return new List<Movement>
            {
                builder.WithTransactionId(1),
                builder.WithTransactionId(2),
                builder.WithTransactionId(3),
            };
        }

        private void InitializeExecutors()
        {
            this.infoExecutor = new Mock<IInfoBuildExecutor>();
            var officialDeltaExecutor = new Mock<IExecutor>();
            officialDeltaExecutor
                .Setup(e => e.Order).Returns(6);
            officialDeltaExecutor
                .Setup(e => e.ProcessType).Returns(ProcessType.OfficialDelta);

            this.executors = new List<IExecutor>
            {
                officialDeltaExecutor.Object,
            };
        }

        private int[] InitializeMovementRepository(List<Movement> movements)
        {
            var movementIds = movements.Select(m => m.MovementTransactionId).ToArray();

            this.movementRepositoryMock
                .Setup(m => m.QueryAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(
                    movements.AsQueryable());
            return movementIds;
        }

        private void InitializeTestDeltaNode(OwnershipNodeStatusType? status = null)
        {
            this.deltaNode = new DeltaNode
            {
                TicketId = 1,
                Ticket = new Ticket()
                {
                    StartDate = MovementBuilder.StartTime,
                    EndDate = MovementBuilder.EndTime,
                },
                Status = status.GetValueOrDefault(),
            };

            this.deltaNodeRepositoryMock
                .Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<DeltaNode, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(this.deltaNode);

            this.deltaNodeRepositoryMock
                .Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<DeltaNode, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(this.deltaNode);
        }

        private void InitializeFinalizerFactory()
        {
            this.finalizerMock = new Mock<IFinalizer>();
            this.loggerMock = new Mock<ITrueLogger<ManualManualMovementProcessor>>();

            this.finalizerFactory = new Mock<IFinalizerFactory>();
            this.finalizerFactory.Setup(f => f.GetFinalizer(TicketType.OfficialDelta))
                .Returns(this.finalizerMock.Object);
        }

        private void InitializeUnitOfWorkFactory()
        {
            this.movementRepositoryMock = new Mock<IRepository<Movement>>();
            this.deltaNodeRepositoryMock = new Mock<IRepository<DeltaNode>>();

            this.unitOfWorkMock = new Mock<IUnitOfWork>();
            this.unitOfWorkMock.Setup(u => u.CreateRepository<Movement>())
                .Returns(this.movementRepositoryMock.Object);

            this.unitOfWorkMock.Setup(u => u.CreateRepository<DeltaNode>())
                .Returns(this.deltaNodeRepositoryMock.Object);

            this.unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
            this.unitOfWorkFactoryMock.Setup(u => u.GetUnitOfWork())
                .Returns(this.unitOfWorkMock.Object);
        }
    }
}