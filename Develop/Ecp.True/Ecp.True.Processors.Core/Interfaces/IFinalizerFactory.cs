// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFinalizerFactory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Interfaces
{
    using Ecp.True.Entities.Enumeration;

    /// <summary>
    /// IFinalizerFactory.
    /// </summary>
    public interface IFinalizerFactory
    {
        /// <summary>
        /// Gets the finalizer.
        /// </summary>
        /// <param name="ticketType">The ticket type.</param>
        /// <returns>IFinalizer.</returns>
        IFinalizer GetFinalizer(TicketType ticketType);

        /// <summary>
        /// finalizerType.
        /// </summary>
        /// <param name="finalizerType">finalizer Type.</param>
        /// <returns>IFinalizer.</returns>
        public IFinalizer GetFinalizer(FinalizerType finalizerType);
    }
}