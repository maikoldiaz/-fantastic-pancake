// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogisticsMovementDetailConfiguration.cs" company="Microsoft">
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
    /// The LogisticsDetailConfiguration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.Query.QueryEntityConfiguration{Ecp.True.Entities.Query.LogisticsMovementDetail}" />
    public class LogisticsMovementDetailConfiguration : QueryEntityConfiguration<LogisticsMovementDetail>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<LogisticsMovementDetail> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.Movement);
            builder.Property(x => x.StorageSource);
            builder.Property(x => x.ProductOrigin);
            builder.Property(x => x.StorageDestination);
            builder.Property(x => x.ProductDestination);
            builder.Property(x => x.OrderPurchase);
            builder.Property(x => x.PosPurchase);
            builder.Property(x => x.Value);
            builder.Property(x => x.Uom);
            builder.Property(x => x.Finding);
            builder.Property(x => x.Diagnostic);
            builder.Property(x => x.Impact);
            builder.Property(x => x.Solution);
            builder.Property(x => x.Status);
            builder.Property(x => x.Order);
            builder.Property(x => x.DateOperation);
        }
    }
}
