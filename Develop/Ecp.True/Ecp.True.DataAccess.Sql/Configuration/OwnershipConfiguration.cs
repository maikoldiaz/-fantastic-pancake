// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipConfiguration.cs" company="Microsoft">
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
    /// The OwnershipConfiguration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{Ecp.True.Entities.Admin.Ownership}" />
    public class OwnershipConfiguration : BlockchainEntityConfiguration<Ownership>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipConfiguration"/> class.
        /// </summary>
        public OwnershipConfiguration()
        : base(x => x.OwnershipId, Sql.Constants.OffchainSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<Ownership> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.MessageTypeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.OwnerId).HasColumnType("int").IsRequired();

            builder.Property(x => x.OwnershipPercentage).HasColumnType("decimal(5,2)").IsRequired();
            builder.Property(x => x.OwnershipVolume).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.AppliedRule).HasColumnType("nvarchar").HasMaxLength(50).IsRequired();
            builder.Property(x => x.RuleVersion).HasColumnType("nvarchar").HasMaxLength(20).IsRequired();
            builder.Property(x => x.ExecutionDate).HasColumnType("datetime").IsRequired();
            builder.Property(x => x.BlockchainMovementTransactionId).HasColumnType("uniqueidentifier").IsRequired(false);
            builder.Property(x => x.BlockchainInventoryProductTransactionId).HasColumnType("uniqueidentifier").IsRequired(false);
            builder.Property(x => x.BlockchainOwnershipId).HasColumnType("uniqueidentifier").IsRequired(false);
            builder.Property(x => x.PreviousBlockchainOwnershipId).HasColumnType("uniqueidentifier").IsRequired(false);
            builder.Property(x => x.IsDeleted).HasColumnType("bit").IsRequired();
            builder.Property(x => x.EventType).HasMaxLength(25);
            builder.Property(x => x.DeltaTicketId).IsRequired(false).HasColumnType("int");
            OwnershipRelationships.Configure(builder);
        }
    }
}
