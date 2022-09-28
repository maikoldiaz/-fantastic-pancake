// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementConciliationConfiguration.cs" company="Microsoft">
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
    /// The Event Object Configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{MovementConciliation}" />
    public class MovementConciliationConfiguration : EntityConfiguration<MovementConciliation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovementConciliationConfiguration"/> class.
        /// </summary>
        public MovementConciliationConfiguration()
            : base(x => x.MovementConciliationId, Sql.Constants.OffchainSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<MovementConciliation> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));
            builder.Property(x => x.MovementTransactionId).HasColumnType("int").IsRequired(false);
            builder.Property(x => x.MovementTypeId).HasColumnType("int").IsRequired(false);
            builder.Property(x => x.SourceNodeId).HasColumnType("int").IsRequired(false);
            builder.Property(x => x.DestinationNodeId).HasColumnType("int").IsRequired(false);
            builder.Property(x => x.SourceProductId).HasColumnType("nvarchar").HasMaxLength(50).IsRequired(false);
            builder.Property(x => x.DestinationProductId).HasColumnType("nvarchar").HasMaxLength(50).IsRequired(false);
            builder.Property(x => x.OwnershipVolume).HasColumnType("decimal(18,2)").IsRequired(false);
            builder.Property(x => x.OwnerId).HasColumnType("int").IsRequired(false);
            builder.Property(x => x.MeasurementUnit).HasColumnType("int").IsRequired(false);
            builder.Property(x => x.SegmentId).HasColumnType("int").IsRequired(false);
            builder.Property(x => x.OwnershipPercentage).HasColumnType("decimal(18,2)").IsRequired(false);
            builder.Property(x => x.NetStandardVolume).HasColumnType("decimal(18,2)").IsRequired(false);
            builder.Property(x => x.Description).HasColumnType("nvarchar").HasMaxLength(260).IsRequired(false);
            builder.Property(x => x.Sign).HasColumnType("nvarchar").HasMaxLength(50).IsRequired(false).HasMaxLength(40);
            builder.Property(x => x.DeltaConciliated).HasColumnType("decimal(18,2)").IsRequired(false);
            builder.Property(x => x.OperationalDate).HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.UncertaintyPercentage).HasColumnType("decimal(18,2)").IsRequired(false);
            builder.Property(x => x.CollectionType).HasColumnType("int").IsRequired();
            builder.Property(x => x.OwnershipTicketConciliationId).HasColumnType("int").IsRequired();
        }
    }
}
