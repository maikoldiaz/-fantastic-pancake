// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainEventConfiguration.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Configuration
{
    /// <summary>
    /// The Blockchain Event Configuration.
    /// </summary>
    public class BlockchainEventConfiguration
    {
        /// <summary>
        /// Gets or sets the name of the contract.
        /// </summary>
        /// <value>
        /// The name of the contract.
        /// </value>
        public string ContractName { get; set; }

        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        /// <value>
        /// The name of the event.
        /// </value>
        public string EventName { get; set; }

        /// <summary>
        /// Gets or sets the contract address.
        /// </summary>
        /// <value>
        /// The contract address.
        /// </value>
        public string ContractAddress { get; set; }

        /// <summary>
        /// Gets or sets the contract abi.
        /// </summary>
        /// <value>
        /// The contract abi.
        /// </value>
        public string ContractAbi { get; set; }
    }
}
