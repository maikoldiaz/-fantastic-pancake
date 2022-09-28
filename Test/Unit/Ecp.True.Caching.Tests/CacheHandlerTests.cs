// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheHandlerTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Caching.Tests
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using CacheManager.Core;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The cache handler tests.
    /// </summary>
    [TestClass]
    public class CacheHandlerTests
    {
        /// <summary>
        /// The key.
        /// </summary>
        private const string Key = "CacheKey";

        /// <summary>
        /// The region.
        /// </summary>
        private const string Region = "CacheRegion";

        /// <summary>
        /// The repository.
        /// </summary>
        private MockRepository repository;

        /// <summary>
        /// The cache handler.
        /// </summary>
        private CacheHandler<int> cacheHandler;

        /// <summary>
        /// The mock bootstrapper.
        /// </summary>
        private Mock<ICacheBootstrapper> mockBootstrapper;

        /// <summary>
        /// The mock cache factory.
        /// </summary>
        private Mock<ICacheFactory> mockCacheFactory;

        /// <summary>
        /// The mock cache manager.
        /// </summary>
        private Mock<ICacheManager<int>> mockCacheManager;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.repository = new MockRepository(MockBehavior.Strict);

            this.mockBootstrapper = this.repository.Create<ICacheBootstrapper>();
            this.mockCacheFactory = this.repository.Create<ICacheFactory>();
            this.mockCacheManager = this.repository.Create<ICacheManager<int>>();

            this.mockBootstrapper.Setup(m => m.CacheStrategy.CacheConfiguration).Returns(new Mock<ICacheManagerConfiguration>().Object);
            this.mockCacheFactory.Setup(m => m.FromConfiguration<int>(It.IsAny<ICacheManagerConfiguration>())).Returns(this.mockCacheManager.Object);

            this.cacheHandler = new CacheHandler<int>(this.mockBootstrapper.Object, this.mockCacheFactory.Object);
        }

        /// <summary>
        /// Caches the handler should throw error when bootstrapper is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CacheHandler_ShouldThrowError_WhenBootstrapperIsNull()
        {
            this.cacheHandler = new CacheHandler<int>(null, this.mockCacheFactory.Object);
        }

        /// <summary>
        /// Caches the handler should throw error when factory is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CacheHandler_ShouldThrowError_WhenFactoryIsNull()
        {
            this.cacheHandler = new CacheHandler<int>(this.mockBootstrapper.Object, null);
        }

        /// <summary>
        /// Get should return value from cache when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetAsync_ShouldReturnValueFromCache_WhenInvokedAsync()
        {
            this.mockCacheManager.Setup(c => c.Get(Key, Region)).Returns(1);
            var result = await this.cacheHandler.GetAsync(Key, Region).ConfigureAwait(false);

            Assert.AreEqual(1, result);
            this.mockCacheManager.Verify(c => c.Get(Key, Region), Times.Once);
        }

        /// <summary>
        /// Set should set value in cache when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task SetAsync_ShouldSetValueInCache_WhenInvokedAsync()
        {
            this.mockCacheManager.Setup(c => c.Put(It.IsAny<CacheItem<int>>()));
            await this.cacheHandler.SetAsync(Key, 1, Region).ConfigureAwait(false);

            Expression<Func<CacheItem<int>, bool>> func = ck => ck.Key == Key &&
                                                                ck.Value == 1 &&
                                                                ck.Region == Region;
            this.mockCacheManager.Verify(c => c.Put(It.Is(func)), Times.Once);
        }

        /// <summary>
        /// Set should set default expiration mode and time out in cache when options are null asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task SetAsync_ShouldSetDefaultExpirationModeAndTimeOutInCache_WhenOptionsAreNullAsync()
        {
            this.mockCacheManager.Setup(c => c.Put(It.IsAny<CacheItem<int>>()));
            await this.cacheHandler.SetAsync(Key, 1, Region, null).ConfigureAwait(false);

            this.mockCacheManager.Verify(c => c.Put(It.IsAny<CacheItem<int>>()), Times.Once);
        }

        /// <summary>
        /// Set should set default expiration mode and time out in cache when expiration is relative to now asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task SetAsync_ShouldSetDefaultExpirationModeAbsoluteInCache_WhenExpirationIsRelativeToNowAsync()
        {
            this.mockCacheManager.Setup(c => c.Put(It.IsAny<CacheItem<int>>()));
            var expiration = TimeSpan.FromSeconds(20);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration,
            };

            await this.cacheHandler.SetAsync(Key, 1, Region, options).ConfigureAwait(false);

            Expression<Func<CacheItem<int>, bool>> func = ck => ck.Key == Key &&
                                                    ck.Value == 1 &&
                                                    ck.Region == Region &&
                                                    ck.ExpirationMode == ExpirationMode.Absolute &&
                                                    ck.ExpirationTimeout == expiration;

            this.mockCacheManager.Verify(c => c.Put(It.Is(func)), Times.Once);
        }

        /// <summary>
        /// Set should set default expiration mode and time out in cache when expriation is absolute asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task SetAsync_ShouldSetAbsoluteExpirationModeInCache_WhenExpirationIsAbsoluteAsync()
        {
            this.mockCacheManager.Setup(c => c.Put(It.IsAny<CacheItem<int>>()));
            var expiration = DateTimeOffset.Now.AddSeconds(20);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = expiration,
            };

            await this.cacheHandler.SetAsync(Key, 1, Region, options).ConfigureAwait(false);

            Expression<Func<CacheItem<int>, bool>> func = ck => ck.Key == Key &&
                                                    ck.Value == 1 &&
                                                    ck.Region == Region &&
                                                    ck.ExpirationMode == ExpirationMode.Absolute &&
                                                    ck.ExpirationTimeout.TotalSeconds <= 20;

            this.mockCacheManager.Verify(c => c.Put(It.Is(func)), Times.Once);
        }

        /// <summary>
        /// Set should set default expiration mode and time out in cache when expriation is absolute asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task SetAsync_ShouldSetSlidingExpirationModeInCache_WhenExpirationIsSlidingAsync()
        {
            this.mockCacheManager.Setup(c => c.Put(It.IsAny<CacheItem<int>>()));
            var expiration = TimeSpan.FromSeconds(20);
            var options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = expiration,
            };

            await this.cacheHandler.SetAsync(Key, 1, Region, options).ConfigureAwait(false);

            Expression<Func<CacheItem<int>, bool>> func = ck => ck.Key == Key &&
                                                    ck.Value == 1 &&
                                                    ck.Region == Region &&
                                                    ck.ExpirationMode == ExpirationMode.Sliding &&
                                                    ck.ExpirationTimeout == expiration;

            this.mockCacheManager.Verify(c => c.Put(It.Is(func)), Times.Once);
        }

        /// <summary>
        /// Remove should delete from cache when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task RemoveAsync_ShouldDeleteFromCache_WhenInvokedAsync()
        {
            this.mockCacheManager.Setup(c => c.Remove(Key, Region)).Returns(true);
            var result = await this.cacheHandler.DeleteAsync(Key, Region).ConfigureAwait(false);

            Assert.IsTrue(result);
            this.mockCacheManager.Verify(c => c.Remove(Key, Region), Times.Once);
        }
    }
}
