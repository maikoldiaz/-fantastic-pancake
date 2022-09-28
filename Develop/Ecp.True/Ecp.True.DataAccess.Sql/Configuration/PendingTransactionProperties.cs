// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PendingTransactionProperties.cs" company="Microsoft">
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
    /// The unbalance comment relationships.
    /// </summary>
    public static class PendingTransactionProperties
    {
        /// <summary>
        /// Configures the relationships for unbalance comment.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void Configure(EntityTypeBuilder<PendingTransaction> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(e => e.SystemTypeId).HasColumnType("int");
            builder.Property(e => e.MessageTypeId).HasColumnType("int").IsRequired(false);
            builder.Property(x => x.ActionTypeId).HasColumnType("int");
            builder.Property(e => e.Identifier).IsRequired(false).HasMaxLength(50);

            builder.Property(e => e.SourceNode).HasColumnName("SourceNodeId").HasMaxLength(100);
            builder.Property(e => e.DestinationNode).HasColumnName("DestinationNodeId").HasMaxLength(100);
            builder.Property(e => e.SourceProduct).HasColumnName("SourceProductId").HasMaxLength(100);
            builder.Property(e => e.DestinationProduct).HasColumnName("DestinationProductId").HasMaxLength(100);

            builder.Property(e => e.Volume).HasMaxLength(50);
            builder.Property(e => e.Units).HasColumnType("int").IsRequired(false);

            builder.Property(e => e.EndDate).HasColumnType("datetime");
            builder.Property(e => e.StartDate).HasColumnType("datetime");
        }
    }
}
