// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileRegistrationConfiguration.cs" company="Microsoft">
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
    /// The register file configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{Ecp.True.Entities.Admin.FileRegistration}" />
    public class FileRegistrationConfiguration : EntityConfiguration<FileRegistration>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileRegistrationConfiguration"/> class.
        /// </summary>
        public FileRegistrationConfiguration()
        : base(x => x.FileRegistrationId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<FileRegistration> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.SystemTypeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.UploadId).IsRequired().HasMaxLength(50);
            builder.Property(x => x.MessageDate).HasColumnName("UploadDate").HasColumnType("datetime");
            builder.Property(x => x.Name).IsRequired().HasMaxLength(500);
            builder.Property(x => x.ActionType).HasColumnName("Action").HasColumnType("int");
            builder.Property(x => x.FileUploadStatus).HasColumnName("Status").HasColumnType("int");
            builder.Property(x => x.BlobPath).IsRequired();
            builder.Property(x => x.PreviousUploadId).HasColumnType("uniqueidentifier");
            builder.Property(x => x.SourceSystem).HasColumnType("varchar").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.SourceTypeId);

            FileRegistrationRelationships.Configure(builder);
        }
    }
}
