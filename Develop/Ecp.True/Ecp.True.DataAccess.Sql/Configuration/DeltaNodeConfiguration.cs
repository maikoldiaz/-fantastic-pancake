// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaNodeConfiguration.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Sql
{
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Sql.Configuration;
    using Ecp.True.Entities.Admin;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The DeltaNode Configuration.
    /// </summary>
    /// <seealso cref="EntityConfiguration{DeltaNode}" />
    public class DeltaNodeConfiguration : EntityConfiguration<DeltaNode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaNodeConfiguration" /> class.
        /// </summary>
        public DeltaNodeConfiguration()
                : base(x => x.DeltaNodeId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<DeltaNode> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.Status).IsRequired().HasColumnType("int").IsRequired();
            builder.Property(s => s.LastApprovedDate).IsRequired(false).HasColumnType("datetime");
            builder.Property(s => s.Comment).IsRequired(false).HasMaxLength(1000);
            builder.Property(s => s.Editor).IsRequired(false).HasMaxLength(50);
            builder.Property(s => s.Approvers).IsRequired(false).HasMaxLength(1000);
            builder.HasOne(s => s.Ticket).WithMany(p => p.DeltaNodes).HasForeignKey(d => d.TicketId).IsRequired();
            builder.HasOne(s => s.Node).WithMany(p => p.DeltaNodes).HasForeignKey(d => d.NodeId).IsRequired();
        }
    }
}
