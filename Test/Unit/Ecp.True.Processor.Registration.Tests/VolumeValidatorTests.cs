// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeValidatorTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
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
    public class VolumeValidatorTests
    {
        /// <summary>
        /// The data annotation validator.
        /// </summary>
        private VolumeValidator<Movement> volumeValidator;

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
            this.volumeValidator = new VolumeValidator<Movement>(this.respositoryFactory.Object);
        }

        /// <summary>
        /// Validates the movement for invalid total percentage but into of deviation.
        /// </summary>
        /// <param name="fileName">The filename.</param>
        /// <returns>The Task.</returns>
        [TestMethod]
        [DataRow("MovementJson/InvalidMovementIncorrectTotalPercentage.json")]
        [DataRow("MovementJson/InvalidMovementIncorrectTotalPercentageWithoutOwners.json")]
        public async Task ProductValidator_ShouldPassValidation_WhenSegmentIsNoSonAndPercentageIsIncorrectAsync(string fileName)
        {
            // Arrange
            this.InitializeCategoryElementRepository(60.2m, false);
            var movement = File.ReadAllText(fileName);
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);

            // Act
            var result = await this.volumeValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        /// <summary>
        /// Validates the movement for correct total percentage.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The Task.</returns>
        [TestMethod]
        [DataRow("MovementJson/MovementCorrectTotalPercentageMoreDecimalPlaces.json")]
        [DataRow("MovementJson/MovementCorrectTotalPercentageMoreDecimalPlacesWithoutOwners.json")]
        public async Task ProductValidator_ShouldPassValidation_WhenSegmentIsNoSonAndThereAreMoreThanTwoDecimalPlacestAsync(string fileName)
        {
            this.InitializeCategoryElementRepository(0.4m, false);
            var movement = File.ReadAllText(fileName);
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            var result = await this.volumeValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            Assert.AreEqual(0, result.ErrorInfo.Count);
            Assert.IsTrue(result.IsSuccess);
        }

        /// <summary>
        /// Validates the movement for correct total percentage.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The Task.</returns>
        [TestMethod]
        [DataRow("MovementJson/MovementCorrectTotalPercentageMoreDecimalPlaces.json")]
        [DataRow("MovementJson/MovementCorrectTotalPercentageMoreDecimalPlacesWithoutOwners.json")]
        public async Task ProductValidator_ShouldFailValidation_WhenSegmentIsSonAndThereAreMoreThanTwoDecimalPlacestAsync(string fileName)
        {
            this.InitializeCategoryElementRepository(0.4m, true);
            var movement = File.ReadAllText(fileName);
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            var result = await this.volumeValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            Assert.AreEqual(1, result.ErrorInfo.Count);
            Assert.IsFalse(result.IsSuccess);
        }

        private void InitializeCategoryElementRepository(decimal? deviationPercentage, bool isOperationalSegment)
        {
            var repos = new Mock<IRepository<CategoryElement>>();
            repos.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new CategoryElement { DeviationPercentage = deviationPercentage, IsOperationalSegment = isOperationalSegment });
            this.respositoryFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(repos.Object);
        }
    }
}