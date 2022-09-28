// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PendingTransactionErrorProcessorTests.cs" company="Microsoft">
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
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Api.Admin;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The pending tranasaction error processor tests.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Api.Tests.ProcessorTestBase" />
    [TestClass]
    public class PendingTransactionErrorProcessorTests : ProcessorTestBase
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private PendingTransactionErrorProcessor processor;

        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockFactory;

        /// <summary>
        /// The PTE mock.
        /// </summary>
        private Mock<IPendingTransactionErrorRepository> pendingTransactionErrorRepositoryMock;

        /// <summary>The unit of work mock factory.</summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        [TestInitialize]
        public void Initialize()
        {
            this.mockFactory = new Mock<IRepositoryFactory>();
            this.pendingTransactionErrorRepositoryMock = new Mock<IPendingTransactionErrorRepository>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.processor = new PendingTransactionErrorProcessor(this.mockFactory.Object, this.pendingTransactionErrorRepositoryMock.Object, this.mockUnitOfWorkFactory.Object);
        }

        /// <summary>
        /// Gets the cut off messaage exceptions returns cut off message exception asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetCutOffMessaageExceptions_ReturnsCutOffMessageExceptionAsync()
        {
            var cutOffMessages = new List<PendingTransactionErrorDto>();
            var repoMock = new Mock<IRepository<PendingTransactionError>>();
            repoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<PendingTransactionError, bool>>>(), nameof(PendingTransaction))).ReturnsAsync(cutOffMessages);

            this.mockFactory.Setup(x => x.CreateRepository<PendingTransactionError>()).Returns(repoMock.Object);

            this.pendingTransactionErrorRepositoryMock.Setup(x => x.GetPendingTransactionErrorsAsync(It.IsAny<Ticket>())).ReturnsAsync(cutOffMessages);
            var repository = await this.processor.GetPendingTransactionsAsync(new Ticket()).ConfigureAwait(false);

            Assert.IsNotNull(repository);
            this.pendingTransactionErrorRepositoryMock.Verify(x => x.GetPendingTransactionErrorsAsync(It.IsAny<Ticket>()), Times.Once);
        }

        /// <summary>
        /// Saves the comments shoul save comments from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SaveComments_ShoulSaveCommentsFromRepository_WhenInvokedAsync()
        {
            IEnumerable<int> errorIds = new List<int>() { 1 };
            var errorComment = new ErrorComment() { ErrorId = errorIds, Comment = "Test Comment" };
            var token = new CancellationToken(false);

            PendingTransactionError pendingTransactionError = new PendingTransactionError()
            {
                ErrorId = 1,
                ErrorMessage = "Test Error message",
                TransactionId = 1,
            };

            var repoMock = new Mock<IRepository<PendingTransactionError>>();
            repoMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(pendingTransactionError);

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<PendingTransactionError>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));

            await this.processor.SaveCommentsAsync(errorComment).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<PendingTransactionError>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Once);
            repoMock.Verify(r => r.Update(It.IsAny<PendingTransactionError>()), Times.Once);
        }

        /// <summary>
        /// Gets the error details should query from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetErrorDetails_ShouldQueryFromRepository_WhenInvokedAsync()
        {
            var errorId = "hjkhkhj";
            IEnumerable<ErrorDetail> errorDetails = new List<ErrorDetail>();
            var repoMock = new Mock<IRepository<ErrorDetail>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(errorDetails);
            this.mockFactory.Setup(m => m.CreateRepository<ErrorDetail>()).Returns(repoMock.Object);
            var result = await this.processor.GetErrorDetailsAsync(errorId, true).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.mockFactory.Verify(m => m.CreateRepository<ErrorDetail>(), Times.Once);
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }
    }
}
