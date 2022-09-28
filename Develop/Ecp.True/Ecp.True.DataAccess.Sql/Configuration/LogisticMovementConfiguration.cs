// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogisticMovementConfiguration.cs" company="Microsoft">
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
    using Ecp.True.Entities.TransportBalance;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The Movement Logistic Configuration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{Ecp.True.Entities.Registration.LogisticMovement}" />
    public class LogisticMovementConfiguration : EntityConfiguration<LogisticMovement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogisticMovementConfiguration"/> class.
        /// </summary>
        public LogisticMovementConfiguration()
            : base(x => x.LogisticMovementId, Sql.Constants.OffchainSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<LogisticMovement> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.MovementTransactionId).IsRequired();
            builder.Property(x => x.EventType);
            builder.Property(x => x.DestinationSystem);
            builder.Property(x => x.MovementOrder);
            builder.Property(x => x.NumReg);
            builder.Property(x => x.NodeOrder);
            builder.Property(x => x.StartTime);
            builder.Property(x => x.SourceLogisticCenterId);
            builder.Property(x => x.DestinationLogisticCenterId);
            builder.Property(x => x.OwnershipVolume);
            builder.Property(x => x.MeasurementUnit);
            builder.Property(x => x.LogisticMovementTypeId);
            builder.Property(x => x.HomologatedMovementType);
            builder.Property(x => x.DocumentNumber);
            builder.Property(x => x.Position);
            builder.Property(x => x.CostCenterId);
            builder.Property(x => x.SapTransactionCode);
            builder.Property(x => x.StatusProcessId);
            builder.Property(x => x.MessageProcess);
            builder.Property(x => x.SapTransactionId);
            builder.Property(x => x.IsCheck);
            builder.Property(x => x.SapSentDate);
            builder.Property(x => x.SourceProductId);
            builder.Property(x => x.DestinationProductId);
            builder.Property(x => x.SourceLogisticNodeId);
            builder.Property(x => x.DestinationLogisticNodeId);
            builder.Property(x => x.ConcatMovementId);

            builder.HasOne(d => d.MovementTransaction)
                    .WithOne(p => p.LogisticMovement)
                    .HasForeignKey<LogisticMovement>(d => d.MovementTransactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(x => x.TicketLogisticMovement)
                .WithMany(t => t.LogisticMovements)
                .HasForeignKey(x => x.TicketId);

            builder.HasOne(x => x.CategoryCostCenter)
                .WithMany(t => t.LogisticMovements)
                .HasForeignKey(x => x.CostCenterId);
        }
    }
}
