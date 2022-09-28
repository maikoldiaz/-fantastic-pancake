// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidatedMovementNodesAreApprovedSpecificationTests.cs" company="Microsoft">
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
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Processors.Delta.Specifications;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static Ecp.True.Entities.Enumeration.OwnershipNodeStatusType;

    /// <summary>
    /// The consolidatedMovementNodesArePendingOfficialTestsTests test class.
    /// </summary>
    [TestClass]
    public class ConsolidatedMovementNodesAreApprovedSpecificationTests
    {
        /// <summary>
        /// ShouldNotBeSatisfied_WhenNodesAreInApproved.
        /// </summary>
        /// <param name="sourceNodeStatus">The source node status.</param>
        /// <param name="destinationNodeStatus">The destination node status.</param>
        /// <param name="shouldBeSatisfied">Whether the specification should be satisfied.</param>
        [TestMethod]
        [DataRow(APPROVED, APPROVED, false)]
        [DataRow(APPROVED, SENT, false)]
        [DataRow(FAILED, APPROVED, false)]
        [DataRow(DELTAS, LOCKED, true)]
        public void ShouldNotBeSatisfied_WhenNodesAreInApproved(OwnershipNodeStatusType sourceNodeStatus, OwnershipNodeStatusType destinationNodeStatus, bool shouldBeSatisfied)
        {
            // Arrange
            var spec = new ConsolidatedMovementNodesAreApprovedSpecification();
            var movement = GetConsolidatedMovement(sourceNodeStatus, destinationNodeStatus);

            // Act
            var isSatisfied = spec.IsSatisfiedBy(movement);
            var expression = spec.ToExpression().Compile();

            // Assert
            Assert.AreEqual(shouldBeSatisfied, isSatisfied);
            Assert.AreEqual(shouldBeSatisfied, expression(movement));
        }

        private static ConsolidatedMovement GetConsolidatedMovement(OwnershipNodeStatusType sourceNodeStatus, OwnershipNodeStatusType destinationNodeStatus) =>
            new ConsolidatedMovement()
            {
                SourceNode = GetNode(sourceNodeStatus),
                DestinationNode = GetNode(destinationNodeStatus),
            };

        private static Node GetNode(OwnershipNodeStatusType sourceNodeStatus) =>
            new Node
            {
                DeltaNodes =
                {
                    GetDeltaNode(SENT, 5),
                    GetDeltaNode(sourceNodeStatus, 6),
                },
            };

        private static DeltaNode GetDeltaNode(OwnershipNodeStatusType status, int ticketId) =>
            new DeltaNode
            {
                Status = status,
                TicketId = ticketId,
            };
    }
}