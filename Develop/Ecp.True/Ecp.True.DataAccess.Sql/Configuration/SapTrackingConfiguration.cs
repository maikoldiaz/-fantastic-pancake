// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapTrackingConfiguration.cs" company="Microsoft">
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
    /// The sap tracking configuration.
    /// </summary>
    public class SapTrackingConfiguration : EntityConfiguration<SapTracking>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SapTrackingConfiguration"/> class.
        /// </summary>
        public SapTrackingConfiguration()
        : base(x => x.SapTrackingId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<SapTracking> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.MovementTransactionId).IsRequired(false).HasColumnType("int");
            builder.Property(x => x.FileRegistrationId).IsRequired(false).HasColumnType("int");
            builder.Property(x => x.OperationalDate).IsRequired(false).HasColumnType("datetime");
            builder.Property(x => x.ErrorMessage).IsRequired(false).HasColumnType("nvarchar(max)");
            builder.Property(x => x.SessionId).IsRequired(false).HasColumnType("nvarchar").HasMaxLength(50);
            builder.Property(x => x.Comment).IsRequired(false).HasColumnType("nvarchar").HasMaxLength(1000);
            builder.HasOne(s => s.Movement).WithMany(p => p.SapTracking).HasForeignKey(d => d.MovementTransactionId);
            builder.HasOne(s => s.FileRegistration).WithMany(p => p.SapTracking).HasForeignKey(d => d.FileRegistrationId);
            builder.Property(x => x.BlobPath).IsRequired(false).HasColumnType("nvarchar").HasMaxLength(256);
        }
    }
}
