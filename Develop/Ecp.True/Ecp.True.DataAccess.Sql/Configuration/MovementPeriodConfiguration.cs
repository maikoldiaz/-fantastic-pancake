// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementPeriodConfiguration.cs" company="Microsoft">
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
    /// The Movement Period Configuration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{Ecp.True.Entities.Admin.MovementPeriod}" />
    public class MovementPeriodConfiguration : EntityConfiguration<MovementPeriod>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovementPeriodConfiguration"/> class.
        /// </summary>
        public MovementPeriodConfiguration()
            : base(x => x.MovementPeriodId, Sql.Constants.OffchainSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<MovementPeriod> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));
            builder.Property(x => x.StartTime).HasColumnType("datetime").IsRequired();
            builder.Property(x => x.EndTime).HasColumnType("datetime").IsRequired();
            builder.HasOne(x => x.MovementTransaction)
                    .WithOne(x => x.Period)
                    .HasForeignKey<MovementPeriod>(x => x.MovementTransactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
