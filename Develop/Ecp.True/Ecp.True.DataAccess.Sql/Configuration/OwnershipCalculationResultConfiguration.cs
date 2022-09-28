// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipCalculationResultConfiguration.cs" company="Microsoft">
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
    using Ecp.True.Entities.Admin;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The OwnershipCalculationResultConfiguration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{Ecp.True.Entities.Admin.OwnershipCalculationResult}" />
    public class OwnershipCalculationResultConfiguration : EntityConfiguration<OwnershipCalculationResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipCalculationResultConfiguration"/> class.
        /// </summary>
        public OwnershipCalculationResultConfiguration()
        : base(x => x.OwnershipCalculationResultId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<OwnershipCalculationResult> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));
            builder.Property(x => x.OwnershipCalculationId).HasColumnType("int").IsRequired();

            builder.HasOne(s => s.OwnershipCalculation).WithMany(p => p.OwnershipCalculationResults).HasForeignKey(d => d.OwnershipCalculationId);
            builder.HasOne(x => x.Owner)
                    .WithMany(x => x.OwnershipCalculationResults)
                    .HasForeignKey(d => d.OwnerId);
        }
    }
}
