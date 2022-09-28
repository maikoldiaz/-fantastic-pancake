// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheManagerLoggerTests.cs" company="Microsoft">
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
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The cache manager logger tests.
    /// </summary>
    [TestClass]
    public class CacheManagerLoggerTests
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private MockRepository repository;

        /// <summary>
        /// The logger.
        /// </summary>
        private CacheManagerLogger logger;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger> mockLogger;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.repository = new MockRepository(MockBehavior.Strict);
            this.mockLogger = this.repository.Create<ITrueLogger>();

            this.logger = new CacheManagerLogger(this.mockLogger.Object);
        }

        /// <summary>
        /// Begins the scope should return null when invoked.
        /// </summary>
        [TestMethod]
        public void BeginScope_ShouldReturnNull_WhenInvoked()
        {
            var state = this.logger.BeginScope(null);
            Assert.IsNull(state);
        }

        /// <summary>
        /// Logger should be enabled for warning and above.
        /// </summary>
        [TestMethod]
        public void Logger_ShouldBeEnabledForWarningAndAbove()
        {
            var enabled = this.logger.IsEnabled(CacheManager.Core.Logging.LogLevel.Debug);
            Assert.IsFalse(enabled);

            enabled = this.logger.IsEnabled(CacheManager.Core.Logging.LogLevel.Information);
            Assert.IsFalse(enabled);

            enabled = this.logger.IsEnabled(CacheManager.Core.Logging.LogLevel.Trace);
            Assert.IsFalse(enabled);

            enabled = this.logger.IsEnabled(CacheManager.Core.Logging.LogLevel.Critical);
            Assert.IsTrue(enabled);

            enabled = this.logger.IsEnabled(CacheManager.Core.Logging.LogLevel.Warning);
            Assert.IsTrue(enabled);

            enabled = this.logger.IsEnabled(CacheManager.Core.Logging.LogLevel.Error);
            Assert.IsTrue(enabled);
        }

        /// <summary>
        /// Logs the should log exception event when exception occurs.
        /// </summary>
        [TestMethod]
        public void Log_ShouldLogExceptionEvent_WhenExceptionOccurs()
        {
            var message = "Test";
            var ex = new Exception();

            this.mockLogger.Setup(l => l.LogError(It.IsAny<EventId>(), It.IsAny<Exception>(), It.IsAny<string>()));

            this.logger.Log(CacheManager.Core.Logging.LogLevel.Critical, 1, message, ex);

            this.mockLogger.Verify(l => l.LogError(It.IsAny<EventId>(), It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Log should log verbose message when verbose event occurs.
        /// </summary>
        [TestMethod]
        public void Log_ShouldLogVerboseMessage_ForDebugLevel()
        {
            var message = "Test";
            this.mockLogger.Setup(l => l.Log(LogLevel.Debug, It.IsAny<EventId>(), message));

            this.logger.Log(CacheManager.Core.Logging.LogLevel.Debug, 1, message, null);

            this.mockLogger.Verify(l => l.Log(LogLevel.Debug, It.IsAny<EventId>(), message), Times.Once);
        }

        /// <summary>
        /// Log should log verbose message when trace event occurs.
        /// </summary>
        [TestMethod]
        public void Log_ShouldLogVerboseMessage_ForTraceLevel()
        {
            var message = "Test";
            this.mockLogger.Setup(l => l.Log(LogLevel.Trace, It.IsAny<EventId>(), message));

            this.logger.Log(CacheManager.Core.Logging.LogLevel.Trace, 1, message, null);

            this.mockLogger.Verify(l => l.Log(LogLevel.Trace, It.IsAny<EventId>(), message), Times.Once);
        }

        /// <summary>
        /// Log should log critical message when critical event occurs.
        /// </summary>
        [TestMethod]
        public void Log_ShouldLogCriticalMessage_ForCriticalLevel()
        {
            var message = "Test";
            this.mockLogger.Setup(l => l.Log(LogLevel.Critical, It.IsAny<EventId>(), message));

            this.logger.Log(CacheManager.Core.Logging.LogLevel.Critical, 1, message, null);

            this.mockLogger.Verify(l => l.Log(LogLevel.Critical, It.IsAny<EventId>(), message), Times.Once);
        }

        /// <summary>
        /// Log should log verbose message when verbose event occurs.
        /// </summary>
        [TestMethod]
        public void Log_ShouldLogErrorMessage_ForErrorLevel()
        {
            var message = "Test";
            this.mockLogger.Setup(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), message));

            this.logger.Log(CacheManager.Core.Logging.LogLevel.Error, 1, message, null);

            this.mockLogger.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), message), Times.Once);
        }

        /// <summary>
        /// Log should log verbose message when verbose event occurs.
        /// </summary>
        [TestMethod]
        public void Log_ShouldLogInformationalMessage_ForInformationLevel()
        {
            var message = "Test";
            this.mockLogger.Setup(l => l.Log(LogLevel.Information, It.IsAny<EventId>(), message));

            this.logger.Log(CacheManager.Core.Logging.LogLevel.Information, 1, message, null);

            this.mockLogger.Verify(l => l.Log(LogLevel.Information, It.IsAny<EventId>(), message), Times.Once);
        }

        /// <summary>
        /// Log should log warning message for warning level.
        /// </summary>
        [TestMethod]
        public void Log_ShouldLogWarningMessage_ForWarningLevel()
        {
            var message = "Test";
            this.mockLogger.Setup(l => l.Log(LogLevel.Warning, It.IsAny<EventId>(), message));

            this.logger.Log(CacheManager.Core.Logging.LogLevel.Warning, 1, message, null);

            this.mockLogger.Verify(l => l.Log(LogLevel.Warning, It.IsAny<EventId>(), message), Times.Once);
        }
    }
}
