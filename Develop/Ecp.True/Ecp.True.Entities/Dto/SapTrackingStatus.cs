// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapTrackingStatus.cs" company="Microsoft">
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
    using Newtonsoft.Json;

    /// <summary>
    /// The Sap Upload Dto.
    /// </summary>
    public class SapTrackingStatus
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SapTrackingStatus"/> class.
        /// </summary>
        /// <param name="processId">The process identifier.</param>
        public SapTrackingStatus(string processId)
        {
            this.ProcessId = processId;
        }

        /// <summary>
        /// Gets or sets the process identifier.
        /// </summary>
        /// <value>
        /// The process identifier.
        /// </value>
        public string ProcessId { get; set; }

        /// <summary>
        /// Gets or sets the document.
        /// </summary>
        /// <value>
        /// The document.
        /// </value>
        [JsonProperty("documents")]
        public SapDocument Document { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is success; otherwise, <c>false</c>.
        /// </value>
        [JsonIgnore]
        public bool IsSuccess { get; set; }
    }
}