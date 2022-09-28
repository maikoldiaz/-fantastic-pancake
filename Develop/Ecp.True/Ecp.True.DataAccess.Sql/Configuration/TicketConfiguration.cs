// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TicketConfiguration.cs" company="Microsoft">
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
    using Ecp.True.Entities.TransportBalance;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The ticket configuration class.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{Ecp.True.Entities.TransportBalance.Ticket}" />
    public class TicketConfiguration : EntityConfiguration<Ticket>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TicketConfiguration"/> class.
        /// </summary>
        public TicketConfiguration()
        : base(x => x.TicketId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<Ticket> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));
            ConfigureColumnMappings(builder);
            builder.HasOne(d => d.CategoryElement)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.CategoryElementId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(d => d.Owner)
                    .WithMany(p => p.TicketOwners)
                    .HasForeignKey(d => d.OwnerId);
            builder.HasOne(d => d.Node)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.TicketId);
        }

        /// <summary>
        /// Configures the column mappings.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private static void ConfigureColumnMappings(EntityTypeBuilder<Ticket> builder)
        {
            builder.Property(x => x.StartDate).HasColumnType("datetime").IsRequired();
            builder.Property(x => x.EndDate).HasColumnType("datetime").IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.ErrorMessage).HasColumnType("nvarchar(max)");
            builder.Property(x => x.TicketTypeId);
            builder.Property(x => x.BlobPath).HasColumnType("nvarchar(max)");
            builder.Property(x => x.OwnerId).HasColumnType("int").IsRequired(false);
            builder.Property(x => x.NodeId).HasColumnType("int").IsRequired(false);
            builder.Property(x => x.TicketGroupId).HasColumnType("nvarchar(255)").IsRequired(false);
            builder.Property(x => x.ScenarioTypeId);
        }
    }
}