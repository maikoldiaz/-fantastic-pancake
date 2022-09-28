// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TicketEntityConfiguration.cs" company="Microsoft">
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
    using Ecp.True.Entities.TransportBalance;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// Ticket Entity Configuration.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.Query.QueryEntityConfiguration{Ecp.True.Entities.TransportBalance.TicketEntity}" />
    public class TicketEntityConfiguration : QueryEntityConfiguration<TicketEntity>
    {
        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void DoConfigure(QueryTypeBuilder<TicketEntity> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));
            builder.Property(x => x.TicketId);
            builder.Property(x => x.TicketTypeId);
            builder.Property(x => x.Segment);
            builder.Property(x => x.CategoryName);
            builder.Property(x => x.TicketStartDate).HasColumnType("datetime");
            builder.Property(x => x.TicketFinalDate).HasColumnType("datetime");
            builder.Property(x => x.CutoffExecutionDate).HasColumnType("datetime");
            builder.Property(x => x.CreatedBy);
            builder.Property(x => x.State);
            builder.Property(x => x.OwnerName);
            builder.Property(x => x.ErrorMessage);
            builder.Property(x => x.BlobPath);
            builder.Property(x => x.NodeName);
            builder.Property(x => x.ScenarioName);
            builder.ToView(Sql.Constants.TicketsView, Sql.Constants.AdminSchema);
        }
    }
}
