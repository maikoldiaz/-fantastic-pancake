// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipCalculationRelationships.cs" company="Microsoft">
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
    /// The ownership calculation relationships.
    /// </summary>
    public static class OwnershipCalculationRelationships
    {
        /// <summary>
        /// Configures the relationships for ownership calculation.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void Configure(EntityTypeBuilder<OwnershipCalculation> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));
            builder.HasOne(s => s.Node).WithMany(p => p.OwnershipCalculations).HasForeignKey(d => d.NodeId);
            builder.HasOne(s => s.Product).WithMany(p => p.OwnershipCalculations).HasForeignKey(d => d.ProductId);
            builder.HasOne(s => s.OwnershipTicket).WithMany(p => p.OwnershipCalculations).HasForeignKey(d => d.OwnershipTicketId);
            builder.HasOne(s => s.Owner).WithMany(p => p.OwnershipCalculations).HasForeignKey(d => d.OwnerId);
        }
    }
}
