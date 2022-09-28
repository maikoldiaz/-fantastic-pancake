// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartnerOwnerMappingConfiguration.cs" company="Microsoft">
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
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The Algorithm Configuration.
    /// </summary>
    /// <seealso cref="EntityConfiguration{Algorithm}" />
    public class PartnerOwnerMappingConfiguration : EntityConfiguration<PartnerOwnerMapping>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PartnerOwnerMappingConfiguration" /> class.
        /// </summary>
        public PartnerOwnerMappingConfiguration()
                : base(x => x.PartnerOwnerMappingId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<PartnerOwnerMapping> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.HasOne(s => s.GrandOwner).WithMany(p => p.GrandOwners).HasForeignKey(d => d.GrandOwnerId);
            builder.HasOne(s => s.PartnerOwner).WithMany(p => p.PartnerOwners).HasForeignKey(d => d.PartnerOwnerId);
        }
    }
}
