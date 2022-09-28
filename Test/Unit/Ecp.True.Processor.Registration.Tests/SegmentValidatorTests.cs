// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SegmentValidatorTests.cs" company="Microsoft">
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
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Registration.Validation;
    using EfCore.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;

    [TestClass]
    public class SegmentValidatorTests
    {
        /// <summary>
        /// The respository factory.
        /// </summary>
        private Mock<IRepositoryFactory> respositoryFactory;

        /// <summary>
        /// The Segment movement validator.
        /// </summary>
        private SegmentValidator<Movement> segmentMovementValidator;

        /// <summary>
        /// The Segment inventory validator.
        /// </summary>
        private SegmentValidator<InventoryProduct> segmentInventoryValidator;

        [TestInitialize]
        public void Initialize()
        {
            this.respositoryFactory = new Mock<IRepositoryFactory>();
            this.segmentMovementValidator = new SegmentValidator<Movement>(this.respositoryFactory.Object);
            this.segmentInventoryValidator = new SegmentValidator<InventoryProduct>(this.respositoryFactory.Object);
        }

        /// <summary>
        /// Validates the null segmentID in movement.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task SegmentValidator_ShouldPassWithNullSegmentIdMovementUnit_WithMovementAsync()
        {
            // Arrange
            var movement = File.ReadAllText("MovementJson/SingleMovement.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);

            // Act
            var result = await this.segmentMovementValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        /// <summary>
        /// Validates the null segmentID in inventory.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task SegmentValidator_ShouldPassWithNullSegmentIdInventoryUnit_WithInventoryAsync()
        {
            // Arrange
            var inventory = File.ReadAllText("InventoryJson/SingleInventory.json");
            var inventoryObject = JsonConvert.DeserializeObject<InventoryProduct>(inventory);
            inventoryObject.SegmentId = null;

            // Act
            var result = await this.segmentInventoryValidator.ValidateAsync(inventoryObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        /// <summary>
        /// Validates the null movement.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "A movement Object of null was inappropriately allowed.")]
        public async Task SegmentValidator_ShouldFailWithNullMovementUnit_WithMovementAsync()
        {
            // Arrange
            Movement movementObject = null;

            // Act
            await this.segmentMovementValidator.ValidateAsync(movementObject).ConfigureAwait(false);
        }

        /// <summary>
        /// Validates the null inventory.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "A inventory Object of null was inappropriately allowed.")]
        public async Task SegmentValidator_ShouldFailWithNullInventoryUnit_WithInventoryAsync()
        {
            // Arrange
            InventoryProduct inventoryObject = null;

            // Act
            await this.segmentInventoryValidator.ValidateAsync(inventoryObject).ConfigureAwait(false);
        }

        /// <summary>
        /// Validate the inventory's sourceid against mapped segemntid must pass.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task InventorySegemntValidator_ShouldPassWithMappedSegmentIdSourceId_WithInventoryAsync()
        {
            // Arrange
            var inventory = File.ReadAllText("InventoryJson/SingleInventory.json");
            var inventoryObject = JsonConvert.DeserializeObject<InventoryProduct>(inventory);
            inventoryObject.SegmentId = 11;

            // Act
            var result = await this.ValidateInventorySegmentIdAsync(1, inventoryObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Validate the inventory's mapped sourceid - Segemntid mapping with wrong startdate and enddate must fail.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task InventorySegemntValidator_ShouldFailWithWrongStartandEndDate_WithInventoryAsync()
        {
            // Arrange
            var inventory = File.ReadAllText("InventoryJson/SingleInventory.json");
            var inventoryObject = JsonConvert.DeserializeObject<InventoryProduct>(inventory);
            inventoryObject.SegmentId = 11;

            // Act
            var result = await this.ValidateInventorySegmentIdAsync(0, inventoryObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(!result);
        }

        /// <summary>
        /// Validate the inventory's sourceid against not mapped segemntid must fail.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), "A inventory Object of null was inappropriately allowed.")]
        public async Task InventorySegemntValidator_ShouldFailWithWrongMappedSegmentIdSourceId_WithInventoryAsync()
        {
            // Arrange
            var nodeTags = new List<NodeTag>();
            var inventory = File.ReadAllText("InventoryJson/SingleInventory.json");
            var inventoryObject = JsonConvert.DeserializeObject<InventoryProduct>(inventory);
            inventoryObject.SegmentId = 7;
            var repos = new Mock<IRepository<NodeTag>>();
            repos.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<NodeTag, bool>>>())).Returns(Task.FromResult<IEnumerable<NodeTag>>(nodeTags));

            // Act
            await this.segmentInventoryValidator.ValidateAsync(inventoryObject).ConfigureAwait(false);
        }

        /// <summary>
        /// Validate the Movement's sourceid against validmapped segemntid must pass.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task MovementSegemntValidator_ShouldPassWithValidMappedSegmentIdMovementSourceId_WithMovementAsync()
        {
            // Arrange
            var movement = File.ReadAllText("MovementJson/SingleMovement.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            movementObject.SegmentId = 11;

            // Act
            var result = await this.ValidateMovementSegmentIdAsync(1, movementObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Validate the Movement's sourceid - segmentid with wrong satartdate and enddate must fail.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task MovementSegemntValidator_ShouldFailWithWrongStartDateandEndDate_WithMovementAsync()
        {
            // Arrange
            var movement = File.ReadAllText("MovementJson/SingleMovement.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            movementObject.SegmentId = 11;

            // Act
            var result = await this.ValidateMovementSegmentIdAsync(0, movementObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(!result);
        }

        private async Task<bool> ValidateInventorySegmentIdAsync(long nodeTagsCount, InventoryProduct inventoryObject)
        {
            var repos = new Mock<IRepository<NodeTag>>();
            repos.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<NodeTag, bool>>>())).ReturnsAsync(nodeTagsCount);
            this.respositoryFactory.Setup(x => x.CreateRepository<NodeTag>()).Returns(repos.Object);
            var result = await this.segmentInventoryValidator.ValidateAsync(inventoryObject).ConfigureAwait(false);
            return result.IsSuccess;
        }

        private async Task<bool> ValidateMovementSegmentIdAsync(long nodeTagsCount, Movement movementObject)
        {
            var repos = new Mock<IRepository<NodeTag>>();
            repos.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<NodeTag, bool>>>())).ReturnsAsync(nodeTagsCount);
            this.respositoryFactory.Setup(x => x.CreateRepository<NodeTag>()).Returns(repos.Object);
            var result = await this.segmentMovementValidator.ValidateAsync(movementObject).ConfigureAwait(false);
            return result.IsSuccess;
        }
    }
}
