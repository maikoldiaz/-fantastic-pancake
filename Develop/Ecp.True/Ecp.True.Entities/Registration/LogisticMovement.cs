// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogisticMovement.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Registration
{
    using System;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// The movemement Logistic.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class LogisticMovement : Entity
    {
        /// <summary>
        /// Gets or sets the movement Logistic identifier.
        /// </summary>
        /// <value>
        /// The movement Logistic identifier.
        /// </value>
        public int LogisticMovementId { get; set; }

        /// <summary>
        /// Gets or sets the movement transaction identifier.
        /// </summary>
        /// <value>
        /// The movement transaction identifier.
        /// </value>
        public int MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        public int TicketId { get; set; }

        /// <summary>
        /// Gets or sets the destination System identifier.
        /// </summary>
        /// <value>
        /// The destination System identifier.
        /// </value>
        public string EventType { get; set; }

        /// <summary>
        /// Gets or sets the destination System identifier.
        /// </summary>
        /// <value>
        /// The destination System identifier.
        /// </value>
        public string DestinationSystem { get; set; }

        /// <summary>
        /// Gets or sets the Order Movement identifier.
        /// </summary>
        /// <value>
        /// The Order Movement identifier.
        /// </value>
        public int? MovementOrder { get; set; }

        /// <summary>
        /// Gets or sets the number of total register.
        /// </summary>
        /// <value>
        /// The number of total register.
        /// </value>
        public int? NumReg { get; set; }

        /// <summary>
        /// Gets or sets the Order Node.
        /// </summary>
        /// <value>
        /// The Order Node.
        /// </value>
        public int? NodeOrder { get; set; }

        /// <summary>
        /// Gets or sets the StartTime Period.
        /// </summary>
        /// <value>
        /// The StartTime Period.
        /// </value>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the Source Plant.
        /// </summary>
        /// <value>
        /// The Source Plant.
        /// </value>
        public string SourceLogisticCenterId { get; set; }

        /// <summary>
        /// Gets or sets the Logistic Center DestinationId.
        /// </summary>
        /// <value>
        /// The Logistic Center DestinationId.
        /// </value>
        public string DestinationLogisticCenterId { get; set; }

        /// <summary>
        /// Gets or sets the Net Standard Quantity.
        /// </summary>
        /// <value>
        /// The Net Standard Quantity.
        /// </value>
        public decimal? OwnershipVolume { get; set; }

        /// <summary>
        /// Gets or sets the Measurement Unit.
        /// </summary>
        /// <value>
        /// The Measurement Unit.
        /// </value>
        public long? MeasurementUnit { get; set; }

        /// <summary>
        /// Gets or sets the Logistic Movement TypeId.
        /// </summary>
        /// <value>
        /// The Logistic Movement TypeId.
        /// </value>
        public string LogisticMovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets the Homologated Movement Type.
        /// </summary>
        /// <value>
        /// The Homologated Movement Type.
        /// </value>
        public string HomologatedMovementType { get; set; }

        /// <summary>
        /// Gets or sets the Document Number.
        /// </summary>
        /// <value>
        /// The Document Number.
        /// </value>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Gets or sets the Position.
        /// </summary>
        /// <value>
        /// The Position.
        /// </value>
        public int? Position { get; set; }

        /// <summary>
        /// Gets or sets the Cost Center.
        /// </summary>
        /// <value>
        /// The Cost Center.
        /// </value>
        public int? CostCenterId { get; set; }

        /// <summary>
        /// Gets or sets the Transaction Code.
        /// </summary>
        /// <value>
        /// The Transaction Code.
        /// </value>
        public string SapTransactionCode { get; set; }

        /// <summary>
        /// Gets or sets the Sap Status Process.
        /// </summary>
        /// <value>
        /// The Sap Status Process.
        /// </value>
        public StatusType StatusProcessId { get; set; }

        /// <summary>
        /// Gets or sets the Sap Information.
        /// </summary>
        /// <value>
        /// The Sap Information.
        /// </value>
        public string MessageProcess { get; set; }

        /// <summary>
        /// Gets or sets the Sap TransactionId.
        /// </summary>
        /// <value>
        /// The Sap TransactionId.
        /// </value>
        public string SapTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the Is Check.
        /// </summary>
        /// <value>
        /// The Is Check.
        /// </value>
        public int IsCheck { get; set; }

        /// <summary>
        /// Gets or sets the Sap Sent Date to movement.
        /// </summary>
        /// <value>
        /// The Sap Sent Date to movement.
        /// </value>
        public DateTime? SapSentDate { get; set; }

        /// <summary>
        /// Gets or sets the movement transaction.
        /// </summary>
        /// <value>
        /// The movement transaction.
        /// </value>
        public virtual Movement MovementTransaction { get; set; }

        /// <summary>
        /// Gets or sets the SourceProduct.
        /// </summary>
        /// <value>
        /// The SourceProduct.
        /// </value>
        public string SourceProductId { get; set; }

        /// <summary>
        /// Gets or sets the DestinationProduct.
        /// </summary>
        /// <value>
        /// The DestinationProduct.
        /// </value>
        public string DestinationProductId { get; set; }

        /// <summary>
        /// Gets or sets the SourceLogisticNodeId.
        /// </summary>
        /// <value>
        /// The SourceLogisticNodeId.
        /// </value>
        public int? SourceLogisticNodeId { get; set; }

        /// <summary>
        /// Gets or sets the DestinationLogisticNodeId.
        /// </summary>
        /// <value>
        /// The DestinationLogisticNodeId.
        /// </value>
        public int? DestinationLogisticNodeId { get; set; }

        /// <summary>
        /// Gets or sets the concatenated movement identifier.
        /// </summary>
        /// <value>
        /// The concatenated movement identifier.
        /// </value>
        public string ConcatMovementId { get; set; }

        /// <summary>
        /// Gets or sets the movement Cost Center.
        /// </summary>
        /// <value>
        /// The movement movement Cost Center.
        /// </value>
        public virtual CategoryElement CategoryCostCenter { get; set; }

        /// <summary>
        /// Gets or sets the Ticket.
        /// </summary>
        /// <value>
        /// The movement Ticket.
        /// </value>
        public virtual Ticket TicketLogisticMovement { get; set; }
    }
}
