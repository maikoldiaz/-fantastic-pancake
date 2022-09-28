// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaExceptionsConfiguration.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Sql.Configuration.Query
{
    using Ecp.True.Core;
    using Ecp.True.Entities.Dto;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The logistics ticket validation result configuration class.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.Query.QueryEntityConfiguration{Ecp.True.Entities.Dto.DeltaExceptions}" />
    public class DeltaExceptionsConfiguration : QueryEntityConfiguration<DeltaExceptions>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<DeltaExceptions> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.Date);
            builder.Property(x => x.DestinationNode);
            builder.Property(x => x.DestinationProduct);
            builder.Property(x => x.Error);
            builder.Property(x => x.Unit);
            builder.Property(x => x.Identifier);
            builder.Property(x => x.Quantity);
            builder.Property(x => x.SourceNode);
            builder.Property(x => x.SourceProduct);
            builder.Property(x => x.Type);
        }
    }
}
