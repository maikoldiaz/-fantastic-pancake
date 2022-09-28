// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaBalance.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Admin
{
    using System;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The delta balances.
    /// </summary>
    public class DeltaBalance : Entity
    {
        /// <summary>
        /// Gets or sets the delta balance identifier.
        /// </summary>
        /// <value>
        /// The delta balance identifier.
        /// </value>
        public int DeltaBalanceId { get; set; }

        /// <summary>
        /// Gets or sets the segment identifier.
        /// </summary>
        /// <value>
        /// The segment identifier.
        /// </value>
        public int SegmentId { get; set; }

        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        public int NodeId { get; set; }

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
        /// Gets or sets the input.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        public decimal? Input { get; set; }

        /// <summary>
        /// Gets or sets the output.
        /// </summary>
        /// <value>
        /// The output.
        /// </value>
        public decimal? Output { get; set; }

        /// <summary>
        /// Gets or sets the delta input.
        /// </summary>
        /// <value>
        /// The delta input.
        /// </value>
        public decimal? DeltaInput { get; set; }

        /// <summary>
        /// Gets or sets the delta output.
        /// </summary>
        /// <value>
        /// The delta output.
        /// </value>
        public decimal? DeltaOutput { get; set; }

        /// <summary>
        /// Gets or sets the initial inventory.
        /// </summary>
        /// <value>
        /// The initial inventory.
        /// </value>
        public decimal? InitialInventory { get; set; }

        /// <summary>
        /// Gets or sets the final inventory.
        /// </summary>
        /// <value>
        /// The final inventory.
        /// </value>
        public decimal? FinalInventory { get; set; }

        /// <summary>
        /// Gets or sets the delta initial inventory.
        /// </summary>
        /// <value>
        /// The delta initial inventory.
        /// </value>
        public decimal? DeltaInitialInventory { get; set; }

        /// <summary>
        /// Gets or sets the delta final inventory.
        /// </summary>
        /// <value>
        /// The delta final inventory.
        /// </value>
        public decimal? DeltaFinalInventory { get; set; }

        /// <summary>
        /// Gets or sets the control.
        /// </summary>
        /// <value>
        /// The control.
        /// </value>
        public decimal? Control { get; set; }

        /// <summary>
        /// Gets or sets the element owner identifier.
        /// </summary>
        /// <value>
        /// The element owner identifier.
        /// </value>
        public int ElementOwnerId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        public string MeasurementUnit { get; set; }
    }
}
