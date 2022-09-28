// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapMovementRequestAttribute.cs" company="Microsoft">
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
    /// The SAP PO Attribute class.
    /// </summary>
    public class SapMovementRequestAttribute
    {
        /// <summary>
        /// Gets or sets the attribute identifier.
        /// </summary>
        /// <value>
        /// The attribute identifier.
        /// </value>
        [JsonProperty("attributeId")]
        public string AttributeId { get; set; }

        /// <summary>
        /// Gets or sets the attribute value.
        /// </summary>
        /// <value>
        /// The attribute value.
        /// </value>
        [JsonProperty("attributeType")]
        public string AttributeType { get; set; }

        /// <summary>
        /// Gets or sets the attribute value.
        /// </summary>
        /// <value>
        /// The attribute value.
        /// </value>
        [JsonProperty("attributeValue")]
        public string AttributeValue { get; set; }

        /// <summary>
        /// Gets or sets the value attribute unit.
        /// </summary>
        /// <value>
        /// The value attribute unit.
        /// </value>
        [JsonProperty("valueAttributeUnit")]
        public string ValueAttributeUnit { get; set; }

        /// <summary>
        /// Gets or sets the attribute description.
        /// </summary>
        /// <value>
        /// The attribute description.
        /// </value>
        [JsonProperty("attributeDescription")]
        public string AttributeDescription { get; set; }
    }
}
