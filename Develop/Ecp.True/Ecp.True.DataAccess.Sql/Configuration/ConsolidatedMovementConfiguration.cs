// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidatedMovementConfiguration.cs" company="Microsoft">
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
    using Ecp.True.Entities.Admin;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The Consolidated Movement Configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{Ecp.True.Entities.Admin.ConsolidatedMovement}" />
    public class ConsolidatedMovementConfiguration : EntityConfiguration<ConsolidatedMovement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsolidatedMovementConfiguration"/> class.
        /// </summary>
        public ConsolidatedMovementConfiguration()
            : base(x => x.ConsolidatedMovementId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<ConsolidatedMovement> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));
            builder.Property(x => x.MovementTypeId).HasMaxLength(150).IsRequired();
            builder.Property(x => x.StartDate).HasColumnType("datetime").IsRequired();
            builder.Property(x => x.EndDate).HasColumnType("datetime").IsRequired();
            builder.Property(x => x.GrossStandardVolume).HasColumnType("decimal(18,2)");
            builder.Property(x => x.NetStandardVolume).HasColumnType("decimal(18,2)");
            builder.Property(x => x.MeasurementUnit).HasMaxLength(50);
            builder.Property(x => x.ExecutionDate).HasColumnType("datetime").IsRequired();
            builder.Property(x => x.IsActive).IsRequired();

            builder.HasOne(x => x.SourceNode).WithMany(x => x.SourceConsolidatedMovements).HasForeignKey(x => x.SourceNodeId);
            builder.HasOne(x => x.SourceProduct).WithMany(x => x.SourceConsolidatedMovements).HasForeignKey(x => x.SourceProductId);
            builder.HasOne(x => x.DestinationNode).WithMany(x => x.DestinationConsolidatedMovements).HasForeignKey(x => x.DestinationNodeId);
            builder.HasOne(x => x.DestinationProduct).WithMany(p => p.DestinationConsolidatedMovements).HasForeignKey(x => x.DestinationProductId);
            builder.HasOne(t => t.Ticket).WithMany(p => p.ConsolidatedMovements).HasForeignKey(d => d.TicketId).IsRequired();
            builder.HasOne(x => x.Segment).WithMany(p => p.ConsolidatedMovementSegments).HasForeignKey(y => y.SegmentId).IsRequired();
            builder.HasOne(s => s.SourceSystem).WithMany(p => p.ConsolidatedMovementSourceSystems).HasForeignKey(d => d.SourceSystemId);
        }
    }
}
