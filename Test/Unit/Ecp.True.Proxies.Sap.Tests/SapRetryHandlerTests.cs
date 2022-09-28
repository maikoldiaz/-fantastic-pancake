// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapRetryHandlerTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.Sap.Tests
{
    using System;
    using System.Net.Http;
    using Ecp.True.Ioc.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Sap.Retry;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The sap retry handler tests.
    /// </summary>
    [TestClass]
    public class SapRetryHandlerTests
    {
        /// <summary>
        /// The mock resolver.
        /// </summary>
        private Mock<IResolver> resolver;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<SapRetryHandler>> logger;

        /// <summary>
        /// Intializes the method.
        /// </summary>
        [TestInitialize]
        public void IntializeMethod()
        {
            this.resolver = new Mock<IResolver>();
            this.logger = new Mock<ITrueLogger<SapRetryHandler>>();
        }

        /// <summary>
        /// Handlers the type should return ethereum retry handler.
        /// </summary>
        [TestMethod]
        public void HandlerType_ShouldReturn_SapRetryHandler()
        {
            var handler = new SapRetryHandler(this.resolver.Object);
            Assert.AreEqual("SapRetryHandler", handler.HandlerType);
        }

        /// <summary>
        /// Determines whether [is faulty response should return false when invoked].
        /// </summary>
        [TestMethod]
        public void IsFaultyResponse_ShouldReturnFalse_WhenInvoked()
        {
            this.resolver.Setup(x => x.GetInstance<ITrueLogger<SapRetryHandler>>()).Returns(this.logger.Object);
            var handler = new SapRetryHandler(this.resolver.Object);
            using (var response = this.GetSapResponse("{IsSuccessStatusCode: true}", System.Net.HttpStatusCode.OK))
            {
                Assert.IsFalse(handler.IsFaultyResponse(response));
            }
        }

        /// <summary>
        /// Determines whether [is faulty response should return true when invoked].
        /// </summary>
        [TestMethod]
        public void IsFaultyResponse_ShouldReturnTrue_WhenInvoked()
        {
            this.resolver.Setup(x => x.GetInstance<ITrueLogger<SapRetryHandler>>()).Returns(this.logger.Object);
            var handler = new SapRetryHandler(this.resolver.Object);
            using (var response = this.GetSapResponse("{IsSuccessStatusCode: false}", System.Net.HttpStatusCode.InternalServerError))
            {
                Assert.IsTrue(handler.IsFaultyResponse(response));
            }
        }

        /// <summary>
        /// Determines whether [is transient fault should return false on timeout exception].
        /// </summary>
        [TestMethod]
        public void IsTransientFault_ShouldReturnFalse_OnAnyOtherException()
        {
            var handler = new SapRetryHandler(this.resolver.Object);
            Assert.IsFalse(handler.IsTransientFault(null));
        }

        /// <summary>
        /// Determines whether [is transient fault should return true on timeout exception].
        /// </summary>
        [TestMethod]
        public void IsTransientFault_ShouldReturnTrue_OnAnyOtherException()
        {
            this.resolver.Setup(x => x.GetInstance<ITrueLogger<SapRetryHandler>>()).Returns(this.logger.Object);
            var handler = new SapRetryHandler(this.resolver.Object);
            Assert.IsTrue(handler.IsTransientFault(new Exception()));
        }

        private HttpResponseMessage GetSapResponse(string content, System.Net.HttpStatusCode statusCode)
        {
            var response = new HttpResponseMessage();
            response.StatusCode = statusCode;
            response.Content = new StringContent(content);
            return response;
        }
    }
}
