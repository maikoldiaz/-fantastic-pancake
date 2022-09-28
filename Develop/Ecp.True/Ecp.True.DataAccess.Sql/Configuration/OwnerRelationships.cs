// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnerRelationships.cs" company="Microsoft">
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
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The owner relationships.
    /// </summary>
    public static class OwnerRelationships
    {
        /// <summary>
        /// Configures the relationships for owner.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void Configure(EntityTypeBuilder<Owner> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.HasOne(x => x.MovementTransaction)
                    .WithMany(x => x.Owners)
                    .HasForeignKey(x => x.MovementTransactionId);

            builder.HasOne(d => d.InventoryProduct)
                    .WithMany(p => p.Owners)
                    .HasForeignKey(d => d.InventoryProductId);

            builder.HasOne(d => d.OwnerElement)
                    .WithMany(p => p.Owners)
                    .HasForeignKey(d => d.OwnerId);
        }
    }
}