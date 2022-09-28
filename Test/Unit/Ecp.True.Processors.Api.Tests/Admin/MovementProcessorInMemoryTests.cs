// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementProcessorInMemoryTests.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Builders;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Interfaces;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Api.Admin;
    using Ecp.True.Processors.Api.Specifications;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Constants = Ecp.True.Core.Constants;

    /// <summary>
    /// The movemenProcessorInMemoryTests test class.
    /// </summary>
    [TestClass]
    public class MovementProcessorInMemoryTests
    {
        /// <summary>
        ///     The Movement Repository mock.
        /// </summary>
        private Mock<IRepository<Movement>> movementRepositoryMock;

        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> unitOfWorkFactoryMock;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private Mock<IUnitOfWork> unitOfWorkMock;

        /// <summary>
        /// The deltaNode repository.
        /// </summary>
        private Mock<IRepository<DeltaNode>> deltaNodeRepositoryMock;

        /// <summary>
        /// The finalizer mock.
        /// </summary>
        private Mock<IFinalizer> finalizerMock;

        /// <summary>
        /// The mock finalizer factory.
        /// </summary>
        private Mock<IFinalizerFactory> finalizerFactory;

        /// <summary>
        ///     The movement processor.
        /// </summary>
        private ManualManualMovementProcessor processor;

        /// <summary>
        /// The in-memory repository.
        /// </summary>
        private IRepository<Movement> movementRepository;

        /// <summary>
        /// The in-memory repository.
        /// </summary>
        private SqlDataContext dataContext;

        /// <summary>
        /// The official manual movements specification.
        /// </summary>
        private OfficialManualMovementsSpecification spec;

        /// <summary>
        /// The logger mock.
        /// </summary>
        private Mock<ITrueLogger<ManualManualMovementProcessor>> loggerMock;

        private List<IExecutor> executors;
        private Mock<IInfoBuildExecutor> infoExecutor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns>The task.</returns>
        [TestInitialize]
        public async Task InitializeAsync()
        {
            this.spec = new OfficialManualMovementsSpecification(
                MovementBuilder.SourceNodeId,
                MovementBuilder.StartTime,
                MovementBuilder.EndTime);

            this.InitializeFinalizerFactory();

            this.InitializeUnitOfWork();

            this.InitializeExecutors();

            var movements = GetMovementListWith4ValidManualMovements(); // A total of 4 valid ✅ manual movements or inventories.

            this.movementRepository.InsertAll(movements);

            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);

            this.processor = new ManualManualMovementProcessor(
                this.unitOfWorkFactoryMock.Object,
                this.finalizerFactory.Object,
                this.infoExecutor.Object,
                this.executors,
                this.loggerMock.Object);
        }

        /// <summary>
        /// ShouldHaveItems.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ShouldHaveOnlyValidItemsAsync()
        {
            // Prepare
            var query = await this.movementRepository.GetAllAsync(m => true).ConfigureAwait(false);
            Trace.WriteLine($"Total movements: {query.Count()}");

            Assert.IsNotNull(query.ToList());

            // Execute
            var movements = await this.processor.GetAssignableMovementsAsync(
                    MovementBuilder.SourceNodeId,
                    MovementBuilder.StartTime,
                    MovementBuilder.EndTime)
                .ConfigureAwait(false);

            // Assert
            Assert.IsInstanceOfType(movements, typeof(IQueryable<Movement>));
            var count = movements.Count();
            Assert.AreEqual(4, count);
        }

        /// <summary>
        /// Gets movement list with 4 valid manual movements.
        /// </summary>
        /// <returns>The list.</returns>
        private static List<Movement> GetMovementListWith4ValidManualMovements()
        {
            return new List<Movement>()
            {
                new MovementBuilder()
                    .WithManualSourceSystem(),           // ✅
                new MovementBuilder()
                    .WithManualSourceSystem()
                    .IsDeleted(),
                new MovementBuilder()
                    .WithManualSourceSystem()
                    .IsOperative(),
                new MovementBuilder()
                    .WithDefaultValuesForInventory(),    // ✅
                new MovementBuilder()
                    .WithManualSourceSystem()
                    .WithStartTime(MovementBuilder.StartTime.AddDays(-1)),
                new MovementBuilder()
                    .WithManualSourceSystem(),           // ✅
                new MovementBuilder()
                    .WithManualSourceSystem(),           // ✅
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

        /// <summary>
        /// Initializes unit of work.
        /// </summary>
        private void InitializeUnitOfWork()
        {
            var movementRepoFactory = new InMemoryRepositoryFactory<Movement>();

            (this.movementRepository, this.dataContext, _) = movementRepoFactory.GetRepository();

            this.movementRepositoryMock = new Mock<IRepository<Movement>>();
            this.deltaNodeRepositoryMock = new Mock<IRepository<DeltaNode>>();

            this.unitOfWorkMock = new Mock<IUnitOfWork>();
            this.unitOfWorkMock.Setup(u => u.CreateRepository<Movement>())
                .Returns(this.movementRepository);

            this.unitOfWorkMock.Setup(u => u.CreateRepository<DeltaNode>())
                .Returns(this.deltaNodeRepositoryMock.Object);

            this.unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
            this.unitOfWorkFactoryMock.Setup(u => u.GetUnitOfWork())
                .Returns(this.unitOfWorkMock.Object);
        }

        /// <summary>
        /// Initializes finalizer factory.
        /// </summary>
        private void InitializeFinalizerFactory()
        {
            this.finalizerMock = new Mock<IFinalizer>();
            this.loggerMock = new Mock<ITrueLogger<ManualManualMovementProcessor>>();

            this.finalizerFactory = new Mock<IFinalizerFactory>();
            this.finalizerFactory.Setup(f => f.GetFinalizer(TicketType.OfficialDelta))
                .Returns(this.finalizerMock.Object);
        }
    }
}