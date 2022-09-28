// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryProductConfiguration.cs" company="Microsoft">
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
    /// The Attribute Object Configuration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{Ecp.True.Entities.Admin.AttributeObject}" />
    public class InventoryProductConfiguration : BlockchainEntityConfiguration<InventoryProduct>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryProductConfiguration"/> class.
        /// </summary>
        public InventoryProductConfiguration()
            : base(x => x.InventoryProductId, Sql.Constants.OffchainSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<InventoryProduct> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));
            builder.Property(e => e.ProductType).IsRequired(false).HasColumnType("int");
            builder.Property(e => e.ProductId).IsRequired().HasMaxLength(20);
            builder.Property(e => e.ProductVolume).HasColumnType("decimal(18,2)");
            builder.Property(e => e.GrossStandardQuantity).HasColumnType("decimal(18,2)");
            builder.Property(e => e.MeasurementUnit).IsRequired(false).HasColumnType("int");
            builder.Property(x => x.BlockchainInventoryProductTransactionId).HasColumnType("uniqueidentifier").IsRequired(false);
            builder.Property(x => x.BatchId).HasMaxLength(150).IsRequired(false);
            builder.Property(x => x.InventoryProductUniqueId).HasMaxLength(150);
            builder.Property(x => x.PreviousBlockchainInventoryProductTransactionId).HasColumnType("uniqueidentifier").IsRequired(false);
            ConfigureColumnMappings(builder);

            InventoryProductRelationships.Configure(builder);
        }

        /// <summary>
        /// Configures the column mappings.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private static void ConfigureColumnMappings(EntityTypeBuilder<InventoryProduct> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));
            builder.Property(e => e.DestinationSystem).IsRequired().HasMaxLength(25);
            builder.Property(e => e.EventType).IsRequired().HasMaxLength(10);
            builder.Property(e => e.InventoryDate).HasColumnType("date");
            builder.Property(e => e.InventoryId).HasMaxLength(50).IsRequired().IsUnicode(false);
            builder.Property(e => e.Observations).HasMaxLength(150).IsRequired(false);
            builder.Property(x => x.ScenarioId).HasColumnType("int").IsRequired();
            builder.Property(e => e.TankName).HasMaxLength(20).IsRequired(false);
            builder.Property(e => e.SystemTypeId).IsRequired();
            builder.Property(x => x.OperatorId).HasColumnType("int").IsRequired(false);
            builder.Property(x => x.Version).IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.DeltaTicketId).IsRequired(false).HasColumnType("int");
            builder.Property(x => x.OfficialDeltaTicketId).IsRequired(false).HasColumnType("int");
        }
    }
}
