// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EthereumRetryHandler.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Blockchain
{
    using System;

    using Ecp.True.Blockchain.SetUp;
    using Ecp.True.ExceptionHandling.Core;

    using Nethereum.JsonRpc.Client;

    /// <summary>
    /// The Ethereum Retry Handler.
    /// </summary>
    public class EthereumRetryHandler : IRetryHandler
    {
        /// <inheritdoc />
        public string HandlerType => "EthereumRetryHandler";

        /// <inheritdoc />
        public bool IsFaultyResponse(object response)
        {
            return false;
        }

        /// <inheritdoc />
        public bool IsTransientFault(Exception exception)
        {
            ArgumentValidators.ThrowIfNull(exception, nameof(exception));

            if ((exception is RpcResponseException &&
                !string.IsNullOrEmpty(exception.Message) &&
                exception.Message.Contains("replacement transaction underpriced", StringComparison.OrdinalIgnoreCase))
                || exception.Message.Contains("timeout", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}
