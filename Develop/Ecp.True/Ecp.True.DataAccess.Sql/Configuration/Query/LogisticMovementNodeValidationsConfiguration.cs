// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogisticMovementNodeValidationsConfiguration.cs" company="Microsoft">
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
    using Ecp.True.Entities.Query;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The database validation configuration class.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.Query.QueryEntityConfiguration{Ecp.True.Entities.Dto.NodesForSegmentResult}" />
    public class LogisticMovementNodeValidationsConfiguration : QueryEntityConfiguration<NodesForSegmentResult>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<NodesForSegmentResult> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.SegmentId);
            builder.Property(x => x.NodeId);
            builder.Property(x => x.NodeName);
            builder.Property(x => x.OperationDate);
            builder.Property(x => x.StartDate);
            builder.Property(x => x.EndDate);
            builder.Property(x => x.StatusId);
            builder.Property(x => x.StatusName);
            builder.Property(x => x.TicketStatusName);
            builder.Property(x => x.IsEnabledForSendToSap);
            builder.Property(x => x.IsApproved);
            builder.Property(x => x.IsActiveInBatch);
            builder.Property(x => x.PredecessorIsApproved);
        }
    }
}
