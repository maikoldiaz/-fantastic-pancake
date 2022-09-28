// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FailureHandlerFactory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.HandleFailure
{
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Processors.Core.Interfaces;

    /// <summary>
    /// CutOffFailureHandler.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.FailureHandler.FailureHandlerFactory" />
    public class FailureHandlerFactory : IFailureHandlerFactory
    {
        /// <summary>
        /// The failure handlers.
        /// </summary>
        private readonly IEnumerable<IFailureHandler> failureHandlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="FailureHandlerFactory"/> class.
        /// </summary>
        /// <param name="failureHandlers">The failureHandlers.</param>
        public FailureHandlerFactory(
                  IEnumerable<IFailureHandler> failureHandlers)
        {
            this.failureHandlers = failureHandlers;
        }

        /// <inheritdoc/>
        public IFailureHandler GetFailureHandler(TicketType ticketType)
        {
            return this.failureHandlers.Single(x => x.TicketType == ticketType);
        }
    }
}