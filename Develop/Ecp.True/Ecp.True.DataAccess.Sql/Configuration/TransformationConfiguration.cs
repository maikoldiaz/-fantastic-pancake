// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformationConfiguration.cs" company="Microsoft">
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
    using Ecp.True.Entities.TransportBalance;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The TransformationConfiguration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{Ecp.True.Entities.TransportBalance.Transformation}" />
    public class TransformationConfiguration : EntityConfiguration<Transformation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransformationConfiguration"/> class.
        /// </summary>
        public TransformationConfiguration()
        : base(x => x.TransformationId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<Transformation> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.MessageTypeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.OriginSourceNodeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.OriginSourceProductId).HasColumnType("nvarchar").IsRequired().HasMaxLength(20);
            builder.Property(x => x.OriginDestinationProductId).HasColumnType("nvarchar").HasMaxLength(20);
            builder.Property(x => x.OriginMeasurementId).HasColumnType("int").IsRequired();
            builder.Property(x => x.DestinationSourceNodeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.DestinationSourceProductId).HasColumnType("nvarchar").IsRequired().HasMaxLength(20);
            builder.Property(x => x.DestinationDestinationProductId).HasColumnType("nvarchar").HasMaxLength(20);
            builder.Property(x => x.DestinationMeasurementId).HasColumnType("int").IsRequired();

            builder.HasOne(s => s.OriginSourceNode).WithMany(p => p.OriginSourceNodeTransformations).HasForeignKey(d => d.OriginSourceNodeId);
            builder.HasOne(s => s.OriginDestinationNode).WithMany(p => p.OriginDestinationNodeTransformations).HasForeignKey(d => d.OriginDestinationNodeId);
            builder.HasOne(s => s.OriginSourceProduct).WithMany(p => p.OriginSourceProductTransformations).HasForeignKey(d => d.OriginSourceProductId);
            builder.HasOne(s => s.OriginDestinationProduct).WithMany(p => p.OriginDestinationProductTransformations).HasForeignKey(d => d.OriginDestinationProductId);
            builder.HasOne(s => s.OriginMeasurement).WithMany(p => p.OriginTransformations).HasForeignKey(d => d.OriginMeasurementId);

            builder.HasOne(s => s.DestinationSourceNode).WithMany(p => p.DestinationSourceNodeTransformations).HasForeignKey(d => d.DestinationSourceNodeId);
            builder.HasOne(s => s.DestinationDestinationNode).WithMany(p => p.DestinationDestinationNodeTransformations).HasForeignKey(d => d.DestinationDestinationNodeId);
            builder.HasOne(s => s.DestinationSourceProduct).WithMany(p => p.DestinationSourceProductTransformations).HasForeignKey(d => d.DestinationSourceProductId);
            builder.HasOne(s => s.DestinationDestinationProduct).WithMany(p => p.DestinationDestinationProductTransformations).HasForeignKey(d => d.DestinationDestinationProductId);
            builder.HasOne(s => s.DestinationMeasurement).WithMany(p => p.DestinationTransformations).HasForeignKey(d => d.DestinationMeasurementId);
        }
    }
}