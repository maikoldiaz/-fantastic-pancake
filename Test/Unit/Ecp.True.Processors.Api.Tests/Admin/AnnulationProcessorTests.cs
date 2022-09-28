// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnnulationProcessorTests.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Processors.Api.Admin;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The reversal processor tests.
    /// </summary>
    [TestClass]
    public class AnnulationProcessorTests : ProcessorTestBase
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private AnnulationProcessor processor;

        /// <summary>
        /// The token.
        /// </summary>
        private CancellationToken token;

        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockFactory;

        /// <summary>The unit of work mock factory.</summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The logistics node repo mock.
        /// </summary>
        private Mock<IRepository<Annulation>> reversalRepoMock;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockFactory = new Mock<IRepositoryFactory>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.processor = new AnnulationProcessor(this.mockFactory.Object, this.mockUnitOfWorkFactory.Object);

            this.token = new CancellationToken(false);
            this.reversalRepoMock = new Mock<IRepository<Annulation>>();
            this.mockFactory.Setup(m => m.CreateRepository<Annulation>()).Returns(this.reversalRepoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Annulation>()).Returns(this.reversalRepoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(this.token));
        }

        /// <summary>
        /// Creates the reversal asynchronous should create reversal from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CreateReversalAsync_ShouldCreateReversalFromRepository_WhenInvokedAsync()
        {
            var reaversal = new Annulation();

            var repoMock = new Mock<IRepository<Annulation>>();
            repoMock.Setup(r => r.Insert(It.IsAny<Annulation>()));

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Annulation>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(this.token));

            await this.processor.CreateAnnulationRelationshipAsync(reaversal).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Annulation>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(this.token), Times.Once);
            repoMock.Verify(r => r.Insert(It.IsAny<Annulation>()), Times.Once);
        }

        /// <summary>
        /// Updates the annulation asynchronous should create reversal from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateAnnulationAsync_ShouldUpdateAnnulationFromRepository_WhenInvokedAsync()
        {
            var annulation = new Annulation();

            var repoMock = new Mock<IRepository<Annulation>>();
            repoMock.Setup(r => r.Update(It.IsAny<Annulation>()));
            repoMock.Setup(r => r.GetByIdAsync(annulation.AnnulationId)).ReturnsAsync(annulation);

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Annulation>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(this.token));

            await this.processor.UpdateAnnulationRelationshipAsync(annulation).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Annulation>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(this.token), Times.Once);
            repoMock.Verify(r => r.Update(It.IsAny<Annulation>()), Times.Once);
            repoMock.Verify(r => r.GetByIdAsync(annulation.AnnulationId), Times.Once);
        }

        /// <summary>
        /// Updates the annulation asynchronous should throw exception if annulation not exist from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task UpdateAnnulationAsync_ShouldThrowExceptionIfAnnulationNotExistFromRepository_WhenInvokedAsync()
        {
            var annulation = new Annulation();

            var repoMock = new Mock<IRepository<Annulation>>();
            repoMock.Setup(r => r.Update(It.IsAny<Annulation>()));
            repoMock.Setup(r => r.GetByIdAsync(annulation.AnnulationId)).ReturnsAsync(default(Annulation));

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Annulation>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(this.token));

            await this.processor.UpdateAnnulationRelationshipAsync(annulation).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Annulation>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(this.token), Times.Never);
            repoMock.Verify(r => r.Update(It.IsAny<Annulation>()), Times.Never);
            repoMock.Verify(r => r.GetByIdAsync(annulation.AnnulationId), Times.Once);
        }

        /// <summary>
        /// Exists the reversal relationship asynchronous should return Source type reversal from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExistsReversalRelationshipAsync_ShouldReturnTypeSource_WhenInvokedAsync()
        {
            var reaversal = new Annulation { AnnulationMovementTypeId = 2, SourceMovementTypeId = 3, IsActive = true, AnnulationCategoryElement = new CategoryElement { IsActive = true, Name = "Test" }, SourceCategoryElement = new CategoryElement { IsActive = true, Name = "SourceTest" } };
            this.reversalRepoMock.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Annulation, bool>>>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<Annulation> { reaversal });

            var result = await this.processor.ExistsAnnulationRelationshipAsync(reaversal).ConfigureAwait(false);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Exists the reversal relationship asynchronous should return Source type reversal from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExistsReversalRelationshipAsync_ShouldReturnTypeReversal_WhenInvokedAsync()
        {
            var reaversal = new Annulation { AnnulationMovementTypeId = 2, SourceMovementTypeId = 3, IsActive = true, AnnulationCategoryElement = new CategoryElement { IsActive = true, Name = "Test" }, SourceCategoryElement = new CategoryElement { IsActive = true, Name = "SourceTest" } };
            var reaversalInput = new Annulation { AnnulationMovementTypeId = 2, SourceMovementTypeId = 4 };
            this.reversalRepoMock.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Annulation, bool>>>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<Annulation> { reaversal });

            var result = await this.processor.ExistsAnnulationRelationshipAsync(reaversalInput).ConfigureAwait(false);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Existses the reversal relationship asynchronous when invoked with null throw argument null exception asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExistsReversalRelationshipAsync_WhenInvokedWithNull_ThrowArgumentNullExceptionAsync()
        {
            var repoMock = new Mock<IRepository<Annulation>>();
            repoMock.Setup(r => r.Insert(It.IsAny<Annulation>()));

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await this.processor.ExistsAnnulationRelationshipAsync(default(Annulation)).ConfigureAwait(false), "Exception should be thrown if no node relationship delete info is passed as argument.").ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Annulation>(), Times.Never);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(this.token), Times.Never);
            repoMock.Verify(r => r.Insert(It.IsAny<Annulation>()), Times.Never);
        }

        /// <summary>
        /// Creates the annulation asynchronous should throw exception if annulation exists.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public async Task CreateAnnulationRelationship_ShouldThrowExceptionIfAnnulationExistFromRepository_WhenInvokedAsync()
        {
            var existingAnnulation = new Annulation
            {
                AnnulationId = 1,
                AnnulationMovementTypeId = 1,
            };

            var newAnnulation = new Annulation
            {
                AnnulationId = 2,
                AnnulationMovementTypeId = 2,
            };

            var reaversal = new Annulation { AnnulationMovementTypeId = 2, SourceMovementTypeId = 3, IsActive = true, AnnulationCategoryElement = new CategoryElement { IsActive = true, Name = "Test" }, SourceCategoryElement = new CategoryElement { IsActive = true, Name = "SourceTest" } };
            this.reversalRepoMock.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Annulation, bool>>>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<Annulation> { reaversal });
            this.reversalRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Annulation, bool>>>())).ReturnsAsync(existingAnnulation);
            this.mockFactory.Setup(a => a.CreateRepository<Annulation>()).Returns(this.reversalRepoMock.Object);

            await this.processor.CreateAnnulationRelationshipAsync(newAnnulation).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the annulation relationship should throw exception if annulation exist from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public async Task UpdateAnnulationRelationship_ShouldThrowExceptionIfAnnulationExistFromRepository_WhenInvokedAsync()
        {
            var existingAnnulation = new Annulation
            {
                AnnulationId = 1,
                AnnulationMovementTypeId = 1,
            };

            var newAnnulation = new Annulation
            {
                AnnulationId = 2,
                AnnulationMovementTypeId = 2,
            };

            var reaversal = new Annulation { AnnulationMovementTypeId = 2, SourceMovementTypeId = 3, IsActive = true, AnnulationCategoryElement = new CategoryElement { IsActive = true, Name = "Test" }, SourceCategoryElement = new CategoryElement { IsActive = true, Name = "SourceTest" } };
            this.reversalRepoMock.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Annulation, bool>>>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<Annulation> { reaversal });
            this.reversalRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Annulation, bool>>>())).ReturnsAsync(existingAnnulation);
            this.mockFactory.Setup(a => a.CreateRepository<Annulation>()).Returns(this.reversalRepoMock.Object);

            await this.processor.UpdateAnnulationRelationshipAsync(newAnnulation).ConfigureAwait(false);
        }
    }
}
