// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeConnectionProductRuleEntityConfiguration.cs" company="Microsoft">
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
    /// The OwnershipNodeConfiguration.
    /// </summary>
    /// <seealso cref="QueryEntityConfiguration{Ecp.True.Entities.Admin.NodeConnectionProductRuleEntity}" />
    public class NodeConnectionProductRuleEntityConfiguration : QueryEntityConfiguration<NodeConnectionProductRuleEntity>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<NodeConnectionProductRuleEntity> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));
            builder.Property(x => x.NodeConnectionProductId);
            builder.Property(x => x.SourceOperator);
            builder.Property(x => x.DestinationOperator);
            builder.Property(x => x.SourceNode);
            builder.Property(x => x.DestinationNode);
            builder.Property(x => x.Product);
            builder.Property(x => x.RuleId);
            builder.Property(x => x.RuleName);
            builder.ToView(Sql.Constants.NodeConnectionProductRuleView, Sql.Constants.AdminSchema);
        }
    }
}
