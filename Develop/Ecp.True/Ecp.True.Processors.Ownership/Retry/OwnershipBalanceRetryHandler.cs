// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipBalanceRetryHandler.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Retry
{
    using System;
    using Ecp.True.Core;
    using Ecp.True.Ioc.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Ownership.Interfaces;

    /// <summary>
    /// Ownership Retry Handler.
    /// </summary>
    /// <seealso cref="Ecp.True.ExceptionHandling.Core.IRetryHandler" />
    public class OwnershipBalanceRetryHandler : IOwnershipBalanceRetryHandler
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly IResolver resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipBalanceRetryHandler"/> class.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        public OwnershipBalanceRetryHandler(IResolver resolver)
        {
            this.resolver = resolver;
        }

        /// <inheritdoc />
        public string HandlerType => "OwnershipBalanceRetryHandler";

        private ITrueLogger<OwnershipBalanceRetryHandler> Logger => this.resolver.GetInstance<ITrueLogger<OwnershipBalanceRetryHandler>>();

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
