// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MsalAppRedisTokenCacheProvider.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.UI.Auth.Token
{
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Options;
    using Microsoft.Identity.Web;
    using Microsoft.Identity.Web.TokenCacheProviders.Distributed;

    /// <summary>
    /// The MsalAppRedisTokenCacheProvider.
    /// </summary>
    public class MsalAppRedisTokenCacheProvider : MsalDistributedTokenCacheAdapter
    {
        /// <summary>
        /// The data protector.
        /// </summary>
        private readonly IDataProtector dataProtector;

        /// <summary>
        /// Initializes a new instance of the <see cref="MsalAppRedisTokenCacheProvider" /> class.
        /// </summary>
        /// <param name="microsoftIdentityOptions">The microsoft identity options.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        /// <param name="cacheOptions">The cache options.</param>
        /// <param name="protectionProvider">The data protector.</param>
        public MsalAppRedisTokenCacheProvider(
                                    IOptions<MicrosoftIdentityOptions> microsoftIdentityOptions,
                                    IHttpContextAccessor httpContextAccessor,
                                    IDistributedCache distributedCache,
                                    IOptions<DistributedCacheEntryOptions> cacheOptions,
                                    IDataProtectionProvider protectionProvider)
            : base(microsoftIdentityOptions, httpContextAccessor, distributedCache, cacheOptions)
        {
            ArgumentValidators.ThrowIfNull(protectionProvider, nameof(protectionProvider));
            this.dataProtector = protectionProvider.CreateProtector("MSAL");
        }

        /// <summary>
        /// Reads the cache bytes asynchronous.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>The task.</returns>
        protected override async Task<byte[]> ReadCacheBytesAsync(string cacheKey)
        {
            var cachedata = await base.ReadCacheBytesAsync(cacheKey).ConfigureAwait(false);
            return cachedata == null ? null : this.dataProtector.Unprotect(cachedata);
        }

        /// <summary>
        /// Writes the cache bytes asynchronous.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="bytes">The bytes.</param>
        /// <returns>The task.</returns>
        protected override async Task WriteCacheBytesAsync(string cacheKey, byte[] bytes)
        {
            var encryptedData = this.dataProtector.Protect(bytes);
            await base.WriteCacheBytesAsync(cacheKey, encryptedData).ConfigureAwait(false);
        }
    }
}
