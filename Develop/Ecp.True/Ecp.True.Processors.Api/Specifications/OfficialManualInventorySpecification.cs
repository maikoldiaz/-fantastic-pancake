// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialManualInventorySpecification.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Specifications
{
    using System;
    using System.Linq.Expressions;
    using Ecp.True.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.Specifications;

    /// <summary>
    ///     Gets whether an inventory is manual.
    /// </summary>
    public class OfficialManualInventorySpecification : CompositeSpecification<Movement>
    {
        /// <summary>
        /// The node id.
        /// </summary>
        private readonly int nodeId;

        /// <summary>
        /// The start date.
        /// </summary>
        private readonly DateTime startTime;

        /// <summary>
        /// The end time.
        /// </summary>
        private readonly DateTime endTime;

        /// <summary>
        ///     Initializes a new instance of the <see cref="OfficialManualInventorySpecification" /> class.
        /// </summary>
        /// <param name="nodeId">The nodeId.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        public OfficialManualInventorySpecification(int nodeId, DateTime startTime, DateTime endTime)
        {
            this.nodeId = nodeId;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        /// <inheritdoc />
        public override Expression<Func<Movement, bool>> ToExpression()
        {
            Expression<Func<Movement, bool>> isManualInventory = m =>
                (m.Period.StartTime == this.startTime.AddDays(-1) || m.Period.EndTime == this.endTime) &&
                (m.MovementSource.SourceNodeId == this.nodeId ||
                 m.MovementDestination.DestinationNodeId == this.nodeId) &&
                m.SourceSystemId == Constants.ManualInvOfficial;

            return isManualInventory
                .AndNot(new MovementWithOfficialTicketSpecification())
                .AndAlso(new OfficialMovementSpecification())
                .AndAlso(new NotDeletedMovementSpecification());
        }
    }
}