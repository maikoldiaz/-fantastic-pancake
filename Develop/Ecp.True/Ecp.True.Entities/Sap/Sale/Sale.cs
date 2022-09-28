// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Sale.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Sap
{
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    /// <summary>
    /// The Sale class.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class Sale
    {
        /// <summary>
        /// Gets or sets the Order Sale.
        /// </summary>
        /// <value>
        /// The Order Sale.
        /// </value>
        [Required]
        [JsonProperty("CREATE_ORDERSALE")]
        public OrderSale OrderSale { get; set; }
    }
}
