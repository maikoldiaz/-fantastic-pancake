// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PendingTransaction.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// The pending transaction.
    /// </summary>
    public class PendingTransaction : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PendingTransaction"/> class.
        /// </summary>
        public PendingTransaction()
        {
            this.Errors = new List<PendingTransactionError>();
        }

        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        public int TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the operation.
        /// </summary>
        /// <value>
        /// The operation.
        /// </value>
        [JsonConverter(typeof(StringEnumConverter))]
        public FileRegistrationActionType? ActionTypeId { get; set; }

        /// <summary>
        /// Gets or sets the source node.
        /// </summary>
        /// <value>
        /// The source node.
        /// </value>
        public string SourceNode { get; set; }

        /// <summary>
        /// Gets or sets the destination node.
        /// </summary>
        /// <value>
        /// The destination node.
        /// </value>
        public string DestinationNode { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the source product.
        /// </summary>
        /// <value>
        /// The source product.
        /// </value>
        public string SourceProduct { get; set; }

        /// <summary>
        /// Gets or sets the destination product.
        /// </summary>
        /// <value>
        /// The destination product.
        /// </value>
        public string DestinationProduct { get; set; }

        /// <summary>
        /// Gets or sets the net volumen.
        /// </summary>
        /// <value>
        /// The net volumen.
        /// </value>
        public string Volume { get; set; }

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        public int? Units { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        public int? TicketId { get; set; }

        /// <summary>
        /// Gets or sets the message type identifier.
        /// </summary>
        /// <value>
        /// The message type identifier.
        /// </value>
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageType? MessageTypeId { get; set; }

        /// <summary>
        /// Gets or sets the file URL.
        /// </summary>
        /// <value>
        /// The file URL.
        /// </value>
        public string BlobName { get; set; }

        /// <summary>
        /// Gets or sets the message identifier.
        /// </summary>
        /// <value>
        /// The message identifier.
        /// </value>
        public string MessageId { get; set; }

        /// <summary>
        /// Gets or sets the system type identifier.
        /// </summary>
        /// <value>
        /// The system type identifier.
        /// </value>
        [JsonConverter(typeof(StringEnumConverter))]
        public SystemType? SystemTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the system.
        /// </summary>
        /// <value>
        /// The name of the system.
        /// </value>
        public int? SystemName { get; set; }

        /// <summary>
        /// Gets or sets the error json.
        /// </summary>
        /// <value>
        /// The error json.
        /// </value>
        public string ErrorJson { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public int? OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the segment identifier.
        /// </summary>
        /// <value>
        /// The segment identifier.
        /// </value>
        public int? SegmentId { get; set; }

        /// <summary>
        /// Gets or sets the type identifier.
        /// </summary>
        /// <value>
        /// The type identifier.
        /// </value>
        public int? TypeId { get; set; }

        /// <summary>
        /// Gets or sets the message type.
        /// </summary>
        /// <value>
        /// The message type.
        /// </value>
        public string MessageType { get; set; }

        /// <summary>
        /// Gets or sets the action type.
        /// </summary>
        /// <value>
        /// The action type.
        /// </value>
        public string ActionType { get; set; }

        /// <summary>
        /// Gets or sets the scenario identifier.
        /// </summary>
        /// <value>
        /// The scenario identifier.
        /// </value>
        public ScenarioType? ScenarioId { get; set; }

        /// <summary>
        /// Gets or sets the OriginMessageId.
        /// </summary>
        /// <value>
        /// The OriginMessageId.
        /// </value>
        public string OriginMessageId { get; set; }

        /// <summary>
        /// Gets or sets the ticket.
        /// </summary>
        /// <value>
        /// The ticket.
        /// </value>
        public virtual Ticket Ticket { get; set; }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>
        /// The owner.
        /// </value>
        public virtual CategoryElement Owner { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public virtual CategoryElement Type { get; set; }

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        public virtual CategoryElement Unit { get; set; }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public virtual ICollection<PendingTransactionError> Errors { get; set; }
    }
}
