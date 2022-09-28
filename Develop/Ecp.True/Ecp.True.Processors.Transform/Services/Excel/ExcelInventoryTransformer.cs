// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelInventoryTransformer.cs" company="Microsoft">
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
    /// The inventory transformer.
    /// </summary>
    public class ExcelInventoryTransformer : IExcelInventoryTransformer
    {
        /// <summary>
        /// The table name.
        /// </summary>
        private const string TableName = "INVENTARIOS";

        /// <summary>
        /// The no sheet message.
        /// </summary>
        private readonly string noSheet = "Inventory Sheet Missing";

        /// <summary>
        /// The inventory builder.
        /// </summary>
        private readonly IExcelInventoryBuilder inventoryBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelInventoryTransformer"/> class.
        /// </summary>
        /// <param name="inventoryBuilder">The inventory builder.</param>
        public ExcelInventoryTransformer(IExcelInventoryBuilder inventoryBuilder)
        {
            this.inventoryBuilder = inventoryBuilder;
        }

        /// <inheritdoc/>
        public async Task<JArray> TransformInventoryAsync(ExcelInput message)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));

            if (!message.Input.Tables.Contains(TableName))
            {
                throw new MissingFieldException(this.noSheet);
            }

            var rows = message.Input.Tables[TableName].AsEnumerable();
            var entities = new ConcurrentBag<JObject>();

            // Invoke inventory builder for all inventory
            var tasks = new List<Task>();
            tasks.AddRange(rows.Select(r => this.TryBuildInventoryAsync(entities, r, message)));

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return new JArray(entities);
        }

        private async Task TryBuildInventoryAsync(ConcurrentBag<JObject> entities, DataRow element, ExcelInput message)
        {
            try
            {
                var input = new ExcelInput(Tuple.Create(element, message.Message.ActionType, false), message.Input);
                var inventory = await this.inventoryBuilder.BuildAsync(input).ConfigureAwait(false);
                inventory[Constants.SegmentId] = message.Message.FileRegistration.SegmentId;
                inventory[Constants.Type] = MessageType.Inventory.ToString();
                entities.Add(inventory);
            }
            catch (InvalidDataException ex)
            {
                message.Message.RecordInventoryFailure();
                message.Message.PopulatePendingTransactions(ex.Message, Constants.Inventory, ex.Message);
            }
            catch (ArgumentException ex)
            {
                message.Message.RecordInventoryFailure();
                message.Message.PopulatePendingTransactions(ex.Message, Constants.Inventory, ex.Message);
            }
            catch (Exception ex)
            {
                message.Message.RecordInventoryFailure();
                message.Message.PopulatePendingTransactions(ex.Message, Constants.Inventory, Constants.TechnicalExceptionParsingErrorMessage);
            }
        }
    }
}
