// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeCostCenterConfiguration.cs" company="Microsoft">
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
    /// The Node CostCenter configuration.
    /// </summary>
    public class NodeCostCenterConfiguration : EntityConfiguration<NodeCostCenter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeCostCenterConfiguration"/> class.
        /// </summary>
        public NodeCostCenterConfiguration()
        : base(x => x.NodeCostCenterId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<NodeCostCenter> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.NodeCostCenterId).HasColumnType("int").IsRequired();
            builder.Property(x => x.SourceNodeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.DestinationNodeId).HasColumnType("int");
            builder.Property(x => x.MovementTypeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.CostCenterId).HasColumnType("int").IsRequired();
            builder.Property(x => x.IsActive).IsRequired();

            builder.HasOne(x => x.SourceNode)
                .WithMany(x => x.SourceNodeCostCenter)
                .HasForeignKey(x => x.SourceNodeId);

            builder.HasOne(x => x.DestinationNode)
                .WithMany(x => x.DestinationNodeCostCenter)
                .HasForeignKey(x => x.DestinationNodeId);

            builder.HasOne(x => x.MovementTypeCategoryElement)
                .WithMany(x => x.MovementTypeNodeCostCenter)
                .HasForeignKey(x => x.MovementTypeId);

            builder.HasOne(x => x.CostCenterCategoryElement)
                .WithMany(x => x.CostCenterNodeCostCenter)
                .HasForeignKey(x => x.CostCenterId);
        }
    }
}
