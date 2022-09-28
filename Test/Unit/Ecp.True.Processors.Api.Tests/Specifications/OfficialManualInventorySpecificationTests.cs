// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialManualInventorySpecificationTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Tests.Specifications
{
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Entities.Builders;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Api.Specifications;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The officialManualInventoriesTests test class.
    /// </summary>
    [TestClass]
    public class OfficialManualInventorySpecificationTests
    {
        /// <summary>
        ///     The inventories list.
        /// </summary>
        private readonly List<Movement> inventories = new List<Movement>();

        /// <summary>
        ///     The specification.
        /// </summary>
        private OfficialManualInventorySpecification spec;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.spec = new OfficialManualInventorySpecification(
                MovementBuilder.SourceNodeId,
                MovementBuilder.StartTime,
                MovementBuilder.EndTime);
        }

        /// <summary>
        ///     Specification should not be satisfied with deleted inventory.
        /// </summary>
        [TestMethod]
        public void ShouldNotBeSatisfiedWithDeletedInventories()
        {
            // Prepare
            var builder = new MovementBuilder();
            var wrongInventory = builder
                .WithManualSourceSystem()
                .IsDeleted();
            this.inventories.Add(wrongInventory);
            this.inventories.Add(new MovementBuilder()
                .WithDefaultValuesForInventory());

            // Execute
            var isSatisfiedBy = this.spec.IsSatisfiedBy(wrongInventory);

            // Assert
            Assert.IsFalse(isSatisfiedBy);
            Assert.AreEqual(1, this.inventories.Count(m => this.spec.IsSatisfiedBy(m)));
        }

        /// <summary>
        ///     Specification should not be satisfied with already assigned inventory.
        /// </summary>
        [TestMethod]
        public void ShouldNotBeSatisfiedWithAlreadyAssignedMovements()
        {
            // Prepare
            var builder = new MovementBuilder();
            var wrongInventory = builder
                .WithDefaultValuesForInventory()
                .WithTicketId(123);
            this.inventories.Add(wrongInventory);
            this.inventories.Add(new MovementBuilder()
                .WithDefaultValuesForInventory()
                .WithNullTicket());

            // Execute
            var isSatisfiedBy = this.spec.IsSatisfiedBy(wrongInventory);

            // Assert
            Assert.IsFalse(isSatisfiedBy);
            Assert.AreEqual(1, this.inventories.Count(m => this.spec.IsSatisfiedBy(m)));
        }

        /// <summary>
        ///     Specification should not be satisfied with already assigned inventory.
        /// </summary>
        [TestMethod]
        public void ShouldBeSatisfiedWithNotAlreadyAssignedMovements()
        {
            // Prepare
            var builder = new MovementBuilder();
            var wrongInventory = builder
                .WithDefaultValuesForInventory()
                .WithNullTicket();
            this.inventories.Add(wrongInventory);
            this.inventories.Add(new MovementBuilder()
                .WithDefaultValuesForInventory()
                .WithNullTicket());

            // Execute
            var isSatisfiedBy = this.spec.IsSatisfiedBy(wrongInventory);

            // Assert
            Assert.IsTrue(isSatisfiedBy);
            Assert.AreEqual(2, this.inventories.Count(m => this.spec.IsSatisfiedBy(m)));
        }

        /// <summary>
        ///     Specification should not be satisfied with movements with different period.
        /// </summary>
        [TestMethod]
        public void ShouldNotBeSatisfiedWithInventoryWithinThePeriod()
        {
            // Prepare
            var wrongInventory = new MovementBuilder()
                .WithEndTime(MovementBuilder.EndTime.AddDays(-2))
                .WithStartTime(MovementBuilder.StartTime.AddDays(2));
            this.inventories.Add(wrongInventory);
            this.inventories.Add(new MovementBuilder()
                .WithDefaultValuesForInventory());

            // Execute
            var isSatisfiedBy = this.spec.IsSatisfiedBy(wrongInventory);

            // Assert
            Assert.IsFalse(isSatisfiedBy);
            Assert.AreEqual(1, this.inventories.Count(m => this.spec.IsSatisfiedBy(m)));
        }
    }
}