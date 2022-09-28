// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialLogisticsGeneratorStrategy.cs" company="Microsoft">
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
    /// Official logistics Strategy.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.DataGenerator.Console.Strategies.DataGeneratorStrategy" />
    public class OfficialLogisticsGeneratorStrategy : DataGeneratorStrategy
    {
        /// <summary>
        /// The data generator factory.
        /// </summary>
        private readonly IDataGeneratorFactory dataGeneratorFactory;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        ////private readonly string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="OfficialLogisticsGeneratorStrategy"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="dataGeneratorFactory">The data generator factory.</param>
        public OfficialLogisticsGeneratorStrategy(IUnitOfWork unitOfWork, IDataGeneratorFactory dataGeneratorFactory)
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
            var seg = await this.BaseSetUpAsync().ConfigureAwait(false);
            await this.SetupOfficialDeltaAsync(seg).ConfigureAwait(false);
            await this.DoGenerateAsync(seg).ConfigureAwait(false);
            Console.WriteLine($"Use this segment to test all valid scenarios: DataGenerator_{this.Config.TestId}");

            // Invalid deltas case
            var invalidNodeStatusSegmentId = await this.GenerateCategoryElementAsync($"DataGenerator_InvalidNodeStatus_{this.Config.TestId}", 2, true).ConfigureAwait(false);
            var invalidNodeStatusSegment = await this.GenerateSegmentAsync(invalidNodeStatusSegmentId, "InvalidNodeStatus").ConfigureAwait(false);
            await this.SetupOfficialDeltaAsync(invalidNodeStatusSegment, true).ConfigureAwait(false);
            Console.WriteLine($"Use this segment to test invalid node status case: DataGenerator_InvalidNodeStatus_{this.Config.TestId}");

            // Invalid Tautology case
            var invalidTautologySegmentId = await this.GenerateCategoryElementAsync($"DataGenerator_InvalidTautology_{this.Config.TestId}", 2, true).ConfigureAwait(false);
            var invalidSTautologySegment = await this.GenerateSegmentAsync(invalidTautologySegmentId, "InvalidTautology").ConfigureAwait(false);
            await this.SetupOfficialDeltaAsync(invalidSTautologySegment).ConfigureAwait(false);
            await this.DoGenerateAsync(invalidSTautologySegment).ConfigureAwait(false);
            Console.WriteLine($"Use this segment to test tautology error case: DataGenerator_InvalidTautology_{this.Config.TestId}");

            // Invalid No Data case
            var invalidNoDataSegmentId = await this.GenerateCategoryElementAsync($"DataGenerator_InvalidNoData_{this.Config.TestId}", 2, true).ConfigureAwait(false);
            var invalidNoDataSegment = await this.GenerateSegmentAsync(invalidNoDataSegmentId, "InvalidNoData").ConfigureAwait(false);
            await this.SetupOfficialDeltaAsync(invalidNoDataSegment).ConfigureAwait(false);
            Console.WriteLine($"Use this segment to test no data error case: DataGenerator_InvalidNoData_{this.Config.TestId}");
        }

        private static int GetRandomNumber()
        {
            return new Random().Next(1000000, 9999999);
        }

        private async Task DoGenerateAsync(DataGeneratorSegment seg)
        {
            var p1 = "30000000004";

            for (var i = 0; i < seg.SourceNodeIds.Count; i++)
            {
                var p2 = i > 1 ? "10000002318" : "30000000004";
                await this.GenerateOfficialMovementAsync(EventType.Insert, 156, seg.SegmentId, 12000, seg.SourceNodeIds[i], seg.DestinationNodeIds[i], p1, p2, 4, false, false).ConfigureAwait(false);
                await this.GenerateOfficialMovementAsync(EventType.Insert, 154, seg.SegmentId, 12000, seg.SourceNodeIds[i], seg.DestinationNodeIds[i], p1, p2, 3, false, false).ConfigureAwait(false);
                await this.GenerateOfficialMovementAsync(EventType.Insert, 155, seg.SegmentId, 12000, seg.SourceNodeIds[i], seg.DestinationNodeIds[i], p1, p2, 4, false, false).ConfigureAwait(false);
                await this.GenerateOfficialMovementAsync(EventType.Insert, 156, seg.SegmentId, 12000, seg.SourceNodeIds[i], seg.DestinationNodeIds[i], p1, p2, 4, true, false).ConfigureAwait(false);
                await this.GenerateOfficialMovementAsync(EventType.Insert, 155, seg.SegmentId, 12000, seg.SourceNodeIds[i], seg.DestinationNodeIds[i], p1, p2, 4, false, true).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Bases the set up asynchronous.
        /// </summary>
        private async Task<DataGeneratorSegment> BaseSetUpAsync()
        {
            this.Config.TestId = DateTime.UtcNow.ToTrue().Ticks.ToString(CultureInfo.InvariantCulture).Substring(5);
            await this.GenerateHomologationAsync().ConfigureAwait(false);

            // Creating segment
            var segmentName = $"DataGenerator_{this.Config.TestId}";
            this.Config.SegmentId = await this.GenerateCategoryElementAsync(segmentName, 2, true).ConfigureAwait(false);

            // Creating segments
            var segment = await this.GenerateSegmentAsync(this.Config.SegmentId).ConfigureAwait(false);

            // create Annulation
            this.Config.MovementTypeId = 156;
            this.Config.CancellationTypeId = 154;
            await this.GenerateAnnulationAsync(156, 154, 1, 2, 1, 3).ConfigureAwait(false);
            await this.GenerateAnnulationAsync(153, 155, 3, 2, 2, 1).ConfigureAwait(false);

            this.Config.OperationalDate = this.Config.EndDate.Date;
            return segment;
        }

        private async Task SetupOfficialDeltaAsync(DataGeneratorSegment seg, bool invalid = false)
        {
            var ticketId = await this.GenerateTicketAsync(seg.SegmentId, this.Config.StartDate, this.Config.EndDate, StatusType.DELTA, TicketType.OfficialDelta).ConfigureAwait(false);
            var status = invalid ? OwnershipNodeStatusType.DELTAS : OwnershipNodeStatusType.APPROVED;

            var approvalDate = this.Config.EndDate.AddDays(1);
            foreach (var nodeId in seg.SourceNodeIds)
            {
                await this.DoGenerateDeltaNodeAsync(ticketId, nodeId, status, approvalDate).ConfigureAwait(false);
            }

            foreach (var nodeId in seg.DestinationNodeIds)
            {
                await this.DoGenerateDeltaNodeAsync(ticketId, nodeId, status, approvalDate).ConfigureAwait(false);
            }

            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        private async Task GenerateHomologationAsync()
        {
            var cte1 = await this.GenerateCategoryElementAsync("Tr. Material a material", 9, true).ConfigureAwait(false);
            var cte2 = await this.GenerateCategoryElementAsync("Tr. trasladar ce a ce", 9, true).ConfigureAwait(false);
            var cte3 = await this.GenerateCategoryElementAsync("Tr. Almacen a Almacen", 9, true).ConfigureAwait(false);
            var cte4 = await this.GenerateCategoryElementAsync("Anul. Tr. Material a material", 9, true).ConfigureAwait(false);
            var cte5 = await this.GenerateCategoryElementAsync("Anul. Tr.trasladar ce a ce", 9, true).ConfigureAwait(false);
            var cte6 = await this.GenerateCategoryElementAsync("Anul. Tr. Almacen a Almacen", 9, true).ConfigureAwait(false);

            var dataMappings = new Dictionary<string, object>
            {
                { $"{cte1}", "Homologated Tr. Material a material" },
                { $"{cte2}", "Homologated Tr.trasladar ce a ce" },
                { $"{cte3}", "Homologated Tr. Almacen a Almacen" },
                { $"{cte4}", "Homologated Anul. Tr. Material a material" },
                { $"{cte5}", "Homologated Anul. Tr.trasladar ce a ce" },
                { $"{cte6}", "Homologated Anul. Tr. Almacen a Almacen" },
                { "153", "Homologated Evacuación Entrada" },
                { "154", "Homologated Evacuación Salida" },
                { "155", "Homologated Anulación Entrada" },
                { "156", "Homologated Anulación Salida" },
            };

            var objects = new List<int> { 18 };

            var parameters = new Dictionary<string, object>
            {
                { "SourceSystemId", 1 },
                { "DestinationSystemId", 6 },
                { "GroupTypeId", 9 },
                { "DataMappings", dataMappings },
                { "Objects", objects },
            };

            await this.dataGeneratorFactory.HomologationDataGenerator.GenerateAsync(parameters).ConfigureAwait(false);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        private async Task<DataGeneratorSegment> GenerateSegmentAsync(int segmentId, string invalidPrefix = null)
        {
            var dataGeneratorSegment = new DataGeneratorSegment
            {
                SegmentId = segmentId,
                IsSon = true,
            };

            var nodePrefix = "DataGenerator";

            if (!string.IsNullOrEmpty(invalidPrefix))
            {
                nodePrefix = $"{nodePrefix}_{invalidPrefix}";
                var sn111 = await this.GenerateNodeAsync($"{nodePrefix}_Source_111_{this.Config.TestId}", "30000000004", "1000:C001", dataGeneratorSegment.SegmentId, 1).ConfigureAwait(false);
                dataGeneratorSegment.SourceNodeIds.Add(sn111);
                var dn111 = await this.GenerateNodeAsync($"{nodePrefix}_Dest_111_{this.Config.TestId}", "30000000004", "1000:C001", dataGeneratorSegment.SegmentId, 2).ConfigureAwait(false);
                dataGeneratorSegment.DestinationNodeIds.Add(dn111);
            }

            var sn110 = await this.GenerateNodeAsync($"{nodePrefix}_Source_110_{this.Config.TestId}", "30000000004", "1000:C001", dataGeneratorSegment.SegmentId, 3).ConfigureAwait(false);
            dataGeneratorSegment.SourceNodeIds.Add(sn110);
            var dn110 = await this.GenerateNodeAsync($"{nodePrefix}_Dest_110_{this.Config.TestId}", "30000000004", "1000:M001", dataGeneratorSegment.SegmentId, 4).ConfigureAwait(false);
            dataGeneratorSegment.DestinationNodeIds.Add(dn110);

            var sn100 = await this.GenerateNodeAsync($"{nodePrefix}_Source_100_{this.Config.TestId}", "30000000004", "1000:C001", dataGeneratorSegment.SegmentId, 5).ConfigureAwait(false);
            dataGeneratorSegment.SourceNodeIds.Add(sn100);
            var dn100 = await this.GenerateNodeAsync($"{nodePrefix}_Dest_100_{this.Config.TestId}", "30000000004", "1001:M001", dataGeneratorSegment.SegmentId, 6).ConfigureAwait(false);
            dataGeneratorSegment.DestinationNodeIds.Add(dn100);

            var sn011 = await this.GenerateNodeAsync($"{nodePrefix}_Source_011_{this.Config.TestId}", "30000000004", "1000:C001", dataGeneratorSegment.SegmentId, 7).ConfigureAwait(false);
            dataGeneratorSegment.SourceNodeIds.Add(sn011);
            var dn011 = await this.GenerateNodeAsync($"{nodePrefix}_Dest_011_{this.Config.TestId}", "10000002318", "1000:C001", dataGeneratorSegment.SegmentId, 8).ConfigureAwait(false);
            dataGeneratorSegment.DestinationNodeIds.Add(dn011);

            var sn010 = await this.GenerateNodeAsync($"{nodePrefix}_Source_010_{this.Config.TestId}", "30000000004", "1000:C001", dataGeneratorSegment.SegmentId, 9).ConfigureAwait(false);
            dataGeneratorSegment.SourceNodeIds.Add(sn010);
            var dn010 = await this.GenerateNodeAsync($"{nodePrefix}_Dest_010_{this.Config.TestId}", "10000002318", "1000:M001", dataGeneratorSegment.SegmentId, 10).ConfigureAwait(false);
            dataGeneratorSegment.DestinationNodeIds.Add(dn010);

            var sn000 = await this.GenerateNodeAsync($"{nodePrefix}_Source_000_{this.Config.TestId}", "30000000004", "1000:C001", dataGeneratorSegment.SegmentId, 11).ConfigureAwait(false);
            dataGeneratorSegment.SourceNodeIds.Add(sn000);
            var dn000 = await this.GenerateNodeAsync($"{nodePrefix}_Dest_000_{this.Config.TestId}", "10000002318", "1001:M001", dataGeneratorSegment.SegmentId, 12).ConfigureAwait(false);
            dataGeneratorSegment.DestinationNodeIds.Add(dn000);

            return dataGeneratorSegment;
        }

        private async Task GenerateOfficialMovementAsync(
           EventType type,
           int movementTypeId,
           int segmentId,
           decimal netStandardVolume,
           int? sourceNodeId,
           int? destinationNodeId,
           string sourceProductId,
           string destinationProductId,
           int officialDeltaMessageTypeId,
           bool noSource,
           bool noDestination)
        {
            var movementId = $"DataGenerator_Official_{DateTime.UtcNow.ToTrue().Ticks.ToString(CultureInfo.InvariantCulture).Substring(5)}_{GetRandomNumber()}";
            var movementParametersOne = new Dictionary<string, object>
            {
                { "EventType", type },
                { "MovementTypeId", movementTypeId },
                { "MovementId", movementId },
                { "OperationalDate", this.Config.OperationalDate },
                { "NetStandardVolume", netStandardVolume },
                { "SegmentId", segmentId },
                { "SourceNodeId", sourceNodeId },
                { "DestinationNodeId", destinationNodeId },
                { "SourceProductId", sourceProductId },
                { "DestinationProductId", destinationProductId },
                { "OfficialDeltaMessageTypeId", officialDeltaMessageTypeId },
                { "ScenarioId", ScenarioType.OFFICER },
                { "StartDate", this.Config.StartDate },
                { "EndDate", this.Config.EndDate },
                { "NotRequiresOwnership", true },
                { "RequiresOwner", true },
                { "NoSource", noSource },
                { "NoDestination", noDestination },
            };

            await this.dataGeneratorFactory.MovementDataGenerator.GenerateAsync(movementParametersOne).ConfigureAwait(false);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        private async Task DoGenerateDeltaNodeAsync(int ticketId, int nodeId, OwnershipNodeStatusType statusType, DateTime lastApprovedDate)
        {
            var parameters = new Dictionary<string, object>
                {
                    { "NodeId", nodeId },
                    { "TicketId", ticketId },
                    { "Status", statusType },
                    { "LastApprovedDate", lastApprovedDate },
                };

            await this.dataGeneratorFactory.DeltaNodeDataGenerator.GenerateAsync(parameters).ConfigureAwait(false);
        }
    }
}
