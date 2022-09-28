// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeadletteredMessageConfiguration.cs" company="Microsoft">
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
    /// The Deadlettered Message Configuration.
    /// </summary>
    /// <seealso cref="IEntityTypeConfiguration{TEntity}" />
    public class DeadletteredMessageConfiguration : EntityConfiguration<DeadletteredMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeadletteredMessageConfiguration"/> class.
        /// </summary>
        public DeadletteredMessageConfiguration()
            : base(x => x.DeadletteredMessageId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<DeadletteredMessage> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.BlobPath).IsRequired();
            builder.Property(x => x.ProcessName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.QueueName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.ErrorMessage).HasColumnType("nvarchar(max)");
            builder.Property(x => x.IsSessionEnabled).HasColumnType("bit").IsRequired().HasDefaultValue(false);
            builder.Property(x => x.TicketId).IsRequired(false);
            builder.Ignore(x => x.Content);
        }
    }
}
