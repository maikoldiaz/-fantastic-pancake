// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeConfigurationDataConfiguration.cs" company="Microsoft">
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
    /// The Node configuration data configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.Query.QueryEntityConfiguration{Ecp.True.Entities.Query.NodeConfiguration}" />
    public class NodeConfigurationDataConfiguration : QueryEntityConfiguration<NodeConfiguration>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<NodeConfiguration> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.NodeId);
            builder.Property(x => x.NodeOrder);
            builder.Property(x => x.ProductId);
            builder.Property(x => x.OwnerId);
            builder.Property(x => x.NodeOwnershipRuleId);
            builder.Property(x => x.NodeProductOwnershipRuleId);
            builder.Property(x => x.ProductOrder);
            builder.Property(x => x.IdentifiedLoss);
            builder.Property(x => x.UnidentifiedLoss);
            builder.Property(x => x.Tolerance);
            builder.Property(x => x.Interface);
            builder.Property(x => x.FinalInventory);
        }
    }
}
