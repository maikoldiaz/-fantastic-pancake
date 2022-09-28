// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataAnnotationValidatorTests.cs" company="Microsoft">
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
    using System.IO;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Registration.Validation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;

    [TestClass]
    public class DataAnnotationValidatorTests
    {
        /// <summary>
        /// The DataAnnotationValidator movement validator.
        /// </summary>
        private DataAnnotationValidator<Movement> dataAnnotationMovementValidator;

        /// <summary>
        /// The DataAnnotationValidator inventory validator.
        /// </summary>
        private DataAnnotationValidator<InventoryProduct> dataAnnotationInventoryValidator;

        [TestInitialize]
        public void Initialize()
        {
            this.dataAnnotationMovementValidator = new DataAnnotationValidator<Movement>();
            this.dataAnnotationInventoryValidator = new DataAnnotationValidator<InventoryProduct>();
        }

        /// <summary>
        /// Validates the inventory type object should pass.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DataAnnotationValidator_ShouldPassWithInventory_WithValidateAsync()
        {
            // Arrange
            var inventory = File.ReadAllText("InventoryJson/SingleInventory.json");
            var inventoryObject = JsonConvert.DeserializeObject<InventoryProduct>(inventory);

            // Act
            var result = await this.dataAnnotationInventoryValidator.ValidateAsync(inventoryObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        /// <summary>
        /// Validates the movement type object should pass.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DataAnnotationValidator_ShouldPassWithMovement_WithValidateAsync()
        {
            // Arrange
            var movement = File.ReadAllText("MovementJson/SingleMovement.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);

            // Act
            var result = await this.dataAnnotationMovementValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        /// <summary>
        /// Validates the invalid inventory type object should fail.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DataAnnotationValidator_ShouldFailWithInvalidinventory_WithValidateAsync()
        {
            // Arrange
            InventoryProduct inventoryObject = new InventoryProduct();

            // Act
            var result = await this.dataAnnotationInventoryValidator.ValidateAsync(inventoryObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(!result.IsSuccess);
        }

        /// <summary>
        /// Validates the invalid movement type object should fail.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DataAnnotationValidator_ShouldFailWithInvalidMovement_WithValidateAsync()
        {
            // Arrange
            Movement movementObject = new Movement();

            // Act
            var result = await this.dataAnnotationMovementValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.IsSuccess);
            Assert.IsNotNull(result.ErrorInfo);
        }

        /// <summary>
        /// Validates the invalid movement type object should fail.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DataAnnotationValidator_ShouldFailWithInvalidMovementSource_WithValidateAsync()
        {
            // Arrange
            Movement movementObject = new Movement();
            movementObject.MovementSource = new MovementSource();

            // Act
            var result = await this.dataAnnotationMovementValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.IsSuccess);
            Assert.IsNotNull(result.ErrorInfo);
        }
    }
}
