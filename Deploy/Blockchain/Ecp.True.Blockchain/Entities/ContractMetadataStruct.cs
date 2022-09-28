// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContractMetadataStruct.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Blockchain.Entities
{
    using Nethereum.ABI.FunctionEncoding.Attributes;

    /// <summary>
    /// The ContractMetadataStruct.
    /// </summary>
    [FunctionOutput]
    public class ContractMetadataStruct
    {
        /// <summary>
        /// Gets or sets the name of the contract.
        /// </summary>
        /// <value>
        /// The name of the contract.
        /// </value>
        [Parameter("string", "ContractName", 1)]
        public string ContractName { get; set; }

        /// <summary>
        /// Gets or sets the contract abi.
        /// </summary>
        /// <value>
        /// The contract abi.
        /// </value>
        [Parameter("string", "ContractAbi", 2)]
        public string ContractAbi { get; set; }

        /// <summary>
        /// Gets or sets the contract creation date.
        /// </summary>
        /// <value>
        /// The contract creation date.
        /// </value>
        [Parameter("string", "ContractCreationDate", 3)]
        public string ContractCreationDate { get; set; }

        /// <summary>
        /// Gets or sets the contract address.
        /// </summary>
        /// <value>
        /// The contract address.
        /// </value>
        [Parameter("address", "ContractAddress", 4)]
        public string ContractAddress { get; set; }

        /// <summary>
        /// Gets or sets the contract version.
        /// </summary>
        /// <value>
        /// The contract version.
        /// </value>
        [Parameter("int64", "ContractVersion", 5)]
        public int ContractVersion { get; set; }
    }
}
