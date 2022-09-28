// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeConnectionControllerTests.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Api.Controllers;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using QueryEntity = Ecp.True.Entities.Query;

    /// <summary>
    /// The node connection controller tests.
    /// </summary>
    [TestClass]
    public class NodeConnectionControllerTests : ControllerTestBase
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private NodeConnectionController controller;

        /// <summary>
        /// The mock processor.
        /// </summary>
        private Mock<INodeConnectionProcessor> mockProcessor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockProcessor = new Mock<INodeConnectionProcessor>();
            this.controller = new NodeConnectionController(this.mockProcessor.Object);
        }

        /// <summary>
        /// Creates the node connection asynchronous should invoke processor to create node connection asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task CreateNodeConnectionAsync_ShouldInvokeProcessor_ToCreateNodeConnectionAsync()
        {
            var nodeConnection = new NodeConnection();
            this.mockProcessor.Setup(x => x.CreateNodeConnectionAsync(nodeConnection));

            var result = await this.controller.CreateNodeConnectionAsync(nodeConnection).ConfigureAwait(false);
            var objectResult = result as EntityResult;

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(objectResult, "Object result should not be null");
            this.mockProcessor.Verify(x => x.CreateNodeConnectionAsync(nodeConnection), Times.Once);
        }

        /// <summary>
        /// Creates a node connection list asynchronous should invoke processor to create node connection asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task CreateNodeConnectionListAsync_ShouldInvokeProcessor_ToCreateNodeConnectionListAsync()
        {
            var nodeConnectionList = new List<NodeConnection> { new NodeConnection() };
            this.mockProcessor.Setup(x => x.CreateNodeConnectionListAsync(nodeConnectionList));

            var result = await this.controller.CreateNodeConnectionListAsync(nodeConnectionList).ConfigureAwait(false);
            var objectResult = result as EntityResult;

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(objectResult, "Object result should not be null");
            this.mockProcessor.Verify(x => x.CreateNodeConnectionListAsync(nodeConnectionList), Times.Once);
        }

        /// <summary>
        /// Updates the node connection asynchronous should invoke processor to update node connection asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task UpdateNodeConnectionAsync_ShouldInvokeProcessor_ToUpdateNodeConnectionAsync()
        {
            NodeConnection nodeConnection = new NodeConnection();
            this.mockProcessor.Setup(x => x.UpdateNodeConnectionAsync(nodeConnection));

            var result = await this.controller.UpdateNodeConnectionAsync(nodeConnection).ConfigureAwait(false);
            var objectResult = result as EntityResult;
            var returnMessage = objectResult.Messagekey;

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(objectResult, "Object result should not be null");
            Assert.IsNotNull(returnMessage, "Update succussfull message should not be null");
            Assert.AreEqual(returnMessage, Entities.Constants.NodeConnectionUpdatedSuccessfully);
            this.mockProcessor.Verify(x => x.UpdateNodeConnectionAsync(nodeConnection), Times.Once);
        }

        /// <summary>
        /// Deletes the node connection asynchronous should invoke processor to delete node connection asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task DeleteNodeConnectionAsync_ShouldInvokeProcessor_ToDeleteNodeConnectionAsync()
        {
            int sourceNodeId = 1;
            int destinationNodeId = 1;
            this.mockProcessor.Setup(x => x.DeleteNodeConnectionAsync(sourceNodeId, destinationNodeId));

            var result = await this.controller.DeleteNodeConnectionAsync(sourceNodeId, destinationNodeId).ConfigureAwait(false);
            var objectResult = result as EntityResult;
            var returnMessage = objectResult.Messagekey;

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(objectResult, "Object result should not be null");
            Assert.IsNotNull(returnMessage, "Delete succussfull message should not be null");
            Assert.AreEqual(returnMessage, Entities.Constants.NodeConnectionDeletedSuccessfully);
            this.mockProcessor.Verify(x => x.DeleteNodeConnectionAsync(sourceNodeId, destinationNodeId), Times.Once);
        }

        /// <summary>
        /// Gets the node connection asynchronous should invoke processor to get node connection asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetNodeConnectionAsync_ShouldInvokeProcessor_ToGetNodeConnectionAsync()
        {
            int sourceNodeId = 1;
            int destinationNodeId = 1;
            var sourceNode = new Node { Name = "SourceNode" };
            var destinationNode = new Node { Name = "DestinationNode" };
            NodeConnection nodeConnection = new NodeConnection() { SourceNode = sourceNode, DestinationNode = destinationNode };
            this.mockProcessor.Setup(x => x.GetNodeConnectionAsync(sourceNodeId, destinationNodeId)).ReturnsAsync(nodeConnection);

            var result = await this.controller.GetNodeConnectionByIdAsync(sourceNodeId, destinationNodeId).ConfigureAwait(false);
            var objectResult = result as EntityResult;
            var returnMessage = (NodeConnection)objectResult.Value;

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(objectResult, "Object result should not be null");
            Assert.IsNotNull(returnMessage, "Node connection should not be null");
            Assert.IsNotNull(returnMessage.SourceNode, "Source node should not be null");
            Assert.AreEqual(returnMessage.SourceNode.Name, sourceNode.Name, "Expected source node should be returned.");
            Assert.AreEqual(returnMessage.DestinationNode.Name, destinationNode.Name, "Expected destination node should be returned.");
            this.mockProcessor.Verify(x => x.GetNodeConnectionAsync(sourceNodeId, destinationNodeId), Times.Once);
        }

        /// <summary>
        /// Queries the node connections asynchronous should invoke processor to get all node connection asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task QueryNodeConnectionsAsync_ShouldInvokeProcessor_ToGetAllNodeConnectionAsync()
        {
            var sourceNode = new Node { Name = "SourceNode" };
            var destinationNode = new Node { Name = "DestinationNode" };

            var nodeConnections = new[] { new NodeConnection() { SourceNode = sourceNode, DestinationNode = destinationNode } }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryAllAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>())).ReturnsAsync(nodeConnections);

            var result = await this.controller.QueryNodeConnectionsAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.AreEqual(result, nodeConnections);
            this.mockProcessor.Verify(x => x.QueryAllAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Queries the node connections asynchronous should invoke processor to get all node connection asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task QueryNodeConnectionProductsAsync_ShouldInvokeProcessor_ToGetAllNodeConnectionProductsAsync()
        {
            var nodeConnections = new[] { new NodeConnectionProduct() }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryAllAsync(It.IsAny<Expression<Func<NodeConnectionProduct, bool>>>())).ReturnsAsync(nodeConnections);

            var result = await this.controller.QueryNodeConnectionProductsAsync(1).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.AreEqual(result, nodeConnections);
            this.mockProcessor.Verify(x => x.QueryAllAsync(It.IsAny<Expression<Func<NodeConnectionProduct, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Updates the node connection asynchronous should invoke processor to update node connection asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task UpdateNodeConnectionProductAsync_ShouldInvokeProcessor_ToUpdateNodeConnectionProductAsync()
        {
            var nodeConnectionProduct = new NodeConnectionProduct();
            this.mockProcessor.Setup(x => x.UpdateNodeConnectionProductAsync(nodeConnectionProduct));

            var result = await this.controller.UpdateNodeConnectionProductAsync(nodeConnectionProduct).ConfigureAwait(false);
            var objectResult = result as EntityResult;
            var returnMessage = objectResult.Messagekey;

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(objectResult, "Object result should not be null");
            Assert.IsNotNull(returnMessage, "Update succussfull message should not be null");
            Assert.AreEqual(returnMessage, Entities.Constants.NodeConnectionProductUpdatedSuccessfully);
            this.mockProcessor.Verify(x => x.UpdateNodeConnectionProductAsync(nodeConnectionProduct), Times.Once);
        }

        /// <summary>
        /// Updates the node connection asynchronous should invoke processor to update node connection asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task UpdateNodeConnectionProductOwnersAsync_ShouldInvokeProcessor_ToUpdateNodeConnectionProductOwnersAsync()
        {
            var owners = new[] { new NodeConnectionProductOwner() };
            var rowVersion = "AAAAAAAAcHE=";

            var productOwners = new UpdateNodeConnectionProductOwners()
            {
                ProductId = 1,
                RowVersion = rowVersion.FromBase64(),
                Owners = owners,
            };
            this.mockProcessor.Setup(x => x.SaveNodeConnectionProductOwnersAsync(productOwners));

            var result = await this.controller.UpdateNodeConnectionProductOwnersAsync(productOwners).ConfigureAwait(false);
            var objectResult = result as EntityResult;
            var returnMessage = objectResult.Messagekey;

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(objectResult, "Object result should not be null");
            Assert.IsNotNull(returnMessage, "Update succussfull message should not be null");
            Assert.AreEqual(returnMessage, Entities.Constants.NodeConnectionUpdatedSuccessfully);
            this.mockProcessor.Verify(x => x.SaveNodeConnectionProductOwnersAsync(productOwners), Times.Once);
        }

        [TestMethod]
        public async Task GetNodeConnectionProductRules_ReturnsRules_WhenInvokedAsync()
        {
            var nodeConnectionProductRules = new List<NodeConnectionProductRuleEntity>() { new NodeConnectionProductRuleEntity() }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryViewAsync<NodeConnectionProductRuleEntity>()).ReturnsAsync(nodeConnectionProductRules);
            var result = await this.controller.QueryNodeConnectionProductRulesViewAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(result, nodeConnectionProductRules);

            this.mockProcessor.Verify(c => c.QueryViewAsync<NodeConnectionProductRuleEntity>(), Times.Once());
        }

        [TestMethod]
        public async Task GetGraphicalNetwork_WhenInvokedAsync()
        {
            var graphicalNetwork = new QueryEntity.GraphicalNetwork(new List<QueryEntity.GraphicalNode>(), new List<QueryEntity.GraphicalNodeConnection>());
            this.mockProcessor.Setup(m => m.GetGraphicalNetworkAsync(1, 1)).ReturnsAsync(graphicalNetwork);
            var result = await this.controller.GetGraphicalNetworkAsync(1, 1).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");

            this.mockProcessor.Verify(c => c.GetGraphicalNetworkAsync(1, 1), Times.Once());
        }

        [TestMethod]
        public async Task GetGraphicalNetworkDestinationNodesDetailsById_WhenInvokedAsync()
        {
            var graphicalNetwork = new QueryEntity.GraphicalNetwork(new List<QueryEntity.GraphicalNode>(), new List<QueryEntity.GraphicalNodeConnection>());
            this.mockProcessor.Setup(m => m.GetGraphicalNetworkDestinationNodesDetailsBySourceNodeIdAsync(1)).ReturnsAsync(graphicalNetwork);
            var result = await this.controller.GetGraphicalNetworkDestinationNodesDetailsByIdAsync(1).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");

            this.mockProcessor.Verify(c => c.GetGraphicalNetworkDestinationNodesDetailsBySourceNodeIdAsync(1), Times.Once());
        }

        [TestMethod]
        public async Task GetGraphicalNetworkSourceNodesDetailsById_WhenInvokedAsync()
        {
            var graphicalNetwork = new QueryEntity.GraphicalNetwork(new List<QueryEntity.GraphicalNode>(), new List<QueryEntity.GraphicalNodeConnection>());
            this.mockProcessor.Setup(m => m.GetGraphicalNetworkSourceNodesDetailsByDestinationNodeIdAsync(1)).ReturnsAsync(graphicalNetwork);
            var result = await this.controller.GetGraphicalNetworkSourceNodesDetailsByIdAsync(1).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");

            this.mockProcessor.Verify(c => c.GetGraphicalNetworkSourceNodesDetailsByDestinationNodeIdAsync(1), Times.Once());
        }

        /// <summary>
        /// Queries the node connection product rules asynchronous.
        /// </summary>
        /// /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetNodeConnectionRulesAsync_ShouldInvokeProcessor_ToGetNodeRulesAsync()
        {
            var nodeRules = new[] { new NodeConnectionProductRule() };
            this.mockProcessor.Setup(m => m.GetProductRulesAsync()).ReturnsAsync(nodeRules);

            var result = await this.controller.GetNodeConnectionProductRulesAsync().ConfigureAwait(false);
            var actionResult = result as EntityResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            Assert.IsNotNull(actionResult, "Result should not be null");
            this.mockProcessor.Verify(m => m.GetProductRulesAsync(), Times.Once);
        }
    }
}
