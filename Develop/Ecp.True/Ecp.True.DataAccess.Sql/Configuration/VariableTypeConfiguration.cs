// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VariableTypeConfiguration.cs" company="Microsoft">
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
    /// The Variable Type Configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{Ecp.True.Entities.Admin.VariableTypeEntity}" />
    public class VariableTypeConfiguration : EntityConfiguration<VariableTypeEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VariableTypeConfiguration"/> class.
        /// </summary>
        public VariableTypeConfiguration()
            : base(x => x.VariableTypeId, Sql.Constants.AdminSchema, true, "VariableType")
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<VariableTypeEntity> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Ignore(x => x.LastModifiedBy);
            builder.Ignore(x => x.LastModifiedDate);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.ShortName).IsRequired().HasMaxLength(10);
            builder.Property(x => x.FicoName).IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.IsConfigurable).IsRequired(false).HasColumnType("bit");
        }
    }
}
