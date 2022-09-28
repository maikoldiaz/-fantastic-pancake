// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementRelationships.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Sql.Configuration
{
    using Ecp.True.Core;
    using Ecp.True.Entities.Registration;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The unbalance comment relationships.
    /// </summary>
    public static class MovementRelationships
    {
        /// <summary>
        /// Configures the relationships for movement.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void Configure(EntityTypeBuilder<Movement> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.Comment).HasMaxLength(200).IsRequired(false);

            builder.HasOne(t => t.Ticket)
                    .WithMany(p => p.Movements)
                    .HasForeignKey(d => d.TicketId)
                    .IsRequired(false);

            builder.HasOne(x => x.Segment)
                    .WithMany(p => p.Movements)
                    .HasForeignKey(y => y.SegmentId)
                    .IsRequired(false);

            builder.HasOne(t => t.OwnershipTicket)
                    .WithMany(p => p.MovementOwnerships)
                    .HasForeignKey(d => d.OwnershipTicketId)
                    .IsRequired(false);

            builder.HasOne(t => t.MovementEvent)
                    .WithMany(p => p.Movements)
                    .HasForeignKey(d => d.MovementEventId)
                    .IsRequired(false);

            builder.HasOne(d => d.FileRegistrationTransaction)
                    .WithMany(p => p.Movements)
                    .HasForeignKey(d => d.FileRegistrationTransactionId);

            builder.HasOne(s => s.Reason)
                .WithMany(p => p.ReasonMovements)
                .HasForeignKey(d => d.ReasonId);

            builder.HasOne(s => s.System)
                .WithMany(p => p.SystemMovements)
                .HasForeignKey(d => d.SystemId);

            builder.HasOne(s => s.Operator)
                .WithMany(p => p.OperatorMovements)
                .HasForeignKey(d => d.OperatorId);

            builder.HasOne(s => s.SourceSystemElement)
                .WithMany(p => p.SourceSystemMovements)
                .HasForeignKey(d => d.SourceSystemId);

            builder.HasOne(s => s.DeltaTicket)
                .WithMany(p => p.DeltaMovements)
                .HasForeignKey(d => d.DeltaTicketId);

            builder.HasOne(s => s.OfficialDeltaTicket)
                .WithMany(p => p.OfficialDeltaMovements)
                .HasForeignKey(d => d.OfficialDeltaTicketId);

            builder.HasOne(s => s.ConsolidatedMovement)
                .WithMany(p => p.Movements)
                .HasForeignKey(d => d.ConsolidatedMovementTransactionId)
                .IsRequired(false);

            builder.HasOne(s => s.ConsolidatedInventoryProduct)
                .WithMany(p => p.Movements)
                .HasForeignKey(d => d.ConsolidatedInventoryProductId)
                .IsRequired(false);

            builder.HasOne(s => s.MeasurementUnitElement)
               .WithMany(p => p.MovementMeasurementUnits)
               .HasForeignKey(d => d.MeasurementUnit);

            builder.HasOne(s => s.MovementType)
               .WithMany(p => p.MovementMovementTypes)
               .HasForeignKey(d => d.MovementTypeId);
        }
    }
}
