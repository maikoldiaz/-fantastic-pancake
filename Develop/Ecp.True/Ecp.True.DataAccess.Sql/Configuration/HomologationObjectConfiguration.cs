// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologationObjectConfiguration.cs" company="Microsoft">
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
    /// The Homologation object Configuration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{Ecp.True.Entities.Admin.HomologationObject}" />
    public class HomologationObjectConfiguration : EntityConfiguration<HomologationObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomologationObjectConfiguration"/> class.
        /// </summary>
        public HomologationObjectConfiguration()
            : base(x => x.HomologationObjectId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<HomologationObject> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.HomologationObjectTypeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.IsRequiredMapping).HasColumnType("bit").IsRequired();

            builder.HasOne(x => x.HomologationGroup)
                    .WithMany(x => x.HomologationObjects)
                    .HasForeignKey(d => d.HomologationGroupId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.HomologationObjectType)
                    .WithMany(x => x.HomologationObjects)
                    .HasForeignKey(x => x.HomologationObjectTypeId);

            builder.HasIndex(x => new { x.HomologationGroupId, x.HomologationObjectTypeId }).IsUnique();
        }
    }
}
