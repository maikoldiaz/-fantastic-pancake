// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PendingTransactionErrorConfiguration.cs" company="Microsoft">
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
    /// The pending transaction error configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{Ecp.True.Entities.TransportBalance.PendingTransactionError}" />
    public class PendingTransactionErrorConfiguration : EntityConfiguration<PendingTransactionError>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PendingTransactionErrorConfiguration"/> class.
        /// </summary>
        public PendingTransactionErrorConfiguration()
              : base(x => x.ErrorId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<PendingTransactionError> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.Comment).HasMaxLength(1000);
            builder.Property(x => x.ErrorMessage).HasMaxLength(500);
            builder.Property(x => x.RecordId).HasColumnName("RecordID").HasMaxLength(250).IsRequired(false);
            builder.Property(x => x.IsRetrying).IsRequired().HasDefaultValue();
            builder.Property(x => x.SessionId).HasColumnType("nvarchar").HasMaxLength(50).IsRequired(false);

            builder.HasOne(x => x.PendingTransaction)
                    .WithMany(x => x.Errors)
                    .HasForeignKey(a => a.TransactionId);
        }
    }
}
