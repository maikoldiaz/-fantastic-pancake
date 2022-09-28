// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryFactoryTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Repositories.Tests
{
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The repository factory tests.
    /// </summary>
    [TestClass]
    public class RepositoryFactoryTests
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private RepositoryFactory factory;

        /// <summary>
        /// The mock data context.
        /// </summary>
        private Mock<ISqlDataContext> mockDataContext;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockDataContext = new Mock<ISqlDataContext>();

            var categoryMock = new Mock<DbSet<Category>>();
            var movementMock = new Mock<DbSet<Movement>>();
            var productMock = new Mock<DbSet<Product>>();
            var storageLocationMock = new Mock<DbSet<StorageLocation>>();
            var elementMock = new Mock<DbSet<CategoryElement>>();
            var nodeMock = new Mock<DbSet<Node>>();
            var homologationMock = new Mock<DbSet<Homologation>>();
            var mappingsMock = new Mock<DbSet<HomologationDataMapping>>();

            this.mockDataContext.Setup(m => m.Set<Category>()).Returns(categoryMock.Object);
            this.mockDataContext.Setup(m => m.Set<Movement>()).Returns(movementMock.Object);
            this.mockDataContext.Setup(m => m.Set<Homologation>()).Returns(homologationMock.Object);
            this.mockDataContext.Setup(m => m.Set<Product>()).Returns(productMock.Object);
            this.mockDataContext.Setup(m => m.Set<StorageLocation>()).Returns(storageLocationMock.Object);
            this.mockDataContext.Setup(m => m.Set<CategoryElement>()).Returns(elementMock.Object);
            this.mockDataContext.Setup(m => m.Set<Node>()).Returns(nodeMock.Object);
            this.mockDataContext.Setup(m => m.Set<HomologationDataMapping>()).Returns(mappingsMock.Object);

            this.factory = new RepositoryFactory(this.mockDataContext.Object);
        }

        /// <summary>
        /// Creates the repository should create repository with data access.
        /// </summary>
        [TestMethod]
        public void CreateRepository_ShouldCreateRepository_WithDataAccess()
        {
            // Act
            var repository = this.factory.CreateRepository<Category>();

            // Assert
            Assert.IsNotNull(repository);
            this.mockDataContext.Verify(m => m.Set<Category>(), Times.Once);
        }

        /// <summary>
        /// Movements the repository should create repository with resolver.
        /// </summary>
        [TestMethod]
        public void MovementRepository_ShouldCreateRepository_WithResolver()
        {
            // Act
            var repository = this.factory.MovementRepository;

            // Assert
            Assert.IsNotNull(repository);
            this.mockDataContext.Verify(m => m.Set<Movement>(), Times.Once);
        }

        /// <summary>
        /// TicketInfoRepository repository should create repository with resolver.
        /// </summary>
        [TestMethod]
        public void TicketInfoRepositorynRepository_ShouldCreateRepository_WithResolver()
        {
            // Act
            var repository = this.factory.TicketInfoRepository;

            // Assert
            Assert.IsNotNull(repository);
            this.mockDataContext.Verify(m => m.Set<Ticket>(), Times.Once);
            this.mockDataContext.Verify(m => m.Set<Movement>(), Times.Once);
            this.mockDataContext.Verify(m => m.Set<InventoryProduct>(), Times.Once);
        }
    }
}
