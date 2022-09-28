// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InMemoryRepositoryFactory.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.DataAccess.Sql.Interfaces;
    using Ecp.True.Entities;
    using Ecp.True.Entities.Core;
    using Ecp.True.Processors.Api.Tests.Admin;
    using Ecp.True.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Moq;

    public class InMemoryRepositoryFactory<T> : TestRepositoryFactory<T>
        where T : Entity
    {
        private SqlDataContext dataContext;

        /// <summary>
        /// Gets a in memory testing repository.
        /// </summary>
        /// <typeparam name="T">The entity.</typeparam>
        /// <returns>The repository.</returns>
        public override (IRepository<T>, SqlDataContext, ISqlTokenProvider) CreateRepository()
        {
            var auditServiceMock = new Mock<IAuditService>();
            var businessContextMock = new Mock<IBusinessContext>();
            var sqlTokenMock = new Mock<ISqlTokenProvider>();

            var options = new DbContextOptionsBuilder<SqlDataContext>()
                .UseInMemoryDatabase(databaseName: $"InMemoryDatabase_{Guid.NewGuid()}")
                .Options;

            auditServiceMock.Setup(m => m.GetAuditLogs(It.IsAny<ChangeTracker>())).Returns(new List<AuditLog>());
            this.dataContext = new SqlDataContext(
                options,
                auditServiceMock.Object,
                businessContextMock.Object,
                sqlTokenMock.Object);

            var repositoryFactory = new RepositoryFactory(this.dataContext);

            return (repositoryFactory.CreateRepository<T>(), this.dataContext, sqlTokenMock.Object);
        }
    }
}