// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperativeNodeRelationshipWithOwnershipConfiguration.cs" company="Microsoft">
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
    using Ecp.True.Entities.Analytics;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The Operative Node Relationship With Ownership Configuration.
    /// </summary>
    /// <seealso cref="EntityConfiguration{OperativeNodeRelationshipWithOwnership}" />
    public class OperativeNodeRelationshipWithOwnershipConfiguration : EntityConfiguration<OperativeNodeRelationshipWithOwnership>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperativeNodeRelationshipWithOwnershipConfiguration" /> class.
        /// </summary>
        public OperativeNodeRelationshipWithOwnershipConfiguration()
                : base(x => x.OperativeNodeRelationshipWithOwnershipId, Sql.Constants.AnalyticsSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<OperativeNodeRelationshipWithOwnership> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.TransferPoint).HasMaxLength(200).IsRequired();
            builder.Property(x => x.LogisticDestinationCenter).HasMaxLength(200).IsRequired();
            builder.Property(x => x.DestinationProduct).HasMaxLength(200).IsRequired();
            builder.Property(x => x.LogisticSourceCenter).HasMaxLength(200).IsRequired();
            builder.Property(x => x.SourceProduct).HasMaxLength(200).IsRequired();
            builder.Property(x => x.SourceSystem).HasMaxLength(200).IsRequired();
            builder.Property(x => x.LoadDate).HasColumnType("datetime").IsRequired();
            builder.Property(x => x.ExecutionID).HasColumnType("uniqueidentifier").IsRequired(false);
            builder.Property(x => x.Notes).HasMaxLength(1000).IsRequired(false);
            builder.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(true);
        }
    }
}
