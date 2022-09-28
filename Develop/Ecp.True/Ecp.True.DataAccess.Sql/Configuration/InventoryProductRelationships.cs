// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryProductRelationships.cs" company="Microsoft">
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
    using Ecp.True.Entities.Registration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The inventory relationships.
    /// </summary>
    public static class InventoryProductRelationships
    {
        /// <summary>
        /// Configures the relationships for inventory.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void Configure(EntityTypeBuilder<InventoryProduct> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.Comment).HasMaxLength(200).IsRequired(false);
            builder.Property(e => e.UncertaintyPercentage).HasColumnType("decimal(5, 2)");

            builder.HasOne(d => d.Product)
                    .WithMany(p => p.ProductInventory)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.OwnershipTicket)
                   .WithMany(p => p.InventoryOwnerships)
                   .HasForeignKey(d => d.OwnershipTicketId)
                   .IsRequired(false);

            builder.HasOne(s => s.Reason)
                    .WithMany(p => p.ReasonInventoryProducts)
                    .HasForeignKey(d => d.ReasonId)
                    .IsRequired(false);

            builder.HasOne(s => s.System)
                    .WithMany(p => p.SystemInventoryProducts)
                    .HasForeignKey(d => d.SystemId)
                    .IsRequired(false);

            builder.HasOne(s => s.Operator)
                    .WithMany(p => p.OperatorInventoryProducts)
                    .HasForeignKey(d => d.OperatorId)
                    .IsRequired(false);

            DoConfigure(builder);
            DoConfigureOthers(builder);
        }

        private static void DoConfigure(EntityTypeBuilder<InventoryProduct> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.HasOne(d => d.SystemType)
                    .WithMany(p => p.InventoryProducts)
                    .HasForeignKey(d => d.SystemTypeId);

            builder.HasOne(d => d.Node)
                    .WithMany(p => p.InventoryProducts)
                    .HasForeignKey(d => d.NodeId);

            builder.HasOne(x => x.Ticket)
                    .WithMany(x => x.InventoryProducts)
                    .HasForeignKey(y => y.TicketId)
                    .IsRequired(false);

            builder.HasOne(x => x.Segment)
                    .WithMany(x => x.InventoryProducts)
                    .HasForeignKey(y => y.SegmentId)
                    .IsRequired(false);
        }

        private static void DoConfigureOthers(EntityTypeBuilder<InventoryProduct> builder)
        {
            builder.HasOne(d => d.FileRegistrationTransaction)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.FileRegistrationTransactionId)
                    .IsRequired(false);

            builder.HasOne(s => s.SourceSystemElement)
                    .WithMany(p => p.SourceSystemInventoryProducts)
                    .HasForeignKey(d => d.SourceSystemId)
                    .IsRequired(false);

            builder.HasOne(s => s.DeltaTicket)
                    .WithMany(p => p.DeltaInventoryProducts)
                    .HasForeignKey(d => d.DeltaTicketId)
                    .IsRequired(false);

            builder.HasOne(s => s.ProductTypeElement)
                    .WithMany(p => p.InventoryProductProductTypes)
                    .HasForeignKey(d => d.ProductType)
                    .IsRequired(false);

            builder.HasOne(s => s.MeasurementUnitElement)
                    .WithMany(p => p.InventoryProductMeasurementUnits)
                    .HasForeignKey(d => d.MeasurementUnit)
                    .IsRequired(false);
        }
    }
}