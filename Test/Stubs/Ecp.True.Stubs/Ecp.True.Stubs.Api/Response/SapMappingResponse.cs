// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapMappingResponse.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Stubs.Api.Response
{
    using System.ComponentModel;
    using Newtonsoft.Json;

    /// <summary>
    /// The Volume Output class.
    /// </summary>
    [DisplayName("Nodes")]
    public class SapMappingResponse
    {
        /// <summary>
        /// Gets or sets sourceSystemId .
        /// </summary>
        /// <value>
        ///  Sets sourceSystemId .
        /// </value>
        [JsonProperty("sourceSystemId")]
        public int SourceSystemId { get; set; }

        /// <summary>
        /// Gets or sets sourceMovementTypeId .
        /// </summary>
        /// <value>
        ///  Sets sourceMovementTypeId .
        /// </value>
        [JsonProperty("sourceMovementTypeId")]
        public int SourceMovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets sourceProductId .
        /// </summary>
        /// <value>
        ///  Sets sourceProductId .
        /// </value>
        [JsonProperty("sourceProductId")]
        public string SourceProductId { get; set; }

        /// <summary>
        /// Gets or sets sourceSystem_SourceNodeId .
        /// </summary>
        /// <value>
        ///  Sets sourceSystem_SourceNodeId .
        /// </value>
        [JsonProperty("sourceSystem_SourceNodeId")]
        public int SourceSystemSourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets sourceSystem_DestinationNodeId .
        /// </summary>
        /// <value>
        ///  Sets sourceSystem_DestinationNodeId .
        /// </value>
        [JsonProperty("sourceSystem_DestinationNodeId")]
        public int SourceSystemDestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets destinationSystemId .
        /// </summary>
        /// <value>
        ///  Sets destinationSystemId .
        /// </value>
        [JsonProperty("destinationSystemId")]
        public int DestinationSystemId { get; set; }

        /// <summary>
        /// Gets or sets destinationMovementTypeId .
        /// </summary>
        /// <value>
        ///  Sets destinationMovementTypeId .
        /// </value>
        [JsonProperty("destinationMovementTypeId")]
        public int DestinationMovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets destinationProductId .
        /// </summary>
        /// <value>
        ///  Sets destinationProductId .
        /// </value>
        [JsonProperty("destinationProductId")]
        public string DestinationProductId { get; set; }

        /// <summary>
        /// Gets or sets destinationSystem_SourceNodeId .
        /// </summary>
        /// <value>
        ///  Sets destinationSystem_SourceNodeId .
        /// </value>
        [JsonProperty("destinationSystem_SourceNodeId")]
        public int DestinationSystemSourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets destinationSystem_DestinationNodeId .
        /// </summary>
        /// <value>
        ///  Sets destinationSystem_DestinationNodeId .
        /// </value>
        [JsonProperty("destinationSystem_DestinationNodeId")]
        public int DestinationSystemDestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets officialSystem .
        /// </summary>
        /// <value>
        ///  Sets officialSystem .
        /// </value>
        [JsonProperty("officialSystem")]
        public int OfficialSystem { get; set; }
    }
}
