// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaResponse.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.OwnershipRules.Response
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// The DeltaResponse.
    /// </summary>
    public class DeltaResponse
    {
        /// <summary>
        /// Gets or sets the result movements.
        /// </summary>
        /// <value>
        /// The result movements.
        /// </value>
        [JsonProperty("resultadoMovimientos")]
        public IEnumerable<DeltaResultMovement> ResultMovements { get; set; }

        /// <summary>
        /// Gets or sets the result inventories.
        /// </summary>
        /// <value>
        /// The result inventories.
        /// </value>
        [JsonProperty("resultadoInventario")]
        public IEnumerable<DeltaResultInventory> ResultInventories { get; set; }

        /// <summary>
        /// Gets or sets the error movements.
        /// </summary>
        /// <value>
        /// The error movements.
        /// </value>
        [JsonProperty("movimientosErrores")]
        public IEnumerable<DeltaErrorMovement> ErrorMovements { get; set; }

        /// <summary>
        /// Gets or sets the error inventories.
        /// </summary>
        /// <value>
        /// The error inventories.
        /// </value>
        [JsonProperty("inventariosErrores")]
        public IEnumerable<DeltaErrorInventory> ErrorInventories { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        [JsonIgnore]
        public string Content { get; set; }
    }
}
