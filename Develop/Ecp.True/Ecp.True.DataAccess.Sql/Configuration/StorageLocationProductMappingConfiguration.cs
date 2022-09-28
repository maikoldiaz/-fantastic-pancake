// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageLocationProductMappingConfiguration.cs" company="Microsoft">
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
    /// The StorageLocationProductMappingConfiguration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{Ecp.True.Entities.Admin.StorageLocationProductMapping}" />
    public class StorageLocationProductMappingConfiguration : EntityConfiguration<StorageLocationProductMapping>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageLocationProductMappingConfiguration" /> class.
        /// </summary>
        public StorageLocationProductMappingConfiguration()
                    : base(x => x.StorageLocationProductMappingId, Sql.Constants.AdminSchema, false)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<StorageLocationProductMapping> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.ToTable("StorageLocationProductMapping", "Admin");

            builder.Property(e => e.ProductId).IsRequired().HasMaxLength(20);
            builder.Property(e => e.StorageLocationId).IsRequired().HasMaxLength(20);

            builder.HasOne(d => d.Product)
                    .WithMany(p => p.StorageLocationProductMappings)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.StorageLocation)
                    .WithMany(p => p.StorageLocationProductMappings)
                    .HasForeignKey(d => d.StorageLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
