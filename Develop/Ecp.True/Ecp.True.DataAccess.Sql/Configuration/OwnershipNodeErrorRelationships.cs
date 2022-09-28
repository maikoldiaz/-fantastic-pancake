// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipNodeErrorRelationships.cs" company="Microsoft">
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
    /// The ownership node error relationships.
    /// </summary>
    public static class OwnershipNodeErrorRelationships
    {
        /// <summary>
        /// Configures the relationships for ownership node error.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void Configure(EntityTypeBuilder<OwnershipNodeError> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.HasOne(s => s.OwnershipNode).WithMany(p => p.OwnershipNodeErrors).HasForeignKey(d => d.OwnershipNodeId);
            builder.HasOne(x => x.InventoryProduct)
                   .WithMany(x => x.NodeErrors)
                   .HasForeignKey(y => y.InventoryProductId);
            builder.HasOne(x => x.Movement)
                   .WithMany(x => x.NodeErrors)
                   .HasForeignKey(y => y.MovementTransactionId);
        }
    }
}