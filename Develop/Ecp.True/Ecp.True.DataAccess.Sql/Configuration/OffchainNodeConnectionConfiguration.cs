// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OffchainNodeConnectionConfiguration.cs" company="Microsoft">
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
    using Ecp.True.Entities.Registration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The offchain node connection Configuration.
    /// </summary>
    public class OffchainNodeConnectionConfiguration : BlockchainEntityConfiguration<OffchainNodeConnection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OffchainNodeConnectionConfiguration"/> class.
        /// </summary>
        public OffchainNodeConnectionConfiguration()
            : base(a => a.Id, Sql.Constants.OffchainSchema, true, nameof(NodeConnection))
        {
        }

        /// <inheritdoc/>
        protected override void DoConfigure(EntityTypeBuilder<OffchainNodeConnection> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.IsDeleted).IsRequired().HasColumnType("bit").HasDefaultValue(false);
            builder.Property(x => x.SourceNodeId).IsRequired();
            builder.Property(x => x.DestinationNodeId).IsRequired();

            builder.HasOne(n => n.NodeConnection)
                    .WithMany(n => n.OffchainNodeConnections)
                    .HasForeignKey(x => x.NodeConnectionId);

            base.DoConfigure(builder);
        }
    }
}
