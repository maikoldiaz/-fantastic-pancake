// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeProductRuleEntityConfiguration.cs" company="Microsoft">
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
    /// The Node Product Rule Entity Configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.Query.QueryEntityConfiguration{Ecp.True.Entities.Admin.NodeProductRuleEntity}" />
    public class NodeProductRuleEntityConfiguration : QueryEntityConfiguration<NodeProductRuleEntity>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<NodeProductRuleEntity> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));
            builder.Property(x => x.Segment);
            builder.Property(x => x.Operator);
            builder.Property(x => x.NodeType);
            builder.Property(x => x.NodeName);
            builder.Property(x => x.StorageLocation);
            builder.Property(x => x.Product);
            builder.Property(x => x.RuleId);
            builder.Property(x => x.RuleName);
            builder.Property(x => x.StorageLocationProductId);
            builder.ToView(Sql.Constants.NodeProductRuleView, Sql.Constants.AdminSchema);
        }
    }
}
