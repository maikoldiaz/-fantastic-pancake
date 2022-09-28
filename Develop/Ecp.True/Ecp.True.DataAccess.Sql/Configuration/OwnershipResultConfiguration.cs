// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipResultConfiguration.cs" company="Microsoft">
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
    /// The OwnershipResultConfiguration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{Ecp.True.Entities.Admin.OwnershipResult}" />
    public class OwnershipResultConfiguration : EntityConfiguration<OwnershipResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipResultConfiguration"/> class.
        /// </summary>
        public OwnershipResultConfiguration()
        : base(x => x.OwnershipResultId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<OwnershipResult> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.MessageTypeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.NodeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.ProductId).HasMaxLength(20).IsRequired();
            builder.Property(x => x.ExecutionDate).HasColumnType("datetime").IsRequired();
            builder.Property(x => x.InitialInventory).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.FinalInventory).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.Input).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.Output).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.OwnerId).HasColumnType("int").IsRequired();
            builder.Property(x => x.OwnershipPercentage).HasColumnType("decimal(5,2)").IsRequired();
            builder.Property(x => x.OwnershipVolume).HasColumnType("decimal(18,2)").IsRequired();

            OwnershipResultRelationships.Configure(builder);
        }
    }
}
