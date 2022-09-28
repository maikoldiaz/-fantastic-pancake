// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PendingTransactionErrorControllerTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Tests.Admin
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Api.Controllers;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The pending transaction error controller tests.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Api.Tests.ControllerTestBase" />
    [TestClass]
    public class PendingTransactionErrorControllerTests : ControllerTestBase
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private PendingTransactionErrorController controller;

        /// <summary>
        /// The mock processor.
        /// </summary>
        private Mock<IPendingTransactionErrorProcessor> mockProcessor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockProcessor = new Mock<IPendingTransactionErrorProcessor>();
            this.controller = new PendingTransactionErrorController(this.mockProcessor.Object);
        }

        /// <summary>
        /// Gets the pending transaction errors by ticket asynchronous when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetPendingTransactionErrorsByTicket_WhenInvokedAsync()
        {
            var list = new List<PendingTransactionErrorDto> { new PendingTransactionErrorDto { ErrorId = 1 } };
            this.mockProcessor.Setup(x => x.GetPendingTransactionsAsync(It.IsAny<Ticket>())).ReturnsAsync(list);

            var result = await this.controller.GetPendingTransactionErrorsAsync(new Ticket()).ConfigureAwait(false);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result as EntityResult);
            this.mockProcessor.Verify(a => a.GetPendingTransactionsAsync(It.IsAny<Ticket>()), Times.Once);
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }

        /// <summary>
        /// Saves the comments when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SaveComments_WhenInvokedAsync()
        {
            List<int> errorIds = new List<int>() { 1 };
            var errorComment = new ErrorComment() { ErrorId = errorIds, Comment = "Test Comment" };
            this.mockProcessor.Setup(x => x.SaveCommentsAsync(It.IsAny<ErrorComment>()));

            var result = await this.controller.SaveCommentsAsync(errorComment).ConfigureAwait(false);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.SaveCommentsAsync(errorComment), Times.Once());
        }

        /// <summary>
        /// Gets the error details when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetRetryErrorDetails_WhenInvokedAsync()
        {
            ErrorDetail errorDetail = new ErrorDetail()
            {
                Identifier = "1",
                Error = "Test Error",
            };

            IEnumerable<ErrorDetail> errorDetails = new List<ErrorDetail>() { errorDetail };
            string errorId = "1";
            this.mockProcessor.Setup(x => x.GetErrorDetailsAsync(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(errorDetails);

            var result = await this.controller.GetRetryErrorDetailsAsync(errorId).ConfigureAwait(false);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.GetErrorDetailsAsync(errorId, true), Times.Once());
        }

        /// <summary>
        /// Gets the error details when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetNonRetryErrorDetails_WhenInvokedAsync()
        {
            ErrorDetail errorDetail = new ErrorDetail()
            {
                Identifier = "1",
                Error = "Test Error",
            };

            IEnumerable<ErrorDetail> errorDetails = new List<ErrorDetail>() { errorDetail };
            string errorId = "1";
            this.mockProcessor.Setup(x => x.GetErrorDetailsAsync(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(errorDetails);

            var result = await this.controller.GetNonRetryErrorDetailsAsync(errorId).ConfigureAwait(false);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.GetErrorDetailsAsync(errorId, false), Times.Once());
        }
    }
}
