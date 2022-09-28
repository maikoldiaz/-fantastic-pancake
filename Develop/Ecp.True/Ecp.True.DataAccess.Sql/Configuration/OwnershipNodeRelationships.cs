// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipNodeRelationships.cs" company="Microsoft">
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
    /// The ownership node relationships.
    /// </summary>
    public static class OwnershipNodeRelationships
    {
        /// <summary>
        /// Configures the relationships for ownership nodes.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void Configure(EntityTypeBuilder<OwnershipNode> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));
            builder.HasOne(s => s.Ticket).WithMany(p => p.OwnershipNodes).HasForeignKey(d => d.TicketId);
            builder.HasOne(s => s.Node).WithMany(p => p.OwnershipNodes).HasForeignKey(d => d.NodeId);
        }
    }
}