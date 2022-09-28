// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateNodeConnectionListAttributeTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Tests.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.Core.Entities;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Host.Api.Filter;
    using Ecp.True.Host.Core.Result;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The node connection nodes filter attribute test class.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Api.Tests.ControllerTestBase" />
    [TestClass]
    public class ValidateNodeConnectionListAttributeTests : ControllerTestBase
    {
        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockFactory;

        /// <summary>
        /// The ResourceProvider mock.
        /// </summary>
        private Mock<IResourceProvider> resourceProviderMock;

        /// <summary>
        /// The NodeConnection Repository mock.
        /// </summary>
        private Mock<IRepository<Node>> nodesRepoMock;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockFactory = new Mock<IRepositoryFactory>();
            this.nodesRepoMock = new Mock<IRepository<Node>>();

            this.resourceProviderMock = new Mock<IResourceProvider>();
            this.resourceProviderMock.Setup(r => r.GetResource(It.IsAny<string>())).Returns<string>(s => s);

            this.SetupHttpContext();
        }

        [TestMethod]
        public async Task ValidateNodeConnectionListAttribute_ShowSucceedIfSourceNodeAndDestinationNodesAreValidAsync()
        {
            // Prepare
            var nodeList = new List<Node> { new Node { NodeId = 1 } };
            this.nodesRepoMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>()))
                .Returns(Task.FromResult((IEnumerable<Node>)nodeList));
            this.mockFactory.Setup(f => f.CreateRepository<Node>())
                .Returns(this.nodesRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            var testNodeConnections = new List<NodeConnection>
            {
                new NodeConnection { NodeConnectionId = 1, SourceNodeId = 1, DestinationNodeId = 2 },
                new NodeConnection { NodeConnectionId = 2, SourceNodeId = 3, DestinationNodeId = 4 },
            };

            this.ActionContext.HttpContext.RequestServices = serviceProviderMock.Object;
            var actionArguments = new Dictionary<string, object> { { "nodeConnections", testNodeConnections } };
            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var actionExecutedContext = new ActionExecutedContext(this.ActionContext, new List<IFilterMetadata>(), null);

            var sut = new ValidateNodeConnectionListAttribute();

            // Execute
            await sut.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(actionExecutedContext))
                .ConfigureAwait(false);
            var result = actionExecutingContext.Result as MultiStatusResult;
            var response = result?.Value as ICollection<NodeConnectionInfo>;
            var nodes = testNodeConnections.Select(n => n.SourceNodeId).Union(testNodeConnections.Select(n => n.DestinationNodeId));
            var notExistingNodes = nodes.Except(nodeList.Select(n => (int?)n.NodeId).ToList()).ToList();

            // Assert
            Assert.IsNotNull(actionExecutingContext.Result);
            Assert.IsNotNull(response);
            Assert.AreEqual(testNodeConnections.Count, response.Count);
            Assert.AreEqual(notExistingNodes.Count, response.SelectMany(r => r.Errors.ErrorCodes).Count());
            Assert.IsTrue(response.All(r => r.Status == EntityInfoCreationStatus.Error));
            Assert.IsFalse(response.Any(r => r.NodeConnection is null));

            this.mockFactory.Verify(m => m.CreateRepository<Node>(), Times.Once);
            this.nodesRepoMock.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>()), Times.Once);
            this.resourceProviderMock.Verify(m => m.GetResource(It.IsAny<string>()), Times.Exactly(notExistingNodes.Count));
        }
    }
}