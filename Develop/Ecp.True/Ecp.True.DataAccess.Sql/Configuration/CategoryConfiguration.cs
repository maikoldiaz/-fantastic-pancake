// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryConfiguration.cs" company="Microsoft">
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
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The category configuration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{Ecp.True.Entities.Admin.Category}" />
    public class CategoryConfiguration : EntityConfiguration<Category>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryConfiguration"/> class.
        /// </summary>
        public CategoryConfiguration()
        : base(x => x.CategoryId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<Category> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Description).HasMaxLength(1000);
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.IsGrouper).IsRequired();
            builder.Property(e => e.IsReadOnly).IsRequired();
            builder.Property(e => e.IsHomologation).IsRequired();
        }
    }
}
