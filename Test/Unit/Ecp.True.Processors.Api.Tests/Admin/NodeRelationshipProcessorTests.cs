// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeRelationshipProcessorTests.cs" company="Microsoft">
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
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Analytics;
    using Ecp.True.Processors.Api.Admin;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The Node relationship tests.
    /// </summary>
    [TestClass]
    public class NodeRelationshipProcessorTests
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private NodeRelationshipProcessor processor;

        /// <summary>
        /// The token.
        /// </summary>
        private CancellationToken token;

        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockFactory;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The logistics node repo mock.
        /// </summary>
        private Mock<IRepository<OperativeNodeRelationship>> operativeNodeRepoMock;

        /// <summary>
        /// The operative node repo mock.
        /// </summary>
        private Mock<IRepository<OperativeNodeRelationshipWithOwnership>> logisticsNodeRepoMock;

        /// <summary>
        ///  Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockFactory = new Mock<IRepositoryFactory>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.processor = new NodeRelationshipProcessor(this.mockUnitOfWorkFactory.Object, this.mockFactory.Object);

            this.token = new CancellationToken(false);
            this.operativeNodeRepoMock = new Mock<IRepository<OperativeNodeRelationship>>();
            this.mockFactory.Setup(m => m.CreateRepository<OperativeNodeRelationship>()).Returns(this.operativeNodeRepoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<OperativeNodeRelationship>()).Returns(this.operativeNodeRepoMock.Object);
            this.logisticsNodeRepoMock = new Mock<IRepository<OperativeNodeRelationshipWithOwnership>>();
            this.mockFactory.Setup(m => m.CreateRepository<OperativeNodeRelationshipWithOwnership>()).Returns(this.logisticsNodeRepoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<OperativeNodeRelationshipWithOwnership>()).Returns(this.logisticsNodeRepoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(this.token));
        }

        /// <summary>
        /// Create Node Relationship Async.
        /// </summary>
        /// <returns>the task.</returns>
        [TestMethod]
        public async Task CreateNodeRelationshipAsync_WhenInvokedWithNull_ThrowArgumentNullExceptionAsync()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await this.processor.CreateNodeRelationshipAsync(default(OperativeNodeRelationship)).ConfigureAwait(false), "Exception should be thrown if no noderelationship is passed as argument.").ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<OperativeNodeRelationship>(), Times.Never);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(this.token), Times.Never);
            this.operativeNodeRepoMock.Verify(r => r.Insert(It.IsAny<OperativeNodeRelationship>()), Times.Never);
            this.operativeNodeRepoMock.Verify(r => r.Update(It.IsAny<OperativeNodeRelationship>()), Times.Never);
            this.operativeNodeRepoMock.Verify(r => r.Delete(It.IsAny<OperativeNodeRelationship>()), Times.Never);
        }

        /// <summary>
        /// Create Node Relationship Async.
        /// </summary>
        /// <returns>the task.</returns>
        [TestMethod]
        public async Task CreateNodeRelationshipAsync_ShouldInvokeRepository_ToCreateNodeRelationshipAsync()
        {
            var nodeRelationship = new OperativeNodeRelationship();
            this.operativeNodeRepoMock.Setup(r => r.Insert(It.IsAny<OperativeNodeRelationship>()));

            await this.processor.CreateNodeRelationshipAsync(nodeRelationship).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<OperativeNodeRelationship>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(this.token), Times.Once);
            this.operativeNodeRepoMock.Verify(r => r.Insert(It.IsAny<OperativeNodeRelationship>()), Times.Once);
            this.operativeNodeRepoMock.Verify(r => r.Update(It.IsAny<OperativeNodeRelationship>()), Times.Never);
            this.operativeNodeRepoMock.Verify(r => r.Delete(It.IsAny<OperativeNodeRelationship>()), Times.Never);
        }

        /// <summary>
        /// Update Node Relationship Async.
        /// </summary>
        /// <returns>the task.</returns>
        [TestMethod]
        public async Task UpdateNodeRelationshipAsync_WhenInvokedWithNull_ThrowArgumentNullExceptionAsync()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await this.processor.UpdateNodeRelationshipAsync(default(OperativeNodeRelationship)).ConfigureAwait(false), "Exception should be thrown if no node relationship update info is passed as argument.").ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<OperativeNodeRelationship>(), Times.Never);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(this.token), Times.Never);
            this.operativeNodeRepoMock.Verify(r => r.Insert(It.IsAny<OperativeNodeRelationship>()), Times.Never);
            this.operativeNodeRepoMock.Verify(r => r.Update(It.IsAny<OperativeNodeRelationship>()), Times.Never);
            this.operativeNodeRepoMock.Verify(r => r.Delete(It.IsAny<OperativeNodeRelationship>()), Times.Never);
        }

        /// <summary>
        /// Update Node Relationship Async.
        /// </summary>
        /// <returns>the task.</returns>
        [TestMethod]
        public async Task UpdateNodeRelationshipAsync_ShouldInvokeRepository_ToUpdateNodeRelationshipAsync()
        {
            var nodeRelationship = new OperativeNodeRelationship() { FieldWaterProduction = "TestProduction", RelatedSourceField = "TestRelatedSourceField", SourceField = "TestSourceField" };
            this.operativeNodeRepoMock.Setup(r => r.Update(It.IsAny<OperativeNodeRelationship>()));
            this.operativeNodeRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(new OperativeNodeRelationship() { IsDeleted = false });

            await this.processor.UpdateNodeRelationshipAsync(nodeRelationship).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<OperativeNodeRelationship>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(this.token), Times.Once);
            this.operativeNodeRepoMock.Verify(r => r.Insert(It.IsAny<OperativeNodeRelationship>()), Times.Never);
            this.operativeNodeRepoMock.Verify(r => r.Update(It.IsAny<OperativeNodeRelationship>()), Times.Once);
            this.operativeNodeRepoMock.Verify(r => r.Delete(It.IsAny<OperativeNodeRelationship>()), Times.Never);
        }

        /// <summary>
        /// Update Node Relationship Async.
        /// </summary>
        /// <returns>the task.</returns>
        [TestMethod]
        public async Task UpdateNodeRelationshipAsync_WhenNodeRelationShipNotFound_ToThrowKeyNotFoundExceptionAsync()
        {
            var nodeRelationship = new OperativeNodeRelationship() { FieldWaterProduction = "TestProduction", RelatedSourceField = "TestRelatedSourceField", SourceField = "TestSourceField" };
            this.operativeNodeRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<OperativeNodeRelationship, bool>>>())).ReturnsAsync((OperativeNodeRelationship)null);

            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => await this.processor.UpdateNodeRelationshipAsync(nodeRelationship).ConfigureAwait(false), "Exception should be thrown if nodeRelationship not found for the given identifier.").ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<OperativeNodeRelationship>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(this.token), Times.Never);
            this.operativeNodeRepoMock.Verify(r => r.Insert(It.IsAny<OperativeNodeRelationship>()), Times.Never);
            this.operativeNodeRepoMock.Verify(r => r.Update(It.IsAny<OperativeNodeRelationship>()), Times.Never);
            this.operativeNodeRepoMock.Verify(r => r.Delete(It.IsAny<OperativeNodeRelationship>()), Times.Never);
        }

        /// <summary>
        /// Get Node Relationship Async.
        /// </summary>
        /// <returns>the task.</returns>
        [TestMethod]
        public async Task GetNodeRelationshipAsync_ShouldInvokeRepositiry_ToGetNodeRelationshipAsync()
        {
            this.operativeNodeRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<OperativeNodeRelationship, bool>>>())).ReturnsAsync(new OperativeNodeRelationship());

            await this.processor.GetNodeRelationshipAsync(1).ConfigureAwait(false);

            this.mockFactory.Verify(m => m.CreateRepository<OperativeNodeRelationship>(), Times.Once);
            this.operativeNodeRepoMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<OperativeNodeRelationship, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Create Logistic Transfer Relationship.
        /// </summary>
        /// <returns>the task.</returns>
        [TestMethod]
        public async Task CreateLogisticTransferRelationshipAsync_ShouldInvokeRepository_ToCreateNodeRelationshipAsync()
        {
            var nodeRelationship = new OperativeNodeRelationshipWithOwnership();
            this.logisticsNodeRepoMock.Setup(r => r.Insert(It.IsAny<OperativeNodeRelationshipWithOwnership>()));

            await this.processor.CreateLogisticTransferRelationshipAsync(nodeRelationship).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<OperativeNodeRelationshipWithOwnership>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(this.token), Times.Once);
            this.logisticsNodeRepoMock.Verify(r => r.Insert(It.IsAny<OperativeNodeRelationshipWithOwnership>()), Times.Once);
            this.logisticsNodeRepoMock.Verify(r => r.Update(It.IsAny<OperativeNodeRelationshipWithOwnership>()), Times.Never);
            this.logisticsNodeRepoMock.Verify(r => r.Delete(It.IsAny<OperativeNodeRelationshipWithOwnership>()), Times.Never);
        }

        /// <summary>
        /// Create Logistic Transfer Relationship.
        /// </summary>
        /// <returns>the task.</returns>
        [TestMethod]
        public async Task CreateLogisticTransferRelationshipAsync_WhenInvokedWithNull_ThrowArgumentNullExceptionAsync()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await this.processor.CreateLogisticTransferRelationshipAsync(default(OperativeNodeRelationshipWithOwnership)).ConfigureAwait(false), "Exception should be thrown if no noderelationship is passed as argument.").ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<OperativeNodeRelationshipWithOwnership>(), Times.Never);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(this.token), Times.Never);
            this.logisticsNodeRepoMock.Verify(r => r.Insert(It.IsAny<OperativeNodeRelationshipWithOwnership>()), Times.Never);
            this.logisticsNodeRepoMock.Verify(r => r.Update(It.IsAny<OperativeNodeRelationshipWithOwnership>()), Times.Never);
            this.logisticsNodeRepoMock.Verify(r => r.Delete(It.IsAny<OperativeNodeRelationshipWithOwnership>()), Times.Never);
        }

        /// <summary>
        /// Delete Logistic Transfer Relationship.
        /// </summary>
        /// <returns>the task.</returns>
        [TestMethod]
        public async Task DeleteLogisticTransferRelationshipAsync_WhenInvokedWithNull_ThrowArgumentNullExceptionAsync()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await this.processor.DeleteLogisticTransferRelationshipAsync(default(OperativeNodeRelationshipWithOwnership)).ConfigureAwait(false), "Exception should be thrown if no node relationship delete info is passed as argument.").ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<OperativeNodeRelationshipWithOwnership>(), Times.Never);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(this.token), Times.Never);
            this.logisticsNodeRepoMock.Verify(r => r.Insert(It.IsAny<OperativeNodeRelationshipWithOwnership>()), Times.Never);
            this.logisticsNodeRepoMock.Verify(r => r.Update(It.IsAny<OperativeNodeRelationshipWithOwnership>()), Times.Never);
            this.logisticsNodeRepoMock.Verify(r => r.Delete(It.IsAny<OperativeNodeRelationshipWithOwnership>()), Times.Never);
        }

        /// <summary>
        /// Delete Logistic Transfer Relationship.
        /// </summary>
        /// <returns>the task.</returns>
        [TestMethod]
        public async Task DeleteLogisticTransferRelationshipAsync_ShouldInvokeRepository_ToSoftDeleteNodeRelationshipAsync()
        {
            var nodeRelationship = new OperativeNodeRelationshipWithOwnership() { Notes = "Test Notes" };
            this.logisticsNodeRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(new OperativeNodeRelationshipWithOwnership() { IsDeleted = false });

            await this.processor.DeleteLogisticTransferRelationshipAsync(nodeRelationship).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<OperativeNodeRelationshipWithOwnership>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(this.token), Times.Once);
            this.logisticsNodeRepoMock.Verify(r => r.Insert(It.IsAny<OperativeNodeRelationshipWithOwnership>()), Times.Never);
            this.logisticsNodeRepoMock.Verify(r => r.Update(It.IsAny<OperativeNodeRelationshipWithOwnership>()), Times.Once);
            this.logisticsNodeRepoMock.Verify(r => r.Delete(It.IsAny<OperativeNodeRelationshipWithOwnership>()), Times.Never);
        }

        /// <summary>
        /// Delete Logistic Transfer Relationship.
        /// </summary>
        /// <returns>the task.</returns>
        [TestMethod]
        public async Task DeleteLogisticTransferRelationshipAsync_WhenNodeRelationShipNotFound_ToThrowKeyNotFoundExceptionAsync()
        {
            var nodeRelationship = new OperativeNodeRelationshipWithOwnership() { Notes = "Test Notes" };
            this.logisticsNodeRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<OperativeNodeRelationshipWithOwnership, bool>>>())).ReturnsAsync((OperativeNodeRelationshipWithOwnership)null);

            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => await this.processor.DeleteLogisticTransferRelationshipAsync(nodeRelationship).ConfigureAwait(false), "Exception should be thrown if nodeRelationship not found for the given identifier.").ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<OperativeNodeRelationshipWithOwnership>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(this.token), Times.Never);
            this.logisticsNodeRepoMock.Verify(r => r.Insert(It.IsAny<OperativeNodeRelationshipWithOwnership>()), Times.Never);
            this.logisticsNodeRepoMock.Verify(r => r.Update(It.IsAny<OperativeNodeRelationshipWithOwnership>()), Times.Never);
            this.logisticsNodeRepoMock.Verify(r => r.Delete(It.IsAny<OperativeNodeRelationshipWithOwnership>()), Times.Never);
        }

        /// <summary>
        /// Check if Logistic Transfer Relationship exists.
        /// </summary>
        /// <returns>the task.</returns>
        [TestMethod]
        public async Task LogisticTransferRelationshipExistsAsync_WhenNodeRelationShipNotFound_ToReturnNodeRelationshipAsync()
        {
            var nodeRelationship = new OperativeNodeRelationshipWithOwnership();
            this.logisticsNodeRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<OperativeNodeRelationshipWithOwnership, bool>>>())).ReturnsAsync(new OperativeNodeRelationshipWithOwnership());

            await this.processor.LogisticTransferRelationshipExistsAsync(nodeRelationship).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<OperativeNodeRelationshipWithOwnership>(), Times.Never);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(this.token), Times.Never);
            this.logisticsNodeRepoMock.Verify(r => r.Insert(It.IsAny<OperativeNodeRelationshipWithOwnership>()), Times.Never);
            this.logisticsNodeRepoMock.Verify(r => r.Update(It.IsAny<OperativeNodeRelationshipWithOwnership>()), Times.Never);
            this.logisticsNodeRepoMock.Verify(r => r.Delete(It.IsAny<OperativeNodeRelationshipWithOwnership>()), Times.Never);
            this.logisticsNodeRepoMock.Verify(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<OperativeNodeRelationshipWithOwnership, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Check if Logistic Transfer Relationship exists.
        /// </summary>
        /// <returns>the task.</returns>
        [TestMethod]
        public async Task LogisticTransferRelationshipExistsAsync_WhenInvokedWithNull_ThrowArgumentNullExceptionAsync()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await this.processor.LogisticTransferRelationshipExistsAsync(default(OperativeNodeRelationshipWithOwnership)).ConfigureAwait(false), "Exception should be thrown if no node relationship delete info is passed as argument.").ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<OperativeNodeRelationshipWithOwnership>(), Times.Never);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(this.token), Times.Never);
            this.logisticsNodeRepoMock.Verify(r => r.Insert(It.IsAny<OperativeNodeRelationshipWithOwnership>()), Times.Never);
            this.logisticsNodeRepoMock.Verify(r => r.Update(It.IsAny<OperativeNodeRelationshipWithOwnership>()), Times.Never);
            this.logisticsNodeRepoMock.Verify(r => r.Delete(It.IsAny<OperativeNodeRelationshipWithOwnership>()), Times.Never);
            this.logisticsNodeRepoMock.Verify(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<OperativeNodeRelationshipWithOwnership, bool>>>()), Times.Never);
        }

        /// <summary>
        /// Check if Operative Transfer Relationship exists.
        /// </summary>
        /// <returns>the task.</returns>
        [TestMethod]
        public async Task OperativeTransferRelationshipExistsAsync_WhenNodeRelationShipNotFound_ToReturnNodeRelationshipAsync()
        {
            var nodeRelationship = new OperativeNodeRelationship();
            this.operativeNodeRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<OperativeNodeRelationship, bool>>>())).ReturnsAsync(new OperativeNodeRelationship());

            await this.processor.OperativeTransferRelationshipExistsAsync(nodeRelationship).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<OperativeNodeRelationship>(), Times.Never);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(this.token), Times.Never);
            this.operativeNodeRepoMock.Verify(r => r.Insert(It.IsAny<OperativeNodeRelationship>()), Times.Never);
            this.operativeNodeRepoMock.Verify(r => r.Update(It.IsAny<OperativeNodeRelationship>()), Times.Never);
            this.operativeNodeRepoMock.Verify(r => r.Delete(It.IsAny<OperativeNodeRelationship>()), Times.Never);
            this.operativeNodeRepoMock.Verify(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<OperativeNodeRelationship, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Check if Operative Transfer Relationship exists.
        /// </summary>
        /// <returns>the task.</returns>
        [TestMethod]
        public async Task OperativeTransferRelationshipExistsAsync_WhenInvokedWithNull_ThrowArgumentNullExceptionAsync()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await this.processor.OperativeTransferRelationshipExistsAsync(default(OperativeNodeRelationship)).ConfigureAwait(false), "Exception should be thrown if no node relationship delete info is passed as argument.").ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<OperativeNodeRelationship>(), Times.Never);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(this.token), Times.Never);
            this.operativeNodeRepoMock.Verify(r => r.Insert(It.IsAny<OperativeNodeRelationship>()), Times.Never);
            this.operativeNodeRepoMock.Verify(r => r.Update(It.IsAny<OperativeNodeRelationship>()), Times.Never);
            this.operativeNodeRepoMock.Verify(r => r.Delete(It.IsAny<OperativeNodeRelationship>()), Times.Never);
            this.operativeNodeRepoMock.Verify(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<OperativeNodeRelationship, bool>>>()), Times.Never);
        }

        /// <summary>
        /// Creates the node relationship asynchronous should throw exception if node relationship exists.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public async Task CreateNodeRelationshipAsync_ShouldThrowExceptionIfNodeRelationshipExistFromRepository_WhenInvokedAsync()
        {
            var existingNode = new OperativeNodeRelationship
            {
                SourceProduct = "1",
                SourceProductType = "2",
                SourceNodeType = "2",
                DestinationNodeType = "3",
                MovementType = "3",
                TransferPoint = "1",
                SourceNode = "1",
                DestinationNode = "1",
                IsDeleted = false,
            };

            var newNode = new OperativeNodeRelationship
            {
                SourceProduct = "1",
                SourceProductType = "2",
                SourceNodeType = "2",
                DestinationNodeType = "3",
                MovementType = "3",
                TransferPoint = "1",
                SourceNode = "1",
                DestinationNode = "1",
                IsDeleted = false,
            };

            this.operativeNodeRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<OperativeNodeRelationship, bool>>>())).ReturnsAsync(existingNode);
            this.mockFactory.Setup(a => a.CreateRepository<OperativeNodeRelationship>()).Returns(this.operativeNodeRepoMock.Object);

            await this.processor.CreateNodeRelationshipAsync(newNode).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates the logistic transfer asynchronous should throw exception if already exists.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public async Task CreateLogisticTransferRelationshipAsync_ShouldThrowExceptionIfLogisticExistFromRepository_WhenInvokedAsync()
        {
            var existingNode = new OperativeNodeRelationshipWithOwnership
            {
                SourceProduct = "1",
                LogisticDestinationCenter = "2",
                LogisticSourceCenter = "2",
                TransferPoint = "1",
                DestinationProduct = "1",
                IsDeleted = false,
            };

            var newNode = new OperativeNodeRelationshipWithOwnership
            {
                SourceProduct = "1",
                LogisticDestinationCenter = "2",
                LogisticSourceCenter = "2",
                TransferPoint = "1",
                DestinationProduct = "1",
                IsDeleted = false,
            };

            this.logisticsNodeRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<OperativeNodeRelationshipWithOwnership, bool>>>())).ReturnsAsync(existingNode);
            this.mockFactory.Setup(a => a.CreateRepository<OperativeNodeRelationshipWithOwnership>()).Returns(this.logisticsNodeRepoMock.Object);

            await this.processor.CreateLogisticTransferRelationshipAsync(newNode).ConfigureAwait(false);
        }
    }
}
