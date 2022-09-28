﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaNodeMovementConfiguration.cs" company="Microsoft">
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
    using System.Diagnostics.CodeAnalysis;
    using Ecp.True.Core;
    using Ecp.True.Entities.Query;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The OfficialDeltaNodeMovementConfiguration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.Query.QueryEntityConfiguration{OfficialDeltaNodeMovementConfiguration}" />
    [ExcludeFromCodeCoverage]
    public class OfficialDeltaNodeMovementConfiguration : QueryEntityConfiguration<OfficialDeltaNodeMovement>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<OfficialDeltaNodeMovement> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.MovementTransactionId);
            builder.Property(x => x.MovementOwnerId);
            builder.Property(x => x.SourceNodeId);
            builder.Property(x => x.DestinationNodeId);
            builder.Property(x => x.SourceProductId);
            builder.Property(x => x.DestinationProductId);
            builder.Property(x => x.MovementTypeId);
            builder.Property(x => x.MovementId);
            builder.Property(x => x.OwnerId);
            builder.Property(x => x.OwnershipValue);
            builder.Property(x => x.OwnershipValueUnit);
            builder.Property(x => x.SegmentId);
            builder.Property(x => x.MeasurementUnit);
            builder.Property(x => x.OperationalDate);
            builder.Property(x => x.StartDate);
            builder.Property(x => x.EndDate);
            builder.Property(x => x.SourceProductTypeId);
            builder.Property(x => x.DestinationProductTypeId);
            builder.Property(x => x.NetStandardVolume);
            builder.Property(x => x.GrossStandardVolume);
            builder.Property(x => x.Version).IsRequired(false);
            builder.Property(x => x.OfficialDeltaTicketId).IsRequired(false);
            builder.Property(x => x.TicketId).IsRequired(false);
            builder.Property(x => x.SourceSystemId).IsRequired(false);
        }
    }
}
