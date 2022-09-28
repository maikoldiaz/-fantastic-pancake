// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonTransformer.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Services.Json
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Entities.Sap.Purchases;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Transform.Services.Json.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Json Transformer.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Transform.Services.Json.Interfaces.IJsonTransformer" />
    public class JsonTransformer : IJsonTransformer
    {
        /// <inheritdoc/>
        public async Task<JToken> TransformJsonAsync(JToken data, TrueMessage message)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            switch (message.Message)
            {
                case MessageType.Purchase:
                case MessageType.Sale:
                    return await TransformContractsJsonAsync(data, message).ConfigureAwait(false);
                case MessageType.Inventory:
                    return await TransformInventoryJsonAsync(data, message).ConfigureAwait(false);
                default:
                    return await TransformMovementJsonAsync(data, message).ConfigureAwait(false);
            }
        }

        private static Task<JToken> TransformMovementJsonAsync(JToken data, TrueMessage message)
        {
            ArgumentValidators.ThrowIfNull(data, nameof(data));
            if (!message.IsRetry)
            {
                UpdateJsonAsync(data, message).ConfigureAwait(false);
            }

            return Task.FromResult(data);
        }

        private static Task<JArray> TransformInventoryJsonAsync(JToken data, TrueMessage message)
        {
            ArgumentValidators.ThrowIfNull(data, nameof(data));

            var inventory = JArray.Parse(JsonConvert.SerializeObject(data));
            ConcurrentBag<JObject> entities = new ConcurrentBag<JObject>();
            inventory.Children().ForEach(e => BuildInventoryProducts(e, entities));
            var result = new JArray(entities);
            if (!message.IsRetry)
            {
                UpdateJsonAsync(result, message).ConfigureAwait(false);
            }

            return Task.FromResult(result);
        }

        private static Task<JToken> TransformContractsJsonAsync(JToken data, TrueMessage message)
        {
            ArgumentValidators.ThrowIfNull(data, nameof(data));

            JArray arrayContracts = message.Message == MessageType.Purchase ? ConvertPurchaseToJToken(data) : ConvertSalesToJToken(data);
            data = JToken.FromObject(arrayContracts);
            if (!message.IsRetry)
            {
                UpdateJsonAsync(data, message).ConfigureAwait(false);
            }

            return Task.FromResult(data);
        }

        private static void BuildInventoryProducts(JToken message, ConcurrentBag<JObject> entities)
        {
            JArray products = message[Transform.Constants.Products] as JArray;
            message[Transform.Constants.Products].Parent.Remove();

            foreach (var product in products)
            {
                ((JObject)product).Merge(message);
                entities.Add((JObject)product);
            }
        }

        private static Task UpdateTokenAsync(JToken entity, TrueMessage message)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));

            if (message.Message.ToString() == Entities.Core.MessageType.Movement.ToString("G"))
            {
                UpdateTokenMovements(entity);
            }
            else if (message.Message.ToString() == Entities.Core.MessageType.Inventory.ToString("G"))
            {
                UpdateTokenInventories(entity);
            }
            else
            {
                UpdateTokenSalesPurchases(entity, message);
            }

            return Task.CompletedTask;
        }

        private static void UpdateTokenSalesPurchases(JToken entity, TrueMessage message)
        {
            entity[Constants.MessageId] = IdGenerator.GenerateSalePurchaseId(entity, message);
            entity[Constants.Type] = message.Message.ToString();
        }

        private static void UpdateTokenInventories(JToken entity)
        {
            UpdateAttributesAsync(entity, Entities.SapConstants.SourceSystem, "SourceSystemId").ConfigureAwait(false);
            UpdateAttributesAsync(entity, Entities.SapConstants.SystemKey, Constants.SystemId).ConfigureAwait(false);
            UpdateAttributesAsync(entity, Entities.SapConstants.TankKey, Constants.TankName).ConfigureAwait(false);
            UpdateAttributesAsync(entity, Entities.SapConstants.NetStandardQuantityKey, "ProductVolume").ConfigureAwait(false);
            UpdateAttributesAsync(entity, Entities.SapConstants.UncertaintyKey, "UncertaintyPercentage").ConfigureAwait(false);
            var inventoryUniqueId = IdGenerator.GenerateInventoryProductUniqueId(entity);
            entity[Constants.InventoryProductUniqueId] = inventoryUniqueId;
            entity[Constants.MessageId] = inventoryUniqueId;
            entity[Constants.Type] = Entities.Core.MessageType.Inventory.ToString("G");
        }

        private static void UpdateTokenMovements(JToken entity)
        {
            UpdateAttributesAsync(entity, Entities.SapConstants.SourceSystem, "SourceSystemId").ConfigureAwait(false);
            UpdateAttributesAsync(entity, Entities.SapConstants.SystemKey, Constants.SystemId).ConfigureAwait(false);
            UpdateAttributesAsync(entity, Entities.SapConstants.TankKey, Constants.TankName).ConfigureAwait(false);
            UpdateAttributesAsync(entity, Entities.SapConstants.UncertaintyKey, "Tolerance").ConfigureAwait(false);
            UpdateAttributesAsync(entity, Entities.SapConstants.GrossStandardQuantityKey, "GrossStandardVolume").ConfigureAwait(false);
            UpdateAttributesAsync(entity, Entities.SapConstants.NetStandardQuantityKey, "NetStandardVolume").ConfigureAwait(false);
            entity[Constants.MessageId] = entity[Constants.MovementId];
            entity[Constants.Type] = Entities.Core.MessageType.Movement.ToString("G");
            if (!IsNullOrEmpty(entity[Entities.SapConstants.MovementDestination]))
            {
                UpdateMovementDestination(entity);
            }

            if (!IsNullOrEmpty(entity[Entities.SapConstants.OfficialInformationKey]))
            {
                UpdateMovementOfficialInformation(entity);
            }
        }

        private static void UpdateMovementOfficialInformation(JToken entity)
        {
            entity[Entities.SapConstants.IsOfficial] = entity[Entities.SapConstants.OfficialInformationKey][Entities.SapConstants.IsOfficial];
            entity[Entities.SapConstants.BackupMovementId] = entity[Entities.SapConstants.OfficialInformationKey][Entities.SapConstants.BackupMovementId];
            entity[Entities.SapConstants.GlobalMovementId] = entity[Entities.SapConstants.OfficialInformationKey][Entities.SapConstants.GlobalMovementId];
        }

        private static void UpdateMovementDestination(JToken entity)
        {
            if (IsNullOrEmpty(entity[Entities.SapConstants.MovementDestination][Entities.SapConstants.DestinationProductId]))
            {
                if (!IsNullOrEmpty(entity[Entities.SapConstants.MovementSource]) && !IsNullOrEmpty(entity[Entities.SapConstants.MovementSource][Entities.SapConstants.SourceProductId]))
                {
                    entity[Entities.SapConstants.MovementDestination][Entities.SapConstants.DestinationProductId] = entity[Entities.SapConstants.MovementSource][Entities.SapConstants.SourceProductId];
                }
                else
                {
                    throw new InvalidDataException(Entities.SapConstants.DestinationProductIdIsMandatory);
                }
            }
        }

        private static bool IsNullOrEmpty(JToken token)
        {
            return (token == null) || (token.Type == JTokenType.Null) || string.IsNullOrEmpty(token.ToString());
        }

        private static Task UpdateAttributesAsync(JToken entity, string propertyName, string attributeName)
        {
            ArgumentValidators.ThrowIfNull(propertyName, nameof(propertyName));
            ArgumentValidators.ThrowIfNull(attributeName, nameof(attributeName));
            if (entity[propertyName] != null)
            {
                entity[attributeName] = entity[propertyName];
                entity[propertyName].Parent.Remove();
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// The update JSON async.
        /// </summary>
        /// <param name="entities">The array.</param>
        /// <param name="message">The isMovement flag.</param>
        /// <returns>The task.</returns>
        private static Task UpdateJsonAsync(JToken entities, TrueMessage message)
        {
            var tasks = new List<Task>();
            tasks.AddRange(entities.Select(e => UpdateTokenAsync(e, message)));
            return Task.WhenAll(tasks);
        }

        private static JArray ConvertSalesToJToken(JToken entity)
        {
            Sale sale = JsonConvert.DeserializeObject<Sale>(entity.ToString());
            JArray array = new JArray();

            foreach (var item in sale.OrderSale.PositionObject.Positions)
            {
                JObject obj = new JObject
                {
                    new JProperty("SessionId", IdGenerator.GenerateSalePurchaseUniqueId(sale.OrderSale.ControlData.MessageId, item.PositionId.Value)),
                    new JProperty("OriginMessageId", sale.OrderSale.ControlData.MessageId),
                    new JProperty("SystemName", SystemType.SAP.ToString()),
                    new JProperty("DocumentNumber", sale.OrderSale.Header.NumberOrder),
                    new JProperty("Position", item.PositionId),
                    new JProperty("MovementTypeId", $"{sale.OrderSale.Header.TypeOrder}"),
                    new JProperty("SourceNodeId", sale.OrderSale.Header.SourceLocation),
                    new JProperty("DestinationNodeId", item.DestinationLocationId),
                    new JProperty("ProductId", item.Material),
                    new JProperty("StartDate", item.StartTime),
                    new JProperty("EndDate", item.EndTime),
                    new JProperty("Owner1Id", sale.OrderSale.Header.OrganizationId),
                    new JProperty("Owner2Id", sale.OrderSale.Header.ClientId),
                    new JProperty("Volume", item.Quantity),
                    new JProperty("MeasurementUnit", item.QuantityUom),
                    new JProperty("PurchaseOrderType", $"{sale.OrderSale.Header.TypeOrder}"),
                    new JProperty("Status", "Activa"),
                    new JProperty("Frequency", item.Frequency),
                    new JProperty("SourceSystem", SystemType.SAP.ToString()),
                    new JProperty("EventType", GetActionType(sale.OrderSale.ControlData.EventSapPo, item.PositionStatus, MessageType.Sale)),
                    new JProperty("ExpeditionClass", sale.OrderSale.Header.ExpeditionClass),
                    new JProperty("DateReceivedPo", sale.OrderSale.ControlData.DateReceivedPo),
                    new JProperty("StatusCredit", sale.OrderSale.Header.CreditStatus),
                    new JProperty("DescriptionStatus", sale.OrderSale.Header.DescriptionStatus),
                    new JProperty("DateOrder", sale.OrderSale.Header.DateOrder),
                    new JProperty("PositionStatus", item.PositionStatus),
                    new JProperty("DestinationStorageLocationId", item.DestinationStorageLocationId),
                    new JProperty("Batch", item.Batch),
                    new JProperty("ActionType", GetActionType(sale.OrderSale.ControlData.EventSapPo, item.PositionStatus, MessageType.Sale)),
                };

                array.Add(obj);
            }

            return array;
        }

        private static string GetActionType(string eventSapPo, string positionStatus, MessageType messageType)
        {
            string contractEvent = EventType.Insert.ToString();

            if (eventSapPo.EqualsIgnoreCase(Entities.SapConstants.EventSapUpdate))
            {
                contractEvent = EventType.Update.ToString();

                if (messageType == MessageType.Purchase && !string.IsNullOrEmpty(positionStatus) && positionStatus.EqualsIgnoreCase("L"))
                {
                    contractEvent = EventType.Delete.ToString();
                }

                if (messageType == MessageType.Sale && !string.IsNullOrEmpty(positionStatus) && positionStatus.EqualsIgnoreCase("X"))
                {
                    contractEvent = EventType.Delete.ToString();
                }
            }

            return contractEvent;
        }

        private static JArray ConvertPurchaseToJToken(JToken entity)
        {
            SapPurchase purchase = JsonConvert.DeserializeObject<SapPurchase>(entity.ToString());
            JArray array = new JArray();

            foreach (var item in purchase.PurchaseOrder.PurchaseOrder.PurchaseItem.PurchaseItem)
            {
                JObject obj = new JObject
                {
                    new JProperty("SessionId", IdGenerator.GenerateSalePurchaseUniqueId(purchase.PurchaseOrder.MessageId, item.Id.Value)),
                    new JProperty("OriginMessageId", purchase.PurchaseOrder.MessageId),
                    new JProperty("SystemName", SystemType.SAP.ToString()),
                    new JProperty("DocumentNumber", purchase.PurchaseOrder.PurchaseOrder.PurchaseOrderId),
                    new JProperty("Position", item.Id),
                    new JProperty("MovementTypeId", $"{purchase.PurchaseOrder.PurchaseOrder.PurchaseOrderType}"),
                    new JProperty("SourceNodeId", string.IsNullOrEmpty(purchase.PurchaseOrder.PurchaseOrder.SourceLocation) ? null : purchase.PurchaseOrder.PurchaseOrder.SourceLocation),
                    new JProperty("DestinationNodeId", item.Location.Destination.DestinationNode),
                    new JProperty("ProductId", item.Commodity.Name),
                    new JProperty("StartDate", item.Period.StartPeriod),
                    new JProperty("EndDate", item.Period.EndPeriod),
                    new JProperty("Owner1Id", purchase.PurchaseOrder.PurchaseOrder.Society.Name),
                    new JProperty("Owner2Id", purchase.PurchaseOrder.PurchaseOrder.Provider.Owner),
                    new JProperty("Volume", item.Commodity.Quantity),
                    new JProperty("MeasurementUnit", item.Commodity.QuantityUom),
                    new JProperty("PurchaseOrderType", $"{purchase.PurchaseOrder.PurchaseOrder.PurchaseOrderType}"),
                    new JProperty("Status", purchase.PurchaseOrder.PurchaseOrder.Status),
                    new JProperty("EstimatedVolume", item.EstimatedVolume),
                    new JProperty("Tolerance", item.Tolerance),
                    new JProperty("Frequency", item.Frequency),
                    new JProperty("SourceSystem", SystemType.SAP.ToString()),
                    new JProperty("DateOrder", purchase.PurchaseOrder.PurchaseOrder.DateOrder),
                    new JProperty("DateReceivedPo", purchase.PurchaseOrder.Date),
                    new JProperty("EventType", GetActionType(purchase.PurchaseOrder.Event, item.PositionStatus, MessageType.Purchase)),
                    new JProperty("Uom", item.Criterion.Uom),
                    new JProperty("PositionStatus", item.PositionStatus),
                    new JProperty("Property", item.Criterion.Property),
                    new JProperty("ActionType", GetActionType(purchase.PurchaseOrder.Event, item.PositionStatus, MessageType.Purchase)),
                };
                array.Add(obj);
            }

            return array;
        }
    }
}
