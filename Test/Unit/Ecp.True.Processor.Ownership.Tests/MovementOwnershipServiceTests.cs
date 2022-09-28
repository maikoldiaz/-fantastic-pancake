// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementOwnershipServiceTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Ownership.Tests
{
    using System.Collections.Generic;
    using Ecp.True.Processors.Ownership.Services;
    using Ecp.True.Proxies.OwnershipRules.Response;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The MovementOwnershipServiceTests.
    /// </summary>
    [TestClass]
    public class MovementOwnershipServiceTests
    {
        /// <summary>
        /// The movement ownership service.
        /// </summary>
        private MovementOwnershipService movementOwnershipService;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.movementOwnershipService = new MovementOwnershipService();
        }

        /// <summary>
        /// Gets the movement ownerships should process.
        /// </summary>
        [TestMethod]
        public void GetMovementOwnershipsc_ShouldProcess()
        {
            var ownershipResultMovement = new OwnershipResultMovement
            {
                AppliedRule = "Rule One",
                ResponseMovementId = "1",
                OwnerId = 1,
                OwnershipPercentage = 12.4M,
                OwnershipVolume = 15.5M,
                RuleVersion = 1,
                ResponseTicket = "12",
            };

            var movementList = new List<OwnershipResultMovement> { ownershipResultMovement };

            // Act
            var result = this.movementOwnershipService.GetMovementOwnerships(movementList);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
