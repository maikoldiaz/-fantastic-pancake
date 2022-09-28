// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPendingTransactionErrorRepository.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// Pending Transaction error Interface.
    /// </summary>
    public interface IPendingTransactionErrorRepository : IRepository<PendingTransactionError>
    {
        /// <summary>
        /// Gets the cut off message exceptions.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>
        /// Returns the cutoff message exceptions.
        /// </returns>
        Task<IEnumerable<PendingTransactionErrorDto>> GetPendingTransactionErrorsAsync(Ticket ticket);
    }
}
