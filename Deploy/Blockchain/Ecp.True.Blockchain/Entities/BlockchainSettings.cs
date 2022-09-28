// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainSettings.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Blockchain.Entities
{
    /// <summary>
    /// Block Chain Settings.
    /// </summary>
    public class BlockchainSettings
    {
        /// <summary>
        /// Gets or sets the movement byte code.
        /// </summary>
        /// <value>
        /// The movement byte code.
        /// </value>
        public string MovementFactoryByteCode { get; set; }

        /// <summary>
        /// Gets or sets the movement abi.
        /// </summary>
        /// <value>
        /// The movement abi.
        /// </value>
        public string MovementFactoryAbi { get; set; }

        /// <summary>
        /// Gets or sets the movement address.
        /// </summary>
        /// <value>
        /// The movement address.
        /// </value>
        public string MovementFactoryContractAddress { get; set; }

        /// <summary>
        /// Gets or sets the inventory address.
        /// </summary>
        /// <value>
        /// The inventory address.
        /// </value>
        public string InventoryFactoryContractAddress { get; set; }

        /// <summary>
        /// Gets or sets the inventory byte code.
        /// </summary>
        /// <value>
        /// The inventory byte code.
        /// </value>
        public string InventoryFactoryByteCode { get; set; }

        /// <summary>
        /// Gets or sets the inventory abi.
        /// </summary>
        /// <value>
        /// The inventory abi.
        /// </value>
        public string InventoryFactoryAbi { get; set; }

        /// <summary>
        /// Gets or sets the RPC endpoint.
        /// </summary>
        /// <value>
        /// The RPC endpoint.
        /// </value>
        public string RpcEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the ethereum account address.
        /// </summary>
        /// <value>
        /// The ethereum account address.
        /// </value>
        public string EthereumAccountAddress { get; set; }

        /// <summary>
        /// Gets or sets the ethereum account key.
        /// </summary>
        /// <value>
        /// The ethereum account key.
        /// </value>
        public string EthereumAccountKey { get; set; }

        /// <summary>
        /// Gets or sets the node byte code.
        /// </summary>
        /// <value>
        /// The node byte code.
        /// </value>
        public string NodeByteCode { get; set; }

        /// <summary>
        /// Gets or sets the node contract abi.
        /// </summary>
        /// <value>
        /// The node abi.
        /// </value>
        public string NodeContractAbi { get; set; }

        /// <summary>
        /// Gets or sets the node contract address.
        /// </summary>
        /// <value>
        /// The node contract address.
        /// </value>
        public string NodeContractAddress { get; set; }

        /// <summary>
        /// Gets or sets the node connection byte code.
        /// </summary>
        /// <value>
        /// The node connection byte code.
        /// </value>
        public string NodeConnectionByteCode { get; set; }

        /// <summary>
        /// Gets or sets the node connection contract abi.
        /// </summary>
        /// <value>
        /// The node connection abi.
        /// </value>
        public string NodeConnectionContractAbi { get; set; }

        /// <summary>
        /// Gets or sets the node connection contract address.
        /// </summary>
        /// <value>
        /// The node connection contract address.
        /// </value>
        public string NodeConnectionContractAddress { get; set; }
    }
}
