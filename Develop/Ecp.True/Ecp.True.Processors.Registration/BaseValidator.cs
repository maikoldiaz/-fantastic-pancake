// <copyright file="BaseValidator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Balance.Operation
{
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Proxies.Azure;

    /// <summary>
    /// Base Validator.
    /// </summary>
    public class BaseValidator
    {
        /// <summary>
        /// The azure Client Factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseValidator" /> class.
        /// </summary>
        /// <param name="azureClientFactory">The azure client factory.</param>
        public BaseValidator(
            IAzureClientFactory azureClientFactory)
        {
            this.azureClientFactory = azureClientFactory;
        }

        /// <summary>
        /// Uploads to queue asynchronous.
        /// </summary>
        /// <param name="fileRegistrationTransaction">The fileregistration transaction.</param>
        /// <param name="queueName">The queue name.</param>
        /// <returns>The Task.</returns>
        protected async Task UploadToQueueAsync(FileRegistrationTransaction fileRegistrationTransaction, string queueName)
        {
            ArgumentValidators.ThrowIfNull(fileRegistrationTransaction, nameof(fileRegistrationTransaction));

            var client = this.azureClientFactory.GetQueueClient(queueName);
            await client.QueueSessionMessageAsync(fileRegistrationTransaction, fileRegistrationTransaction.SessionId).ConfigureAwait(false);
        }
    }
}
