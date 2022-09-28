// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PendingOfficialInventoryConfiguration.cs" company="Microsoft">
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
    /// The PendingOfficialInventoryConfiguration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.Query.QueryEntityConfiguration{PendingOfficialInventoryConfiguration}" />
    public class PendingOfficialInventoryConfiguration : QueryEntityConfiguration<PendingOfficialInventory>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<PendingOfficialInventory> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.InventoryProductID);
            builder.Property(x => x.InventoryProductUniqueId);
            builder.Property(x => x.InventoryDate);
            builder.Property(x => x.NodeId);
            builder.Property(x => x.ProductID);
            builder.Property(x => x.OwnerId);
            builder.Property(x => x.OwnerShipValue);
            builder.Property(x => x.MeasurementUnit);
            builder.Property(x => x.SegmentId);
        }
    }
}
