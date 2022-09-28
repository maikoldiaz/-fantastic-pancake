// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditLogConfiguration.cs" company="Microsoft">
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
    using Ecp.True.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using SqlConstants = Ecp.True.DataAccess.Sql.Constants;

    /// <summary>
    /// The audit log configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{Ecp.True.Entities.AuditLog}" />
    public class AuditLogConfiguration : EntityConfiguration<AuditLog>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditLogConfiguration"/> class.
        /// </summary>
        public AuditLogConfiguration()
        : base(x => x.AuditLogId, SqlConstants.AuditSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<AuditLog> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.LogDate).HasColumnType("datetime").IsRequired().HasMaxLength(150);
            builder.Property(x => x.LogType).HasMaxLength(10);
            builder.Property(x => x.User).IsRequired().HasMaxLength(260);
            builder.Property(x => x.Field).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Entity).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Identity).HasMaxLength(20);

            builder.Ignore(p => p.EntityEntry);
        }
    }
}
