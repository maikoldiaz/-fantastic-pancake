// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceBusHelper.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.Utils
{
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Text;
    using System.Threading.Tasks;

    using global::Bdd.Core.Utils;

    using Microsoft.Azure.ServiceBus;

    public static class ServiceBusHelper
    {
        private static readonly NameValueCollection Settings = ConfigurationManager.GetSection("serviceBus") as NameValueCollection;

        private static string DefaultKeyPrefix => ConfigurationManager.AppSettings[nameof(DefaultKeyPrefix)];

        public static async Task PutAsync(string queueName, string messageBody, string label = null, string keyPrefix = null)
        {
            var connectionString = Settings.GetValue("ConnectionStringKey", string.IsNullOrWhiteSpace(keyPrefix) ? DefaultKeyPrefix : keyPrefix);
            var sbConnectionString = (await KeyVaultHelper.GetKeyVaultSecretAsync(connectionString).ConfigureAwait(false)).Value;
            var queueClient = new QueueClient(sbConnectionString, queueName);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));
            message.Label = label;
            await queueClient.SendAsync(message).ConfigureAwait(false);
        }
    }
}