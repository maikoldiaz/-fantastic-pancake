// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapLogisticMovementDetailConfiguration.cs" company="Microsoft">
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
    using Ecp.True.Entities.Sap;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The sap logistic movement detail configuration.
    /// </summary>
    public class SapLogisticMovementDetailConfiguration : QueryEntityConfiguration<SapLogisticMovementDetail>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<SapLogisticMovementDetail> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));
            builder.Property(x => x.MovementTransactionId);
            builder.Property(x => x.State);
            builder.Property(x => x.Description);
            builder.Property(x => x.MovementType);
            builder.Property(x => x.SourceCenter);
            builder.Property(x => x.SourceStorage);
            builder.Property(x => x.SourceProduct);
            builder.Property(x => x.DestinationCenter);
            builder.Property(x => x.DestinationStorage);
            builder.Property(x => x.DestinationProduct);
            builder.Property(x => x.OwnershipVolume);
            builder.Property(x => x.Units);
            builder.Property(x => x.OperationalDate);
            builder.Property(x => x.MovementId);
            builder.Property(x => x.CostCenter);
            builder.Property(x => x.GmCode);
            builder.Property(x => x.DocumentNumber);
            builder.Property(x => x.Position);
            builder.Property(x => x.Order);
            builder.Property(x => x.AccountingDate);
            builder.Property(x => x.Segment);
            builder.Property(x => x.Scenario);
            builder.Property(x => x.Owner);
            builder.Property(x => x.LogisticMovementId);
        }
    }
}
