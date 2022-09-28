// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementNodeValidatorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Registration.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Registration.Validation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;

    [TestClass]
    public class MovementNodeValidatorTests
    {
        /// <summary>
        /// The respository factory.
        /// </summary>
        private Mock<IRepositoryFactory> respositoryFactory;

        /// <summary>
        /// The node validator.
        /// </summary>
        private NodeValidator<Movement> nodeValidator;

        /// <summary>
        /// The node validator.
        /// </summary>
        private NodeValidator<Event> nodeEventValidator;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.respositoryFactory = new Mock<IRepositoryFactory>();
            this.nodeValidator = new NodeValidator<Movement>(this.respositoryFactory.Object);
            this.nodeEventValidator = new NodeValidator<Event>(this.respositoryFactory.Object);
        }

        /// <summary>
        /// Validators the movement for identified loss type.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProductValidator_ShouldValidateWithErrorForMovementIdentifiedLossForNullMovementSource_WithMovementAsync()
        {
            var movement = File.ReadAllText("MovementJson/MovementIdentifiedLoss.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            var result = await this.nodeValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Validators the movement for identified loss type.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProductValidator_ShouldValidateWithErrorForMovementIdentifiedLossForNullMovementSourceId_WithMovementAsync()
        {
            var movement = File.ReadAllText("MovementJson/MovementIdentifiedLoss.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);

            movementObject.MovementSource = new MovementSource
            {
                SourceProductId = "5",
            };
            var storageLocationProduct = new StorageLocationProduct
            {
                StorageLocationProductId = 5,
            };
            var repos = new Mock<IRepository<StorageLocationProduct>>();
            repos.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<StorageLocationProduct, bool>>>())).ReturnsAsync(storageLocationProduct);
            this.respositoryFactory.Setup(x => x.CreateRepository<StorageLocationProduct>()).Returns(repos.Object);

            var result = await this.nodeValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Validators the movement for error with identified loss type.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProductValidator_ShouldValidateWithErrorForMovementIdentifiedLossForNullMovementDestination_WithMovementAsync()
        {
            var movement = File.ReadAllText("MovementJson/MovementIdentifiedLoss.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            movementObject.MovementDestination = null;

            var result = await this.nodeValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Validators the movement for null movement destination node id.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProductValidator_ShouldValidateWithErrorForMovementIdentifiedLossForNullMovementDestinationNodeId_WithMovementAsync()
        {
            var movement = File.ReadAllText("MovementJson/MovementIdentifiedLoss.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            var storageLocationProduct = new StorageLocationProduct
            {
                StorageLocationProductId = 5,
            };
            var repos = new Mock<IRepository<StorageLocationProduct>>();
            repos.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<StorageLocationProduct, bool>>>())).ReturnsAsync(storageLocationProduct);
            this.respositoryFactory.Setup(x => x.CreateRepository<StorageLocationProduct>()).Returns(repos.Object);

            movementObject.MovementDestination = new MovementDestination
            {
                DestinationProductId = "5",
            };

            var result = await this.nodeValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.ErrorInfo.Any());
        }

        /// <summary>
        /// Validators the movement for invalid source node.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProductValidator_ShouldValidateSourceNodeInvalidProduct_WithMovementAsync()
        {
            var movement = File.ReadAllText("MovementJson/SingleMovement.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            var nodeConnection = new NodeConnection
            {
                DestinationNodeId = 5,
            };
            var repos = new Mock<IRepository<NodeConnection>>();
            repos.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>())).ReturnsAsync(nodeConnection);

            var nodeStorageLocation = new NodeStorageLocation
            {
                NodeId = 5,
            };

            nodeStorageLocation.Products.Add(new StorageLocationProduct { ProductId = "5" });

            var nodeStorageLocationRepo = new Mock<IRepository<NodeStorageLocation>>();
            nodeStorageLocationRepo.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<NodeStorageLocation, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<NodeStorageLocation> { nodeStorageLocation });

            this.respositoryFactory.Setup(x => x.CreateRepository<NodeConnection>()).Returns(repos.Object);
            this.respositoryFactory.Setup(x => x.CreateRepository<NodeStorageLocation>()).Returns(nodeStorageLocationRepo.Object);
            var result = await this.nodeValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.ErrorInfo.Any());
        }

        /// <summary>
        /// Validators the movement for valid source node.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProductValidator_ShouldValidateSourceNodeValidProduct_WithMovementAsync()
        {
            var movement = File.ReadAllText("MovementJson/SingleMovement.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            var nodeConnection = new NodeConnection
            {
                DestinationNodeId = 5,
            };

            var nodeStorageLocation = new NodeStorageLocation
            {
                NodeId = 5,
            };

            nodeStorageLocation.Products.Add(new StorageLocationProduct { ProductId = "5" });

            var repos = new Mock<IRepository<NodeConnection>>();
            repos.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>())).ReturnsAsync(nodeConnection);

            var nodeStorageLocationRepo = new Mock<IRepository<NodeStorageLocation>>();
            nodeStorageLocationRepo.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<NodeStorageLocation, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<NodeStorageLocation> { nodeStorageLocation });

            this.respositoryFactory.Setup(x => x.CreateRepository<NodeConnection>()).Returns(repos.Object);
            this.respositoryFactory.Setup(x => x.CreateRepository<NodeStorageLocation>()).Returns(nodeStorageLocationRepo.Object);
            var result = await this.nodeValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.ErrorInfo.Any());
        }

        /// <summary>
        /// Validators the movement for invalid product mapping.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProductValidator_ShouldValidateDestinationNodeInvalidProduct_WithMovementAsync()
        {
            var movement = File.ReadAllText("MovementJson/Movement2.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            var repos = new Mock<IRepository<NodeConnection>>();
            repos.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>())).ReturnsAsync(1);

            var nodeStorageLocationRepo = new Mock<IRepository<NodeStorageLocation>>();
            nodeStorageLocationRepo.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<NodeStorageLocation, bool>>>())).ReturnsAsync(1);

            this.respositoryFactory.Setup(x => x.CreateRepository<NodeConnection>()).Returns(repos.Object);
            this.respositoryFactory.Setup(x => x.CreateRepository<NodeStorageLocation>()).Returns(nodeStorageLocationRepo.Object);
            var result = await this.nodeValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.ErrorInfo.Any());
        }

        /// <summary>
        /// Validators the movement for connection exists.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProductValidator_ShouldValidateNodeConnectionExists_WithMovementAsync()
        {
            var movement = File.ReadAllText("MovementJson/SingleMovement.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            var repos = new Mock<IRepository<NodeConnection>>();
            repos.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>())).ReturnsAsync(default(NodeConnection));

            var storageLocationProductRepo = new Mock<IRepository<StorageLocationProduct>>();
            storageLocationProductRepo.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<StorageLocationProduct, bool>>>())).ReturnsAsync(default(StorageLocationProduct));

            this.respositoryFactory.Setup(x => x.CreateRepository<NodeConnection>()).Returns(repos.Object);
            this.respositoryFactory.Setup(x => x.CreateRepository<StorageLocationProduct>()).Returns(storageLocationProductRepo.Object);
            var result = await this.nodeValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Validators the movement for success on all scenario.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProductValidator_ShouldValidateNode_WithMovementAsync()
        {
            var nodeConnection = new NodeConnection
            {
                SourceNodeId = 5,
            };

            var movement = File.ReadAllText("MovementJson/SingleMovement.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            var repos = new Mock<IRepository<NodeConnection>>();
            repos.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>())).ReturnsAsync(nodeConnection);
            this.respositoryFactory.Setup(x => x.CreateRepository<NodeConnection>()).Returns(repos.Object);

            var nodeStorageLocation = new NodeStorageLocation
            {
                NodeId = 5,
            };

            nodeStorageLocation.Products.Add(new StorageLocationProduct { ProductId = "2" });
            nodeStorageLocation.Products.Add(new StorageLocationProduct { ProductId = "3" });

            var nodeStorageLocationRepo = new Mock<IRepository<NodeStorageLocation>>();
            nodeStorageLocationRepo.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<NodeStorageLocation, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<NodeStorageLocation> { nodeStorageLocation });
            this.respositoryFactory.Setup(x => x.CreateRepository<NodeStorageLocation>()).Returns(nodeStorageLocationRepo.Object);

            var result = await this.nodeValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Validated valid SourceNodId and valid DestinationNodeId should Pass.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task NodeValidator_ShouldPassWithValidSourceNodeIdAndDestinationNodeId_WithEventAsync()
        {
            // Arrange
            var eventElement = File.ReadAllText("EventJson/ValidEvent.json");
            var eventObject = JsonConvert.DeserializeObject<Event>(eventElement);
            var repos = new Mock<IRepository<Node>>();
            this.respositoryFactory.Setup(x => x.CreateRepository<Node>()).Returns(repos.Object);
            repos.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Node());
            var repoNodeConnection = new Mock<IRepository<NodeConnection>>();
            repoNodeConnection.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>())).ReturnsAsync(new NodeConnection());
            this.respositoryFactory.Setup(x => x.CreateRepository<NodeConnection>()).Returns(repoNodeConnection.Object);

            // Act
            var result = await this.nodeEventValidator.ValidateAsync(eventObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        /// <summary>
        /// Validates SourceNode and DestinationNode invalid connection should fail.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task NodeValidator_ShouldFailWithValidNodeConnectioninEvent_WithEventAsync()
        {
            // Arrange
            var eventElement = File.ReadAllText("EventJson/ValidEvent.json");
            var eventObject = JsonConvert.DeserializeObject<Event>(eventElement);
            var repos = new Mock<IRepository<Node>>();
            this.respositoryFactory.Setup(x => x.CreateRepository<Node>()).Returns(repos.Object);
            repos.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Node());
            var repoNodeConnection = new Mock<IRepository<NodeConnection>>();
            repoNodeConnection.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>())).ReturnsAsync((NodeConnection)null);
            this.respositoryFactory.Setup(x => x.CreateRepository<NodeConnection>()).Returns(repoNodeConnection.Object);

            // Act
            var result = await this.nodeEventValidator.ValidateAsync(eventObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(!result.IsSuccess);
        }

        /// <summary>
        /// Validates the invalid sourcenodeid and invalid destinationnodeid should fail.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task NodeValidator_ShouldFailWithInValidSourceNodeIdAndInvalidDestinationNodeId_WithEventAsync()
        {
            // Arrange
            var eventElement = File.ReadAllText("EventJson/ValidEvent.json");
            var eventObject = JsonConvert.DeserializeObject<Event>(eventElement);
            eventObject.SourceNodeId = 10000;
            eventObject.DestinationNodeId = 10000;
            var repos = new Mock<IRepository<Node>>();
            this.respositoryFactory.Setup(x => x.CreateRepository<Node>()).Returns(repos.Object);
            repos.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Node)null);
            var repoNodeConnection = new Mock<IRepository<NodeConnection>>();
            repoNodeConnection.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>())).ReturnsAsync(new NodeConnection());
            this.respositoryFactory.Setup(x => x.CreateRepository<NodeConnection>()).Returns(repoNodeConnection.Object);

            // Act
            var result = await this.nodeEventValidator.ValidateAsync(eventObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(!result.IsSuccess);
        }
    }
}
