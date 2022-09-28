// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConciliationProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Conciliation.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Conciliation.Entities;

    /// <summary>
    /// The IConciliationProcessor.
    /// </summary>
    public interface IConciliationProcessor : IProcessor
    {
        /// <summary>
        /// Generates the CalculateDelta.
        /// </summary>
        /// <param name="movementConciliations">the segmentMovements.</param>
        /// <param name="ticket">the ticket.</param>
        /// <param name="otherSegmentMovements">the otherSegmentMovements.</param>
        /// <returns>Task.</returns>
        Task CalculateConciliationAsync(MovementConciliations movementConciliations, Ticket ticket, IEnumerable<MovementConciliationDto> otherSegmentMovements);

        /// <summary>
        /// This Procedure is used to read point nodes connections.
        /// </summary>
        /// <param name="nodeId">The node Id.</param>
        /// <param name="segmentId">The segment Id.</param>
        /// <returns>Returns the nodes availables.</returns>
        Task<IEnumerable<ConciliationNodesResult>> GetConciliationNodeConnectionsAsync(int? nodeId, int? segmentId);

        /// <summary>
        /// Get Conciliation Transfer Point Movements.
        /// </summary>
        /// <param name="connectionConciliationNodes">Connection conciliationNode.</param>
        /// <returns>TransferPointConciliationMovements.</returns>
        Task<IEnumerable<TransferPointConciliationMovement>> GetConciliationTransferPointMovementsAsync(ConnectionConciliationNodesResquest connectionConciliationNodes);

        /// <summary>
        /// Start Conciliation.
        /// </summary>
        /// <param name="conciliationNodes">The conciliation Node.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task DoConciliationAsync(ConciliationNodesResquest conciliationNodes);

        /// <summary>
        /// Manual Conciliation Queue.
        /// </summary>
        /// <param name="conciliationNodes">conciliationNodes.</param>
        /// <returns>bool.</returns>
        Task InitializeConciliationAsync(ConciliationNodesResquest conciliationNodes);

        /// <summary>
        /// Finalizes the process asynchronous.
        /// </summary>
        /// <param name="conciliationRuleData">The ticket identifier.</param>
        /// <returns>The task.</returns>
        Task FinalizeProcessAsync(ConciliationRuleData conciliationRuleData);

        /// <summary>
        /// Update OwnershipNode for ticket.
        /// </summary>
        /// <param name="ticketId">ticketId. </param>
        /// <param name="status">status. </param>
        /// <param name="ownershipStatus">ownership status. </param>
        /// <param name="nodeId">nodeId. </param>
        /// <returns>Task. </returns>
        Task UpdateOwnershipNodeAsync(int ticketId, StatusType status, OwnershipNodeStatusType ownershipStatus, int? nodeId);

        /// <summary>
        /// Get conciliation movements for ticket.
        /// </summary>
        /// <param name="ticketId">ticketId. </param>
        /// <param name="nodeId">nodeId. </param>
        /// <returns>Task. </returns>
        Task<IEnumerable<Movement>> GetConciliationMovementsAsync(int ticketId, int? nodeId);

        /// <summary>
        /// Registers the negative conciliation movements asynchronous.
        /// </summary>
        /// <param name="movements">The movement.</param>
        /// <returns>The task.</returns>
        Task RegisterNegativeMovementsAsync(IEnumerable<Movement> movements);

        /// <summary>
        /// Get conciliation nodes.
        /// </summary>
        /// <param name="ticketId">Ticket id.</param>
        /// <param name="nodeId">Node id.</param>
        /// <returns>OwnershipNodeData list task.</returns>
        Task<IEnumerable<OwnershipNodeData>> GetConciliationNodesAsync(int ticketId, int? nodeId);

        /// <summary>
        /// Update ticket status.
        /// </summary>
        /// <param name="ticketId">ticketId. </param>
        /// <param name="statusType">statusType. </param>
        /// <param name="error">error. </param>
        /// <returns>Task. </returns>
        Task UpdateStatusTicketAsync(int ticketId, StatusType statusType, string error);

        /// <summary>
        /// Update relationship other segment movements.
        /// </summary>
        /// <param name="ticketId">ticketId. </param>
        /// <param name="nodeId">nodeId. </param>
        /// <returns>Task. </returns>
        Task DeleteRelationshipOtherSegmentMovementsAsync(int ticketId, int? nodeId);

        /// <summary>
        /// Delete  relationship other segment movements.
        /// </summary>
        /// <param name="ticketId">ticketId. </param>
        /// <param name="nodeId">nodeId. </param>
        /// <returns>Task. </returns>
        Task DeleteConciliationMovementsAsync(int ticketId, int? nodeId);
    }
}
