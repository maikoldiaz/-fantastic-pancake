// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RedisConnectionManager.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Caching
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Ecp.True.Logging;
    using StackExchange.Redis;

    /// <summary>
    /// The REDIS connection manager.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class RedisConnectionManager
    {
        // Need to think of something can make the pool size dynamically adjusted based on load
        private const int PoolSize = 10;

        /// <summary>
        /// The connections.
        /// </summary>
        private static readonly ConcurrentDictionary<string, Lazy<IConnectionMultiplexer>[]> Connections = new ConcurrentDictionary<string, Lazy<IConnectionMultiplexer>[]>();

        /// <summary>
        /// Gets the multiplexer.
        /// </summary>
        /// <param name="redisConnectionString">The redis connection string.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>The connection multiplexer.</returns>
        public static IConnectionMultiplexer GetMultiplexer(string redisConnectionString, ITrueLogger logger)
        {
            var pool = Connections.GetOrAdd(redisConnectionString, key =>
            {
                return Enumerable.Range(0, PoolSize).Select(_ => new Lazy<IConnectionMultiplexer>(() =>
                {
                    var rc = ConfigurationOptions.Parse(key);
                    rc.ConnectTimeout = (int)Constants.RedisConnectTimeOut.TotalMilliseconds;
                    rc.SyncTimeout = (int)Constants.RedisSyncTimeOut.TotalMilliseconds;
                    rc.AbortOnConnectFail = false;
                    rc.ConnectRetry = Constants.RedisMaxRetry;

                    var connectionMux = ConnectionMultiplexer.Connect(rc);

                    connectionMux.ConnectionRestored += (sender, args) =>
                    {
                        logger.LogInformation(
                            $"Connection restored, type: '{args.ConnectionType}', failure: '{args.FailureType}', exception: {args.Exception}",
                            Constants.CacheTag);
                    };

                    connectionMux.ConnectionFailed += (sender, args) =>
                    {
                        logger.LogError($"Connection failed, type: '{args.ConnectionType}', failure: '{args.FailureType}', exception: {args.Exception}", Constants.CacheTag);
                    };

                    connectionMux.InternalError += (sender, args) =>
                    {
                        logger.LogError($"Internal error, type: '{args.ConnectionType}', origin: '{args.Origin}', exception: {args.Exception}", Constants.CacheTag);
                    };

                    connectionMux.ErrorMessage += (sender, args) =>
                    {
                        logger.LogError($"Server error message: '{args.Message}'", Constants.CacheTag);
                    };

                    return connectionMux;
                })).ToArray();
            });

            if (pool.Length == 1)
            {
                return pool.First().Value;
            }

            var notInitializedMux = pool.FirstOrDefault(m => !m.IsValueCreated);
            if (notInitializedMux != null)
            {
                return notInitializedMux.Value;
            }

            var loadMap = new Dictionary<int, long>();
            for (int i = 0; i < pool.Length; i++)
            {
                loadMap[i] = pool[i].Value.GetCounters().TotalOutstanding;
            }

            var leastLoadedIndex = loadMap.OrderBy(m => m.Value).First().Key;
            return pool[leastLoadedIndex].Value;
        }
    }
}
