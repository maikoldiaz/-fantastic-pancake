// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CancellationMovementDataConfiguration.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Sql.Configuration.Query
{
    using System.Globalization;
    using Ecp.True.Core;
    using Ecp.True.Entities.Query;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The Cancellation Movement data configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.Query.QueryEntityConfiguration{Ecp.True.Entities.Query.CancellationMovementDetail}" />
    public class CancellationMovementDataConfiguration : QueryEntityConfiguration<CancellationMovementDetail>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<CancellationMovementDetail> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.MovementTransactionId);
            builder.Property(x => x.OperationalDate);
            builder.Property(x => x.SourceNodeId);
            builder.Property(x => x.DestinationNodeId);
            builder.Property(x => x.SourceProductId);
            builder.Property(x => x.DestinationProductId);
            builder.Property(x => x.NetVolume);
            builder.Property(x => x.MessageTypeId);
            builder.Property(x => x.MovementType);
            builder.Property(x => x.MovementTypeId);
            builder.Property(x => x.OwnerId);
            builder.Property(x => x.OwnershipPercentage);
            builder.Property(x => x.OwnershipVolume);
            builder.Property(x => x.Unit);
            builder.Property(x => x.AppliedRule);
            builder.Property(x => x.ProductType);
            builder.Property(x => x.SegmentId);
            builder.Property(x => x.RuleVersion);
            builder.Ignore(CancellationMovementDetail.SourceSystem);
            builder.Ignore(CancellationMovementDetail.ExecutionDate.ToString(CultureInfo.InvariantCulture));
        }
    }
}
