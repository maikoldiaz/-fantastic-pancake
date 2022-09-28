// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeCostCenterControllerTests.cs" company="Microsoft">
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
    public class NodeCostCenterControllerTests : ControllerTestBase
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private NodeCostCenterController controller;

        /// <summary>
        /// The mock processor.
        /// </summary>
        private Mock<INodeCostCenterProcessor> mockProcessor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockProcessor = new Mock<INodeCostCenterProcessor>();
            this.controller = new NodeCostCenterController(this.mockProcessor.Object);
        }

        /// <summary>
        /// Querie the node cost centers asynchronous should invoke processor to get all node connection asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task QueryNodeCostCentersAsync_ShouldInvokeProcessor_ToGetAllNodeCostCenterAsync()
        {
            // Prepare
            var nodeCostCenters = new[] { new NodeCostCenter() { } }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryAllAsync(It.IsAny<Expression<Func<NodeCostCenter, bool>>>())).ReturnsAsync(nodeCostCenters);

            // Execute
            var result = await this.controller.QueryNodeCostCentersAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.AreEqual(result, nodeCostCenters);
            this.mockProcessor.Verify(x => x.QueryAllAsync(It.IsAny<Expression<Func<NodeCostCenter, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Creates the node cost centers asynchronous should invoke processor to create all node connection asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task CreateNodeCostCenterAsync_ShouldInvokeProcessor_ToCreateNodeCostCenterAsync()
        {
            // Prepare
            var nodeCostCenters = new List<NodeCostCenter> { new NodeCostCenter { NodeCostCenterId = 1 } };
            var nodeCostCenterInfos = new List<NodeCostCenterInfo> { new NodeCostCenterInfo { NodeCostCenter = nodeCostCenters.FirstOrDefault() } };

            this.mockProcessor.Setup(m => m.CreateNodeCostCentersAsync(It.IsAny<List<NodeCostCenter>>())).ReturnsAsync(nodeCostCenterInfos);

            // Execute
            var result = await this.controller.CreateNodeCostCenterAsync(nodeCostCenters).ConfigureAwait(false);
            var objectResult = result as EntityResult;

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(objectResult, "Ok Result should not be null");
            this.mockProcessor.Verify(x => x.CreateNodeCostCentersAsync(nodeCostCenters), Times.Once);
        }

        /// <summary>
        /// Update the node cost centers asynchronous should invoke processor to update all node connection asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task UpdateNodeCostCenterAsync_ShouldInvokeProcessor_ToUpodateNodeCostCenterAsync()
        {
            // Prepare
            var nodeCostCenter = new NodeCostCenter();
            this.mockProcessor.Setup(m => m.UpdateNodeCostCenterAsync(nodeCostCenter)).ReturnsAsync(new NodeCostCenter());

            // Execute
            await this.controller.UpdateNodeCostCenterAsync(nodeCostCenter).ConfigureAwait(false);

            // Assert
            this.mockProcessor.Verify(p => p.UpdateNodeCostCenterAsync(nodeCostCenter), Times.Once);
        }

        /// <summary>
        /// Delete the node cost centers asynchronous should invoke processor to create node connection asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task DeleteNodeCostCenterAsync_ShouldInvokeProcessor_ToDeleteNodeCostCenterAsync()
        {
            // Prepare
            var nodeCostCenter = new NodeCostCenter { NodeCostCenterId = 1 };

            this.mockProcessor.Setup(m => m.DeleteNodeCostCenterAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Execute
            var result = await this.controller.DeleteNodeCostCenterAsync(nodeCostCenter.NodeCostCenterId).ConfigureAwait(false);
            var objectResult = result as EntityResult;

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(objectResult, "Ok Result should not be null");
            this.mockProcessor.Verify(x => x.DeleteNodeCostCenterAsync(nodeCostCenter.NodeCostCenterId), Times.Once);
        }
    }
}