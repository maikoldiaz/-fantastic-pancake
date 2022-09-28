// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceBusSettings.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Configuration
{
    /// <summary>
    /// The integration service bus settings.
    /// </summary>
    public class ServiceBusSettings
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets the tenant identifier.
        /// </summary>
        /// <value>
        /// The tenant identifier.
        /// </value>
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the name of the excel queue.
        /// </summary>
        /// <value>
        /// The name of the excel queue.
        /// </value>
        public string ExcelQueueName { get; set; }

        /// <summary>
        /// Gets or sets the name of the validate inventory queue.
        /// </summary>
        /// <value>
        /// The name of the validate inventory queue.
        /// </value>
        public string ValidatedInventoryQueueName { get; set; }

        /// <summary>
        /// Gets or sets the name of the validate movement queue.
        /// </summary>
        /// <value>
        /// The name of the validate movement queue.
        /// </value>
        public string ValidatedMovementQueueName { get; set; }

        /// <summary>
        /// Gets or sets the minimum back off.
        /// </summary>
        /// <value>
        /// The minimum back off.
        /// </value>
        public int MinimumBackOff { get; set; }

        /// <summary>
        /// Gets or sets the maximum back off.
        /// </summary>
        /// <value>
        /// The maximum back off.
        /// </value>
        public int MaximumBackOff { get; set; }

        /// <summary>
        /// Gets or sets the maximum retry count.
        /// </summary>
        /// <value>
        /// The maximum retry count.
        /// </value>
        public int MaximumRetryCount { get; set; }

        /// <summary>
        /// Gets the resource URI.
        /// </summary>
        /// <value>
        /// The resource URI.
        /// </value>
        public string Resource => $"sb://{this.Namespace}.servicebus.windows.net/";
    }
}
