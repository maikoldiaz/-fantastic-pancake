// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderSale.cs" company="Microsoft">
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
    public class OrderSale
    {
        /// <summary>
        /// Gets or sets the Control Data.
        /// </summary>
        /// <value>
        /// The Control Data.
        /// </value>
        [Required]
        [JsonProperty("DATOSCONTROL")]
        public ControlData ControlData { get; set; }

        /// <summary>
        /// Gets or sets the Header.
        /// </summary>
        /// <value>
        /// The Header.
        /// </value>
        [Required]
        [JsonProperty("HEADER")]
        public Header Header { get; set; }

        /// <summary>
        /// Gets or sets the Positions.
        /// </summary>
        /// <value>
        /// The Positions.
        /// </value>
        [Required]
        [JsonProperty("POSITIONS")]
        public PositionObject PositionObject { get; set; }
    }
}
