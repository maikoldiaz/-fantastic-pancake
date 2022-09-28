// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaExtensionTests.cs" company="Microsoft">
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
    using System.Globalization;
    using System.Linq;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Delta;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Proxies.OwnershipRules.Response;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The ValidationExecutorTests.
    /// </summary>
    [TestClass]
    public class DeltaExtensionTests
    {
        [TestMethod]
        public void ToRequest_OfficialDeltaConsolidatedMovement()
        {
            var consolidatedOwners = new ConsolidatedOwner { OwnerId = 123, OwnershipVolume = 24.125M };
            var consolidatedMovement = new List<ConsolidatedMovement>
            {
                new ConsolidatedMovement { ConsolidatedMovementId = 1, DestinationNodeId = 2 },
            };
            consolidatedMovement.First().ConsolidatedOwners.Add(consolidatedOwners);
            var officialDeltaConsolidatedMovement = DeltaExtensions.ToRequest(consolidatedMovement);
            Assert.IsNotNull(officialDeltaConsolidatedMovement);
            Assert.AreEqual("1", officialDeltaConsolidatedMovement.First().ConsolidatedMovementId);
            Assert.AreEqual(24.13M, officialDeltaConsolidatedMovement.First().OwnershipVolume);
        }

        [TestMethod]
        public void ToRequest_OfficialDeltaCancellationTypes()
        {
            var cancellationTypes = new List<Annulation>
            {
                new Annulation { SourceMovementTypeId = 1, AnnulationMovementTypeId = 2 },
            };
            var officialDeltaCancellationTypes = DeltaExtensions.ToRequest(cancellationTypes);
            Assert.IsNotNull(officialDeltaCancellationTypes);
            Assert.AreEqual("1", officialDeltaCancellationTypes.First().SourceMovementTypeId);
        }

        [TestMethod]
        public void ToRequest_OfficialDeltaPendingOfficialInventory()
        {
            var pendingOfficialInventory = new List<PendingOfficialInventory>
            {
                new PendingOfficialInventory { InventoryProductID = 1, InventoryProductUniqueId = "2", InventoryDate = DateTime.Today, NodeId = 1, OwnerShipValue = 28.333M },
            };

            var ticket = new Ticket { StartDate = DateTime.Today.AddDays(-1), EndDate = DateTime.Today };
            var officialDeltaPendingOfficialInventory = DeltaExtensions.ToRequest(pendingOfficialInventory, ticket);
            Assert.IsNotNull(officialDeltaPendingOfficialInventory);
            Assert.AreEqual("1", officialDeltaPendingOfficialInventory.First().InventoryProductID);
            Assert.AreEqual(28.33M, officialDeltaPendingOfficialInventory.First().OwnerShipValue);
            Assert.AreEqual(pendingOfficialInventory.First().InventoryDate.ToString(Constants.DateStringFormat, CultureInfo.InvariantCulture), officialDeltaPendingOfficialInventory.First().InventoryDate);
        }

        [TestMethod]
        public void ToRequest_OfficialDeltaConsolidatedInventoryProduct()
        {
            var consolidatedOwners = new ConsolidatedOwner { OwnerId = 123, OwnershipVolume = 28.333M };
            var consolidatedInventoryProduct = new List<ConsolidatedInventoryProduct>
            {
                new ConsolidatedInventoryProduct { ConsolidatedInventoryProductId = 1, NodeId = 2, InventoryDate = new DateTime(2020, 6, 1) },
            };
            consolidatedInventoryProduct.First().ConsolidatedOwners.Add(consolidatedOwners);
            var officialDeltaConsolidatedInventoryProduct = DeltaExtensions.ToRequest(consolidatedInventoryProduct);
            Assert.IsNotNull(officialDeltaConsolidatedInventoryProduct);
            Assert.AreEqual("1", officialDeltaConsolidatedInventoryProduct.First().ConsolidatedInventoryProductId);
            Assert.AreEqual(28.33M, officialDeltaConsolidatedInventoryProduct.First().OwnershipVolume);
            Assert.AreEqual(consolidatedInventoryProduct.First().InventoryDate.ToString(Constants.DateStringFormat, CultureInfo.InvariantCulture), officialDeltaConsolidatedInventoryProduct.First().InventoryDate);
        }

        [TestMethod]
        public void ToRequest_OfficialDeltaPendingOfficialMovement()
        {
            var pendingOfficialMovement = new List<PendingOfficialMovement>
            {
                new PendingOfficialMovement { MovementTransactionId = 1, DestinationNodeId = 2, OwnerShipValue = 23.333M },
            };
            var officialDeltaPendingOfficialMovement = DeltaExtensions.ToRequest(pendingOfficialMovement);
            Assert.IsNotNull(officialDeltaPendingOfficialMovement);
            Assert.AreEqual("1", officialDeltaPendingOfficialMovement.First().MovementTransactionId);
            Assert.AreEqual(23.33M, officialDeltaPendingOfficialMovement.First().OwnerShipValue);
        }

        [TestMethod]
        public void ToRequest_OriginalMovement()
        {
            var originalMovement = new List<OriginalMovement>
            {
                new OriginalMovement { MovementId = "abc", MovementTransactionId = 1 },
            };
            var deltaOriginalMovement = DeltaExtensions.ToRequest(originalMovement);
            Assert.IsNotNull(deltaOriginalMovement);
            Assert.AreEqual("abc", deltaOriginalMovement.First().MovementId);
        }

        [TestMethod]
        public void ToRequest_UpdatedMovement()
        {
            var updatedMovement = new List<UpdatedMovement>
            {
                new UpdatedMovement { MovementId = "abc", MovementTransactionId = 1, EventType = "test" },
            };
            var deltaUpdatedMovement = DeltaExtensions.ToRequest(updatedMovement);
            Assert.IsNotNull(deltaUpdatedMovement);
            Assert.AreEqual("abc", deltaUpdatedMovement.First().MovementId);
        }

        [TestMethod]
        public void ToRequest_OriginalInventory()
        {
            var originalInventory = new List<OriginalInventory>
            {
                new OriginalInventory { InventoryProductUniqueId = "abc", InventoryProductId = 1 },
            };
            var deltaOriginalInventory = DeltaExtensions.ToRequest(originalInventory);
            Assert.IsNotNull(deltaOriginalInventory);
            Assert.AreEqual("abc", deltaOriginalInventory.First().InventoryProductUniqueId);
        }

        [TestMethod]
        public void ToRequest_UpdatedInventory()
        {
            var updatedInventory = new List<UpdatedInventory>
            {
                new UpdatedInventory { InventoryProductUniqueId = "abc", InventoryProductId = 1, EventType = "test" },
            };
            var deltaUpdatedInventory = DeltaExtensions.ToRequest(updatedInventory);
            Assert.IsNotNull(deltaUpdatedInventory);
            Assert.AreEqual("abc", deltaUpdatedInventory.First().InventoryProductUniqueId);
        }

        [TestMethod]
        public void ToResponse_OfficialDeltaResultInventory()
        {
            var officialDeltaResultInventory = new List<OfficialDeltaResultInventory>
            {
                new OfficialDeltaResultInventory { InventoryTransactionId = "1", Sign = "POSITIVO", Origin = Entities.Enumeration.OriginType.OFICIAL },
            };
            var officialResultInventory = DeltaExtensions.ToResponse(officialDeltaResultInventory);
            Assert.IsNotNull(officialResultInventory);
            Assert.AreEqual(1, officialResultInventory.First().TransactionId);
        }

        [TestMethod]
        public void ToResponse_OfficialDeltaResultInventoryOfficialDelta()
        {
            var officialDeltaResultInventory = new List<OfficialDeltaResultInventory>
            {
                new OfficialDeltaResultInventory { MovementTransactionId = "1", Sign = "POSITIVO", Origin = Entities.Enumeration.OriginType.DELTAOFICIAL },
            };
            var officialResultInventory = DeltaExtensions.ToResponse(officialDeltaResultInventory);
            Assert.IsNotNull(officialResultInventory);
            Assert.AreEqual(1, officialResultInventory.First().TransactionId);
        }

        [TestMethod]
        public void ToResponse_OfficialDeltaResultInventoryOfficialDeltaWhenSignIsOtherThanPositiveOrNegative()
        {
            var officialDeltaResultInventory = new List<OfficialDeltaResultInventory>
            {
                new OfficialDeltaResultInventory { MovementTransactionId = "1", Sign = "IGUAL", Origin = Entities.Enumeration.OriginType.DELTAOFICIAL },
            };
            var officialResultInventory = DeltaExtensions.ToResponse(officialDeltaResultInventory);
            Assert.IsTrue(!officialResultInventory.Any());
        }

        [TestMethod]
        public void ToResponse_OfficialDeltaErrorInventory()
        {
            var officialDeltaErrorInventory = new List<OfficialDeltaErrorInventory>
            {
                new OfficialDeltaErrorInventory { InventoryTransactionId = "1", Origin = Entities.Enumeration.OriginType.OFICIAL },
            };
            var officialErrorInventory = DeltaExtensions.ToResponse(officialDeltaErrorInventory);
            Assert.IsNotNull(officialErrorInventory);
            Assert.AreEqual(1, officialErrorInventory.First().InventoryProductId);
        }

        [TestMethod]
        public void ToResponse_OfficialDeltaErrorMovement()
        {
            var officialDeltaErrorMovement = new List<OfficialDeltaErrorMovement>
            {
                new OfficialDeltaErrorMovement { MovementTransactionId = "1", Origin = Entities.Enumeration.OriginType.OFICIAL },
            };
            var officialErrorMovement = DeltaExtensions.ToResponse(officialDeltaErrorMovement);
            Assert.IsNotNull(officialErrorMovement);
            Assert.AreEqual(1, officialErrorMovement.First().MovementTransactionId);
        }

        [TestMethod]
        public void ToResponse_DeltaResultInventory()
        {
            var deltaResultInventory = new List<DeltaResultInventory>
            {
                new DeltaResultInventory { InventoryTransactionId = 1, Sign = "POSITIVO" },
            };
            var resultInventory = DeltaExtensions.ToResponse(deltaResultInventory);
            Assert.IsNotNull(resultInventory);
            Assert.AreEqual(1, resultInventory.First().InventoryProductId);
        }

        [TestMethod]
        public void ToResponse_DeltaResultMovement()
        {
            var deltaResultMovement = new List<DeltaResultMovement>
            {
                new DeltaResultMovement { MovementTransactionId = 1, Sign = "POSITIVO", },
            };
            var resultMovement = DeltaExtensions.ToResponse(deltaResultMovement);
            Assert.IsNotNull(resultMovement);
            Assert.AreEqual(1, resultMovement.First().MovementTransactionId);
        }

        [TestMethod]
        public void ToResponse_DeltaErrorInventory()
        {
            var deltaErrorInventory = new List<DeltaErrorInventory>
            {
                new DeltaErrorInventory { InventoryTransactionId = 1 },
            };
            var errorInventory = DeltaExtensions.ToResponse(deltaErrorInventory);
            Assert.IsNotNull(errorInventory);
            Assert.AreEqual(1, errorInventory.First().InventoryProductId);
        }

        [TestMethod]
        public void ToResponse_DeltaErrorMovement()
        {
            var deltaErrorMovement = new List<DeltaErrorMovement>
            {
                new DeltaErrorMovement { MovementTransactionId = 1 },
            };
            var errorMovement = DeltaExtensions.ToResponse(deltaErrorMovement);
            Assert.IsNotNull(errorMovement);
            Assert.AreEqual(1, errorMovement.First().MovementTransactionId);
        }

        [TestMethod]
        public void ToRequest_OfficialDeltaMovement()
        {
            var officialDeltaMovement = new OfficialDeltaMovement
            {
                MovementTransactionId = 123,
                MovementOwnerId = 100,
                SourceNodeId = 10,
                DestinationNodeId = 20,
                SourceProductId = "30",
                DestinationProductId = "40",
                MovementTypeId = 9,
                OwnerId = 200,
                OwnershipVolume = 100,
            };
            var officialDeltaMovements = new List<OfficialDeltaMovement> { officialDeltaMovement };
            var resultOfficialDeltaMovement = DeltaExtensions.ToRequest(officialDeltaMovements);
            Assert.IsNotNull(resultOfficialDeltaMovement);
            Assert.AreEqual("123", resultOfficialDeltaMovement.First().MovementTransactionId);
            Assert.AreEqual("10", resultOfficialDeltaMovement.First().SourceNodeId);
            Assert.AreEqual("200", resultOfficialDeltaMovement.First().OwnerId);
            Assert.AreEqual(100, resultOfficialDeltaMovement.First().OwnershipVolume);
        }

        [TestMethod]
        public void ToRequest_OfficialDeltaInventory()
        {
            var officialDeltaInventory = new OfficialDeltaInventory
            {
                MovementTransactionId = 123,
                MovementOwnerId = 100,
                OperationalDate = new DateTime(2020, 1, 2),
                NodeId = 10,
                OwnerId = 200,
                OwnershipVolume = 100,
            };
            var officialDeltaInventories = new List<OfficialDeltaInventory> { officialDeltaInventory };
            var resultOfficialDeltaInventory = DeltaExtensions.ToRequest(officialDeltaInventories);
            Assert.IsNotNull(resultOfficialDeltaInventory);
            Assert.AreEqual("123", resultOfficialDeltaInventory.First().MovementTransactionId);
            Assert.AreEqual("10", resultOfficialDeltaInventory.First().NodeId);
            Assert.AreEqual("200", resultOfficialDeltaInventory.First().OwnerId);
            Assert.AreEqual(100, resultOfficialDeltaInventory.First().OwnershipVolume);
        }

        [TestMethod]
        public void GetNodeList_ShouldGetNodes()
        {
            var pendingOfficialInventory = new PendingOfficialInventory() { NodeId = 10 };
            var pendingOfficialMovement = new PendingOfficialMovement() { SourceNodeId = 20, DestinationNodeId = 30 };
            var officialDeltaData = new OfficialDeltaData
            {
                Ticket = new Ticket() { TicketId = 100 },
                PendingOfficialInventories = new List<PendingOfficialInventory>() { pendingOfficialInventory },
                PendingOfficialMovements = new List<PendingOfficialMovement>() { pendingOfficialMovement },
            };
            var result = DeltaExtensions.GetPendingOfficialMovementNodes(officialDeltaData);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Distinct().Count());
            Assert.AreEqual(10, result.First());
            Assert.AreEqual(20, result.ToArray()[1]);
            Assert.AreEqual(30, result.ToArray()[2]);
        }
    }
}
