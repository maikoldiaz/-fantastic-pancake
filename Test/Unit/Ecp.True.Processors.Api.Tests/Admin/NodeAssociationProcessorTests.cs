// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeAssociationProcessorTests.cs" company="Microsoft">
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
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Processors.Api.Admin;
    using EfCore.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The node processor tests.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Api.Tests.ProcessorTestBase" />
    [TestClass]
    public class NodeAssociationProcessorTests : ProcessorTestBase
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private NodeTagProcessor processor;

        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockFactory;

        /// <summary>
        /// The mock business context.
        /// </summary>
        private Mock<IBusinessContext> mockBusinessContext;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockFactory = new Mock<IRepositoryFactory>();
            this.mockBusinessContext = new Mock<IBusinessContext>();
            this.processor = new NodeTagProcessor(this.mockFactory.Object, this.mockBusinessContext.Object);
        }

        /// <summary>
        /// Checks the node name exists should return true for node name exists in repository when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task AssociateNode_ShouldAssocaiteNodes_WhenInvokedAsync()
        {
            var repoMock = new Mock<IRepository<NodeTag>>();
            repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeTag, bool>>>())).ReturnsAsync(new NodeTag());
            this.mockFactory.Setup(m => m.CreateRepository<NodeTag>()).Returns(repoMock.Object);

            await this.processor.TagNodeAsync(new TaggedNodeInfo() { OperationalType = 0 }).ConfigureAwait(false);

            this.mockFactory.Verify(m => m.CreateRepository<NodeTag>(), Times.Once);
        }
    }
}
