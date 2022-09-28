// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidationDataGeneratorStrategy.cs" company="Microsoft">
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
    using System.Linq;
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
    /// The Consolidation Data Generator Strategy.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.DataGenerator.Console.Strategies.DataGeneratorStrategy" />
    public class ConsolidationDataGeneratorStrategy : DataGeneratorStrategy
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
        /// Initializes a new instance of the <see cref="ConsolidationDataGeneratorStrategy" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="dataGeneratorFactory">The data generator factory.</param>
        public ConsolidationDataGeneratorStrategy(IUnitOfWork unitOfWork, IDataGeneratorFactory dataGeneratorFactory)
            : base(unitOfWork, dataGeneratorFactory)
        {
            this.unitOfWork = unitOfWork;
            this.dataGeneratorFactory = dataGeneratorFactory;
        }

        /// <summary>
        /// Generates asynchronous.
        /// </summary>
        /// <param name="overrideMenu">if set to <c>true</c> [override menu].</param>
        /// <returns>The Task.</returns>
        public override async Task GenerateAsync(bool overrideMenu = false)
        {
            if (overrideMenu)
            {
                Console.WriteLine($"Generating base Consolidation Data. Please wait...{Environment.NewLine}");
                await this.ConsolidationHappyFlowAsync().ConfigureAwait(false);
            }
            else
            {
                Console.WriteLine($"Consolidation Data Generation{Environment.NewLine}");

                var menu = new ActionMenu()
                    .Add("Consolidation Happy Flow", async () => await this.ConsolidationHappyFlowAsync().ConfigureAwait(false))
                    .Add("Consolidation Official Delta Ticket Exists", async () => await this.OfficialDeltaTicketAlreadyExistsAsync().ConfigureAwait(false))
                    .Add("Consolidation Previous Period Not Approved Nodes Exists", async () => await this.PreviousPeriodNotApprovedNodesExistsAsync().ConfigureAwait(false))
                    .Add("Consolidation Data Already Exists", async () => await this.ConsolidationDataAlreadyExistsAsync().ConfigureAwait(false));

                await menu.DisplayAsync().ConfigureAwait(false);
            }
        }

        private static int GetRandomNumber()
        {
            return new Random().Next(1000000, 9999999);
        }

        private async Task PreviousPeriodNotApprovedNodesExistsAsync()
        {
            Console.WriteLine($"Data generation started..{Environment.NewLine}");
            this.Config.TestId = DateTime.UtcNow.ToTrue().Ticks.ToString(CultureInfo.InvariantCulture).Substring(5);
            var segmentId = await this.GenerateCategoryElementAsync($"DataGenerator_{this.Config.TestId}_PAN", 2, false).ConfigureAwait(false);
            var ticketId = await this.GenerateTicketAsync(
                segmentId, this.Config.StartDate.AddDays(-20), this.Config.StartDate.AddDays(-1), StatusType.PROCESSED, TicketType.OfficialDelta).ConfigureAwait(false);
            var nodeId = await this.GenerateNodeAsync($"DataGenerator_PreviousPeriod_{segmentId}_{this.Config.TestId}", segmentId, 1).ConfigureAwait(false);
            await this.GenerateDeltaNodeAsync(ticketId, nodeId, OwnershipNodeStatusType.DELTAS, null).ConfigureAwait(false);

            Console.WriteLine($"TestId : {this.Config.TestId} {Environment.NewLine}");
            Console.WriteLine($"SegmentId: {segmentId}{Environment.NewLine}");
        }

        private async Task OfficialDeltaTicketAlreadyExistsAsync()
        {
            Console.WriteLine($"Data generation started..{Environment.NewLine}");
            this.Config.TestId = DateTime.UtcNow.ToTrue().Ticks.ToString(CultureInfo.InvariantCulture).Substring(5);
            var segmentId = await this.GenerateCategoryElementAsync($"DataGenerator_{this.Config.TestId}_OD", 2, false).ConfigureAwait(false);
            await this.GenerateTicketAsync(segmentId, this.Config.StartDate, this.Config.EndDate, StatusType.PROCESSING, TicketType.OfficialDelta).ConfigureAwait(false);

            Console.WriteLine($"TestId : {this.Config.TestId} {Environment.NewLine}");
            Console.WriteLine($"SegmentId: {segmentId}{Environment.NewLine}");
        }

        private async Task ConsolidationDataAlreadyExistsAsync()
        {
            await this.ConsolidationHappyFlowAsync().ConfigureAwait(false);

            await this.DoGenerateConsolidationAsync(this.Config.ConsolidationSegments.Last(x => x.IsSon)).ConfigureAwait(false);
            await this.DoGenerateConsolidationAsync(this.Config.ConsolidationSegments.Last(x => !x.IsSon)).ConfigureAwait(false);

            Console.WriteLine($"TestId : {this.Config.TestId} {Environment.NewLine}");
            foreach (var seg in this.Config.ConsolidationSegments)
            {
                Console.WriteLine($"SegmentId : {seg.SegmentId}");
            }
        }

        /// <summary>
        /// Consolidations the happy flow asynchronous.
        /// </summary>
        private async Task ConsolidationHappyFlowAsync()
        {
            Console.WriteLine($"Data generation started..{Environment.NewLine}");

            await this.BaseSetUpAsync().ConfigureAwait(false);
            foreach (var seg in this.Config.ConsolidationSegments)
            {
                await this.DoGenerateAsync(seg).ConfigureAwait(false);

                await this.GenerateOfficialDataAsync(seg).ConfigureAwait(false);
            }

            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);

            Console.WriteLine($"TestId : {this.Config.TestId} {Environment.NewLine}");
            foreach (var seg in this.Config.ConsolidationSegments)
            {
                Console.WriteLine($"SegmentId : {seg.SegmentId}");
            }
        }

        private async Task DoGenerateConsolidationAsync(DataGeneratorSegment seg)
        {
            // Generate consolidated movements
            await this.GenerateConsolidatedMovementAsync(seg.CutoffTicketId, seg.SegmentId, seg.SourceNodeIds[0], seg.DestinationNodeIds[0]).ConfigureAwait(false);
            await this.GenerateConsolidatedMovementAsync(seg.CutoffTicketId, seg.SegmentId, seg.SourceNodeIds[0], seg.DestinationNodeIds[0]).ConfigureAwait(false);
            await this.GenerateConsolidatedMovementAsync(seg.CutoffTicketId, seg.SegmentId, seg.SourceNodeIds[1], seg.DestinationNodeIds[1]).ConfigureAwait(false);
            await this.GenerateConsolidatedMovementAsync(seg.CutoffTicketId, seg.SegmentId, seg.SourceNodeIds[1], seg.DestinationNodeIds[1]).ConfigureAwait(false);

            // Generate consolidated inventory products
            await this.GenerateConsolidatedInventoryProductAsync(seg.CutoffTicketId, seg.SegmentId, seg.SourceNodeIds[0], this.Config.StartDate.AddDays(-1).Date).ConfigureAwait(false);
            await this.GenerateConsolidatedInventoryProductAsync(seg.CutoffTicketId, seg.SegmentId, seg.SourceNodeIds[1], this.Config.EndDate.Date).ConfigureAwait(false);
            await this.GenerateConsolidatedInventoryProductAsync(seg.CutoffTicketId, seg.SegmentId, seg.DestinationNodeIds[0], this.Config.StartDate.AddDays(-1).Date).ConfigureAwait(false);
            await this.GenerateConsolidatedInventoryProductAsync(seg.CutoffTicketId, seg.SegmentId, seg.DestinationNodeIds[1], this.Config.EndDate.Date).ConfigureAwait(false);
        }

        private async Task DoGenerateAsync(DataGeneratorSegment seg)
        {
            // Generate movements
            var date = this.Config.StartDate.Date;
            while (date <= this.Config.EndDate.Date)
            {
                await this.GenerateMovementAsync(
                    16000,
                    20000,
                    seg.CutoffTicketId,
                    seg.SegmentId,
                    seg.SourceNodeIds[0],
                    seg.DestinationNodeIds[0],
                    date,
                    seg.OwnershipTicketId,
                    seg.IsSon,
                    "10000002318")
                    .ConfigureAwait(false);
                await this.GenerateMovementAsync(
                    14000,
                    16000,
                    seg.CutoffTicketId,
                    seg.SegmentId,
                    seg.SourceNodeIds[0],
                    seg.DestinationNodeIds[0],
                    date,
                    seg.OwnershipTicketId,
                    seg.IsSon,
                    "10000002318")
                    .ConfigureAwait(false);
                await this.GenerateMovementAsync(
                    18000,
                    20000,
                    seg.CutoffTicketId,
                    seg.SegmentId,
                    seg.SourceNodeIds[0],
                    seg.DestinationNodeIds[0],
                    date,
                    seg.OwnershipTicketId,
                    seg.IsSon,
                    "10000002372")
                    .ConfigureAwait(false);
                await this.GenerateMovementAsync(
                    13000,
                    16000,
                    seg.CutoffTicketId,
                    seg.SegmentId,
                    seg.SourceNodeIds[0],
                    seg.DestinationNodeIds[1],
                    date,
                    seg.OwnershipTicketId,
                    seg.IsSon,
                    "10000002318")
                    .ConfigureAwait(false);
                await this.GenerateMovementAsync(
                    12000,
                    16000,
                    seg.CutoffTicketId,
                    seg.SegmentId,
                    seg.SourceNodeIds[1],
                    seg.DestinationNodeIds[1],
                    date,
                    seg.OwnershipTicketId,
                    seg.IsSon,
                    "10000002318")
                    .ConfigureAwait(false);
                await this.GenerateMovementAsync(
                    8000,
                    14000,
                    seg.CutoffTicketId,
                    seg.SegmentId,
                    seg.SourceNodeIds[1],
                    seg.DestinationNodeIds[1],
                    date,
                    seg.OwnershipTicketId,
                    seg.IsSon,
                    "10000002318",
                    true)
                    .ConfigureAwait(false);
                date = date.AddDays(1);
            }

            // Generate inventory products
            await this.DoGenerateInventoryProductsAsync(seg, this.Config.StartDate.AddDays(-1).Date).ConfigureAwait(false);
            await this.DoGenerateInventoryProductsAsync(seg, this.Config.EndDate.Date).ConfigureAwait(false);
        }

        private async Task DoGenerateInventoryProductsAsync(DataGeneratorSegment seg, DateTime inventoryDate)
        {
            await this.GenerateInventoryProductAsync(16000, 20000, seg.CutoffTicketId, seg.SegmentId, seg.SourceNodeIds[0], inventoryDate, seg.OwnershipTicketId, seg.IsSon).ConfigureAwait(false);
            await this.GenerateInventoryProductAsync(14000, 16000, seg.CutoffTicketId, seg.SegmentId, seg.SourceNodeIds[1], inventoryDate, seg.OwnershipTicketId, seg.IsSon).ConfigureAwait(false);
            await this.GenerateInventoryProductAsync(12000, 16000, seg.CutoffTicketId, seg.SegmentId, seg.DestinationNodeIds[0], inventoryDate, seg.OwnershipTicketId, seg.IsSon).ConfigureAwait(false);
            await this.GenerateInventoryProductAsync(8000, 14000, seg.CutoffTicketId, seg.SegmentId, seg.DestinationNodeIds[1], inventoryDate, seg.OwnershipTicketId, seg.IsSon).ConfigureAwait(false);
        }

        /// <summary>
        /// Bases the set up asynchronous.
        /// </summary>
        private async Task BaseSetUpAsync()
        {
            var systemName = $"DataGenerator_{this.Config.TestId}_system";
            this.Config.TestId = DateTime.UtcNow.ToTrue().Ticks.ToString(CultureInfo.InvariantCulture).Substring(5);

            this.Config.SystemId = await this.GenerateCategoryElementAsync(systemName, 8, false).ConfigureAwait(false);

            // Creating segments
            this.Config.ConsolidationSegments.Add(await this.GenerateSegmentAsync($"DataGenerator_{this.Config.TestId}_SON1", true).ConfigureAwait(false));
            this.Config.ConsolidationSegments.Add(await this.GenerateSegmentAsync($"DataGenerator_{this.Config.TestId}_SON2", true).ConfigureAwait(false));
            this.Config.ConsolidationSegments.Add(await this.GenerateSegmentAsync($"DataGenerator_{this.Config.TestId}_NSON1", false).ConfigureAwait(false));
            this.Config.ConsolidationSegments.Add(await this.GenerateSegmentAsync($"DataGenerator_{this.Config.TestId}_NSON2", false).ConfigureAwait(false));

            // create Annulation
            this.Config.MovementTypeId = 156;
            this.Config.CancellationTypeId = 154;
            await this.GenerateAnnulationAsync().ConfigureAwait(false);

            this.Config.OperationalDate = this.Config.EndDate.Date;
        }

        private async Task<DataGeneratorSegment> GenerateSegmentAsync(string segmentName, bool isSon)
        {
            var segmentId = await this.GenerateCategoryElementAsync(segmentName, 2, isSon).ConfigureAwait(false);

            var cutoffTicketId = await this.GenerateTicketAsync(segmentId, this.Config.StartDate, this.Config.EndDate, StatusType.PROCESSED, TicketType.Cutoff).ConfigureAwait(false);
            var ownershipTicketId = await this.GenerateTicketAsync(segmentId, this.Config.StartDate, this.Config.EndDate, StatusType.PROCESSED, TicketType.Ownership).ConfigureAwait(false);

            var consolidationSegment = new DataGeneratorSegment
            {
                SegmentId = segmentId,
                IsSon = isSon,
                OwnershipTicketId = ownershipTicketId,
                CutoffTicketId = cutoffTicketId,
            };

            // create Node
            var nodeId1 = await this.GenerateNodeAsync($"DataGenerator_Source_{segmentId}_{this.Config.TestId}_1", segmentId, 1).ConfigureAwait(false);
            consolidationSegment.SourceNodeIds.Add(nodeId1);

            var nodeId2 = await this.GenerateNodeAsync($"DataGenerator_Source_{segmentId}_{this.Config.TestId}_2", segmentId, 1).ConfigureAwait(false);
            consolidationSegment.SourceNodeIds.Add(nodeId2);

            var nodeId3 = await this.GenerateNodeAsync($"DataGenerator_Dest_{segmentId}_{this.Config.TestId}_3", segmentId, 1).ConfigureAwait(false);
            consolidationSegment.DestinationNodeIds.Add(nodeId3);

            var nodeId4 = await this.GenerateNodeAsync($"DataGenerator_Dest_{segmentId}_{this.Config.TestId}_4", segmentId, 1).ConfigureAwait(false);
            consolidationSegment.DestinationNodeIds.Add(nodeId4);

            return consolidationSegment;
        }

        private async Task GenerateOfficialDataAsync(DataGeneratorSegment consolidationSegment)
        {
            // Generate  official movements with versions
            // Positive movement scenario.
            await this.GenerateOfficialMovementAsync(
               EventType.Insert,
               consolidationSegment.SegmentId,
               ((this.Config.EndDate.Date.Subtract(this.Config.StartDate.Date).Days + 1) * 30000) + 11000,
               consolidationSegment.SourceNodeIds[0],
               consolidationSegment.DestinationNodeIds[0],
               null,
               null).ConfigureAwait(false);

            await this.GenerateOfficialMovementAsync(
                EventType.Insert,
                consolidationSegment.SegmentId,
                ((this.Config.EndDate.Date.Subtract(this.Config.StartDate.Date).Days + 1) * 30000) + 11000,
                consolidationSegment.SourceNodeIds[0],
                consolidationSegment.DestinationNodeIds[0],
                null,
                null).ConfigureAwait(false);

            // Negative movement scenario.
            await this.GenerateOfficialMovementAsync(
                EventType.Insert,
                consolidationSegment.SegmentId,
                ((this.Config.EndDate.Date.Subtract(this.Config.StartDate.Date).Days + 1) * 13000) - 12000,
                consolidationSegment.SourceNodeIds[0],
                consolidationSegment.DestinationNodeIds[1],
                null,
                null).ConfigureAwait(false);

            await this.GenerateOfficialMovementAsync(
                EventType.Insert,
                consolidationSegment.SegmentId,
                ((this.Config.EndDate.Date.Subtract(this.Config.StartDate.Date).Days + 1) * 13000) - 11000,
                consolidationSegment.SourceNodeIds[0],
                consolidationSegment.DestinationNodeIds[1],
                null,
                null).ConfigureAwait(false);

            // Positive inventory scenario.
            await this.GenerateOfficialInventoryProductAsync(EventType.Insert, consolidationSegment.SegmentId, 20000, consolidationSegment.SourceNodeIds[0], null, null).ConfigureAwait(false);
            await this.GenerateOfficialInventoryProductAsync(EventType.Insert, consolidationSegment.SegmentId, 18000, consolidationSegment.SourceNodeIds[1], null, null).ConfigureAwait(false);

            // Negative inventory scenario.
            await this.GenerateOfficialInventoryProductAsync(EventType.Insert, consolidationSegment.SegmentId, 7000, consolidationSegment.DestinationNodeIds[0], null, null).ConfigureAwait(false);
            await this.GenerateOfficialInventoryProductAsync(EventType.Insert, consolidationSegment.SegmentId, 6000, consolidationSegment.DestinationNodeIds[1], null, null).ConfigureAwait(false);
        }

        private async Task GenerateOfficialMovementAsync(
           EventType type,
           int segmentId,
           decimal netStandardVolume,
           int? sourceNodeId,
           int? destinationNodeId,
           int? officialDeltaTicketID,
           int? officialDeltaMessageTypeId)
        {
            var movementId = $"DataGenerator_Official_{DateTime.UtcNow.ToTrue().Ticks.ToString(CultureInfo.InvariantCulture).Substring(5)}_{GetRandomNumber()}";
            var movementParametersOne = new Dictionary<string, object>
            {
                { "EventType", type },
                { "MovementTypeId", this.Config.MovementTypeId },
                { "MovementId", movementId },
                { "OperationalDate", this.Config.OperationalDate },
                { "NetStandardVolume", netStandardVolume },
                { "SegmentId", segmentId },
                { "SystemId", this.Config.SystemId },
                { "SourceNodeId", sourceNodeId },
                { "DestinationNodeId", destinationNodeId },
                { "OfficialDeltaTicketId", officialDeltaTicketID },
                { "OfficialDeltaMessageTypeId", officialDeltaMessageTypeId },
                { "ScenarioId", ScenarioType.OFFICER },
                { "StartDate", this.Config.StartDate },
                { "EndDate", this.Config.EndDate },
                { "NotRequiresOwnership", true },
                { "RequiresOwner", true },
            };

            await this.dataGeneratorFactory.MovementDataGenerator.GenerateAsync(movementParametersOne).ConfigureAwait(false);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        private async Task GenerateOfficialInventoryProductAsync(
            EventType type,
            int segmentId,
            decimal productVolume,
            int? nodeId,
            int? officialDeltaTicketID,
            int? officialDeltaMessageTypeId)
        {
            var inventoryProductUniqueId = $"DataGenerator_Official_{DateTime.UtcNow.ToTrue().Ticks.ToString(CultureInfo.InvariantCulture).Substring(5)}_{GetRandomNumber()}";
            var parameters = new Dictionary<string, object>
            {
                { "EventType", type },
                { "InventoryId", inventoryProductUniqueId },
                { "InventoryProductUniqueId", inventoryProductUniqueId },
                { "InventoryDate", this.Config.OperationalDate },
                { "ProductVolume", productVolume },
                { "SegmentId", segmentId },
                { "OfficialDeltaTicketId", officialDeltaTicketID },
                { "OfficialDeltaMessageTypeId", officialDeltaMessageTypeId },
                { "SystemId", this.Config.SystemId },
                { "ScenarioId", ScenarioType.OFFICER },
                { "NodeId", nodeId },
                { "NotRequiresOwnership", true },
                { "RequiresOwner", true },
            };

            await this.dataGeneratorFactory.InventoryProductDataGenerator.GenerateAsync(parameters).ConfigureAwait(false);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Generates the movement asynchronous.
        /// </summary>
        /// <param name="netStandardVolume">The net standard volume.</param>
        /// <param name="grossStandardVolume">The gross standard volume.</param>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="segmentId">The segment identifier.</param>
        /// <param name="sourceNodeId">The source node identifier.</param>
        /// <param name="destinationNodeId">The destination node identifier.</param>
        /// <param name="operationalDate">The operational date.</param>
        /// <param name="ownershipTicketId">The ownership ticket identifier.</param>
        /// <param name="isSon">if set to <c>true</c> [is son].</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="hasDelta">if set to <c>true</c> [has delta].</param>
        private async Task GenerateMovementAsync(
            decimal netStandardVolume,
            decimal grossStandardVolume,
            int ticketId,
            int segmentId,
            int sourceNodeId,
            int destinationNodeId,
            DateTime operationalDate,
            int ownershipTicketId,
            bool isSon,
            string productId,
            bool hasDelta = false)
        {
            var movementId = $"DataGenerator_{this.Config.TestId}_{DateTime.UtcNow.ToTrue().Ticks.ToString(CultureInfo.InvariantCulture).Substring(6)}_{GetRandomNumber()}";
            var movementParametersOne = new Dictionary<string, object>
            {
                { "EventType", EventType.Insert },
                { "MovementTypeId", this.Config.MovementTypeId },
                { "CancellationMovementTypeId", this.Config.CancellationTypeId },
                { "MovementId", movementId },
                { "TicketId", ticketId },
                { "SourceProductId", productId },
                { "DestinationProductId", productId },
                { "OwnershipTicketId", ownershipTicketId },
                { "OperationalDate", operationalDate },
                { "NetStandardVolume", netStandardVolume },
                { "GrossStandardVolume", grossStandardVolume },
                { "SegmentId", segmentId },
                { "SourceNodeId",  sourceNodeId },
                { "DestinationNodeId",  destinationNodeId },
                { "NotRequiresOwnership", !isSon },
                { "RequiresOwner", !isSon },
                { "HasDelta", hasDelta },
                { "Delta", 1000 },
            };

            await this.dataGeneratorFactory.MovementDataGenerator.GenerateAsync(movementParametersOne).ConfigureAwait(false);
        }

        /// <summary>
        /// Generates the inventory product asynchronous.
        /// </summary>
        /// <param name="productVolume">The product volume.</param>
        /// <param name="grossStandardQuantity">The gross standard quantity.</param>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="segmentId">The segment identifier.</param>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="inventoryDate">The inventory date.</param>
        /// <param name="ownershipTicketId">The ownership ticket identifier.</param>
        /// <param name="isSon">if set to <c>true</c> [is son].</param>
        private async Task GenerateInventoryProductAsync(
            decimal productVolume,
            decimal grossStandardQuantity,
            int? ticketId,
            int segmentId,
            int nodeId,
            DateTime inventoryDate,
            int ownershipTicketId,
            bool isSon)
        {
            var inventoryProductUniqueId = $"DataGenerator_{this.Config.TestId}_{DateTime.UtcNow.ToTrue().Ticks.ToString(CultureInfo.InvariantCulture).Substring(6)}_{GetRandomNumber()}";
            var parameters = new Dictionary<string, object>
            {
                { "EventType", EventType.Insert },
                { "InventoryId", inventoryProductUniqueId },
                { "InventoryProductUniqueId", inventoryProductUniqueId },
                { "TicketId", ticketId },
                { "OwnershipTicketId", ownershipTicketId },
                { "InventoryDate", inventoryDate },
                { "ProductVolume", productVolume },
                { "GrossStandardQuantity", grossStandardQuantity },
                { "SegmentId", segmentId },
                { "NodeId",  nodeId },
                { "NotRequiresOwnership", !isSon }, // (if NSOn then true else false)
                { "RequiresOwner", !isSon },  // (if NSOn then true else false)
            };

            await this.dataGeneratorFactory.InventoryProductDataGenerator.GenerateAsync(parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Generates the consolidated movement asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="segmentId">The segment identifier.</param>
        /// <param name="sourceNodeId">The source node identifier.</param>
        /// <param name="destinationNodeId">The destination node identifier.</param>
        private async Task GenerateConsolidatedMovementAsync(
            int ticketId,
            int segmentId,
            int sourceNodeId,
            int destinationNodeId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "TicketId", ticketId },
                { "SourceProductId", "10000002318" },
                { "DestinationProductId", "10000002318" },
                { "StartDate", this.Config.StartDate },
                { "EndDate", this.Config.EndDate },
                { "SegmentId", segmentId },
                { "SourceNodeId",  sourceNodeId },
                { "DestinationNodeId",  destinationNodeId },
            };

            await this.dataGeneratorFactory.ConsolidatedMovementDataGenerator.GenerateAsync(parameters).ConfigureAwait(false);
        }

        private async Task GenerateConsolidatedInventoryProductAsync(
            int ticketId,
            int segmentId,
            int nodeId,
            DateTime inventoryDate)
        {
            var parameters = new Dictionary<string, object>
            {
                { "TicketId", ticketId },
                { "InventoryDate", inventoryDate },
                { "SegmentId", segmentId },
                { "NodeId",  nodeId },
            };

            await this.dataGeneratorFactory.ConsolidatedInventoryProductDataGenerator.GenerateAsync(parameters).ConfigureAwait(false);
        }
    }
}
