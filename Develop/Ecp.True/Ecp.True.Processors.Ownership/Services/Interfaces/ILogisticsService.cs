// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogisticsService.cs" company="Microsoft">
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
    using System.Data;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// The ILogisticsService.
    /// </summary>
    public interface ILogisticsService
    {
        /// <summary>
        /// Transforms the specified movements.
        /// </summary>
        /// <param name="movements">The movements.</param>
        /// <param name="ticket">The ticket.</param>
        /// <param name="systemType">The systemType.</param>
        /// <param name="scenarioType">The scenarioType.</param>
        /// <returns>The details.</returns>
        Task<IEnumerable<GenericLogisticsMovement>> TransformAsync(IEnumerable<GenericLogisticsMovement> movements, Ticket ticket, SystemType systemType, ScenarioType scenarioType);

        /// <summary>
        /// Generates the specified official logistics details.
        /// </summary>
        /// <param name="officialLogisticsDetails">The official logistics details.</param>
        /// <returns>The dataset.</returns>
        DataSet Generate(IEnumerable<OfficialLogisticsDetails> officialLogisticsDetails);

        /// <summary>
        /// Process logistic movement.
        /// </summary>
        /// <param name="logisticMovement">The logistic movement param.</param>
        /// <returns>The response.</returns>
        Task ProcessLogisticMovementAsync(LogisticMovementResponse logisticMovement);

        /// <summary>
        /// Finalize process of official movements.
        /// </summary>
        /// <param name="officialLogisticsMovement">The official logistics movements.</param>
        /// <param name="ticket">The ticket.</param>
        /// <param name="systemType">The system type.</param>
        /// <returns>The response.</returns>
        Task DoFinalizeOfficialProcessAsync(IEnumerable<GenericLogisticsMovement> officialLogisticsMovement, Ticket ticket, SystemType systemType);

        /// <summary>
        /// Finalize process of operative movements.
        /// </summary>
        /// <param name="officialLogisticsMovement">The operative logistics movements.</param>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The response.</returns>
        Task DoFinalizeAsync(IEnumerable<GenericLogisticsMovement> officialLogisticsMovement, Ticket ticket);

        /// <summary>
        /// Get the logistic movement.
        /// </summary>
        /// <param name="movementId">The movementId.</param>
        /// <returns>the logistic movement.</returns>
        Task<LogisticMovement> GetLogisticMovementByMovementIdAsync(string movementId);

        /// <summary>
        /// Get the logistic movement.
        /// </summary>
        /// <param name="movementId">The movementId.</param>
        /// <returns>Exits logistic movement.</returns>
        Task<bool> ExistLogisticMovementByMovementIdAsync(string movementId);
    }
}
