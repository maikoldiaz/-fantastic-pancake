// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipResultRelationships.cs" company="Microsoft">
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
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The OwnershipResultRelationships.
    /// </summary>
    public static class OwnershipResultRelationships
    {
        /// <summary>
        /// Configures the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void Configure(EntityTypeBuilder<OwnershipResult> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.HasOne(s => s.Node).WithMany(p => p.OwnershipResults).HasForeignKey(d => d.NodeId);
            builder.HasOne(s => s.Product).WithMany(p => p.OwnershipResults).HasForeignKey(d => d.ProductId);
            builder.HasOne(s => s.InventoryProduct).WithMany(p => p.Results).HasForeignKey(d => d.InventoryProductId);
            builder.HasOne(s => s.MovementTransaction).WithMany(p => p.Results).HasForeignKey(d => d.MovementTransactionId);
        }
    }
}
