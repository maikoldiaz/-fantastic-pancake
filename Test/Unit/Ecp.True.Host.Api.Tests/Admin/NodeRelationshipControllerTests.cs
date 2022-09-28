// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeRelationshipControllerTests.cs" company="Microsoft">
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
    using Ecp.True.Entities.Analytics;
    using Ecp.True.Host.Api.Controllers;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class NodeRelationshipControllerTests
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private NodeRelationshipController controller;

        /// <summary>
        /// The mock processor.
        /// </summary>
        private Mock<INodeRelationshipProcessor> mockProcessor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockProcessor = new Mock<INodeRelationshipProcessor>();
            this.controller = new NodeRelationshipController(this.mockProcessor.Object);
        }

        /// <summary>
        /// Creates the node relationship asynchronous should invoke processor to create node connection asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task CreateNodeRelationshipAsync_ShouldInvokeProcessor_ToCreateNodeRelationshipAsync()
        {
            var nodeRelationship = new OperativeNodeRelationship();
            this.mockProcessor.Setup(x => x.CreateNodeRelationshipAsync(nodeRelationship));

            var result = await this.controller.CreateNodeRelationshipAsync(nodeRelationship).ConfigureAwait(false);
            var objectResult = result as EntityResult;

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(objectResult, "Object result should not be null");
            this.mockProcessor.Verify(x => x.CreateNodeRelationshipAsync(nodeRelationship), Times.Once);
        }

        /// <summary>
        /// Updates the node relationship asynchronous should invoke processor to update node relationship asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task UpdateNodeRelationshipAsync_ShouldInvokeProcessor_ToUpdateNodeRelationshipAsync()
        {
            var nodeRelationship = new OperativeNodeRelationship();
            this.mockProcessor.Setup(x => x.UpdateNodeRelationshipAsync(nodeRelationship));

            var result = await this.controller.UpdateNodeRelationshipAsync(nodeRelationship).ConfigureAwait(false);
            var objectResult = result as EntityResult;
            var returnMessage = objectResult.Messagekey;

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(objectResult, "Object result should not be null");
            Assert.IsNotNull(returnMessage, "Update succussfull message should not be null");
            Assert.AreEqual(returnMessage, Entities.Constants.NodeRelationshipUpdatedSuccessfully);
            this.mockProcessor.Verify(x => x.UpdateNodeRelationshipAsync(nodeRelationship), Times.Once);
        }

        /// <summary>
        /// Deletes the node relationship asynchronous should invoke processor to delete node relationship asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task DeleteNodeRelationshipAsync_ShouldInvokeProcessor_ToDeleteNodeRelationshipAsync()
        {
            var nodeRelationship = new OperativeNodeRelationship();

            this.mockProcessor.Setup(x => x.UpdateNodeRelationshipAsync(nodeRelationship));

            var result = await this.controller.DeleteNodeRelationshipAsync(nodeRelationship).ConfigureAwait(false);
            var objectResult = result as EntityResult;
            var returnMessage = objectResult.Messagekey;

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(objectResult, "Object result should not be null");
            Assert.IsNotNull(returnMessage, "Delete succussfull message should not be null");
            Assert.AreEqual(returnMessage, Entities.Constants.NodeRelationshipDeletedSuccessfully);
            this.mockProcessor.Verify(x => x.UpdateNodeRelationshipAsync(nodeRelationship), Times.Once);
        }

        /// <summary>
        /// Gets the node relationship asynchronous should invoke processor to get node relationship asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task GetNodeRelationshipAsync_ShouldInvokeProcessor_ToGetNodeRelationshipAsync()
        {
            int nodeRelationshipId = 1;

            OperativeNodeRelationship nodeRelationship = new OperativeNodeRelationship();
            this.mockProcessor.Setup(x => x.GetNodeRelationshipAsync(nodeRelationshipId)).ReturnsAsync(nodeRelationship);

            var result = await this.controller.GetNodeRelationshipAsync(nodeRelationshipId).ConfigureAwait(false);
            var objectResult = result as EntityResult;
            var returnMessage = (OperativeNodeRelationship)objectResult.Value;

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(objectResult, "Object result should not be null");
            Assert.IsNotNull(returnMessage, "Node connection should not be null");
            this.mockProcessor.Verify(x => x.GetNodeRelationshipAsync(nodeRelationshipId), Times.Once);
        }

        /// <summary>
        /// Queries the node relationships asynchronous should invoke processor to get all node connection asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task QueryNodeRelationshipsAsync_ShouldInvokeProcessor_ToGetAllNodeConnectionAsync()
        {
            var nodeRelationships = new[] { new OperativeNodeRelationship() }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryAllAsync(It.IsAny<Expression<Func<OperativeNodeRelationship, bool>>>())).ReturnsAsync(nodeRelationships);

            var result = await this.controller.QueryNodeRelationshipsAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.AreEqual(result, nodeRelationships);
            this.mockProcessor.Verify(x => x.QueryAllAsync(It.IsAny<Expression<Func<OperativeNodeRelationship, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Queries the logistic transfer relationship asynchronous should invoke processor to get all node connection asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task QueryLogisticTransferRelationshipAsync_ShouldInvokeProcessor_ToGetAllNodeConnectionAsync()
        {
            var nodeRelationships = new[] { new OperativeNodeRelationshipWithOwnership() }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryAllAsync(It.IsAny<Expression<Func<OperativeNodeRelationshipWithOwnership, bool>>>())).ReturnsAsync(nodeRelationships);

            var result = await this.controller.QueryLogisticTransferRelationshipAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.AreEqual(result, nodeRelationships);
            this.mockProcessor.Verify(x => x.QueryAllAsync(It.IsAny<Expression<Func<OperativeNodeRelationshipWithOwnership, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Creates the node relationship asynchronous should invoke processor to create node connection asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task CreateLogisticTransferRelationshipAsync_ShouldInvokeProcessor_ToCreateNodeRelationshipAsync()
        {
            var nodeRelationship = new OperativeNodeRelationshipWithOwnership();
            this.mockProcessor.Setup(x => x.CreateLogisticTransferRelationshipAsync(nodeRelationship));

            var result = await this.controller.CreateLogisticTransferRelationshipAsync(nodeRelationship).ConfigureAwait(false);
            var objectResult = result as EntityResult;

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(objectResult, "Object result should not be null");
            this.mockProcessor.Verify(x => x.CreateLogisticTransferRelationshipAsync(nodeRelationship), Times.Once);
        }

        /// <summary>
        /// Deletes the node relationship asynchronous should invoke processor to delete node relationship asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task DeleteLogisticTransferRelationshipAsync_ShouldInvokeProcessor_ToDeleteNodeRelationshipAsync()
        {
            var nodeRelationship = new OperativeNodeRelationshipWithOwnership();

            this.mockProcessor.Setup(x => x.DeleteLogisticTransferRelationshipAsync(nodeRelationship));

            var result = await this.controller.DeleteLogisticTransferRelationshipAsync(nodeRelationship).ConfigureAwait(false);
            var objectResult = result as EntityResult;
            var returnMessage = objectResult.Messagekey;

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(objectResult, "Object result should not be null");
            Assert.IsNotNull(returnMessage, "Delete succussfull message should not be null");
            Assert.AreEqual(returnMessage, Entities.Constants.NodeRelationshipDeletedSuccessfully);
            this.mockProcessor.Verify(x => x.DeleteLogisticTransferRelationshipAsync(nodeRelationship), Times.Once);
        }

        /// <summary>
        /// Logistics the transfer relationship exists asynchronous should invoke processor to return node relationship asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task LogisticTransferRelationshipExistsAsync_ShouldInvokeProcessor_ToReturnNodeRelationshipAsync()
        {
            var nodeRelationship = new OperativeNodeRelationshipWithOwnership();
            this.mockProcessor.Setup(m => m.LogisticTransferRelationshipExistsAsync(It.IsAny<OperativeNodeRelationshipWithOwnership>())).ReturnsAsync(nodeRelationship);

            var result = await this.controller.ExistsLogisticTransferRelationshipAsync(nodeRelationship).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            this.mockProcessor.Verify(x => x.LogisticTransferRelationshipExistsAsync(It.IsAny<OperativeNodeRelationshipWithOwnership>()), Times.Once);
        }

        /// <summary>
        /// Logistics the transfer relationship exists asynchronous should invoke processor to return node relationship asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task OperativeTransferRelationshipExistsAsync_ShouldInvokeProcessor_ToReturnNodeRelationshipAsync()
        {
            var nodeRelationship = new OperativeNodeRelationship();
            this.mockProcessor.Setup(m => m.OperativeTransferRelationshipExistsAsync(It.IsAny<OperativeNodeRelationship>())).ReturnsAsync(nodeRelationship);

            var result = await this.controller.ExistsOperativeTransferRelationshipAsync(nodeRelationship).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            this.mockProcessor.Verify(x => x.OperativeTransferRelationshipExistsAsync(It.IsAny<OperativeNodeRelationship>()), Times.Once);
        }
    }
}
