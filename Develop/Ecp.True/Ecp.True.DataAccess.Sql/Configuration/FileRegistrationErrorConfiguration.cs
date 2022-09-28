// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileRegistrationErrorConfiguration.cs" company="Microsoft">
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
    /// The register file error log configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{Ecp.True.Entities.Admin.FileRegistrationError}" />
    public class FileRegistrationErrorConfiguration : EntityConfiguration<FileRegistrationError>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileRegistrationErrorConfiguration"/> class.
        /// </summary>
        public FileRegistrationErrorConfiguration()
            : base(x => x.FileRegistrationErrorId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<FileRegistrationError> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Ignore(x => x.TempId);
            builder.Property(x => x.FileRegistrationId).IsRequired();
            builder.Property(x => x.ErrorMessage).IsRequired();
            builder.Property(x => x.MessageId).HasMaxLength(50);

            builder.HasOne(x => x.FileRegistration)
                    .WithMany(x => x.FileRegistrationErrors)
                    .HasForeignKey(a => a.FileRegistrationId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Ignore(x => x.LastModifiedBy);
            builder.Ignore(x => x.LastModifiedDate);
        }
    }
}
