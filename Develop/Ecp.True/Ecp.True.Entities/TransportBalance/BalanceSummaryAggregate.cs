// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BalanceSummaryAggregate.cs" company="Microsoft">
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
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;

    /// <summary>
    /// The Ownership Node Balance Summary.
    /// </summary>
    public class BalanceSummaryAggregate : Entity
    {
        /// <summary>
        /// Gets or sets the initial inventory.
        /// </summary>
        /// <value>
        /// The initial inventory.
        /// </value>
        public decimal? InitialInventory { get; set; }

        /// <summary>
        /// Gets or sets the inputs.
        /// </summary>
        /// <value>
        /// The inputs.
        /// </value>
        public decimal? Inputs { get; set; }

        /// <summary>
        /// Gets or sets the outputs.
        /// </summary>
        /// <value>
        /// The outputs.
        /// </value>
        public decimal? Outputs { get; set; }

        /// <summary>
        /// Gets or sets the identified losses.
        /// </summary>
        /// <value>
        /// The identified losses.
        /// </value>
        public decimal? IdentifiedLosses { get; set; }

        /// <summary>
        /// Gets or sets the interface.
        /// </summary>
        /// <value>
        /// The interface.
        /// </value>
        public decimal? Interface { get; set; }

        /// <summary>
        /// Gets or sets the tolerance.
        /// </summary>
        /// <value>
        /// The tolerance.
        /// </value>
        public decimal? Tolerance { get; set; }

        /// <summary>
        /// Gets or sets the unidentified losses.
        /// </summary>
        /// <value>
        /// The unidentified losses.
        /// </value>
        public decimal? UnidentifiedLosses { get; set; }

        /// <summary>
        /// Gets or sets the final inventory.
        /// </summary>
        /// <value>
        /// The final inventory.
        /// </value>
        public decimal? FinalInventory { get; set; }

        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        public decimal? Volume { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        public string MeasurementUnit { get; set; }

        /// <summary>
        /// Gets or sets the control.
        /// </summary>
        /// <value>
        /// The control.
        /// </value>
        public string Control { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public OwnershipNodeStatusType? OwnershipStatusId { get; set; }
    }
}