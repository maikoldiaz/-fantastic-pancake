// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISegmentBalanceService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Balance
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;

    /// <summary>
    /// The segment balance service.
    /// </summary>
    public interface ISegmentBalanceService
    {
        /// <summary>
        /// Processes the ownership asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>The Task.</returns>
        Task<IEnumerable<SegmentUnbalance>> ProcessSegmentAsync(int ticketId, IUnitOfWork unitOfWork);
    }
}
