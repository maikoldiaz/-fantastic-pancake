// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeConnectionProductRelationships.cs" company="Microsoft">
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

    /// <summary>
    /// The unbalance comment relationships.
    /// </summary>
    public static class NodeConnectionProductRelationships
    {
        /// <summary>
        /// Configures the relationships for unbalance comment.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void Configure(EntityTypeBuilder<NodeConnectionProduct> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.HasOne(x => x.NodeConnection)
                    .WithMany(c => c.Products)
                    .HasForeignKey(a => a.NodeConnectionId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Product)
                    .WithMany(c => c.NodeConnectionProducts)
                    .HasForeignKey(a => a.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.NodeConnectionProductRule)
                    .WithMany(p => p.NodeConnectionProducts)
                    .HasForeignKey(a => a.NodeConnectionProductRuleId);
        }
    }
}
