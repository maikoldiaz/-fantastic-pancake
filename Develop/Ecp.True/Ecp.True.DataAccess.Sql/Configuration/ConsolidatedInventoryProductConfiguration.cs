// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidatedInventoryProductConfiguration.cs" company="Microsoft">
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
    /// The Consolidated Movement Configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{Ecp.True.Entities.Admin.ConsolidatedInventoryProduct}" />
    public class ConsolidatedInventoryProductConfiguration : EntityConfiguration<ConsolidatedInventoryProduct>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsolidatedInventoryProductConfiguration"/> class.
        /// </summary>
        public ConsolidatedInventoryProductConfiguration()
            : base(x => x.ConsolidatedInventoryProductId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<ConsolidatedInventoryProduct> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));
            builder.Property(x => x.InventoryDate).HasColumnType("datetime").IsRequired();
            builder.Property(x => x.GrossStandardQuantity).HasColumnType("decimal(18,2)");
            builder.Property(x => x.ProductVolume).HasColumnType("decimal(18,2)");
            builder.Property(x => x.MeasurementUnit).HasMaxLength(50);
            builder.Property(x => x.ExecutionDate).HasColumnType("datetime").IsRequired();
            builder.Property(x => x.IsActive).IsRequired();

            builder.HasOne(x => x.Node).WithMany(x => x.ConsolidatedInventoryProducts).HasForeignKey(x => x.NodeId).IsRequired();
            builder.HasOne(x => x.Product).WithMany(x => x.ConsolidatedInventoryProducts).HasForeignKey(x => x.ProductId).IsRequired();
            builder.HasOne(t => t.Ticket).WithMany(p => p.ConsolidatedInventoryProducts).HasForeignKey(d => d.TicketId).IsRequired();
            builder.HasOne(x => x.Segment).WithMany(p => p.ConsolidatedInventoryProductSegments).HasForeignKey(y => y.SegmentId).IsRequired();
            builder.HasOne(s => s.SourceSystem).WithMany(p => p.ConsolidatedInventoryProductSourceSystems).HasForeignKey(d => d.SourceSystemId).IsRequired();
        }
    }
}
