// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportExecutionEntityConfiguration.cs" company="Microsoft">
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
    public class ReportExecutionEntityConfiguration : QueryEntityConfiguration<ReportExecutionEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportExecutionEntityConfiguration"/> class.
        /// </summary>
        public ReportExecutionEntityConfiguration()
        : base(false)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<ReportExecutionEntity> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.StartDate).HasColumnType("datetime");
            builder.Property(x => x.EndDate).HasColumnType("datetime");
            builder.Property(x => x.CreatedDate).HasColumnType("datetime");
            builder.Ignore(x => x.CreatedBy);
            builder.ToView("view_ReportExecution", Sql.Constants.AdminSchema);
        }
    }
}
