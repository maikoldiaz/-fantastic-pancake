// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaCancellationTypes.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.OwnershipRules.OfficialDeltaDataRequest
{
    using System.ComponentModel;
    using Newtonsoft.Json;

    /// <summary>
    /// The OfficialDeltaCancellationTypes.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Query.QueryEntity" />
    [DisplayName("configuracionTiposMovimientos")]
    public class OfficialDeltaCancellationTypes
    {
        /// <summary>
        /// Gets or sets the source movement type identifier.
        /// </summary>
        /// <value>
        /// The source movement type identifier.
        /// </value>
        [JsonProperty("idTipoMovimiento")]
        public string SourceMovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets the reversed movement type identifier.
        /// </summary>
        /// <value>
        /// The reversed movement type identifier.
        /// </value>
        [JsonProperty("idTipoAnulacion")]
        public string AnnulationMovementTypeId { get; set; }
    }
}
