// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContractRelationships.cs" company="Microsoft">
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
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The contract relationships.
    /// </summary>
    public static class ContractRelationships
    {
        /// <summary>
        /// Configures the relationships for contract.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void Configure(EntityTypeBuilder<Contract> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.HasOne(t => t.DestinationNode)
                    .WithMany(p => p.DestinationContracts)
                    .HasForeignKey(d => d.DestinationNodeId);

            builder.HasOne(t => t.SourceNode)
                    .WithMany(p => p.SourceContracts)
                    .HasForeignKey(d => d.SourceNodeId);

            builder.HasOne(t => t.Product)
                    .WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.ProductId);

            builder.HasOne(t => t.MovementType)
                    .WithMany(p => p.ContractMovementTypes)
                    .HasForeignKey(d => d.MovementTypeId)
                    .IsRequired();

            builder.HasOne(t => t.Owner1)
                    .WithMany(p => p.Owner1Contracts)
                    .HasForeignKey(d => d.Owner1Id);

            builder.HasOne(t => t.MeasurementUnitDetail)
                    .WithMany(p => p.Units)
                    .HasForeignKey(d => d.MeasurementUnit)
                    .IsRequired();

            builder.HasOne(t => t.Owner2)
                    .WithMany(p => p.Owner2Contracts)
                    .HasForeignKey(d => d.Owner2Id);
        }
    }
}