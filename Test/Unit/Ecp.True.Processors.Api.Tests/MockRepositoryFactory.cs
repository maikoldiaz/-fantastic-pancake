// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockRepositoryFactory.cs" company="Microsoft">
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
    using System;
    using System.Linq.Expressions;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Core;
    using Moq;

    public class MockRepositoryFactory<T>
        where T : Entity
    {
        /// <summary>
        /// Gets a mock repository.
        /// </summary>
        /// <typeparam name="T">The entity.</typeparam>
        /// <returns>The repository.</returns>
        public (Mock<IRepository<T>>, SqlDataContext, ISqlTokenProvider) CreateRepository()
        {
            var movementRepositoryMock = new Mock<IRepository<T>>();
            movementRepositoryMock.Setup(m => m.QueryAllAsync(It.IsAny<Expression<Func<T, bool>>>(), It.IsAny<string[]>()));

            var repositoryFactoryMock = new Mock<IRepositoryFactory>();
            repositoryFactoryMock.Setup(f => f.CreateRepository<T>())
                .Returns(movementRepositoryMock.Object);

            return (movementRepositoryMock, null, null);
        }
    }
}