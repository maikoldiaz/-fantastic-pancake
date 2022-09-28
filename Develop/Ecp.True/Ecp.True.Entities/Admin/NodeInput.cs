// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeInput.cs" company="Microsoft">
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
    /// The Node Category Tagging class.
    /// </summary>
    public class NodeInput : Entity
    {
        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        public int NodeId { get; set; }

        /// <summary>
        /// Gets or sets the node name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the node name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets the calculation date.
        /// </summary>
        /// <value>
        /// The calculation date.
        /// </value>
        public DateTime CalculationDate { get; set; }

        /// <summary>
        /// Gets or sets the control limit.
        /// </summary>
        /// <value>
        /// The Control Limit.
        /// </value>
        public decimal? ControlLimit { get; set; }

        /// <summary>
        /// Gets or sets the acceptable balance percentage.
        /// </summary>
        /// <value>
        /// The Acceptable Balance Percentage.
        /// </value>
        public decimal? AcceptableBalancePercentage { get; set; }
    }
}
