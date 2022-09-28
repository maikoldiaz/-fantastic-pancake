// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementContractConfiguration.cs" company="Microsoft">
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
    /// The Event Object Configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{MovementContract}" />
    public class MovementContractConfiguration : EntityConfiguration<MovementContract>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovementContractConfiguration"/> class.
        /// </summary>
        public MovementContractConfiguration()
            : base(x => x.MovementContractId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<MovementContract> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));
            builder.Property(x => x.DocumentNumber).HasColumnType("int").IsRequired();
            builder.Property(x => x.MovementTypeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.SourceNodeId).HasColumnType("int");
            builder.Property(x => x.DestinationNodeId).HasColumnType("int");
            builder.Property(x => x.ProductId).HasColumnType("nvarchar").IsRequired().HasMaxLength(40);
            builder.Property(x => x.StartDate).HasColumnType("datetime").IsRequired();
            builder.Property(x => x.EndDate).HasColumnType("datetime").IsRequired();
            builder.Property(x => x.Owner1Id).HasColumnType("int");
            builder.Property(x => x.Owner2Id).HasColumnType("int");
            builder.Property(x => x.Volume).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.MeasurementUnit).HasColumnType("int").IsRequired();
            builder.Property(x => x.IsDeleted).HasColumnType("bit");
            builder.Property(x => x.ContractId).HasColumnType("int").IsRequired();
            ConfigureColumnMappings(builder);
        }

        /// <summary>
        /// Configures the column mappings.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private static void ConfigureColumnMappings(EntityTypeBuilder<MovementContract> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.HasOne(t => t.DestinationNode)
                    .WithMany(p => p.DestinationMovementContracts)
                    .HasForeignKey(d => d.DestinationNodeId);

            builder.HasOne(t => t.SourceNode)
                    .WithMany(p => p.SourceMovementContracts)
                    .HasForeignKey(d => d.SourceNodeId);

            builder.HasOne(t => t.Product)
                    .WithMany(p => p.MovementContracts)
                    .HasForeignKey(d => d.ProductId)
                    .IsRequired();

            builder.HasOne(t => t.MovementType)
                    .WithMany(p => p.MovementContractMovementTypes)
                    .HasForeignKey(d => d.MovementTypeId)
                    .IsRequired();

            builder.HasOne(t => t.Owner1)
                    .WithMany(p => p.Owner1MovementContracts)
                    .HasForeignKey(d => d.Owner1Id);

            builder.HasOne(t => t.MeasurementUnitDetail)
                    .WithMany(p => p.MovementContractUnits)
                    .HasForeignKey(d => d.MeasurementUnit)
                    .IsRequired();

            builder.HasOne(t => t.Owner2)
                    .WithMany(p => p.Owner2MovementContracts)
                    .HasForeignKey(d => d.Owner2Id);

            builder.HasOne(t => t.Contract)
                   .WithMany(p => p.MovementContracts)
                   .HasForeignKey(d => d.ContractId)
                   .IsRequired();
        }
    }
}
