// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClassificationValidatorTests.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Registration.Validation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The classification validator tests.
    /// </summary>
    [TestClass]
    public class ClassificationValidatorTests
    {
        /// <summary>
        /// The validator.
        /// </summary>
        private ClassificationValidator<Movement> validator;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.validator = new ClassificationValidator<Movement>();
        }

        /// <summary>
        /// Validates the should return success when movement classification is valid.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task Validate_ShouldReturnSuccess_WhenClassificationIsMovementAsync()
        {
            var movement = new Movement
            {
                Classification = Core.Constants.MovementClassification,
            };

            var result = await this.validator.ValidateAsync(movement).ConfigureAwait(false);
            Assert.IsTrue(result.IsSuccess);
        }

        /// <summary>
        /// Validates the should return success when movement classification is valid.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task Validate_ShouldReturnSuccess_WhenClassificationIsSpecialMovementAsync()
        {
            var movement = new Movement
            {
                Classification = Core.Constants.SpecialMovementClassification,
            };

            var result = await this.validator.ValidateAsync(movement).ConfigureAwait(false);
            Assert.IsTrue(result.IsSuccess);
        }

        /// <summary>
        /// Validates the should return success when movement classification is valid.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task Validate_ShouldReturnSuccess_WhenClassificationIsLossAsync()
        {
            var movement = new Movement
            {
                Classification = Core.Constants.LossClassification,
            };

            var result = await this.validator.ValidateAsync(movement).ConfigureAwait(false);
            Assert.IsTrue(result.IsSuccess);
        }

        /// <summary>
        /// Validates the should return success when movement classification is valid.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task Validate_ShouldReturnSuccess_WhenClassificationIsInvalidAsync()
        {
            var movement = new Movement();

            var result = await this.validator.ValidateAsync(movement).ConfigureAwait(false);

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(1, result.ErrorInfo.Count);
            Assert.AreEqual(Processors.Registration.Constants.InvalidClassificationMessage, result.ErrorInfo[0].Message);
        }
    }
}
