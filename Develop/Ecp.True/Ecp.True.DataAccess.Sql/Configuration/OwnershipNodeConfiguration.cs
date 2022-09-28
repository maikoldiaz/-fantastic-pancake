// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipNodeConfiguration.cs" company="Microsoft">
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
    /// The OwnershipNodeConfiguration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{Ecp.True.Entities.Admin.OwnershipNode}" />
    public class OwnershipNodeConfiguration : EntityConfiguration<OwnershipNode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipNodeConfiguration"/> class.
        /// </summary>
        public OwnershipNodeConfiguration()
        : base(x => x.OwnershipNodeId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<OwnershipNode> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.TicketId).HasColumnType("int").IsRequired();
            builder.Property(x => x.NodeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.Status).HasColumnType("int").IsRequired();
            builder.Property(x => x.OwnershipStatus).HasColumnName("OwnershipStatusId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.Editor).HasColumnType("nvarchar").HasMaxLength(50).IsRequired(false);
            builder.Property(x => x.EditorConnectionId).HasColumnType("nvarchar").HasMaxLength(50).IsRequired(false);
            builder.Property(x => x.Comment).HasColumnType("nvarchar").HasMaxLength(200).IsRequired(false);
            builder.Property(x => x.ApproverAlias).HasColumnName("ApproverAlias").HasColumnType("nvarchar").HasMaxLength(50).IsRequired(false);
            builder.Property(x => x.RegistrationDate).HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.OwnershipAnalyticsErrorMessage).HasColumnType("nvarchar").HasMaxLength(255).IsRequired(false);
            builder.Property(x => x.OwnershipAnalyticsStatus).HasColumnType("int").IsRequired(false);
            OwnershipNodeRelationships.Configure(builder);
        }
    }
}
