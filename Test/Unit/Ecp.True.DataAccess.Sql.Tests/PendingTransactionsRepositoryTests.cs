// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PendingTransactionsRepositoryTests.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Sql.Interfaces;
    using Ecp.True.Entities;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Repositories.Specialized;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The BalanceRepositoryTests.
    /// </summary>
    [TestClass]
    public class PendingTransactionsRepositoryTests
    {
        /// <summary>
        /// The business context.
        /// </summary>
        private Mock<IBusinessContext> businessContext;

        /// <summary>
        /// The data context.
        /// </summary>
        private SqlDataContext dataContext;

        /// <summary>
        /// The data access movement.
        /// </summary>
        private SqlDataAccess<PendingTransactionError> dataAccess;

        /// <summary>
        /// The data access inventory product.
        /// </summary>
        private SqlDataAccess<PendingTransaction> pendingTransaction;

        /// <summary>
        /// The storage location product.
        /// </summary>
        private SqlDataAccess<Node> node;

        /// <summary>
        /// The node connection product.
        /// </summary>
        private SqlDataAccess<Product> product;

        /// <summary>
        /// The element.
        /// </summary>
        private SqlDataAccess<CategoryElement> element;

        /// <summary>
        /// The mock audit service.
        /// </summary>
        private Mock<IAuditService> mockAuditService;

        /// <summary>
        /// The balance repository.
        /// </summary>
        private PendingTransactionErrorRepository repository;

        /// <summary>
        /// The SQL token provider.
        /// </summary>
        private Mock<ISqlTokenProvider> sqlTokenProvider;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockAuditService = new Mock<IAuditService>();
            this.businessContext = new Mock<IBusinessContext>();
            this.sqlTokenProvider = new Mock<ISqlTokenProvider>();
            var options = new DbContextOptionsBuilder<SqlDataContext>()
                                    .UseInMemoryDatabase(databaseName: $"InMemoryDatabase_{Guid.NewGuid()}")
                                    .Options;
            this.mockAuditService.Setup(m => m.GetAuditLogs(It.IsAny<ChangeTracker>())).Returns(new List<AuditLog>());
            this.dataContext = new SqlDataContext(options, this.mockAuditService.Object, this.businessContext.Object, this.sqlTokenProvider.Object);

            this.dataAccess = new SqlDataAccess<PendingTransactionError>(this.dataContext);
            this.pendingTransaction = new SqlDataAccess<PendingTransaction>(this.dataContext);
            this.node = new SqlDataAccess<Node>(this.dataContext);
            this.product = new SqlDataAccess<Product>(this.dataContext);
            this.element = new SqlDataAccess<CategoryElement>(this.dataContext);

            this.repository = new PendingTransactionErrorRepository(this.dataAccess);
        }

        /// <summary>
        /// Gets the pending transaction errors asynchronous when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetPendingTransactionErrorsAsync_ReturnsPendingTransactionErrors_WhenInvokedAsync()
        {
            var ticketEntity = new Ticket { TicketId = 1, CategoryElementId = 2, StartDate = new DateTime(2019, 02, 12), EndDate = new DateTime(2019, 02, 16), Status = 0, CreatedBy = "System", CreatedDate = new DateTime(2019, 02, 18), LastModifiedBy = null, LastModifiedDate = null };

            var pendingTransError1 = new PendingTransactionError { ErrorMessage = "test", TransactionId = 1, ErrorId = 1 };
            this.dataAccess.Insert(pendingTransError1);

            var pendingTrans1 = new PendingTransaction { TransactionId = 1, ActionTypeId = FileRegistrationActionType.Insert, TicketId = 1, StartDate = DateTime.Now, EndDate = DateTime.MaxValue, Units = 1 };
            this.pendingTransaction.Insert(pendingTrans1);

            var nodeEntity = new Node { NodeId = 1, Name = "Node One", };
            this.node.Insert(nodeEntity);

            var product1 = new Product { ProductId = "1", Name = "Crudo caño limón (CCL)" };
            this.product.Insert(product1);

            var categoryElement1 = new CategoryElement { ElementId = 2, CategoryId = 1, Name = "Segments1" };
            this.element.Insert(categoryElement1);

            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            var result = await this.repository.GetPendingTransactionErrorsAsync(ticketEntity).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
