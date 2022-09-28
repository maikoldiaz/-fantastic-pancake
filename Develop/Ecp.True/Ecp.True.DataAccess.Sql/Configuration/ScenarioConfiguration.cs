// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScenarioConfiguration.cs" company="Microsoft">
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
    using Ecp.True.Entities;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using SqlConstants = Ecp.True.DataAccess.Sql.Constants;

    /// <summary>
    /// The scenario configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{Ecp.True.Entities.Scenario}" />
    public class ScenarioConfiguration : EntityConfiguration<Scenario>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScenarioConfiguration"/> class.
        /// </summary>
        public ScenarioConfiguration()
        : base(x => x.ScenarioId, SqlConstants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<Scenario> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.Name).HasMaxLength(50);
            builder.Property(x => x.Sequence).IsRequired().HasMaxLength(10);
            builder.Ignore(x => x.LastModifiedBy);
            builder.Ignore(x => x.LastModifiedDate);
        }
    }
}
