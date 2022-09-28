// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaErrorConfiguration.cs" company="Microsoft">
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
    /// The DeltaError Configuration.
    /// </summary>
    /// <seealso cref="EntityConfiguration{DeltaError}" />
    public class DeltaErrorConfiguration : EntityConfiguration<DeltaError>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaErrorConfiguration" /> class.
        /// </summary>
        public DeltaErrorConfiguration()
                : base(x => x.DeltaErrorId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<DeltaError> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.MovementTransactionId).IsRequired(false).HasColumnType("int");
            builder.Property(x => x.InventoryProductId).IsRequired(false).HasColumnType("int");
            builder.Property(x => x.TicketId).IsRequired().HasColumnType("int");
            builder.Property(x => x.ErrorMessage).IsRequired().HasColumnType("nvarchar(max)");

            builder.HasOne(d => d.Movement)
                .WithMany(p => p.DeltaErrors)
                .HasForeignKey(d => d.MovementTransactionId);

            builder.HasOne(s => s.InventoryProduct)
                .WithMany(p => p.DeltaErrors)
                .HasForeignKey(d => d.InventoryProductId);

            builder.HasOne(s => s.Ticket)
                .WithMany(p => p.DeltaErrors)
                .HasForeignKey(d => d.TicketId);
        }
    }
}
