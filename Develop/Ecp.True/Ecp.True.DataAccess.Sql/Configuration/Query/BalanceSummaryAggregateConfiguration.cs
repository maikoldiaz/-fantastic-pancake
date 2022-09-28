// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BalanceSummaryAggregateConfiguration.cs" company="Microsoft">
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
    using Ecp.True.Entities.Admin;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The database validation configuration class.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.Query.QueryEntityConfiguration{Ecp.True.Entities.Admin.BalanceSummaryAggregate}" />
    public class BalanceSummaryAggregateConfiguration : QueryEntityConfiguration<BalanceSummaryAggregate>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<BalanceSummaryAggregate> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.InitialInventory).HasColumnType("decimal");
            builder.Property(x => x.Inputs).HasColumnType("decimal");
            builder.Property(x => x.Outputs).HasColumnType("decimal");
            builder.Property(x => x.IdentifiedLosses).HasColumnType("decimal");
            builder.Property(x => x.Interface).HasColumnType("decimal");
            builder.Property(x => x.Tolerance).HasColumnType("decimal");
            builder.Property(x => x.UnidentifiedLosses).HasColumnType("decimal");
            builder.Property(x => x.FinalInventory).HasColumnType("decimal");
            builder.Property(x => x.Volume).HasColumnType("decimal");
            builder.Property(x => x.MeasurementUnit);
            builder.Property(x => x.Control);
            builder.Property(x => x.OwnershipStatusId);
        }
    }
}
