// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageLocationProductConfiguration.cs" company="Microsoft">
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
    /// The Storage Location Product Configuration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{Ecp.True.Entities.Admin.StorageLocationProducts}" />
    public class StorageLocationProductConfiguration : EntityConfiguration<StorageLocationProduct>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageLocationProductConfiguration"/> class.
        /// </summary>
        public StorageLocationProductConfiguration()
            : base(x => x.StorageLocationProductId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<StorageLocationProduct> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(e => e.ProductId).IsRequired().HasMaxLength(20);
            builder.Property(e => e.UncertaintyPercentage).HasColumnType("decimal(5, 2)");

            StorageLocationProductRelationships.Configure(builder);
        }
    }
}
