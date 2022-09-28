// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OffchainNodeConfiguration.cs" company="Microsoft">
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
    using Ecp.True.Entities.Registration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The offchain node configuration.
    /// </summary>
    public class OffchainNodeConfiguration : BlockchainEntityConfiguration<OffchainNode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OffchainNodeConfiguration"/> class.
        /// </summary>
        public OffchainNodeConfiguration()
            : base(a => a.Id, Sql.Constants.OffchainSchema, true, nameof(Node))
        {
        }

        /// <inheritdoc/>
        protected override void DoConfigure(EntityTypeBuilder<OffchainNode> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.NodeStateTypeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.LastUpdateDate).HasColumnName(@"LastUpdateDate").HasColumnType("datetime");

            builder.HasOne(n => n.Node).WithMany(n => n.OffchainNodes).HasForeignKey(x => x.NodeId);

            base.DoConfigure(builder);
        }
    }
}
