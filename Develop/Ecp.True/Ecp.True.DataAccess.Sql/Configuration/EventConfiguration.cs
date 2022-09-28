// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventConfiguration.cs" company="Microsoft">
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
    /// The Event Object Configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{Event}" />
    public class EventConfiguration : EntityConfiguration<Event>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventConfiguration"/> class.
        /// </summary>
        public EventConfiguration()
            : base(x => x.EventId, Sql.Constants.AdminSchema, true, "Event")
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<Event> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.DestinationNodeId).IsRequired().HasMaxLength(20);
            builder.Property(x => x.SourceProductId).IsRequired().HasMaxLength(20);
            builder.Property(x => x.DestinationProductId).IsRequired().HasMaxLength(20);
            builder.Property(x => x.StartDate).HasColumnType("datetime");
            builder.Property(x => x.EndDate).HasColumnType("datetime");
            builder.Property(x => x.Owner1Id);
            builder.Property(x => x.Owner2Id);
            builder.Property(x => x.Volume).HasColumnType("decimal(18,2)");
            builder.Property(x => x.MeasurementUnit).IsRequired().HasMaxLength(50);

            builder.Ignore(x => x.ActionType);
            builder.Ignore(x => x.FileRegistrationTransactionId);

            EventRelationships.Configure(builder);
        }
    }
}
