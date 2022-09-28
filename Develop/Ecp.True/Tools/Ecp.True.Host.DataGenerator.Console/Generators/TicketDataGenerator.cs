// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TicketDataGenerator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.DataGenerator.Console.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.DataGenerator.Console.Interfaces;

    /// <summary>
    /// The TicketDataGenerator.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.DataGenerator.Console.Interfaces.IDataGenerator" />
    public class TicketDataGenerator : DataGeneratorBase, IDataGenerator
    {
        /// <summary>
        /// The save ticket repository.
        /// </summary>
        private readonly IRepository<SaveTicketResult> saveTicketRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketDataGenerator"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public TicketDataGenerator(IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            this.saveTicketRepository = unitOfWork.CreateRepository<SaveTicketResult>();
        }

        /// <summary>
        /// Generates the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The identity.
        /// </returns>
        public async Task<int> GenerateAsync(IDictionary<string, object> parameters)
        {
            ArgumentValidators.ThrowIfNull(parameters, nameof(parameters));
            var ticket = GetTicket(parameters);

            var ticketParameters = new Dictionary<string, object>
            {
                { "@CategoryElementId", ticket.CategoryElementId },
                { "@StartDate", ticket.StartDate },
                { "@EndDate", ticket.EndDate },
                { "@UserId", "System" },
                { "@TicketTypeId", TicketType.Logistics },
                { "@FirstTimeNodes", new List<int>().ToDataTable(Repositories.Constants.NodeIdColumnName, Repositories.Constants.NodeListType) },
                { "@UncertaintyPercentage", DBNull.Value },
                { "@OwnerId", DBNull.Value },
                { "@NodeId", DBNull.Value },
                { "@TicketGroupId", DBNull.Value },
                { "@SessionId", DBNull.Value },
            };

            var tickets = await this.saveTicketRepository.ExecuteQueryAsync(Repositories.Constants.SaveTicketProcedureName, ticketParameters).ConfigureAwait(false);
            return tickets.FirstOrDefault().TicketId;
        }

        /// <summary>
        /// Gets the ticket.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The Ticket.</returns>
        private static Ticket GetTicket(IDictionary<string, object> parameters)
        {
            var ticket = new Ticket
            {
                CategoryElementId = GetInt(parameters, "CategoryElementId", 156),
                StartDate = GetDate(parameters, "StartDate", DateTime.UtcNow.ToTrue()),
                EndDate = GetDate(parameters, "EndDate", DateTime.UtcNow.ToTrue()),
                Status = parameters.TryGetValue("Status", out object status) ? (StatusType)Enum.Parse(typeof(StatusType), status.ToString()) : StatusType.PROCESSING,
                TicketTypeId = parameters.TryGetValue("TicketTypeId", out object ticketTypeId) ? (TicketType)Enum.Parse(typeof(TicketType), ticketTypeId.ToString()) : TicketType.Cutoff,
            };

            return ticket;
        }
    }
}
