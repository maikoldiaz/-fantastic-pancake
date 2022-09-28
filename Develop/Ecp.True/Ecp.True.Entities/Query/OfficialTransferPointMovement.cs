// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialTransferPointMovement.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Query
{
    using System;

    /// <summary>
    /// The category.
    /// </summary>
    public class OfficialTransferPointMovement : QueryEntity
    {
        /// <summary>
        /// Gets or sets the movement identifier.
        /// </summary>
        /// <value>
        /// The movement identifier.
        /// </value>
        public string MovementId { get; set; }

        /// <summary>
        /// Gets or sets the movement identifier.
        /// </summary>
        /// <value>
        /// The movement identifier.
        /// </value>
        public int MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the name of the movement type.
        /// </summary>
        /// <value>
        /// The name of the movement type.
        /// </value>
        public string MovementTypeName { get; set; }

        /// <summary>
        /// Gets or sets the source node.
        /// </summary>
        /// <value>
        /// The source node.
        /// </value>
        public string SourceNodeName { get; set; }

        /// <summary>
        /// Gets or sets the destination node.
        /// </summary>
        /// <value>
        /// The destination node.
        /// </value>
        public string DestinationNodeName { get; set; }

        /// <summary>
        /// Gets or sets the source product.
        /// </summary>
        /// <value>
        /// The source product.
        /// </value>
        public string SourceProductName { get; set; }

        /// <summary>
        /// Gets or sets the destination product.
        /// </summary>
        /// <value>
        /// The destination product.
        /// </value>
        public string DestinationProductName { get; set; }

        /// <summary>
        /// Gets or sets the movement volume.
        /// </summary>
        /// <value>
        /// The movement volume.
        /// </value>
        public decimal NetStandardVolume { get; set; }

        /// <summary>
        /// Gets or sets the operational date.
        /// </summary>
        /// <value>
        /// The operational date.
        /// </value>
        public DateTime OperationalDate { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        public string MeasurementUnit { get; set; }

        /// <summary>
        /// Gets or sets the sap tracking identifier.
        /// </summary>
        /// <value>
        /// The sap tracking identifier.
        /// </value>
        public int? SapTrackingId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has errors.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has errors; otherwise, <c>false</c>.
        /// </value>
        public int? ErrorCount { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }
    }
}
