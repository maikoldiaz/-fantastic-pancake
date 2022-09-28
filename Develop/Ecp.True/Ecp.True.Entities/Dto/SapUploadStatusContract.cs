// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapUploadStatusContract.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Dto
{
    using System;
    using System.Globalization;
    using Ecp.True.Core;
    using Ecp.True.Entities.Query;
    using Newtonsoft.Json;

    /// <summary>
    /// class SapUploadStatusContract.
    /// </summary>
    public class SapUploadStatusContract
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SapUploadStatusContract" /> class.
        /// </summary>
        /// <param name="contract">contract.</param>
        /// <param name="destinationSystem">destinationSystem.</param>
        public SapUploadStatusContract(SapUploadContract contract, string destinationSystem)
        {
            ArgumentValidators.ThrowIfNull(contract, nameof(contract));
            this.SourceSystem = "TRUE";
            this.DestinationSystem = destinationSystem;
            this.FileRegistrationId = contract.FileRegistrationId;
            this.OriginMessageId = contract.OriginMessageId;
            this.TransactionId = Convert.ToString(contract?.TransactionId, CultureInfo.InvariantCulture);
            this.ProcessingTime = contract.ProcessingTime;
            this.StatusMessage = contract.StatusMessage;
            this.Information = contract.Information;
            this.OrderId = Convert.ToString(contract?.OrderId, CultureInfo.InvariantCulture);
            this.SourceTypeId = contract.SourceTypeId;
        }

        /// <summary>
        /// Gets or sets the File Registration Id.
        /// </summary>
        /// <value>
        /// The  File Registration Id.
        /// </value>
        [JsonIgnore]
        public int FileRegistrationId { get; set; }

        /// <summary>
        /// Gets or sets the Source System.
        /// </summary>
        /// <value>
        /// The Source System.
        /// </value>
        public string SourceSystem { get; set; }

        /// <summary>
        /// Gets or sets the Destination System.
        /// </summary>
        /// <value>
        /// The Destination System.
        /// </value>
        public string DestinationSystem { get; set; }

        /// <summary>
        /// Gets or sets the Origin Message ID.
        /// </summary>
        /// <value>
        /// The  Origin Message ID.
        /// </value>
        [JsonProperty("OriginMessageID")]
        public string OriginMessageId { get; set; }

        /// <summary>
        /// Gets or sets Transaction ID.
        /// </summary>
        /// <value>
        /// The Transaction ID.
        /// </value>
        [JsonProperty("TransactionID")]
        public string TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the Processing Time.
        /// </summary>
        /// <value>
        /// The Processing Time.
        /// </value>
        public string ProcessingTime { get; set; }

        /// <summary>
        /// Gets or sets the Status Message.
        /// </summary>
        /// <value>
        /// The Status Message.
        /// </value>
        public string StatusMessage { get; set; }

        /// <summary>
        /// Gets or sets the Information.
        /// </summary>
        /// <value>
        /// The Information.
        /// </value>
        public string Information { get; set; }

        /// <summary>
        /// Gets or sets the Order ID.
        /// </summary>
        /// <value>
        /// The Order ID.
        /// </value>
        [JsonProperty("OrderID")]
        public string OrderId { get; set; }

        /// <summary>
        /// Gets or sets the Source Type Id.
        /// </summary>
        /// <value>
        /// The  Source Type Id.
        /// </value>
        [JsonProperty("IDWricef")]
        public string SourceTypeId { get; set; }
    }
}
