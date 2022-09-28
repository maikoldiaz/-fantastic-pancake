// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalOnlyCacheStrategyTests.cs" company="Microsoft">
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
    /// Local only cache strategy tests.
    /// </summary>
    [TestClass]
    public class LocalOnlyCacheStrategyTests
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
        /// The local only cache strategy.
        /// </summary>
        private LocalOnlyCacheStrategy localOnlyCacheStrategy;

        /// <summary>
        /// The initialize.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockLogger = this.mockRepository.Create<ITrueLogger>();
            this.localOnlyCacheStrategy = new LocalOnlyCacheStrategy(this.mockLogger.Object, TimeSpan.MaxValue, true);
        }

        /// <summary>
        /// The local only cache configuration..
        /// </summary>
        [TestMethod]
        public void LocalOnlyCacheConfiguration_WhenInvoked()
        {
            // Arrange
            var mockMultiplexer = new Mock<IConnectionMultiplexer>();

            mockMultiplexer.Setup(_ => _.Configuration).Returns("LocalOnly");

            // Act
            var result = this.localOnlyCacheStrategy.CacheConfiguration;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.CacheHandleConfigurations[0].Key == "truememory");
        }
    }
}