// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologationGroupConfiguration.cs" company="Microsoft">
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
    /// The Homologation group Configuration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{Ecp.True.Entities.Admin.HomologationGroup}" />
    public class HomologationGroupConfiguration : EntityConfiguration<HomologationGroup>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomologationGroupConfiguration"/> class.
        /// </summary>
        public HomologationGroupConfiguration()
            : base(x => x.HomologationGroupId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<HomologationGroup> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.HasOne(x => x.Homologation)
                    .WithMany(x => x.HomologationGroups)
                    .HasForeignKey(d => d.HomologationId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Group)
                .WithMany(x => x.Groups)
                .HasForeignKey(x => x.GroupTypeId);
        }
    }
}
