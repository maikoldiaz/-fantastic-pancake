// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileRegistrationTransactionConfiguration.cs" company="Microsoft">
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
    /// The FileRegistrationTransactionConfiguration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{FileRegistrationTransaction}" />
    public class FileRegistrationTransactionConfiguration : EntityConfiguration<FileRegistrationTransaction>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileRegistrationTransactionConfiguration"/> class.
        /// </summary>
        public FileRegistrationTransactionConfiguration()
        : base(x => x.FileRegistrationTransactionId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<FileRegistrationTransaction> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.FileRegistrationId).HasColumnType("int").IsRequired();
            builder.Property(x => x.StatusTypeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.RecordId).HasColumnName("RecordID").HasMaxLength(250).IsRequired(false);

            // Ignored properties only needed for session message
            builder.Ignore(x => x.SystemTypeId);
            builder.Ignore(x => x.UploadId);
            builder.Ignore(x => x.ActionType);
            builder.Ignore(x => x.FileRegistrationCreatedDate);
            builder.Ignore(x => x.MessageType);
            builder.Ignore(x => x.IsRetry);
            builder.Ignore(x => x.SkipValidation);

            builder.HasOne(d => d.FileRegistration)
                    .WithMany(p => p.FileRegistrationTransactions)
                    .HasForeignKey(d => d.FileRegistrationId);
        }
    }
}
