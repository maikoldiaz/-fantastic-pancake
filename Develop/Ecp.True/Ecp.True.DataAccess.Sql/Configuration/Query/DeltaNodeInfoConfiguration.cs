// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaNodeInfoConfiguration.cs" company="Microsoft">
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
    /// The delta node information configuration.
    /// </summary>
    public class DeltaNodeInfoConfiguration : QueryEntityConfiguration<DeltaNodeInfo>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<DeltaNodeInfo> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));
            builder.Property(x => x.TicketId);
            builder.Property(x => x.StartDate).HasColumnType("datetime");
            builder.Property(x => x.EndDate).HasColumnType("datetime");
            builder.Property(x => x.ExecutionDate).HasColumnType("datetime");
            builder.Property(x => x.CreatedBy);
            builder.Property(x => x.Status);
            builder.Property(x => x.Segment);
            builder.Property(x => x.NodeName);
            builder.Property(x => x.NodeId);
            builder.Property(x => x.SegmentId);
            builder.Property(x => x.TicketTypeId);
            builder.Property(x => x.DeltaNodeId);
            builder.Property(x => x.TicketStatus);
            builder.Property(x => x.TicketStatusId);
            builder.ToView(Sql.Constants.DeltaNodeView, Sql.Constants.AdminSchema);
        }
    }
}
