// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelInventoryBuilder.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Builders.Excel.Movement
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Transform.Builders.Excel.Common;
    using Ecp.True.Processors.Transform.Builders.Excel.Interfaces;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The movement builder.
    /// </summary>
    public class ExcelInventoryBuilder : ExcelBuilder, IExcelInventoryBuilder
    {
        /// <summary>
        /// The owner builder.
        /// </summary>
        private readonly IExcelOwnerBuilder ownerBuilder;

        /// <summary>
        /// The attribute builder.
        /// </summary>
        private readonly IExcelAttributeBuilder attributeBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelInventoryBuilder"/> class.
        /// </summary>
        /// <param name="ownerBuilder">The owner builder.</param>
        /// <param name="attributeBuilder">The attribute builder.</param>
        public ExcelInventoryBuilder(IExcelOwnerBuilder ownerBuilder, IExcelAttributeBuilder attributeBuilder)
        {
            this.ownerBuilder = ownerBuilder;
            this.attributeBuilder = attributeBuilder;
        }

        /// <inheritdoc/>
        public override async Task<JObject> BuildAsync(ExcelInput element)
        {
            ArgumentValidators.ThrowIfNull(element, nameof(element));
            var args = (Tuple<DataRow, string, bool>)element.Arguments;
            var dr = args.Item1;

            var inventoryId = dr[Constants.InventoryIdKey].ToString();
            var nodeId = dr[Constants.NodeIdKey].ToString();

            var result = CreateRoot(inventoryId, nodeId, args.Item2, dr);
            var entities = new ConcurrentBag<JObject>();

            await this.BuildProductAsync(entities, dr, element).ConfigureAwait(false);

            entities.ForEach(result.Merge);
            return result;
        }

        private static JObject CreateRoot(string inventoryId, string nodeId, string actionType, DataRow row)
        {
            var entity = new JObject();

            entity.Add(ValidateOrigenNull(row));
            entity.Add("DestinationSystem", SystemType.TRUE.ToString("G"));
            entity.Add("EventType", actionType);
            entity.Add(Constants.InventoryId, inventoryId);
            entity.Add(GetValue(row, Constants.InventoryDateKey, Constants.InventoryDate, JTokenType.Date));
            entity.Add(Constants.NodeId, nodeId);
            entity.Add(GetValue(row, Constants.ScenarioIdKey, Constants.Scenario, JTokenType.Float));
            entity.Add("Observations", GetValue(row, "Observaciones"));
            entity.Add(GetValue(row, Constants.ToleranceKey, "UncertaintyPercentage", JTokenType.None));
            entity.Add(Constants.ProductId, GetValue(row, "Producto"));
            entity.Add("ProductType", GetValue(row, "TipoProducto"));
            entity.Add(GetValue(row, Constants.NetStandardVolumeKey, "ProductVolume", JTokenType.Float));
            entity.Add(GetValue(row, Constants.GrossStandardVolumeKey, "GrossStandardQuantity", JTokenType.Float));
            entity.Add("MeasurementUnit", GetValue(row, "UnidadMedida"));
            entity.Add(Constants.BatchId, GetValue(row, Constants.BatchId));
            entity.Add(Constants.TankName, GetValue(row, Constants.TankNameKey));
            entity.Add(Constants.Version, GetValue(row, Constants.Version));
            entity.Add(Constants.SystemId, GetValue(row, Constants.SystemIdKey));
            entity.Add(Constants.OperatorId, GetValue(row, Constants.OperatorKey));
            entity.Add(Constants.OriginalId, entity.GetOrDefault(Constants.InventoryId, string.Empty));
            var inventoryUniqueId = IdGenerator.GenerateInventoryProductUniqueId(entity);
            if (entity.HasStringValue(Constants.Scenario, Convert.ToString((int)ScenarioType.OFFICER, CultureInfo.InvariantCulture)))
            {
                entity[Constants.InventoryId] = inventoryUniqueId;
            }

            entity.Add(Constants.InventoryProductUniqueId, inventoryUniqueId);
            entity.Add(Constants.MessageId, inventoryUniqueId);

            if (entity.GetOrDefault(Constants.InventoryId, string.Empty).Length > 50)
            {
                throw new InvalidDataException(Entities.Constants.InventoryIdLengthExceeded);
            }

            return entity;
        }

        private static async Task DoBuildAsync(ConcurrentBag<JObject> entities, Func<ExcelInput, Task<JObject>> builder, ExcelInput element)
        {
            ArgumentValidators.ThrowIfNull(entities, nameof(entities));
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            var result = await builder(element).ConfigureAwait(false);
            entities.Add(result);
        }

        private async Task<JObject> DoBuildProductAsync(ExcelInput element)
        {
            ArgumentValidators.ThrowIfNull(element, nameof(element));

            var result = new JObject();
            var entities = new ConcurrentBag<JObject>();

            var tasks = new List<Task>();
            tasks.Add(DoBuildAsync(entities, this.attributeBuilder.BuildAsync, element));
            tasks.Add(DoBuildAsync(entities, this.ownerBuilder.BuildAsync, element));

            await Task.WhenAll(tasks).ConfigureAwait(false);

            entities.ForEach(result.Merge);

            return result;
        }

        private async Task BuildProductAsync(ConcurrentBag<JObject> entities, DataRow element, ExcelInput message)
        {
            var input = new ExcelInput(Tuple.Create(element, "Insert", false), message.Input);
            var obj = await this.DoBuildProductAsync(input).ConfigureAwait(false);
            entities.Add(obj);
        }
    }
}
