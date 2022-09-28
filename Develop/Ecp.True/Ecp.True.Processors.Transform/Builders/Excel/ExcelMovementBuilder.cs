// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelMovementBuilder.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Transform.Builders.Excel.Common;
    using Ecp.True.Processors.Transform.Builders.Excel.Interfaces;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The movement builder.
    /// </summary>
    public class ExcelMovementBuilder : ExcelBuilder, IExcelMovementBuilder
    {
        /// <summary>
        /// The movement identifier key.
        /// </summary>
        private const string MovementIdKey = "IdMovimiento";

        /// <summary>
        /// The initial date.
        /// </summary>
        private const string InitialDate = "FechaHoraInicial";

        /// <summary>
        /// The end date.
        /// </summary>
        private const string EndDate = "FechaHoraFinal";

        /// <summary>
        /// The owner builder.
        /// </summary>
        private readonly IExcelOwnerBuilder ownerBuilder;

        /// <summary>
        /// The attribute builder.
        /// </summary>
        private readonly IExcelAttributeBuilder attributeBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelMovementBuilder"/> class.
        /// </summary>
        /// <param name="ownerBuilder">The owner builder.</param>
        /// <param name="attributeBuilder">The attribute builder.</param>
        public ExcelMovementBuilder(IExcelOwnerBuilder ownerBuilder, IExcelAttributeBuilder attributeBuilder)
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

            var movementId = dr[MovementIdKey].ToString();

            var result = CreateRoot(movementId, args.Item2, dr);
            var entities = new ConcurrentBag<JObject>();

            var tasks = new List<Task>();
            tasks.Add(DoBuildAsync(entities, this.attributeBuilder.BuildAsync, element));
            tasks.Add(DoBuildAsync(entities, this.ownerBuilder.BuildAsync, element));

            await Task.WhenAll(tasks).ConfigureAwait(false);

            entities.ForEach(result.Merge);
            return result;
        }

        private static JObject CreateRoot(string movementId, string actionType, DataRow row)
        {
            var entity = new JObject();

            entity.Add(ValidateOrigenNull(row));
            entity.Add("EventType", actionType);
            entity.Add("MovementId", movementId);
            entity.Add(Constants.MovementTypeId, GetValue(row, "IdTipoMovimiento"));
            entity.Add(GetValue(row, InitialDate, Constants.OperationalDate, JTokenType.Date));
            entity.Add(GetValue(row, Constants.GrossStandardVolumeKey, "GrossStandardVolume", JTokenType.Float));
            entity.Add(GetValue(row, Constants.NetStandardVolumeKey, "NetStandardVolume", JTokenType.Float));
            entity.Add("MeasurementUnit", GetValue(row, "UnidadMedida"));
            entity.Add(GetValue(row, Constants.ScenarioIdKey, Constants.Scenario, JTokenType.Float));
            entity.Add("Observations", GetValue(row, "Observaciones"));
            entity.Add("Classification", GetValue(row, "Clasificacion"));
            entity.Add(GetValue(row, Constants.ToleranceKey, "UncertaintyPercentage", JTokenType.None));

            var period = new JObject();
            period.Add(GetValue(row, InitialDate, "StartTime", JTokenType.Date));
            period.Add(GetValue(row, EndDate, "EndTime", JTokenType.Date));

            entity.Add(new JProperty("Period", period));

            var source = new JObject();
            var sourceNodeId = GetValue(row, "OrigenMovimiento", Constants.SourceNodeId, JTokenType.None);
            var sourceProductId = GetValue(row, "IdProductoOrigen", Constants.SourceProductId, JTokenType.None);
            var sourceProductTypeId = GetValue(row, "IdTipoProductoOrigen", "SourceProductTypeId", JTokenType.None);
            if (!string.IsNullOrEmpty(sourceNodeId.Value.ToString()) || !string.IsNullOrEmpty(sourceProductId.Value.ToString()) || !string.IsNullOrEmpty(sourceProductTypeId.Value.ToString()))
            {
                source.Add(sourceNodeId);
                source.Add(sourceProductId);
                source.Add(sourceProductTypeId);
                entity.Add(new JProperty(Constants.MovementSource, source));
            }

            var destination = new JObject();
            var destinationNodeId = GetValue(row, "DestinoMovimiento", Constants.DestinationNodeId, JTokenType.None);
            var destinationProductId = GetValue(row, "IdProductoDestino", Constants.DestinationProductId, JTokenType.None);
            var destinationProductTypeId = GetValue(row, "IdTipoProductoDestino", "DestinationProductTypeId", JTokenType.None);
            if (!string.IsNullOrEmpty(destinationNodeId.Value.ToString()) ||
                !string.IsNullOrEmpty(destinationProductTypeId.Value.ToString()) ||
                !string.IsNullOrEmpty(destinationProductTypeId.Value.ToString()))
            {
                destination.Add(destinationNodeId);
                destination.Add(destinationProductId);
                destination.Add(destinationProductTypeId);
                entity.Add(new JProperty(Constants.MovementDestination, destination));
            }

            entity.Add(Constants.BatchId, GetValue(row, Constants.BatchId));
            entity.Add(Constants.Version, GetValue(row, Constants.Version));
            entity.Add(Constants.SystemId, GetValue(row, Constants.SystemIdKey));
            entity.Add(Constants.OperatorId, GetValue(row, Constants.OperatorKey));
            entity.Add(Constants.OriginalId, entity.GetOrDefault(Constants.MovementId, string.Empty));
            entity[Constants.MovementId] = IdGenerator.GenerateMovementId(entity);
            if (entity[Constants.MovementDestination] != null)
            {
                UpdateMovementDestination(entity);
            }

            return entity;
        }

        private static void UpdateMovementDestination(JToken entity)
        {
            if ((entity[Constants.MovementDestination][Constants.DestinationProductId] == null ||
                string.IsNullOrEmpty(entity[Constants.MovementDestination][Constants.DestinationProductId].ToString())) &&
                entity[Constants.MovementSource] != null && entity[Constants.MovementSource]["SourceProductId"] != null)
            {
                entity[Constants.MovementDestination][Constants.DestinationProductId] = entity[Constants.MovementSource][Constants.SourceProductId];
            }
        }

        private static async Task DoBuildAsync(ConcurrentBag<JObject> entities, Func<ExcelInput, Task<JObject>> builder, ExcelInput element)
        {
            ArgumentValidators.ThrowIfNull(entities, nameof(entities));
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            var result = await builder(element).ConfigureAwait(false);
            entities.Add(result);
        }
    }
}
