// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityExtensionsTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Proxies.OwnershipRules.Response;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Entity Extensions Tests.
    /// </summary>
    [TestClass]
    public class EntityExtensionsTests
    {
        /// <summary>
        /// Should Return Movement for purchases and sales.
        /// </summary>
        [TestMethod]
        public void ToMovementsForPurchaseAndSales_ShouldReturnMovement()
        {
            // Arrange
            var movement = new Movement
            {
                MovementTransactionId = 1,
                MessageTypeId = 1,
                SystemTypeId = 3,
                MovementDestination = new MovementDestination
                {
                    DestinationNodeId = 20,
                    DestinationProductId = "30",
                    DestinationProductTypeId = 40,
                },
                MeasurementUnit = 31,
                SegmentId = 1042,
                OperationalDate = DateTime.UtcNow,
                GrossStandardVolume = 4000,
                NetStandardVolume = 9000,
            };

            var commercialMovementsResult = new CommercialMovementsResult
            {
                MovementId = 120,
                MovementType = Constants.PurchaseMovementType,
                ContractId = 10,
                OwnerId = 30,
                Volume = 8000,
                AppliedRule = "6",
                RuleVersion = "1",
            };

            var movementContract = new MovementContract
            {
                MovementContractId = 10,
                DocumentNumber = 12300102,
                Position = 1,
                SourceNodeId = 368,
                DestinationNodeId = 367,
                ProductId = "10000002318",
                Owner1Id = 30,
                Owner2Id = 29,
                Volume = 8000,
                MeasurementUnit = 31,
                IsDeleted = false,
                MovementTypeId = 20,
            };

            var ticket = new Ticket
            {
                TicketId = 12375,
            };

            // Act
            var result = movement.ToMovementsForPurchaseAndSales(commercialMovementsResult, ticket, movementContract);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(movementContract, result.MovementContract);
        }

        /// <summary>
        /// Should Return Movement contract for purchases and sales.
        /// </summary>
        [TestMethod]
        public void ToMovementContractForPurchaseAndSales_ShouldReturnMovementContract()
        {
            // Arrange
            var contract = new Ecp.True.Entities.Registration.Contract
            {
                ContractId = 10,
                DocumentNumber = 12300102,
                Position = 1,
                SourceNodeId = 368,
                DestinationNodeId = 367,
                ProductId = "10000002318",
                Owner1Id = 30,
                Owner2Id = 29,
                Volume = 8000,
                MeasurementUnit = 31,
                IsDeleted = false,
                MovementTypeId = 20,
            };
            var commercialMovementsResult = new CommercialMovementsResult
            {
                MovementId = 120,
                MovementType = Constants.PurchaseMovementType,
                ContractId = 10,
                OwnerId = 30,
                Volume = 8000,
                AppliedRule = "6",
                RuleVersion = "1",
            };

            // Act
            var result = contract.ToMovementContractForPurchaseAndSales(commercialMovementsResult, 12375);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(contract.DocumentNumber, result.DocumentNumber);
            Assert.AreEqual(contract.Volume, result.Volume);
            Assert.AreEqual(contract.Owner1Id, result.Owner1Id);
            Assert.AreEqual(contract.Owner2Id, result.Owner2Id);
        }

        /// <summary>
        /// Should return PendingTransaction.
        /// </summary>
        [TestMethod]
        public void ToPendingTransaction_ShouldReturnPendingTransaction()
        {
            // Arrange
            var inventoryProduct = new InventoryProduct()
            {
                ProductId = "10000002372",
                ProductType = 1139,
                ProductVolume = 3000.00M,
                MeasurementUnit = 31,
                SystemTypeId = 3,
                NodeId = 100,
                InventoryDate = DateTime.Now,
                InventoryId = "10",
                SegmentId = 1125,
                SourceSystemId = 165,
            };
            var fileRegistrationTransaction = new FileRegistrationTransaction()
            {
                SystemTypeId = SystemType.EXCEL,
                UploadId = "100",
                BlobPath = "TestBlobPath",
                ActionType = FileRegistrationActionType.Insert,
                RecordId = "10",
            };
            var errors = new List<string>();

            // Act
            var result = inventoryProduct.ToPendingTransaction(fileRegistrationTransaction, errors);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(inventoryProduct.ProductId, result.SourceProduct);
            Assert.AreEqual(fileRegistrationTransaction.BlobPath, result.BlobName);
        }

        /// <summary>
        /// Should return PendingTransaction by taking movement.
        /// </summary>
        [TestMethod]
        public void ToPendingTransactionFromMovement_ShouldReturnPendingTransaction()
        {
            // Arrange
            var movement = new Movement()
            {
                MovementSource = new MovementSource
                {
                    SourceNodeId = 10,
                    SourceProductId = "100",
                    SourceProductTypeId = 1000,
                },
                MovementDestination = new MovementDestination
                {
                    DestinationNodeId = 10,
                    DestinationProductId = "100",
                    DestinationProductTypeId = 1000,
                },
                MeasurementUnit = 31,
                SystemTypeId = 3,
                SegmentId = 1125,
                OperationalDate = DateTime.UtcNow,
                Period = new MovementPeriod(),
                NetStandardVolume = 100.00M,
                MovementId = "21",
                SourceSystemId = 161,
                Observations = "{\"idMessageRomss\":\"42423424\", \"observations\":\"Observaciones del Movimiento\"}",
            };
            var fileRegistrationTransaction = new FileRegistrationTransaction()
            {
                SystemTypeId = SystemType.EXCEL,
                UploadId = "100",
                BlobPath = "TestBlobPath",
                ActionType = FileRegistrationActionType.Insert,
                RecordId = "10",
            };
            var errors = new List<string>();

            // Act
            var result = movement.ToPendingTransaction(fileRegistrationTransaction, errors);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(movement.NetStandardVolume, movement.NetStandardVolume);
            Assert.AreEqual(fileRegistrationTransaction.BlobPath, result.BlobName);
        }

        /// <summary>
        /// Should return PendingTransaction by taking movement roomsGRB.
        /// </summary>
        [TestMethod]
        public void ToPendingTransactionFromMovementRoomsGRB_ShouldReturnPendingTransaction()
        {
            // Arrange
            var movement = new Movement()
            {
                MovementSource = new MovementSource
                {
                    SourceNodeId = 10,
                    SourceProductId = "100",
                    SourceProductTypeId = 1000,
                },
                MovementDestination = new MovementDestination
                {
                    DestinationNodeId = 10,
                    DestinationProductId = "100",
                    DestinationProductTypeId = 1000,
                },
                MeasurementUnit = 31,
                SystemTypeId = 3,
                SegmentId = 1125,
                OperationalDate = DateTime.UtcNow,
                Period = new MovementPeriod(),
                NetStandardVolume = 100.00M,
                MovementId = "21",
                SourceSystemId = 160,
                Observations = "{\"idMessageRomss\":\"42423424\", \"observations\":\"Observaciones del Movimiento\"}",
            };
            var fileRegistrationTransaction = new FileRegistrationTransaction()
            {
                SystemTypeId = SystemType.EXCEL,
                UploadId = "100",
                BlobPath = "TestBlobPath",
                ActionType = FileRegistrationActionType.Insert,
                RecordId = "10",
            };
            var errors = new List<string>();

            // Act
            var result = movement.ToPendingTransaction(fileRegistrationTransaction, errors);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(movement.NetStandardVolume, movement.NetStandardVolume);
            Assert.AreEqual(fileRegistrationTransaction.BlobPath, result.BlobName);
        }

        /// <summary>
        /// Should return PendingTransaction by taking JToken.
        /// </summary>
        [TestMethod]
        public void ToPendingTransactionFromHomologated_ShouldReturnPendingTransaction()
        {
            // Arrange
            var homologated = new JObject
            {
                { Constants.Type, "1" },
                { Constants.SegmentId, "TestSegmentValue" },
                { "SystemName", "TestSystemName" },
                { "Observations", "{\"idMessageRomss\":\"42423424\", \"observations\":\"Observaciones del Movimiento\"}" },
                { "SourceSystemId", "161" },
                { "OriginMessageId", "12345" },
            };

            var fileRegistrationTransaction = new FileRegistrationTransaction()
            {
                SystemTypeId = SystemType.EXCEL,
                UploadId = "100",
                BlobPath = "TestBlobPath",
                ActionType = FileRegistrationActionType.Insert,
                RecordId = "10",
                FileRegistrationCreatedDate = DateTime.UtcNow,
            };
            var errors = new List<string>();

            // Act
            var result = homologated.ToPendingTransaction(fileRegistrationTransaction, errors);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(fileRegistrationTransaction.BlobPath, result.BlobName);
            Assert.AreEqual(fileRegistrationTransaction.UploadId, result.MessageId);
        }

        /// <summary>
        /// Should return PendingTransaction by taking Event.
        /// </summary>
        [TestMethod]
        public void ToPendingTransactionFromEvent_ShouldReturnPendingTransaction()
        {
            // Arrange
            var eventObject = new Entities.Registration.Event()
            {
                SourceNodeId = 10,
                SourceProductId = "100",
                DestinationNodeId = 20,
                DestinationProductId = "200",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow,
                Volume = 1000.00M,
                MeasurementUnit = "31",
            };

            var fileRegistrationTransaction = new FileRegistrationTransaction()
            {
                SystemTypeId = SystemType.EXCEL,
                UploadId = "100",
                BlobPath = "TestBlobPath",
                ActionType = FileRegistrationActionType.Insert,
                RecordId = "10",
                FileRegistrationCreatedDate = DateTime.UtcNow,
            };
            var errors = new List<string>();

            // Act
            var result = eventObject.ToPendingTransaction(fileRegistrationTransaction, errors);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(fileRegistrationTransaction.BlobPath, result.BlobName);
            Assert.AreEqual(fileRegistrationTransaction.UploadId, result.MessageId);
            Assert.AreEqual(eventObject.MeasurementUnit, result.Units.ToString());
        }

        /// <summary>
        /// Should return PendingTransaction by taking contract.
        /// </summary>
        [TestMethod]
        public void ToPendingTransactionFromContract_ShouldReturnPendingTransaction()
        {
            // Arrange
            var contractObject = new Entities.Registration.Contract()
            {
                SourceNodeId = 10,
                ProductId = "100",
                DestinationNodeId = 20,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow,
                Volume = 1000.00M,
                MeasurementUnit = 31,
            };

            var fileRegistrationTransaction = new FileRegistrationTransaction()
            {
                SystemTypeId = SystemType.EXCEL,
                UploadId = "100",
                BlobPath = "TestBlobPath",
                ActionType = FileRegistrationActionType.Insert,
                RecordId = "10",
                FileRegistrationCreatedDate = DateTime.UtcNow,
            };
            var errors = new List<string>();

            // Act
            var result = contractObject.ToPendingTransaction(fileRegistrationTransaction, errors);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(fileRegistrationTransaction.BlobPath, result.BlobName);
            Assert.AreEqual(fileRegistrationTransaction.UploadId, result.MessageId);
            Assert.AreEqual(contractObject.ProductId, result.SourceProduct);
        }

        /// <summary>
        /// Should return PendingTransaction.
        /// </summary>
        [TestMethod]
        public void PopulatePendingTransactions_ShouldReturnPendingTransaction()
        {
            // Arrange
            var message = new TrueMessage()
            {
                FileRegistration = new FileRegistration
                {
                    SystemTypeId = SystemType.EVENTS,
                    UploadId = "10",
                    ActionType = FileRegistrationActionType.Insert,
                    CreatedDate = DateTime.UtcNow,
                },
            };

            JObject entity = new JObject
            {
                { Constants.Type, "TestTypeValue" },
                { Constants.SegmentId, "TestSegmentValue" },
                { "Observations", "{\"idMessageRomss\":\"42423424\", \"observations\":\"Observaciones del Movimiento\"}" },
                { "SystemName", "TestSystemName" },
                { "OriginMessageId", "123456" },
            };

            var fileRegistrationTransaction = new FileRegistrationTransaction()
            {
                SystemTypeId = SystemType.EXCEL,
                UploadId = "100",
                BlobPath = "TestBlobPath",
                ActionType = FileRegistrationActionType.Insert,
                RecordId = "10",
                FileRegistrationCreatedDate = DateTime.UtcNow,
            };

            // Act
            message.PopulatePendingTransactions(string.Empty, entity, fileRegistrationTransaction);

            // Assert
            Assert.IsNotNull(message.PendingTransactions);
        }

        /// <summary>
        /// Should return PendingTransaction.
        /// </summary>
        [TestMethod]
        public void PopulatePendingTransactionsFromTrueMessage_ShouldReturnPendingTransaction()
        {
            // Arrange
            var message = new TrueMessage()
            {
                FileRegistration = new FileRegistration
                {
                    SystemTypeId = SystemType.EXCEL,
                    UploadId = "10",
                    ActionType = FileRegistrationActionType.Insert,
                    CreatedDate = DateTime.UtcNow,
                    BlobPath = "Test Blob Path",
                },
            };

            // Act
            message.PopulatePendingTransactions(string.Empty, "Movement", Constants.TechnicalExceptionParsingErrorMessage);

            // Assert
            Assert.IsNotNull(message.PendingTransactions);
        }

        /// <summary>
        /// Should return Transformation.
        /// </summary>
        [TestMethod]
        public void ToEntity_ShouldReturnTransformation()
        {
            // Arrange
            var inputObj = new TransformationDto()
            {
                MessageTypeId = Entities.Core.MessageType.Events,
                TransformationId = 10,
                Origin = new Origin
                {
                    SourceNodeId = 10,
                    DestinationNodeId = 20,
                    SourceProductId = "100",
                    DestinationProductId = "200",
                    MeasurementUnitId = 31,
                },
                Destination = new Destination
                {
                    SourceNodeId = 10,
                    DestinationNodeId = 20,
                    SourceProductId = "100",
                    DestinationProductId = "200",
                    MeasurementUnitId = 31,
                },
            };

            // Act
            var result = inputObj.ToEntity();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.TransformationId, inputObj.TransformationId);
        }

        /// <summary>
        /// Copies the property values should return copied properties.
        /// </summary>
        [TestMethod]
        public void CopyPropertyValues_ShouldReturnCopiedProperties()
        {
            var origin = new Origin
            {
                SourceNodeId = 10,
                DestinationNodeId = 20,
                SourceProductId = "100",
                DestinationProductId = "200",
                MeasurementUnitId = 31,
            };
            var destination = origin.CopyPropertyValuesWithName<Origin, Destination>();
            Assert.IsNotNull(destination);
            Assert.AreEqual(destination.GetType(), typeof(Destination));
        }
    }
}
