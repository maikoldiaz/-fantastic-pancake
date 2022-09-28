// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IServiceBusQueueClient.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.Azure
{
    using System.Threading.Tasks;

    /// <summary>
    /// The service bus queue client.
    /// </summary>
    public interface IServiceBusQueueClient
    {
        /// <summary>
        /// Queues the message asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of message body.</typeparam>
        /// <param name="messageBody">The message body.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task QueueMessageAsync<T>(T messageBody);

        /// <summary>
        /// Queues the message asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of message.</typeparam>
        /// <param name="messageBody">The message body.</param>
        /// <param name="messageId">The message id.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task QueueMessageAsync<T>(T messageBody, string messageId);

        /// <summary>
        /// Queues the message asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of message body.</typeparam>
        /// <param name="messageBody">The message body.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task QueueSessionMessageAsync<T>(T messageBody, string sessionId);

        /// <summary>
        /// Queues the session message asynchronous.
        /// </summary>
        /// <param name="messageBody">The message body.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>Returns the task.</returns>
        Task QueueSessionMessageAsync(string messageBody, string sessionId);

        /// <summary>
        /// Queues the schedule message asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of message body.</typeparam>
        /// <param name="messageBody">The message body.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="intervalInSecs">The interval in secs.</param>
        /// <returns>
        /// Returns the Task.
        /// </returns>
        Task QueueScheduleMessageAsync<T>(T messageBody, string sessionId, int intervalInSecs);
    }
}
