// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapControllerTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Sap.Api.Tests
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Host.Sap.Api.Controllers;
    using Ecp.True.Processors.Transform.Input.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The category controller tests.
    /// </summary>
    [TestClass]
    public class SapControllerTests
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private SapController controller;

        /// <summary>
        /// The mock processor.
        /// </summary>
        private Mock<IInputFactory> mockProcessor;

        /// <summary>
        /// The mock business context.
        /// </summary>
        private Mock<IBusinessContext> mockBusinessContext;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockProcessor = new Mock<IInputFactory>();
            this.mockBusinessContext = new Mock<IBusinessContext>();

            this.mockBusinessContext.Setup(m => m.ActivityId).Returns(Guid.NewGuid());
            this.controller = new SapController(this.mockBusinessContext.Object, this.mockProcessor.Object);
        }

        /// <summary>
        /// Updates the ownership node status asynchronous should invoke processor to update ownership node status asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task CreateMovementsAsync_ShouldInvokeProcessor_ToCreateMovementsAsync()
        {
            var movements = new[]
            {
                new SapMovement(),
            };
            this.mockProcessor.Setup(m => m.SaveSapJsonArrayAsync(It.IsAny<object>(), It.IsAny<TrueMessage>()));

            var result = await this.controller.CreateMovementsAsync(movements).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.SaveSapJsonArrayAsync(It.IsAny<object>(), It.Is<TrueMessage>(m => m.Message == Entities.Core.MessageType.Movement && !m.ShouldHomologate)), Times.Once());
        }

        /// <summary>
        /// Updates the official movements asynchronous single movement scenario should invoke processor to update movements asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task UpdateOfficialMovementsAsync_SingleMovementScenario_ShouldInvokeProcessor_ToUpdateMovementsAsync()
        {
            SapMovement officialMovement = new SapMovement
            {
                MovementId = "2",
                BackupMovement = new BackupMovement
                {
                    IsOfficial = true,
                    GlobalMovementId = "123",
                },
            };

            var movements = new[]
            {
                officialMovement,
            };
            this.mockProcessor.Setup(m => m.SaveSapJsonArrayAsync(It.IsAny<object>(), It.IsAny<TrueMessage>()));

            var result = await this.controller.UpdateOfficialPointsAsync(movements).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.SaveSapJsonArrayAsync(It.IsAny<object>(), It.Is<TrueMessage>(m => m.Message == Entities.Core.MessageType.Movement && !m.ShouldHomologate)), Times.Once());
        }

        /// <summary>
        /// Updates the official movements asynchronous multiple movement scenario should invoke processor to update movements asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task UpdateOfficialMovementsAsync_MultipleMovementScenario_ShouldInvokeProcessor_ToUpdateMovementsAsync()
        {
            SapMovement backupMovement = new SapMovement
            {
                MovementId = "1",
                BackupMovement = new BackupMovement
                {
                    IsOfficial = false,
                    GlobalMovementId = "123",
                },
            };

            SapMovement officialMovement = new SapMovement
            {
                MovementId = "2",
                BackupMovement = new BackupMovement
                {
                    IsOfficial = true,
                    GlobalMovementId = "123",
                    BackupMovementId = "1",
                },
            };

            var movements = new[]
            {
                backupMovement,officialMovement,
            };
            this.mockProcessor.Setup(m => m.SaveSapJsonArrayAsync(It.IsAny<object>(), It.IsAny<TrueMessage>()));

            var result = await this.controller.UpdateOfficialPointsAsync(movements).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.SaveSapJsonArrayAsync(It.IsAny<object>(), It.Is<TrueMessage>(m => m.Message == Entities.Core.MessageType.Movement && !m.ShouldHomologate)), Times.Once());
        }

        /// <summary>
        /// Updates the ownership node status asynchronous should invoke processor to update ownership node status asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task CreateInventoriesAsync_ShouldInvokeProcessor_ToCreateInventoriesAsync()
        {
            var inventories = new[]
            {
                new SapInventory(),
            };
            this.mockProcessor.Setup(m => m.SaveSapJsonArrayAsync(It.IsAny<object>(), It.IsAny<TrueMessage>()));

            var result = await this.controller.CreateInventoryAsync(inventories).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.SaveSapJsonArrayAsync(It.IsAny<object>(), It.Is<TrueMessage>(m => m.Message == Entities.Core.MessageType.Inventory && !m.ShouldHomologate)), Times.Once());
        }
    }
}
