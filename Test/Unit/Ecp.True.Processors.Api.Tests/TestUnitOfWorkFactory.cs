// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestUnitOfWorkFactory.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Tests
{
    using System.Threading;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Moq;

    /// <summary>
    /// The UnitOf work factory.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    public class TestUnitOfWorkFactory<T>
        where T : Entity, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestUnitOfWorkFactory{T}"/> class.
        /// </summary>
        public TestUnitOfWorkFactory()
        {
            this.RepositoryFactoryMock = new MockRepositoryFactory<T>();

            (this.RepositoryMock, _, _) = this.RepositoryFactoryMock.CreateRepository();

            this.UnitOfWork = new Mock<IUnitOfWork>();
            this.UnitOfWork
                .Setup(u => u.CreateRepository<T>())
                .Returns(this.RepositoryMock.Object);
            this.UnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
        }

        /// <summary>
        /// Gets the unit of work mock.
        /// </summary>
        /// <value>
        /// The unit of work.
        /// </value>
        public Mock<IUnitOfWork> UnitOfWork { get; }

        /// <summary>
        /// Gets the unit of work factory mock.
        /// </summary>
        /// <value>
        /// The unit of work factory.
        /// </value>
        public Mock<IUnitOfWorkFactory> UnitOfWorkFactory { get; }

        /// <summary>
        /// Gets the repository mock factory.
        /// </summary>
        /// <value>
        /// The mock.
        /// </value>
        public MockRepositoryFactory<T> RepositoryFactoryMock { get; }

        /// <summary>
        /// Gets the repository mock.
        /// </summary>
        /// <value>
        /// The repository mock.
        /// </value>
        public Mock<IRepository<T>> RepositoryMock { get; }

        /// <summary>
        /// Gets the unitOfWorkFactory mock.
        /// </summary>
        /// <returns>The mock.</returns>
        public Mock<IUnitOfWorkFactory> GetUnitOfWorkFactoryMock() => this.CreateUnitOfWorkFactory();

        private Mock<IUnitOfWorkFactory> CreateUnitOfWorkFactory()
        {
            this.UnitOfWork
                .Setup(u => u.SaveAsync(CancellationToken.None))
                .ReturnsAsync(1);

            this.UnitOfWorkFactory
                .Setup(u => u.GetUnitOfWork())
                .Returns(this.UnitOfWork.Object);

            return this.UnitOfWorkFactory;
        }
    }
}