// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RedisTokenCacheProviderExtension.cs" company="Microsoft">
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
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Identity.Web.TokenCacheProviders;

    /// <summary>
    /// The RedisTokenCacheProviderExtension.
    /// </summary>
    public static class RedisTokenCacheProviderExtension
    {
        /// <summary>
        /// Adds both the app and per-user in-memory token caches.
        /// </summary>
        /// <param name="services">The services collection to add to.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="instanceName">Name of the instance.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        public static IServiceCollection AddRedisTokenCaches(
            this IServiceCollection services, string connectionString, string instanceName)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connectionString;
                options.InstanceName = instanceName;
            });

            AddRedisAppTokenCache(services);
            AddRedisUserTokenCache(services);
            return services;
        }

        /// <summary>
        /// Adds the in-memory based application token cache to the service collection.
        /// </summary>
        /// <param name="services">The services collection to add to.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddRedisAppTokenCache(
            this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSingleton<IMsalTokenCacheProvider, MsalAppRedisTokenCacheProvider>();
            return services;
        }

        /// <summary>
        /// Adds the in-memory based per user token cache to the service collection.
        /// </summary>
        /// <param name="services">The services collection to add to.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public static IServiceCollection AddRedisUserTokenCache(
            this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddHttpContextAccessor();
            services.AddSingleton<IMsalTokenCacheProvider, MsalAppRedisTokenCacheProvider>();
            return services;
        }
    }
}
