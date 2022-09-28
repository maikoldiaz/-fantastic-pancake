// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipErrorConfiguration.cs" company="Microsoft">
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
    using Ecp.True.Entities.Admin;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>Ownership Calculation Errors Configuration.</summary>
    /// <seealso cref="QueryEntityConfiguration{Entities.Query.OwnershipError}" />
    public class OwnershipErrorConfiguration : QueryEntityConfiguration<OwnershipError>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<OwnershipError> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));
            builder.Property(x => x.OperationId);
            builder.Property(x => x.OwnershipNodeId);
            builder.Property(x => x.Type);
            builder.Property(x => x.Operation);
            builder.Property(x => x.OperationDate);
            builder.Property(x => x.ExecutionDate);
            builder.Property(x => x.Segment);
            builder.Property(x => x.NetVolume);
            builder.Property(x => x.ProductOrigin);
            builder.Property(x => x.NodeOrigin);
            builder.Property(x => x.NodeDestination);
            builder.Property(x => x.ProductDestination);
            builder.Property(x => x.ErrorMessage);
            builder.ToView(Sql.Constants.NodeCalculationErrorsView, Sql.Constants.AdminSchema);
        }
    }
}
