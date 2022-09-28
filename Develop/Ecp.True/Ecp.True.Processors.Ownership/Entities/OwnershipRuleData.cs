// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipRuleData.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Entities
{
    using System.Collections.Generic;
    using Ecp.True.Core.Entities;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Proxies.OwnershipRules.Request;
    using Ecp.True.Proxies.OwnershipRules.Response;

    /// <summary>
    /// The ownership rule data class.
    /// </summary>
    public class OwnershipRuleData : OrchestratorMetaData
    {
        /// <summary>
        /// Gets or sets the ticket id.
        /// </summary>
        /// <value>
        /// The ticket id.
        /// </value>
        public int TicketId { get; set; }

        /// <summary>
        /// Gets or sets the ownership rule request.
        /// </summary>
        /// <value>
        /// The ownership rule request.
        /// </value>
        public OwnershipRuleRequest OwnershipRuleRequest { get; set; }

        /// <summary>
        /// Gets or sets the ownership rule response.
        /// </summary>
        /// <value>
        /// The ownership rule response.
        /// </value>
        public OwnershipRuleResponse OwnershipRuleResponse { get; set; }

        /// <summary>
        /// Gets or sets the transferPoint MovementWithOwnership.
        /// </summary>
        /// <value>
        /// The TransferPoint MovementWithOwnership.
        /// </value>
        public IEnumerable<OwnershipAnalytics> TransferPointMovements { get; set; }

        /// <summary>
        /// Gets or sets the list of errors.
        /// </summary>
        /// <value>
        /// The list of errors.
        /// </value>
        public IEnumerable<ErrorInfo> Errors { get; set; }

        /// <summary>
        /// Gets or sets the ownerships.
        /// </summary>
        /// <value>
        /// The ownerships.
        /// </value>
        public IEnumerable<Ownership> Ownerships { get; set; }

        /// <summary>
        /// Gets or sets the list of cancellation movements.
        /// </summary>
        /// <value>
        /// Return the list of cancellation movements.
        /// </value>
        public IEnumerable<CancellationMovementDetail> CancellationMovements { get; set; }

        /// <summary>
        /// Gets or sets the ownership calculations.
        /// </summary>
        /// <value>
        /// The ownership calculations.
        /// </value>
        public IEnumerable<OwnershipCalculation> OwnershipCalculations { get; set; }

        /// <summary>
        /// Gets or sets the segment ownership calculations.
        /// </summary>
        /// <value>
        /// The segment ownership calculations.
        /// </value>
        public IEnumerable<SegmentOwnershipCalculation> SegmentOwnershipCalculations { get; set; }

        /// <summary>
        /// Gets or sets the system ownership calculations.
        /// </summary>
        /// <value>
        /// The system ownership calculations.
        /// </value>
        public IEnumerable<SystemOwnershipCalculation> SystemOwnershipCalculations { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it has ProcessingErrors.
        /// </summary>
        /// <value>
        /// The Processing Errors.
        /// </value>
        public bool HasProcessingErrors { get; set; }

        /// <summary>
        /// Gets or sets the movements.
        /// </summary>
        /// <value>
        /// The movements.
        /// </value>
        public IEnumerable<Movement> Movements { get; set; }

        /// <summary>
        /// Gets or sets the ownership node identifier.
        /// </summary>
        /// <value>
        /// The ownership node identifier.
        /// </value>
        public int? OwnershipNodeId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there are deleted movement ownerships.
        /// </summary>
        /// <value>
        /// true or false.
        /// </value>
        public bool HasDeletedMovementOwnerships { get; set; }
    }
}
