// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlConnectionSettings.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Configuration
{
    /// <summary>
    /// The SQL connection configuration.
    /// </summary>
    public class SqlConnectionSettings
    {
        /// <summary>
        /// Gets or sets the service bus address.
        /// </summary>
        /// <value>
        /// The service bus address.
        /// </value>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets the database URL.
        /// </summary>
        /// <value>
        /// The database URL.
        /// </value>
        public string ResourceUrl => $"https://database.windows.net/";

        /// <summary>
        /// Gets or sets the tenant identifier.
        /// </summary>
        /// <value>
        /// The tenant identifier.
        /// </value>
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the name of the queue.
        /// </summary>
        /// <value>
        /// The name of the queue.
        /// </value>
        public int MaxRetryCount { get; set; }

        /// <summary>
        /// Gets or sets the retry interval in secs.
        /// </summary>
        /// <value>
        /// The retry interval in secs.
        /// </value>
        public int RetryIntervalInSecs { get; set; }

        /// <summary>
        /// Gets or sets the command timeout in secs.
        /// </summary>
        /// <value>
        /// The command timeout in secs.
        /// </value>
        public int? CommandTimeoutInSecs { get; set; }
    }
}
