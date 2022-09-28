// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitOfWorkFactoryTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Core.Tests
{
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Host.Functions.Core.Setup;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The unit of work factory tests.
    /// </summary>
    [TestClass]
    public class UnitOfWorkFactoryTests
    {
        /// <summary>
        /// The factory.
        /// </summary>
        private UnitOfWorkFactory factory;

        /// <summary>
        /// The mock data context.
        /// </summary>
        private Mock<ISqlDataContext> mockDataContext;

        /// <summary>
        /// The mock business context.
        /// </summary>
        private Mock<IRepositoryFactory> mockRepoFactory;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockDataContext = new Mock<ISqlDataContext>();
            this.mockRepoFactory = new Mock<IRepositoryFactory>();

            this.factory = new UnitOfWorkFactory(this.mockDataContext.Object, this.mockRepoFactory.Object);
        }

        /// <summary>
        /// Gets the unit of work should return unit of work with respository factory when initialized.
        /// </summary>
        [TestMethod]
        public void GetUnitOfWork_ShouldReturnUnitOfWork_WithRespositoryFactory_WhenInitialized()
        {
            // Act
            using (var unitOfWork = this.factory.GetUnitOfWork())
            {
                // Assert
                Assert.IsNotNull(unitOfWork);
            }
        }
    }
}
