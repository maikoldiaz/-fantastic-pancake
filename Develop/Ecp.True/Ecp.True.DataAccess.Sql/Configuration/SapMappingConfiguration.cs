// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapMappingConfiguration.cs" company="Microsoft">
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
    /// The sap Mapping configuration.
    /// </summary>
    public class SapMappingConfiguration : EntityConfiguration<SapMapping>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SapMappingConfiguration"/> class.
        /// </summary>
        public SapMappingConfiguration()
        : base(x => x.SapMappingId, Sql.Constants.AdminSchema, true)
        {
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(EntityTypeBuilder<SapMapping> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.SourceSystemId).IsRequired().HasColumnType("int");
            builder.Property(x => x.OfficialSystem).IsRequired().HasColumnType("int");
            builder.Property(x => x.SourceMovementTypeId).IsRequired().HasColumnType("int");
            builder.Property(x => x.SourceProductId).IsRequired().HasColumnType("nvarchar(20)");
            builder.Property(x => x.SourceSystemSourceNodeId).IsRequired().HasColumnType("int");
            builder.Property(x => x.SourceSystemDestinationNodeId).IsRequired().HasColumnType("int");
            builder.Property(x => x.DestinationSystemId).IsRequired().HasColumnType("int");
            builder.Property(x => x.DestinationMovementTypeId).IsRequired().HasColumnType("int");
            builder.Property(x => x.DestinationProductId).IsRequired().HasColumnType("nvarchar(20)");
            builder.Property(x => x.DestinationSystemSourceNodeId).IsRequired().HasColumnType("int");
            builder.Property(x => x.DestinationSystemDestinationNodeId).IsRequired().HasColumnType("int");
            builder.Ignore(x => x.LastModifiedBy);
            builder.Ignore(x => x.LastModifiedDate);
        }
    }
}
