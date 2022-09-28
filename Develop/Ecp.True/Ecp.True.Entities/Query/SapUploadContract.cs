// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapUploadContract.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Query
{
    using Newtonsoft.Json;

    /// <summary>
    /// The SAP upload class.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Query.QueryEntity" />
    public class SapUploadContract : QueryEntity
    {
        /// <summary>
        /// Gets or sets the File Registration Id.
        /// </summary>
        /// <value>
        /// The  File Registration Id.
        /// </value>
        public int FileRegistrationId { get; set; }

        /// <summary>
        /// Gets or sets Transaction ID.
        /// </summary>
        /// <value>
        /// The Transaction ID.
        /// </value>
        [JsonProperty("TransactionID")]
        public int? TransactionId { get; set; }

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
        /// Gets or sets the Origin Message Id.
        /// </summary>
        /// <value>
        /// The Origin MessageId.
        /// </value>
        public string OriginMessageId { get; set; }

        /// <summary>
        /// Gets or sets the Order ID.
        /// </summary>
        /// <value>
        /// The Order ID.
        /// </value>
        [JsonProperty("OrderID")]
        public int? OrderId { get; set; }

        /// <summary>
        /// Gets or sets the Source Type Id.
        /// </summary>
        /// <value>
        /// The Source Type Id.
        /// </value>
        [JsonProperty("IDWricef")]
        public string SourceTypeId { get; set; }
    }
}
