// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SegmentUnbalanceRelationships.cs" company="Microsoft">
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
    /// The segment unbalance relationships.
    /// </summary>
    public static class SegmentUnbalanceRelationships
    {
        /// <summary>
        /// Configures the relationships for segment unbalance.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void Configure(EntityTypeBuilder<SegmentUnbalance> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));
            builder.HasOne(s => s.Segment).WithMany(p => p.SegmentUnbalances).HasForeignKey(d => d.SegmentId);
            builder.HasOne(s => s.Product).WithMany(p => p.SegmentUnbalances).HasForeignKey(d => d.ProductId);
            builder.HasOne(s => s.UnbalanceTicket).WithMany(p => p.SegmentUnbalances).HasForeignKey(d => d.TicketId);
        }
    }
}
