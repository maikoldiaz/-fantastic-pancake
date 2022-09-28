// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheBootstrapperTests.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using StackExchange.Redis;

    /// <summary>
    /// cache bootstrapper tests.
    /// </summary>
    [TestClass]
    public class CacheBootstrapperTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<CacheBootstrapper>> mockLogger;

        /// <summary>
        /// The mock repository.
        /// </summary>
        private MockRepository mockRepository;

        /// <summary>
        /// The cache boot strapper.
        /// </summary>
        private CacheBootstrapper cacheBootstrapper;

        /// <summary>
        /// The initialize.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockLogger = this.mockRepository.Create<ITrueLogger<CacheBootstrapper>>();
            this.cacheBootstrapper = new CacheBootstrapper(this.mockLogger.Object);
        }

        /// <summary>
        /// Checks local with back plane strategy.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task CheckStrategyExists_ShouldReturnLocalWithBackplaneStrategy_WhenInvokedAsync()
        {
            // Arrange
            var testData = "Test Redis Connection string";

            var mockMultiplexer = new Mock<IConnectionMultiplexer>();

            mockMultiplexer.Setup(_ => _.Configuration).Returns("LocalWithBackplane");

            // Act
            this.cacheBootstrapper.Setup(CacheMode.LocalWithBackplane, TimeSpan.MinValue, testData, true);
            var result = await Task.Run(() => this.cacheBootstrapper.CacheStrategy.CacheConfiguration).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasBackplane);
            Assert.IsTrue(result.CacheHandleConfigurations[1].IsBackplaneSource);
            Assert.IsTrue(result.CacheHandleConfigurations[1].Key == "trueredis");
        }

        /// <summary>
        /// Checks local only cache strategy.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task CheckStrategyExists_ShouldReturnLocalOnlyCacheStrategy_WhenInvokedAsync()
        {
            // Arrange
            var testData = "Test Redis Connection string";

            var mockMultiplexer = new Mock<IConnectionMultiplexer>();

            mockMultiplexer.Setup(_ => _.Configuration).Returns("LocalOnly");

            // Act
            this.cacheBootstrapper.Setup(CacheMode.LocalOnly, TimeSpan.MinValue, testData, true);
            var result = await Task.Run(() => this.cacheBootstrapper.CacheStrategy.CacheConfiguration).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.CacheHandleConfigurations[0].Key == "truememory");
        }

        /// <summary>
        /// Checks distributed only cache strategy.
        /// </summary>
        [TestMethod]
        public void CheckStrategyExists_ShouldReturnDistributedOnlyCacheStrategy_WhenInvoked()
        {
            // Arrange
            var testData = "Test Redis Connection string";

            var mockMultiplexer = new Mock<IConnectionMultiplexer>();

            mockMultiplexer.Setup(_ => _.Configuration).Returns("DistributedOnly");

            // Act
            this.cacheBootstrapper.Setup(CacheMode.DistributedOnly, TimeSpan.MinValue, testData, true);
            var result = this.cacheBootstrapper.CacheStrategy.CacheConfiguration;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.HasBackplane);
            Assert.IsTrue(result.CacheHandleConfigurations[0].Key == "trueredis");
        }

        /// <summary>
        /// Checks no strategy exist.
        /// </summary>
        [TestMethod]
        public void CheckNoStrategyExists_WhenInvoked()
        {
            // Arrange
            var testData = "Test Redis Connection string";

            var mockMultiplexer = new Mock<IConnectionMultiplexer>();

            mockMultiplexer.Setup(_ => _.Configuration).Returns("NoStrategy");

            // Act
            this.cacheBootstrapper.Setup(CacheMode.NoCache, TimeSpan.MinValue, testData, true);
            var result = this.cacheBootstrapper.CacheStrategy;

            // Assert
            Assert.IsNull(result.CacheConfiguration);
        }
    }
}