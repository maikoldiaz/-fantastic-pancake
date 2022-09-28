// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaNodeToAddManualMovementsSpec.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Admin
{
    using System;
    using System.Linq.Expressions;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Specifications;

    /// <summary>
    /// The official delta existing ticket spec.
    /// </summary>
    public class DeltaNodeToAddManualMovementsSpec : CompositeSpecification<DeltaNode>
    {
        /// <summary>
        /// The delta node id.
        /// </summary>
        private readonly int deltaNodeId;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaNodeToAddManualMovementsSpec"/> class.
        /// </summary>
        /// <param name="deltaNodeId">The delta node id.</param>
        public DeltaNodeToAddManualMovementsSpec(int deltaNodeId)
        {
            this.deltaNodeId = deltaNodeId;
        }

        /// <inheritdoc />
        public override Expression<Func<DeltaNode, bool>> ToExpression() =>
            d => d.DeltaNodeId == this.deltaNodeId
            && d.Status != OwnershipNodeStatusType.APPROVED
            && d.Status != OwnershipNodeStatusType.SUBMITFORAPPROVAL;
    }
}