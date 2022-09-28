// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFinalizer.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Entities.Enumeration;

    /// <summary>
    /// The finalizer processor.
    /// </summary>
    public interface IFinalizer
    {
        /// <summary>
        /// Gets the type of the ticket.
        /// </summary>
        /// <value>
        /// The type of the ticket.
        /// </value>
        TicketType Type { get; }

        /// <summary>
        /// Gets the type of the finalizer.
        /// </summary>
        /// <value>
        /// The type of the ticket.
        /// </value>
        FinalizerType Finalizer { get; }

        /// <summary>
        /// Processes the asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>The task.</returns>
        Task ProcessAsync(int ticketId);

        /// <summary>
        /// Processes the asynchronous.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The task.</returns>
        Task ProcessAsync(object data);
    }
}
