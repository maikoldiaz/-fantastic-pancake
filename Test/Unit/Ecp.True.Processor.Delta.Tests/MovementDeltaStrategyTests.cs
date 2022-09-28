// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementDeltaStrategyTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Delta.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Delta.Delta;
    using Ecp.True.Processors.Delta.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class MovementDeltaStrategyTests
    {
        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger> mockLogger;

        /// <summary>
        /// The movement delta strategy.
        /// </summary>
        private MovementDeltaStrategy movementDeltaStrategy;

        // </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();

            this.movementDeltaStrategy =
                new MovementDeltaStrategy(
                    this.mockLogger.Object);
        }

        [TestMethod]
        public void MovementDeltaStrategy_Should_ReturnPostiveMovements_With_Source_Movement_DestinationMovement()
        {
            var originalMovements = new List<OriginalMovement>
            {
                new OriginalMovement
              {
            MovementId = "22",
            CancellationType = "44",
            CreatedBy = "User",
            MovementTransactionId = 115,
            NetStandardVolume = 100,
              },
            };
            var updatedMovement = new List<UpdatedMovement>
            {
                new UpdatedMovement
                {
                    MovementId = "22",
                    MovementTransactionId = 115,
                    NetStandardVolume = 200,
                },
            };
            var originalInventory = new List<OriginalInventory>
            {
                new OriginalInventory
                {
              InventoryProductUniqueId = "510",
              CreatedBy = "FICO",
              CreatedDate = DateTime.UtcNow,
              InventoryProductId = 3255,
              LastModifiedBy = "User",
              LastModifiedDate = DateTime.UtcNow,
              ProductVolume = 5000,
                },
            };
            var resultMovements = new List<ResultMovement>
            {
                new ResultMovement
               {
                Delta = 3500,
                MovementId = "22",
                MovementTransactionId = 115,
                Sign = true,
               },
            };
            var movements = new List<Movement>
             {
                 new Movement
                 {
                     MovementId = "22",
                     Ownerships = new List<Ownership>
                     {
                         new Ownership
                         {
                             OwnerId = 30,
                             OwnershipPercentage = 40,
                             IsDeleted = false,
                         },
                         new Ownership
                         {
                             OwnerId = 27,
                             OwnershipPercentage = 60,
                             IsDeleted = false,
                         },
                     },
                     MovementTransactionId = 115,
                     SegmentId = 326,
                     MovementSource = new MovementSource
                     {
                         SourceNodeId = 12,
                         SourceProductId = "SourceProductId",
                     },
                     MovementDestination = new MovementDestination
                     {
                         DestinationNodeId = 13,
                         DestinationProductId = "DestinationProductId",
                     },
                 },
             };

            var updatedInventory = new List<UpdatedInventory> { new UpdatedInventory { } };
            var cancellationTypes = new List<Annulation> { new Annulation { } };
            var deltaData = new DeltaData { Ticket = new Ticket { TicketId = 123 } };
            deltaData.OriginalMovements = originalMovements;
            deltaData.UpdatedMovements = updatedMovement;
            deltaData.OriginalInventories = originalInventory;
            deltaData.UpdatedInventories = updatedInventory;
            deltaData.CancellationTypes = cancellationTypes;
            deltaData.ResultMovements = resultMovements;
            deltaData.Movements = movements;
            var result = this.movementDeltaStrategy.Build(deltaData);
            var udatedOwnerShipValue = result.Select(x => x.Owners.Where(y => y.OwnerId == 22));
            Assert.IsNotNull(udatedOwnerShipValue);
            Assert.AreEqual(3500, result.FirstOrDefault().NetStandardVolume);
            Assert.AreEqual(1400, result.FirstOrDefault().Owners.FirstOrDefault().OwnershipValue);
            Assert.AreEqual("Volumen", result.FirstOrDefault().Owners.FirstOrDefault().OwnershipValueUnit);
            Assert.IsNotNull(result.FirstOrDefault().MovementSource);
            Assert.IsNotNull(result.FirstOrDefault().MovementDestination);
            Assert.AreEqual(result.FirstOrDefault().MovementDestination.DestinationNodeId, movements.FirstOrDefault().MovementDestination.DestinationNodeId);
            Assert.AreEqual(result.FirstOrDefault().MovementSource.SourceNodeId, movements.FirstOrDefault().MovementSource.SourceNodeId);
        }

        [TestMethod]
        public void MovementDeltaStrategy_Should_ReturnNegativeMovement_DestinationNodeIsNull()
        {
            var originalMovements = new List<OriginalMovement>
            {
                new OriginalMovement
              {
            MovementId = "22",
            CancellationType = "44",
            CreatedBy = "User",
            MovementTransactionId = 115,
            NetStandardVolume = 100,
              },
            };
            var updatedMovement = new List<UpdatedMovement>
            {
                new UpdatedMovement
                {
                    MovementId = "22",
                    MovementTransactionId = 115,
                    NetStandardVolume = 200,
                },
            };
            var originalInventory = new List<OriginalInventory>
            {
                new OriginalInventory
                {
              InventoryProductUniqueId = "510",
              CreatedBy = "FICO",
              CreatedDate = DateTime.UtcNow,
              InventoryProductId = 3255,
              LastModifiedBy = "User",
              LastModifiedDate = DateTime.UtcNow,
              ProductVolume = 5000,
                },
            };
            var resultMovements = new List<ResultMovement>
            {
                new ResultMovement
               {
                Delta = 3500,
                MovementId = "22",
                MovementTransactionId = 115,
                Sign = false,
               },
            };
            var movements = new List<Movement>
             {
                 new Movement
                 {
                     MovementId = "22",
                     Ownerships = new List<Ownership>
                     {
                         new Ownership
                         {
                             OwnerId = 30,
                             OwnershipPercentage = 40,
                         },
                     },
                     MovementTypeId = 44,
                     MovementTransactionId = 115,
                     MovementSource = new MovementSource
                     {
                         SourceNodeId = 120 ,
                         SourceProductId = "10",
                     },
                 },
             };

            var updatedInventory = new List<UpdatedInventory> { new UpdatedInventory { } };
            var cancellationTypes = new List<Annulation>
            {
                new Annulation
                 {
                    SourceMovementTypeId = 44,
                 },
            };
            var deltaData = new DeltaData { Ticket = new Ticket { TicketId = 123 } };
            deltaData.OriginalMovements = originalMovements;
            deltaData.UpdatedMovements = updatedMovement;
            deltaData.OriginalInventories = originalInventory;
            deltaData.UpdatedInventories = updatedInventory;
            deltaData.CancellationTypes = cancellationTypes;
            deltaData.ResultMovements = resultMovements;
            deltaData.Movements = movements;
            var result = this.movementDeltaStrategy.Build(deltaData);
            var udatedOwnerShipValue = result.Select(x => x.MovementDestination != null);
            Assert.IsNotNull(udatedOwnerShipValue);
            Assert.IsNotNull(result.FirstOrDefault().MovementDestination);
            Assert.IsNull(result.FirstOrDefault().MovementSource);
            Assert.AreEqual(120, result.FirstOrDefault().MovementDestination.DestinationNodeId);
            Assert.AreEqual("10", result.FirstOrDefault().MovementDestination.DestinationProductId);
            Assert.AreEqual(1, result.FirstOrDefault().SystemTypeId);
            Assert.AreEqual(true, result.FirstOrDefault().IsSystemGenerated);
        }

        [TestMethod]
        public void MovementDeltaStrategyWithNegativeSign_Should_ReturnMovementWithDestinationNode_SourceNodeIsNull()
        {
            var originalMovements = new List<OriginalMovement>
            {
                new OriginalMovement
              {
            MovementId = "22",
            CancellationType = "44",
            CreatedBy = "User",
            MovementTransactionId = 115,
            NetStandardVolume = 100,
              },
            };
            var updatedMovement = new List<UpdatedMovement>
            {
                new UpdatedMovement
                {
                    MovementId = "22",
                    MovementTransactionId = 115,
                    NetStandardVolume = 200,
                },
            };
            var originalInventory = new List<OriginalInventory>
            {
                new OriginalInventory
                {
              InventoryProductUniqueId = "510",
              CreatedBy = "FICO",
              CreatedDate = DateTime.UtcNow,
              InventoryProductId = 3255,
              LastModifiedBy = "User",
              LastModifiedDate = DateTime.UtcNow,
              ProductVolume = 5000,
                },
            };
            var resultMovements = new List<ResultMovement>
            {
                new ResultMovement
               {
                Delta = 3500,
                MovementId = "22",
                MovementTransactionId = 115,
                Sign = false,
               },
            };
            var movements = new List<Movement>
             {
                 new Movement
                 {
                     MovementId = "22",
                     Ownerships = new List<Ownership>
                     {
                         new Ownership
                         {
                             OwnerId = 30,
                             OwnershipPercentage = 40,
                         },
                     },
                     MovementTypeId = 44,
                     MovementTransactionId = 115,
                     MovementDestination = new MovementDestination
                     {
                         DestinationNodeId = 120 ,
                         DestinationProductId = "10",
                     },
                 },
             };

            var updatedInventory = new List<UpdatedInventory> { new UpdatedInventory { } };
            var cancellationTypes = new List<Annulation>
            {
                new Annulation
                 {
                    SourceMovementTypeId = 44,
                 },
            };
            var deltaData = new DeltaData { Ticket = new Ticket { TicketId = 123 } };
            deltaData.OriginalMovements = originalMovements;
            deltaData.UpdatedMovements = updatedMovement;
            deltaData.OriginalInventories = originalInventory;
            deltaData.UpdatedInventories = updatedInventory;
            deltaData.CancellationTypes = cancellationTypes;
            deltaData.ResultMovements = resultMovements;
            deltaData.Movements = movements;
            var result = this.movementDeltaStrategy.Build(deltaData);
            var udatedOwnerShipValue = result.Select(x => x.MovementSource != null);
            Assert.IsNotNull(udatedOwnerShipValue);
            Assert.IsNotNull(result.FirstOrDefault().MovementSource);
            Assert.IsNull(result.FirstOrDefault().MovementDestination);
            Assert.AreEqual(120, result.FirstOrDefault().MovementSource.SourceNodeId);
            Assert.AreEqual("10", result.FirstOrDefault().MovementSource.SourceProductId);
            Assert.AreEqual("10", result.FirstOrDefault().MovementSource.SourceProductId);
            Assert.AreEqual(1, result.FirstOrDefault().SystemTypeId);
            Assert.AreEqual(true, result.FirstOrDefault().IsSystemGenerated);
        }

        [TestMethod]
        public void MovementDeltaStrategyWithNegativeSign_Should_ReturnMovementsWithDestinationNode_SourceNode()
        {
            var originalMovements = new List<OriginalMovement>
            {
                new OriginalMovement
              {
            MovementId = "22",
            CancellationType = "44",
            CreatedBy = "User",
            MovementTransactionId = 115,
            NetStandardVolume = 100,
              },
            };
            var updatedMovement = new List<UpdatedMovement>
            {
                new UpdatedMovement
                {
                    MovementId = "22",
                    MovementTransactionId = 115,
                    NetStandardVolume = 200,
                },
            };
            var originalInventory = new List<OriginalInventory>
            {
                new OriginalInventory
                {
              InventoryProductUniqueId = "510",
              CreatedBy = "FICO",
              CreatedDate = DateTime.UtcNow,
              InventoryProductId = 3255,
              LastModifiedBy = "User",
              LastModifiedDate = DateTime.UtcNow,
              ProductVolume = 5000,
                },
            };
            var resultMovements = new List<ResultMovement>
            {
                new ResultMovement
               {
                Delta = 3500,
                MovementId = "22",
                MovementTransactionId = 115,
                Sign = false,
               },
            };
            var movements = new List<Movement>
             {
                 new Movement
                 {
                     MovementId = "22",
                     Ownerships = new List<Ownership>
                     {
                         new Ownership
                         {
                             OwnerId = 30,
                             OwnershipPercentage = 40,
                         },
                     },
                     MovementTypeId = 44,
                     MovementTransactionId = 115,
                     MovementDestination = new MovementDestination
                     {
                         DestinationNodeId = 120 ,
                         DestinationProductId = "Destination ProductId",
                     },
                     MovementSource = new MovementSource
                     {
                         SourceNodeId = 120 ,
                         SourceProductId = "Source ProductId",
                     },
                 },
             };

            var updatedInventory = new List<UpdatedInventory> { new UpdatedInventory { } };
            var cancellationTypes = new List<Annulation>
            {
                new Annulation
                 {
                    SourceMovementTypeId = 44,
                 },
            };
            var deltaData = new DeltaData { Ticket = new Ticket { TicketId = 123 } };
            deltaData.OriginalMovements = originalMovements;
            deltaData.UpdatedMovements = updatedMovement;
            deltaData.OriginalInventories = originalInventory;
            deltaData.UpdatedInventories = updatedInventory;
            deltaData.CancellationTypes = cancellationTypes;
            deltaData.ResultMovements = resultMovements;
            deltaData.Movements = movements;
            var result = this.movementDeltaStrategy.Build(deltaData);
            var udatedOwnerShipValue = result.Select(x => x.MovementSource != null);
            Assert.IsTrue(result.Count() == 2);
            Assert.IsNotNull(udatedOwnerShipValue);
            Assert.IsNotNull(result.FirstOrDefault().MovementSource);
            Assert.IsNotNull(result.Last().MovementDestination);
            Assert.AreEqual(120, result.FirstOrDefault().MovementSource.SourceNodeId);
            Assert.AreEqual("Destination ProductId", result.FirstOrDefault().MovementSource.SourceProductId);
            Assert.AreEqual("Source ProductId", result.Last().MovementDestination.DestinationProductId);
            Assert.AreEqual(1, result.FirstOrDefault().SystemTypeId);
            Assert.AreEqual(true, result.FirstOrDefault().IsSystemGenerated);
        }
    }
}
