// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaDataGeneratorStrategy.cs" company="Microsoft">
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
    using Ecp.True.Host.DataGenerator.Console.Interfaces;

    /// <summary>
    /// The DeltaDataGeneratorStrategy.
    /// </summary>
    public class DeltaDataGeneratorStrategy : DataGeneratorStrategy
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
        /// Initializes a new instance of the <see cref="DeltaDataGeneratorStrategy" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="dataGeneratorFactory">The data generator factory.</param>
        public DeltaDataGeneratorStrategy(IUnitOfWork unitOfWork, IDataGeneratorFactory dataGeneratorFactory)
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
            Console.WriteLine($"Delta Data Generation{Environment.NewLine}");
            var noUpdateId = $"NoUpdate_{DateTime.UtcNow.ToTrue().Ticks}_DataGenerator";

            var menu = new ActionMenu()
                .Add("Delta Data All Scenarios", async () => await this.AllScenarioDataAsync().ConfigureAwait(false))
                .Add("NoUpdate Scenario", async () => await this.NoUpdateScenarioAsync(noUpdateId).ConfigureAwait(false))
                .Add("Update With Delete Scenario", async () => await this.UpdateWithDeleteScenarioAsync().ConfigureAwait(false))
                .Add("Update With Update Scenario", async () => await this.UpdateWithUpdateScenarioAsync().ConfigureAwait(false))
                .Add("Only Update Scenario", async () => await this.OnlyUpdateScenarioAsync().ConfigureAwait(false))
                .Add("Update With Invalid Node Scenario", async () => await this.UpdateWithInvalidNodeScenarioAsync().ConfigureAwait(false));

            await menu.DisplayAsync().ConfigureAwait(false);
        }

        private async Task AllScenarioDataAsync()
        {
            Console.WriteLine($"Data generation started..{Environment.NewLine}");

            await this.BaseSetUpAsync().ConfigureAwait(false);

            var noUpdateMovementId = $"NoUpdate_{DateTime.UtcNow.ToTrue().Ticks}_DataGenerator";

            // 3 Original Movement insert with ticketId(cutoff). Sequence : IUU
            await this.GenerateAsync(EventType.Insert, noUpdateMovementId, 1500, this.Config.CutoffTicketId).ConfigureAwait(false);
            await this.GenerateAsync(EventType.Update, noUpdateMovementId, -1500, this.Config.CutoffTicketId).ConfigureAwait(false);
            await this.GenerateAsync(EventType.Update, noUpdateMovementId, 1600, this.Config.CutoffTicketId).ConfigureAwait(false);

            // 3 Original movements with ticketId(cutoff) and 1 Updated movements without ticketId. Sequence : IUUD
            var updateWithDeleteMovementId = $"UpdateWithDelete_{this.Config.TestId}_DataGenerator";
            await this.GenerateAsync(EventType.Insert, updateWithDeleteMovementId, 1500, this.Config.CutoffTicketId).ConfigureAwait(false);
            await this.GenerateAsync(EventType.Update, updateWithDeleteMovementId, -1500, this.Config.CutoffTicketId).ConfigureAwait(false);
            await this.GenerateAsync(EventType.Update, updateWithDeleteMovementId, 1600, this.Config.CutoffTicketId).ConfigureAwait(false);
            await this.GenerateAsync(EventType.Delete, updateWithDeleteMovementId, -1600, null).ConfigureAwait(false);

            // 3 Original movements with ticketId(cutoff) and 2 Updated movements without ticketId. Sequence : IUUUU
            var updateWithUpdateMovementId = $"UpdateWithUpdate_{this.Config.TestId}_DataGenerator";
            await this.GenerateAsync(EventType.Insert, updateWithUpdateMovementId, 1500, this.Config.CutoffTicketId).ConfigureAwait(false);
            await this.GenerateAsync(EventType.Update, updateWithUpdateMovementId, -1500, this.Config.CutoffTicketId).ConfigureAwait(false);
            await this.GenerateAsync(EventType.Update, updateWithUpdateMovementId, 1600, this.Config.CutoffTicketId).ConfigureAwait(false);
            await this.GenerateAsync(EventType.Update, updateWithUpdateMovementId, -1600, null).ConfigureAwait(false);
            await this.GenerateAsync(EventType.Update, updateWithUpdateMovementId, 1400, null).ConfigureAwait(false);

            // 3 Original movements with ticketId(cutoff) and 2 Updated movements without ticketId. Sequence : IUUUU
            var onlyUpdateMovementId = $"OnlyUpdate_{this.Config.TestId}_DataGenerator";
            await this.GenerateAsync(EventType.Insert, onlyUpdateMovementId, 1600, null).ConfigureAwait(false);

            // 3 Original movements with ticketId(cutoff) and 2 Updated movements without ticketId. Sequence : IUUUU
            var invalidNodeMovementId = $"InvalidNode_{this.Config.TestId}_DataGenerator";
            await this.GenerateAsync(EventType.Insert, invalidNodeMovementId, 1500, this.Config.CutoffTicketId, false).ConfigureAwait(false);
            await this.GenerateAsync(EventType.Update, invalidNodeMovementId, -1500, this.Config.CutoffTicketId, false).ConfigureAwait(false);
            await this.GenerateAsync(EventType.Update, invalidNodeMovementId, 1600, this.Config.CutoffTicketId, false).ConfigureAwait(false);
            await this.GenerateAsync(EventType.Update, invalidNodeMovementId, -1600, null, false).ConfigureAwait(false);
            await this.GenerateAsync(EventType.Update, invalidNodeMovementId, 1400, null, false).ConfigureAwait(false);

            Console.WriteLine($"TestId : {this.Config.TestId}");
            Console.WriteLine($"SegmentId : {this.Config.SegmentId} {Environment.NewLine}");
        }

        private async Task NoUpdateScenarioAsync(string movementId, bool isValid = true)
        {
            // 3 Original Movement insert with ticketId(cutoff). Sequence : IUU
            Console.WriteLine($"Data generation started..{Environment.NewLine}");
            await this.BaseSetUpAsync().ConfigureAwait(false);
            await this.GenerateAsync(EventType.Insert, movementId, 1500, this.Config.CutoffTicketId, isValid).ConfigureAwait(false);
            await this.GenerateAsync(EventType.Update, movementId, -1500, this.Config.CutoffTicketId, isValid).ConfigureAwait(false);
            await this.GenerateAsync(EventType.Update, movementId, 1600, this.Config.CutoffTicketId, isValid).ConfigureAwait(false);

            Console.WriteLine($"TestId : {this.Config.TestId} {Environment.NewLine}");
            Console.WriteLine($"SegmentId : {this.Config.SegmentId} {Environment.NewLine}");
        }

        ////private async Task UpdateWithInsertScenarioAsync()
        ////{
        ////    3 Original movements with ticketId(cutoff) and 2 Updated movements without ticketId. Sequence : IUUDI
        ////    Console.WriteLine($"Data generation started..{Environment.NewLine}");
        ////    await this.BaseSetUpAsync().ConfigureAwait(false);
        ////    var id = $"UpdateWithInsert_{this.Config.TestId}_DataGenerator";
        ////    await this.NoUpdateScenarioAsync(id).ConfigureAwait(false);
        ////    await this.GenerateAsync(EventType.Delete, id, -1600, null).ConfigureAwait(false);
        ////    await this.GenerateAsync(EventType.Insert, id, 1800, null).ConfigureAwait(false);
        ////    Console.WriteLine($"TestId : {this.Config.TestId} {Environment.NewLine}");
        ////}

        private async Task UpdateWithDeleteScenarioAsync()
        {
            // 3 Original movements with ticketId(cutoff) and 1 Updated movements without ticketId. Sequence : IUUD
            Console.WriteLine($"Data generation started..{Environment.NewLine}");
            await this.BaseSetUpAsync().ConfigureAwait(false);
            var id = $"UpdateWithDelete_{this.Config.TestId}_DataGenerator";
            await this.NoUpdateScenarioAsync(id).ConfigureAwait(false);
            await this.GenerateAsync(EventType.Delete, id, -1600, null).ConfigureAwait(false);

            Console.WriteLine($"TestId : {this.Config.TestId} {Environment.NewLine}");
            Console.WriteLine($"SegmentId : {this.Config.SegmentId} {Environment.NewLine}");
        }

        private async Task UpdateWithUpdateScenarioAsync()
        {
            // 3 Original movements with ticketId(cutoff) and 2 Updated movements without ticketId. Sequence : IUUUU
            Console.WriteLine($"Data generation started..{Environment.NewLine}");
            await this.BaseSetUpAsync().ConfigureAwait(false);
            var id = $"UpdateWithUpdate_{this.Config.TestId}_DataGenerator";
            await this.NoUpdateScenarioAsync(id).ConfigureAwait(false);
            await this.GenerateAsync(EventType.Update, id, -1600, null).ConfigureAwait(false);
            await this.GenerateAsync(EventType.Update, id, 1400, null).ConfigureAwait(false);

            Console.WriteLine($"TestId : {this.Config.TestId}");
            Console.WriteLine($"SegmentId : {this.Config.SegmentId} {Environment.NewLine}");
        }

        private async Task OnlyUpdateScenarioAsync()
        {
            // 3 Original movements with ticketId(cutoff) and 2 Updated movements without ticketId. Sequence : IUUUU
            Console.WriteLine($"Data generation started..{Environment.NewLine}");
            await this.BaseSetUpAsync().ConfigureAwait(false);
            var id = $"OnlyUpdate_{this.Config.TestId}_DataGenerator";
            await this.GenerateAsync(EventType.Insert, id, 1600, null).ConfigureAwait(false);

            Console.WriteLine($"TestId : {this.Config.TestId} {Environment.NewLine}");
            Console.WriteLine($"SegmentId : {this.Config.SegmentId} {Environment.NewLine}");
        }

        private async Task UpdateWithInvalidNodeScenarioAsync()
        {
            // 3 Original movements with ticketId(cutoff) and 2 Updated movements without ticketId. Sequence : IUUUU
            Console.WriteLine($"Data generation started..{Environment.NewLine}");
            await this.BaseSetUpAsync().ConfigureAwait(false);
            var id = $"InvalidNode_{this.Config.TestId}_DataGenerator";
            await this.NoUpdateScenarioAsync(id, false).ConfigureAwait(false);
            await this.GenerateAsync(EventType.Update, id, -1600, null, false).ConfigureAwait(false);
            await this.GenerateAsync(EventType.Update, id, 1400, null, false).ConfigureAwait(false);

            Console.WriteLine($"TestId : {this.Config.TestId} {Environment.NewLine}");
            Console.WriteLine($"SegmentId : {this.Config.SegmentId} {Environment.NewLine}");
        }

        private async Task BaseSetUpAsync()
        {
            this.Config.TestId = DateTime.UtcNow.ToTrue().Ticks.ToString(CultureInfo.InvariantCulture).Substring(5);

            // Creating segment
            var segmentName = $"DataGenerator_{this.Config.TestId}";
            this.Config.SegmentId = await this.GenerateCategoryElementAsync(segmentName, 2, true).ConfigureAwait(false);

            // Creating ProductType
            var productTypeName = $"DataGenerator_ProdType{this.Config.TestId}";
            this.Config.ProductTypeId = await this.GenerateCategoryElementAsync(productTypeName, 11, false).ConfigureAwait(false);

            // Creating nodes
            this.Config.SourceNodeId = await this.GenerateNodeAsync($"DataGenerator_Source_{this.Config.SegmentId}_{this.Config.TestId}", this.Config.SegmentId, 1).ConfigureAwait(false);
            this.Config.DestinationNodeId = await this.GenerateNodeAsync($"DataGenerator_Destination_{this.Config.SegmentId}_{this.Config.TestId}", this.Config.SegmentId, 1).ConfigureAwait(false);

            // Creating invalid nodes
            this.Config.InvalidSourceNodeId = await this.GenerateNodeAsync($"DataGenerator_InvalidSource_{this.Config.SegmentId}_{this.Config.TestId}", 10, 1).ConfigureAwait(false);
            this.Config.InvalidDestinationNodeId = await this.GenerateNodeAsync($"DataGenerator_InvalidDestination_{this.Config.SegmentId}_{this.Config.TestId}", 10, 1).ConfigureAwait(false);

            // Creating tickets
            this.Config.CutoffTicketId = await this.GenerateTicketAsync(
                this.Config.SegmentId, this.Config.StartDate, this.Config.EndDate, StatusType.PROCESSED, TicketType.Cutoff).ConfigureAwait(false);
            this.Config.OwnershipTicketId = await this.GenerateTicketAsync(
                this.Config.SegmentId, this.Config.StartDate, this.Config.EndDate, StatusType.PROCESSED, TicketType.Ownership).ConfigureAwait(false);

            if (this.Config.IsCancellationCase)
            {
                this.Config.MovementTypeId = await this.GenerateCategoryElementAsync("DataGenerator_CancellationType", 9, false).ConfigureAwait(false);
                this.Config.CancellationTypeId = 0;
            }
            else
            {
                this.Config.MovementTypeId = 156;
                this.Config.CancellationTypeId = 154;
                await this.GenerateAnnulationAsync().ConfigureAwait(false);
            }

            this.Config.OperationalDate = this.Config.StartDate.Date;

            // Create node connection
            await this.GenerateNodeConnectionAsync().ConfigureAwait(false);
        }

        private async Task GenerateAsync(
            EventType type,
            string movementId,
            decimal netStandardVolume,
            int? ticketId,
            bool isValid = true)
        {
            await this.GenerateMovementAsync(type, movementId, netStandardVolume, ticketId, isValid).ConfigureAwait(false);
            await this.GenerateInventoryProductAsync(type, movementId, netStandardVolume, ticketId, isValid).ConfigureAwait(false);
        }

        private async Task GenerateMovementAsync(
            EventType type,
            string movementId,
            decimal netStandardVolume,
            int? ticketId,
            bool isValid)
        {
            var movementParametersOne = new Dictionary<string, object>
            {
                { "EventType", type },
                { "MovementTypeId", this.Config.MovementTypeId },
                { "MovementId", movementId },
                { "TicketId", ticketId },
                { "OwnershipTicketId", this.Config.OwnershipTicketId },
                { "OperationalDate", this.Config.OperationalDate },
                { "NetStandardVolume", netStandardVolume },
                { "SegmentId", this.Config.SegmentId },
                { "sourceNodeId",  isValid ? this.Config.SourceNodeId : this.Config.InvalidSourceNodeId },
                { "DestinationNodeId",  isValid ? this.Config.DestinationNodeId : this.Config.InvalidDestinationNodeId },
                { "DestinationProductTypeId",  this.Config.ProductTypeId },
                { "SourceProductTypeId",  this.Config.ProductTypeId },
            };

            await this.dataGeneratorFactory.MovementDataGenerator.GenerateAsync(movementParametersOne).ConfigureAwait(false);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        private async Task GenerateInventoryProductAsync(
            EventType type,
            string inventoryProductUniqueId,
            decimal productVolume,
            int? ticketId,
            bool isValid)
        {
            var parameters = new Dictionary<string, object>
            {
                { "EventType", type },
                { "InventoryId", inventoryProductUniqueId },
                { "InventoryProductUniqueId", inventoryProductUniqueId },
                { "TicketId", ticketId },
                { "OwnershipTicketId", this.Config.OwnershipTicketId },
                { "InventoryDate", this.Config.OperationalDate },
                { "ProductVolume", productVolume },
                { "SegmentId", this.Config.SegmentId },
                { "NodeId",  isValid ? this.Config.SourceNodeId : this.Config.InvalidSourceNodeId },
                { "ProductType",  this.Config.ProductTypeId },
            };

            await this.dataGeneratorFactory.InventoryProductDataGenerator.GenerateAsync(parameters).ConfigureAwait(false);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }
    }
}
