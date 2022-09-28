// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeConfiguration.cs" company="Microsoft">
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
    /// The Node Configuration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{Ecp.True.Entities.Admin.Node}" />
    public class NodeConfiguration : EntityConfiguration<Node>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeConfiguration"/> class.
        /// </summary>
        public NodeConfiguration()
            : base(x => x.NodeId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<Node> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Ignore(x => x.NodeTypeId);
            builder.Ignore(x => x.OperatorId);
            builder.Ignore(x => x.SegmentId);
            builder.Ignore(x => x.AutoOrder);

            builder.Ignore(x => x.NodeType);
            builder.Ignore(x => x.Operator);
            builder.Ignore(x => x.Segment);

            builder.HasOne(s => s.LogisticCenter)
                .WithMany(p => p.Nodes)
                .HasForeignKey(d => d.LogisticCenterId);

            builder.HasOne(s => s.NodeOwnershipRule)
                .WithMany(p => p.Nodes)
                .HasForeignKey(d => d.NodeOwnershipRuleId);

            builder.HasOne(s => s.Unit)
                .WithMany(p => p.NodeUnits)
                .HasForeignKey(d => d.UnitId);

            ConfigureColumnMappings(builder);
        }

        /// <summary>
        /// Configures the column mappings.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private static void ConfigureColumnMappings(EntityTypeBuilder<Node> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Description).HasMaxLength(1000);
            builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(x => x.SendToSap).HasColumnName("SendToSAP").HasDefaultValue(false);
            builder.Property(e => e.LogisticCenterId).HasMaxLength(20);
            builder.Property(e => e.AcceptableBalancePercentage).HasColumnType("decimal(5,2)");
            builder.Property(e => e.ControlLimit).HasColumnType("decimal(18,2)");
            builder.Property(e => e.Capacity).HasColumnType("decimal(18,2)").IsRequired(false);
            builder.Property(x => x.IsExportation).HasDefaultValue(false);
        }
    }
}
