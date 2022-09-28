// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementContract.cs" company="Microsoft">
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

    /// <summary>
    /// The movement contract.
    /// </summary>
    public class MovementContract : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovementContract"/> class.
        /// </summary>
        public MovementContract()
        {
            this.Movements = new List<Movement>();
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int MovementContractId { get; set; }

        /// <summary>
        /// Gets or sets the document identifier.
        /// </summary>
        /// <value>
        /// The document identifier.
        /// </value>
        public int DocumentNumber { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The contract position.
        /// </value>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The contract type identifier.
        /// </value>
        public int MovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        public int? SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the destination node identifier.
        /// </summary>
        /// <value>
        /// The destination node identifier.
        /// </value>
        public int? DestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the first owner identifier.
        /// </summary>
        /// <value>
        /// The first owner identifier.
        /// </value>
        public int? Owner1Id { get; set; }

        /// <summary>
        /// Gets or sets the second owner identifier.
        /// </summary>
        /// <value>
        /// The second owner identifier.
        /// </value>
        public int? Owner2Id { get; set; }

        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        public decimal Volume { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        public int MeasurementUnit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the ContractId.
        /// </summary>
        /// <value>
        /// The ContractId.
        /// </value>
        public int ContractId { get; set; }

        /// <summary>
        /// Gets or sets the source node.
        /// </summary>
        /// <value>
        /// The source node.
        /// </value>
        public virtual Node SourceNode { get; set; }

        /// <summary>
        /// Gets or sets the destination node.
        /// </summary>
        /// <value>
        /// The destination node.
        /// </value>
        public virtual Node DestinationNode { get; set; }

        /// <summary>
        /// Gets or sets the destination product.
        /// </summary>
        /// <value>
        /// The destination product.
        /// </value>
        public virtual Product Product { get; set; }

        /// <summary>
        /// Gets or sets the first owner.
        /// </summary>
        /// <value>
        /// The first owner.
        /// </value>
        public virtual CategoryElement Owner1 { get; set; }

        /// <summary>
        /// Gets or sets the second owner.
        /// </summary>
        /// <value>
        /// The second owner.
        /// </value>
        public virtual CategoryElement Owner2 { get; set; }

        /// <summary>
        /// Gets or sets the movement type.
        /// </summary>
        /// <value>
        /// The movement type.
        /// </value>
        public virtual CategoryElement MovementType { get; set; }

        /// <summary>
        /// Gets or sets the contract event.
        /// </summary>
        /// <value>
        /// The ownership contract.
        /// </value>
        public virtual CategoryElement MeasurementUnitDetail { get; set; }

        /// <summary>
        /// Gets the Movements.
        /// </summary>
        /// <value>
        /// The Movements.
        /// </value>
        public virtual ICollection<Movement> Movements { get; }

        /// <summary>
        /// Gets or sets the Contract.
        /// </summary>
        /// <value>
        /// The Contract.
        /// </value>
        public virtual Contract Contract { get; set; }
    }
}
