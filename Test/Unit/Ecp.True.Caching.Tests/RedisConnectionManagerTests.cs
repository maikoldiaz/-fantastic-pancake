// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RedisConnectionManagerTests.cs" company="Microsoft">
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
    using Ecp.True.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using StackExchange.Redis;

    /// <summary>
    /// Redis connection cache manager tests.
    /// </summary>
    [TestClass]
    public class RedisConnectionManagerTests
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
        /// The initialize.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockLogger = this.mockRepository.Create<ITrueLogger>();
        }

        /// <summary>
        /// The redis manager gets the multiplexer.
        /// </summary>
        [TestMethod]
        public void Manager_GetMultiplexer_WhenInvoked()
        {
            // Arrange
            var testData = "Test Redis Connection string";

            this.mockLogger.Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<string>()));

            var mockMultiplexer = new Mock<IConnectionMultiplexer>();

            mockMultiplexer.Setup(_ => _.IsConnected).Returns(false);

            // Act
            var result = RedisConnectionManager.GetMultiplexer(testData, this.mockLogger.Object);

            // Assert
            Assert.IsFalse(result.IsConnected);
        }
    }
}