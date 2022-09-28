// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CutOffBalanceRetryHandler.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Balance.Retry
{
    using System;
    using Ecp.True.Core;
    using Ecp.True.Ioc.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Balance.Interfaces;

    /// <summary>
    /// Base Retry Handler.
    /// </summary>
    /// <seealso cref="Ecp.True.ExceptionHandling.Core.IRetryHandler" />
    public class CutOffBalanceRetryHandler : ICutOffBalanceRetryHandler
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly IResolver resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="CutOffBalanceRetryHandler"/> class.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        public CutOffBalanceRetryHandler(IResolver resolver)
        {
            this.resolver = resolver;
        }

        /// <inheritdoc />
        public string HandlerType => "CutOffBalanceRetryHandler";

        private ITrueLogger<CutOffBalanceRetryHandler> Logger => this.resolver.GetInstance<ITrueLogger<CutOffBalanceRetryHandler>>();

        /// <inheritdoc />
        public bool IsFaultyResponse(object response)
        {
            return false;
        }

        /// <inheritdoc />
        public bool IsTransientFault(Exception exception)
        {
            if (exception == null)
            {
                return false;
            }

            this.Logger.LogError(exception, $"Retrying as there was exception {exception.Message}.", Constants.CutOffRetryFailed);
            return true;
        }
    }
}
