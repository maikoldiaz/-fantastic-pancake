// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeConnectionProductOwnerConfiguration.cs" company="Microsoft">
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
    /// The Node Connection Product Owner Configuration.
    /// </summary>
    public class NodeConnectionProductOwnerConfiguration : EntityConfiguration<NodeConnectionProductOwner>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeConnectionProductOwnerConfiguration"/> class.
        /// </summary>
        public NodeConnectionProductOwnerConfiguration()
           : base(x => x.NodeConnectionProductOwnerId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<NodeConnectionProductOwner> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(e => e.OwnershipPercentage).HasColumnType("decimal(5,2)").IsRequired();

            builder.HasOne(x => x.NodeConnectionProduct)
                    .WithMany(c => c.Owners)
                    .HasForeignKey(a => a.NodeConnectionProductId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Owner)
                    .WithMany(c => c.NodeConnectionProductOwners)
                    .HasForeignKey(a => a.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
