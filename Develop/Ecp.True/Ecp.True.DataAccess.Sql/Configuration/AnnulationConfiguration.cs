// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnnulationConfiguration.cs" company="Microsoft">
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
    /// The reversal configuration.
    /// </summary>
    public class AnnulationConfiguration : EntityConfiguration<Annulation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnnulationConfiguration"/> class.
        /// </summary>
        public AnnulationConfiguration()
        : base(x => x.AnnulationId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<Annulation> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.SourceMovementTypeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.AnnulationMovementTypeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.SourceNodeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.DestinationNodeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.SourceProductId).HasColumnType("int").IsRequired();
            builder.Property(x => x.DestinationProductId).HasColumnType("int").IsRequired();
            builder.Property(x => x.SapTransactionCodeId).HasColumnType("int");
            builder.Property(x => x.IsActive).IsRequired();

            builder.HasOne(x => x.SourceCategoryElement)
                .WithMany(x => x.AnnulationSourceMovements)
                .HasForeignKey(x => x.SourceMovementTypeId);

            builder.HasOne(x => x.AnnulationCategoryElement)
                .WithMany(x => x.AnnulationMovements)
                .HasForeignKey(x => x.AnnulationMovementTypeId);

            builder.HasOne(x => x.SourceNodeOriginType)
                .WithMany(x => x.SourceNodes)
                .HasForeignKey(x => x.SourceNodeId);

            builder.HasOne(x => x.DestinationNodeOriginType)
                .WithMany(x => x.DestinationNodes)
                .HasForeignKey(x => x.DestinationNodeId);

            builder.HasOne(x => x.SourceProductOriginType)
                .WithMany(x => x.SourceProducts)
                .HasForeignKey(x => x.SourceProductId);

            builder.HasOne(x => x.DestinationProductOriginType)
                .WithMany(x => x.DestinationProducts)
                .HasForeignKey(x => x.DestinationProductId);

            builder.HasOne(x => x.SapTransactionCode)
                .WithMany(x => x.AnnulationSapTransaction)
                .HasForeignKey(x => x.SapTransactionCodeId);
        }
    }
}
