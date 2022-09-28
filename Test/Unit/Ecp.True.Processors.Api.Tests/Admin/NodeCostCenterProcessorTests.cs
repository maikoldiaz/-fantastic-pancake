// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeCostCenterProcessorTests.cs" company="Microsoft">
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
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core.Entities;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Processors.Api.Admin;
    using Ecp.True.Processors.Api.Services.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class NodeCostCenterProcessorTests
    {
        /// <summary>
        /// A node cost center list used for testing.
        /// </summary>
        private List<NodeCostCenter> testNodeCostCenters;

        /// <summary>
        /// The processor.
        /// </summary>
        private NodeCostCenterProcessor processor;

        /// <summary>
        /// The mock of the repository.
        /// </summary>
        private Mock<IRepository<NodeCostCenter>> repoMock;

        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockFactory;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The node cost center validator.
        /// </summary>
        private Mock<INodeCostCenterValidator> mockValidator;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> unitOfWork;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockFactory = new Mock<IRepositoryFactory>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockValidator = new Mock<INodeCostCenterValidator>();
            this.mockValidator
                .Setup(v => v.ValidateForDeletionAsync(It.IsAny<NodeCostCenter>()))
                .Returns(Task.FromResult(new ValidationResult { IsSuccess = true }));

            this.processor = new NodeCostCenterProcessor(this.mockUnitOfWorkFactory.Object, this.mockFactory.Object, this.mockValidator.Object);
        }

        /// <summary>
        /// Create node cost center async should throw argument null exception when a null entity is passed.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CreateNodeCostCentersAsync_ShouldThrowArgumentNullException_WhenNullEntityIsPassedAsync()
        {
            // Prepare
            this.InitializeMocksForVoidAndNullListTests();

            // Execute
            var sut = this.processor.CreateNodeCostCentersAsync(default(List<NodeCostCenter>)).ConfigureAwait(false);
            var token = new CancellationToken(false);

            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await sut).ConfigureAwait(false);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<NodeCostCenter>(), Times.Never);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Never);
            this.repoMock.Verify(r => r.Insert(It.IsAny<NodeCostCenter>()), Times.Never);
            this.repoMock.Verify(r => r.Update(It.IsAny<NodeCostCenter>()), Times.Never);
        }

        /// <summary>
        /// Create node cost center should throw argument null exception when a void list is passed.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CreateNodeCostCentersAsync_ShouldThrowArgumentNullException_WhenVoidListIsPassedAsync()
        {
            // Prepare
            this.InitializeMocksForVoidAndNullListTests();

            // Execute
            var sut = this.processor.CreateNodeCostCentersAsync(new List<NodeCostCenter>()).ConfigureAwait(false);
            var token = new CancellationToken(false);

            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await sut).ConfigureAwait(false);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<NodeCostCenter>(), Times.Never);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Never);
            this.repoMock.Verify(r => r.Insert(It.IsAny<NodeCostCenter>()), Times.Never);
            this.repoMock.Verify(r => r.Update(It.IsAny<NodeCostCenter>()), Times.Never);
        }

        /// <summary>
        /// Create node cost center async should throw node cost center exisists exception when the cost centers already exist.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CreateNodeCostCentersAsync_ShouldReturnInfoWithDuplicatedStatus_WhenCostCentersExistsAndAsync()
        {
            // Prepare
            this.InitializeMocksForVoidAndNullListTests();
            this.testNodeCostCenters.Add(new NodeCostCenter { NodeCostCenterId = 10, SourceNodeId = 20, DestinationNodeId = 30, CostCenterId = 40, IsDeleted = false });
            this.testNodeCostCenters.ForEach(t => t.IsActive = true);

            // Execute
            var sut = await this.processor.CreateNodeCostCentersAsync(this.testNodeCostCenters).ConfigureAwait(false);
            var duplicated = sut.AsEnumerable().Where(n => n.Status == NodeCostCenterInfo.CreationStatus.Duplicated);

            // Assert
            Assert.AreEqual(3, duplicated.Count());
        }

        /// <summary>
        /// Create node cost center async should activate the node cost centers when inactive cost centers are passed.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CreateNodeCostCentersAsync_ShouldActivateNodeCostCenters_WhenInactiveCostCenterIsPassedAsync()
        {
            // Prepare
            this.InitializeTestNodeCostCenters();
            this.testNodeCostCenters.ForEach(t => t.IsActive = false);
            this.ConfigureVoidRepository();
            this.ConfigureUnitOfWorkMock();
            this.ConfigureUnitOfWorkFactory();

            var token = new CancellationToken(false);

            // Execute
            var sut = await this.processor.CreateNodeCostCentersAsync(this.testNodeCostCenters).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(sut.AsEnumerable().Select(n => n.NodeCostCenter).Any(n => n.IsActive.Value), "Inactive cost center must be active");

            this.repoMock.Verify(
                r => r.Insert(It.IsAny<NodeCostCenter>()),
                Times.Exactly(this.testNodeCostCenters.Count));
            this.mockUnitOfWorkFactory.Verify(
                m => m.GetUnitOfWork().SaveAsync(token),
                Times.Once);
            this.repoMock.Verify(
                r => r.Update(It.IsAny<NodeCostCenter>()),
                Times.Never);
        }

        /// <summary>
        /// Create node cost center async should create node costs async.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CreateNodeCostCentersAsync_Should_CreateNodeCostCentersAsync()
        {
            this.InitializeTestNodeCostCenters();
            this.ConfigureRepositoryWithTestNodeCostCenters();
            this.ConfigureUnitOfWorkMock();
            this.ConfigureUnitOfWorkFactory();

            // Prepare
            this.repoMock = new Mock<IRepository<NodeCostCenter>>();
            this.repoMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<NodeCostCenter, bool>>>(), It.IsAny<string[]>()))
                .Returns(Task.FromResult(default(IEnumerable<NodeCostCenter>)));
            var token = new CancellationToken(false);

            this.ConfigureUnitOfWorkFactory();

            // Execute
            await this.processor.CreateNodeCostCentersAsync(this.testNodeCostCenters).ConfigureAwait(false);

            // Asert
            this.mockUnitOfWorkFactory.Verify(
                m => m.GetUnitOfWork().CreateRepository<NodeCostCenter>(),
                Times.Once);
            this.repoMock.Verify(
                r => r.Insert(It.IsAny<NodeCostCenter>()),
                Times.Exactly(this.testNodeCostCenters.Count));
            this.mockUnitOfWorkFactory.Verify(
                m => m.GetUnitOfWork().SaveAsync(token),
                Times.Once);
            this.repoMock.Verify(
                r => r.UpdateAll(It.IsAny<IEnumerable<NodeCostCenter>>()),
                Times.Never);
        }

        /// <summary>
        /// Create node cost center async should throw exception when NodeCostCenter has movements async.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task DeleteNodeCostCenterAsync_ShouldThrowException_WhenNodeCostCenterHasMovementsAsync()
        {
            this.mockValidator
                .Setup(v => v.ValidateForDeletionAsync(It.IsAny<NodeCostCenter>()))
                .Returns(Task.FromResult(new ValidationResult { IsSuccess = false }));

            this.InitializeTestNodeCostCenters();
            this.ConfigureRepositoryWithTestNodeCostCenters();
            this.ConfigureUnitOfWorkMock();
            this.ConfigureUnitOfWorkFactory();

            // Prepare
            var token = new CancellationToken(false);

            this.ConfigureUnitOfWorkFactory();

            // Execute
            var nodeCostCenterToDelete = this.testNodeCostCenters.FirstOrDefault();
            var sut = this.processor.DeleteNodeCostCenterAsync(nodeCostCenterToDelete.NodeCostCenterId).ConfigureAwait(false);

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => await sut).ConfigureAwait(false);

            Assert.AreEqual(Entities.Constants.NodeCostCenterHasMovements, exception.Message);

            this.mockUnitOfWorkFactory.Verify(
                m => m.GetUnitOfWork().CreateRepository<NodeCostCenter>(),
                Times.Once);
            this.repoMock.Verify(
                r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeCostCenter, bool>>>()),
                Times.Once);
            this.mockValidator.Verify(
                v => v.ValidateForDeletionAsync(nodeCostCenterToDelete),
                Times.Once);
            this.repoMock.Verify(
                r => r.Insert(It.IsAny<NodeCostCenter>()),
                Times.Never);
            this.repoMock.Verify(
                r => r.Delete(It.IsAny<NodeCostCenter>()),
                Times.Never);
            this.repoMock.Verify(
                r => r.Update(It.IsAny<NodeCostCenter>()),
                Times.Never);
            this.mockUnitOfWorkFactory.Verify(
                m => m.GetUnitOfWork().SaveAsync(token),
                Times.Never);
        }

        /// <summary>
        /// Create node cost center async should create node costs async.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task DeleteNodeCostCenterAsync_Should_DeleteNodeCostCenterAsync()
        {
            this.InitializeTestNodeCostCenters();
            this.ConfigureRepositoryWithTestNodeCostCenters();
            this.ConfigureUnitOfWorkMock();
            this.ConfigureUnitOfWorkFactory();

            // Prepare
            var token = new CancellationToken(false);

            this.ConfigureUnitOfWorkFactory();

            // Execute
            await this.processor.DeleteNodeCostCenterAsync(this.testNodeCostCenters.FirstOrDefault().NodeCostCenterId).ConfigureAwait(false);

            // Asert
            this.mockUnitOfWorkFactory.Verify(
                m => m.GetUnitOfWork().CreateRepository<NodeCostCenter>(),
                Times.Once);
            this.repoMock.Verify(
                r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeCostCenter, bool>>>()),
                Times.Once);
            this.repoMock.Verify(
                r => r.Insert(It.IsAny<NodeCostCenter>()),
                Times.Never);
            this.repoMock.Verify(
                r => r.Delete(It.IsAny<NodeCostCenter>()),
                Times.Never);
            this.repoMock.Verify(
                r => r.Update(It.IsAny<NodeCostCenter>()),
                Times.Once);
            this.mockUnitOfWorkFactory.Verify(
                m => m.GetUnitOfWork().SaveAsync(token),
                Times.Once);
        }

        /// <summary>
        /// Update node cost center async should throw exception when a null entity doesn't exist.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task DeleteNodeCostCentersAsync_ShouldThrowException_WhenEntityDoesntExistAsync()
        {
            // Prepare
            this.InitializeMocksForVoidAndNullListTests();
            var nonExistentNodeCostCenter = new NodeCostCenter { NodeCostCenterId = 100, SourceNodeId = 101, DestinationNodeId = 102, MovementTypeId = 103 };

            // Execute
            var sut = this.processor.DeleteNodeCostCenterAsync(nonExistentNodeCostCenter.NodeCostCenterId).ConfigureAwait(false);
            var token = new CancellationToken(false);

            // Assert
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => await sut).ConfigureAwait(false);
            this.mockUnitOfWorkFactory.Verify(
                m => m.GetUnitOfWork().CreateRepository<NodeCostCenter>(),
                Times.Once);
            this.repoMock.Verify(
                r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeCostCenter, bool>>>()),
                Times.Once);
            this.repoMock.Verify(
                r => r.Insert(It.IsAny<NodeCostCenter>()),
                Times.Never);
            this.repoMock.Verify(
                r => r.Update(It.IsAny<NodeCostCenter>()),
                Times.Never);
            this.mockUnitOfWorkFactory.Verify(
                m => m.GetUnitOfWork().SaveAsync(token),
                Times.Never);
        }

        /// <summary>
        /// Update node cost center async should update is active async.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateNodeCostCentersAsync_Should_UpdateNodeCostCenterWithNullDestinatioNoNodeAsync()
        {
            // Prepare
            this.testNodeCostCenters = new List<NodeCostCenter>
            {
                new NodeCostCenter
                {
                    NodeCostCenterId = 1, SourceNodeId = 1, DestinationNodeId = null, MovementTypeId = 3, CostCenterId = 4,
                    IsDeleted = false, IsActive = false,
                },
            };
            this.ConfigureRepositoryWithTestNodeCostCenters();
            this.ConfigureUnitOfWorkMock();
            this.ConfigureUnitOfWorkFactory();

            var token = new CancellationToken(false);
            var nodeCostCenter = new NodeCostCenter();
            nodeCostCenter.CopyFrom(this.testNodeCostCenters.FirstOrDefault());
            nodeCostCenter.NodeCostCenterId = this.testNodeCostCenters.FirstOrDefault().NodeCostCenterId;
            nodeCostCenter.IsActive = !nodeCostCenter.IsActive;

            // Execute
            var sut = await this.processor.UpdateNodeCostCenterAsync(nodeCostCenter).ConfigureAwait(false);

            // Asert
            Assert.AreEqual(nodeCostCenter.IsActive, sut.IsActive, "Should update the IsActive property");
            Assert.AreEqual(nodeCostCenter.CostCenterId, sut.CostCenterId, "Should update the CostCenterId property");

            this.mockUnitOfWorkFactory.Verify(
                m => m.GetUnitOfWork().CreateRepository<NodeCostCenter>(),
                Times.Once);
            this.repoMock.Verify(
                r => r.Insert(It.IsAny<NodeCostCenter>()),
                Times.Never);
            this.repoMock.Verify(
                r => r.Update(It.IsAny<NodeCostCenter>()),
                Times.Once);
            this.mockUnitOfWorkFactory.Verify(
                m => m.GetUnitOfWork().SaveAsync(token),
                Times.Once);
        }

        /// <summary>
        /// Update node cost center async should update is active async.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateNodeCostCentersAsync_ShouldUpdateIsActive_UpdateNodeCostCentersAsync()
        {
            // Prepare
            this.InitializeTestNodeCostCenters();
            this.ConfigureRepositoryWithTestNodeCostCenters();
            this.ConfigureUnitOfWorkMock();
            this.ConfigureUnitOfWorkFactory();

            var token = new CancellationToken(false);
            var nodeCostCenter = new NodeCostCenter();
            nodeCostCenter.CopyFrom(this.testNodeCostCenters.FirstOrDefault());
            nodeCostCenter.NodeCostCenterId = this.testNodeCostCenters.FirstOrDefault().NodeCostCenterId;
            nodeCostCenter.IsActive = !nodeCostCenter.IsActive;

            // Execute
            var sut = await this.processor.UpdateNodeCostCenterAsync(nodeCostCenter).ConfigureAwait(false);

            // Asert
            Assert.AreEqual(nodeCostCenter.IsActive, sut.IsActive, "Should update the IsActive property");
            Assert.AreEqual(nodeCostCenter.CostCenterId, sut.CostCenterId, "Should update the CostCenterId property");

            this.mockUnitOfWorkFactory.Verify(
                m => m.GetUnitOfWork().CreateRepository<NodeCostCenter>(),
                Times.Once);
            this.repoMock.Verify(
                r => r.Insert(It.IsAny<NodeCostCenter>()),
                Times.Never);
            this.repoMock.Verify(
                r => r.Update(It.IsAny<NodeCostCenter>()),
                Times.Once);
            this.mockUnitOfWorkFactory.Verify(
                m => m.GetUnitOfWork().SaveAsync(token),
                Times.Once);
        }

        /// <summary>
        /// Update node cost center async should update the cost center id async.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateNodeCostCentersAsync_ShouldUpdateCostCenterId_UpdateNodeCostCentersAsync()
        {
            // Prepare
            this.InitializeTestNodeCostCenters();
            this.ConfigureRepositoryWithTestNodeCostCenters();
            this.ConfigureUnitOfWorkMock();
            this.ConfigureUnitOfWorkFactory();

            var token = new CancellationToken(false);
            var nodeCostCenter = new NodeCostCenter();
            nodeCostCenter.CopyFrom(this.testNodeCostCenters.FirstOrDefault());
            nodeCostCenter.NodeCostCenterId = this.testNodeCostCenters.FirstOrDefault().NodeCostCenterId;
            nodeCostCenter.CostCenterId = nodeCostCenter.CostCenterId.GetValueOrDefault() - 1;

            // Execute
            var sut = await this.processor.UpdateNodeCostCenterAsync(nodeCostCenter).ConfigureAwait(false);

            // Asert
            Assert.AreEqual(nodeCostCenter.IsActive, sut.IsActive, "Should update the IsActive property");
            Assert.AreEqual(nodeCostCenter.CostCenterId, sut.CostCenterId, "Should update the CostCenterId property");

            this.mockUnitOfWorkFactory.Verify(
                m => m.GetUnitOfWork().CreateRepository<NodeCostCenter>(),
                Times.Once);
            this.repoMock.Verify(
                r => r.Insert(It.IsAny<NodeCostCenter>()),
                Times.Never);
            this.repoMock.Verify(
                r => r.Update(It.IsAny<NodeCostCenter>()),
                Times.Once);
            this.mockUnitOfWorkFactory.Verify(
                m => m.GetUnitOfWork().SaveAsync(token),
                Times.Once);
        }

        /// <summary>
        /// Update node cost center async should throw argument null exception when a null entity is passed.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateNodeCostCentersAsync_ShouldThrowArgumentNullException_WhenNullEntityIsPassedAsync()
        {
            // Prepare
            this.InitializeMocksForVoidAndNullListTests();

            // Execute
            var sut = this.processor.UpdateNodeCostCenterAsync(default(NodeCostCenter)).ConfigureAwait(false);
            var token = new CancellationToken(false);

            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await sut).ConfigureAwait(false);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<NodeCostCenter>(), Times.Never);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Never);
            this.repoMock.Verify(r => r.Insert(It.IsAny<NodeCostCenter>()), Times.Never);
            this.repoMock.Verify(r => r.Update(It.IsAny<NodeCostCenter>()), Times.Never);
        }

        /// <summary>
        /// Update node cost center async should throw exception when a null entity doesn't exist.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateNodeCostCentersAsync_ShouldThrowException_WhenEntityDoesntExistAsync()
        {
            // Prepare
            this.InitializeMocksForVoidAndNullListTests();
            var nonExistentNodeCostCenter = new NodeCostCenter { NodeCostCenterId = 100, SourceNodeId = 101, DestinationNodeId = 102, MovementTypeId = 103 };

            // Execute
            var sut = this.processor.UpdateNodeCostCenterAsync(nonExistentNodeCostCenter).ConfigureAwait(false);
            var token = new CancellationToken(false);

            // Assert
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => await sut).ConfigureAwait(false);
            this.mockUnitOfWorkFactory.Verify(
                m => m.GetUnitOfWork().CreateRepository<NodeCostCenter>(),
                Times.Once);
            this.repoMock.Verify(
                r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeCostCenter, bool>>>()),
                Times.Once);
            this.repoMock.Verify(
                r => r.Insert(It.IsAny<NodeCostCenter>()),
                Times.Never);
            this.repoMock.Verify(
                r => r.Update(It.IsAny<NodeCostCenter>()),
                Times.Never);
            this.mockUnitOfWorkFactory.Verify(
                m => m.GetUnitOfWork().SaveAsync(token),
                Times.Never);
        }

        /// <summary>
        /// Update node cost center async should throw exception when an identical nodes - cost center - movement type combinationalready exists.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateNodeCostCentersAsync_ShouldThrowException_WhenIdenticalCombinationAlreadyExistsAsync()
        {
            // Prepare
            this.InitializeMocksForVoidAndNullListTests();
            var identicalNodeCostCenter = this.testNodeCostCenters.FirstOrDefault();

            // Execute
            var sut = this.processor.UpdateNodeCostCenterAsync(identicalNodeCostCenter).ConfigureAwait(false);
            var token = new CancellationToken(false);

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => await sut).ConfigureAwait(false);
            Assert.AreEqual(Entities.Constants.NodeCostCenterAlreadyExistsInactive, exception?.Message);
            this.mockUnitOfWorkFactory.Verify(
                m => m.GetUnitOfWork().CreateRepository<NodeCostCenter>(),
                Times.Once);
            this.repoMock.Verify(
                r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeCostCenter, bool>>>()),
                Times.Once);
            this.repoMock.Verify(
                r => r.Insert(It.IsAny<NodeCostCenter>()),
                Times.Never);
            this.repoMock.Verify(
                r => r.Update(It.IsAny<NodeCostCenter>()),
                Times.Never);
            this.mockUnitOfWorkFactory.Verify(
                m => m.GetUnitOfWork().SaveAsync(token),
                Times.Never);
        }

        private void InitializeTestNodeCostCenters()
        {
            this.testNodeCostCenters = new List<NodeCostCenter>
            {
                new NodeCostCenter { NodeCostCenterId = 1, SourceNodeId = 1, DestinationNodeId = 2, MovementTypeId = 3, CostCenterId = 4, IsDeleted = false, IsActive = false },
                new NodeCostCenter { NodeCostCenterId = 2, SourceNodeId = 2, DestinationNodeId = 3, MovementTypeId = 4, CostCenterId = 5, IsDeleted = false, IsActive = false },
                new NodeCostCenter { NodeCostCenterId = 3, SourceNodeId = 3, DestinationNodeId = 4, MovementTypeId = 5, CostCenterId = 6, IsDeleted = false, IsActive = false },
            };
        }

        private void InitializeMocksForVoidAndNullListTests()
        {
            this.InitializeTestNodeCostCenters();
            this.ConfigureRepositoryWithTestNodeCostCenters();
            this.ConfigureUnitOfWorkMock();
            this.ConfigureUnitOfWorkFactory();
        }

        private void ConfigureVoidRepository()
        {
            this.repoMock = new Mock<IRepository<NodeCostCenter>>();
            this.repoMock
                .Setup(
                    x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<NodeCostCenter, bool>>>()))
                .Returns(Task.FromResult(default(NodeCostCenter)));
        }

        private void ConfigureRepositoryWithTestNodeCostCenters()
        {
            this.repoMock = new Mock<IRepository<NodeCostCenter>>();

            this.testNodeCostCenters.ForEach(
                nodeCostCenter =>
                {
                    SetUpRepoUsingExistingValues(nodeCostCenter);
                    SetUpRepoUsingPrincipaKey(nodeCostCenter);
                    SetUpRepoUsingUniqueValues(nodeCostCenter);
                });

            void SetUpRepoUsingExistingValues(NodeCostCenter nodeCostCenter)
            {
                int sourceNodeId = nodeCostCenter.SourceNodeId.GetValueOrDefault();
                int? destinationNodeId = nodeCostCenter.DestinationNodeId;
                int movementTypeId = nodeCostCenter.MovementTypeId.GetValueOrDefault();

                this.repoMock.Setup(
                    x => x.SingleOrDefaultAsync(
                x => x.SourceNodeId == sourceNodeId
                && x.DestinationNodeId == destinationNodeId
                && x.MovementTypeId == movementTypeId
                && !x.IsDeleted))
                    .Returns(Task.FromResult(nodeCostCenter));
            }

            void SetUpRepoUsingUniqueValues(NodeCostCenter nodeCostCenter)
            {
                int sourceNodeId = nodeCostCenter.SourceNodeId.GetValueOrDefault();
                int? destinationNodeId = nodeCostCenter.DestinationNodeId;
                int movementTypeId = nodeCostCenter.MovementTypeId.GetValueOrDefault();
                int nodeCostCenterId = nodeCostCenter.NodeCostCenterId;

                this.repoMock.Setup(
                    x => x.SingleOrDefaultAsync(
                        x => x.NodeCostCenterId == nodeCostCenterId
                        && x.SourceNodeId == sourceNodeId
                        && x.DestinationNodeId == destinationNodeId
                        && x.MovementTypeId == movementTypeId))
                    .Returns(Task.FromResult(nodeCostCenter));
            }

            void SetUpRepoUsingPrincipaKey(NodeCostCenter nodeCostCenter)
            {
                var nodeCostCenterId = nodeCostCenter.NodeCostCenterId;
                this.repoMock
                    .Setup(x => x.SingleOrDefaultAsync(
                        n => n.NodeCostCenterId == nodeCostCenterId
                        && !n.IsDeleted))
                    .Returns(Task.FromResult(nodeCostCenter));
            }
        }

        private void ConfigureUnitOfWorkFactory()
        {
            var token = new CancellationToken(false);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<NodeCostCenter>())
                .Returns(this.repoMock.Object);
        }

        private void ConfigureUnitOfWorkMock()
        {
            this.unitOfWork = new Mock<IUnitOfWork>();
            this.mockFactory.Setup(x => x.CreateRepository<NodeCostCenter>()).Returns(this.repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork())
                .Returns(this.unitOfWork.Object);
        }
    }
}
