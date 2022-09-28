// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelMovementTransformer.cs" company="Microsoft">
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
    using System.IO;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Transform.Builders.Excel.Interfaces;
    using Ecp.True.Processors.Transform.Services.Excel.Interfaces;
    using Newtonsoft.Json.Linq;
    using Constants = Ecp.True.Core.Constants;

    /// <summary>
    /// The movement transformer.
    /// </summary>
    public class ExcelMovementTransformer : IExcelMovementTransformer
    {
        /// <summary>
        /// The table name.
        /// </summary>
        private const string TableName = "MOVIMIENTOS";

        /// <summary>
        /// The movement ID.
        /// </summary>
        private readonly string movementId = "MovementId";

        /// <summary>
        /// The no sheet message.
        /// </summary>
        private readonly string noSheet = "Movement Sheet Missing";

        /// <summary>
        /// The movement builder.
        /// </summary>
        private readonly IExcelMovementBuilder movementBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelMovementTransformer"/> class.
        /// </summary>
        /// <param name="movementBuilder">The movement builder.</param>
        public ExcelMovementTransformer(IExcelMovementBuilder movementBuilder)
        {
            this.movementBuilder = movementBuilder;
        }

        /// <inheritdoc/>
        public async Task<JArray> TransformMovementAsync(ExcelInput message)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));

            if (!message.Input.Tables.Contains(TableName))
            {
                throw new MissingFieldException(this.noSheet);
            }

            var rows = message.Input.Tables[TableName].AsEnumerable();
            var entities = new ConcurrentBag<JObject>();

            var tasks = new List<Task>();
            tasks.AddRange(rows.Select(r => this.TryBuildMovementAsync(entities, r, message)));

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return new JArray(entities);
        }

        private async Task TryBuildMovementAsync(ConcurrentBag<JObject> entities, DataRow element, ExcelInput message)
        {
            try
            {
                var input = new ExcelInput(Tuple.Create(element, message.Message.ActionType, true), message.Input);
                var movement = await this.movementBuilder.BuildAsync(input).ConfigureAwait(false);
                movement[Constants.MessageId] = movement[this.movementId];
                movement[Constants.SegmentId] = message.Message.FileRegistration.SegmentId;
                movement[Constants.Type] = MessageType.Movement.ToString();

                entities.Add(movement);
            }
            catch (InvalidDataException ex)
            {
                message.Message.RecordMovementFailure();
                message.Message.PopulatePendingTransactions(ex.Message, Constants.Movement, ex.Message);
            }
            catch (ArgumentException ex)
            {
                message.Message.RecordMovementFailure();
                message.Message.PopulatePendingTransactions(ex.Message, Constants.Movement, ex.Message);
            }
            catch (Exception ex)
            {
                message.Message.RecordMovementFailure();
                message.Message.PopulatePendingTransactions(ex.Message, Constants.Movement, Constants.TechnicalExceptionParsingErrorMessage);
            }
        }
    }
}
