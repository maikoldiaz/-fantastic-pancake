// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipNodeStatusType.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Enumeration
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// The ownership node status.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OwnershipNodeStatusType
    {
        /// <summary>
        /// The sent
        /// </summary>
        SENT = 1,

        /// <summary>
        /// The ownership
        /// </summary>
        OWNERSHIP = 2,

        /// <summary>
        /// The failed
        /// </summary>
        FAILED = 3,

        /// <summary>
        /// The locked
        /// </summary>
        LOCKED = 4,

        /// <summary>
        /// The unlocked
        /// </summary>
        UNLOCKED = 5,

        /// <summary>
        /// The publishing
        /// </summary>
        PUBLISHING = 6,

        /// <summary>
        /// The published
        /// </summary>
        PUBLISHED = 7,

        /// <summary>
        /// The submitforapproval
        /// </summary>
        SUBMITFORAPPROVAL = 8,

        /// <summary>
        /// The approved
        /// </summary>
        APPROVED = 9,

        /// <summary>
        /// The rejected
        /// </summary>
        REJECTED = 10,

        /// <summary>
        /// The reopened
        /// </summary>
        REOPENED = 11,

        /// <summary>
        /// The deltas
        /// </summary>
        DELTAS = 12,

        /// <summary>
        /// The RECONCILED
        /// </summary>
        RECONCILED = 13,

        /// <summary>
        /// The NOTRECONCILED
        /// </summary>
        NOTRECONCILED = 14,

        /// <summary>
        /// The CONCILIATIONFAILED
        /// </summary>
        CONCILIATIONFAILED = 15,
    }
}
