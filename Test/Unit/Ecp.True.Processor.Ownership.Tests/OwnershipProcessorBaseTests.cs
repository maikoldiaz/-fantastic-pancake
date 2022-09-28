// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipProcessorBaseTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Ownership.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Ownership;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The OwnershipProcessorTests.
    /// </summary>
    [TestClass]
    public class OwnershipProcessorBaseTests : OwnershipProcessorBase
    {
        /// <summary>
        /// The mock repository factory.
        /// </summary>
        private IEnumerable<Movement> movements;

        /// <summary>
        /// The mock repository factory.
        /// </summary>
        private IEnumerable<InventoryProduct> inventories;

        /// <summary>
        /// Gets the distinct owners.
        /// </summary>
        [TestMethod]
        public void GetOwners_ShouldGetOwnersPerProductAndNode_WithSuccess()
        {
            this.SetupData(100);
            var distinctOwners = GetOwners(this.movements, this.inventories, "P1", 30, new List<int> { 100 });
            Assert.IsNotNull(distinctOwners);
            Assert.AreEqual(1, distinctOwners.Count());
            Assert.AreEqual(100, distinctOwners.ElementAt(0));
        }

        /// <summary>
        /// Setups the data.
        /// </summary>
        /// <param name="ownershipTicketId">The ownership ticket identifier.</param>
        private void SetupData(int ownershipTicketId)
        {
            var movementOne = this.GetMovement(100, ownershipTicketId, 100, "1000");
            var movementSourceOne = this.GetMovementSource(100, 100, 100, "P1");
            var sourceNodeOne = this.GetNode(100);
            var sourceProductOne = this.GetProduct("P1");
            var movementDestinationOne = this.GetMovementDestination(100, 100, 100, "P1");
            var destinationNodeOne = this.GetNode(100);
            var destinationProductOne = this.GetProduct("P1");
            var movementOwnershipOne = this.GetOwnership(100, 100, MessageType.MovementOwnership, ownershipTicketId, 100, null, 0);

            movementSourceOne.SourceNode = sourceNodeOne;
            movementSourceOne.SourceProduct = sourceProductOne;
            movementDestinationOne.DestinationNode = destinationNodeOne;
            movementDestinationOne.DestinationProduct = destinationProductOne;

            movementOne.MovementSource = movementSourceOne;
            movementOne.MovementDestination = movementDestinationOne;
            movementOne.Ownerships.Add(movementOwnershipOne);

            var movementTwo = this.GetMovement(200, ownershipTicketId, 200, "2000");
            var movementSourceTwo = this.GetMovementSource(200, 200, 200, "P2");
            var sourceNodeTwo = this.GetNode(200);
            var sourceProductTwo = this.GetProduct("P1");
            var movementDestinationTwo = this.GetMovementDestination(200, 200, 200, "P2");
            var destinationNodeTwo = this.GetNode(200);
            var destinationProductTwo = this.GetProduct("P2");
            var movementOwnershipTwo = this.GetOwnership(200, 200, MessageType.MovementOwnership, ownershipTicketId, 200, null, 0);

            movementSourceTwo.SourceNode = sourceNodeTwo;
            movementSourceTwo.SourceProduct = sourceProductTwo;
            movementDestinationTwo.DestinationNode = destinationNodeTwo;
            movementDestinationTwo.DestinationProduct = destinationProductTwo;

            movementTwo.MovementSource = movementSourceTwo;
            movementTwo.MovementDestination = movementDestinationTwo;
            movementTwo.Ownerships.Add(movementOwnershipTwo);

            var inventoryProductOne = this.GetInventoryProduct(100, "P1", ownershipTicketId, "INV1", 100, 100);
            var inventoryNodeOne = this.GetNode(100);
            var productOne = this.GetProduct("P1");

            inventoryProductOne.Node = inventoryNodeOne;
            inventoryProductOne.Product = productOne;
            inventoryProductOne.InventoryDate = DateTime.UtcNow;

            var inventoryProductTwo = this.GetInventoryProduct(200, "P2", ownershipTicketId, "INV2", 200, 200);
            var inventoryNodeTwo = this.GetNode(200);
            var productTwo = this.GetProduct("P2");

            inventoryProductTwo.Node = inventoryNodeTwo;
            inventoryProductTwo.Product = productTwo;
            inventoryProductOne.InventoryDate = DateTime.UtcNow;

            this.movements = new List<Movement> { movementOne, movementTwo };
            this.inventories = new List<InventoryProduct> { inventoryProductOne, inventoryProductTwo };
        }

        /// <summary>
        /// Gets the movement.
        /// </summary>
        /// <param name="movementTransactionId">The movement transaction identifier.</param>
        /// <param name="ownershipTicketId">The ownership ticket identifier.</param>
        /// <param name="days">The days.</param>
        /// <param name="movementId">The movement identifier.</param>
        /// <returns>The movement.</returns>
        private Movement GetMovement(int movementTransactionId, int ownershipTicketId, int days, string movementId)
        {
            var movement = new Movement();
            movement.MovementTransactionId = movementTransactionId;
            movement.MessageTypeId = (int)MessageType.Movement;
            movement.SystemTypeId = 1;
            movement.EventType = "INSERT";
            movement.MovementId = movementId;
            movement.MovementTypeId = 1;
            movement.TicketId = 123;
            movement.OwnershipTicketId = ownershipTicketId;
            movement.SegmentId = 12;
            movement.OperationalDate = DateTime.UtcNow.AddDays(days + 1);
            movement.GrossStandardVolume = 1200;
            movement.NetStandardVolume = 100;
            movement.UncertaintyPercentage = 25;
            movement.MeasurementUnit = 30;
            movement.ScenarioId = ScenarioType.OPERATIONAL;
            movement.Observations = "Observations";
            movement.Classification = "Classification";
            movement.IsDeleted = false;

            return movement;
        }

        /// <summary>
        /// Gets the ownership.
        /// </summary>
        /// <param name="ownershipId">The ownership identifier.</param>
        /// <param name="ownerId">The owner identifier.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="ownershipTicketId">The ownership ticket identifier.</param>
        /// <param name="movementTransactionId">The movement transaction identifier.</param>
        /// <param name="inventoryProductId">The inventory product identifier.</param>
        /// <param name="days">The days.</param>
        /// <returns>
        /// The Ownership.
        /// </returns>
        private Ownership GetOwnership(int ownershipId, int ownerId, MessageType messageType, int ownershipTicketId, int? movementTransactionId, int? inventoryProductId, int days)
        {
            var ownership = new Ownership();
            ownership.OwnershipId = ownershipId;
            ownership.MessageTypeId = messageType;
            ownership.TicketId = ownershipTicketId;
            ownership.MovementTransactionId = movementTransactionId;
            ownership.InventoryProductId = inventoryProductId;
            ownership.OwnerId = ownerId;
            ownership.OwnershipPercentage = 25;
            ownership.OwnershipVolume = 123;
            ownership.AppliedRule = "Rule One";
            ownership.RuleVersion = "Version 1";
            ownership.ExecutionDate = DateTime.UtcNow.AddDays(days + 1);

            return ownership;
        }

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <returns>The Node.</returns>
        private Node GetNode(int nodeId)
        {
            var node = new Node();
            node.NodeId = nodeId;
            node.Name = $"Node-{nodeId}";
            node.Description = $"Description-{nodeId}";
            node.IsActive = true;
            node.NodeTypeId = 32;
            node.SegmentId = 1;
            node.AutoOrder = true;
            node.OperatorId = 1;
            node.Order = 1;
            node.SendToSap = true;
            node.LogisticCenterId = "10089";
            node.ControlLimit = 200;
            node.AcceptableBalancePercentage = 10;

            return node;
        }

        /// <summary>
        /// Gets the movement destination.
        /// </summary>
        /// <param name="movementDestinationId">The movement destination identifier.</param>
        /// <param name="movementTransactionId">The movement transaction identifier.</param>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <returns>The MovementDestination.</returns>
        private MovementDestination GetMovementDestination(int movementDestinationId, int movementTransactionId, int nodeId, string productId)
        {
            var movementDestination = new MovementDestination();
            movementDestination.MovementDestinationId = movementDestinationId;
            movementDestination.MovementTransactionId = movementTransactionId;
            movementDestination.DestinationNodeId = nodeId;
            movementDestination.DestinationStorageLocationId = 123;
            movementDestination.DestinationProductId = productId;
            movementDestination.DestinationProductTypeId = 1;

            return movementDestination;
        }

        /// <summary>
        /// Gets the movement source.
        /// </summary>
        /// <param name="movementSourceId">The movement source identifier.</param>
        /// <param name="movementTransactionId">The movement transaction identifier.</param>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <returns>The MovementSource.</returns>
        private MovementSource GetMovementSource(int movementSourceId, int movementTransactionId, int nodeId, string productId)
        {
            var movementSource = new MovementSource();
            movementSource.MovementSourceId = movementSourceId;
            movementSource.MovementTransactionId = movementTransactionId;
            movementSource.SourceNodeId = nodeId;
            movementSource.SourceStorageLocationId = 123;
            movementSource.SourceProductId = productId;
            movementSource.SourceProductTypeId = 1;

            return movementSource;
        }

        /// <summary>
        /// Gets the product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>The Product.</returns>
        private Product GetProduct(string productId)
        {
            var product = new Product();
            product.ProductId = productId;
            product.Name = $"Name-{productId}";
            product.IsActive = true;

            return product;
        }

        /// <summary>
        /// Gets the inventory product.
        /// </summary>
        /// <param name="inventoryProductId">The inventory product identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="ownershipTicketId">The ownership ticket identifier.</param>
        /// <returns>The InventoryProduct.</returns>
        private InventoryProduct GetInventoryProduct(int inventoryProductId, string productId, int ownershipTicketId, string inventoryId, int days, int nodeId)
        {
            var inventoryProduct = new InventoryProduct();
            inventoryProduct.InventoryProductId = inventoryProductId;
            inventoryProduct.ProductId = productId;
            inventoryProduct.ProductType = 1;
            inventoryProduct.ProductVolume = 122;
            inventoryProduct.MeasurementUnit = 30;
            inventoryProduct.UncertaintyPercentage = 23;
            inventoryProduct.OwnershipTicketId = ownershipTicketId;
            inventoryProduct.SystemTypeId = 1;
            inventoryProduct.DestinationSystem = "SINOPER";
            inventoryProduct.EventType = "INSERT";
            inventoryProduct.TankName = "TANQUE";
            inventoryProduct.InventoryId = inventoryId;
            inventoryProduct.TicketId = 1;
            inventoryProduct.InventoryDate = DateTime.UtcNow.AddDays(days + 1);
            inventoryProduct.NodeId = nodeId;
            inventoryProduct.Observations = "Barrel";
            inventoryProduct.ScenarioId = ScenarioType.OPERATIONAL;
            inventoryProduct.IsDeleted = false;
            inventoryProduct.FileRegistrationTransactionId = 122;
            inventoryProduct.SegmentId = 12;

            return inventoryProduct;
        }
    }
}
