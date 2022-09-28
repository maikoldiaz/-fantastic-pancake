// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipNodeErrorConfiguration.cs" company="Microsoft">
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
    /// The OwnershipNodeErrorConfiguration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{Ecp.True.Entities.Admin.OwnershipNodeError}" />
    public class OwnershipNodeErrorConfiguration : EntityConfiguration<OwnershipNodeError>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipNodeErrorConfiguration"/> class.
        /// </summary>
        public OwnershipNodeErrorConfiguration()
        : base(x => x.OwnershipNodeErrorId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<OwnershipNodeError> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.OwnershipNodeId).HasColumnType("int").IsRequired();
            builder.Property(x => x.ErrorMessage).HasMaxLength(1000).IsRequired();
            builder.Property(x => x.ExecutionDate).HasColumnType("datetime");

            OwnershipNodeErrorRelationships.Configure(builder);
        }
    }
}
