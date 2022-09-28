// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageLocationProductOwnerConfiguration.cs" company="Microsoft">
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
    /// The storage location product owner configuration.
    /// </summary>
    public class StorageLocationProductOwnerConfiguration : EntityConfiguration<StorageLocationProductOwner>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageLocationProductOwnerConfiguration"/> class.
        /// </summary>
        public StorageLocationProductOwnerConfiguration()
        : base(x => x.StorageLocationProductOwnerId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<StorageLocationProductOwner> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.OwnershipPercentage).HasColumnType("decimal(5, 2)");

            builder.HasOne(x => x.StorageLocationProduct)
                    .WithMany(x => x.Owners)
                    .HasForeignKey(x => x.StorageLocationProductId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Owner)
                    .WithMany(x => x.StorageLocationProductOwners)
                    .HasForeignKey(x => x.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
