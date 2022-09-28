// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICalculationService.cs" company="Microsoft">
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
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Balance.Calculation.Input;
    using Ecp.True.Processors.Balance.Calculation.Output;

    /// <summary>
    /// The calculation service.
    /// </summary>
    public interface ICalculationService
    {
        /// <summary>
        /// Gets the variable type.
        /// </summary>
        /// <value>
        /// The variable type.
        /// </value>
        MovementCalculationStep Type { get; }

        /// <summary>
        /// Processes the interfaces asynchronous.
        /// </summary>
        /// <param name="balanceInputInfo">The balance input info.</param>
        /// <param name="ticket">The ticket.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>
        /// The collection of interface.
        /// </returns>
        Task<CalculationOutput> ProcessAsync(
            BalanceInputInfo balanceInputInfo, Ticket ticket, IUnitOfWork unitOfWork);
    }
}
