// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OriginalInventory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Query
{
    /// <summary>
    /// The OriginalInventory.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Query.QueryEntity" />
    public class OriginalInventory : QueryEntity
    {
        /// <summary>
        /// Gets or sets the inventory identifier.
        /// </summary>
        /// <value>
        /// The inventory identifier.
        /// </value>
        public string InventoryProductUniqueId { get; set; }

        /// <summary>
        /// Gets or sets the inventory product identifier.
        /// </summary>
        /// <value>
        /// The inventory transaction identifier.
        /// </value>
        public int InventoryProductId { get; set; }

        /// <summary>
        /// Gets or sets the product volume.
        /// </summary>
        /// <value>
        /// The product volume.
        /// </value>
        public decimal ProductVolume { get; set; }
    }
}
