// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelEventTransformer.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Services.Excel
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Transform.Builders.Excel.Interfaces;
    using Ecp.True.Processors.Transform.Services.Excel.Interfaces;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The excel event transformer.
    /// </summary>
    public class ExcelEventTransformer : IExcelEventTransformer
    {
        /// <summary>
        /// The table name.
        /// </summary>
        private const string TableName = "Eventos";

        /// <summary>
        /// The event ID.
        /// </summary>
        private readonly string sessionId = "SessionId";

        /// <summary>
        /// The noSheet.
        /// </summary>
        private readonly string noSheet = "Event Sheet Missing";

        /// <summary>
        /// The event builder.
        /// </summary>
        private readonly IExcelEventBuilder eventBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelEventTransformer"/> class.
        /// </summary>
        /// <param name="eventBuilder">The event builder.</param>
        public ExcelEventTransformer(IExcelEventBuilder eventBuilder)
        {
            this.eventBuilder = eventBuilder;
        }

        /// <inheritdoc/>
        public async Task<JArray> TransformEventAsync(ExcelInput message)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));

            if (!message.Input.Tables.Contains(TableName))
            {
                throw new MissingFieldException(this.noSheet);
            }

            var rows = message.Input.Tables[TableName].AsEnumerable();
            var entities = new ConcurrentBag<JObject>();

            var tasks = new List<Task>();
            tasks.AddRange(rows.Select(r => this.TryBuildEventAsync(entities, r, message)));

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return new JArray(entities);
        }

        private async Task TryBuildEventAsync(ConcurrentBag<JObject> entities, DataRow element, ExcelInput message)
        {
            try
            {
                var input = new ExcelInput(Tuple.Create(element, message.Message.ActionType, true), message.Input);
                var eventObject = await this.eventBuilder.BuildAsync(input).ConfigureAwait(false);

                eventObject[Constants.MessageId] = eventObject[this.sessionId];
                eventObject[Constants.Type] = MessageType.Events.ToString();
                entities.Add(eventObject);
            }
            catch (Exception ex)
            {
                message.Message.RecordEventFailure();
                message.Message.PopulatePendingTransactions(ex.Message, MessageType.Events.ToString(), Constants.TechnicalExceptionParsingErrorMessage);
            }
        }
    }
}
