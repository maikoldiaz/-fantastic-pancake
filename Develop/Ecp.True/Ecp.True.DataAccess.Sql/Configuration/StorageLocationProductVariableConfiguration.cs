// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageLocationProductVariableConfiguration.cs" company="Microsoft">
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
    /// The Storage Location Product Variable Configuration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{Ecp.True.Entities.Admin.StorageLocationProductVariable}" />
    public class StorageLocationProductVariableConfiguration : EntityConfiguration<StorageLocationProductVariable>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageLocationProductVariableConfiguration"/> class.
        /// </summary>
        public StorageLocationProductVariableConfiguration()
            : base(x => x.StorageLocationProductVariableId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<StorageLocationProductVariable> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.HasOne(t => t.StorageLocationProduct)
                .WithMany(t => t.StorageLocationProductVariables)
                .HasForeignKey(t => t.StorageLocationProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.VariableType)
                .WithMany(t => t.StorageLocationProductVariables)
                .HasForeignKey(t => t.VariableTypeId);
        }
    }
}
