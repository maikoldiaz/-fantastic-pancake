﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapMovementRequestSource.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.Sap.Request
{
    using Newtonsoft.Json;

    /// <summary>
    /// The SAP PO Movement Source class.
    /// </summary>
    public class SapMovementRequestSource
    {
        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        [JsonProperty("sourceNodeId")]
        public string SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the source storage location identifier.
        /// </summary>
        /// <value>
        /// The source storage location identifier.
        /// </value>
        [JsonProperty("sourceStorageLocationId")]
        public string SourceStorageLocationId { get; set; }

        /// <summary>
        /// Gets or sets the source product identifier.
        /// </summary>
        /// <value>
        /// The source product identifier.
        /// </value>
        [JsonProperty("sourceProductId")]
        public string SourceProductId { get; set; }

        /// <summary>
        /// Gets or sets the source product type identifier.
        /// </summary>
        /// <value>
        /// The source product type identifier.
        /// </value>
        [JsonProperty("sourceProductTypeId")]
        public string SourceProductTypeId { get; set; }
    }
}
