// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialErrorInventory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Entities.OfficialDelta
{
    using Ecp.True.Entities.Enumeration;

    /// <summary>
    /// The OfficialErrorInventory.
    /// </summary>
    public class OfficialErrorInventory
    {
        /// <summary>
        /// Gets or sets the inventory identifier.
        /// </summary>
        /// <value>
        /// The inventory identifier.
        /// </value>
        public string InventoryId { get; set; }

        /// <summary>
        /// Gets or sets the inventory transaction identifier.
        /// </summary>
        /// <value>
        /// The inventory transaction identifier.
        /// </value>
        public int InventoryProductId { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the origen.
        /// </summary>
        /// <value>
        /// The origen.
        /// </value>
        public OriginType Origin { get; set; }
    }
}
