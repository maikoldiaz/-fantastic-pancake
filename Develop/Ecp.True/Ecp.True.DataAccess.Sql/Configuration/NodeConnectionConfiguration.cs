// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeConnectionConfiguration.cs" company="Microsoft">
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
    using System.Diagnostics.CodeAnalysis;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The node connection configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{Ecp.True.Entities.Admin.NodeConnection}" />
    [ExcludeFromCodeCoverage]
    public class NodeConnectionConfiguration : EntityConfiguration<NodeConnection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeConnectionConfiguration"/> class.
        /// </summary>
        public NodeConnectionConfiguration()
            : base(x => x.NodeConnectionId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<NodeConnection> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.Description).HasMaxLength(300);
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(e => e.ControlLimit).HasColumnType("decimal(18,2)");
            builder.Property(e => e.IsTransfer).IsRequired().HasDefaultValue(false);

            builder.HasOne(x => x.Algorithm)
                    .WithMany(x => x.NodeConnections)
                    .HasForeignKey(x => x.AlgorithmId)
                    .IsRequired(false);

            builder.HasOne(x => x.SourceNode)
                    .WithMany(x => x.SourceConnections)
                    .HasForeignKey(x => x.SourceNodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(x => x.DestinationNode)
                    .WithMany(x => x.DestinationConnections)
                    .HasForeignKey(x => x.DestinationNodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
