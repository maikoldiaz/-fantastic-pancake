// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperativeNodeRelationshipConfiguration.cs" company="Microsoft">
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
    /// The Operative Node Relationship Configuration.
    /// </summary>
    /// <seealso cref="EntityConfiguration{OperativeNodeRelationship}" />
    public class OperativeNodeRelationshipConfiguration : EntityConfiguration<OperativeNodeRelationship>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperativeNodeRelationshipConfiguration" /> class.
        /// </summary>
        public OperativeNodeRelationshipConfiguration()
                : base(x => x.OperativeNodeRelationshipId, Sql.Constants.AnalyticsSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<OperativeNodeRelationship> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.TransferPoint).HasMaxLength(200).IsRequired();
            builder.Property(x => x.SourceField).HasMaxLength(200).IsRequired();
            builder.Property(x => x.FieldWaterProduction).HasMaxLength(200).IsRequired();
            builder.Property(x => x.RelatedSourceField).HasMaxLength(200).IsRequired();
            builder.Property(x => x.DestinationNode).HasMaxLength(200).IsRequired();
            builder.Property(x => x.DestinationNodeType).HasMaxLength(200).IsRequired();
            builder.Property(x => x.MovementType).HasMaxLength(200).IsRequired();
            builder.Property(x => x.SourceNode).HasMaxLength(200).IsRequired();
            builder.Property(x => x.SourceNodeType).HasMaxLength(200).IsRequired();
            builder.Property(x => x.SourceProduct).HasMaxLength(200).IsRequired();
            builder.Property(x => x.SourceProductType).HasMaxLength(200).IsRequired();
            builder.Property(x => x.LoadDate).HasColumnType("datetime").IsRequired();
            builder.Property(x => x.ExecutionID).HasColumnType("uniqueidentifier").IsRequired(false);
            builder.Property(x => x.Notes).HasMaxLength(200).IsRequired(false);
            builder.Property(x => x.IsDeleted).HasColumnType("bit").IsRequired(false);
        }
    }
}
