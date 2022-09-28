// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BalanceInput.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Balance.Calculation.Input
{
    using System.Collections.Generic;
    using Ecp.True.Entities.Admin;

    /// <summary>
    /// The TInput.
    /// </summary>
    public class BalanceInput
    {
        /// <summary>
        /// Gets or sets the nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        public IEnumerable<NodeInput> Nodes { get; set; } = new List<NodeInput>();

        /// <summary>
        /// Gets or sets the nodes for interface calculation.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        public IEnumerable<NodeInput> InterfaceNodes { get; set; } = new List<NodeInput>();

        /// <summary>
        /// Gets or sets the Movements.
        /// </summary>
        /// <value>
        /// The Movements.
        /// </value>
        public IEnumerable<MovementCalculationInput> Movements { get; set; } = new List<MovementCalculationInput>();

        /// <summary>
        /// Gets or sets the Inventories.
        /// </summary>
        /// <value>
        /// The Inventories.
        /// </value>
        public IEnumerable<InventoryInput> Inventories { get; set; } = new List<InventoryInput>();
    }
}
