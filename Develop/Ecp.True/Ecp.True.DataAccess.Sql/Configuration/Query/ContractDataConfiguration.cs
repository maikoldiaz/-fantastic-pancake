// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContractDataConfiguration.cs" company="Microsoft">
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
    using Ecp.True.Entities.Query;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The Node configuration data configuration.
    /// </summary>
    public class ContractDataConfiguration : QueryEntityConfiguration<Contract>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<Contract> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.ContractId);
            builder.Property(x => x.ContractUnit);
            builder.Property(x => x.ContractValue);
            builder.Property(x => x.BuyerOwnerId);
            builder.Property(x => x.SellerOwnerId);
            builder.Property(x => x.SourceNodeId);
            builder.Property(x => x.DestinationNodeId);
            builder.Property(x => x.ProductId);
            builder.Ignore(x => x.ResponseContractUnit);
            builder.Ignore(x => x.IsFinal);
        }
    }
}
