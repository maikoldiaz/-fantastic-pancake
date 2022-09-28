// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementEventConfiguration.cs" company="Microsoft">
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
    /// The Movement Event Object Configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{MovementEvent}" />
    public class MovementEventConfiguration : EntityConfiguration<MovementEvent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovementEventConfiguration"/> class.
        /// </summary>
        public MovementEventConfiguration()
            : base(x => x.MovementEventId, Sql.Constants.AdminSchema, true, "MovementEvent")
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<MovementEvent> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));
            builder.Property(x => x.MovementEventId).HasColumnType("int").IsRequired();
            builder.Property(x => x.EventTypeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.SourceNodeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.DestinationNodeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.SourceProductId).HasColumnType("nvarchar").IsRequired().HasMaxLength(20);
            builder.Property(x => x.DestinationProductId).HasColumnType("nvarchar").IsRequired().HasMaxLength(20);
            builder.Property(x => x.StartDate).HasColumnType("datetime").IsRequired();
            builder.Property(x => x.EndDate).HasColumnType("datetime").IsRequired();
            builder.Property(x => x.Owner1Id).HasColumnType("int").IsRequired();
            builder.Property(x => x.Owner2Id).HasColumnType("int").IsRequired();
            builder.Property(x => x.Volume).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.MeasurementUnit).HasColumnType("nvarchar(50)").IsRequired();
            ConfigureRelationships(builder);
        }

        /// <summary>
        /// Configures the relationships.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private static void ConfigureRelationships(EntityTypeBuilder<MovementEvent> builder)
        {
            builder.HasOne(t => t.DestinationNode)
                    .WithMany(p => p.DestinationMovementEvents)
                    .HasForeignKey(d => d.DestinationNodeId)
                    .IsRequired();

            builder.HasOne(t => t.SourceNode)
                    .WithMany(p => p.SourceMovementEvents)
                    .HasForeignKey(d => d.SourceNodeId)
                    .IsRequired();

            builder.HasOne(t => t.SourceProduct)
                    .WithMany(p => p.SourceMovementEvents)
                    .HasForeignKey(d => d.SourceProductId)
                    .IsRequired();

            builder.HasOne(t => t.DestinationProduct)
                    .WithMany(p => p.DestinationMovementEvents)
                    .HasForeignKey(d => d.DestinationProductId)
                    .IsRequired();

            builder.HasOne(t => t.EventType)
                    .WithMany(p => p.MovementEventTypes)
                    .HasForeignKey(d => d.EventTypeId)
                    .IsRequired();

            builder.HasOne(t => t.Owner1)
                    .WithMany(p => p.Owner1MovementEvents)
                    .HasForeignKey(d => d.Owner1Id)
                    .IsRequired();

            builder.HasOne(t => t.Owner2)
                    .WithMany(p => p.Owner2MovementEvents)
                    .HasForeignKey(d => d.Owner2Id)
                    .IsRequired();
        }
    }
}
