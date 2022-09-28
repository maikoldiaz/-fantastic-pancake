// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnbalanceCommentProperties.cs" company="Microsoft">
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
    /// The unbalance comment relationships.
    /// </summary>
    public static class UnbalanceCommentProperties
    {
        /// <summary>
        /// Configures the relationships for unbalance comment.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void Configure(EntityTypeBuilder<UnbalanceComment> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.ProductId).HasMaxLength(20).IsRequired();
            builder.Property(x => x.Unbalance).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.Units).HasMaxLength(50).IsRequired();
            builder.Property(x => x.UnbalancePercentage).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.ControlLimit).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.Status).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.SessionId).IsRequired(false).HasColumnType("nvarchar").HasMaxLength(50);
        }
    }
}
