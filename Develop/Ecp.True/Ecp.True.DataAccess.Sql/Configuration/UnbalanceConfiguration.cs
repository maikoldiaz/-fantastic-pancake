// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnbalanceConfiguration.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Sql.Configuration
{
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The Unbalance Configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{Unbalance}" />
    public class UnbalanceConfiguration : BlockchainEntityConfiguration<Unbalance>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnbalanceConfiguration"/> class.
        /// </summary>
        public UnbalanceConfiguration()
            : base(x => x.UnbalanceId, Sql.Constants.OffchainSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<Unbalance> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            ConfigureColumnMappings(builder);

            builder.HasOne(x => x.Node)
                    .WithMany(x => x.Unbalances)
                    .HasForeignKey(y => y.NodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(x => x.Product)
                    .WithMany(x => x.Unbalances)
                    .HasForeignKey(y => y.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(x => x.Ticket)
                    .WithMany(x => x.Unbalances)
                    .HasForeignKey(y => y.TicketId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
        }

        /// <summary>
        /// Configures the column mappings.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private static void ConfigureColumnMappings(EntityTypeBuilder<Unbalance> builder)
        {
            builder.Property(x => x.ProductId).HasMaxLength(20).IsRequired();
            builder.Property(x => x.InitialInventory).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.Inputs).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.Outputs).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.FinalInventory).HasColumnName(@"FinalInvnetory").HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.IdentifiedLosses).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.UnbalanceAmount).HasColumnName(@"Unbalance").HasColumnType("decimal").IsRequired(false);
            builder.Property(x => x.Interface).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.Tolerance).HasColumnType("decimal(18,2)").IsRequired(false);
            builder.Property(x => x.ToleranceIdentifiedLosses).HasColumnType("decimal(18,2)");
            builder.Property(x => x.ToleranceInputs).HasColumnType("decimal(18,2)");
            builder.Property(x => x.ToleranceOutputs).HasColumnType("decimal(18,2)");
            builder.Property(x => x.ToleranceInitialInventory).HasColumnType("decimal(18,2)");
            builder.Property(x => x.ToleranceFinalInventory).HasColumnType("decimal(18,2)");
            builder.Property(x => x.UnidentifiedLosses).HasColumnType("decimal(18,2)").IsRequired(false);
            builder.Property(x => x.InterfaceUnbalance).HasColumnType("decimal(18,2)");
            builder.Property(x => x.ToleranceUnbalance).HasColumnType("decimal(18,2)");
            builder.Property(x => x.UnidentifiedLossesUnbalance).HasColumnType("decimal(18,2)");
            builder.Property(x => x.CalculationDate).HasColumnType("datetime").IsRequired();
            builder.Property(x => x.StandardUncertainty).HasColumnType("decimal(18,2)").IsRequired(false);
            builder.Property(x => x.AverageUncertainty).HasColumnType("decimal(18,2)").IsRequired(false);
            builder.Property(x => x.Warning).HasColumnType("decimal(18,2)").IsRequired(false);
            builder.Property(x => x.Action).HasColumnType("decimal(18,2)").IsRequired(false);
            builder.Property(x => x.ControlTolerance).HasColumnType("decimal(18,2)").IsRequired(false);
            builder.Property(x => x.AverageUncertaintyUnbalancePercentage).HasColumnType("decimal(18,2)").IsRequired(false);
        }
    }
}
