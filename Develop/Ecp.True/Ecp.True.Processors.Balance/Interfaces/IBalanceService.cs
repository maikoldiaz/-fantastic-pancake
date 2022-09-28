// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBalanceService.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Balance.Calculation.Input;
    using Ecp.True.Processors.Balance.Calculation.Output;

    /// <summary>
    /// The balance service.
    /// </summary>
    public interface IBalanceService
    {
        /// <summary>
        /// Processed the step.
        /// </summary>
        /// <param name="getUnbalances">The get unbalances.</param>
        /// <param name="step">The step.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>
        /// The collection of movements.
        /// </returns>
        Task<CalculationOutput> ProcessStepAsync(UnbalanceRequest getUnbalances, MovementCalculationStep step, IUnitOfWork unitOfWork, ITrueLogger logger);

        /// <summary>
        /// Process the calculation.
        /// </summary>
        /// <param name="balanceInput">The balance input.</param>
        /// <param name="ticket">The ticket.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="step">the step.</param>
        /// <returns>The calculation output.</returns>
        Task<CalculationOutput> ProcessCalculationAsync(BalanceInput balanceInput, Ticket ticket, IUnitOfWork unitOfWork, MovementCalculationStep step);

        /// <summary>
        /// Get balance input.
        /// </summary>
        /// <param name="getUnbalances">The get unbalances.</param>
        /// <returns>
        /// A <see cref="Task{TResult}" /> representing the result of the asynchronous operation.
        /// </returns>
        Task<BalanceInput> GetBalanceInputAsync(UnbalanceRequest getUnbalances);
    }
}
