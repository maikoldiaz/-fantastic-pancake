// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementControllerTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Tests.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Host.Api.Controllers;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The MovementController test class.
    /// </summary>
    [TestClass]
    public class MovementControllerTests
    {
        private Mock<IManualMovementProcessor> mockProcessor;
        private MovementController movementController;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockProcessor = new Mock<IManualMovementProcessor>();
            this.movementController = new MovementController(this.mockProcessor.Object);
        }

        [TestMethod]
        public async Task ShouldInvokeMovementProcessorAsync()
        {
            // Prepare
            this.mockProcessor
                .Setup(p => p.GetAssignableMovementsAsync(
                    It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(new List<Movement>().AsQueryable()));

            // Execute
            var startTime = DateTime.Now.AddDays(-1);
            var endTime = DateTime.Now;
            await this.movementController.QueryManualMovementsAsync(1, startTime, endTime)
                .ConfigureAwait(false);

            // Assert
            this.mockProcessor.Verify(m => m.GetAssignableMovementsAsync(1, startTime, endTime), Times.Once);
        }

        /// <summary>
        /// UpdateTicketShouldInvokeProcessorAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateTicketShouldInvokeProcessorAsync()
        {
            // Prepare
            var deltaNode = new DeltaNode()
            {
                DeltaNodeId = 2,
            };

            // Execute
            await this.movementController.UpdateManualMovementsForTicketAsync(deltaNode.DeltaNodeId, new[] { 1, 2, 3 }).ConfigureAwait(false);

            // Assert
            this.mockProcessor.Verify(p => p.UpdateTicketManualMovementsAsync(deltaNode.DeltaNodeId, new[] { 1, 2, 3 }));
        }
    }
}