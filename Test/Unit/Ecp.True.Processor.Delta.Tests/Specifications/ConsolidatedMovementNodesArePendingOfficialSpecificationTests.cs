// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidatedMovementNodesArePendingOfficialSpecificationTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Delta.Tests.Specifications
{
    using System.Collections.Generic;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Processors.Delta.Specifications;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The consolidatedMovementNodesAreApprovedSpecificationTests test class.
    /// </summary>
    [TestClass]
    public class ConsolidatedMovementNodesArePendingOfficialSpecificationTests
    {
        private readonly List<int> pendingNodes = new List<int> { 1, 2, 3 };
        private ConsolidatedMovementNodesArePendingOfficialSpecification spec;

        [TestInitialize]
        public void Initialize()
        {
            this.spec = new ConsolidatedMovementNodesArePendingOfficialSpecification(this.pendingNodes);
        }

        /// <summary>
        /// ShouldBeSatisfied_WhenTheDestinationNodeIsContainedInThePendingNodesCollection.
        /// </summary>
        /// <param name="destinationNodeId">The destination node.</param>
        /// <param name="sourceNodeId">The source node.</param>
        /// <param name="shouldBeSatisfied">Should be satisfied.</param>
        [TestMethod]
        [DataRow(1, 7, true)]
        [DataRow(7, 1, true)]
        [DataRow(7, 8, false)]
        public void ShouldBeSatisfied_WhenTheDestinationNodeIsContainedInThePendingNodesCollection(int destinationNodeId, int sourceNodeId, bool shouldBeSatisfied)
        {
            // Arrange
            var movement = new ConsolidatedMovement
            {
                DestinationNodeId = destinationNodeId,
                SourceNodeId = sourceNodeId,
            };

            // Act
            var isSatisfied = this.spec.IsSatisfiedBy(movement);
            var expression = this.spec.ToExpression().Compile();

            // Assert
            Assert.AreEqual(shouldBeSatisfied, isSatisfied);
            Assert.AreEqual(shouldBeSatisfied, expression(movement));
        }
    }
}