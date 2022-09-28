// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditServiceTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Sql.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Sql.Interfaces;
    using Ecp.True.Entities;
    using Ecp.True.Entities.Admin;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The Audit Service tests.
    /// </summary>
    [TestClass]
    public class AuditServiceTests
    {
        /// <summary>
        /// The audit service.
        /// </summary>
        private AuditService auditService;

        /// <summary>
        /// The mock audit service.
        /// </summary>
        private Mock<IAuditService> mockAuditService;

        /// <summary>
        /// The data context.
        /// </summary>
        private SqlDataContext dataContext;

        /// <summary>
        /// The business context.
        /// </summary>
        private Mock<IBusinessContext> businessContext;

        /// <summary>
        /// The SQL token provider.
        /// </summary>
        private Mock<ISqlTokenProvider> sqlTokenProvider;

        /// <summary>
        /// Initilizes this instance.
        /// </summary>
        [TestInitialize]
        public void Initilize()
        {
            this.mockAuditService = new Mock<IAuditService>();
            this.businessContext = new Mock<IBusinessContext>();
            this.sqlTokenProvider = new Mock<ISqlTokenProvider>();
            this.mockAuditService.Setup(m => m.GetAuditLogs(It.IsAny<ChangeTracker>())).Returns(new List<AuditLog>());
            var options = new DbContextOptionsBuilder<SqlDataContext>()
                                    .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                                    .Options;
            this.dataContext = new SqlDataContext(options, this.mockAuditService.Object, this.businessContext.Object, this.sqlTokenProvider.Object);
            this.auditService = new AuditService();
        }

        /// <summary>
        /// Gets the audit logs.
        /// </summary>
        [TestMethod]
        public void GetAuditLogs()
        {
            // Arrange
            var changeTracker = this.dataContext.ChangeTracker;
            var dataAccess = new SqlDataAccess<Homologation>(this.dataContext);
            dataAccess.Insert(new Homologation());

            // Act
            var logs = this.auditService.GetAuditLogs(changeTracker);

            // Assert
            Assert.IsNotNull(logs);
            Assert.IsTrue(logs.Any(), "Audit log should have generated.");
            Assert.AreEqual(3, logs.Count());
        }

        /// <summary>
        /// Gets the audit logs should not audit if not auditable entity.
        /// </summary>
        [TestMethod]
        public void GetAuditLogs_ShouldNotAuditIfNotAuditableEntity()
        {
            // Arrange
            var changeTracker = this.dataContext.ChangeTracker;
            var dataAccess = new SqlDataAccess<User>(this.dataContext);
            dataAccess.Insert(new User { Name = "test" });

            // Act
            var logs = this.auditService.GetAuditLogs(changeTracker);

            // Assert
            Assert.IsNotNull(logs);
            Assert.IsFalse(logs.Any());
        }

        /// <summary>
        /// Gets the audit logs should filter entities on update operation when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetAuditLogs_ShouldFilterEntitiesOnUpdateOperation_WhenInvokedAsync()
        {
            // Arrange
            var category = new Category();
            var changeTracker = this.dataContext.ChangeTracker;
            var dataAccess = new SqlDataAccess<Category>(this.dataContext);

            dataAccess.Insert(category);
            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);
            var result = await dataAccess.GetByIdAsync(category.CategoryId).ConfigureAwait(false);
            result.Name = "New Category";
            dataAccess.Update(result);

            // Act
            var logs = this.auditService.GetAuditLogs(changeTracker);

            // Assert
            Assert.IsNotNull(logs);
            Assert.IsTrue(logs.Count() == 1);
        }
    }
}
