// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditLog.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities
{
    using System;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The audit log entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class AuditLog : Entity
    {
        /// <summary>
        /// Gets or sets the audit identifier.
        /// </summary>
        /// <value>
        /// The audit identifier.
        /// </value>
        public int AuditLogId { get; set; }

        /// <summary>
        /// Gets or sets the old value.
        /// </summary>
        /// <value>
        /// The old value.
        /// </value>
        public string OldValue { get; set; }

        /// <summary>
        ///  Gets or sets the new value.
        /// </summary>
        /// <value>
        /// The new value.
        /// </value>
        public string NewValue { get; set; }

        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        /// <value>
        /// The field.
        /// </value>
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the entity.
        /// </summary>
        /// <value>
        /// The entity.
        /// </value>
        public string Entity { get; set; }

        /// <summary>
        /// Gets or sets the Identity.
        /// </summary>
        /// <value>
        /// The Identity.
        /// </value>
        public string Identity { get; set; }

        /// <summary>
        /// Gets or sets the log date.
        /// </summary>
        /// <value>
        /// The log date.
        /// </value>
        public DateTime LogDate { get; set; }

        /// <summary>
        /// Gets or sets the type of the log.
        /// </summary>
        /// <value>
        /// The type of the log.
        /// </value>
        public string LogType { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the entity entry.
        /// </summary>
        /// <value>
        /// The entity entry.
        /// </value>
        public object EntityEntry { get; set; }
    }
}
