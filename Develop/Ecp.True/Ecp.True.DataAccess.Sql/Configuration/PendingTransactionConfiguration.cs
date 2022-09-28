// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PendingTransactionConfiguration.cs" company="Microsoft">
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
    /// The Product Configuration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{Ecp.True.Entities.Admin.Product}" />
    public class PendingTransactionConfiguration : EntityConfiguration<PendingTransaction>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PendingTransactionConfiguration"/> class.
        /// </summary>
        public PendingTransactionConfiguration()
            : base(x => x.TransactionId, Sql.Constants.AdminSchema, false)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<PendingTransaction> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            PendingTransactionProperties.Configure(builder);

            builder.Property(e => e.BlobName).IsRequired().HasMaxLength(500);
            builder.Property(e => e.MessageId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.ErrorJson).IsRequired();
            builder.Property(e => e.SystemName).IsRequired(false).HasColumnType("int");
            builder.Property(e => e.TypeId).HasColumnType("int").IsRequired(false);
            builder.Property(e => e.SegmentId).HasColumnType("int").IsRequired(false);
            builder.Property(e => e.OwnerId).HasColumnType("int").IsRequired(false);
            builder.Property(x => x.ActionType).HasMaxLength(50).IsRequired(false);
            builder.Property(x => x.MessageType).HasMaxLength(50).IsRequired(false);
            builder.Property(x => x.ScenarioId).IsRequired(false);
            builder.Property(x => x.OriginMessageId).IsRequired(false);
            builder.HasOne(x => x.Ticket).WithMany(x => x.PendingTransactions).HasForeignKey(x => x.TicketId);
            builder.HasOne(s => s.Type).WithMany(p => p.PendingTransactionTypes).HasForeignKey(d => d.TypeId);
            builder.HasOne(s => s.Owner).WithMany(p => p.PendingTransactionOwners).HasForeignKey(d => d.OwnerId);
            builder.HasOne(s => s.Unit).WithMany(p => p.PendingTransactionUnits).HasForeignKey(d => d.Units);
        }
    }
}
