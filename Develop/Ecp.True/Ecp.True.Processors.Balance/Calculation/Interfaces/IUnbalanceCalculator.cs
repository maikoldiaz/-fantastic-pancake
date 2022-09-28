// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUnbalanceCalculator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Balance.Calculation.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Processors.Balance.Calculation.Input;

    /// <summary>
    /// The unbalance calculator.
    /// </summary>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    public interface IUnbalanceCalculator
    {
        /// <summary>
        /// Calculates the asynchronous.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The unbalances output.</returns>
        Task<IEnumerable<UnbalanceComment>> CalculateAsync(CalculationInput input);
    }
}
