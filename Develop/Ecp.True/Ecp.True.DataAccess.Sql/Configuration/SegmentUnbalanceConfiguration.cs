// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SegmentUnbalanceConfiguration.cs" company="Microsoft">
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
    /// The SegmentUnbalanceConfiguration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{Ecp.True.Entities.Admin.SegmentUnbalance}" />
    public class SegmentUnbalanceConfiguration : EntityConfiguration<SegmentUnbalance>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentUnbalanceConfiguration"/> class.
        /// </summary>
        public SegmentUnbalanceConfiguration()
        : base(x => x.SegmentUnbalanceId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<SegmentUnbalance> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));
            builder.Property(x => x.SegmentId).HasColumnType("int").IsRequired();
            builder.Property(x => x.ProductId).HasMaxLength(20).IsRequired();
            builder.Property(x => x.Date).HasColumnType("datetime").IsRequired();
            builder.Property(x => x.TicketId).HasColumnType("int").IsRequired(false);
            SegmentUnbalanceProperties.Configure(builder);
            SegmentUnbalanceRelationships.Configure(builder);
        }
    }
}
