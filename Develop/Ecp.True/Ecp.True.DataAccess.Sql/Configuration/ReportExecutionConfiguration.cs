// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportExecutionConfiguration.cs" company="Microsoft">
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
    using SqlConstants = Ecp.True.DataAccess.Sql.Constants;

    /// <summary>
    /// The execution status configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{Ecp.True.Entities.ReportExecution}" />
    public class ReportExecutionConfiguration : EntityConfiguration<ReportExecution>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportExecutionConfiguration"/> class.
        /// </summary>
        public ReportExecutionConfiguration()
        : base(x => x.ExecutionId, SqlConstants.AdminSchema, false)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<ReportExecution> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.StatusTypeId).IsRequired().HasColumnType("int");
            builder.Property(x => x.ReportTypeId).IsRequired().HasColumnType("int");
            builder.Property(x => x.ScenarioId).IsRequired().HasColumnType("int");
            builder.Property(x => x.StartDate).IsRequired().HasColumnType("date");
            builder.Property(x => x.EndDate).IsRequired().HasColumnType("date");
            builder.Property(x => x.Name).IsRequired(false).HasColumnType("nvarchar(200)");
            builder.Property(x => x.Hash).IsRequired(false).HasColumnType("nvarchar(256)");
            builder.Property(x => x.OwnerId);

            builder.HasOne(s => s.Element).WithMany(p => p.ReportExecutions).HasForeignKey(d => d.ElementId).IsRequired(false);
            builder.HasOne(s => s.Node).WithMany(p => p.ReportExecutions).HasForeignKey(d => d.NodeId).IsRequired(false);
            builder.HasOne(s => s.Category).WithMany(p => p.ReportExecutions).HasForeignKey(d => d.CategoryId).IsRequired(false);
        }
    }
}
