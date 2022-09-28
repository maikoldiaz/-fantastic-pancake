// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialTransferPointMovementConfiguration.cs" company="Microsoft">
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
    /// The Official Transfer PointMovement Configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.Query.QueryEntityConfiguration{Ecp.True.Entities.Query.OfficialTransferPointMovement}" />
    public class OfficialTransferPointMovementConfiguration : QueryEntityConfiguration<OfficialTransferPointMovement>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<OfficialTransferPointMovement> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.DestinationNodeName);
            builder.Property(x => x.SourceNodeName);
            builder.Property(x => x.MeasurementUnit);
            builder.Property(x => x.MovementId);
            builder.Property(x => x.MovementTypeName);
            builder.Property(x => x.NetStandardVolume);
            builder.Property(x => x.OperationalDate);
            builder.Property(x => x.SourceProductName);
            builder.Property(x => x.DestinationProductName);
            builder.Property(x => x.SapTrackingId);
            builder.Property(x => x.ErrorCount);
            builder.Property(x => x.ErrorMessage);
        }
    }
}
