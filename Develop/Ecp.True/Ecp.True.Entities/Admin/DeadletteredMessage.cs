// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeadletteredMessage.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Admin
{
    using Ecp.True.Entities.Core;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The DeadletteredMessage.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class DeadletteredMessage : Entity
    {
        /// <summary>
        /// Gets or sets the deadlettered message identifier.
        /// </summary>
        /// <value>
        /// The deadlettered message identifier.
        /// </value>
        public int DeadletteredMessageId { get; set; }

        /// <summary>
        /// Gets or sets the BLOB path.
        /// </summary>
        /// <value>
        /// The BLOB path.
        /// </value>
        public string BlobPath { get; set; }

        /// <summary>
        /// Gets or sets the name of the process.
        /// </summary>
        /// <value>
        /// The name of the process.
        /// </value>
        public string ProcessName { get; set; }

        /// <summary>
        /// Gets or sets the name of the queue.
        /// </summary>
        /// <value>
        /// The name of the queue.
        /// </value>
        public string QueueName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DeadletteredMessage"/> is status.
        /// </summary>
        /// <value>
        ///   <c>true</c> if status; otherwise, <c>false</c>.
        /// </value>
        public bool Status { get; set; }

        /// <summary>
        /// Gets or sets the name of the process.
        /// </summary>
        /// <value>
        /// The name of the process.
        /// </value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is session enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is session enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsSessionEnabled { get; set; }

        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        public int? TicketId { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public JObject Content { get; set; }
    }
}
