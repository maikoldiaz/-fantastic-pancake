﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PendingOfficialMovementConfiguration.cs" company="Microsoft">
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
    /// The PendingOfficialConfiguration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.Query.QueryEntityConfiguration{PendingOfficialConfiguration}" />
    public class PendingOfficialMovementConfiguration : QueryEntityConfiguration<PendingOfficialMovement>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<PendingOfficialMovement> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.MovementId);
            builder.Property(x => x.MovementTransactionId);
            builder.Property(x => x.OwnerId);
            builder.Property(x => x.SourceNodeId);
            builder.Property(x => x.SourceNodeSegmentId);
            builder.Property(x => x.SourceNodeOrder);
            builder.Property(x => x.SourceNodeSystem);
            builder.Property(x => x.DestinationNodeId);
            builder.Property(x => x.DestinationNodeSegmentID);
            builder.Property(x => x.DestinationNodeOrder);
            builder.Property(x => x.DestinationNodeSystem);
            builder.Property(x => x.SourceProductId);
            builder.Property(x => x.DestinationProductId);
            builder.Property(x => x.SystemId);
            builder.Property(x => x.OwnerShipValue);
            builder.Property(x => x.MeasurementUnit);
            builder.Property(x => x.SegmentId);
            builder.Property(x => x.OperationalDate);
            builder.Property(x => x.SegmentId);
            builder.Property(x => x.SourceProductTypeId);
            builder.Property(x => x.DestinationProductTypeId);
            builder.Property(x => x.StartDate);
            builder.Property(x => x.EndDate);
        }
    }
}
