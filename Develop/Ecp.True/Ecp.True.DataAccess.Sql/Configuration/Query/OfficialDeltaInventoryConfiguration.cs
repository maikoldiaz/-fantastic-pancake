// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaInventoryConfiguration.cs" company="Microsoft">
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
    /// The OfficialDeltaInventoryConfiguration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.Query.QueryEntityConfiguration{OfficialDeltaInventoryConfiguration}" />
    [ExcludeFromCodeCoverage]
    public class OfficialDeltaInventoryConfiguration : QueryEntityConfiguration<OfficialDeltaInventory>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<OfficialDeltaInventory> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.MovementTransactionId);
            builder.Property(x => x.MovementOwnerId);
            builder.Property(x => x.OperationalDate);
            builder.Property(x => x.NodeId);
            builder.Property(x => x.ProductId);
            builder.Property(x => x.OwnerId);
            builder.Property(x => x.OwnershipVolume);
            builder.Property(x => x.SourceNodeId);
            builder.Property(x => x.SourceProductId);
            builder.Property(x => x.DestinationNodeId);
            builder.Property(x => x.DestinationProductId);
            builder.Property(x => x.MeasurementUnit);
        }
    }
}
