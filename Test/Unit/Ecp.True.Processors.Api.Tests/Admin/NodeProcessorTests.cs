// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeProcessorTests.cs" company="Microsoft">
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
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Api.Admin;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Proxies.Azure;
    using EfCore.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The node processor tests.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Api.Tests.ProcessorTestBase" />
    [TestClass]
    public class NodeProcessorTests : ProcessorTestBase
    {
        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The processor.
        /// </summary>
        private NodeProcessor processor;

        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockFactory;

        /// <summary>
        /// The mock business context.
        /// </summary>
        private Mock<IBusinessContext> mockBusinessContext;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The mock node order manager.
        /// </summary>
        private Mock<INodeOrderManager> mockNodeOrderManager;

        /// <summary>
        /// The mock category element.
        /// </summary>
        private Mock<IRepository<CategoryElement>> catMock;

        /// <summary>
        /// The configuration handler mock.
        /// </summary>
        private Mock<IConfigurationHandler> configurationHandlerMock;

        /// <summary>
        /// The configuration handler mock.
        /// </summary>
        private Mock<IConfigurationHandler> configurationHandlerMockSystemConfig;

        /// <summary>
        /// The system config.
        /// </summary>
        private SystemSettings systemConfig;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.systemConfig = new SystemSettings { NodeTagGracePeriodInMonths = 6 };
            this.mockFactory = new Mock<IRepositoryFactory>();
            this.catMock = new Mock<IRepository<CategoryElement>>();
            this.mockBusinessContext = new Mock<IBusinessContext>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.configurationHandlerMock = new Mock<IConfigurationHandler>();
            this.configurationHandlerMockSystemConfig = new Mock<IConfigurationHandler>();
            var serviceBusQueueClient = new Mock<IServiceBusQueueClient>();
            this.mockNodeOrderManager = new Mock<INodeOrderManager>();
            this.mockAzureClientFactory.Setup(x => x.GetQueueClient(It.IsAny<string>())).Returns(serviceBusQueueClient.Object);
            this.configurationHandlerMock.Setup(x => x.GetConfigurationAsync<ServiceBusSettings>(It.IsAny<string>())).ReturnsAsync(new ServiceBusSettings());
            this.configurationHandlerMockSystemConfig.Setup(m => m.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings)).ReturnsAsync(this.systemConfig);
            this.processor = new NodeProcessor(this.mockUnitOfWorkFactory.Object, this.mockBusinessContext.Object, this.mockFactory.Object, this.mockAzureClientFactory.Object, this.configurationHandlerMock.Object, this.mockNodeOrderManager.Object);
            }

        /// <summary>
        /// Checks the node name exists should return true for node name exists in repository when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task CheckNodeNameExists_ShouldReturnTrueForNodeNameExistsInRepository_WhenInvokedAsync()
        {
            var node = new Node
            {
                NodeId = 1,
                Name = "Node One",
            };
            var repoMock = new Mock<IRepository<Node>>();
            repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>(), "LogisticCenter", "LogisticCenter.StorageLocations")).ReturnsAsync(node);
            this.mockFactory.Setup(m => m.CreateRepository<Node>()).Returns(repoMock.Object);

            var result = await this.processor.GetNodeByNameAsync(node.Name).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.mockFactory.Verify(m => m.CreateRepository<Node>(), Times.Once);
        }

        /// <summary>
        /// Checks the node name exists should return false for node name exists in repository when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task CheckNodeNameExists_ShouldReturnFalseForNodeNameExistsInRepository_WhenInvokedAsync()
        {
            var node = new Node
            {
                NodeId = 1,
                Name = "Node One",
            };
            var repoMock = new Mock<IRepository<Node>>();
            repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(() => null);
            this.mockFactory.Setup(m => m.CreateRepository<Node>()).Returns(repoMock.Object);

            var result = await this.processor.GetNodeByNameAsync(node.Name).ConfigureAwait(false);

            Assert.IsNull(result);
            this.mockFactory.Verify(m => m.CreateRepository<Node>(), Times.Once);
        }

        /// <summary>
        /// Nodes the storage name exists should return true for node storage name exists in repository when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task NodeStorageNameExists_ShouldReturnTrueForNodeStorageNameExistsInRepository_WhenInvokedAsync()
        {
            var nodeStorageLocation = new NodeStorageLocation
            {
                NodeId = 1,
                Name = "Location One",
            };
            var repoMock = new Mock<IRepository<NodeStorageLocation>>();
            repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeStorageLocation, bool>>>())).ReturnsAsync(nodeStorageLocation);
            this.mockFactory.Setup(m => m.CreateRepository<NodeStorageLocation>()).Returns(repoMock.Object);

            var result = await this.processor.GetStorageLocationByNameAsync(nodeStorageLocation.Name, nodeStorageLocation.NodeId).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.mockFactory.Verify(m => m.CreateRepository<NodeStorageLocation>(), Times.Once);
        }

        /// <summary>
        /// Nodes the storage name exists should return false for node storage name exists in repository when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task NodeStorageNameExists_ShouldReturnFalseForNodeStorageNameExistsInRepository_WhenInvokedAsync()
        {
            var nodeStorageLocation = new NodeStorageLocation
            {
                NodeId = 1,
                Name = "Location One",
            };
            var repoMock = new Mock<IRepository<NodeStorageLocation>>();
            repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeStorageLocation, bool>>>())).ReturnsAsync(() => null);
            this.mockFactory.Setup(m => m.CreateRepository<NodeStorageLocation>()).Returns(repoMock.Object);

            var result = await this.processor.GetStorageLocationByNameAsync(nodeStorageLocation.Name, nodeStorageLocation.NodeId).ConfigureAwait(false);

            Assert.IsNull(result);
            this.mockFactory.Verify(m => m.CreateRepository<NodeStorageLocation>(), Times.Once);
        }

        /// <summary>
        /// Query all asynchronous should query from repository when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task QueryAllAsync_ShouldQueryFromRepository_WhenInvokedAsync()
        {
            var products = new[] { new Node() }.AsQueryable();
            var repoMock = new Mock<IRepository<Node>>();
            repoMock.Setup(r => r.QueryAllAsync(x => x.IsActive == true)).ReturnsAsync(products);
            this.mockFactory.Setup(m => m.CreateRepository<Node>()).Returns(repoMock.Object);

            var result = await this.processor.QueryAllAsync<Node>(x => x.IsActive == true).ConfigureAwait(false);

            Assert.AreEqual(result, products);
            repoMock.Verify(m => m.QueryAllAsync(x => x.IsActive == true), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<Node>(), Times.Once);
        }

        /// <summary>
        /// Saves the node asynchronous should create node when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SaveNodeAsync_ShouldCreateNode_WhenInvokedAsync()
        {
            var productLocation = new StorageLocationProduct { IsActive = true, ProductId = "10000002049" };

            var nodeStorageLocation = new NodeStorageLocation { IsActive = true, Name = "North-Storage-Location-51", StorageLocationId = "1000:M001" };
            nodeStorageLocation.Products.Add(productLocation);

            var node = new Node { IsActive = true, Name = "North-7", SendToSap = true, NodeTypeId = 1, OperatorId = 3, SegmentId = 2, Order = 2, AutoOrder = true };
            node.NodeStorageLocations.Add(nodeStorageLocation);

            var catElement1 = new CategoryElement { ElementId = 1, CategoryId = 1 };

            this.systemConfig = new SystemSettings { NodeTagGracePeriodInMonths = 6 };
            this.configurationHandlerMock.Setup(m => m.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings)).ReturnsAsync(this.systemConfig);

            var repoMock = new Mock<IRepository<Node>>();
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<Node>()).Returns(repoMock.Object);

            var nodeTagRepoMock = new Mock<IRepository<NodeTag>>();
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<NodeTag>()).Returns(nodeTagRepoMock.Object);

            this.catMock = new Mock<IRepository<CategoryElement>>();
            this.catMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(catElement1);

            this.mockFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(this.catMock.Object);

            await this.processor.SaveNodeAsync(node).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(x => x.GetUnitOfWork().CreateRepository<NodeTag>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(x => x.GetUnitOfWork().CreateRepository<Node>(), Times.Once);
        }

        /// <summary>
        /// Saves the node asynchronous should create node with Capacity and UnitId when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SaveNodeAsync_ShouldCreateNodeWithCapacityAndUnitId_WhenInvokedAsync()
        {
            var productLocation = new StorageLocationProduct { IsActive = true, ProductId = "10000002049" };

            var nodeStorageLocation = new NodeStorageLocation { IsActive = true, Name = "North-Storage-Location-51", StorageLocationId = "1000:M001" };
            nodeStorageLocation.Products.Add(productLocation);

            var node = new Node { IsActive = true, NodeId = 10, Name = "North-7", SendToSap = true, NodeTypeId = 1, OperatorId = 3, SegmentId = 2, Order = 2, AutoOrder = true, UnitId = 31, Capacity = 11 };
            node.NodeStorageLocations.Add(nodeStorageLocation);

            var catElement1 = new CategoryElement { ElementId = 1, CategoryId = 1 };

            this.systemConfig = new SystemSettings { NodeTagGracePeriodInMonths = 6 };
            this.configurationHandlerMock.Setup(m => m.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings)).ReturnsAsync(this.systemConfig);

            var repoMock = new Mock<IRepository<Node>>();
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<Node>()).Returns(repoMock.Object);

            var nodeTagRepoMock = new Mock<IRepository<NodeTag>>();
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<NodeTag>()).Returns(nodeTagRepoMock.Object);

            this.catMock = new Mock<IRepository<CategoryElement>>();
            this.catMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(catElement1);

            this.mockFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(this.catMock.Object);

            await this.processor.SaveNodeAsync(node).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(x => x.GetUnitOfWork().CreateRepository<NodeTag>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(x => x.GetUnitOfWork().CreateRepository<Node>(), Times.Once);

            Assert.AreEqual(1, node.OffchainNodes.Count);
            Assert.AreEqual(node.NodeId, node.OffchainNodes.First().NodeId);
            Assert.AreEqual(node.Name, node.OffchainNodes.First().Name);
            Assert.AreEqual(node.IsActive, node.OffchainNodes.First().IsActive);
            Assert.AreEqual((int)NodeState.CreatedNode, node.OffchainNodes.First().NodeStateTypeId);
        }

        /// <summary>
        /// Saves the node asynchronous should create node with Capacity and UnitId when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SaveNodeAsync_ShouldCreateNodeWithStartDateMinusSixMonths_WhenInvokedAsync()
        {
            var productLocation = new StorageLocationProduct { IsActive = true, ProductId = "10000002049" };

            var nodeStorageLocation = new NodeStorageLocation { IsActive = true, Name = "North-Storage-Location-51", StorageLocationId = "1000:M001" };
            nodeStorageLocation.Products.Add(productLocation);

            var nodeTag = new NodeTag { NodeTagId = 1, StartDate = DateTime.UtcNow.Date.AddMonths(-6) };

            var node = new Node { NodeId = 1, IsActive = true, Name = "North-7", SendToSap = true, NodeTypeId = 1, OperatorId = 3, SegmentId = 2, Order = 2, AutoOrder = true, UnitId = 31, Capacity = 11 };
            node.NodeStorageLocations.Add(nodeStorageLocation);
            node.NodeTags.Add(nodeTag);

            var catElement1 = new CategoryElement { ElementId = 1, CategoryId = 1 };

            var repoMock = new Mock<IRepository<Node>>();
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<Node>()).Returns(repoMock.Object);

            this.systemConfig = new SystemSettings { NodeTagGracePeriodInMonths = 6 };
            this.configurationHandlerMock.Setup(m => m.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings)).ReturnsAsync(this.systemConfig);

            var nodeTagRepoMock = new Mock<IRepository<NodeTag>>();
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<NodeTag>()).Returns(nodeTagRepoMock.Object);

            this.catMock = new Mock<IRepository<CategoryElement>>();
            this.catMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(catElement1);

            this.mockFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(this.catMock.Object);

            await this.processor.SaveNodeAsync(node).ConfigureAwait(false);

            repoMock.Setup(r => r.GetByIdAsync(node.NodeId)).ReturnsAsync(node);
            this.mockFactory.Setup(m => m.CreateRepository<Node>()).Returns(repoMock.Object);

            var result = await this.processor.GetNodeByIdAsync(node.NodeId).ConfigureAwait(false);
            var nodeTagResult = result.NodeTags.FirstOrDefault();
            var newStartDate = DateTime.UtcNow.Date.AddMonths(-this.systemConfig.NodeTagGracePeriodInMonths.GetValueOrDefault());

            Assert.AreEqual(nodeTagResult.StartDate, newStartDate);
            this.mockUnitOfWorkFactory.Verify(x => x.GetUnitOfWork().CreateRepository<NodeTag>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(x => x.GetUnitOfWork().CreateRepository<Node>(), Times.Once);
        }

        [TestMethod]
        public async Task UpdateNodeAsync_ShouldUpdateNode_WhenInvokedAsync()
        {
            var owner = new StorageLocationProductOwner { };
            var storageLocationProductVariable = new StorageLocationProductVariable { };
            var productLocation = new StorageLocationProduct { IsActive = true, ProductId = "10000002049" };
            productLocation.Owners.Add(owner);
            productLocation.StorageLocationProductVariables.Add(storageLocationProductVariable);

            var nodeStorageLocation = new NodeStorageLocation { IsActive = true, Name = "North-Storage-Location-51", StorageLocationId = "1000:M001" };
            nodeStorageLocation.Products.Add(productLocation);

            var node = new Node { IsActive = true, AutoOrder = true, SegmentId = 10, Order = 1, Name = "Name", NodeId = 10, Capacity = 12, UnitId = 123 };
            node.NodeStorageLocations.Add(nodeStorageLocation);

            var nodeFromRepoCall = new List<Node>();

            var repoMock = new Mock<IRepository<Node>>();
            repoMock.Setup(y => y.SingleOrDefaultAsync(
                It.IsAny<Expression<Func<Node, bool>>>(),
                It.IsAny<string[]>())).ReturnsAsync(node);

            repoMock.Setup(y => y.Update(It.IsAny<Node>())).Callback<Node>(a => nodeFromRepoCall.Add(a));

            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<Node>()).Returns(repoMock.Object);

            var offchainRepoMock = new Mock<IRepository<OffchainNode>>();
            offchainRepoMock.Setup(x => x.Insert(It.IsAny<OffchainNode>()));
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<OffchainNode>()).Returns(offchainRepoMock.Object);

            var nodeConnections = new List<NodeConnection>()
            {
                new NodeConnection
                {
                    IsActive = true,
                },
            };

            var nodeConnectionRepoMock = new Mock<IRepository<NodeConnection>>();
            nodeConnectionRepoMock.Setup(y => y.GetAllAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>(), "Products", "Products.Owners")).ReturnsAsync(nodeConnections);
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<NodeConnection>()).Returns(nodeConnectionRepoMock.Object);

            await this.processor.UpdateNodeAsync(node).ConfigureAwait(false);

            Assert.IsTrue(nodeFromRepoCall.Count == 1);
            Assert.AreEqual(12, nodeFromRepoCall.First().Capacity);
            Assert.AreEqual(123, nodeFromRepoCall.First().UnitId);
            Assert.AreEqual("Name", nodeFromRepoCall.First().Name);
            Assert.AreEqual("Name", nodeFromRepoCall.First().Name);

            this.mockUnitOfWorkFactory.Verify(x => x.GetUnitOfWork().CreateRepository<Node>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(x => x.GetUnitOfWork().CreateRepository<OffchainNode>(), Times.Once);

            repoMock.Verify(y => y.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>(), It.IsAny<string[]>()), Times.Once);
            offchainRepoMock.Verify(x => x.Insert(It.Is<OffchainNode>(x => this.ValidateOffchainNode(x, node))), Times.Once);
        }

        /// <summary>
        /// Saves the node asynchronous should update node with Capacity and UnitId when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateNodeAsync_ShouldUpdateNodeWithCapacityAndUnitId_WhenInvokedAsync()
        {
            var owner = new StorageLocationProductOwner { };
            var storageLocationProductVariable = new StorageLocationProductVariable { };
            var productLocation = new StorageLocationProduct { IsActive = true, ProductId = "10000002049" };
            productLocation.Owners.Add(owner);
            productLocation.StorageLocationProductVariables.Add(storageLocationProductVariable);

            var nodeStorageLocation = new NodeStorageLocation { IsActive = true, Name = "North-Storage-Location-51", StorageLocationId = "1000:M001" };
            nodeStorageLocation.Products.Add(productLocation);

            var node = new Node { IsActive = true, AutoOrder = true, SegmentId = 10, NodeId = 10, Order = 1, Capacity = 11, UnitId = 31, Name = "Name" };
            node.NodeStorageLocations.Add(nodeStorageLocation);

            var repoMock = new Mock<IRepository<Node>>();
            repoMock.Setup(y => y.SingleOrDefaultAsync(
                It.IsAny<Expression<Func<Node, bool>>>(),
                It.IsAny<string[]>())).ReturnsAsync(node);
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<Node>()).Returns(repoMock.Object);

            var offchainRepoMock = new Mock<IRepository<OffchainNode>>();
            offchainRepoMock.Setup(x => x.Insert(It.IsAny<OffchainNode>()));
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<OffchainNode>()).Returns(offchainRepoMock.Object);

            var nodeConnections = new List<NodeConnection>()
            {
                new NodeConnection
                {
                    IsActive = true,
                },
            };

            var nodeConnectionRepoMock = new Mock<IRepository<NodeConnection>>();
            nodeConnectionRepoMock.Setup(y => y.GetAllAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>(), "Products", "Products.Owners")).ReturnsAsync(nodeConnections);
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<NodeConnection>()).Returns(nodeConnectionRepoMock.Object);

            await this.processor.UpdateNodeAsync(node).ConfigureAwait(false);

            repoMock.Verify(y => y.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockUnitOfWorkFactory.Verify(x => x.GetUnitOfWork().CreateRepository<Node>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(x => x.GetUnitOfWork().CreateRepository<OffchainNode>(), Times.Once);

            offchainRepoMock.Verify(x => x.Insert(It.Is<OffchainNode>(x => this.ValidateOffchainNode(x, node))), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task UpdateNodeAsync_ShouldRaiseException_WhenInvokedAsync()
        {
            var productLocation = new StorageLocationProduct { IsActive = true, ProductId = "10000002049" };

            var nodeStorageLocation = new NodeStorageLocation { IsActive = true, Name = "North-Storage-Location-51", StorageLocationId = "1000:M001" };
            nodeStorageLocation.Products.Add(productLocation);

            var node = new Node { IsActive = true, AutoOrder = true };
            node.NodeStorageLocations.Add(nodeStorageLocation);

            var repoMock = new Mock<IRepository<Node>>();
            Node nodeDetail = null;
            repoMock.Setup(y => y.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>(), "NodeStorageLocations", "NodeStorageLocations.Products", "NodeStorageLocations.Products.Owners")).ReturnsAsync(nodeDetail);
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<Node>()).Returns(repoMock.Object);

            var nodeConnections = new List<NodeConnection>()
            {
                new NodeConnection
                {
                    IsActive = true,
                },
            };

            var nodeConnectionRepoMock = new Mock<IRepository<NodeConnection>>();
            nodeConnectionRepoMock.Setup(y => y.GetAllAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>(), "Products", "Products.Owners")).ReturnsAsync(nodeConnections);
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<NodeConnection>()).Returns(nodeConnectionRepoMock.Object);

            await this.processor.UpdateNodeAsync(node).ConfigureAwait(false);

            repoMock.Verify(y => y.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>(), "NodeStorageLocations", "NodeStorageLocations.Products", "NodeStorageLocations.Products.Owners"), Times.Once);
            this.mockUnitOfWorkFactory.Verify(x => x.GetUnitOfWork().CreateRepository<Node>(), Times.Once);
        }

        /// <summary>
        /// Gets the node by identifier asynchronous should query from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetNodeByIdAsync_ShouldQueryFromRepository_WhenInvokedAsync()
        {
            var node = new Node { NodeId = 1, Name = "Node" };
            var repoMock = new Mock<IRepository<Node>>();
            repoMock.Setup(r => r.GetByIdAsync(node.NodeId)).ReturnsAsync(node);
            this.mockFactory.Setup(m => m.CreateRepository<Node>()).Returns(repoMock.Object);

            var result = await this.processor.GetNodeByIdAsync(node.NodeId).ConfigureAwait(false);

            Assert.AreEqual(result, node);
            repoMock.Verify(m => m.GetByIdAsync(node.NodeId), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<Node>(), Times.Once);
        }

        [TestMethod]
        public async Task UpdateNodePropertiesAsync_ShouldReturnSuccessAsync()
        {
            var node = new Node { IsActive = true, NodeId = 1, Name = "Node", ControlLimit = 2.1m, AcceptableBalancePercentage = 3.2m };

            var nodeRepository = new Mock<IRepository<Node>>();
            nodeRepository.Setup(y => y.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(node);
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<Node>()).Returns(nodeRepository.Object);

            var offchainRepoMock = new Mock<IRepository<OffchainNode>>();
            offchainRepoMock.Setup(x => x.Insert(It.IsAny<OffchainNode>()));
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<OffchainNode>()).Returns(offchainRepoMock.Object);

            await this.processor.UpdateNodePropertiesAsync(node).ConfigureAwait(false);

            nodeRepository.Verify(y => y.GetByIdAsync(It.IsAny<int>()), Times.Once);
            offchainRepoMock.Verify(x => x.Insert(It.Is<OffchainNode>(x => this.ValidateOffchainNode(x, node))), Times.Once);

            this.mockUnitOfWorkFactory.Verify(x => x.GetUnitOfWork().CreateRepository<Node>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(x => x.GetUnitOfWork().CreateRepository<OffchainNode>(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task UpdateNodePropertiesAsync_ShouldRaiseKeyNotFoundExceptionAsync()
        {
            var node = new Node { NodeId = 10 };
            Node returnNodeObject = null;
            var nodeRepository = new Mock<IRepository<Node>>();
            nodeRepository.Setup(y => y.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(returnNodeObject);
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<Node>()).Returns(nodeRepository.Object);
            await this.processor.UpdateNodePropertiesAsync(node).ConfigureAwait(false);

            nodeRepository.Verify(y => y.GetByIdAsync(It.IsAny<int>()), Times.Once);
            this.mockUnitOfWorkFactory.Verify(x => x.GetUnitOfWork().CreateRepository<Node>(), Times.Once);
        }

        [TestMethod]
        public async Task UpdateStorageLocationProductAsync_ShouldReturnSuccessAsync()
        {
            var productLocation = new StorageLocationProduct
            {
                StorageLocationProductId = 1,
                IsActive = true,
                UncertaintyPercentage = 10.0m,
                OwnershipRuleId = 1,
            };

            productLocation.Owners.Add(new StorageLocationProductOwner { OwnerId = 29, OwnershipPercentage = 12, });
            productLocation.Owners.Add(new StorageLocationProductOwner { OwnerId = 27, OwnershipPercentage = 88, });

            var locRepository = new Mock<IRepository<StorageLocationProduct>>();
            locRepository.Setup(y => y.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(productLocation);
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<StorageLocationProduct>()).Returns(locRepository.Object);

            await this.processor.UpdateStorageLocationProductAsync(productLocation).ConfigureAwait(false);

            locRepository.Verify(y => y.GetByIdAsync(It.IsAny<int>()), Times.Once);
            this.mockUnitOfWorkFactory.Verify(x => x.GetUnitOfWork().CreateRepository<StorageLocationProduct>(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task UpdateStorageLocationProductAsync_ShouldRaiseKeyNotFoundExceptionAsync()
        {
            var productLocation = new StorageLocationProduct
            {
                StorageLocationProductId = 1,
                IsActive = true,
                UncertaintyPercentage = 10.0m,
                OwnershipRuleId = 1,
            };

            StorageLocationProduct productLocationResponse = null;

            var locRepository = new Mock<IRepository<StorageLocationProduct>>();
            locRepository.Setup(y => y.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(productLocationResponse);
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<StorageLocationProduct>()).Returns(locRepository.Object);

            await this.processor.UpdateStorageLocationProductAsync(productLocation).ConfigureAwait(false);

            locRepository.Verify(y => y.GetByIdAsync(It.IsAny<int>()), Times.Once);
            this.mockUnitOfWorkFactory.Verify(x => x.GetUnitOfWork().CreateRepository<StorageLocationProduct>(), Times.Once);
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

            StorageLocationProduct locProduct = new StorageLocationProduct { IsActive = true };
            locProduct.Initialize();
            var locRepository = new Mock<IRepository<StorageLocationProduct>>();
            locRepository.Setup(y => y.SingleOrDefaultAsync(It.IsAny<Expression<Func<StorageLocationProduct, bool>>>(), "Owners")).ReturnsAsync(locProduct);
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<StorageLocationProduct>()).Returns(locRepository.Object);

            await this.processor.UpdateStorageLocationProductOwnersAsync(productOwners).ConfigureAwait(false);

            locRepository.Verify(y => y.SingleOrDefaultAsync(It.IsAny<Expression<Func<StorageLocationProduct, bool>>>(), "Owners"), Times.Once);
            this.mockUnitOfWorkFactory.Verify(x => x.GetUnitOfWork().CreateRepository<StorageLocationProduct>(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task UpdateStorageLocationProductOwnersAsync_ShouldRaiseKeyNotFoundExceptionAsync()
        {
            StorageLocationProductOwner owner1 = new StorageLocationProductOwner
            {
                StorageLocationProductId = 1,
                OwnerId = 10,
                OwnershipPercentage = 50,
            };

            StorageLocationProductOwner[] owners = new StorageLocationProductOwner[2];
            owners[0] = owner1;

            var rowVersion = "AAAAAAAAcHE=";

            var productOwners = new UpdateStorageLocationProductOwners()
            {
                ProductId = 1,
                RowVersion = rowVersion.FromBase64(),
                Owners = owners,
            };

            StorageLocationProduct locProduct = null;
            var locRepository = new Mock<IRepository<StorageLocationProduct>>();
            locRepository.Setup(y => y.SingleOrDefaultAsync(It.IsAny<Expression<Func<StorageLocationProduct, bool>>>(), "Owners")).ReturnsAsync(locProduct);
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<StorageLocationProduct>()).Returns(locRepository.Object);

            await this.processor.UpdateStorageLocationProductOwnersAsync(productOwners).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(x => x.GetUnitOfWork().CreateRepository<StorageLocationProduct>(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(System.IO.InvalidDataException))]
        public async Task UpdateStorageLocationProductOwnersAsync_ShouldRaiseInvalidOperationExceptionAsync()
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
                OwnershipPercentage = 40,
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

            StorageLocationProduct locProduct = new StorageLocationProduct { IsActive = true };
            locProduct.Initialize();
            var locRepository = new Mock<IRepository<StorageLocationProduct>>();
            locRepository.Setup(y => y.SingleOrDefaultAsync(It.IsAny<Expression<Func<StorageLocationProduct, bool>>>(), "Owners")).ReturnsAsync(locProduct);
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<StorageLocationProduct>()).Returns(locRepository.Object);

            await this.processor.UpdateStorageLocationProductOwnersAsync(productOwners).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(x => x.GetUnitOfWork().CreateRepository<StorageLocationProduct>(), Times.Once);
        }

        /// <summary>
        /// Update node ownership rules should not bulk update the ownership rules when invoked with valid details, when concurrency scenario occurs.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        [ExpectedException(typeof(DBConcurrencyException))]
        public async Task UpdateNodeOwnershipRulesAsync_ShouldCallNotStoredProcedureToUpdateOwnershipRules_WhenInvokedAsync()
        {
            var bulkUpdateRequest = new OwnershipRuleBulkUpdateRequest
            {
                Ids = new List<BulkUpdateEntity>()
                {
                    new BulkUpdateEntity
                    {
                        Id = 1,
                        RowVersion = "AAAAAAAA/S0=",
                    },
                    new BulkUpdateEntity
                    {
                        Id = 2,
                        RowVersion = "AAAAAAAA/S0=",
                    },
                },
                OwnershipRuleType = OwnershipRuleType.Node,
                OwnershipRuleId = 1,
            };
            var repoMock = new Mock<IRepository<Node>>();
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Node>()).Returns(repoMock.Object);
            this.mockFactory.Setup(m => m.CreateRepository<Node>()).Returns(repoMock.Object);

            await this.processor.UpdateNodeOwnershipRulesAsync(bulkUpdateRequest).ConfigureAwait(false);
        }

        /// <summary>
        /// Update node ownership rules should bulk update the ownership rules when invoked with valid details, when no concurrency scenario occurs.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task UpdateNodeOwnershipRulesAsync_ShouldCallStoredProcedureToUpdateOwnershipRules_WhenInvokedAsync()
        {
            var bulkUpdateRequest = new OwnershipRuleBulkUpdateRequest
            {
                Ids = new List<BulkUpdateEntity>()
                {
                    new BulkUpdateEntity
                    {
                        Id = 1,
                        RowVersion = Convert.ToBase64String(Convert.FromBase64String("AAAxxx==")),
                    },
                    new BulkUpdateEntity
                    {
                        Id = 2,
                        RowVersion = Convert.ToBase64String(Convert.FromBase64String("AAAxxx==")),
                    },
                },
                OwnershipRuleType = OwnershipRuleType.Node,
                OwnershipRuleId = 1,
            };
            var repoMock = new Mock<IRepository<Node>>();
            var bulkUpdateRepoMock = new Mock<IRepository<OwnershipRuleBulkUpdateRequestDto>>();
            var bulkUpdateRequests = new Dictionary<int, string>();
            foreach (var req in bulkUpdateRequest.Ids)
            {
                bulkUpdateRequests.Add(req.Id, req.RowVersion);
            }

            repoMock.Setup(
                m => m.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(new List<Node>()
                {
                    new Node
                    {
                        NodeId = 1,
                        RowVersion = Convert.FromBase64String("AAAxxx=="),
                    }
                    , new Node
                    {
                        NodeId = 2,
                        RowVersion = Convert.FromBase64String("AAAxxx=="),
                    },
                });
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Node>()).Returns(repoMock.Object);
            this.mockFactory.Setup(m => m.CreateRepository<OwnershipRuleBulkUpdateRequestDto>()).Returns(bulkUpdateRepoMock.Object);
            await this.processor.UpdateNodeOwnershipRulesAsync(bulkUpdateRequest).ConfigureAwait(false);
            bulkUpdateRepoMock.Verify(r => r.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Update node product rules should not bulk update the ownership rules when invoked with valid details, when concurrency scenario occurs.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        [ExpectedException(typeof(DBConcurrencyException))]
        public async Task UpdateNodeOwnershipRulesAsync_ShouldCallNotStoredProcedureToUpdateNodeProductOwnershipRules_WhenInvokedAsync()
        {
            var bulkUpdateRequest = new OwnershipRuleBulkUpdateRequest
            {
                Ids = new List<BulkUpdateEntity>()
                {
                    new BulkUpdateEntity
                    {
                        Id = 1,
                        RowVersion = "AAAAAAAA/S0=",
                    },
                    new BulkUpdateEntity
                    {
                        Id = 2,
                        RowVersion = "AAAAAAAA/S0=",
                    },
                },
                OwnershipRuleType = OwnershipRuleType.StorageLocationProduct,
                OwnershipRuleId = 1,
            };
            var repoMock = new Mock<IRepository<StorageLocationProduct>>();
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<StorageLocationProduct>()).Returns(repoMock.Object);
            this.mockFactory.Setup(m => m.CreateRepository<StorageLocationProduct>()).Returns(repoMock.Object);

            await this.processor.UpdateNodeOwnershipRulesAsync(bulkUpdateRequest).ConfigureAwait(false);
        }

        /// <summary>
        /// Update node ownership rules should bulk update the node product ownership rules when invoked with valid details, when no concurrency scenario occurs.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task UpdateNodeOwnershipRulesAsync_ShouldCallStoredProcedureToUpdateNodeProductOwnershipRules_WhenInvokedAsync()
        {
            var bulkUpdateRequest = new OwnershipRuleBulkUpdateRequest
            {
                Ids = new List<BulkUpdateEntity>()
                {
                    new BulkUpdateEntity
                    {
                        Id = 1,
                        RowVersion = Convert.ToBase64String(Convert.FromBase64String("AAAxxx==")),
                    },
                    new BulkUpdateEntity
                    {
                        Id = 2,
                        RowVersion = Convert.ToBase64String(Convert.FromBase64String("AAAxxx==")),
                    },
                },
                OwnershipRuleType = OwnershipRuleType.StorageLocationProduct,
                OwnershipRuleId = 1,
            };
            var repoMock = new Mock<IRepository<StorageLocationProduct>>();
            var bulkUpdateRepoMock = new Mock<IRepository<OwnershipRuleBulkUpdateRequestDto>>();
            var bulkUpdateRequests = new Dictionary<int, string>();
            foreach (var req in bulkUpdateRequest.Ids)
            {
                bulkUpdateRequests.Add(req.Id, req.RowVersion);
            }

            repoMock.Setup(
                m => m.GetAllAsync(It.IsAny<Expression<Func<StorageLocationProduct, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(new List<StorageLocationProduct>()
                {
                    new StorageLocationProduct
                    {
                        StorageLocationProductId = 1,
                        RowVersion = Convert.FromBase64String("AAAxxx=="),
                    }
                    , new StorageLocationProduct
                    {
                        StorageLocationProductId = 2,
                        RowVersion = Convert.FromBase64String("AAAxxx=="),
                    },
                });
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<StorageLocationProduct>()).Returns(repoMock.Object);
            this.mockFactory.Setup(m => m.CreateRepository<OwnershipRuleBulkUpdateRequestDto>()).Returns(bulkUpdateRepoMock.Object);
            await this.processor.UpdateNodeOwnershipRulesAsync(bulkUpdateRequest).ConfigureAwait(false);
            bulkUpdateRepoMock.Verify(r => r.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Update node product rules should not bulk update the node connection rules when invoked with valid details, when concurrency scenario occurs.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        [ExpectedException(typeof(DBConcurrencyException))]
        public async Task UpdateNodeOwnershipRulesAsync_ShouldCallNotStoredProcedureToUpdateNodeConnectionOwnershipRules_WhenInvokedAsync()
        {
            var bulkUpdateRequest = new OwnershipRuleBulkUpdateRequest
            {
                Ids = new List<BulkUpdateEntity>()
                {
                    new BulkUpdateEntity
                    {
                        Id = 1,
                        RowVersion = "AAAAAAAA/S0=",
                    },
                    new BulkUpdateEntity
                    {
                        Id = 2,
                        RowVersion = "AAAAAAAA/S0=",
                    },
                },
                OwnershipRuleType = OwnershipRuleType.NodeConnectionProduct,
                OwnershipRuleId = 1,
            };
            var repoMock = new Mock<IRepository<NodeConnectionProduct>>();
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<NodeConnectionProduct>()).Returns(repoMock.Object);
            this.mockFactory.Setup(m => m.CreateRepository<NodeConnectionProduct>()).Returns(repoMock.Object);

            await this.processor.UpdateNodeOwnershipRulesAsync(bulkUpdateRequest).ConfigureAwait(false);
        }

        /// <summary>
        /// Update node connection rules should bulk update the node connection ownership rules when invoked with valid details, when no concurrency scenario occurs.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task UpdateNodeOwnershipRulesAsync_ShouldCallStoredProcedureToUpdateNodeConnectionRules_WhenInvokedAsync()
        {
            var bulkUpdateRequest = new OwnershipRuleBulkUpdateRequest
            {
                Ids = new List<BulkUpdateEntity>()
                {
                    new BulkUpdateEntity
                    {
                        Id = 1,
                        RowVersion = Convert.ToBase64String(Convert.FromBase64String("AAAxxx==")),
                    },
                    new BulkUpdateEntity
                    {
                        Id = 2,
                        RowVersion = Convert.ToBase64String(Convert.FromBase64String("AAAxxx==")),
                    },
                },
                OwnershipRuleType = OwnershipRuleType.NodeConnectionProduct,
                OwnershipRuleId = 1,
            };
            var repoMock = new Mock<IRepository<NodeConnectionProduct>>();
            var bulkUpdateRepoMock = new Mock<IRepository<OwnershipRuleBulkUpdateRequestDto>>();
            var bulkUpdateRequests = new Dictionary<int, string>();
            foreach (var req in bulkUpdateRequest.Ids)
            {
                bulkUpdateRequests.Add(req.Id, req.RowVersion);
            }

            repoMock.Setup(
                m => m.GetAllAsync(It.IsAny<Expression<Func<NodeConnectionProduct, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(new List<NodeConnectionProduct>()
                {
                    new NodeConnectionProduct
                    {
                        NodeConnectionProductId = 1,
                        RowVersion = Convert.FromBase64String("AAAxxx=="),
                    }
                    , new NodeConnectionProduct
                    {
                        NodeConnectionProductId = 2,
                        RowVersion = Convert.FromBase64String("AAAxxx=="),
                    },
                });
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<NodeConnectionProduct>()).Returns(repoMock.Object);
            this.mockFactory.Setup(m => m.CreateRepository<OwnershipRuleBulkUpdateRequestDto>()).Returns(bulkUpdateRepoMock.Object);
            await this.processor.UpdateNodeOwnershipRulesAsync(bulkUpdateRequest).ConfigureAwait(false);
            bulkUpdateRepoMock.Verify(r => r.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Update node ownership rules should throw argument null exception if the request is null.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task UpdateNodeOwnershipRulesAsync_ShouldThrowArgumentNullException_WhenInvokedWithNullObjectAsync()
        {
            var repoMock = new Mock<IRepository<OwnershipRuleBulkUpdateRequestDto>>();
            this.mockFactory.Setup(m => m.CreateRepository<OwnershipRuleBulkUpdateRequestDto>()).Returns(repoMock.Object);

            await this.processor.UpdateNodeOwnershipRulesAsync(null).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task GetNodeOwnershipRulesAsync_ShouldReturnNodeOwnershipRule_WhenInvokedAsync()
        {
            var mockNodeOwnershipRuleRepository = new Mock<IRepository<NodeOwnershipRule>>();
            mockNodeOwnershipRuleRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<NodeOwnershipRule, bool>>>())).ReturnsAsync(new List<NodeOwnershipRule>());
            this.mockFactory.Setup(m => m.CreateRepository<NodeOwnershipRule>()).Returns(mockNodeOwnershipRuleRepository.Object);

            await this.processor.GetNodeOwnershipRulesAsync().ConfigureAwait(false);

            mockNodeOwnershipRuleRepository.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<NodeOwnershipRule, bool>>>()), Times.Once);
        }

        [TestMethod]
        public async Task GetNodeProductRulesAsync_ShouldReturnNodeProductRule_WhenInvokedAsync()
        {
            var mockNodeProductRuleRepository = new Mock<IRepository<NodeProductRule>>();
            mockNodeProductRuleRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<NodeProductRule, bool>>>())).ReturnsAsync(new List<NodeProductRule>());
            this.mockFactory.Setup(m => m.CreateRepository<NodeProductRule>()).Returns(mockNodeProductRuleRepository.Object);

            await this.processor.GetNodeProductRulesAsync().ConfigureAwait(false);

            mockNodeProductRuleRepository.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<NodeProductRule, bool>>>()), Times.Once);
        }

        [TestMethod]
        public async Task GetNodeTypeAsync_ShouldReturnCategoryElement_WhenInvokedAsync()
        {
            var mockCategoryElementRepository = new Mock<INodeRepository>();
            mockCategoryElementRepository.Setup(a => a.GetNodeTypeForNodeAsync(It.IsAny<int>())).ReturnsAsync(new CategoryElement());
            this.mockFactory.Setup(m => m.NodeRepository).Returns(mockCategoryElementRepository.Object);

            await this.processor.GetNodeTypeAsync(1).ConfigureAwait(false);

            mockCategoryElementRepository.Verify(r => r.GetNodeTypeForNodeAsync(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public async Task GetNodeWithSameOrderAsync_ShouldReturnNode_WhenInvokedAsync()
        {
            var mockNodeRepository = new Mock<INodeRepository>();
            mockNodeRepository.Setup(a => a.GetNodeWithSameOrderAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new Node());
            this.mockFactory.Setup(m => m.NodeRepository).Returns(mockNodeRepository.Object);

            await this.processor.GetNodeWithSameOrderAsync(1, 1, 2).ConfigureAwait(false);

            mockNodeRepository.Verify(r => r.GetNodeWithSameOrderAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        public bool ValidateOffchainNode(OffchainNode offchain, Node node)
        {
            ArgumentValidators.ThrowIfNull(offchain, nameof(offchain));
            ArgumentValidators.ThrowIfNull(node, nameof(node));
            return offchain.Name == node.Name &&
                offchain.NodeStateTypeId == (int)NodeState.UpdatedNode &&
                offchain.NodeId == node.NodeId &&
                offchain.IsActive == node.IsActive;
        }
    }
}
