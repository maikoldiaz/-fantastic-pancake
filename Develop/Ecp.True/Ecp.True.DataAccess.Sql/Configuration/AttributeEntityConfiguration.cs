// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeEntityConfiguration.cs" company="Microsoft">
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
    public class AttributeEntityConfiguration : EntityConfiguration<AttributeEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeEntityConfiguration"/> class.
        /// </summary>
        public AttributeEntityConfiguration()
            : base(x => x.Id, Sql.Constants.AdminSchema, true, "Attribute")
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<AttributeEntity> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.AttributeId).IsRequired().HasColumnType("int");
            builder.Property(x => x.AttributeValue).IsRequired().HasMaxLength(150);
            builder.Property(x => x.ValueAttributeUnit).IsRequired().HasColumnType("int");
            builder.Property(x => x.AttributeDescription).HasMaxLength(150);
            builder.Property(x => x.AttributeType).HasMaxLength(150);

            builder.HasOne(d => d.InventoryProduct)
                .WithMany(p => p.Attributes)
                .HasForeignKey(d => d.InventoryProductId);

            builder.HasOne(d => d.Attribute)
                .WithMany(p => p.Attributes)
                .HasForeignKey(d => d.AttributeId);

            builder.HasOne(d => d.ValueAttributeUnitElement)
                .WithMany(p => p.ValueAttributeUnits)
                .HasForeignKey(d => d.ValueAttributeUnit);
        }
    }
}
