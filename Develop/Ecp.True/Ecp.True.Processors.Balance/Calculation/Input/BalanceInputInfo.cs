// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BalanceInputInfo.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using Ecp.True.Entities.Admin;

    /// <summary>
    /// The TInput.
    /// </summary>
    public class BalanceInputInfo
    {
        /// <summary>
        /// Gets or sets the calculation date.
        /// </summary>
        /// <value>
        /// The calculation date.
        /// </value>
        public DateTime CalculationDate { get; set; }

        /// <summary>
        /// Gets or sets the nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        public IEnumerable<NodeInput> Nodes { get; set; }

        /// <summary>
        /// Gets or sets the interface nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        public IEnumerable<NodeInput> InterfaceNodes { get; set; }

        /// <summary>
        /// Gets or sets the Movements.
        /// </summary>
        /// <value>
        /// The Movements.
        /// </value>
        public IEnumerable<MovementCalculationInput> Movements { get; set; }

        /// <summary>
        /// Gets or sets the Inventories.
        /// </summary>
        /// <value>
        /// The Inventories.
        /// </value>
        public IEnumerable<InventoryInput> InitialInventories { get; set; }

        /// <summary>
        /// Gets or sets the Inventories.
        /// </summary>
        /// <value>
        /// The Inventories.
        /// </value>
        public IEnumerable<InventoryInput> FinalInventories { get; set; }
    }
}
