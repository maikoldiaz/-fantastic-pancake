// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryDetailStruct.cs" company="Microsoft">
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
    public class InventoryDetailStruct
    {
        /// <summary>
        /// Gets or sets the movement identifier.
        /// </summary>
        /// <value>
        /// The movement identifier.
        /// </value>
        [Parameter("string", "InventoryProductId", 1)]
        public string InventoryProductId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [Parameter("int256", "ProductVolume", 2)]
        public long ProductVolume { get; set; }

        [Parameter("string", "MeasurementUnit", 3)]
        public string MeasurementUnit { get; set; }

        [Parameter("int64", "InventoryDate", 4)]
        public long InventoryDate { get; set; }

        [Parameter("string", "BlockchainInventoryProductTransactionId", 5)]
        public string BlockchainInventoryProductTransactionId { get; set; }

        [Parameter("string", "Metadata", 6)]
        public string Metadata { get; set; }
    }
}