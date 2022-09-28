// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipNodeMovementInventoryDetailsConfiguration.cs" company="Microsoft">
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
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.Query.QueryEntityConfiguration{Ecp.True.Entities.Admin.OwnershipNodeMovementInventoryDetails}" />
    public class OwnershipNodeMovementInventoryDetailsConfiguration : QueryEntityConfiguration<OwnershipNodeMovementInventoryDetails>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<OwnershipNodeMovementInventoryDetails> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.MovementType);
            builder.Property(x => x.OperationalDate).HasColumnType("datetime");
            builder.Property(x => x.TankName);
            builder.Property(x => x.SourceNode);
            builder.Property(x => x.DestinationNode);
            builder.Property(x => x.SourceProduct);
            builder.Property(x => x.DestinationProduct);
            builder.Property(x => x.NetVolume).HasColumnType("decimal").IsRequired(false);
            builder.Property(x => x.Unit);
            builder.Property(x => x.UnitId).HasColumnType("int");
            builder.Property(x => x.OwnershipVolume).HasColumnType("decimal").IsRequired(false);
            builder.Property(x => x.OwnershipPercentage).HasColumnType("decimal").IsRequired(false);
            builder.Property(x => x.TransactionId).HasColumnType("int");
            builder.Property(x => x.OwnershipFunction);
            builder.Property(x => x.SourceProductId);
            builder.Property(x => x.DestinationProductId);
            builder.Property(x => x.SourceNodeId).HasColumnType("int");
            builder.Property(x => x.DestinationNodeId).HasColumnType("int").IsRequired(false);
            builder.Property(x => x.OwnerId).HasColumnType("int");
            builder.Property(x => x.OwnerName);
            builder.Property(x => x.ReasonId).HasColumnType("int").IsRequired(false);
            builder.Property(x => x.Reason);
            builder.Property(x => x.Comment);
            builder.Property(x => x.IsMovement).HasColumnType("int");
            builder.Property(x => x.VariableTypeId).HasColumnType("int");
            builder.Property(x => x.Color);
            builder.Property(x => x.ContractId).HasColumnType("int").IsRequired(false);
            builder.Property(x => x.MovementContractId).HasColumnType("int").IsRequired(false);
            builder.Property(x => x.DocumentNumber).HasColumnType("int").IsRequired(false);
            builder.Property(x => x.Position).HasColumnType("int").IsRequired(false);
            builder.Property(x => x.MovementId);
            builder.Property(x => x.SourceMovementId);
            builder.Property(x => x.MovementTypeId).HasColumnType("int");
        }
    }
}
