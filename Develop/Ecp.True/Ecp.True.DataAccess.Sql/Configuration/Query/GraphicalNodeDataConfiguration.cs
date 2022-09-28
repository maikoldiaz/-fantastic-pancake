// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphicalNodeDataConfiguration.cs" company="Microsoft">
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
    /// The graphical node data configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.Query.QueryEntityConfiguration{Ecp.True.Entities.Query.GraphicalNodeConnection}" />
    public class GraphicalNodeDataConfiguration : QueryEntityConfiguration<GraphicalNode>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<GraphicalNode> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.NodeId);
            builder.Property(x => x.NodeName);
            builder.Property(x => x.AcceptableBalancePercentage);
            builder.Property(x => x.ControlLimit);
            builder.Property(x => x.Segment);
            builder.Property(x => x.Operator);
            builder.Property(x => x.NodeType);
            builder.Property(x => x.SegmentColor);
            builder.Property(x => x.NodeTypeIcon);
            builder.Property(x => x.IsActive);
            builder.Ignore(x => x.InputConnections);
            builder.Ignore(x => x.OutputConnections);
        }
    }
}
