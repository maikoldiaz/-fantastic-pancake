// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FinalizerFactory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Processors.Core.Interfaces;

    /// <summary>
    /// FinalizerFactory.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.FinalizerFactory" />
    public class FinalizerFactory : IFinalizerFactory
    {
        /// <summary>
        /// The failure handlers.
        /// </summary>
        private readonly IEnumerable<IFinalizer> finalizers;

        /// <summary>
        /// Initializes a new instance of the <see cref="FinalizerFactory"/> class.
        /// </summary>
        /// <param name="finalizers">The failureHandlers.</param>
        public FinalizerFactory(
                  IEnumerable<IFinalizer> finalizers)
        {
            this.finalizers = finalizers;
        }

        /// <inheritdoc/>
        public IFinalizer GetFinalizer(TicketType ticketType)
        {
            return this.finalizers.Single(x => x.Type == ticketType);
        }

        /// <summary>
        /// finalizerType.
        /// </summary>
        /// <param name="finalizerType">finalizer Type.</param>
        /// <returns>i finalizer Type.</returns>
        public IFinalizer GetFinalizer(FinalizerType finalizerType)
        {
            return this.finalizers.Single(x => x.Finalizer == finalizerType);
        }
    }
}