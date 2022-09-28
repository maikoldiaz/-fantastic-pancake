// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnbalanceCommentConfiguration.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Sql.Configuration
{
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The Unbalance Configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{UnbalanceComment}" />
    public class UnbalanceCommentConfiguration : EntityConfiguration<UnbalanceComment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnbalanceCommentConfiguration"/> class.
        /// </summary>
        public UnbalanceCommentConfiguration()
            : base(x => x.UnbalanceId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<UnbalanceComment> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.CalculationDate).HasColumnType("datetime").IsRequired();
            builder.Ignore(x => x.Inputs);
            builder.Ignore(x => x.InitialInventory);
            builder.Ignore(x => x.Outputs);
            builder.Ignore(x => x.FinalInventory);
            builder.Ignore(x => x.IdentifiedLosses);
            builder.Ignore(x => x.NodeName);
            builder.Ignore(x => x.ProductName);
            builder.Ignore(x => x.UnitName);
            builder.Ignore(x => x.UnbalancePercentageText);

            UnbalanceCommentProperties.Configure(builder);
            UnbalanceCommentRelationships.Configure(builder);
        }
    }
}
