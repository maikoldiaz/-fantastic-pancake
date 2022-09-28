// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementSourceConfiguration.cs" company="Microsoft">
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
    /// The Movement Source Configuration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{Ecp.True.Entities.Admin.MovementSource}" />
    public class MovementSourceConfiguration : EntityConfiguration<MovementSource>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovementSourceConfiguration"/> class.
        /// </summary>
        public MovementSourceConfiguration()
            : base(x => x.MovementSourceId, Sql.Constants.OffchainSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<MovementSource> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.SourceProductId).HasMaxLength(20);
            builder.Property(x => x.SourceProductTypeId).HasColumnType("int").IsRequired(false);

            builder.HasOne(x => x.SourceNode)
                    .WithMany(x => x.MovementSources)
                    .HasForeignKey(x => x.SourceNodeId);

            builder.HasOne(d => d.MovementTransaction)
                    .WithOne(p => p.MovementSource)
                    .HasForeignKey<MovementSource>(d => d.MovementTransactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(x => x.SourceStorageLocation)
                    .WithMany(x => x.Sources)
                    .HasForeignKey(x => x.SourceStorageLocationId)
                    .IsRequired(false);

            builder.HasOne(x => x.SourceProduct)
                    .WithMany(x => x.Sources)
                    .HasForeignKey(x => x.SourceProductId);

            builder.HasOne(x => x.SourceProductType)
                    .WithMany(x => x.MovementSourceProductTypes)
                    .HasForeignKey(x => x.SourceProductTypeId);
        }
    }
}
