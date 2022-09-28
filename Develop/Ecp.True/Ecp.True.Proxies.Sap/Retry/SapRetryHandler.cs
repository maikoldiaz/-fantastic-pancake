// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapRetryHandler.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.Sap.Retry
{
    using System;
    using System.Net.Http;
    using Ecp.True.Core;
    using Ecp.True.ExceptionHandling.Core;
    using Ecp.True.Ioc.Interfaces;
    using Ecp.True.Logging;

    /// <summary>
    /// Ownership Rule Retry Handler.
    /// </summary>
    /// <seealso cref="Ecp.True.ExceptionHandling.Core.IRetryHandler" />
    public class SapRetryHandler : IRetryHandler
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly IResolver resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="SapRetryHandler"/> class.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        public SapRetryHandler(IResolver resolver)
        {
            this.resolver = resolver;
        }

        /// <inheritdoc />
        public string HandlerType => "SapRetryHandler";

        private ITrueLogger<SapRetryHandler> Logger => this.resolver.GetInstance<ITrueLogger<SapRetryHandler>>();

        /// <inheritdoc />
        public bool IsFaultyResponse(object response)
        {
            try
            {
                var responseMessage = (HttpResponseMessage)response;
                ArgumentValidators.ThrowIfNull(responseMessage, nameof(responseMessage));
                this.Logger.LogInformation($"SAP returned results with status {responseMessage.StatusCode}.", Constants.SapSync);
                return !(responseMessage.IsSuccessStatusCode || (((int)responseMessage.StatusCode >= 400) && ((int)responseMessage.StatusCode <= 499)));
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Not retrying as there was exception {ex.Message}.", Constants.SapSync);
                return false;
            }
        }

        /// <inheritdoc />
        public bool IsTransientFault(Exception exception)
        {
            if (exception == null)
            {
                return false;
            }

            this.Logger.LogError(exception, $"Retrying as there was exception {exception.Message}.", Constants.SapSync);
            return true;
        }
    }
}
