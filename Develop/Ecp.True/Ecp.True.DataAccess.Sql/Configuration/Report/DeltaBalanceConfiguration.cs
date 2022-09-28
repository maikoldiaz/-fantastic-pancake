// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaBalanceConfiguration.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Sql
{
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Sql.Configuration;
    using Ecp.True.Entities.Admin;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The Delta Balance Configuration.
    /// </summary>
    /// <seealso cref="EntityConfiguration{DeltaBalance}" />
    public class DeltaBalanceConfiguration : EntityConfiguration<DeltaBalance>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaBalanceConfiguration" /> class.
        /// </summary>
        public DeltaBalanceConfiguration()
                : base(x => x.DeltaBalanceId, Sql.Constants.ReportSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<DeltaBalance> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.NodeId).IsRequired().HasColumnType("int");
            builder.Property(x => x.SegmentId).IsRequired().HasColumnType("int");
            builder.Property(x => x.StartDate).IsRequired().HasColumnType("date");
            builder.Property(x => x.EndDate).IsRequired().HasColumnType("date");
            builder.Property(x => x.Input).IsRequired(false).HasColumnType("decimal(21,2)");
            builder.Property(x => x.Output).IsRequired(false).HasColumnType("decimal(21,2)");
            builder.Property(x => x.DeltaInput).IsRequired(false).HasColumnType("decimal(21,2)");
            builder.Property(x => x.DeltaOutput).IsRequired(false).HasColumnType("decimal(21,2)");
            builder.Property(x => x.InitialInventory).IsRequired(false).HasColumnType("decimal(21,2)");
            builder.Property(x => x.FinalInventory).IsRequired(false).HasColumnType("decimal(21,2)");
            builder.Property(x => x.DeltaInitialInventory).IsRequired(false).HasColumnType("decimal(21,2)");
            builder.Property(x => x.DeltaFinalInventory).IsRequired(false).HasColumnType("decimal(21,2)");
            builder.Property(x => x.Control).IsRequired(false).HasColumnType("decimal(21,2)");
            builder.Property(x => x.ElementOwnerId).IsRequired().HasColumnType("int");
            builder.Property(x => x.ProductId).IsRequired().HasMaxLength(20);
            builder.Property(x => x.MeasurementUnit).HasMaxLength(150).IsRequired(false);
            builder.Ignore(x => x.LastModifiedBy);
            builder.Ignore(x => x.LastModifiedDate);
        }
    }
}
