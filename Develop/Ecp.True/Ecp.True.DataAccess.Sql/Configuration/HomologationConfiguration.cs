// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologationConfiguration.cs" company="Microsoft">
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
    /// The Homologation Configuration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{Ecp.True.Entities.Admin.Homologation}" />
    public class HomologationConfiguration : EntityConfiguration<Homologation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomologationConfiguration"/> class.
        /// </summary>
        public HomologationConfiguration()
            : base(x => x.HomologationId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<Homologation> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.SourceSystemId).HasColumnType("int").IsRequired();
            builder.Property(x => x.DestinationSystemId).HasColumnType("int").IsRequired();

            builder.HasOne(x => x.SourceSystem)
                .WithMany(t => t.SourceHomologationSystems)
                .HasForeignKey(x => x.SourceSystemId);

            builder.HasOne(x => x.DestinationSystem)
                .WithMany(t => t.DestinationHomologationSystems)
                .HasForeignKey(x => x.DestinationSystemId);
        }
    }
}
