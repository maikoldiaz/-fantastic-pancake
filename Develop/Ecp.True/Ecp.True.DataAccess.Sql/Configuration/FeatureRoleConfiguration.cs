// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FeatureRoleConfiguration.cs" company="Microsoft">
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
    using Ecp.True.Entities;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The FeatureRoleConfiguration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{Ecp.True.Entities.FeatureRole}" />
    public class FeatureRoleConfiguration : EntityConfiguration<FeatureRole>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureRoleConfiguration"/> class.
        /// </summary>
        public FeatureRoleConfiguration()
                : base(x => x.FeatureRoleId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<FeatureRole> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Ignore(x => x.LastModifiedBy);
            builder.Ignore(x => x.LastModifiedDate);

            builder.HasOne(x => x.Feature)
                .WithMany(y => y.FeatureRoles)
                .HasForeignKey(z => z.FeatureId);

            builder.HasOne(x => x.Role)
                .WithMany(y => y.FeatureRoles)
                .HasForeignKey(z => z.RoleId);
        }
    }
}
