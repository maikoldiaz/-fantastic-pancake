// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaNodeApprovalHistoryConfiguration.cs" company="Microsoft">
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
    /// The DeltaNodeApprovalHistory Configuration.
    /// </summary>
    /// <seealso cref="EntityConfiguration{DeltaNodeApprovalHistory}" />
    public class DeltaNodeApprovalHistoryConfiguration : EntityConfiguration<DeltaNodeApprovalHistory>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaNodeApprovalHistoryConfiguration" /> class.
        /// </summary>
        public DeltaNodeApprovalHistoryConfiguration()
                : base(x => x.DeltaNodeApprovalHistoryId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<DeltaNodeApprovalHistory> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.Status).IsRequired().HasColumnType("int");
            builder.Property(s => s.Date).IsRequired().HasColumnType("datetime");
            builder.Property(x => x.NodeId).IsRequired().HasColumnType("int");
            builder.HasOne(s => s.Ticket).WithMany(p => p.DeltaNodeApprovalHistories).HasForeignKey(d => d.TicketId).IsRequired();
        }
    }
}
