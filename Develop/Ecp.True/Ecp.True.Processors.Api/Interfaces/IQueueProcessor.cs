// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IQueueProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Interfaces
{
    using System.Threading.Tasks;
    using Ecp.True.Processors.Core.Interfaces;

    /// <summary>
    /// The ownerShip processor interface.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.Interfaces.IProcessor" />
    public interface IQueueProcessor : IProcessor
    {
        /// <summary>
        /// Push Message To Queue asynchronous.
        /// </summary>
        /// <param name="ticketId">The operational cut off.</param>
        /// <param name="queueName">The name of queue.</param>
        /// <returns>The Ownership Recalculation.</returns>
        Task PushQueueSessionMessageAsync(int ticketId, string queueName);

        /// <summary>
        /// Push Message To Queue asynchronous.
        /// </summary>
        /// <param name="ticketId">The operational cut off.</param>
        /// <param name="queueName">The name of queue.</param>
        /// <returns>The Ownership Recalculation.</returns>
        Task PushQueueMessageAsync(int ticketId, string queueName);
    }
}
