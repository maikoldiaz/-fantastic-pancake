// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialManualMovementSpecificationTests.cs" company="Microsoft">
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
    ///     The manualMovementInventoriesSpecificationTests test class.
    /// </summary>
    [TestClass]
    public class OfficialManualMovementSpecificationTests
    {
        /// <summary>
        ///     The movement list.
        /// </summary>
        private readonly List<Movement> movements = new List<Movement>();

        /// <summary>
        ///     The specification.
        /// </summary>
        private OfficialManualMovementsSpecification spec;

        [TestInitialize]
        public void Initialize()
        {
            this.spec = new OfficialManualMovementsSpecification(
                MovementBuilder.SourceNodeId,
                MovementBuilder.StartTime,
                MovementBuilder.EndTime);
        }

        /// <summary>
        ///     Specification should not be satisfied with deleted movements.
        /// </summary>
        [TestMethod]
        public void ShouldNotBeSatisfiedWithDeletedMovements()
        {
            // Prepare
            var builder = new MovementBuilder();
            var wrongMovement = builder.IsDeleted();
            this.movements.Add(wrongMovement);
            this.movements.Add(new MovementBuilder());

            // Execute
            var isSatisfiedBy = this.spec.IsSatisfiedBy(wrongMovement);

            // Assert
            Assert.IsFalse(isSatisfiedBy);
            Assert.AreEqual(1, this.movements.Count(m => this.spec.IsSatisfiedBy(m)));
        }

        /// <summary>
        ///     Specification should not be satisfied with already assigned movements.
        /// </summary>
        [TestMethod]
        public void ShouldNotBeSatisfiedWithAlreadyAssignedMovements()
        {
            // Prepare
            var builder = new MovementBuilder();
            var wrongMovement = builder.WithTicketId(123);
            this.movements.Add(wrongMovement);
            this.movements.Add(new MovementBuilder());

            // Execute
            var isSatisfiedBy = this.spec.IsSatisfiedBy(wrongMovement);

            // Assert
            Assert.IsFalse(isSatisfiedBy);
            Assert.AreEqual(1, this.movements.Count(m => this.spec.IsSatisfiedBy(m)));
        }

        /// <summary>
        ///     Specification should not be satisfied with operative movements.
        /// </summary>
        [TestMethod]
        public void ShouldNotBeSatisfiedWithOperativeMovements()
        {
            // Prepare
            var wrongMovement = new MovementBuilder()
                .IsOperative();
            this.movements.Add(wrongMovement);
            this.movements.Add(new MovementBuilder());

            // Execute
            var isSatisfiedBy = this.spec.IsSatisfiedBy(wrongMovement);

            // Assert
            Assert.IsFalse(isSatisfiedBy);
            Assert.AreEqual(1, this.movements.Count(m => this.spec.IsSatisfiedBy(m)));
        }

        /// <summary>
        ///     Specification should not be satisfied with movements with different period.
        /// </summary>
        [TestMethod]
        public void ShouldNotBeSatisfiedWithMovementsWithDifferentPeriod()
        {
            // Prepare
            var wrongMovement = new MovementBuilder()
                .WithStartTime(MovementBuilder.StartTime.AddDays(-1));
            this.movements.Add(wrongMovement);
            this.movements.Add(new MovementBuilder());

            // Execute
            var isSatisfiedBy = this.spec.IsSatisfiedBy(wrongMovement);

            // Assert
            Assert.IsFalse(isSatisfiedBy);
            Assert.AreEqual(1, this.movements.Count(m => this.spec.IsSatisfiedBy(m)));
        }

        /// <summary>
        ///     Specification should not be satisfied with movements from a different node.
        /// </summary>
        [TestMethod]
        public void ShouldNotBeSatisfiedWithMovementsFromADifferentNode()
        {
            // Prepare
            var wrongMovement = new MovementBuilder()
                .WithSourceNodeId(5);
            this.movements.Add(wrongMovement);
            this.movements.Add(new MovementBuilder());

            // Execute
            var isSatisfiedBy = this.spec.IsSatisfiedBy(wrongMovement);

            // Assert
            Assert.IsFalse(isSatisfiedBy);
            Assert.AreEqual(1, this.movements.Count(m => this.spec.IsSatisfiedBy(m)));
        }

        /// <summary>
        ///     Specification should not be satisfied with movements or inventories that are not manual.
        /// </summary>
        [TestMethod]
        public void ShouldNotBeSatisfiedWithNotManualMovements()
        {
            // Prepare
            var wrongMovement = new MovementBuilder()
                .WithNotManualSourceSystem();
            this.movements.Add(wrongMovement);
            this.movements.Add(new MovementBuilder());
            var inventory = new MovementBuilder();
            this.movements.Add(inventory);

            // Execute
            var isSatisfiedBy = this.spec.IsSatisfiedBy(wrongMovement);

            // Assert
            Assert.IsFalse(isSatisfiedBy);
            Assert.AreEqual(2, this.movements.Count(m => this.spec.IsSatisfiedBy(m)));
        }
    }
}