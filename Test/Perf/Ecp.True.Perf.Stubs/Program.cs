// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Perf.Stubs
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Azure.ServiceBus;

    [SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable", Justification = "Pending")]
    public class Program
    {
        private const string QueueName = "movements";
        private static readonly string ServiceBusConnectionString = ConfigurationManager.AppSettings["ServiceBusConnectionString"];
        private static IQueueClient queueClient;

        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Pending")]
        [SuppressMessage("Usage", "VSTHRD002:Avoid problematic synchronous waits", Justification = "Pending")]
        [SuppressMessage("Microsoft.Performance", "CA1801:ReviewUnusedParameters", Justification = "Pending")]
        public static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            int j = 0;
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            string dtstr = DateTime.UtcNow.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            int fno = 4;
            List<string> messageList = new List<string>();
            string output = null;

            // while (j < batchCount)
            {
                messageList.Clear();
                for (int i = 0; i < 1; i++)
                {
                    output = $"{{\r\n  \"SystemTypeId\": \"Sinoper\",\r\n \"UploadFileId\": \"{dtstr}_{fno}\"\r\n}}";
                    messageList.Add(output);
                    fno++;
                }
            }

            var tasks = messageList.Select(eachmsg =>
            {
                var message = new Message(Encoding.UTF8.GetBytes(eachmsg));
                message.SessionId = Guid.NewGuid().ToString();
                return queueClient.SendAsync(message);
            });

            await Task.WhenAll(tasks).ConfigureAwait(false);

            await queueClient.CloseAsync().ConfigureAwait(false);

            int waitToSendNextbatch = 1000;
            Console.WriteLine($"Wait: {waitToSendNextbatch / 1000} seconds");
            Thread.Sleep(waitToSendNextbatch);

            j++;
        }
    }
}