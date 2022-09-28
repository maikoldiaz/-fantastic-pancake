// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementConfiguration.cs" company="Microsoft">
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
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The Movement Configuration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{Ecp.True.Entities.Admin.Movement}" />
    public class MovementConfiguration : BlockchainEntityConfiguration<Movement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovementConfiguration"/> class.
        /// </summary>
        public MovementConfiguration()
            : base(x => x.MovementTransactionId, Sql.Constants.OffchainSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<Movement> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            ConfigureColumnMappings(builder);
            ConfigureMore(builder);
            ConfigureMoreMappings(builder);
            MovementRelationships.Configure(builder);
        }

        /// <summary>
        /// Configure Column Mappings.
        /// </summary>
        /// <param name="builder">builder.</param>
        private static void ConfigureColumnMappings(EntityTypeBuilder<Movement> builder)
        {
            builder.Property(x => x.SystemTypeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.SystemId).HasColumnType("int").IsRequired(false);
            builder.Property(x => x.EventType).HasMaxLength(25).IsRequired();
            builder.Property(x => x.MessageTypeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.VariableTypeId).HasColumnType("int");
            builder.Property(x => x.MovementId).HasMaxLength(50).IsRequired().IsUnicode(false);
            builder.Property(x => x.MovementTypeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.OperationalDate).HasColumnType("date").IsRequired();
            builder.Property(x => x.GrossStandardVolume).HasColumnType("decimal(18,2)");
            builder.Property(x => x.NetStandardVolume).HasColumnType("decimal(18,2)");
            builder.Property(x => x.MeasurementUnit).HasColumnType("int").IsRequired(false);
            builder.Property(x => x.ScenarioId).HasColumnType("int").IsRequired();
            builder.Property(x => x.Observations).HasMaxLength(150);
            builder.Property(x => x.Classification).IsRequired().HasMaxLength(30);
            builder.Property(x => x.MovementContractId).IsRequired(false).HasColumnType("int");
            builder.Property(x => x.IsSystemGenerated).HasColumnType("bit").IsRequired(false);
            builder.Property(x => x.IsReconciled).HasColumnType("bit").IsRequired(false);
        }

        /// <summary>
        /// Configures the more.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private static void ConfigureMore(EntityTypeBuilder<Movement> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.Tolerance).HasColumnType("decimal(18,2)").IsRequired(false);
            builder.Property(x => x.GlobalMovementId).IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.BackupMovementId).HasColumnType("varchar").HasMaxLength(50).IsRequired(false);
            builder.Property(x => x.SapProcessStatus).IsRequired(false).HasMaxLength(50).HasColumnName("SAPProcessStatus");
            builder.Property(x => x.BalanceStatus).IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.OperatorId).HasColumnType("int").IsRequired(false);
            builder.Property(x => x.MovementEventId).IsRequired(false).HasColumnType("int");
            builder.Property(x => x.SourceMovementId).IsRequired(false).HasColumnType("int");
            builder.HasOne(x => x.MovementContract).WithMany(x => x.Movements).HasForeignKey(x => x.MovementContractId);
        }

        /// <summary>
        /// Configures the more mappings.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private static void ConfigureMoreMappings(EntityTypeBuilder<Movement> builder)
        {
            builder.Property(e => e.UncertaintyPercentage).HasColumnType("decimal(5, 2)");
            builder.Property(x => x.BlockchainMovementTransactionId).HasColumnType("uniqueidentifier").IsRequired(false);
            builder.Property(x => x.PreviousBlockchainMovementTransactionId).HasColumnType("uniqueidentifier").IsRequired(false);
            builder.Property(e => e.IsOfficial).IsRequired();
            builder.Property(x => x.Version).IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.BatchId).HasMaxLength(25);
            builder.Property(x => x.IsTransferPoint).IsRequired().HasColumnType("bit");
            builder.Property(x => x.SourceMovementTransactionId).IsRequired(false).HasColumnType("int");
            builder.Property(x => x.SourceInventoryProductId).IsRequired(false).HasColumnType("int");
            builder.Property(x => x.DeltaTicketId).IsRequired(false).HasColumnType("int");
            builder.Property(x => x.OfficialDeltaTicketId).IsRequired(false).HasColumnType("int");
            builder.Property(x => x.PendingApproval).IsRequired(false).HasColumnType("bit");
            builder.Property(x => x.OfficialDeltaMessageTypeId).IsRequired(false).HasColumnType("int");
            builder.Property(x => x.OriginalMovementTransactionId).IsRequired(false).HasColumnType("int");
            builder.Property(x => x.OwnershipTicketConciliationId).IsRequired(false).HasColumnType("int");
            builder.Property(x => x.IsReconciled).HasColumnType("bit").IsRequired(false);
            builder.Ignore(x => x.IsValidClassification);

            builder.HasOne(s => s.SourceInventoryProduct)
                .WithMany(p => p.DeltaMovements)
                .HasForeignKey(d => d.SourceInventoryProductId);

            builder.HasOne(s => s.SourceMovement)
                .WithMany(p => p.DeltaMovements)
                .HasForeignKey(d => d.SourceMovementTransactionId);

            builder.HasOne(s => s.OriginalMovement)
               .WithMany(p => p.OriginalMovements)
               .HasForeignKey(d => d.OriginalMovementTransactionId);
        }
    }
}