// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologationDataMappingConfiguration.cs" company="Microsoft">
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
    /// The Homologation data mapping Configuration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{Ecp.True.Entities.Admin.HomologationDataMapping}" />
    public class HomologationDataMappingConfiguration : EntityConfiguration<HomologationDataMapping>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomologationDataMappingConfiguration"/> class.
        /// </summary>
        public HomologationDataMappingConfiguration()
            : base(x => x.HomologationDataMappingId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<HomologationDataMapping> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.SourceValue).IsRequired().HasMaxLength(100);
            builder.Property(x => x.DestinationValue).IsRequired().HasMaxLength(100);

            builder.Ignore(x => x.Value);

            builder.HasOne(x => x.HomologationGroup)
                    .WithMany(x => x.HomologationDataMapping)
                    .HasForeignKey(x => x.HomologationGroupId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
