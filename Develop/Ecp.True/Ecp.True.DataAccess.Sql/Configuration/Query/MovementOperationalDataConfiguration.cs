// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementOperationalDataConfiguration.cs" company="Microsoft">
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
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The Movement operational data configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.Query.QueryEntityConfiguration{Ecp.True.Entities.Query.MovementOperationalData}" />
    public class MovementOperationalDataConfiguration : QueryEntityConfiguration<MovementOperationalData>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<MovementOperationalData> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.Ticket);
            builder.Property(x => x.MovementTransactionId);
            builder.Property(x => x.MovementId);
            builder.Property(x => x.OperationalDate);
            builder.Property(x => x.SourceNodeId);
            builder.Property(x => x.DestinationNodeId);
            builder.Property(x => x.SourceProductId);
            builder.Property(x => x.DestinationProductId);
            builder.Property(x => x.NetVolume);
            builder.Property(x => x.MovementTypeId);
            builder.Property(e => e.MessageTypeId).HasColumnType("int");
            builder.Property(x => x.OwnerId);
            builder.Property(x => x.OwnershipValue);
            builder.Property(x => x.OwnershipUnit);
            builder.Ignore(x => x.ResponseMovementTypeId);
        }
    }
}
