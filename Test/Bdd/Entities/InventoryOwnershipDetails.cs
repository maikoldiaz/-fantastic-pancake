// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryOwnershipDetails.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.Entities
{
    using Nethereum.ABI.FunctionEncoding.Attributes;

    [FunctionOutput]
    public class InventoryOwnershipDetails
    {
        /// <summary>
        /// Gets or sets the movement identifier.
        /// </summary>
        /// <value>
        /// The movement identifier.
        /// </value>
        [Parameter("string", "inventoryId", 1)]
        public string InventoryId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [Parameter("string", "blockchainInventoryProductTransactionId", 2)]
        public string BlockchainInventoryProductTransactionId { get; set; }

        [Parameter("string", "OwnerId", 3)]
        public string OwnerId { get; set; }

        [Parameter("int256", "ownershipValue", 4)]
        public long OwnershipValue { get; set; }

        [Parameter("string", "ownershipValueUnit", 5)]
        public string OwnershipValueUnit { get; set; }

        [Parameter("string", "ownerTransactionId", 6)]
        public string OwnerTransactionId { get; set; }
    }
}