// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityResultTests.cs" company="Microsoft">
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
    public class EntityResultTests : ControllerTestBase
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
        /// Execute should return ok object result when value is passed.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task Execute_ShouldReturnOkObjectResult_WhenValueIsPassedAsync()
        {
            var result = new EntityResult(new Category());

            await result.ExecuteResultAsync(this.ActionContext).ConfigureAwait(false);

            this.MockResourceProvider.Verify(r => r.GetResource(It.IsAny<string>()), Times.Never);
            Assert.AreEqual(StatusCodes.Status200OK, this.HttpContext.Response.StatusCode);
        }

        /// <summary>
        /// Execute should return not found object result when entity not found asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task Execute_ShouldReturnNotFoundObjectResult_WhenEntityNotFoundAsync()
        {
            var result = new EntityResult(null, Constants.CategoryNotExist);

            await result.ExecuteResultAsync(this.ActionContext).ConfigureAwait(false);

            this.MockResourceProvider.Verify(r => r.GetResource(Constants.CategoryNotExist), Times.Once);
        }

        /// <summary>
        /// Execute should return not found object result when entity not found asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task Execute_ShouldReturnOkObjectResult_WhenOnlyMessageAsync()
        {
            var result = new EntityResult(Constants.CategoryCreatedSuccessfully);

            await result.ExecuteResultAsync(this.ActionContext).ConfigureAwait(false);

            this.MockResourceProvider.Verify(r => r.GetResource(Constants.CategoryCreatedSuccessfully), Times.Once);
            Assert.AreEqual(StatusCodes.Status200OK, this.HttpContext.Response.StatusCode);
        }
    }
}
