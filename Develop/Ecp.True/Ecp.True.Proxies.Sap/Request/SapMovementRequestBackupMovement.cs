// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapMovementRequestBackupMovement.cs" company="Microsoft">
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
    /// The SAP PO Backup Movement class.
    /// </summary>
    public class SapMovementRequestBackupMovement
    {
        /// <summary>
        /// Gets or sets the movement identifier.
        /// </summary>
        /// <value>
        /// The movement identifier.
        /// </value>
        [JsonProperty("backupMovementId")]
        public string BackupMovementId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether movement belongs to official transfer point.
        /// </summary>
        /// <value>
        /// The is official field.
        /// </value>
        [JsonProperty("isOfficial")]
        public bool? IsOfficial { get; set; }

        /// <summary>
        /// Gets or sets the global movementId.
        /// </summary>
        /// <value>
        /// The global movementId.
        /// </value>
        [JsonProperty("globalMovementId")]
        public string GlobalMovementId { get; set; }
    }
}
