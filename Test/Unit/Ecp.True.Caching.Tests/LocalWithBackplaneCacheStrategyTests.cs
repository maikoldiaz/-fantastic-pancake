// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalWithBackplaneCacheStrategyTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Caching.Tests
{
    using System;
    using Ecp.True.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using StackExchange.Redis;

    /// <summary>
    /// Local with backplane cache strategy tests.
    /// </summary>
    [TestClass]
    public class LocalWithBackplaneCacheStrategyTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger> mockLogger;

        /// <summary>
        /// The mock repository.
        /// </summary>
        private MockRepository mockRepository;

        /// <summary>
        /// The no cache strategy.
        /// </summary>
        private LocalWithBackplaneCacheStrategy localWithBackplaneCacheStrategy;

        /// <summary>
        /// The initialize.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            var testData = "Test Redis Connection string";
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockLogger = this.mockRepository.Create<ITrueLogger>();
            this.localWithBackplaneCacheStrategy = new LocalWithBackplaneCacheStrategy(this.mockLogger.Object, testData, TimeSpan.MaxValue, true);
        }

        /// <summary>
        /// The local with backplane configuration.
        /// </summary>
        [TestMethod]
        public void LocalWithBackplaneConfiguration_WhenInvoked()
        {
            // Arrange
            var mockMultiplexer = new Mock<IConnectionMultiplexer>();

            mockMultiplexer.Setup(_ => _.Configuration).Returns("LocalWithBackplane");

            // Act
            var result = this.localWithBackplaneCacheStrategy.CacheConfiguration;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasBackplane);
            Assert.IsTrue(result.CacheHandleConfigurations[1].IsBackplaneSource);
            Assert.IsTrue(result.CacheHandleConfigurations[1].Key == "trueredis");
        }
    }
}