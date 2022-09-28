// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryElementConfiguration.cs" company="Microsoft">
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
    /// The category element configuration.
    /// </summary>
    public class CategoryElementConfiguration : EntityConfiguration<CategoryElement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryElementConfiguration"/> class.
        /// </summary>
        public CategoryElementConfiguration()
        : base(x => x.ElementId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<CategoryElement> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Description).HasMaxLength(1000);
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.Color).HasMaxLength(20);
            builder.Property(x => x.IsOperationalSegment).HasColumnType("bit").IsRequired(false);
            builder.Property(x => x.DeviationPercentage);

            builder.HasOne(x => x.Category)
                    .WithMany(x => x.Elements)
                    .HasForeignKey(x => x.CategoryId);

            builder.HasOne(x => x.Icon)
                .WithMany(x => x.CategoryElements)
                .HasForeignKey(x => x.IconId);
        }
    }
}
