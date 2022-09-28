// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaNodeErrorConfiguration.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Sql
{
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Sql.Configuration;
    using Ecp.True.Entities.Admin;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The DeltaError Configuration.
    /// </summary>
    /// <seealso cref="EntityConfiguration{DeltaError}" />
    public class DeltaNodeErrorConfiguration : EntityConfiguration<DeltaNodeError>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaNodeErrorConfiguration" /> class.
        /// </summary>
        public DeltaNodeErrorConfiguration()
                : base(x => x.DeltaNodeErrorId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<DeltaNodeError> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.ErrorMessage).IsRequired().HasColumnType("nvarchar(max)");
            builder.Ignore(x => x.LastModifiedBy);
            builder.Ignore(x => x.LastModifiedDate);
            builder.HasOne(s => s.DeltaNode).WithMany(p => p.DeltaNodeErrors).HasForeignKey(d => d.DeltaNodeId).IsRequired();
            builder.HasOne(s => s.InventoryProduct).WithMany(p => p.DeltaNodeErrors).HasForeignKey(d => d.InventoryProductId).IsRequired(false);
            builder.HasOne(s => s.Movement).WithMany(p => p.DeltaNodeErrors).HasForeignKey(d => d.MovementTransactionId).IsRequired(false);
            builder.HasOne(x => x.ConsolidatedMovement).WithMany(x => x.DeltaNodeErrors).HasForeignKey(d => d.ConsolidatedMovementId).IsRequired(false);
            builder.HasOne(x => x.ConsolidatedInventoryProduct).WithMany(x => x.DeltaNodeErrors).HasForeignKey(d => d.ConsolidatedInventoryProductId).IsRequired(false);
        }
    }
}
