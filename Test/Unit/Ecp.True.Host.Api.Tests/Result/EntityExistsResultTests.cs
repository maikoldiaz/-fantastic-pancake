// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityExistsResultTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Tests
{
    using System.Threading.Tasks;
    using Ecp.True.Entities;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Host.Core.Result;
    using Microsoft.AspNetCore.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The Entity Exists Result Tests.
    /// </summary>
    [TestClass]
    public class EntityExistsResultTests : ControllerTestBase
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.SetupHttpContext();
        }

        /// <summary>
        /// Entities the exists result should execute result for key node name to bad request object result asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task EntityExistsResult_ShouldExecuteResult_ForKeyNodeNameAsync()
        {
            var actionResult = new EntityExistsResult(nameof(Node), new Node());
            await actionResult.ExecuteResultAsync(this.ActionContext).ConfigureAwait(false);

            this.MockResourceProvider.Verify(r => r.GetResource(Constants.NodeNameMustBeUnique), Times.Once);
            Assert.AreEqual(StatusCodes.Status200OK, this.HttpContext.Response.StatusCode);
        }

        /// <summary>
        /// Entities the exists result should execute result for key storage location name to bad request object result asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task EntityExistsResult_ShouldExecuteResult_ForKeyStorageLocationNameAsync()
        {
            var actionResult = new EntityExistsResult(nameof(NodeStorageLocation), new NodeStorageLocation());
            await actionResult.ExecuteResultAsync(this.ActionContext).ConfigureAwait(false);

            this.MockResourceProvider.Verify(r => r.GetResource(Constants.StorageNameMustBeUnique), Times.Once);
            Assert.AreEqual(StatusCodes.Status200OK, this.HttpContext.Response.StatusCode);
        }

        /// <summary>
        /// Entities the exists result should execute result for key storage location name to bad request object result asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task EntityExistsResult_ShouldExecuteResult_ForKeyCategoryNameAsync()
        {
            var actionResult = new EntityExistsResult(nameof(Category), new Category());
            await actionResult.ExecuteResultAsync(this.ActionContext).ConfigureAwait(false);

            this.MockResourceProvider.Verify(r => r.GetResource(Constants.CategoryNameAlreadyExists), Times.Once);
            Assert.AreEqual(StatusCodes.Status200OK, this.HttpContext.Response.StatusCode);
        }

        /// <summary>
        /// Entities the exists result should execute result for key storage location name to bad request object result asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task EntityExistsResult_ShouldExecuteResult_ForKeyCategoryElementNameAsync()
        {
            var actionResult = new EntityExistsResult(nameof(CategoryElement), new CategoryElement());
            await actionResult.ExecuteResultAsync(this.ActionContext).ConfigureAwait(false);

            this.MockResourceProvider.Verify(r => r.GetResource(Constants.CategoryElementNameAlreadyExists), Times.Once);
            Assert.AreEqual(StatusCodes.Status200OK, this.HttpContext.Response.StatusCode);
        }

        /// <summary>
        /// Entities the exists result should execute result for key storage location name to bad request object result with default message asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task EntityExistsResult_ShouldExecuteResult_WhenElementIsNotFoundAsync()
        {
            var actionResult = new EntityExistsResult(nameof(LogisticCenter), new LogisticCenter());
            await actionResult.ExecuteResultAsync(this.ActionContext).ConfigureAwait(false);

            this.MockResourceProvider.Verify(r => r.GetResource(Constants.StorageNameMustBeUnique), Times.Never);
        }
    }
}
