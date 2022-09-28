// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceBusMessagingConfig.cs" company="Microsoft">
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
    /// The service bus messaging configuration.
    /// </summary>
    public class ServiceBusMessagingConfig
    {
        /// <summary>
        /// Gets or sets the service bus address.
        /// </summary>
        /// <value>
        /// The service bus address.
        /// </value>
        public string ServiceBusAddress { get; set; }

        /// <summary>
        /// Gets or sets the name of the queue.
        /// </summary>
        /// <value>
        /// The name of the queue.
        /// </value>
        public string QueueName { get; set; }

        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        /// <value>
        /// The name of the entity.
        /// </value>
        public string EntityName { get; set; }

        /// <summary>
        /// Gets the entity key.
        /// </summary>
        /// <value>
        /// The entity key.
        /// </value>
        public string EntityKey => $"{this.ServiceBusAddress}_{this.EntityName}";
    }
}
