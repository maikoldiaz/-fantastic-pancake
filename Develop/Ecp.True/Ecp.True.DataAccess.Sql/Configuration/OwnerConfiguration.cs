// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnerConfiguration.cs" company="Microsoft">
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
    using Ecp.True.Entities.Registration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The Attribute Object Configuration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{Ecp.True.Entities.Admin.AttributeObject}" />
    public class OwnerConfiguration : BlockchainEntityConfiguration<Owner>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OwnerConfiguration"/> class.
        /// </summary>
        public OwnerConfiguration()
            : base(x => x.Id, Sql.Constants.OffchainSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<Owner> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(e => e.OwnerId).IsRequired().HasColumnType("int");
            builder.Property(e => e.OwnershipValue).HasColumnType("decimal(18,2)");
            builder.Property(e => e.OwnershipValueUnit).IsRequired().HasMaxLength(50);
            builder.Property(x => x.BlockchainMovementTransactionId).HasColumnType("uniqueidentifier").IsRequired(false);
            builder.Property(x => x.BlockchainInventoryProductTransactionId).HasColumnType("uniqueidentifier").IsRequired(false);

            OwnerRelationships.Configure(builder);
        }
    }
}
