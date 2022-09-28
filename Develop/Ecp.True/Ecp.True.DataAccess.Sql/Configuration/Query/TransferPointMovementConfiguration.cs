// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransferPointMovementConfiguration.cs" company="Microsoft">
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
    /// The transfer point movement configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.Query.QueryEntityConfiguration{Ecp.True.Entities.Query.TransferPointMovement}" />
    public class TransferPointMovementConfiguration : QueryEntityConfiguration<TransferPointMovement>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<TransferPointMovement> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.AlgorithmId);
            builder.Property(x => x.DestinationNode);
            builder.Property(x => x.DestinationNodeId);
            builder.Property(x => x.DestinationNodeType);
            builder.Property(x => x.DestinationNodeTypeId);
            builder.Property(x => x.EndDate);
            builder.Property(x => x.MeasurementUnit);
            builder.Property(x => x.MovementId);
            builder.Property(x => x.MovementTransactionId);
            builder.Property(x => x.MovementType);
            builder.Property(x => x.MovementTypeId);
            builder.Property(x => x.NetVolume);
            builder.Property(x => x.OperationalDate);
            builder.Property(x => x.SourceNode);
            builder.Property(x => x.SourceNodeId);
            builder.Property(x => x.SourceNodeType);
            builder.Property(x => x.SourceNodeTypeId);
            builder.Property(x => x.SourceProduct);
            builder.Property(x => x.SourceProductId);
            builder.Property(x => x.SourceProductType);
            builder.Property(x => x.SourceProductTypeId);
            builder.Property(x => x.StartDate);
            builder.Property(x => x.TicketId);
        }
    }
}
