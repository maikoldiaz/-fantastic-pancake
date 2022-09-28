// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceBusQueueClient.cs" company="Microsoft">
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
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Ecp.True.Chaos.Interfaces;
    using Microsoft.Azure.ServiceBus;
    using Newtonsoft.Json;

    /// <summary>
    /// The service bus queue client.
    /// </summary>
    /// <seealso cref="Ecp.True.Proxies.Azure.IServiceBusQueueClient" />
    public class ServiceBusQueueClient : IServiceBusQueueClient
    {
        /// <summary>
        /// The queue client.
        /// </summary>
        private readonly IQueueClient queueClient;

        /// <summary>
        /// The chaos manager.
        /// </summary>
        private readonly IChaosManager chaosManager;

        /// <summary>
        /// The deadletter manager.
        /// </summary>
        private readonly IDeadLetterManager deadLetterManager;

        /// <summary>
        /// The deadletter manager.
        /// </summary>
        private readonly string deadLetter = "deadletter";

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBusQueueClient" /> class.
        /// </summary>
        /// <param name="queueClient">The queue client.</param>
        /// <param name="chaosManager">The chaos manager.</param>
        /// <param name="deadLetterManager">The deadletter manager.</param>
        public ServiceBusQueueClient(IQueueClient queueClient, IChaosManager chaosManager, IDeadLetterManager deadLetterManager)
        {
            this.queueClient = queueClient;
            this.chaosManager = chaosManager;
            this.deadLetterManager = deadLetterManager;
        }

        /// <summary>
        /// Queues the message asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of message body.</typeparam>
        /// <param name="messageBody">The message body.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public Task QueueMessageAsync<T>(T messageBody)
        {
            return this.SendMessageAsync(messageBody, null);
        }

        /// <summary>
        /// Queues the message asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of message.</typeparam>
        /// <param name="messageBody">The message body.</param>
        /// <param name="messageId">The message id.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public Task QueueMessageAsync<T>(T messageBody, string messageId)
        {
            return this.SendMessageAsync(messageBody, messageId);
        }

        /// <summary>
        /// Queues the message asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of message body.</typeparam>
        /// <param name="messageBody">The message body.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public Task QueueSessionMessageAsync<T>(T messageBody, string sessionId)
        {
            return this.SendSessionMessageAsync(messageBody, sessionId);
        }

        /// <summary>
        /// Queues the session message asynchronous.
        /// </summary>
        /// <param name="messageBody">The message body.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>Returns the task.</returns>
        public Task QueueSessionMessageAsync(string messageBody, string sessionId)
        {
            return this.SendMessageAsync(messageBody, sessionId);
        }

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
        public Task QueueScheduleMessageAsync<T>(T messageBody, string sessionId, int intervalInSecs)
        {
            var json = JsonConvert.SerializeObject(messageBody);
            var message = new Message(Encoding.UTF8.GetBytes(json));
            message.ScheduledEnqueueTimeUtc = DateTime.UtcNow.AddSeconds(intervalInSecs);
            message.SessionId = sessionId;

            if (this.chaosManager.HasChaos)
            {
                message.Label = this.chaosManager.ChaosValue;
            }

            if (this.deadLetterManager.IsDeadLettered)
            {
                message.ReplyTo = this.deadLetter;
            }

            return this.queueClient.SendAsync(message);
        }

        /// <summary>
        /// Sends the message asynchronous.
        /// </summary>
        /// <param name="messageBody">The message body.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <returns>Return the task.</returns>
        private Task SendMessageAsync(string messageBody, string messageId)
        {
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));
            message.SessionId = messageId;
            if (this.chaosManager.HasChaos)
            {
                message.Label = this.chaosManager.ChaosValue;
            }

            if (this.deadLetterManager.IsDeadLettered)
            {
                message.ReplyTo = this.deadLetter;
            }

            return this.queueClient.SendAsync(message);
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="messageBody">The message body.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <returns>
        /// Return the task.
        /// </returns>
        private Task SendMessageAsync<T>(T messageBody, string messageId)
        {
            var json = JsonConvert.SerializeObject(messageBody);
            var message = new Message(Encoding.UTF8.GetBytes(json));

            if (!string.IsNullOrWhiteSpace(messageId))
            {
                message.MessageId = messageId;
            }

            if (this.chaosManager.HasChaos)
            {
                message.Label = this.chaosManager.ChaosValue;
            }

            if (this.deadLetterManager.IsDeadLettered)
            {
                message.ReplyTo = this.deadLetter;
            }

            return this.queueClient.SendAsync(message);
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="messageBody">The message body.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>
        /// Return the task.
        /// </returns>
        private Task SendSessionMessageAsync<T>(T messageBody, string sessionId)
        {
            var json = JsonConvert.SerializeObject(messageBody);
            var message = new Message(Encoding.UTF8.GetBytes(json));
            message.SessionId = sessionId;

            if (this.chaosManager.HasChaos)
            {
                message.Label = this.chaosManager.ChaosValue;
            }

            if (this.deadLetterManager.IsDeadLettered)
            {
                message.ReplyTo = this.deadLetter;
            }

            return this.queueClient.SendAsync(message);
        }
    }
}
