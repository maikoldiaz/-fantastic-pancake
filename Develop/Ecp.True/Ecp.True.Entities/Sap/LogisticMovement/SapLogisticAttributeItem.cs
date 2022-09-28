// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapLogisticAttributeItem.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Sap
{
    using Newtonsoft.Json;

    /// <summary>
    /// The Logistic Attribute Request.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class SapLogisticAttributeItem
    {
        /// <summary>
        /// Gets or Sets the attribute id.
        /// </summary>
        [JsonProperty("ATTRIBUTEID")]
        public string AttributeId { get; set; }

        /// <summary>
        /// Gets or Sets the attribute type.
        /// </summary>
        [JsonProperty("ATTRIBUTETYPE")]
        public string AttributeType { get; set; }

        /// <summary>
        /// Gets or Sets the attribute value.
        /// </summary>
        [JsonProperty("ATTRIBUTEVALUE")]
        public double AttributeValue { get; set; }

        /// <summary>
        /// Gets or Sets the attribute unit.
        /// </summary>
        [JsonProperty("VALUEATTRIBUTEUNIT")]
        public string AttributeUnit { get; set; }

        /// <summary>
        /// Gets or Sets the attribute description.
        /// </summary>
        [JsonProperty("ATTRIBUTEDESCRIPTION")]
        public string AttributeDescription { get; set; }
    }
}
