// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileRegistrationInfoConfiguration.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Sql.Configuration.Query
{
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The register file configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.Query.QueryEntityConfiguration{Ecp.True.Entities.Admin.FileRegistrationInfo}" />
    public class FileRegistrationInfoConfiguration : QueryEntityConfiguration<FileRegistrationInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileRegistrationInfoConfiguration"/> class.
        /// </summary>
        public FileRegistrationInfoConfiguration()
        : base(false)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<FileRegistrationInfo> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.ErrorCount);
            builder.Property(x => x.Status);
            builder.Property(x => x.SystemTypeId);
            builder.Property(x => x.UploadId);
            builder.Property(x => x.Name);
            builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").HasColumnType("datetime");
            builder.Property(x => x.CreatedBy);
            builder.Property(x => x.ActionType).HasColumnName("FileActionType");
            builder.Property(x => x.SegmentName);
            builder.Property(x => x.RecordsProcessed);
            builder.Property(x => x.IsParsed);
            builder.ToView("view_FileRegistrationStatus", Sql.Constants.AdminSchema);
        }
    }
}
