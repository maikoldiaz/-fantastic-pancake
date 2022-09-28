// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheManagerLoggerFactoryTests.cs" company="Microsoft">
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
    using Ecp.True.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The cache manager logger factory tests.
    /// </summary>
    [TestClass]
    public class CacheManagerLoggerFactoryTests
    {
        /// <summary>
        /// The factory.
        /// </summary>
        private CacheManagerLoggerFactory factory;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<CacheManagerLoggerFactory>> mockLogger;

        /// <summary>
        /// The repository.
        /// </summary>
        private MockRepository repository;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.repository = new MockRepository(MockBehavior.Strict);
            this.mockLogger = this.repository.Create<ITrueLogger<CacheManagerLoggerFactory>>();

            this.factory = new CacheManagerLoggerFactory(this.mockLogger.Object);
        }

        /// <summary>
        /// Create logger should return cache manager logger.
        /// </summary>
        [TestMethod]
        public void CreateLogger_ShouldReturn_CacheManagerLogger()
        {
            var logger = this.factory.CreateLogger("category");

            Assert.IsNotNull(logger);
            Assert.IsInstanceOfType(logger, typeof(CacheManagerLogger));
        }

        /// <summary>
        /// Create logger should return cache manager logger.
        /// </summary>
        [TestMethod]
        public void CreateLoggerGeneric_ShouldReturn_CacheManagerLogger()
        {
            var logger = this.factory.CreateLogger<Tuple<int, int>>(Tuple.Create(1, 2));

            Assert.IsNotNull(logger);
            Assert.IsInstanceOfType(logger, typeof(CacheManagerLogger));
        }
    }
}
