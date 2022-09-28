// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContractConfiguration.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Sql.Configuration
{
    using Ecp.True.Core;
    using Ecp.True.Entities.Registration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The Event Object Configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{Contract}" />
    public class ContractConfiguration : EntityConfiguration<Contract>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractConfiguration"/> class.
        /// </summary>
        public ContractConfiguration()
            : base(x => x.ContractId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<Contract> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.ProductId).IsRequired().HasMaxLength(40);
            builder.Property(x => x.StartDate).HasColumnType("datetime");
            builder.Property(x => x.EndDate).HasColumnType("datetime");

            builder.Property(x => x.Owner1Id).IsRequired(false);
            builder.Property(x => x.Owner2Id).IsRequired(false);

            builder.Property(x => x.Volume).HasColumnType("decimal(18,2)");

            builder.Property(x => x.SourceSystem).IsRequired(false);
            builder.Property(x => x.DateOrder).IsRequired(false);
            builder.Property(x => x.DateReceivedPo).IsRequired(false);
            builder.Property(x => x.MessageId).IsRequired(false);
            builder.Property(x => x.PurchaseOrderType).IsRequired(false);
            builder.Property(x => x.ExpeditionClass).IsRequired(false);
            builder.Property(x => x.Status).IsRequired(false);
            builder.Property(x => x.StatusCredit).IsRequired(false);
            builder.Property(x => x.DescriptionStatus).IsRequired(false);
            builder.Property(x => x.PositionStatus).IsRequired(false);
            builder.Property(x => x.Frequency).IsRequired(false);
            builder.Property(x => x.EstimatedVolume).IsRequired(false).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Tolerance).IsRequired(false).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Value).IsRequired(false).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Property).IsRequired(false);
            builder.Property(x => x.Uom).IsRequired(false);
            builder.Ignore(x => x.ActionType);
            builder.Property(x => x.FileRegistrationTransactionId).IsRequired(false);
            builder.Property(x => x.DocumentNumber);
            builder.Property(x => x.Position);
            builder.Property(x => x.Position);
            builder.Property(x => x.SourceNodeId).IsRequired(false);
            builder.Property(x => x.DestinationNodeId).IsRequired(false);
            builder.Property(x => x.MovementTypeId);
            builder.Property(x => x.MeasurementUnit);
            builder.Property(x => x.EventType).IsRequired(false);
            builder.Property(x => x.DestinationStorageLocationId).IsRequired(false);
            builder.Property(x => x.Batch).IsRequired(false);
            builder.Property(x => x.IsDeleted).IsRequired(false);
            builder.Property(x => x.OriginMessageId);

            ContractRelationships.Configure(builder);
        }
    }
}
