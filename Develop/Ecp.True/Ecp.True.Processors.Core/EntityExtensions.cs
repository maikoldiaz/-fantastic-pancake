// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityExtensions.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Proxies.OwnershipRules.Response;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Contract = Ecp.True.Entities.Registration.Contract;
    using Event = Ecp.True.Entities.Registration.Event;

    /// <summary>
    /// The extensions.
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// Converts to pendingtransaction.
        /// </summary>
        /// <param name="inventoryProduct">The inventory product.</param>
        /// <param name="fileRegistrationTransaction">The file registration transaction message.</param>
        /// <param name="errors">The errors.</param>
        /// <returns>The pending transaction.</returns>
        public static PendingTransaction ToPendingTransaction(this InventoryProduct inventoryProduct, FileRegistrationTransaction fileRegistrationTransaction, IEnumerable<string> errors)
        {
            ArgumentValidators.ThrowIfNull(inventoryProduct, nameof(inventoryProduct));
            ArgumentValidators.ThrowIfNull(fileRegistrationTransaction, nameof(fileRegistrationTransaction));
            var pendingTransaction = new PendingTransaction
            {
                SourceNode = Convert.ToString(inventoryProduct.NodeId, CultureInfo.InvariantCulture),
                SourceProduct = inventoryProduct.ProductId,
                Volume = inventoryProduct.ProductVolume.GetValueOrDefault().ToString(CultureInfo.InvariantCulture),
                Units = inventoryProduct.MeasurementUnit,
                MessageTypeId = MessageType.Inventory,
                SystemTypeId = fileRegistrationTransaction.SystemTypeId,
                MessageId = fileRegistrationTransaction.UploadId,
                BlobName = fileRegistrationTransaction.BlobPath,
                ActionTypeId = fileRegistrationTransaction.ActionType,
                StartDate = inventoryProduct.InventoryDate,
                ErrorJson = JsonConvert.SerializeObject(errors),
                Identifier = inventoryProduct.Observations.GetIdentifierRoms(inventoryProduct.SourceSystemId.GetValueOrDefault().ToString(CultureInfo.InvariantCulture), inventoryProduct.InventoryId),
                SystemName = inventoryProduct.SourceSystemId,
                SegmentId = inventoryProduct.SegmentId,
                MessageType = fileRegistrationTransaction.MessageType.ToString(),
            };

            pendingTransaction.ValidateScenarioId(inventoryProduct.ScenarioId);
            errors.ForEach(x => pendingTransaction.Errors.Add(new PendingTransactionError
            {
                ErrorMessage = x,
                RecordId = fileRegistrationTransaction.RecordId,
            }));
            return pendingTransaction;
        }

        /// <summary>
        /// Converts to pendingtransaction.
        /// </summary>
        /// <param name="movement">The movement.</param>
        /// <param name="fileRegistrationTransaction">The file registration transaction message.</param>
        /// <param name="errors">The errors.</param>
        /// <returns>The pending transaction.</returns>
        public static PendingTransaction ToPendingTransaction(this Movement movement, FileRegistrationTransaction fileRegistrationTransaction, IEnumerable<string> errors)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));
            ArgumentValidators.ThrowIfNull(fileRegistrationTransaction, nameof(fileRegistrationTransaction));
            var pendingTransaction = new PendingTransaction
            {
                SourceNode = movement.MovementSource?.SourceNodeId.GetValueOrDefault().ToString(CultureInfo.InvariantCulture),
                SourceProduct = movement.MovementSource?.SourceProductId,
                DestinationNode = movement.MovementDestination?.DestinationNodeId.GetValueOrDefault().ToString(CultureInfo.InvariantCulture),
                DestinationProduct = movement.MovementDestination?.DestinationProductId,
                StartDate = movement.OperationalDate,
                EndDate = movement.Period?.EndTime,
                Volume = movement.NetStandardVolume.GetValueOrDefault().ToString(CultureInfo.InvariantCulture),
                Units = movement.MeasurementUnit,
                MessageTypeId = MessageType.Movement,
                SystemTypeId = fileRegistrationTransaction.SystemTypeId,
                MessageId = fileRegistrationTransaction.UploadId,
                BlobName = fileRegistrationTransaction.BlobPath,
                ActionTypeId = fileRegistrationTransaction.ActionType,
                ErrorJson = JsonConvert.SerializeObject(errors),
                Identifier = movement.Observations.GetIdentifierRoms(movement.SourceSystemId.GetValueOrDefault().ToString(CultureInfo.InvariantCulture), movement.MovementId),
                SystemName = movement.SourceSystemId,
                SegmentId = movement.SegmentId,
                MessageType = fileRegistrationTransaction.MessageType.ToString(),
            };

            pendingTransaction.ValidateScenarioId(movement.ScenarioId);
            errors.ForEach(x => pendingTransaction.Errors.Add(new PendingTransactionError
            {
                ErrorMessage = x,
                RecordId = fileRegistrationTransaction.RecordId,
            }));
            return pendingTransaction;
        }

        /// <summary>
        /// Converts to pendingtransaction.
        /// </summary>
        /// <param name="homologated">The object.</param>
        /// <param name="fileRegistrationTransaction">The file registration transaction message.</param>
        /// <param name="errors">The errors.</param>
        /// <returns>
        /// The pending transaction.
        /// </returns>
        public static PendingTransaction ToPendingTransaction(this JToken homologated, FileRegistrationTransaction fileRegistrationTransaction, IEnumerable<string> errors)
        {
            ArgumentValidators.ThrowIfNull(homologated, nameof(homologated));
            ArgumentValidators.ThrowIfNull(fileRegistrationTransaction, nameof(fileRegistrationTransaction));
            Enum.TryParse(homologated[Constants.Type].ToString(), true, out MessageType messageType);

            (string identifier, DateTime? startDate, DateTime? endDate, int? segmentId, ScenarioType? scenarioType, string systemName, string originMessageId) =
                 GetIdentifierAndDateDetails(homologated, fileRegistrationTransaction.FileRegistrationCreatedDate);

            var systemId = GetSystemName(systemName, fileRegistrationTransaction.SystemTypeId.ToString());

            var pendingTransaction = new PendingTransaction
            {
                MessageTypeId = (messageType == MessageType.Purchase || messageType == MessageType.Sale) ? MessageType.Contract : messageType,
                SystemTypeId = fileRegistrationTransaction.SystemTypeId,
                MessageId = fileRegistrationTransaction.UploadId,
                BlobName = fileRegistrationTransaction.BlobPath,
                ActionTypeId = fileRegistrationTransaction.ActionType,
                StartDate = startDate,
                EndDate = endDate,
                ErrorJson = JsonConvert.SerializeObject(errors),
                Identifier = identifier,
                SystemName = systemId,
                SegmentId = segmentId,
                MessageType = messageType.ToString(),
            };

            pendingTransaction.ValidateScenarioId(scenarioType.GetValueOrDefault());
            errors.ForEach(x => pendingTransaction.Errors.Add(new PendingTransactionError
            {
                ErrorMessage = x,
                RecordId = fileRegistrationTransaction.RecordId,
            }));

            return pendingTransaction;
        }

        /// <summary>
        /// Converts to pendingtransaction.
        /// </summary>
        /// <param name="eventObject">The Event.</param>
        /// <param name="fileRegistrationTransaction">The file registration transaction message.</param>
        /// <param name="errors">The errors.</param>
        /// <returns>The pending transaction.</returns>
        public static PendingTransaction ToPendingTransaction(this Event eventObject, FileRegistrationTransaction fileRegistrationTransaction, IEnumerable<string> errors)
        {
            ArgumentValidators.ThrowIfNull(eventObject, nameof(eventObject));
            ArgumentValidators.ThrowIfNull(fileRegistrationTransaction, nameof(fileRegistrationTransaction));
            var pendingTransaction = new PendingTransaction
            {
                SourceNode = Convert.ToString(eventObject.SourceNodeId, CultureInfo.InvariantCulture),
                SourceProduct = eventObject.SourceProductId,
                DestinationNode = Convert.ToString(eventObject.DestinationNodeId, CultureInfo.InvariantCulture),
                DestinationProduct = eventObject.DestinationProductId,
                StartDate = eventObject.StartDate,
                EndDate = eventObject.EndDate,
                Volume = Convert.ToString(eventObject.Volume, CultureInfo.InvariantCulture),
                Units = eventObject.MeasurementUnit.ToNullableInt(),
                MessageTypeId = MessageType.Events,
                SystemTypeId = fileRegistrationTransaction.SystemTypeId,
                MessageId = fileRegistrationTransaction.UploadId,
                BlobName = fileRegistrationTransaction.BlobPath,
                ActionTypeId = fileRegistrationTransaction.ActionType,
                ErrorJson = JsonConvert.SerializeObject(errors),
                MessageType = fileRegistrationTransaction.MessageType.ToString(),
            };

            errors.ForEach(x => pendingTransaction.Errors.Add(new PendingTransactionError
            {
                ErrorMessage = x,
                RecordId = fileRegistrationTransaction.RecordId,
            }));
            return pendingTransaction;
        }

        /// <summary>
        /// Converts to pendingtransaction.
        /// </summary>
        /// <param name="contractObject">The contract.</param>
        /// <param name="fileRegistrationTransaction">The file registration transaction message.</param>
        /// <param name="errors">The errors.</param>
        /// <returns>The pending transaction.</returns>
        public static PendingTransaction ToPendingTransaction(this Contract contractObject, FileRegistrationTransaction fileRegistrationTransaction, IEnumerable<string> errors)
        {
            ArgumentValidators.ThrowIfNull(contractObject, nameof(contractObject));
            ArgumentValidators.ThrowIfNull(fileRegistrationTransaction, nameof(fileRegistrationTransaction));
            var pendingTransaction = new PendingTransaction
            {
                SourceNode = Convert.ToString(contractObject.SourceNodeId, CultureInfo.InvariantCulture),
                SourceProduct = contractObject.ProductId,
                DestinationNode = Convert.ToString(contractObject.DestinationNodeId, CultureInfo.InvariantCulture),
                StartDate = contractObject.StartDate == DateTime.MinValue ? DateTime.Now : contractObject.StartDate,
                EndDate = contractObject.EndDate == DateTime.MinValue ? DateTime.Now : contractObject.EndDate,
                Volume = Convert.ToString(contractObject.Volume, CultureInfo.InvariantCulture),
                Units = null,
                MessageTypeId = MessageType.Contract,
                SystemTypeId = fileRegistrationTransaction.SystemTypeId,
                MessageId = fileRegistrationTransaction.UploadId,
                BlobName = fileRegistrationTransaction.BlobPath,
                ActionTypeId = fileRegistrationTransaction.ActionType,
                ErrorJson = JsonConvert.SerializeObject(errors),
                MessageType = fileRegistrationTransaction.MessageType.ToString(),
                OriginMessageId = contractObject.OriginMessageId,
            };

            errors.ForEach(x => pendingTransaction.Errors.Add(new PendingTransactionError
            {
                ErrorMessage = x,
                RecordId = fileRegistrationTransaction.RecordId,
            }));

            return pendingTransaction;
        }

        /// <summary>
        /// Converts to pendingtransaction.
        /// </summary>
        /// <param name="fileRegistration">The file registration.</param>
        /// <param name="errors">The errors.</param>
        /// <returns>The PendingTransaction.</returns>
        public static PendingTransaction ToPendingTransaction(this FileRegistration fileRegistration, IEnumerable<string> errors)
        {
            ArgumentValidators.ThrowIfNull(fileRegistration, nameof(fileRegistration));

            var pendingTransaction = new PendingTransaction
            {
                BlobName = fileRegistration.BlobPath,
                MessageId = fileRegistration.UploadId,
                ErrorJson = JsonConvert.SerializeObject(errors),
            };

            errors.ForEach(x => pendingTransaction.Errors.Add(new PendingTransactionError
            {
                ErrorMessage = x,
            }));

            return pendingTransaction;
        }

        /// <summary>
        /// Adds the pending transactions.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="error">The error.</param>
        /// <param name="entity">Json entity.</param>
        /// <param name="fileRegistrationTransaction">The File Registration Transaction.</param>
        public static void PopulatePendingTransactions(this TrueMessage message, string error, JToken entity, FileRegistrationTransaction fileRegistrationTransaction)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            ArgumentValidators.ThrowIfNull(fileRegistrationTransaction, nameof(fileRegistrationTransaction));

            (string identifier, DateTime? startDate, DateTime? endDate, int? segmentId, ScenarioType? scenarioType, string systemName, string originMessageId) =
                GetIdentifierAndDateDetails(entity, fileRegistrationTransaction.FileRegistrationCreatedDate);

            var systemId = GetSystemName(systemName, fileRegistrationTransaction.SystemTypeId.ToString());

            var pendingTransaction = new PendingTransaction
            {
                SystemTypeId = message.FileRegistration.SystemTypeId,
                MessageId = message.FileRegistration.UploadId,
                ActionTypeId = message.FileRegistration.ActionType,
                ErrorJson = JsonConvert.SerializeObject(error),
                BlobName = fileRegistrationTransaction.BlobPath,
                StartDate = startDate,
                EndDate = endDate,
                Identifier = identifier,
                SystemName = systemId,
                MessageTypeId = message.GetMessageType(entity[Constants.Type].ToString()),
                OriginMessageId = originMessageId,
            };

            pendingTransaction.ValidateScenarioId(scenarioType.GetValueOrDefault());
            pendingTransaction.Errors.Add(new PendingTransactionError { ErrorMessage = error, RecordId = fileRegistrationTransaction.RecordId });
            message.PendingTransactions.Add(pendingTransaction);
        }

        /// <summary>
        /// Adds the pending transactions.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorJson">The errorJson.</param>
        /// <param name="messageType">message type.</param>
        /// <param name="errorMessage">The errorMessage.</param>
        public static void PopulatePendingTransactions(this TrueMessage message, string errorJson, string messageType, string errorMessage)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            var systemOriginName = Enum.IsDefined(typeof(SourceSystem), message.FileRegistration.SystemTypeId.ToString())
                ? (int?)Enum.Parse<SourceSystem>(message.FileRegistration.SystemTypeId.ToString()) : null;

            var pendingTransaction = new PendingTransaction
            {
                SystemTypeId = message.FileRegistration.SystemTypeId,
                MessageId = message.FileRegistration.UploadId,
                ActionTypeId = message.FileRegistration.ActionType,
                StartDate = message.FileRegistration.CreatedDate,
                ErrorJson = JsonConvert.SerializeObject(errorJson),
                BlobName = message.FileRegistration.BlobPath,
                MessageTypeId = message.GetMessageType(messageType),
                SystemName = systemOriginName,
                MessageType = messageType,
            };

            pendingTransaction.Errors.Add(new PendingTransactionError { ErrorMessage = errorMessage });
            message.PendingTransactions.Add(pendingTransaction);
        }

        /// <summary>
        /// Adds the pending transactions.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorJson">The errorJson.</param>
        /// <param name="messageType">message type.</param>
        /// <param name="errorMessage">The errorMessage.</param>
        /// <param name="entity">Json entity.</param>
        public static void PopulatePendingTransactions(this TrueMessage message, string errorJson, string messageType, string errorMessage, JToken entity)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));

            (string identifier, DateTime? startDate, DateTime? endDate, int? segmentId, ScenarioType? scenarioType, string systemName, string originMessageId) =
                GetIdentifierAndDateDetails(entity, message.FileRegistration.CreatedDate);
            var systemOriginName = Enum.IsDefined(typeof(SourceSystem), message.FileRegistration.SystemTypeId.ToString())
                ? (int?)Enum.Parse<SourceSystem>(message.FileRegistration.SystemTypeId.ToString()) : null;

            var pendingTransaction = new PendingTransaction
            {
                SystemTypeId = message.FileRegistration.SystemTypeId,
                MessageId = message.FileRegistration.UploadId,
                ActionTypeId = message.FileRegistration.ActionType,
                StartDate = message.FileRegistration.CreatedDate,
                ErrorJson = JsonConvert.SerializeObject(errorJson),
                BlobName = message.FileRegistration.BlobPath,
                MessageTypeId = message.GetMessageType(messageType),
                SystemName = systemOriginName,
                MessageType = messageType,
                Identifier = identifier,
            };

            pendingTransaction.ValidateScenarioId(scenarioType.GetValueOrDefault());
            pendingTransaction.Errors.Add(new PendingTransactionError { ErrorMessage = errorMessage });
            message.PendingTransactions.Add(pendingTransaction);
        }

        /// <summary>Does the generate.</summary>
        /// <param name="inputObj">The input object.</param>
        /// <returns>The transformation object.</returns>
        public static Transformation ToEntity(this TransformationDto inputObj)
        {
            ArgumentValidators.ThrowIfNull(inputObj, nameof(inputObj));

            return inputObj.MessageTypeId == MessageType.Movement
                ? new Transformation
                {
                    TransformationId = inputObj.TransformationId,
                    OriginSourceNodeId = inputObj.Origin.SourceNodeId,
                    OriginDestinationNodeId = inputObj.Origin.DestinationNodeId,
                    OriginSourceProductId = inputObj.Origin.SourceProductId,
                    OriginDestinationProductId = inputObj.Origin.DestinationProductId,
                    OriginMeasurementId = inputObj.Origin.MeasurementUnitId,
                    DestinationSourceNodeId = inputObj.Destination.SourceNodeId,
                    DestinationDestinationNodeId = inputObj.Destination.DestinationNodeId,
                    DestinationSourceProductId = inputObj.Destination.SourceProductId,
                    DestinationDestinationProductId = inputObj.Destination.DestinationProductId,
                    DestinationMeasurementId = inputObj.Destination.MeasurementUnitId,
                    IsDeleted = false,
                    MessageTypeId = (int)MessageType.Movement,
                    RowVersion = inputObj.RowVersion?.FromBase64(),
                }
                : new Transformation
                {
                    TransformationId = inputObj.TransformationId,
                    OriginSourceNodeId = inputObj.Origin.SourceNodeId,
                    OriginSourceProductId = inputObj.Origin.SourceProductId,
                    OriginMeasurementId = inputObj.Origin.MeasurementUnitId,
                    DestinationSourceNodeId = inputObj.Destination.SourceNodeId,
                    DestinationSourceProductId = inputObj.Destination.SourceProductId,
                    DestinationMeasurementId = inputObj.Destination.MeasurementUnitId,
                    IsDeleted = false,
                    MessageTypeId = (int)MessageType.Inventory,
                    RowVersion = inputObj.RowVersion?.FromBase64(),
                };
        }

        /// <summary>Gets the new movements..</summary>
        /// <param name="movement">The source movement.</param>
        /// <param name="commercialMovementsResult">The input commercial result movement.</param>
        /// <param name="ticket">The ownership ticket .</param>
        /// <param name="movementContract">The movement contract.</param>
        /// <returns>The new movements.</returns>
        public static Movement ToMovementsForPurchaseAndSales(
            this Movement movement,
            CommercialMovementsResult commercialMovementsResult,
            Ticket ticket,
            MovementContract movementContract)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));
            ArgumentValidators.ThrowIfNull(commercialMovementsResult, nameof(commercialMovementsResult));
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));

            var ownership = new Ownership
            {
                OwnerId = commercialMovementsResult.OwnerId,
                OwnershipPercentage = 100.00M,
                OwnershipVolume = commercialMovementsResult.Volume,
                AppliedRule = commercialMovementsResult.AppliedRule,
                RuleVersion = commercialMovementsResult.RuleVersion,
                TicketId = ticket.TicketId,
                ExecutionDate = DateTime.UtcNow.ToTrue(),
                MessageTypeId = MessageType.SpecialMovement,
            };

            var newMovement = commercialMovementsResult.MovementType == Constants.PurchaseMovementType
                ? new Movement
                {
                    MovementDestination = new MovementDestination
                    {
                        DestinationNodeId = movement.MovementDestination.DestinationNodeId,
                        DestinationProductId = movement.MovementDestination.DestinationProductId,
                        DestinationProductTypeId = movement.MovementDestination.DestinationProductTypeId,
                    },
                    MovementTypeId = Convert.ToInt32(Constants.PurchaseMovementTypeId, CultureInfo.InvariantCulture),
                    NetStandardVolume = commercialMovementsResult.Volume,
                    MeasurementUnit = movement.MeasurementUnit,
                    SegmentId = ticket.CategoryElementId,
                    SourceMovementId = movement.MovementTransactionId,
                    OwnershipTicketId = ticket.TicketId,
                    MovementContract = movementContract,
                    OperationalDate = ticket.StartDate.Date,
                    Ownerships = new List<Ownership> { ownership },
                }
                : new Movement
                {
                    MovementSource = new MovementSource
                    {
                        SourceNodeId = movement.MovementDestination.DestinationNodeId,
                        SourceProductId = movement.MovementDestination.DestinationProductId,
                        SourceProductTypeId = movement.MovementDestination.DestinationProductTypeId,
                    },
                    MovementTypeId = Convert.ToInt32(Constants.SaleMovementTypeId, CultureInfo.InvariantCulture),
                    NetStandardVolume = commercialMovementsResult.Volume,
                    MeasurementUnit = movement.MeasurementUnit,
                    SegmentId = ticket.CategoryElementId,
                    SourceMovementId = movement.MovementTransactionId,
                    OperationalDate = ticket.StartDate.Date,
                    OwnershipTicketId = ticket.TicketId,
                    MovementContract = movementContract,
                    Ownerships = new List<Ownership> { ownership },
                };
            newMovement.PopulateDefaultValues();
            return newMovement;
        }

        /// <summary>Gets the new movements..</summary>
        /// <param name="contract">The source contract.</param>
        /// <param name="commercialMovementsResult">The input commercial result movement.</param>
        /// <param name="ticketId">The ownership ticket id.</param>
        /// <returns>The new movements.</returns>
        public static MovementContract ToMovementContractForPurchaseAndSales(this Contract contract, CommercialMovementsResult commercialMovementsResult, int ticketId)
        {
            ArgumentValidators.ThrowIfNull(contract, nameof(contract));
            ArgumentValidators.ThrowIfNull(commercialMovementsResult, nameof(commercialMovementsResult));
            ArgumentValidators.ThrowIfNull(ticketId, nameof(ticketId));
            bool parsePurchaseMovementResult = int.TryParse(Constants.PurchaseMovementTypeId, out int purchaseMovementTypeId);
            bool parseSaleMovementTypeResult = int.TryParse(Constants.SaleMovementTypeId, out int saleMovementTypeId);

            if (parsePurchaseMovementResult && parseSaleMovementTypeResult)
            {
                return new MovementContract
                {
                    DocumentNumber = contract.DocumentNumber,
                    Position = contract.Position,
                    MovementTypeId = commercialMovementsResult.MovementType == Constants.PurchaseMovementType ?
                    purchaseMovementTypeId : saleMovementTypeId,
                    SourceNodeId = contract.SourceNodeId,
                    DestinationNodeId = contract.DestinationNodeId,
                    ProductId = contract.ProductId,
                    StartDate = contract.StartDate,
                    EndDate = contract.EndDate,
                    Owner1Id = contract.Owner1Id,
                    Volume = commercialMovementsResult.Volume,
                    MeasurementUnit = contract.MeasurementUnit,
                    Owner2Id = contract.Owner2Id,
                    IsDeleted = contract.IsDeleted,
                    ContractId = contract.ContractId,
                };
            }

            return new MovementContract();
        }

        /// <summary>Gets the new movements..</summary>
        /// <param name="contract">The source contract.</param>
        /// <returns>The new movements.</returns>
        public static MovementContract ToMovementContractData(this Contract contract)
        {
            ArgumentValidators.ThrowIfNull(contract, nameof(contract));
            var movementContract = new MovementContract
            {
                DocumentNumber = contract.DocumentNumber,
                Position = contract.Position,
                MovementTypeId = contract.MovementTypeId,
                SourceNodeId = contract.SourceNodeId,
                DestinationNodeId = contract.DestinationNodeId,
                ProductId = contract.ProductId,
                StartDate = contract.StartDate,
                EndDate = contract.EndDate,
                Owner1Id = contract.Owner1Id,
                Volume = contract.Volume,
                MeasurementUnit = contract.MeasurementUnit,
                Owner2Id = contract.Owner2Id,
                IsDeleted = contract.IsDeleted,
                ContractId = contract.ContractId,
            };
            return movementContract;
        }

        /// <summary>
        /// Inserts the index of the in inventory movement.
        /// </summary>
        /// <param name="inventoryProduct">The inventory product.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="uniqueIdentifier">The unique identifier.</param>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="dateTime">The date time.</param>
        public static void InsertInInventoryMovementIndex(this InventoryProduct inventoryProduct, IUnitOfWork unitOfWork, string uniqueIdentifier, string eventType, DateTime? dateTime)
        {
            var uniqueHash = string.Join(
               ",", uniqueIdentifier + dateTime + eventType).GetHash(50);
            InsertData(unitOfWork, new InventoryMovementIndex { RecordHashColumn = uniqueHash });
        }

        /// <summary>
        /// Inserts the index of the in inventory movement.
        /// </summary>
        /// <param name="movement">The movement.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="uniqueIdentifier">The unique identifier.</param>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="dateTime">The date time.</param>
        public static void InsertInInventoryMovementIndex(this Movement movement, IUnitOfWork unitOfWork, string uniqueIdentifier, string eventType, DateTime? dateTime)
        {
            var uniqueHash = string.Join(
               ",", uniqueIdentifier + dateTime + eventType).GetHash(50);
            InsertData(unitOfWork, new InventoryMovementIndex { RecordHashColumn = uniqueHash });
        }

        private static void InsertData(IUnitOfWork unitOfWork, InventoryMovementIndex inventoryMovementIndex)
        {
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            var inventoryMovementIndexRepository = unitOfWork.CreateRepository<InventoryMovementIndex>();
            inventoryMovementIndexRepository.Insert(inventoryMovementIndex);
        }

        private static void PopulateDefaultValues(this Movement movement)
        {
            movement.Classification = string.Empty;
            movement.SystemTypeId = Convert.ToInt32(SystemType.TRUE, CultureInfo.InvariantCulture);
            movement.MovementId = DateTime.UtcNow.Ticks.ToString(CultureInfo.InvariantCulture);
            movement.MessageTypeId = Convert.ToInt32(MessageType.SpecialMovement, CultureInfo.InvariantCulture);
            movement.EventType = EventType.Insert.ToString();
            movement.ScenarioId = ScenarioType.OPERATIONAL;
        }

        private static string GetIdentifierRoms(JToken homologated, MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.Inventory:
                case MessageType.Movement:
                case MessageType.Loss:
                case MessageType.SpecialMovement:
                    string messageRomss = GetMessageRomssFromObservations(homologated);

                    if (!string.IsNullOrEmpty(messageRomss))
                    {
                        return messageRomss;
                    }

                    return messageType == MessageType.Inventory ? homologated["InventoryId"].ToString() : homologated["MovementId"].ToString();
                default:
                    return homologated["DocumentNumber"] + string.Empty;
            }
        }

        private static string GetIdentifierRoms(this string observations, string sourceSystemId, string id)
        {
            var sourceSystem = GetSourceSystemId(sourceSystemId);
            var definition = new { idMessageRomss = string.Empty, observations = string.Empty };
            if (ValidateIdRooms(sourceSystem) && observations.IsValidJson())
            {
                definition = JsonConvert.DeserializeAnonymousType(observations, definition);
            }

            return !string.IsNullOrEmpty(definition.idMessageRomss) ? definition.idMessageRomss : id;
        }

        private static int GetSourceSystemId(string sourceSystemId)
        {
            var sourceSystemWasParsed = Enum.TryParse(sourceSystemId, out SourceSystem sourceSystemParsed);
            if (sourceSystemWasParsed)
            {
                return (int)sourceSystemParsed;
            }

            var sourceSystemIdWasParsed = int.TryParse(sourceSystemId, out int sourceSystemIdParsed);
            return sourceSystemIdWasParsed ? sourceSystemIdParsed : 0;
        }

        private static string GetMessageRomssFromObservations(JToken homologated)
        {
            string messageRomss = string.Empty;
            string observations = homologated["Observations"].ToString();
            var definition = new { idMessageRomss = string.Empty, observations = string.Empty };

            var sourceSystem = GetSourceSystemId(homologated["SourceSystemId"]?.ToString());

            if (ValidateIdRooms(sourceSystem) && observations.IsValidJson())
            {
                var observation = JsonConvert.DeserializeAnonymousType(observations, definition);
                messageRomss = observation.idMessageRomss;
            }

            return messageRomss;
        }

        private static bool ValidateIdRooms(int sourceSystem)
        {
            return sourceSystem == (int)SourceSystem.ROMSSGRB || sourceSystem == (int)SourceSystem.ROMSSGRC;
        }

        private static (string identifier, DateTime? startDate, DateTime? endDate, int? segmentId, ScenarioType? scenarioType, string systemName, string originMessageId)
            GetIdentifierAndDateDetails(JToken homologated, DateTime? defaultStartDate)
        {
            string startDateValue = string.Empty;
            string endDateValue = string.Empty;
            Enum.TryParse(homologated[Constants.Type].ToString(), true, out MessageType messageType);
            string identifier = GetIdentifierRoms(homologated, messageType);
            string originMessageId = string.Empty;

            switch (messageType)
            {
                case MessageType.Inventory:
                    startDateValue = homologated["InventoryDate"] != null ? homologated["InventoryDate"].ToString() : string.Empty;
                    break;
                case MessageType.Movement:
                case MessageType.Loss:
                case MessageType.SpecialMovement:
                    startDateValue = homologated["OperationalDate"] != null ? homologated["OperationalDate"].ToString() : string.Empty;
                    endDateValue = homologated.SelectToken("Period.EndTime") != null ? homologated.SelectToken("Period.EndTime").ToString() : string.Empty;
                    break;
                default:
                    identifier = homologated["DocumentNumber"] + string.Empty;
                    originMessageId = GetOriginMessageId(homologated);
                    break;
            }

            (int? segmentId, ScenarioType? scenarioId, string systemName) = GetSegmentAndScenarioType(homologated);

            DateTime? startDate = defaultStartDate;
            DateTime? endDate = null;

            if (DateTime.TryParse(startDateValue, out DateTime initialDate))
            {
                startDate = initialDate;
            }

            if (DateTime.TryParse(endDateValue, out DateTime finalDate))
            {
                endDate = finalDate;
            }

            return (identifier, startDate, endDate, segmentId, scenarioId, systemName, originMessageId);
        }

        private static string GetOriginMessageId(JToken homologated)
        {
            return homologated["OriginMessageId"] != null ? homologated["OriginMessageId"].ToString() : string.Empty;
        }

        private static (int? segmentId, ScenarioType? scenarioId, string systemName) GetSegmentAndScenarioType(JToken homologated)
        {
            int? segmentId = null;
            if (homologated.SelectToken(Constants.SegmentId) != null && int.TryParse(homologated[Constants.SegmentId].ToString(), out int segmentValue))
            {
                segmentId = segmentValue;
            }

            ScenarioType? scenarioId = null;
            if (homologated.SelectToken(Constants.Scenario) != null &&
                Enum.TryParse(Convert.ToString(homologated[Constants.Scenario], CultureInfo.InvariantCulture), true, out ScenarioType scenarioType))
            {
                scenarioId = scenarioType;
            }

            var systemName = homologated["SourceSystemId"] != null ? homologated["SourceSystemId"].ToString() : string.Empty;
            return (segmentId, scenarioId, systemName);
        }

        private static int? GetSystemName(string systemName, string systemType)
        {
            if (Enum.IsDefined(typeof(SourceSystem), systemName))
            {
                return (int)Enum.Parse<SourceSystem>(systemName);
            }
            else
            {
                return Enum.IsDefined(typeof(SourceSystem), systemType) ? (int)Enum.Parse<SourceSystem>(systemType) : (int?)null;
            }
        }

        private static void ValidateScenarioId(this PendingTransaction pendingTransaction, ScenarioType scenarioType)
        {
            var result = ((int)scenarioType).ToString(CultureInfo.InvariantCulture);
            if (!string.IsNullOrWhiteSpace(result) && Enum.TryParse(typeof(ScenarioType), result, true, out object validObject) && Enum.IsDefined(typeof(ScenarioType), validObject))
            {
                pendingTransaction.ScenarioId = scenarioType;
            }
        }
    }
}