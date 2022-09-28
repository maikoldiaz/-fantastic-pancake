// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnerValidatorTests.cs" company="Microsoft">
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
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Registration.Validation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;

    [TestClass]
    public class OwnerValidatorTests
    {
        /// <summary>
        /// The data annotation validator.
        /// </summary>
        private OwnershipValidator<Movement> ownerValidator;

        /// <summary>
        /// The respository factory.
        /// </summary>
        private Mock<IRepositoryFactory> respositoryFactory;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.respositoryFactory = new Mock<IRepositoryFactory>();
            var repos = new Mock<IRepository<CategoryElement>>();
            this.respositoryFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(repos.Object);
            this.ownerValidator = new OwnershipValidator<Movement>(this.respositoryFactory.Object);
        }

        /// <summary>
        /// Validates the movement for incorrect volume.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProductValidator_ShouldValidateWithErrorForVolumeUnit_WithMovementAsync()
        {
            this.InitializeCategoryElementRepository(null, false);
            var movement = File.ReadAllText("MovementJson/InvalidMovementVolume.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            var result = await this.ownerValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Validates the movement for different volume.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProductValidator_ShouldValidateWithErrorForDifferentVolumeUnitAsync()
        {
            var movement = File.ReadAllText("MovementJson/InvalidMovementOwnershipValueUnit.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            var result = await this.ownerValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Validates the movement for invalid total percentage.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProductValidator_ShouldValidateWithErrorForIncorrectTotalPercentageAsync()
        {
            this.InitializeCategoryElementRepository(null, true);
            var movement = File.ReadAllText("MovementJson/InvalidMovementIncorrectTotalPercentage.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            var result = await this.ownerValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Validates the movement for invalid total percentage but into of deviation.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProductValidator_ShouldValidateWithErrorForIncorrectTotalPercentageIntoDeviationAsync()
        {
            // Arrange
            this.InitializeCategoryElementRepository(60.2m, true);
            var movement = File.ReadAllText("MovementJson/InvalidMovementIncorrectTotalPercentage.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);

            // Act
            var result = await this.ownerValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        /// <summary>
        /// Validates the movement for correct total percentage.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProductValidator_ShouldValidateWithErrorForCorrectTotalPercentageAsync()
        {
            this.InitializeCategoryElementRepository(null, true);
            var movement = File.ReadAllText("MovementJson/MovementCorrectTotalPercentage.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            var result = await this.ownerValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            Assert.AreEqual(0, result.ErrorInfo.Count);
        }

        /// <summary>
        /// Validates the movement for no owner specified.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProductValidator_ShouldValidateWithErrorForNoOnwerAsync()
        {
            this.InitializeCategoryElementRepository(null, true);
            var movement = File.ReadAllText("MovementJson/MovementWithNoOwner.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            var result = await this.ownerValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            Assert.AreEqual(0, result.ErrorInfo.Count);
        }

        private void InitializeCategoryElementRepository(decimal? deviationPercentage, bool isOperationalSegment)
        {
            var repos = new Mock<IRepository<CategoryElement>>();
            repos.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new CategoryElement { DeviationPercentage = deviationPercentage, IsOperationalSegment = isOperationalSegment });
            this.respositoryFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(repos.Object);
        }
    }
}
