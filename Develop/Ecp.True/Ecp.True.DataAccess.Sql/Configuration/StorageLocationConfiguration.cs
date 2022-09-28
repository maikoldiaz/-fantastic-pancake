// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageLocationConfiguration.cs" company="Microsoft">
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
    /// The Storage Location Configuration.
    /// </summary>
    /// <seealso cref="IEntityTypeConfiguration{TEntity}" />
    public class StorageLocationConfiguration : EntityConfiguration<StorageLocation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageLocationConfiguration"/> class.
        /// </summary>
        public StorageLocationConfiguration()
            : base(x => x.StorageLocationId, Sql.Constants.AdminSchema, false)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<StorageLocation> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.StorageLocationId).HasMaxLength(20);
            builder.Property(x => x.Name).HasMaxLength(150).IsRequired();
            builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);

            builder.HasOne(x => x.LogisticCenter)
                    .WithMany(x => x.StorageLocations)
                    .HasForeignKey(d => d.LogisticCenterId);
        }
    }
}
