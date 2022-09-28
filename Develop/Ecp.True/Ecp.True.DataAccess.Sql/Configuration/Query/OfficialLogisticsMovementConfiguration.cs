// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialLogisticsMovementConfiguration.cs" company="Microsoft">
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
    using Ecp.True.Core;
    using Ecp.True.Entities.Query;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// Ticket Entity Configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.Query.QueryEntityConfiguration{Ecp.True.Entities.TransportBalance.OfficialLogisticsMovement}" />
    public class OfficialLogisticsMovementConfiguration : QueryEntityConfiguration<GenericLogisticsMovement>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<GenericLogisticsMovement> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));
            builder.Property(x => x.ConcatMovementId);
            builder.Property(x => x.SourceNodeId);
            builder.Property(x => x.SourceNode);
            builder.Property(x => x.DestinationNodeId);
            builder.Property(x => x.DestinationNode);
            builder.Property(x => x.SourceProductId);
            builder.Property(x => x.SourceProduct);
            builder.Property(x => x.DestinationProductId);
            builder.Property(x => x.DestinationProduct);
            builder.Property(x => x.SourceStorageLocationId);
            builder.Property(x => x.SourceStorageLocation);
            builder.Property(x => x.DestinationStorageLocationId);
            builder.Property(x => x.DestinationStorageLocation);
            builder.Property(x => x.SourceLogisticCenterId);
            builder.Property(x => x.SourceLogisticCenter);
            builder.Property(x => x.DestinationLogisticCenterId);
            builder.Property(x => x.DestinationLogisticCenter);
            builder.Property(x => x.MovementTypeId);
            builder.Property(x => x.NetStandardVolume);
            builder.Property(x => x.MeasurementUnit);
            builder.Property(x => x.OwnershipValue);
            builder.Property(x => x.OwnershipValueUnit);
            builder.Property(x => x.Order);
            builder.Property(x => x.StartDate);
            builder.Property(x => x.EndDate);
            builder.Property(x => x.OperationDate);
            builder.Ignore(x => x.LogisticsMovementType);
            builder.Ignore(x => x.HomologatedMovementType);
            builder.Ignore(x => x.HasAnnulation);
            builder.Ignore(x => x.MeasurementUnitName);
        }
    }
}
