// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryAssociationControllerTests.cs" company="Microsoft">
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
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Host.Api.Controllers;
    using Ecp.True.Processors.Api.Interfaces;
    using EfCore.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The category controller tests.
    /// </summary>
    [TestClass]
    public class CategoryAssociationControllerTests
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private NodeTagController controller;

        /// <summary>
        /// The mock processor.
        /// </summary>
        private Mock<INodeTagProcessor> mockProcessor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockProcessor = new Mock<INodeTagProcessor>();
            this.controller = new NodeTagController(this.mockProcessor.Object);
        }

        /// <summary>
        /// Gets the categories asynchronous should return active categories.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetNodeAssociationsAsync_ShouldInvokeProcessor_ToReturnCategoriesAsync()
        {
            var nodes = new[] { new NodeTag() }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryAllAsync<NodeTag>(null)).ReturnsAsync(nodes);
            var result = await this.controller.QueryAllAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.AreEqual(result, nodes);

            this.mockProcessor.Verify(x => x.QueryAllAsync<NodeTag>(null), Times.Once());
        }

        /// <summary>
        /// Gets the categories asynchronous should return active categories.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task AssociateNode_ShouldInvokeProcessor_ToReturnCategoriesAsync()
        {
            var result = await this.controller.TagNodeAsync(It.IsAny<TaggedNodeInfo>()).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");

            this.mockProcessor.Verify(x => x.TagNodeAsync(It.IsAny<TaggedNodeInfo>()), Times.Once());
        }
    }
}
