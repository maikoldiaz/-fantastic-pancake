// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementDestinationConfiguration.cs" company="Microsoft">
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
    /// The Movement Destination Configuration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{Ecp.True.Entities.Admin.MovementDestination}" />
    public class MovementDestinationConfiguration : EntityConfiguration<MovementDestination>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovementDestinationConfiguration"/> class.
        /// </summary>
        public MovementDestinationConfiguration()
            : base(x => x.MovementDestinationId, Sql.Constants.OffchainSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<MovementDestination> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.DestinationProductId).HasMaxLength(20);
            builder.Property(x => x.DestinationProductTypeId).HasColumnType("int").IsRequired(false);

            builder.HasOne(x => x.MovementTransaction)
                    .WithOne(x => x.MovementDestination)
                    .HasForeignKey<MovementDestination>(d => d.MovementTransactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(x => x.DestinationNode)
                    .WithMany(x => x.MovementDestinations)
                    .HasForeignKey(x => x.DestinationNodeId);

            builder.HasOne(x => x.DestinationStorageLocation)
                    .WithMany(x => x.Destinations)
                    .HasForeignKey(x => x.DestinationStorageLocationId)
                    .IsRequired(false);

            builder.HasOne(x => x.DestinationProduct)
                    .WithMany(p => p.Destinations)
                    .HasForeignKey(x => x.DestinationProductId);

            builder.HasOne(x => x.DestinationProductType)
                    .WithMany(x => x.MovementDestinationProductTypes)
                    .HasForeignKey(x => x.DestinationProductTypeId);
        }
    }
}
