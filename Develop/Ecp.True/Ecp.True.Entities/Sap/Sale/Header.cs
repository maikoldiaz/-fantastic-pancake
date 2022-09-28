// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Header.cs" company="Microsoft">
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
    using System;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    /// <summary>
    /// The Header class.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class Header
    {
        /// <summary>
        /// Gets or sets the NumberOrder.
        /// </summary>
        /// <value>
        /// The NumberOrder.
        /// </value>
        [StringLength(10, ErrorMessage = SapConstants.NumberOrderLengthExceeded)]
        [Required(ErrorMessage = SapConstants.NumberOrderRequired)]
        [JsonProperty("NUMBERORDER")]
        public string NumberOrder { get; set; }

        /// <summary>
        /// Gets or sets the Date Order.
        /// </summary>
        /// <value>
        /// The Date Order.
        /// </value>
        [JsonProperty("DATEORDER")]
        public DateTime? DateOrder { get; set; }

        /// <summary>
        /// Gets or sets the Type Order.
        /// </summary>
        /// <value>
        /// The Type Order.
        /// </value>
        [JsonProperty("TYPEORDER")]
        public string TypeOrder { get; set; }

        /// <summary>
        /// Gets or sets the Organization Id.
        /// </summary>
        /// <value>
        /// The Organization Id.
        /// </value>
        [JsonProperty("ORGANIZATIONID")]
        public string OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the Owner.
        /// </summary>
        /// <value>
        /// The Owner.
        /// </value>
        [JsonProperty("OWNER")]
        public string Owner { get; set; }

        /// <summary>
        /// Gets or sets the Client Id.
        /// </summary>
        /// <value>
        /// The Client Id.
        /// </value>
        [JsonProperty("CLIENTID")]
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the Expedition Class.
        /// </summary>
        /// <value>
        /// The Expedition Class.
        /// </value>
        [JsonProperty("EXPEDITIONCLASS")]
        public string ExpeditionClass { get; set; }

        /// <summary>
        /// Gets or sets the Observations.
        /// </summary>
        /// <value>
        /// The Observations.
        /// </value>
        [JsonProperty("OBSERVATIONS")]
        public string Observations { get; set; }

        /// <summary>
        /// Gets or sets the Source Location.
        /// </summary>
        /// <value>
        /// The Source Location.
        /// </value>
        [JsonProperty("SOURCELOCATION")]
        public string SourceLocation { get; set; }

        /// <summary>
        /// Gets or sets the Status Credit.
        /// </summary>
        /// <value>
        /// The Status Credit.
        /// </value>
        [JsonProperty("STATUS_CREDIT")]
        public string CreditStatus { get; set; }

        /// <summary>
        /// Gets or sets the Description Status.
        /// </summary>
        /// <value>
        /// The Description Status.
        /// </value>
        [JsonProperty("DESCRIPTION_STATUS")]
        public string DescriptionStatus { get; set; }
    }
}
