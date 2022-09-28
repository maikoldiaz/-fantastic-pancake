// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeConnectionProcessorTests.cs" company="Microsoft">
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
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Api.Admin;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using QueryEntity = Ecp.True.Entities.Query;

    /// <summary>
    /// The node connection processor tests.
    /// </summary>
    [TestClass]
    public class NodeConnectionProcessorTests
    {
        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The processor.
        /// </summary>
        private NodeConnectionProcessor processor;

        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockFactory;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockFactory = new Mock<IRepositoryFactory>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            var serviceBusQueueClient = new Mock<IServiceBusQueueClient>();
            this.mockAzureClientFactory.Setup(x => x.GetQueueClient(It.IsAny<string>())).Returns(serviceBusQueueClient.Object);
            this.processor = new NodeConnectionProcessor(this.mockUnitOfWorkFactory.Object, this.mockFactory.Object, this.mockAzureClientFactory.Object);
        }

        /// <summary>
        /// Creates the node connection asynchronous should invoke processor to throw argument null exception asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task CreateNodeConnectionAsync_ShouldInvokeProcessor_ToThrowArgumentNullExceptionAsync()
        {
            var nodes = new List<Node> { new Node { NodeId = 1 }, new Node { NodeId = 2 } };
            var token = new CancellationToken(false);

            var nodeRepoMock = new Mock<IRepository<Node>>();
            nodeRepoMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>())).Returns(Task.FromResult<IEnumerable<Node>>(nodes));
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(nodeRepoMock.Object);

            var repoMock = new Mock<IRepository<NodeConnection>>();
            repoMock.Setup(r => r.Insert(It.IsAny<NodeConnection>()));
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<NodeConnection>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await this.processor.CreateNodeConnectionAsync(default(NodeConnection)).ConfigureAwait(false), "Exception should be thrown if no connection is passed as argument.").ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<NodeConnection>(), Times.Never);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Never);
            repoMock.Verify(r => r.Insert(It.IsAny<NodeConnection>()), Times.Never);
            repoMock.Verify(r => r.Update(It.IsAny<NodeConnection>()), Times.Never);
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
            var nodes = new List<Node> { new Node { NodeId = 1 }, new Node { NodeId = 2 } };
            var nodeStorageLocations = new List<NodeStorageLocation>();
            var nodeConnection = new NodeConnection() { SourceNodeId = 1, DestinationNodeId = 2 };
            var token = new CancellationToken(false);

            var nodeRepoMock = new Mock<IRepository<Node>>();
            nodeRepoMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>())).Returns(Task.FromResult<IEnumerable<Node>>(nodes));

            var nodeStorageLocationRepoMock = new Mock<IRepository<NodeStorageLocation>>();
            nodeRepoMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<NodeStorageLocation, bool>>>())).Returns(Task.FromResult<IEnumerable<NodeStorageLocation>>(nodeStorageLocations));

            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(nodeRepoMock.Object);
            this.mockFactory.Setup(x => x.CreateRepository<NodeStorageLocation>()).Returns(nodeStorageLocationRepoMock.Object);

            var repoMock = new Mock<IRepository<NodeConnection>>();
            repoMock.Setup(r => r.Insert(It.IsAny<NodeConnection>()));
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<NodeConnection>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));

            await this.processor.CreateNodeConnectionAsync(nodeConnection).ConfigureAwait(false);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<NodeConnection>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Once);
            repoMock.Verify(r => r.Insert(It.IsAny<NodeConnection>()), Times.Once);
            repoMock.Verify(r => r.Update(It.IsAny<NodeConnection>()), Times.Never);
        }

        /// <summary>
        /// Creates the node connection asynchronous should invoke processor to throw node connection exists exception asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task CreateNodeConnectionAsync_ShouldInvokeProcessor_ToThrowNodeConnectionExistsExceptionAsync()
        {
            var nodes = new List<Node> { new Node { NodeId = 1 }, new Node { NodeId = 2 } };
            var nodeConnection = new NodeConnection() { SourceNodeId = 1, DestinationNodeId = 2, IsActive = true };
            var token = new CancellationToken(false);

            var nodeRepoMock = new Mock<IRepository<Node>>();
            nodeRepoMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>())).Returns(Task.FromResult<IEnumerable<Node>>(nodes));
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(nodeRepoMock.Object);

            var repoMock = new Mock<IRepository<NodeConnection>>();
            repoMock.Setup(r => r.Insert(It.IsAny<NodeConnection>()));
            repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(nodeConnection);

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<NodeConnection>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));

            var ex = await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => await this.processor.CreateNodeConnectionAsync(nodeConnection).ConfigureAwait(false), "Exception should be thrown if connection already exists.").ConfigureAwait(false);
            Assert.AreEqual(ex.Message, Entities.Constants.NodeConnectionAlreadyExists, "Invalid message is thrown in case of node connection already exists.");
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<NodeConnection>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Never);
            repoMock.Verify(r => r.Insert(It.IsAny<NodeConnection>()), Times.Never);
            repoMock.Verify(r => r.Update(It.IsAny<NodeConnection>()), Times.Never);
        }

        /// <summary>
        /// Creates the node connection asynchronous should invoke processor to activate connectionfor inavtive connection asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task CreateNodeConnectionAsync_ShouldInvokeProcessor_ToActivateConnectionForInactiveConnectionAsync()
        {
            var nodes = new List<Node> { new Node { NodeId = 1 }, new Node { NodeId = 2 } };
            var nodeConnection = new NodeConnection() { SourceNode = new Node(), SourceNodeId = 1, DestinationNodeId = 2, DestinationNode = new Node(), IsActive = false };
            var token = new CancellationToken(false);

            var nodeRepoMock = new Mock<IRepository<Node>>();
            nodeRepoMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>())).Returns(Task.FromResult<IEnumerable<Node>>(nodes));
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(nodeRepoMock.Object);

            var repoMock = new Mock<IRepository<NodeConnection>>();
            repoMock.Setup(r => r.Insert(It.IsAny<NodeConnection>()));
            repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(nodeConnection);

            var offchainRepoMock = new Mock<IRepository<OffchainNodeConnection>>();
            offchainRepoMock.Setup(r => r.Insert(It.IsAny<OffchainNodeConnection>()));

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<NodeConnection>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<OffchainNodeConnection>()).Returns(offchainRepoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));

            await this.processor.CreateNodeConnectionAsync(nodeConnection).ConfigureAwait(false);

            Assert.IsTrue(nodeConnection.IsActive.Value, "Inactive connection must be activated");

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<OffchainNodeConnection>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<NodeConnection>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Once);

            repoMock.Verify(r => r.Insert(It.IsAny<NodeConnection>()), Times.Never);
            repoMock.Verify(r => r.Update(It.IsAny<NodeConnection>()), Times.Once);
            offchainRepoMock.Verify(r => r.Insert(It.IsAny<OffchainNodeConnection>()), Times.Once);
        }

        /// <summary>
        /// Updates the node connection asynchronous should invoke processor to throw argument null exception asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task UpdateNodeConnectionAsync_ShouldInvokeProcessor_ToThrowArgumentNullExceptionAsync()
        {
            var token = new CancellationToken(false);

            var repoMock = new Mock<IRepository<NodeConnection>>();
            repoMock.Setup(r => r.Update(It.IsAny<NodeConnection>()));
            repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>())).ReturnsAsync(default(NodeConnection));

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<NodeConnection>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await this.processor.UpdateNodeConnectionAsync(default(NodeConnection)).ConfigureAwait(false), "Argument null exception must be thrown if no connection is passed in argument.").ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<NodeConnection>(), Times.Never);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Never);
            repoMock.Verify(r => r.Update(It.IsAny<NodeConnection>()), Times.Never);
        }

        /// <summary>
        /// Updates the node connection asynchronous should invoke processor to throw exception if connection not exists asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task UpdateNodeConnectionAsync_ShouldInvokeProcessor_ToThrowExceptionIfConnectionNotExistsAsync()
        {
            var nodeConnection = new NodeConnection() { SourceNodeId = 1, DestinationNodeId = 2, IsActive = false };
            var token = new CancellationToken(false);

            var repoMock = new Mock<IRepository<NodeConnection>>();
            repoMock.Setup(r => r.Update(It.IsAny<NodeConnection>()));
            repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>())).ReturnsAsync(default(NodeConnection));

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<NodeConnection>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));

            var ex = await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => await this.processor.UpdateNodeConnectionAsync(nodeConnection).ConfigureAwait(false), "Exception should be thrown while update operation if no connection exists.").ConfigureAwait(false);
            Assert.AreEqual(ex.Message, Entities.Constants.NodeConnectionDoesNotExists, "Invalid message is thrown while updating node connection that does not exists.");

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<NodeConnection>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Never);
            repoMock.Verify(r => r.Update(It.IsAny<NodeConnection>()), Times.Never);
        }

        /// <summary>
        /// Updates the node connection asynchronous should invoke processor to update connection asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task UpdateNodeConnectionAsync_ShouldInvokeProcessor_ToUpdateConnectionAsync()
        {
            var nodeConnection = new NodeConnection() { SourceNodeId = 1, DestinationNodeId = 2, IsActive = false, Description = "test description" };
            var inputNodeConnection = new NodeConnection() { SourceNodeId = 1, DestinationNodeId = 2, IsActive = true, Description = "test description  updated" };
            var token = new CancellationToken(false);

            var repoMock = new Mock<IRepository<NodeConnection>>();
            repoMock.Setup(r => r.Update(It.IsAny<NodeConnection>()));
            repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(nodeConnection);

            var offchainRepoMock = new Mock<IRepository<OffchainNodeConnection>>();
            offchainRepoMock.Setup(r => r.Insert(It.IsAny<OffchainNodeConnection>()));

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<OffchainNodeConnection>()).Returns(offchainRepoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<NodeConnection>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));

            await this.processor.UpdateNodeConnectionAsync(inputNodeConnection).ConfigureAwait(false);

            Assert.AreEqual(nodeConnection.IsActive.Value, inputNodeConnection.IsActive.Value);
            Assert.AreEqual(nodeConnection.Description, inputNodeConnection.Description);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<OffchainNodeConnection>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<NodeConnection>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Once);

            repoMock.Verify(r => r.Update(It.IsAny<NodeConnection>()), Times.Once);
            offchainRepoMock.Verify(r => r.Insert(It.IsAny<OffchainNodeConnection>()), Times.Once);
        }

        /// <summary>
        /// Deletes the node connection asynchronous should invoke processor to throw exception if connection not exists asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task DeleteNodeConnectionAsync_ShouldInvokeProcessor_ToThrowExceptionIfConnectionNotExistsAsync()
        {
            var token = new CancellationToken(false);
            var nodes = new List<Node> { new Node { NodeId = 1 }, new Node { NodeId = 2 } };

            var movementRepositoryMock = new Mock<IMovementRepository>();
            movementRepositoryMock.Setup(x => x.HasActiveMovementForConnectionAsync(1, 1)).ReturnsAsync(false);

            var nodeRepoMock = new Mock<IRepository<Node>>();
            nodeRepoMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>())).Returns(Task.FromResult<IEnumerable<Node>>(nodes));
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(nodeRepoMock.Object);
            this.mockFactory.Setup(x => x.MovementRepository).Returns(movementRepositoryMock.Object);

            var repoMock = new Mock<IRepository<NodeConnection>>();
            repoMock.Setup(r => r.Update(It.IsAny<NodeConnection>()));
            repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>())).ReturnsAsync(default(NodeConnection));

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<NodeConnection>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));

            var ex = await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => await this.processor.DeleteNodeConnectionAsync(1, 1).ConfigureAwait(false), "Exception should be thrown while update operation if no connection exists.").ConfigureAwait(false);
            Assert.AreEqual(ex.Message, Entities.Constants.NodeConnectionDoesNotExists, "Invalid message is thrown while deleting node connection that does not exists.");
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<NodeConnection>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Never);
            repoMock.Verify(r => r.Update(It.IsAny<NodeConnection>()), Times.Never);
            repoMock.Verify(r => r.Delete(It.IsAny<NodeConnection>()), Times.Never);
        }

        /// <summary>
        /// Deletes the node connection asynchronous should invoke processor to throw exception if movement exists asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task DeleteNodeConnectionAsync_ShouldInvokeProcessor_ToThrowExceptionIfMovementExistsAsync()
        {
            var token = new CancellationToken(false);
            var nodes = new List<Node> { new Node { NodeId = 1 }, new Node { NodeId = 2 } };

            var movementRepositoryMock = new Mock<IMovementRepository>();
            movementRepositoryMock.Setup(x => x.HasActiveMovementForConnectionAsync(1, 1)).ReturnsAsync(true);

            var nodeRepoMock = new Mock<IRepository<Node>>();
            nodeRepoMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>())).Returns(Task.FromResult<IEnumerable<Node>>(nodes));
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(nodeRepoMock.Object);
            this.mockFactory.Setup(x => x.MovementRepository).Returns(movementRepositoryMock.Object);

            var repoMock = new Mock<IRepository<NodeConnection>>();
            repoMock.Setup(r => r.Update(It.IsAny<NodeConnection>()));
            repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>())).ReturnsAsync(default(NodeConnection));

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<NodeConnection>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));

            var ex = await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => await this.processor.DeleteNodeConnectionAsync(1, 1).ConfigureAwait(false), "Exception should be thrown while update operation if movement exists.").ConfigureAwait(false);
            Assert.AreEqual(ex.Message, Entities.Constants.NodeConnectionDeleteConflict, "Invalid message is thrown while deleting node connection that has movement exists.");
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<NodeConnection>(), Times.Never);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Never);
            repoMock.Verify(r => r.Update(It.IsAny<NodeConnection>()), Times.Never);
            repoMock.Verify(r => r.Delete(It.IsAny<NodeConnection>()), Times.Never);
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
            var nodeConnection = new NodeConnection() { SourceNodeId = 1, DestinationNodeId = 2, IsActive = true, Description = "test description", IsDeleted = true };
            var nodes = new List<Node> { new Node { NodeId = 1 }, new Node { NodeId = 2 } };

            var nodeRepoMock = new Mock<IRepository<Node>>();
            nodeRepoMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>())).Returns(Task.FromResult<IEnumerable<Node>>(nodes));
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(nodeRepoMock.Object);

            var repoMock = new Mock<IRepository<NodeConnection>>();
            repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>(), nameof(NodeConnection.SourceNode), nameof(NodeConnection.DestinationNode))).ReturnsAsync(nodeConnection);
            this.mockFactory.Setup(m => m.CreateRepository<NodeConnection>()).Returns(repoMock.Object);

            var result = await this.processor.GetNodeConnectionAsync(nodeConnection.SourceNodeId.Value, nodeConnection.DestinationNodeId.Value).ConfigureAwait(false);

            Assert.IsNotNull(result, "Node connection result should not be null");
            Assert.AreEqual(result.SourceNodeId, nodeConnection.SourceNodeId, "Node connection source node identifier should not be same.");
            this.mockFactory.Verify(m => m.CreateRepository<NodeConnection>(), Times.Once);
            repoMock.Verify(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>(), nameof(NodeConnection.SourceNode), nameof(NodeConnection.DestinationNode)), Times.Once);
        }

        /// <summary>
        /// Updates the node connection product asynchronous should throw exception when node connection product asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task UpdateNodeConnectionProductAsync_ShouldThrowException_WhenNodeConnectionProductAsync()
        {
            var nodeConnectionProduct = new NodeConnectionProduct() { NodeConnectionId = 1, NodeConnectionProductId = 1, ProductId = "1000" };
            var token = new CancellationToken(false);

            NodeConnectionProduct returnObject = null;

            var nodeConnectionProductRepoMock = new Mock<IRepository<NodeConnectionProduct>>();
            nodeConnectionProductRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(returnObject);
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<NodeConnectionProduct>()).Returns(nodeConnectionProductRepoMock.Object);

            var ex = await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => await this.processor.UpdateNodeConnectionProductAsync(nodeConnectionProduct).ConfigureAwait(false), "Exception should be thrown while update operation if node connection product does not exist.").ConfigureAwait(false);
            Assert.AreEqual(ex.Message, Entities.Constants.NodeConnectionProductDoesNotExists);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Never);
            nodeConnectionProductRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        }

        /// <summary>
        /// Updates the node connection product asynchronous should invoke processor to update node connection product asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task UpdateNodeConnectionProductAsync_ShouldInvokeProcessor_ToUpdateNodeConnectionProductAsync()
        {
            var nodeConnectionProduct = new NodeConnectionProduct() { NodeConnectionId = 1, NodeConnectionProductId = 1, ProductId = "1000" };
            var token = new CancellationToken(false);

            var nodeConnectionProductRepoMock = new Mock<IRepository<NodeConnectionProduct>>();
            nodeConnectionProductRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(nodeConnectionProduct);
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<NodeConnectionProduct>()).Returns(nodeConnectionProductRepoMock.Object);

            await this.processor.UpdateNodeConnectionProductAsync(nodeConnectionProduct).ConfigureAwait(false);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Once);
            nodeConnectionProductRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        }

        /// <summary>
        /// Saves the node connection product owners asynchronous should return success asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task SaveNodeConnectionProductOwnersAsync_ShouldReturnSuccessAsync()
        {
            NodeConnectionProductOwner owner1 = new NodeConnectionProductOwner
            {
                NodeConnectionProductId = 1,
                OwnerId = 10,
                OwnershipPercentage = 50,
            };

            NodeConnectionProductOwner owner2 = new NodeConnectionProductOwner
            {
                NodeConnectionProductId = 1,
                OwnerId = 11,
                OwnershipPercentage = 50,
            };

            NodeConnectionProductOwner[] owners = new NodeConnectionProductOwner[2];
            owners[0] = owner1;
            owners[1] = owner2;

            var rowVersion = "AAAAAAAAcHE=";

            var productOwners = new UpdateNodeConnectionProductOwners()
            {
                ProductId = 1,
                RowVersion = rowVersion.FromBase64(),
                Owners = owners,
            };

            NodeConnectionProduct locProduct = new NodeConnectionProduct();
            locProduct.Initialize();
            var locRepository = new Mock<IRepository<NodeConnectionProduct>>();
            locRepository.Setup(y => y.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeConnectionProduct, bool>>>(), "Owners")).ReturnsAsync(locProduct);
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<NodeConnectionProduct>()).Returns(locRepository.Object);

            await this.processor.SaveNodeConnectionProductOwnersAsync(productOwners).ConfigureAwait(false);

            locRepository.Verify(y => y.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeConnectionProduct, bool>>>(), "Owners"), Times.Once);
            this.mockUnitOfWorkFactory.Verify(x => x.GetUnitOfWork().CreateRepository<NodeConnectionProduct>(), Times.Once);
        }

        /// <summary>
        /// Saves the node connection product owners asynchronous should throw exception when node connection product asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task SaveNodeConnectionProductOwnersAsync_ShouldThrowException_WhenNodeConnectionProductAsync()
        {
            var token = new CancellationToken(false);

            NodeConnectionProductOwner owner1 = new NodeConnectionProductOwner
            {
                NodeConnectionProductId = 1,
                OwnerId = 10,
                OwnershipPercentage = 50,
            };

            NodeConnectionProductOwner owner2 = new NodeConnectionProductOwner
            {
                NodeConnectionProductId = 1,
                OwnerId = 11,
                OwnershipPercentage = 50,
            };

            NodeConnectionProductOwner[] owners = new NodeConnectionProductOwner[2];
            owners[0] = owner1;
            owners[1] = owner2;

            var rowVersion = "AAAAAAAAcHE=";

            var productOwners = new UpdateNodeConnectionProductOwners()
            {
                ProductId = 1,
                RowVersion = rowVersion.FromBase64(),
                Owners = owners,
            };

            NodeConnectionProduct locProduct = null;
            var locRepository = new Mock<IRepository<NodeConnectionProduct>>();
            locRepository.Setup(y => y.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeConnectionProduct, bool>>>(), "Owners")).ReturnsAsync(locProduct);
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<NodeConnectionProduct>()).Returns(locRepository.Object);

            var ex = await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => await this.processor.SaveNodeConnectionProductOwnersAsync(productOwners).ConfigureAwait(false), "Exception should be thrown while update operation if node connection product does not exists.").ConfigureAwait(false);
            Assert.AreEqual(ex.Message, Entities.Constants.NodeConnectionProductDoesNotExists);
            locRepository.Verify(y => y.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeConnectionProduct, bool>>>(), "Owners"), Times.Once);
            this.mockUnitOfWorkFactory.Verify(x => x.GetUnitOfWork().CreateRepository<NodeConnectionProduct>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Never);
        }

        /// <summary>
        /// Saves the node connection product owners asynchronous should throw exception when ownership percentage is not valid asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task SaveNodeConnectionProductOwnersAsync_ShouldThrowException_WhenOwnershipPercentageIsNotValidAsync()
        {
            var token = new CancellationToken(false);

            NodeConnectionProductOwner owner1 = new NodeConnectionProductOwner
            {
                NodeConnectionProductId = 1,
                OwnerId = 10,
                OwnershipPercentage = 20,
            };

            NodeConnectionProductOwner owner2 = new NodeConnectionProductOwner
            {
                NodeConnectionProductId = 1,
                OwnerId = 11,
                OwnershipPercentage = 50,
            };

            NodeConnectionProductOwner[] owners = new NodeConnectionProductOwner[2];
            owners[0] = owner1;
            owners[1] = owner2;

            var rowVersion = "AAAAAAAAcHE=";

            var productOwners = new UpdateNodeConnectionProductOwners()
            {
                ProductId = 1,
                RowVersion = rowVersion.FromBase64(),
                Owners = owners,
            };

            NodeConnectionProduct locProduct = new NodeConnectionProduct();
            locProduct.Initialize();
            var locRepository = new Mock<IRepository<NodeConnectionProduct>>();
            locRepository.Setup(y => y.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeConnectionProduct, bool>>>(), "Owners")).ReturnsAsync(locProduct);
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<NodeConnectionProduct>()).Returns(locRepository.Object);

            var ex = await Assert.ThrowsExceptionAsync<InvalidDataException>(async () => await this.processor.SaveNodeConnectionProductOwnersAsync(productOwners).ConfigureAwait(false), "Exception should be thrown while update operation if ownership percentages sum is not 100.0.").ConfigureAwait(false);
            Assert.AreEqual(ex.Message, Entities.Constants.ProductOwnerShipTotalValueValidation);
            locRepository.Verify(y => y.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeConnectionProduct, bool>>>(), "Owners"), Times.Once);
            this.mockUnitOfWorkFactory.Verify(x => x.GetUnitOfWork().CreateRepository<NodeConnectionProduct>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Never);
        }

        /// <summary>
        /// Get graphicalNetwork should return success asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetGraphicalNetworkAsync_ShouldReturnSuccessAsync()
        {
            IEnumerable<QueryEntity.GraphicalNode> graphicalNode = new List<QueryEntity.GraphicalNode>();
            var repoGraphicalNodeMock = new Mock<IRepository<QueryEntity.GraphicalNode>>();
            repoGraphicalNodeMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(graphicalNode);
            this.mockFactory.Setup(m => m.CreateRepository<QueryEntity.GraphicalNode>()).Returns(repoGraphicalNodeMock.Object);

            IEnumerable<QueryEntity.GraphicalNodeConnection> graphicalNodeConnection = new List<QueryEntity.GraphicalNodeConnection>();
            var repoGraphicalNodeConnectioneMock = new Mock<IRepository<QueryEntity.GraphicalNodeConnection>>();
            repoGraphicalNodeConnectioneMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(graphicalNodeConnection);
            this.mockFactory.Setup(m => m.CreateRepository<QueryEntity.GraphicalNodeConnection>()).Returns(repoGraphicalNodeConnectioneMock.Object);

            var result = await this.processor.GetGraphicalNetworkAsync(1, 1).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.mockFactory.Verify(m => m.CreateRepository<QueryEntity.GraphicalNode>(), Times.Once);
            repoGraphicalNodeMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);

            Assert.IsNotNull(result);
            this.mockFactory.Verify(m => m.CreateRepository<QueryEntity.GraphicalNodeConnection>(), Times.Once);
            repoGraphicalNodeConnectioneMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Get graphicalNetwork destinationNodes details by SourceNodeIdAsync should return success asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetGraphicalNetworkDestinationNodesDetailsBySourceNodeIdAsync_ShouldReturnSuccessAsync()
        {
            IEnumerable<QueryEntity.GraphicalNode> graphicalNode = new List<QueryEntity.GraphicalNode>();
            var repoGraphicalNodeMock = new Mock<IRepository<QueryEntity.GraphicalNode>>();
            repoGraphicalNodeMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(graphicalNode);
            this.mockFactory.Setup(m => m.CreateRepository<QueryEntity.GraphicalNode>()).Returns(repoGraphicalNodeMock.Object);

            IEnumerable<QueryEntity.GraphicalNodeConnection> graphicalNodeConnection = new List<QueryEntity.GraphicalNodeConnection>();
            var repoGraphicalNodeConnectioneMock = new Mock<IRepository<QueryEntity.GraphicalNodeConnection>>();
            repoGraphicalNodeConnectioneMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(graphicalNodeConnection);
            this.mockFactory.Setup(m => m.CreateRepository<QueryEntity.GraphicalNodeConnection>()).Returns(repoGraphicalNodeConnectioneMock.Object);

            var result = await this.processor.GetGraphicalNetworkDestinationNodesDetailsBySourceNodeIdAsync(1).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.mockFactory.Verify(m => m.CreateRepository<QueryEntity.GraphicalNode>(), Times.Once);
            repoGraphicalNodeMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);

            Assert.IsNotNull(result);
            this.mockFactory.Verify(m => m.CreateRepository<QueryEntity.GraphicalNodeConnection>(), Times.Once);
            repoGraphicalNodeConnectioneMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Get graphicalNetwork destinationNodes details by SourceNodeIdAsync and SelfNodeConnection should return success asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetGraphicalNetworkDestinationNodesDetailsBySourceNodeIdSelfNodeConnectionAsync_ShouldReturnSuccessAsync()
        {
            List<QueryEntity.GraphicalNode> graphicalNode = new List<QueryEntity.GraphicalNode>();
            graphicalNode.Add(new QueryEntity.GraphicalNode()
            {
                NodeId = 1192,
                NodeName = "Test1192",
            });
            graphicalNode.Add(new QueryEntity.GraphicalNode()
            {
                NodeId = 1430,
                NodeName = "Test1430",
            });

            List<QueryEntity.GraphicalNodeConnection> graphicalNodeConnections = new List<QueryEntity.GraphicalNodeConnection>();
            graphicalNodeConnections.Add(new QueryEntity.GraphicalNodeConnection()
            {
                SourceNodeId = 1192,
                DestinationNodeId = 1430,
            });
            graphicalNodeConnections.Add(new QueryEntity.GraphicalNodeConnection()
            {
                SourceNodeId = 1430,
                DestinationNodeId = 1192,
            });
            graphicalNodeConnections.Add(new QueryEntity.GraphicalNodeConnection()
            {
                SourceNodeId = 1430,
                DestinationNodeId = 1430,
            });

            List<QueryEntity.GraphicalNodeConnection> expectedGraphicalNodeConnections = new List<QueryEntity.GraphicalNodeConnection>();
            expectedGraphicalNodeConnections.Add(new QueryEntity.GraphicalNodeConnection()
            {
                SourceNodeId = 1430,
                DestinationNodeId = 1192,
            });
            expectedGraphicalNodeConnections.Add(new QueryEntity.GraphicalNodeConnection()
            {
                SourceNodeId = 1430,
                DestinationNodeId = 1430,
            });

            var repoGraphicalNodeMock = new Mock<IRepository<QueryEntity.GraphicalNode>>();
            repoGraphicalNodeMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(graphicalNode.AsEnumerable());
            this.mockFactory.Setup(m => m.CreateRepository<QueryEntity.GraphicalNode>()).Returns(repoGraphicalNodeMock.Object);

            var repoGraphicalNodeConnectioneMock = new Mock<IRepository<QueryEntity.GraphicalNodeConnection>>();
            repoGraphicalNodeConnectioneMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(graphicalNodeConnections.AsEnumerable());
            this.mockFactory.Setup(m => m.CreateRepository<QueryEntity.GraphicalNodeConnection>()).Returns(repoGraphicalNodeConnectioneMock.Object);

            var result = await this.processor.GetGraphicalNetworkDestinationNodesDetailsBySourceNodeIdAsync(1430).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.mockFactory.Verify(m => m.CreateRepository<QueryEntity.GraphicalNode>(), Times.Once);
            repoGraphicalNodeMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);

            Assert.IsNotNull(result);
            this.mockFactory.Verify(m => m.CreateRepository<QueryEntity.GraphicalNodeConnection>(), Times.Once);
            repoGraphicalNodeConnectioneMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);

            Assert.IsTrue(result.GraphicalNodes.Count() == graphicalNode.Count);
            Assert.IsTrue(result.GraphicalNodeConnections.Count() == expectedGraphicalNodeConnections.Count);
        }

        /// <summary>
        /// Get graphicalNetwork sourceNodes details by SourceNodeIdAsync should return success asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetGraphicalNetworkSourceNodesDetailsByDestinationNodeIdAsync_ShouldReturnSuccessAsync()
        {
            IEnumerable<QueryEntity.GraphicalNode> graphicalNode = new List<QueryEntity.GraphicalNode>();
            var repoGraphicalNodeMock = new Mock<IRepository<QueryEntity.GraphicalNode>>();
            repoGraphicalNodeMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(graphicalNode);
            this.mockFactory.Setup(m => m.CreateRepository<QueryEntity.GraphicalNode>()).Returns(repoGraphicalNodeMock.Object);

            IEnumerable<QueryEntity.GraphicalNodeConnection> graphicalNodeConnection = new List<QueryEntity.GraphicalNodeConnection>();
            var repoGraphicalNodeConnectioneMock = new Mock<IRepository<QueryEntity.GraphicalNodeConnection>>();
            repoGraphicalNodeConnectioneMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(graphicalNodeConnection);
            this.mockFactory.Setup(m => m.CreateRepository<QueryEntity.GraphicalNodeConnection>()).Returns(repoGraphicalNodeConnectioneMock.Object);

            var result = await this.processor.GetGraphicalNetworkSourceNodesDetailsByDestinationNodeIdAsync(1).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.mockFactory.Verify(m => m.CreateRepository<QueryEntity.GraphicalNode>(), Times.Once);
            repoGraphicalNodeMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);

            Assert.IsNotNull(result);
            this.mockFactory.Verify(m => m.CreateRepository<QueryEntity.GraphicalNodeConnection>(), Times.Once);
            repoGraphicalNodeConnectioneMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Get graphicalNetwork sourceNodes details by destinationNodeIdAsync and self node connections should return success asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetGraphicalNetworkSourceNodesDetailsByDestinationNodeIdSelfNodeConnectionAsync_ShouldReturnSuccessAsync()
        {
            List<QueryEntity.GraphicalNode> graphicalNode = new List<QueryEntity.GraphicalNode>();
            graphicalNode.Add(new QueryEntity.GraphicalNode()
            {
                NodeId = 1192,
                NodeName = "Test1192",
            });
            graphicalNode.Add(new QueryEntity.GraphicalNode()
            {
                NodeId = 1283,
                NodeName = "Test1283",
            });
            graphicalNode.Add(new QueryEntity.GraphicalNode()
            {
                NodeId = 1430,
                NodeName = "Test1430",
            });

            List<QueryEntity.GraphicalNodeConnection> graphicalNodeConnections = new List<QueryEntity.GraphicalNodeConnection>();
            graphicalNodeConnections.Add(new QueryEntity.GraphicalNodeConnection()
            {
                SourceNodeId = 1192,
                DestinationNodeId = 1430,
            });
            graphicalNodeConnections.Add(new QueryEntity.GraphicalNodeConnection()
            {
                SourceNodeId = 1283,
                DestinationNodeId = 1430,
            });
            graphicalNodeConnections.Add(new QueryEntity.GraphicalNodeConnection()
            {
                SourceNodeId = 1430,
                DestinationNodeId = 1192,
            });
            graphicalNodeConnections.Add(new QueryEntity.GraphicalNodeConnection()
            {
                SourceNodeId = 1430,
                DestinationNodeId = 1430,
            });

            List<QueryEntity.GraphicalNodeConnection> expectedGraphicalNodeConnections = new List<QueryEntity.GraphicalNodeConnection>();
            expectedGraphicalNodeConnections.Add(new QueryEntity.GraphicalNodeConnection()
            {
                SourceNodeId = 1192,
                DestinationNodeId = 1430,
            });
            expectedGraphicalNodeConnections.Add(new QueryEntity.GraphicalNodeConnection()
            {
                SourceNodeId = 1283,
                DestinationNodeId = 1430,
            });
            expectedGraphicalNodeConnections.Add(new QueryEntity.GraphicalNodeConnection()
            {
                SourceNodeId = 1430,
                DestinationNodeId = 1430,
            });

            var repoGraphicalNodeMock = new Mock<IRepository<QueryEntity.GraphicalNode>>();
            repoGraphicalNodeMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(graphicalNode.AsEnumerable());
            this.mockFactory.Setup(m => m.CreateRepository<QueryEntity.GraphicalNode>()).Returns(repoGraphicalNodeMock.Object);

            var repoGraphicalNodeConnectioneMock = new Mock<IRepository<QueryEntity.GraphicalNodeConnection>>();
            repoGraphicalNodeConnectioneMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(graphicalNodeConnections.AsEnumerable());
            this.mockFactory.Setup(m => m.CreateRepository<QueryEntity.GraphicalNodeConnection>()).Returns(repoGraphicalNodeConnectioneMock.Object);

            var result = await this.processor.GetGraphicalNetworkSourceNodesDetailsByDestinationNodeIdAsync(1430).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.mockFactory.Verify(m => m.CreateRepository<QueryEntity.GraphicalNode>(), Times.Once);
            repoGraphicalNodeMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);

            Assert.IsNotNull(result);
            this.mockFactory.Verify(m => m.CreateRepository<QueryEntity.GraphicalNodeConnection>(), Times.Once);
            repoGraphicalNodeConnectioneMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);

            Assert.IsTrue(result.GraphicalNodes.Count() == graphicalNode.Count);
            Assert.IsTrue(result.GraphicalNodeConnections.Count() == expectedGraphicalNodeConnections.Count);
        }

        /// <summary>
        /// Create node connection list asyncrhonously should throw argument null exception when the node connection list is null.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CreateNodeConnectionListAsync_ShouldThrowArgumentNullException_WhenNodeConnectionListIsNullAsync()
        {
            // Prepare
            Mock<IRepository<NodeConnection>> repoMock = this.InitializeRepositoryForNullAndVoidTests();
            var nullList = default(List<NodeConnection>);

            // Execute
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                async () => await this.processor.CreateNodeConnectionListAsync(nullList).ConfigureAwait(false))
                .ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<NodeConnection>(), Times.Never);
            this.mockFactory.Verify(m => m.CreateRepository<NodeConnection>(), Times.Never);

            repoMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>()), Times.Never);
            repoMock.Verify(r => r.Insert(It.IsAny<NodeConnection>()), Times.Never);
            repoMock.Verify(r => r.Update(It.IsAny<NodeConnection>()), Times.Never);
            this.mockAzureClientFactory.Verify(
                a => a.GetQueueClient(QueueConstants.BlockchainNodeConnectionQueue),
                Times.Never);
            this.mockAzureClientFactory.Verify(
                a => a.GetQueueClient(QueueConstants.BlockchainNodeConnectionQueue)
                    .QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()),
                Times.Never);
        }

        /// <summary>
        /// Create node connection list asyncrhonously should throw argument null exception when the node connection list is void.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CreateNodeConnectionListAsync_ShouldThrowArgumentNullException_WhenNodeConnectionListIsVoidAsync()
        {
            // Prepare
            Mock<IRepository<NodeConnection>> repoMock = this.InitializeRepositoryForNullAndVoidTests();
            var voidList = new List<NodeConnection>();

            // Execute
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                async () => await this.processor.CreateNodeConnectionListAsync(voidList).ConfigureAwait(false))
                .ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<NodeConnection>(), Times.Never);
            this.mockFactory.Verify(m => m.CreateRepository<NodeConnection>(), Times.Never);

            repoMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>()), Times.Never);
            repoMock.Verify(r => r.Insert(It.IsAny<NodeConnection>()), Times.Never);
            repoMock.Verify(r => r.Update(It.IsAny<NodeConnection>()), Times.Never);
            this.mockAzureClientFactory.Verify(
                a => a.GetQueueClient(QueueConstants.BlockchainNodeConnectionQueue),
                Times.Never);
            this.mockAzureClientFactory.Verify(
                a => a.GetQueueClient(QueueConstants.BlockchainNodeConnectionQueue)
                    .QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()),
                Times.Never);
        }

        /// <summary>
        /// Create node connection list asyncrhonously should .
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CreateNodeConnectionListAsync_ShouldCreateNodeConnectionListAsync()
        {
            // Prepare
            var nodeConnectionlist = new List<NodeConnection>
            {
                new NodeConnection { NodeConnectionId = 1, SourceNodeId = 1 },
                new NodeConnection { NodeConnectionId = 2, SourceNodeId = 1 },
                new NodeConnection { NodeConnectionId = 3, SourceNodeId = 1 },
            };
            var token = new CancellationToken(false);
            this.SetupNodeStorageLocationRepository();
            var nodeConnectionRepoMock = this.SetupNodeConnectionRepositoryForInsert(token);

            // Execute
            var sut = await this.processor.CreateNodeConnectionListAsync(nodeConnectionlist).ConfigureAwait(false);

            // Assert
            Assert.IsInstanceOfType(sut, typeof(IEnumerable<NodeConnectionInfo>));
            Assert.AreEqual(3, sut.Count(), "Should return as many results as NodeConnecions were suplied");
            this.mockUnitOfWorkFactory.Verify(
                m => m.GetUnitOfWork().CreateRepository<NodeConnection>(),
                Times.Once);
            this.mockUnitOfWorkFactory.Verify(
                m => m.GetUnitOfWork().SaveAsync(token),
                Times.Exactly(nodeConnectionlist.Count));
            nodeConnectionRepoMock.Verify(
                r => r.Insert(It.IsAny<NodeConnection>()),
                Times.Exactly(nodeConnectionlist.Count));
            nodeConnectionRepoMock.Verify(
                r => r.Update(It.IsAny<NodeConnection>()),
                Times.Never);
            this.mockAzureClientFactory.Verify(
                a => a.GetQueueClient(QueueConstants.BlockchainNodeConnectionQueue),
                Times.Exactly(nodeConnectionlist.Count));
            this.mockAzureClientFactory.Verify(
                a => a.GetQueueClient(QueueConstants.BlockchainNodeConnectionQueue)
                    .QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()),
                Times.Exactly(nodeConnectionlist.Count));
        }

        /// <summary>
        /// Create node connection list asyncrhonously should .
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CreateNodeConnectionListAsync_ShouldReturnInfoWithDuplicatedStatus_WhenNodeConnectionsExistsAsync()
        {
            // Prepare
            var nodeConnectionlist = new List<NodeConnection>
            {
                new NodeConnection { NodeConnectionId = 1, SourceNodeId = 1, DestinationNodeId = 3, IsActive = true },
                new NodeConnection { NodeConnectionId = 2, SourceNodeId = 2, DestinationNodeId = 3, IsActive = true },
                new NodeConnection { NodeConnectionId = 3, SourceNodeId = 3, DestinationNodeId = 3, IsActive = true },
            };

            var token = new CancellationToken(false);
            this.SetupNodeStorageLocationRepository();
            var nodeConnectionRepoMock = this.SetupNodeConnectionRepositoryForInsert(token);
            SetupNodeConnectionRepositoryForQuerying(nodeConnectionlist, nodeConnectionRepoMock);
            this.SetupOffchainConnectionRepo();

            // Execute
            var sut = await this.processor.CreateNodeConnectionListAsync(nodeConnectionlist).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(sut.All(n => n.Status == EntityInfoCreationStatus.Duplicated));
            this.mockUnitOfWorkFactory.Verify(
                m => m.GetUnitOfWork().CreateRepository<NodeConnection>(),
                Times.Once);
            this.mockUnitOfWorkFactory.Verify(
                m => m.GetUnitOfWork().SaveAsync(token),
                Times.Never);
            nodeConnectionRepoMock.Verify(
                r => r.Insert(It.IsAny<NodeConnection>()),
                Times.Never);
            nodeConnectionRepoMock.Verify(
                r => r.Update(It.IsAny<NodeConnection>()),
                Times.Never);
            this.mockAzureClientFactory.Verify(
                a => a.GetQueueClient(QueueConstants.BlockchainNodeConnectionQueue),
                Times.Never);
            this.mockAzureClientFactory.Verify(
                a => a.GetQueueClient(QueueConstants.BlockchainNodeConnectionQueue)
                    .QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()),
                Times.Never);
        }

        /// <summary>
        /// Create node connection list asyncrhonously should .
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CreateNodeConnectionListAsync_ShouldActivateNodeConenctions_WhenInactiveNodeConnectionsArePassedAsync()
        {
            // Prepare
            var nodeConnectionlist = new List<NodeConnection>
            {
                new NodeConnection { NodeConnectionId = 1, SourceNodeId = 3, DestinationNodeId = 3, IsActive = false },
                new NodeConnection { NodeConnectionId = 2, SourceNodeId = 2, DestinationNodeId = 3, IsActive = false },
                new NodeConnection { NodeConnectionId = 3, SourceNodeId = 1, DestinationNodeId = 3, IsActive = false },
            };
            var token = new CancellationToken(false);
            var nodeConnectionRepoMock = this.SetupNodeConnectionRepositoryForInsert(token);
            this.SetupNodeStorageLocationRepository();
            SetupNodeConnectionRepositoryForQuerying(nodeConnectionlist, nodeConnectionRepoMock);
            this.SetupOffchainConnectionRepo();

            // Execute
            await this.processor.CreateNodeConnectionListAsync(nodeConnectionlist).ConfigureAwait(false);

            // Assert
            this.mockUnitOfWorkFactory.Verify(
                m => m.GetUnitOfWork().CreateRepository<NodeConnection>(),
                Times.Once);
            this.mockUnitOfWorkFactory.Verify(
                m => m.GetUnitOfWork().SaveAsync(token),
                Times.Exactly(nodeConnectionlist.Count));
            nodeConnectionRepoMock.Verify(
                r => r.Insert(It.IsAny<NodeConnection>()),
                Times.Never);
            nodeConnectionRepoMock.Verify(
                r => r.Update(It.Is<NodeConnection>(n => n.IsActive.GetValueOrDefault())),
                Times.Exactly(nodeConnectionlist.Count));
            this.mockAzureClientFactory.Verify(
                a => a.GetQueueClient(QueueConstants.BlockchainNodeConnectionQueue),
                Times.Exactly(nodeConnectionlist.Count));
            this.mockAzureClientFactory.Verify(
                a => a.GetQueueClient(QueueConstants.BlockchainNodeConnectionQueue)
                    .QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()),
                Times.Exactly(nodeConnectionlist.Count));
        }

        private static void SetupNodeConnectionRepositoryForQuerying(List<NodeConnection> nodeConnectionlist, Mock<IRepository<NodeConnection>> nodeConnectionRepoMock)
        {
            foreach (var nodeConnection in nodeConnectionlist)
            {
                var sourceId = nodeConnection.SourceNodeId;
                var destId = nodeConnection.DestinationNodeId;
                nodeConnectionRepoMock
                    .Setup(
                    r => r.SingleOrDefaultAsync(
                        x => x.SourceNodeId == sourceId
                        && x.DestinationNodeId == destId
                        && !x.IsDeleted, It.IsAny<string[]>()))
                    .Returns(Task.FromResult(nodeConnection));
            }
        }

        private void SetupOffchainConnectionRepo()
        {
            var offchainRepoMock = new Mock<IRepository<OffchainNodeConnection>>();
            offchainRepoMock.Setup(r => r.Insert(It.IsAny<OffchainNodeConnection>()));
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<OffchainNodeConnection>()).Returns(offchainRepoMock.Object);
        }

        private Mock<IRepository<NodeConnection>> SetupNodeConnectionRepositoryForInsert(CancellationToken token)
        {
            var nodeConnectionRepoMock = new Mock<IRepository<NodeConnection>>();
            nodeConnectionRepoMock.Setup(r => r.Insert(It.IsAny<NodeConnection>()));
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<NodeConnection>()).Returns(nodeConnectionRepoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));

            return nodeConnectionRepoMock;
        }

        private void SetupNodeStorageLocationRepository()
        {
            var nodeStorageLocations = new List<NodeStorageLocation>();

            var nodeStorageLocationRepoMock = new Mock<IRepository<NodeStorageLocation>>();
            nodeStorageLocationRepoMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<NodeStorageLocation, bool>>>())).Returns(Task.FromResult<IEnumerable<NodeStorageLocation>>(nodeStorageLocations));
            this.mockFactory.Setup(x => x.CreateRepository<NodeStorageLocation>()).Returns(nodeStorageLocationRepoMock.Object);
        }

        private Mock<IRepository<NodeConnection>> InitializeRepositoryForNullAndVoidTests()
        {
            var repoMock = new Mock<IRepository<NodeConnection>>();
            repoMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>()))
                .Returns(Task.FromResult(default(NodeConnection)));
            this.mockFactory
                .Setup(f => f.CreateRepository<NodeConnection>())
                .Returns(repoMock.Object);
            return repoMock;
        }
    }
}
