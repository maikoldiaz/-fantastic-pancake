// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeStorageLocationConfiguration.cs" company="Microsoft">
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
    /// The NodeStorage Location Configuration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{Ecp.True.Entities.Admin.NodeStorageLocation}" />
    public class NodeStorageLocationConfiguration : EntityConfiguration<NodeStorageLocation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeStorageLocationConfiguration"/> class.
        /// </summary>
        public NodeStorageLocationConfiguration()
            : base(x => x.NodeStorageLocationId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<NodeStorageLocation> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Description).HasMaxLength(1000);
            builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(x => x.SendToSap).HasColumnName(@"SendToSAP").IsRequired().HasDefaultValue(false);
            builder.Property(e => e.StorageLocationId).HasMaxLength(20);

            ConfigureColumnMappings(builder);
        }

        /// <summary>
        /// Configure Column Mappings.
        /// </summary>
        /// <param name="builder">builder.</param>
        private static void ConfigureColumnMappings(EntityTypeBuilder<NodeStorageLocation> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.HasOne(x => x.StorageLocationType)
                    .WithMany(c => c.NodeStorageLocations)
                    .HasForeignKey(a => a.StorageLocationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(s => s.StorageLocation)
                    .WithMany(x => x.NodeStorageLocations)
                    .HasForeignKey(a => a.StorageLocationId);

            builder.HasOne(x => x.Node)
                    .WithMany(x => x.NodeStorageLocations)
                    .HasForeignKey(x => x.NodeId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
