// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeControllerTests.cs" company="Microsoft">
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

    /// <summary>
    /// The node controller tests.
    /// </summary>
    [TestClass]
    public class NodeControllerTests : ControllerTestBase
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private NodeController controller;

        /// <summary>
        /// The mock processor.
        /// </summary>
        private Mock<INodeProcessor> mockProcessor;

        /// <summary>
        /// The mock ownership processor.
        /// </summary>
        private Mock<INodeOwnershipProcessor> mockOwnershipProcessor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockProcessor = new Mock<INodeProcessor>();
            this.mockOwnershipProcessor = new Mock<INodeOwnershipProcessor>();
            this.controller = new NodeController(this.mockProcessor.Object);
        }

        /// <summary>
        /// Nodes the name exists asynchronous should invoke processor to return true.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task NodeNameExistsAsync_ShouldInvokeProcessor_ToReturnTheNodeEntityAsync()
        {
            var node = new Node { Name = "Node" };
            this.mockProcessor.Setup(m => m.GetNodeByNameAsync(node.Name)).ReturnsAsync(node);

            var result = await this.controller.ExistsNodeAsync(node.Name).ConfigureAwait(false);

            var entityExistsResult = result as EntityExistsResult;

            // Assert
            Assert.IsNotNull(entityExistsResult);

            this.mockProcessor.Verify(c => c.GetNodeByNameAsync(node.Name), Times.Once());
        }

        /// <summary>
        /// Nodes the name exists asynchronous should invoke processor to return null entity.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task NodeNameExistsAsync_ShouldInvokeProcessor_ToReturnNullEntityAsync()
        {
            var node = new Node { Name = "Node" };
            this.mockProcessor.Setup(m => m.GetNodeByNameAsync(node.Name)).ReturnsAsync(() => null);

            var result = await this.controller.ExistsNodeAsync(node.Name).ConfigureAwait(false);

            var entityExistsResult = result as EntityExistsResult;

            // Assert
            Assert.IsNotNull(entityExistsResult);

            this.mockProcessor.Verify(c => c.GetNodeByNameAsync(node.Name), Times.Once());
        }

        /// <summary>
        /// Nodes the storage name exists asynchronous should invoke processor to return the node storage name entity.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task NodeStorageNameExistsAsync_ShouldInvokeProcessor_ToReturnTheNodeStorageNameEntityAsync()
        {
            var storageLocation = new NodeStorageLocation
            {
                Name = "Location",
                NodeId = 1,
            };

            this.mockProcessor.Setup(m => m.GetStorageLocationByNameAsync(storageLocation.Name, storageLocation.NodeId)).ReturnsAsync(storageLocation);

            var result = await this.controller.ExistsNodeStorageLocationAsync(storageLocation.Name, storageLocation.NodeId).ConfigureAwait(false);

            var entityExistsResult = result as EntityExistsResult;

            // Assert
            Assert.IsNotNull(entityExistsResult);

            this.mockProcessor.Verify(c => c.GetStorageLocationByNameAsync(storageLocation.Name, storageLocation.NodeId), Times.Once());
        }

        /// <summary>
        /// Nodes the storage name exists asynchronous should invoke processor to return null entity.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task NodeStorageNameExistsAsync_ShouldInvokeProcessor_ToReturnNullEntityAsync()
        {
            var storageLocation = new NodeStorageLocation
            {
                Name = "Location",
                NodeId = 1,
            };
            this.mockProcessor.Setup(m => m.GetStorageLocationByNameAsync(storageLocation.Name, storageLocation.NodeId)).ReturnsAsync(() => null);

            var result = await this.controller.ExistsNodeStorageLocationAsync(storageLocation.Name, storageLocation.NodeId).ConfigureAwait(false);

            var entityExistsResult = result as EntityExistsResult;

            // Assert
            Assert.IsNotNull(entityExistsResult);

            this.mockProcessor.Verify(c => c.GetStorageLocationByNameAsync(storageLocation.Name, storageLocation.NodeId), Times.Once());
        }

        /// <summary>
        /// Creates the node asynchronous should invoke processor to return200 success.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task CreateNodeAsync_ShouldInvokeProcessor_ToReturn200SuccessAsync()
        {
            var node = new Node();
            this.mockProcessor.Setup(m => m.SaveNodeAsync(node));

            var result = await this.controller.CreateNodeAsync(node).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.SaveNodeAsync(node), Times.Once());
        }

        /// <summary>
        /// Creates the node asynchronous should invoke processor to check if any node exists with the same order.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task ExistsNodeWithSameOrderAsync_ShouldInvokeProcessor_ToCheckIfNodesExistWithSameOrderAsync()
        {
            this.mockProcessor.Setup(m => m.GetNodeWithSameOrderAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new Node());

            var result = await this.controller.GetNodeWithSameOrderAsync(1, 10, 1).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.GetNodeWithSameOrderAsync(1, It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }

        /// <summary>
        /// Creates the node asynchronous should invoke processor to return200 success.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task UpdateNodeAsync_ShouldInvokeProcessor_ToUpdateNodeAsync()
        {
            var node = new Node();
            this.mockProcessor.Setup(m => m.UpdateNodeAsync(node));

            var result = await this.controller.UpdateNodeAsync(node).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.UpdateNodeAsync(node), Times.Once());
        }

        /// <summary>
        /// Gets the nodes asynchronous should return active nodes.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetNodesAsync_ShouldInvokeProcessor_ToReturnNodesAsync()
        {
            var nodes = new[] { new Node() }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryAllAsync<Node>(null)).ReturnsAsync(nodes);

            var result = await this.controller.QueryNodesAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.AreEqual(result, nodes);

            this.mockProcessor.Verify(c => c.QueryAllAsync<Node>(null), Times.Once());
        }

        /// <summary>
        /// Gets the node storage locations asynchronous should invoke processor to return node storage locations asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetNodeStorageLocationsAsync_ShouldInvokeProcessor_ToReturnNodeStorageLocationsAsync()
        {
            var nodes = new[] { new NodeStorageLocation() }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryAllAsync<NodeStorageLocation>(It.IsAny<Expression<Func<NodeStorageLocation, bool>>>())).ReturnsAsync(nodes);

            var result = await this.controller.QueryNodeStorageLocationsByNodeIdAsync(1).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.AreEqual(result, nodes);

            this.mockProcessor.Verify(c => c.QueryAllAsync<NodeStorageLocation>(It.IsAny<Expression<Func<NodeStorageLocation, bool>>>()), Times.Once());
        }

        /// <summary>
        /// Gets the node by identifier asynchronous should invoke processor to return node asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetNodeByIdAsync_ShouldInvokeProcessor_ToReturnNodeAsync()
        {
            var node = new Node();
            this.mockProcessor.Setup(m => m.GetNodeByIdAsync(1)).ReturnsAsync(node);

            var result = await this.controller.GetNodeByIdAsync(1).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.GetNodeByIdAsync(1), Times.Once());
        }

        /// <summary>
        /// Gets the node by identifier asynchronous should invoke processor to return node asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetNodeByIdAsync_ShouldReturnNotFound_WhenNodeIsNotFoundAsync()
        {
            this.mockProcessor.Setup(m => m.GetNodeByIdAsync(1)).Returns(Task.FromResult(default(Node)));

            var result = await this.controller.GetNodeByIdAsync(1).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.GetNodeByIdAsync(1), Times.Once());
        }

        [TestMethod]
        public async Task UpdateNodePropertiesAsync_ShouldReturnSuccessAsync()
        {
            var node = new Node();
            node.NodeId = 1;
            node.ControlLimit = 1.0m;
            node.AcceptableBalancePercentage = 2.2m;
            this.mockProcessor.Setup(m => m.UpdateNodePropertiesAsync(node));

            var result = await this.controller.UpdateNodePropertiesAsync(node).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.UpdateNodePropertiesAsync(node), Times.Once());
        }

        [TestMethod]
        public async Task UpdateStorageLocationProduct_ShouldReturnSuccessAsync()
        {
            var storageLocationProduct = new StorageLocationProduct();
            storageLocationProduct.StorageLocationProductId = 1;
            storageLocationProduct.UncertaintyPercentage = 1.0m;
            this.mockProcessor.Setup(m => m.UpdateStorageLocationProductAsync(storageLocationProduct));

            var result = await this.controller.UpdateStorageLocationProductAsync(storageLocationProduct).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.UpdateStorageLocationProductAsync(storageLocationProduct), Times.Once());
        }

        [TestMethod]
        public async Task UpdateStorageLocationProductOwnersAsync_ShouldReturnSuccessAsync()
        {
            StorageLocationProductOwner owner1 = new StorageLocationProductOwner
            {
                StorageLocationProductId = 1,
                OwnerId = 10,
                OwnershipPercentage = 50,
            };

            StorageLocationProductOwner owner2 = new StorageLocationProductOwner
            {
                StorageLocationProductId = 1,
                OwnerId = 11,
                OwnershipPercentage = 50,
            };

            StorageLocationProductOwner[] owners = new StorageLocationProductOwner[2];
            owners[0] = owner1;
            owners[1] = owner2;

            var rowVersion = "AAAAAAAAcHE=";

            var productOwners = new UpdateStorageLocationProductOwners()
            {
                ProductId = 1,
                RowVersion = rowVersion.FromBase64(),
                Owners = owners,
            };

            this.mockProcessor.Setup(m => m.UpdateStorageLocationProductOwnersAsync(productOwners));

            var result = await this.controller.UpdateStorageLocationProductOwnersAsync(productOwners).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.UpdateStorageLocationProductOwnersAsync(productOwners), Times.Once());
        }

        [TestMethod]
        public async Task GetNodeTypeAsync_ShouldInvokeProcessor_ToGetNodeTypeAsync()
        {
            CategoryElement categoryElement = new CategoryElement() { Name = "TestCategory" };
            this.mockProcessor.Setup(x => x.GetNodeTypeAsync(1)).ReturnsAsync(categoryElement);

            var result = await this.controller.GetNodeTypeAsync(1).ConfigureAwait(false);
            var objectResult = result as EntityResult;
            var returnMessage = objectResult.Value;

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(objectResult, "Object result should not be null");
            Assert.IsNotNull(returnMessage, "Node connection should not be null");
            this.mockProcessor.Verify(x => x.GetNodeTypeAsync(1), Times.Once);
        }

        [TestMethod]
        public async Task GetLogisticCenterNameAsync_ShouldInvokeProcessor_ToGetNodeAsync()
        {
            string nodeName = "test";
            var node = new Node();
            this.mockProcessor.Setup(x => x.GetNodeByNameAsync(nodeName)).ReturnsAsync(node);

            var result = await this.controller.GetLogisticCenterNameAsync(nodeName).ConfigureAwait(false);
            var objectResult = result as EntityResult;
            var returnMessage = objectResult.Value;

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(objectResult, "Object result should not be null");
            Assert.IsNotNull(returnMessage, "Node should not be null");
            this.mockProcessor.Verify(x => x.GetNodeByNameAsync(nodeName), Times.Once);
        }

        /// <summary>
        /// Queries the node product rules asynchronous.
        /// </summary>
        /// /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetNodeProductRulesAsync_ShouldInvokeProcessor_ToGetNodeRulesAsync()
        {
            var nodeRules = new[] { new NodeProductRule() };
            this.mockProcessor.Setup(m => m.GetNodeProductRulesAsync()).ReturnsAsync(nodeRules);

            var result = await this.controller.GetNodeProductRulesAsync().ConfigureAwait(false);
            var actionResult = result as EntityResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            Assert.IsNotNull(actionResult, "Result should not be null");
            this.mockProcessor.Verify(m => m.GetNodeProductRulesAsync(), Times.Once);
        }

        /// <summary>
        /// Queries the node rules asynchronous.
        /// </summary>
        /// /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetNodeOwnershipRulesAsync_ShouldInvokeProcessor_ToGetNodeRulesAsync()
        {
            var nodeRules = new[] { new NodeOwnershipRule() };
            this.mockProcessor.Setup(m => m.GetNodeOwnershipRulesAsync()).ReturnsAsync(nodeRules);

            var result = await this.controller.GetNodeOwnershipRulesAsync().ConfigureAwait(false);
            var actionResult = result as EntityResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            Assert.IsNotNull(actionResult, "Result should not be null");
            this.mockProcessor.Verify(m => m.GetNodeOwnershipRulesAsync(), Times.Once);
        }
    }
}
