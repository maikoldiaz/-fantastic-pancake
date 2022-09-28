// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EthereumRetryHandlerTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.Azure.Tests
{
    using System;
    using Ecp.True.Proxies.Azure.Retry;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Nethereum.JsonRpc.Client;

    /// <summary>
    /// The ethereum retry handler tests.
    /// </summary>
    [TestClass]
    public class EthereumRetryHandlerTests
    {
        /// <summary>
        /// The underpriced message.
        /// </summary>
        private readonly string underpricedMessage = "replacement transaction underpriced";

        /// <summary>
        /// The timeout message.
        /// </summary>
        private readonly string timeoutMessage = "blockchain transacation timeout";

        /// <summary>
        /// Handlers the type should return ethereum retry handler.
        /// </summary>
        [TestMethod]
        public void HandlerType_ShouldReturn_EthereumRetryHandler()
        {
            var handler = new EthereumRetryHandler();
            Assert.AreEqual("EthereumRetryHandler", handler.HandlerType);
        }

        /// <summary>
        /// Determines whether [is faulty response should return false when invoked].
        /// </summary>
        [TestMethod]
        public void IsFaultyResponse_ShouldReturnFalse_WhenInvoked()
        {
            var handler = new EthereumRetryHandler();
            Assert.IsFalse(handler.IsFaultyResponse("test"));
        }

        /// <summary>
        /// Determines whether [is transient fault should return true on gas underpriced exception].
        /// </summary>
        [TestMethod]
        public void IsTransientFault_ShouldReturnTrue_OnGasUnderpricedException()
        {
            var handler = new EthereumRetryHandler();
            var ex = new RpcResponseException(new RpcError(125, this.underpricedMessage));
            Assert.IsTrue(handler.IsTransientFault(ex));
        }

        /// <summary>
        /// Determines whether [is transient fault should return true on timeout exception].
        /// </summary>
        [TestMethod]
        public void IsTransientFault_ShouldReturnTrue_OnTimeoutException()
        {
            var handler = new EthereumRetryHandler();
            var ex = new RpcClientTimeoutException(this.timeoutMessage);
            Assert.IsTrue(handler.IsTransientFault(ex));
        }

        /// <summary>
        /// Determines whether [is transient fault should return true on timeout exception].
        /// </summary>
        [TestMethod]
        public void IsTransientFault_ShouldReturnFalse_OnAnyOtherException()
        {
            var handler = new EthereumRetryHandler();
            var ex = new Exception();
            Assert.IsFalse(handler.IsTransientFault(ex));
        }
    }
}
