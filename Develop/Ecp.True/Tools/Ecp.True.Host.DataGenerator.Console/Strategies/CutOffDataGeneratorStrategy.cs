// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CutOffDataGeneratorStrategy.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.DataGenerator.Console.Strategies
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Host.DataGenerator.Console.Entities;
    using Ecp.True.Host.DataGenerator.Console.Interfaces;

    /// <summary>
    /// The FirstTimeNodeCutOffDataGeneratorStrategy.
    /// </summary>
    public class CutOffDataGeneratorStrategy : DataGeneratorStrategy
    {
        /// <summary>
        /// The data generator factory.
        /// </summary>
        private readonly IDataGeneratorFactory dataGeneratorFactory;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CutOffDataGeneratorStrategy" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="dataGeneratorFactory">The data generator factory.</param>
        public CutOffDataGeneratorStrategy(IUnitOfWork unitOfWork, IDataGeneratorFactory dataGeneratorFactory)
            : base(unitOfWork, dataGeneratorFactory)
        {
            this.unitOfWork = unitOfWork;
            this.dataGeneratorFactory = dataGeneratorFactory;
        }

        /// <summary>
        /// Generates the data asynchronous.
        /// </summary>
        /// <param name="overrideMenu">if set to <c>true</c> [override menu].</param>
        /// <returns>The Task.</returns>
        public override async Task GenerateAsync(bool overrideMenu = false)
        {
            Console.WriteLine($"CutOff Data Generation{Environment.NewLine}");

            var menu = new ActionMenu()
                .Add("CutOff Data All Scenarios", async () => await this.AllScenarioDataAsync().ConfigureAwait(false));

            await menu.DisplayAsync().ConfigureAwait(false);
        }

        private static int GetRandomNumber()
        {
            return new Random().Next(1000000, 9999999);
        }

        private async Task AllScenarioDataAsync()
        {
            Console.WriteLine($"Data generation started..{Environment.NewLine}");

            await this.BaseSetUpAsync().ConfigureAwait(false);

            var segment = await this.GenerateSegmentAsync(this.Config.SegmentId).ConfigureAwait(false);

            await this.GenerateDataAsync(segment).ConfigureAwait(false);

            Console.WriteLine($"TestId : {this.Config.TestId}");
            Console.WriteLine($"SegmentId : {this.Config.SegmentId} {Environment.NewLine}");
        }

        private async Task GenerateDataAsync(DataGeneratorSegment segment)
        {
            // First time node case
            await this.GenerateFirstTimeNodeDataAsync(segment).ConfigureAwait(false);

            // Without update nodes case
            await this.GenerateWithoutUpdateNodeDataAsync(segment).ConfigureAwait(false);

            // With update nodes case
            await this.GenerateWithUpdateNodeDataAsync(segment).ConfigureAwait(false);
        }

        private async Task GenerateFirstTimeNodeDataAsync(DataGeneratorSegment segment)
        {
            var movementId = $"DataGenerator_CutOff_{this.Config.TestId}_{GetRandomNumber()}";
            var inventoryProductUniqueId = $"DataGenerator_CutOff_{this.Config.TestId}_{GetRandomNumber()}";
            await this.GenerateMovementAsync(movementId, EventType.Insert, 1000, null, this.Config.StartDate.AddDays(2), segment.SourceNodeIds[0], segment.DestinationNodeIds[0]).ConfigureAwait(false);
            await this.GenerateInventoryProductAsync(inventoryProductUniqueId, EventType.Insert, 1000, null, this.Config.StartDate.AddDays(1), segment.SourceNodeIds[0]).ConfigureAwait(false);
            await this.GenerateInventoryProductAsync(inventoryProductUniqueId, EventType.Insert, 1000, null, this.Config.StartDate.AddDays(2), segment.SourceNodeIds[0]).ConfigureAwait(false);
        }

        private async Task GenerateWithoutUpdateNodeDataAsync(DataGeneratorSegment segment)
        {
            var movementId = $"DataGenerator_CutOff_{this.Config.TestId}_{GetRandomNumber()}";
            await this.GenerateMovementAsync(
                movementId, EventType.Insert, 1000, this.Config.CutoffTicketId, this.Config.StartDate, segment.SourceNodeIds[2], segment.DestinationNodeIds[2]).ConfigureAwait(false);

            movementId = $"DataGenerator_CutOff_{this.Config.TestId}_{GetRandomNumber()}";
            await this.GenerateMovementAsync(
                movementId, EventType.Insert, 1000, this.Config.CutoffTicketId, this.Config.StartDate.AddDays(1), segment.SourceNodeIds[2], segment.DestinationNodeIds[2]).ConfigureAwait(false);

            movementId = $"DataGenerator_CutOff_{this.Config.TestId}_{GetRandomNumber()}";
            await this.GenerateMovementAsync(movementId, EventType.Insert, 1000, null, this.Config.StartDate.AddDays(2), segment.SourceNodeIds[2], segment.DestinationNodeIds[2]).ConfigureAwait(false);

            var inventoryProductUniqueId = $"DataGenerator_CutOff_{this.Config.TestId}_{GetRandomNumber()}";
            await this.GenerateInventoryProductAsync(
                inventoryProductUniqueId,
                EventType.Insert,
                1000,
                this.Config.CutoffTicketId,
                this.Config.StartDate.AddDays(-1),
                segment.SourceNodeIds[2]).ConfigureAwait(false);

            inventoryProductUniqueId = $"DataGenerator_CutOff_{this.Config.TestId}_{GetRandomNumber()}";
            await this.GenerateInventoryProductAsync(
                inventoryProductUniqueId,
                EventType.Insert,
                1000,
                this.Config.CutoffTicketId,
                this.Config.StartDate,
                segment.SourceNodeIds[2]).ConfigureAwait(false);

            inventoryProductUniqueId = $"DataGenerator_CutOff_{this.Config.TestId}_{GetRandomNumber()}";
            await this.GenerateInventoryProductAsync(
                inventoryProductUniqueId,
                EventType.Insert,
                1000,
                this.Config.CutoffTicketId,
                this.Config.StartDate.AddDays(1),
                segment.SourceNodeIds[2]).ConfigureAwait(false);

            inventoryProductUniqueId = $"DataGenerator_CutOff_{this.Config.TestId}_{GetRandomNumber()}";
            await this.GenerateInventoryProductAsync(
                inventoryProductUniqueId,
                EventType.Insert,
                1000,
                null,
                this.Config.StartDate.AddDays(2),
                segment.SourceNodeIds[2]).ConfigureAwait(false);
        }

        private async Task GenerateWithUpdateNodeDataAsync(DataGeneratorSegment segment)
        {
            var movementId = $"DataGenerator_CutOff_{this.Config.TestId}_{GetRandomNumber()}";
            await this.GenerateMovementAsync(
                movementId, EventType.Insert, 1000, this.Config.CutoffTicketId, this.Config.StartDate, segment.SourceNodeIds[1], segment.DestinationNodeIds[1]).ConfigureAwait(false);

            movementId = $"DataGenerator_CutOff_{this.Config.TestId}_{GetRandomNumber()}";
            await this.GenerateMovementAsync(
                movementId, EventType.Insert, 1000, this.Config.CutoffTicketId, this.Config.StartDate.AddDays(1), segment.SourceNodeIds[1], segment.DestinationNodeIds[1]).ConfigureAwait(false);

            movementId = $"DataGenerator_CutOff_{this.Config.TestId}_{GetRandomNumber()}";
            await this.GenerateMovementAsync(movementId, EventType.Insert, 1000, null, this.Config.StartDate.AddDays(2), segment.SourceNodeIds[1], segment.DestinationNodeIds[1]).ConfigureAwait(false);

            var inventoryProductUniqueId = $"DataGenerator_CutOff_{this.Config.TestId}_{GetRandomNumber()}";
            await this.GenerateInventoryProductAsync(
                inventoryProductUniqueId,
                EventType.Insert,
                1000,
                this.Config.CutoffTicketId,
                this.Config.StartDate.AddDays(-1),
                segment.SourceNodeIds[1]).ConfigureAwait(false);

            inventoryProductUniqueId = $"DataGenerator_CutOff_{this.Config.TestId}_{GetRandomNumber()}";
            await this.GenerateInventoryProductAsync(
                inventoryProductUniqueId,
                EventType.Insert,
                1000,
                this.Config.CutoffTicketId,
                this.Config.StartDate,
                segment.SourceNodeIds[1]).ConfigureAwait(false);

            // Source Node
            inventoryProductUniqueId = $"DataGenerator_CutOff_{this.Config.TestId}_{GetRandomNumber()}";
            await this.GenerateInventoryProductAsync(
                inventoryProductUniqueId,
                EventType.Insert,
                1000,
                this.Config.CutoffTicketId,
                this.Config.StartDate.AddDays(1),
                segment.SourceNodeIds[1]).ConfigureAwait(false);

            await this.GenerateInventoryProductAsync(
                inventoryProductUniqueId,
                EventType.Update,
                -1000,
                null,
                this.Config.StartDate.AddDays(1),
                segment.SourceNodeIds[1]).ConfigureAwait(false);

            await this.GenerateInventoryProductAsync(
                inventoryProductUniqueId,
                EventType.Update,
                5000,
                null,
                this.Config.StartDate.AddDays(1),
                segment.SourceNodeIds[1]).ConfigureAwait(false);

            // Destination Node
            inventoryProductUniqueId = $"DataGenerator_CutOff_{this.Config.TestId}_{GetRandomNumber()}";
            await this.GenerateInventoryProductAsync(
                inventoryProductUniqueId,
                EventType.Insert,
                2000,
                this.Config.CutoffTicketId,
                this.Config.StartDate.AddDays(1),
                segment.DestinationNodeIds[1]).ConfigureAwait(false);

            await this.GenerateInventoryProductAsync(
                inventoryProductUniqueId,
                EventType.Update,
                -2000,
                null,
                this.Config.StartDate.AddDays(1),
                segment.DestinationNodeIds[1]).ConfigureAwait(false);

            await this.GenerateInventoryProductAsync(
                inventoryProductUniqueId,
                EventType.Update,
                6000,
                null,
                this.Config.StartDate.AddDays(1),
                segment.DestinationNodeIds[1]).ConfigureAwait(false);

            inventoryProductUniqueId = $"DataGenerator_CutOff_{this.Config.TestId}_{GetRandomNumber()}";
            await this.GenerateInventoryProductAsync(
                inventoryProductUniqueId,
                EventType.Insert,
                1000,
                null,
                this.Config.StartDate.AddDays(2),
                segment.SourceNodeIds[1]).ConfigureAwait(false);
        }

        private async Task BaseSetUpAsync()
        {
            this.Config.TestId = DateTime.UtcNow.ToTrue().Ticks.ToString(CultureInfo.InvariantCulture).Substring(5);

            // Creating segment
            var segmentName = $"DataGenerator_{this.Config.TestId}";
            this.Config.SegmentId = await this.GenerateCategoryElementAsync(segmentName, 2, true).ConfigureAwait(false);

            // Creating tickets
            this.Config.CutoffTicketId = await this.GenerateTicketAsync(
                this.Config.SegmentId, this.Config.StartDate, this.Config.StartDate.AddDays(1), StatusType.PROCESSED, TicketType.Cutoff).ConfigureAwait(false);

            this.Config.OperationalDate = this.Config.StartDate.Date;
        }

        private async Task<DataGeneratorSegment> GenerateSegmentAsync(int segmentId)
        {
            var dataGeneratorSegment = new DataGeneratorSegment
            {
                SegmentId = segmentId,
                IsSon = true,
            };

            var sn1 = await this.GenerateNodeAsync($"First_Source_{this.Config.TestId}", "10000002318", "1000:C001", dataGeneratorSegment.SegmentId, 3).ConfigureAwait(false);
            dataGeneratorSegment.SourceNodeIds.Add(sn1);
            var dn1 = await this.GenerateNodeAsync($"First_Dest_{this.Config.TestId}", "10000002318", "1000:M001", dataGeneratorSegment.SegmentId, 4).ConfigureAwait(false);
            dataGeneratorSegment.DestinationNodeIds.Add(dn1);

            var sn2 = await this.GenerateNodeAsync($"NonFirstWithUpdate_Source_{this.Config.TestId}", "10000002318", "1000:C001", dataGeneratorSegment.SegmentId, 5).ConfigureAwait(false);
            dataGeneratorSegment.SourceNodeIds.Add(sn2);
            var dn2 = await this.GenerateNodeAsync($"NonFirstWithUpdate_Dest_{this.Config.TestId}", "10000002318", "1001:M001", dataGeneratorSegment.SegmentId, 6).ConfigureAwait(false);
            dataGeneratorSegment.DestinationNodeIds.Add(dn2);

            var sn3 = await this.GenerateNodeAsync($"NonFirstWithoutUpdate_Source_{this.Config.TestId}", "10000002318", "1000:C001", dataGeneratorSegment.SegmentId, 7).ConfigureAwait(false);
            dataGeneratorSegment.SourceNodeIds.Add(sn3);
            var dn3 = await this.GenerateNodeAsync($"NonFirstWithoutUpdate_Dest_{this.Config.TestId}", "10000002318", "1000:C001", dataGeneratorSegment.SegmentId, 8).ConfigureAwait(false);
            dataGeneratorSegment.DestinationNodeIds.Add(dn3);

            return dataGeneratorSegment;
        }

        private async Task GenerateMovementAsync(
            string movementId,
            EventType type,
            decimal netStandardVolume,
            int? ticketId,
            DateTime operationalDate,
            int sourceNodeId,
            int destinationNodeId)
        {
            var movementParametersOne = new Dictionary<string, object>
            {
                { "EventType", type },
                { "MovementTypeId", this.Config.MovementTypeId },
                { "MovementId", movementId },
                { "TicketId", ticketId },
                { "OwnershipTicketId", null },
                { "OperationalDate", operationalDate },
                { "NetStandardVolume", netStandardVolume },
                { "SegmentId", this.Config.SegmentId },
                { "sourceNodeId",  sourceNodeId },
                { "DestinationNodeId",  destinationNodeId },
                { "NotRequiresOwnership", true },
            };

            await this.dataGeneratorFactory.MovementDataGenerator.GenerateAsync(movementParametersOne).ConfigureAwait(false);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        private async Task GenerateInventoryProductAsync(
            string inventoryProductUniqueId,
            EventType type,
            decimal productVolume,
            int? ticketId,
            DateTime operationalDate,
            int sourceNodeId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "EventType", type },
                { "InventoryId", inventoryProductUniqueId },
                { "InventoryProductUniqueId", inventoryProductUniqueId },
                { "TicketId", ticketId },
                { "OwnershipTicketId", null },
                { "InventoryDate", operationalDate },
                { "ProductVolume", productVolume },
                { "SegmentId", this.Config.SegmentId },
                { "NodeId",  sourceNodeId },
                { "NotRequiresOwnership", true },
            };

            await this.dataGeneratorFactory.InventoryProductDataGenerator.GenerateAsync(parameters).ConfigureAwait(false);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }
    }
}
