// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResultInventory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------\

namespace Ecp.True.Processors.Delta.Entities
{
    /// <summary>
    /// The ResultInventory.
    /// </summary>
    public class ResultInventory
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
        /// The inventory product identifier.
        /// </value>
        public int InventoryProductId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ResultInventory"/> is sign.
        /// </summary>
        /// <value>
        ///   <c>true</c> if sign; otherwise, <c>false</c>.
        /// </value>
        public bool Sign { get; set; }

        /// <summary>
        /// Gets or sets the delta.
        /// </summary>
        /// <value>
        /// The delta.
        /// </value>
        public decimal Delta { get; set; }
    }
}
