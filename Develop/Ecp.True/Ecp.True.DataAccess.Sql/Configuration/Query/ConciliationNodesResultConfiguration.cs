// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConciliationNodesResultConfiguration.cs" company="Microsoft">
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
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.Query.QueryEntityConfiguration{Ecp.True.Entities.Query.ConciliationNodesResult}" />
    public class ConciliationNodesResultConfiguration : QueryEntityConfiguration<ConciliationNodesResult>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<ConciliationNodesResult> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.NodeConnectionId);
            builder.Property(x => x.Description);
            builder.Property(x => x.DestinationNodeId);
            builder.Property(x => x.DestinationNodeName);
            builder.Property(x => x.DestinationSegmentId);
            builder.Property(x => x.DestinationSegmentName);
            builder.Property(x => x.SourceNodeId);
            builder.Property(x => x.SourceNodeName);
            builder.Property(x => x.SourceSegmentId);
            builder.Property(x => x.SourceSegmentName);
        }
    }
}
