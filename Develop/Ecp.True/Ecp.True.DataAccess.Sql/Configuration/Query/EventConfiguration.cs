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

namespace Ecp.True.DataAccess.Sql.Configuration.Query
{
    using Ecp.True.Core;
    using Ecp.True.Entities.Query;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The Event data configuration.
    /// </summary>
    public class EventConfiguration : QueryEntityConfiguration<Event>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<Event> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.EventIdentifier);
            builder.Property(x => x.SourceNodeId);
            builder.Property(x => x.DestinationNodeId);
            builder.Property(x => x.SourceProductId);
            builder.Property(x => x.DestinationProductId);
            builder.Property(x => x.MeasurementUnit);
            builder.Property(x => x.OwnerId1);
            builder.Property(x => x.OwnerId2);
            builder.Property(x => x.OwnershipValue);
            builder.Property(x => x.IsAgreement);
            builder.Ignore(x => x.IsFinal);
        }
    }
}
