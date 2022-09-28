// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAnalyticalOwnershipCalculationService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Processors.Ownership.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Query;

    /// <summary>
    /// The Analytical calculation service.
    /// </summary>
    public interface IAnalyticalOwnershipCalculationService
    {
        /// <summary>
        /// Gets the ownership analytical data asynchronous.
        /// </summary>
        /// <param name="transferPointMovements">The transfer point movements.</param>
        /// <returns>List of Ownership Analytics.</returns>
        Task<IEnumerable<OwnershipAnalytics>> GetOwnershipAnalyticalDataAsync(IEnumerable<TransferPointMovement> transferPointMovements);

        /// <summary>
        /// Gets the transfer point movements asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>List of transfer point movements.</returns>
        Task<IEnumerable<TransferPointMovement>> GetTransferPointMovementsAsync(int ticketId);
    }
}
