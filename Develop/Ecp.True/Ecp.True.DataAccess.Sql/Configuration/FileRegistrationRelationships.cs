// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileRegistrationRelationships.cs" company="Microsoft">
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
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The file registration relationships.
    /// </summary>
    public static class FileRegistrationRelationships
    {
        /// <summary>
        /// Configures the relationships for file registration.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void Configure(EntityTypeBuilder<FileRegistration> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.HasOne(x => x.Segment)
                    .WithMany(x => x.FileRegistrations)
                    .HasForeignKey(y => y.SegmentId)
                    .IsRequired(false);
        }
    }
}