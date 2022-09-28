// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementBuilder.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Builders
{
    using System;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// Builds a manual movement with default values.
    /// </summary>
    internal class MovementBuilder : LazyBuilder<Movement, MovementBuilder>
    {
        /// <summary>
        /// Gets the deltaNode id.
        /// </summary>
        /// <value>1.</value>
        public static int DeltaNodeId => 1;

        /// <summary>
        ///     Gets 1 the SourceNodeId.
        /// </summary>
        /// <value>
        ///     1..
        /// </value>
        public static int SourceNodeId => 1;

        /// <summary>
        ///     Gets the SegmentId.
        /// </summary>
        /// <value>
        ///     1.
        /// </value>
        public static int SegmentId => 1;

        /// <summary>
        ///     Gets the DestinationNodeId.
        /// </summary>
        /// <value>
        ///     2.
        /// </value>
        public static int DestinationNodeId => 2;

        /// <summary>
        ///     Gets the EndTime.
        /// </summary>
        /// <value>
        ///     aaaa, MM, dd 2021, 5, 4.
        /// </value>
        public static DateTime EndTime => new DateTime(2021, 5, 4);

        /// <summary>
        ///     Gets the StartTime.
        /// </summary>
        /// <value>
        ///     aaaa, MM, dd 2021, 5, 3.
        /// </value>
        public static DateTime StartTime => new DateTime(2021, 5, 3);

        public static implicit operator Movement(MovementBuilder builder)
        {
            return builder.Build();
        }

        /// <summary>
        /// Test movement with default values for manual movements.
        /// </summary>
        /// <returns>The test builder.</returns>
        public MovementBuilder WithManualSourceSystem()
        {
            return this;
        }

        /// <summary>
        /// Test movement with default values for manual movements.
        /// </summary>
        /// <returns>The test builder.</returns>
        public MovementBuilder IsDeleted()
        {
            this.Do(m => m.IsDeleted = true);
            return this;
        }

        /// <summary>
        /// Official manual movement.
        /// </summary>
        /// <returns>The test builder.</returns>
        public MovementBuilder IsOfficial()
        {
            this.Do(m => m.ScenarioId = ScenarioType.OFFICER);
            return this;
        }

        /// <summary>
        /// Operative manual movement.
        /// </summary>
        /// <returns>The test builder.</returns>
        public MovementBuilder IsOperative()
        {
            this.Do(m => m.IsOfficial = false);
            this.Do(m => m.ScenarioId = ScenarioType.OPERATIONAL);
            return this;
        }

        /// <summary>
        /// Manual movement with start time.
        /// </summary>
        /// <returns>The test builder.</returns>
        /// <param name="startTime">The time the official movement starts.</param>
        public MovementBuilder WithStartTime(DateTime startTime)
        {
            this.Do(m => m.Period.StartTime = startTime.Date);
            return this;
        }

        /// <summary>
        /// Manual movement with start time.
        /// </summary>
        /// <returns>The test builder.</returns>
        /// <param name="endTime">The time the official movement starts.</param>
        public MovementBuilder WithEndTime(DateTime endTime)
        {
            this.Do(m => m.Period.StartTime = endTime.Date);
            return this;
        }

        /// <summary>
        /// Manual movement with start time.
        /// </summary>
        /// <returns>The test builder.</returns>
        /// <param name="sourceNodeId">The sourcenode id for the official movement starts.</param>
        public MovementBuilder WithSourceNodeId(int sourceNodeId)
        {
            this.Do(m => m.MovementSource.SourceNodeId = sourceNodeId);
            return this;
        }

        /// <summary>
        /// Manual movement with start time.
        /// </summary>
        /// <returns>The test builder.</returns>
        public MovementBuilder WithNotManualSourceSystem()
        {
            this.Do(m => m.SourceSystemId = 0);
            return this;
        }

        /// <summary>
        /// Inventory movement with start time.
        /// </summary>
        /// <returns>The test builder.</returns>
        public MovementBuilder WithDefaultValuesForInventory()
        {
            this.Do(m => m.SourceSystemId = Constants.ManualInvOfficial);
            this.Do(m => m.Period.StartTime = StartTime.AddDays(-1));
            return this;
        }

        /// <summary>
        /// Inventory movement with start time.
        /// </summary>
        /// <returns>The test builder.</returns>
        public MovementBuilder WithInventorySourceSystem()
        {
            this.Do(m => m.SourceSystemId = Constants.ManualInvOfficial);
            return this;
        }

        /// <summary>
        /// Movement with ticket id.
        /// </summary>
        /// <param name="ticketId">The ticket Id.</param>
        /// <returns>The test builder.</returns>
        public MovementBuilder WithTicketId(int ticketId)
        {
            this.Do(m => m.OfficialDeltaTicketId = ticketId);
            return this;
        }

        /// <summary>
        /// With null ticket.
        /// </summary>
        /// <returns>The test builder.</returns>
        public MovementBuilder WithNullTicket()
        {
            this.Do(m => m.OfficialDeltaTicketId = null);
            return this;
        }

        /// <summary>
        /// Sets the transactionId.
        /// </summary>
        /// <param name="movementTransactionId">The movement transaction id.</param>
        /// <returns>the movement.</returns>
        public MovementBuilder WithTransactionId(int movementTransactionId)
        {
            this.Do(m => m.MovementTransactionId = movementTransactionId);
            return this;
        }

        /// <summary>
        /// Sets the status of the deltaNode with nodeId cref="nodeId" associated with the movement.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="status">The deltaNode status.</param>
        /// <returns>The builder.</returns>
        public MovementBuilder WithDeltaNodeState(int nodeId, OwnershipNodeStatusType status)
        {
            this.Do(m =>
                m.Ticket.DeltaNodes.Add(new DeltaNode { NodeId = nodeId, Status = status }));
            return this;
        }

        /// <summary>
        /// Constructs the default manual movement.
        /// </summary>
        /// <returns>The default manual movement.</returns>
        protected override Movement Construct()
        {
            return new Movement
            {
                IsDeleted = false,
                ScenarioId = ScenarioType.OFFICER,
                IsOfficial = true,
                SegmentId = SegmentId,
                OfficialDeltaTicketId = null,
                SourceSystemId = Constants.ManualMovOfficial,
                MovementSource = new MovementSource
                {
                    SourceNodeId = SourceNodeId,
                },
                MovementDestination = new MovementDestination
                {
                    DestinationNodeId = DestinationNodeId,
                },
                Period = new MovementPeriod
                {
                    StartTime = StartTime,
                    EndTime = EndTime,
                },
            };
        }
    }
}