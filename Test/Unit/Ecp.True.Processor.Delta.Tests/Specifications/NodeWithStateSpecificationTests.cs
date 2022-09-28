// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeWithStateSpecificationTests.cs" company="Microsoft">
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
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Processors.Delta.Specifications;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static Ecp.True.Entities.Enumeration.OwnershipNodeStatusType;

    /// <summary>
    /// The nodeWithStateSpecificationTests test class.
    /// </summary>
    [TestClass]
    public class NodeWithStateSpecificationTests
    {
        private ConsolidatedMovement consolidatedMovement;

        /// <summary>
        /// IsSatisfied_ShouldReturnTrue_WhenNodeIsNull.
        /// </summary>
        [TestMethod]
        public void IsSatisfied_ShouldReturnTrue_WhenSourceNodeIsNull()
        {
            // Arrange
            this.consolidatedMovement = new ConsolidatedMovement()
            {
                SourceNode = null,
            };
            var spec = new NodeNotInStateSpecification(m => m.SourceNode, APPROVED);

            // Act
            var isSatisfied = spec.IsSatisfiedBy(this.consolidatedMovement);

            // Assert
            Assert.IsTrue(isSatisfied);
        }

        /// <summary>
        /// IsSatisfied_ShouldReturnTrue_WhenNodeIsNull.
        /// </summary>
        [TestMethod]
        public void IsSatisfied_ShouldReturnTrue_WhenDestinationNodeIsNull()
        {
            // Arrange
            this.consolidatedMovement = new ConsolidatedMovement()
            {
                DestinationNode = null,
            };
            var spec = new NodeNotInStateSpecification(m => m.DestinationNode, APPROVED);

            // Act
            var isSatisfied = spec.IsSatisfiedBy(this.consolidatedMovement);

            // Assert
            Assert.IsTrue(isSatisfied);
        }

        /// <summary>
        /// ShouldBeSatisfied_WithLastTicketOnly.
        /// </summary>
        [TestMethod]
        public void ShouldBeSatisfied_WithLastTicketOnly()
        {
            // Arrange
            this.consolidatedMovement = this.GetTestNotApprovedConsolidatedMovement();
            var spec = new NodeNotInStateSpecification(m => m.DestinationNode, APPROVED);

            // Act
            var isSatisfied = this.TestConsolidatedMethod(spec);

            // Assert
            Assert.IsFalse(isSatisfied);
        }

        /// <summary>
        /// ShouldBeSatisfied_WithNodesWithoutDeltaNodes.
        /// </summary>
        [TestMethod]
        public void ShouldBeSatisfied_WithNodesWithoutDeltaNodes()
        {
            // Arrange
            this.consolidatedMovement = this.GetMovementWithoutDeltaNodes();
            var spec = new NodeNotInStateSpecification(m => m.DestinationNode, APPROVED);

            // Act
            var isSatisfied = this.TestConsolidatedMethod(spec);

            // Assert
            Assert.IsTrue(isSatisfied);
        }

        /// <summary>
        /// ShouldHave_IncludesForBothNodesAndDeltaNodes.
        /// </summary>
        [TestMethod]
        public void ShouldHave_IncludesForBothNodesAndDeltaNodes()
        {
            // Arrange
            var spec = new NodeNotInStateSpecification(m => m.DestinationNode, APPROVED);
            var requiredIncludes = new List<string>
            {
                $"{nameof(ConsolidatedMovement.DestinationNode)}",
                $"{nameof(ConsolidatedMovement.DestinationNode)}.{nameof(Node.DeltaNodes)}",
                $"{nameof(ConsolidatedMovement.SourceNode)}",
                $"{nameof(ConsolidatedMovement.SourceNode)}.{nameof(Node.DeltaNodes)}",
            };

            // Act
            var actualIncludes = spec.IncludeProperties;

            // Assert
            Assert.IsTrue(requiredIncludes.TrueForAll(r => actualIncludes.Contains(r)));
        }

        private static Node GetTestNode(int nodeId)
        {
            return new Node { NodeId = nodeId };
        }

        private static DeltaNode GetDeltaNode(int ticketId, OwnershipNodeStatusType ownershipNodeStatusType)
        {
            return new DeltaNode
            {
                Node = GetTestNode(1),
                NodeId = GetTestNode(1).NodeId,
                Status = ownershipNodeStatusType,
                TicketId = ticketId,
            };
        }

        private bool TestConsolidatedMethod(NodeNotInStateSpecification spec)
        {
            var isSatisfied = spec.IsSatisfiedBy(this.consolidatedMovement);
            return isSatisfied;
        }

        private ConsolidatedMovement GetTestNotApprovedConsolidatedMovement()
        {
            var testApprovedNode = GetTestNode(1);
            testApprovedNode.DeltaNodes.AddRange(new List<DeltaNode>
            {
                GetDeltaNode(2, APPROVED),
                GetDeltaNode(1, SENT),
            });

            return this.consolidatedMovement = new ConsolidatedMovement()
            {
                DestinationNode = testApprovedNode,
            };
        }

        private ConsolidatedMovement GetMovementWithoutDeltaNodes()
        {
            var node = GetTestNode(1);

            return this.consolidatedMovement = new ConsolidatedMovement()
            {
                DestinationNode = node,
            };
        }
    }
}