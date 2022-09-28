// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IManualMovementProcessor.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Interfaces
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The movement processor.
    /// </summary>
    public interface IManualMovementProcessor
    {
        /// <summary>
        /// Gets the manual movements asynchronously.
        /// </summary>
        /// <returns>The enumerable of movements.</returns>
        /// <param name="nodeId">The nodeId.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        Task<IQueryable<Movement>> GetAssignableMovementsAsync(int nodeId, DateTime startTime, DateTime endTime);

        /// <summary>
        /// Updates the manual movements and adds them to a ticket.
        /// </summary>
        /// <param name="deltaNodeId">The delta node to add the movements to.</param>
        /// <param name="movementIds">The movements to add.</param>
        /// <returns>The task.</returns>
        Task UpdateTicketManualMovementsAsync(int deltaNodeId, int[] movementIds);
    }
}