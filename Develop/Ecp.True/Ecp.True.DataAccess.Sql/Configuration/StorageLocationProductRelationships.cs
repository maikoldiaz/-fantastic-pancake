// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageLocationProductRelationships.cs" company="Microsoft">
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
    /// The segment unbalance relationships.
    /// </summary>
    public static class StorageLocationProductRelationships
    {
        /// <summary>
        /// Configures the relationships for segment unbalance.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void Configure(EntityTypeBuilder<StorageLocationProduct> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.HasOne(t => t.Product)
                .WithMany(t => t.ProductLocations)
                .HasForeignKey(t => t.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.NodeStorageLocation)
                .WithMany(t => t.Products)
                .HasForeignKey(t => t.NodeStorageLocationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.NodeProductRule)
                .WithMany(p => p.StorageLocationProducts)
                .HasForeignKey(d => d.NodeProductRuleId);

            builder.HasOne(d => d.OwnershipRule)
                .WithMany(p => p.StorageLocationProducts)
                .HasForeignKey(d => d.OwnershipRuleId);
        }
    }
}
