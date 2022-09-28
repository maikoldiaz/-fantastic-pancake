// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOwnershipValidator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core.Entities;
    using Ecp.True.Proxies.OwnershipRules.Response;

    /// <summary>
    /// The Ownership Validator.
    /// </summary>
    public interface IOwnershipValidator
    {
        /// <summary>
        /// Validates ownership rule error asynchronous.
        /// </summary>
        /// <param name="inventoryList">List of inventories.</param>
        /// <param name="movementList">List of movements.</param>
        /// <returns>The task.</returns>
        Task<IEnumerable<ErrorInfo>> ValidateOwnershipRuleErrorAsync(IEnumerable<OwnershipErrorInventory> inventoryList, IEnumerable<OwnershipErrorMovement> movementList);

        /// <summary>
        /// Checks the ticket exists asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>Return the ticket.</returns>
        Task<(ICollection<ErrorInfo>, bool)> CheckTicketExistsAsync(int ticketId);
    }
}
