// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeNotInStateSpecification.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Specifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Specifications;

    /// <summary>
    /// Specifies a consolidated movement with one of its nodes in a given state.
    /// </summary>
    public class NodeNotInStateSpecification : CompositeSpecification<ConsolidatedMovement>
    {
        private readonly OwnershipNodeStatusType status;
        private readonly Func<ConsolidatedMovement, Node> nodeSelector;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeNotInStateSpecification"/> class.
        /// </summary>
        /// <param name="nodeSelector">The node selector.</param>
        /// <param name="status">The ownership node status.</param>
        public NodeNotInStateSpecification(Func<ConsolidatedMovement, Node> nodeSelector, OwnershipNodeStatusType status)
        {
            this.status = status;
            this.nodeSelector = nodeSelector;
        }

        /// <inheritdoc />
        public override ICollection<string> IncludeProperties { get; } =
            new List<string>
            {
                $"{nameof(ConsolidatedMovement.DestinationNode)}",
                $"{nameof(ConsolidatedMovement.DestinationNode)}.{nameof(Node.DeltaNodes)}",
                $"{nameof(ConsolidatedMovement.SourceNode)}",
                $"{nameof(ConsolidatedMovement.SourceNode)}.{nameof(Node.DeltaNodes)}",
            };

        /// <inheritdoc/>
        public override Expression<Func<ConsolidatedMovement, bool>> ToExpression() =>
            movement => this.nodeSelector(movement) == null ||
                        this.nodeSelector(movement).DeltaNodes
                            .OrderByDescending(dn => dn.TicketId)
                            .FirstOrDefault() == null ||
                        (this.nodeSelector(movement).DeltaNodes
                            .OrderByDescending(dn => dn.TicketId)
                            .FirstOrDefault().Status != this.status);
    }
}