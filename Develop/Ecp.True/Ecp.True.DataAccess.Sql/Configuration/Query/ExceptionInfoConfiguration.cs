// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionInfoConfiguration.cs" company="Microsoft">
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
    /// The exception information configuration.
    /// </summary>
    public class ExceptionInfoConfiguration : QueryEntityConfiguration<ExceptionInfo>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<ExceptionInfo> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));
            builder.Property(x => x.IsRetry);
            builder.Property(x => x.ErrorId);
            builder.Property(x => x.Id);
            builder.Property(x => x.FileRegistrationTransactionId);
            builder.Property(x => x.MessageId);
            builder.Property(x => x.SystemName);
            builder.Property(x => x.SystemTypeName);
            builder.Property(x => x.Process);
            builder.Property(x => x.FileName);
            builder.Property(x => x.UploadId);

            builder.Property(x => x.CreationDate).HasColumnType("datetime");
            builder.ToView(Sql.Constants.ExceptionView, Sql.Constants.AdminSchema);
        }
    }
}
