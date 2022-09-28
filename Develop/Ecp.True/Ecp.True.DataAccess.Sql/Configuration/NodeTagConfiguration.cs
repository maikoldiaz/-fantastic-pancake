// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeTagConfiguration.cs" company="Microsoft">
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
    using EfCore.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The NodeCategory Group.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{EfCore.Models.NodeTag}" />
    public class NodeTagConfiguration : EntityConfiguration<NodeTag>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeTagConfiguration"/> class.
        /// </summary>
        public NodeTagConfiguration()
            : base(x => x.NodeTagId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<NodeTag> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(e => e.EndDate).HasColumnType("datetime");
            builder.Property(e => e.StartDate).HasColumnType("datetime");

            builder.HasOne(d => d.CategoryElement)
                    .WithMany(p => p.NodeTags)
                    .HasForeignKey(d => d.ElementId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.Node)
                    .WithMany(p => p.NodeTags)
                    .HasForeignKey(d => d.NodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
