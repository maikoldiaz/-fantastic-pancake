// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitOfWorkTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Tests
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The unit of work tests.
    /// </summary>
    [TestClass]
    public class UnitOfWorkTests
    {
        /// <summary>
        /// The unit of work instance.
        /// </summary>
        private UnitOfWork unitOfWork;

        /// <summary>
        /// The mock data context.
        /// </summary>
        private Mock<IDataContext> mockDataContext;

        /// <summary>
        /// The mock repository factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockRepositoryFactory;

        [TestInitialize]
        public void Initialize()
        {
            this.mockDataContext = new Mock<IDataContext>();
            this.mockRepositoryFactory = new Mock<IRepositoryFactory>();

            this.unitOfWork = new UnitOfWork(this.mockDataContext.Object, this.mockRepositoryFactory.Object);
        }

        /// <summary>
        /// Creates the repository should create repository from repository factory when invoked.
        /// </summary>
        [TestMethod]
        public void CreateRepository_ShouldCreateRepositoryFromRepositoryFactory_WhenInvoked()
        {
            // Arrange
            var repo = new Mock<IRepository<Category>>();
            this.mockRepositoryFactory.Setup(f => f.CreateRepository<Category>()).Returns(repo.Object);

            // Act
            var categoryRepository = this.unitOfWork.CreateRepository<Category>();

            // Assert
            Assert.IsNotNull(categoryRepository);
            this.mockRepositoryFactory.Verify(f => f.CreateRepository<Category>(), Times.Once);
        }

        /// <summary>
        /// Saves the asynchronous should save using data context when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task SaveAsync_ShouldSaveUsingDataContext_WhenInvokedAsync()
        {
            // Arrange
            this.mockDataContext.Setup(m => m.SaveAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);

            // Asert
            Assert.AreEqual(1, result);
            this.mockDataContext.Verify(m => m.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public void Dispose_ShouldDisposeDataContext_WhenInvoked()
        {
            // Arange
            this.mockDataContext.Setup(m => m.Dispose());

            // Act
            this.unitOfWork.Dispose();

            // Assert
            this.mockDataContext.Verify(m => m.Dispose(), Times.Once);
        }
    }
}
