// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventRelationships.cs" company="Microsoft">
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
    /// The event relationships.
    /// </summary>
    public static class EventRelationships
    {
        /// <summary>
        /// Configures the relationships for event.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void Configure(EntityTypeBuilder<Event> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.HasOne(t => t.DestinationNode)
                    .WithMany(p => p.DestinationEvents)
                    .HasForeignKey(d => d.DestinationNodeId);

            builder.HasOne(t => t.SourceNode)
                    .WithMany(p => p.SourceEvents)
                    .HasForeignKey(d => d.SourceNodeId);

            builder.HasOne(t => t.SourceProduct)
                    .WithMany(p => p.SourceEvents)
                    .HasForeignKey(d => d.SourceProductId);

            builder.HasOne(t => t.DestinationProduct)
                    .WithMany(p => p.DestinationEvents)
                    .HasForeignKey(d => d.DestinationProductId);

            builder.HasOne(t => t.EventType)
                    .WithMany(p => p.EventTypes)
                    .HasForeignKey(d => d.EventTypeId);

            builder.HasOne(t => t.Owner1)
                    .WithMany(p => p.Owner1Events)
                    .HasForeignKey(d => d.Owner1Id);

            builder.HasOne(t => t.Owner2)
                    .WithMany(p => p.Owner2Events)
                    .HasForeignKey(d => d.Owner2Id);
        }
    }
}