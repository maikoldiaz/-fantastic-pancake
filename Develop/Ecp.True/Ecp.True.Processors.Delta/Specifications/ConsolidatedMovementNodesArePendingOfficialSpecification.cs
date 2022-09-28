// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidatedMovementNodesArePendingOfficialSpecification.cs" company="Microsoft">
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
    using System.Linq.Expressions;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Specifications;

    /// <summary>
    /// Specifies whether a consolidated movement has nodes that belong to the given pending official node collection.
    /// </summary>
    public class ConsolidatedMovementNodesArePendingOfficialSpecification : CompositeSpecification<ConsolidatedMovement>
    {
        private readonly ICollection<int> pendingOfficialNodes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsolidatedMovementNodesArePendingOfficialSpecification"/> class.
        /// </summary>
        /// <param name="pendingOfficialNodes">The list of pending official nodes IDs.</param>
        public ConsolidatedMovementNodesArePendingOfficialSpecification(ICollection<int> pendingOfficialNodes)
        {
            this.pendingOfficialNodes = pendingOfficialNodes;
        }

        /// <inheritdoc />
        public override Expression<Func<ConsolidatedMovement, bool>> ToExpression()
        {
            return movement => this.pendingOfficialNodes.Contains(movement.SourceNodeId.GetValueOrDefault())
                               || this.pendingOfficialNodes.Contains(movement.DestinationNodeId.GetValueOrDefault());
        }
    }
}