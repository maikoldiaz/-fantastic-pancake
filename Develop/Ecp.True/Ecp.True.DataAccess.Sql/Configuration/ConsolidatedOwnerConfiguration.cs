// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidatedOwnerConfiguration.cs" company="Microsoft">
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
    /// The Consolidated Owner Configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{Ecp.True.Entities.Admin.ConsolidatedOwner}" />
    public class ConsolidatedOwnerConfiguration : EntityConfiguration<ConsolidatedOwner>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsolidatedOwnerConfiguration"/> class.
        /// </summary>
        public ConsolidatedOwnerConfiguration()
            : base(x => x.ConsolidatedOwnerId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<ConsolidatedOwner> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));
            builder.Property(x => x.OwnerId).HasColumnType("int").IsRequired();
            builder.Property(x => x.OwnershipVolume).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.OwnershipPercentage).HasColumnType("decimal(5,2)").IsRequired();

            builder.HasOne(x => x.Owner).WithMany(x => x.ConsolidatedOwners).HasForeignKey(d => d.OwnerId).IsRequired();
            builder.HasOne(x => x.ConsolidatedMovement).WithMany(x => x.ConsolidatedOwners).HasForeignKey(d => d.ConsolidatedMovementId).IsRequired(false);
            builder.HasOne(x => x.ConsolidatedInventoryProduct).WithMany(x => x.ConsolidatedOwners).HasForeignKey(d => d.ConsolidatedInventoryProductId).IsRequired(false);
        }
    }
}
