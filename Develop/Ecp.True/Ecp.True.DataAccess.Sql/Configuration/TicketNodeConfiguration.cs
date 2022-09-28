// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TicketNodeConfiguration.cs" company="Microsoft">
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
    /// The Ticket Nodes configuration.
    /// </summary>
    public class TicketNodeConfiguration : EntityConfiguration<TicketNode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TicketNodeConfiguration"/> class.
        /// </summary>
        public TicketNodeConfiguration()
        : base(x => x.TicketNodeId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<TicketNode> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.TicketNodeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.TicketId).HasColumnType("int").IsRequired();
            builder.Property(x => x.NodeId).HasColumnType("int").IsRequired();

            builder.HasOne(x => x.Ticket)
                .WithMany(x => x.TicketNodes)
                .HasForeignKey(x => x.TicketId);

            builder.HasOne(x => x.Node)
                .WithMany(x => x.TicketNodes)
                .HasForeignKey(x => x.NodeId);
        }
    }
}
