// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapLogisticDto.cs" company="Microsoft">
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
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    /// <summary>
    /// The Logistic Movement Request.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class SapLogisticDto
    {
        /// <summary>
        /// Gets or Sets the SourceSystem.
        /// </summary>
        [JsonProperty("SOURCESYSTEM")]
        public string SourceSystem { get; set; }

        /// <summary>
        /// Gets or Sets the DestinationSystem.
        /// </summary>
        [JsonProperty("DESTINATIONSYSTEM")]
        public string DestinationSystem { get; set; }

        /// <summary>
        /// Gets or Sets the EventType.
        /// </summary>
        [JsonProperty("EVENTTYPE")]
        public string EventType { get; set; }

        /// <summary>
        /// Gets or Sets the MovementId.
        /// </summary>
        [JsonProperty("MOVEMENTID")]
        public string MovementId { get; set; }

        /// <summary>
        /// Gets or Sets the Order.
        /// </summary>
        [JsonProperty("ORDER")]
        [Required]
        public SapLogisticOrder Order { get; set; }

        /// <summary>
        /// Gets or Sets the MovementTypeId.
        /// </summary>
        [JsonProperty("MOVEMENTTYPEID")]
        public string MovementTypeId { get; set; }

        /// <summary>
        /// Gets or Sets the OperationalDate.
        /// </summary>
        [JsonProperty("OPERATIONALDATE")]
        public string OperationalDate { get; set; }

        /// <summary>
        /// Gets or Sets the Period.
        /// </summary>
        [JsonProperty("PERIOD")]
        [Required]
        public SapLogisticPeriod Period { get; set; }

        /// <summary>
        /// Gets or Sets the MovementSource.
        /// </summary>
        [JsonProperty("MOVEMENTSOURCE")]
        [Required]
        public SapLogisticSource MovementSource { get; set; }

        /// <summary>
        /// Gets or Sets the MovementDestination.
        /// </summary>
        [JsonProperty("MOVEMENTDESTINATION")]
        [Required]
        public SapLogisticDestination MovementDestination { get; set; }

        /// <summary>
        /// Gets or Sets the attribute.
        /// </summary>
        [JsonProperty("ATTRIBUTES")]
        public SapLogisticAttributeObject Attribute { get; set; }

        /// <summary>
        /// Gets or Sets the NetStandardQuantity.
        /// </summary>
        [JsonProperty("NETSTANDARDQUANTITY")]
        public string NetStandardQuantity { get; set; }

        /// <summary>
        /// Gets or Sets the MeasurementUnit.
        /// </summary>
        [JsonProperty("MEASUREMENTUNIT")]
        public string MeasurementUnit { get; set; }

        /// <summary>
        /// Gets or Sets the PoNumber.
        /// </summary>
        [JsonProperty("PO_NUMBER")]
        public string PoNumber { get; set; }

        /// <summary>
        /// Gets or Sets the PoItem.
        /// </summary>
        [JsonProperty("PO_ITEM")]
        public string PoItem { get; set; }

        /// <summary>
        /// Gets or Sets the SalesOrd.
        /// </summary>
        [JsonProperty("SALES_ORD")]
        public string SalesOrd { get; set; }

        /// <summary>
        /// Gets or Sets the PositionOrd.
        /// </summary>
        [JsonProperty("POSITIONORD")]
        public string PositionOrd { get; set; }

        /// <summary>
        /// Gets or Sets the ConstCenter.
        /// </summary>
        [JsonProperty("COSTCENTER")]
        public string ConstCenter { get; set; }

        /// <summary>
        /// Gets or Sets the TransactionCode.
        /// </summary>
        [JsonProperty("TRANSACTIONCODE")]
        public string TransactionCode { get; set; }

        /// <summary>
        /// Gets or Sets the BkTxt.
        /// </summary>
        [JsonProperty("BKTXT")]
        public string ScenarioType { get; set; }
    }
}
