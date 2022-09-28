// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPendingTransactionErrorProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Core.Interfaces;

    /// <summary>
    /// The volumetric processor.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.Interfaces.IProcessor" />
    public interface IPendingTransactionErrorProcessor : IProcessor
    {
        /// <summary>
        /// Gets the cut off message exceptions.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>
        /// Returns the cut off message exceptions.
        /// </returns>
        Task<IEnumerable<PendingTransactionErrorDto>> GetPendingTransactionsAsync(Ticket ticket);

        /// <summary>
        /// Saves the comments asynchronous.
        /// </summary>
        /// <param name="errorComment">The error comment dto.</param>
        /// <returns>Return the status of update comment.</returns>
        Task SaveCommentsAsync(ErrorComment errorComment);

        /// <summary>
        /// Gets the error details asynchronous.
        /// </summary>
        /// <param name="errorId">The error identifier.</param>
        /// <param name="canRetry">if set to <c>true</c> [can retry].</param>
        /// <returns>
        /// the error details.
        /// </returns>
        Task<IEnumerable<ErrorDetail>> GetErrorDetailsAsync(string errorId, bool canRetry);
    }
}
