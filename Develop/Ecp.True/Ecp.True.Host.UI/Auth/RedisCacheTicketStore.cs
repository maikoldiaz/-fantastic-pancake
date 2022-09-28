// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RedisCacheTicketStore.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.UI.Auth
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.Extensions.Caching.Distributed;

    /// <summary>
    /// The redis cache ticket store.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Authentication.Cookies.ITicketStore" />
    public class RedisCacheTicketStore : ITicketStore
    {
        /// <summary>
        /// The key prefix.
        /// </summary>
        private const string KeyPrefix = "TrueAuthSessionStore-";

        /// <summary>
        /// The cache.
        /// </summary>
        private readonly IDistributedCache cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCacheTicketStore"/> class.
        /// </summary>
        /// <param name="cache">The cache.</param>
        public RedisCacheTicketStore(IDistributedCache cache)
        {
            this.cache = cache;
        }

        /// <summary>
        /// Store the identity ticket and return the associated key.
        /// </summary>
        /// <param name="ticket">The identity information to store.</param>
        /// <returns>
        /// The key that can be used to retrieve the identity later.
        /// </returns>
        public async Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var key = KeyPrefix + Guid.NewGuid().ToString();
            await this.RenewAsync(key, ticket).ConfigureAwait(false);
            return key;
        }

        /// <summary>
        /// Tells the store that the given identity should be updated.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="ticket">the ticket.</param>
        /// <returns>The task.</returns>
        public async Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));

            var options = new DistributedCacheEntryOptions();
            var expiresUtc = ticket.Properties.ExpiresUtc;
            if (expiresUtc.HasValue)
            {
                options.SetAbsoluteExpiration(expiresUtc.Value);
            }

            var val = SerializeToBytes(ticket);
            await this.cache.SetAsync(key, val, options).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves an identity from the store for the given key.
        /// </summary>
        /// <param name="key">The key associated with the identity.</param>
        /// <returns>
        /// The identity associated with the given key, or if not found.
        /// </returns>
        public async Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            var bytes = await this.cache.GetAsync(key).ConfigureAwait(false);
            return DeserializeFromBytes(bytes);
        }

        /// <summary>
        /// Remove the identity associated with the given key.
        /// </summary>
        /// <param name="key">The key associated with the identity.</param>
        /// <returns>The task.</returns>
        public async Task RemoveAsync(string key)
        {
            await this.cache.RemoveAsync(key).ConfigureAwait(false);
        }

        private static byte[] SerializeToBytes(AuthenticationTicket source)
        {
            return TicketSerializer.Default.Serialize(source);
        }

        private static AuthenticationTicket DeserializeFromBytes(byte[] source)
        {
            return source == null ? null : TicketSerializer.Default.Deserialize(source);
        }
    }
}
