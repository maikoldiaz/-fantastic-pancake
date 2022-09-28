// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationalDeltaMovement.cs" company="Microsoft">
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
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The category.
    /// </summary>
    public class OperationalDeltaMovement : QueryEntity
    {
        /// <summary>
        /// Gets or sets the movement identifier.
        /// </summary>
        /// <value>
        /// The inventory identifier.
        /// </value>
        public string MovementId { get; set; }

        /// <summary>
        /// Gets or sets the movement type.
        /// </summary>
        /// <value>
        /// The movement type.
        /// </value>
        public string MovementType { get; set; }

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
        /// Gets or sets the source product.
        /// </summary>
        /// <value>
        /// The source node.
        /// </value>
        public string SourceProduct { get; set; }

        /// <summary>
        /// Gets or sets the destination product.
        /// </summary>
        /// <value>
        /// The destination node.
        /// </value>
        public string DestinationProduct { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the inventory date.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public DateTime OperationalDate { get; set; }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        public EventType Action { get; set; }
    }
}
