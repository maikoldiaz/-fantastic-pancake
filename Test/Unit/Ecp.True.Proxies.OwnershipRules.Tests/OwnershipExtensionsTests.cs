// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipExtensionsTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.OwnershipRules.Tests
{
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Proxies.OwnershipRules.OfficialDeltaDataRequest;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static System.Globalization.CultureInfo;
    using static Ecp.True.Entities.Enumeration.MovementType;

    /// <summary>
    /// The ownershipExtensionsTests test class.
    /// </summary>
    [TestClass]
    public class OwnershipExtensionsTests
    {
        /// <summary>
        /// ShouldReturnWhetherTheOfficialDeltaConsolidatedMovementIsOfTheGivenType.
        /// </summary>
        /// <param name="type">The movement type.</param>
        /// <param name="isOftype">Whether the movement is of the given type.</param>
        [TestMethod]
        [DataRow(UnidentifiedLoss, true)]
        [DataRow(Tolerance, false)]
        public void ShouldReturnWhetherTheOfficialDeltaConsolidatedMovementIsOfTheGivenType(MovementType type, bool isOftype)
        {
            // Arrange
            var movement = new OfficialDeltaConsolidatedMovement
            {
                MovementTypeId = ((int)UnidentifiedLoss).ToString(InvariantCulture),
            };

            // Act
            bool actualIsOftype = movement.IsOfType(type);

            // Assert
            Assert.AreEqual(isOftype, actualIsOftype);
        }
    }
}