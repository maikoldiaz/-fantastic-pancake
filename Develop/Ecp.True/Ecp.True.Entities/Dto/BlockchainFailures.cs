// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainFailures.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Dto
{
    using System.Collections.Generic;

    /// <summary>
    /// The blockchain failures.
    /// </summary>
    public class BlockchainFailures
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlockchainFailures"/> class.
        /// </summary>
        public BlockchainFailures()
        {
            this.Connections = new List<int>();
            this.Owners = new List<int>();
            this.Nodes = new List<int>();
            this.Ownerships = new List<int>();
            this.Movements = new List<int>();
            this.Unbalances = new List<int>();
            this.InventoryProducts = new List<int>();
        }

        /// <summary>
        ///     Gets or sets the Ownership.
        /// </summary>
        /// <value>
        ///     The Ownership Id.
        /// </value>
        public IEnumerable<int> Ownerships { get; set; }

        /// <summary>
        /// Gets or sets the unbalances.
        /// </summary>
        /// <value>
        /// The unbalances.
        /// </value>
        public IEnumerable<int> Unbalances { get; set; }

        /// <summary>
        ///     Gets or sets the movement.
        /// </summary>
        /// <value>
        ///     The Movement Id.
        /// </value>
        public IEnumerable<int> Movements { get; set; }

        /// <summary>
        ///     Gets or sets the Inventory Product.
        /// </summary>
        /// <value>
        ///     The Inventory Product Id.
        /// </value>
        public IEnumerable<int> InventoryProducts { get; set; }

        /// <summary>
        /// Gets or sets the nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        public IEnumerable<int> Nodes { get; set; }

        /// <summary>
        /// Gets or sets the connections.
        /// </summary>
        /// <value>
        /// The connections.
        /// </value>
        public IEnumerable<int> Connections { get; set; }

        /// <summary>
        /// Gets or sets the owners.
        /// </summary>
        /// <value>
        /// The owners.
        /// </value>
        public IEnumerable<int> Owners { get; set; }
    }
}
