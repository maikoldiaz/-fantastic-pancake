// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISystemOwnershipCalculationService.cs" company="Microsoft">
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
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The IOwnershipProcessor.
    /// </summary>
    public interface ISystemOwnershipCalculationService
    {
        /// <summary>
        /// Processes the ownership asynchronous.
        /// </summary>
        /// <param name="inventories">The inventories.</param>
        /// <param name="movements">The movements.</param>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>The Task.</returns>
        Task<IEnumerable<SystemOwnershipCalculation>> ProcessSystemOwnershipAsync(
            IEnumerable<InventoryProduct> inventories,
            IEnumerable<Movement> movements,
            int ticketId);
    }
}
