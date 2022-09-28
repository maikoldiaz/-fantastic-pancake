// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaDataGeneratorStrategy.cs" company="Microsoft">
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
    /// The DeltaDataGeneratorStrategy.
    /// </summary>
    public class OfficialDeltaDataGeneratorStrategy : DataGeneratorStrategy
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
        /// The unit of work.
        /// </summary>
        private DataGeneratorSegment consolidationSegment;

        /// <summary>
        /// Initializes a new instance of the <see cref="OfficialDeltaDataGeneratorStrategy" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="dataGeneratorFactory">The data generator factory.</param>
        public OfficialDeltaDataGeneratorStrategy(IUnitOfWork unitOfWork, IDataGeneratorFactory dataGeneratorFactory)
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
            Console.WriteLine($"Official Delta Data Generation{Environment.NewLine}");
            this.consolidationSegment = this.Config.ConsolidationSegments.Last(x => x.IsSon);
            await this.BaseSetUpAsync().ConfigureAwait(false);

            var menu = new ActionMenu()
                .Add("Official Scenario Happy Flow", async () => await this.HappyPathOfficialScenarioSetupAsync().ConfigureAwait(false))
                .Add("Official Scenario Unhappy Flow", async () => await this.UnHappyPathOfficialScenarioSetupAsync().ConfigureAwait(false))
                .Add("Official Delta Flow", async () => await this.OfficialDeltaSetupAsync().ConfigureAwait(false));

            await menu.DisplayAsync().ConfigureAwait(false);
        }

        private static int GetRandomNumber()
        {
            return new Random().Next(1000000, 9999999);
        }

        private async Task UnHappyPathOfficialScenarioSetupAsync()
        {
            Console.WriteLine($"Data generation started..{Environment.NewLine}");

            await this.BaseSetUpAsync().ConfigureAwait(false);

            // generate deltaNodes
            await this.DoCreateDeltaNodeAsync().ConfigureAwait(false);

            // generate movements to be excluded and/or deleted.
            await this.DoGenerateMovementsForOfficialDeltaAsync().ConfigureAwait(false);

            Console.WriteLine($"TestId : {this.Config.TestId} {Environment.NewLine}");
        }

        private async Task OfficialDeltaSetupAsync()
        {
            // run one among the two below to get official delta inventory and movement data setup.
            await this.HappyPathOfficialDeltaSetupAsync().ConfigureAwait(false);
            await this.UnHappyPathOfficialDeltaSetupAsync().ConfigureAwait(false);
        }

        private async Task HappyPathOfficialDeltaSetupAsync()
        {
            Console.WriteLine($"Data generation started..{Environment.NewLine}");

            // deltaNode with approved state from previous period
            await this.GenerateDeltaNodeAsync(
                this.Config.OfficialDeltaTicketId2,
                this.consolidationSegment.DestinationNodeIds[0],
                OwnershipNodeStatusType.APPROVED,
                DateTime.Today.AddDays(15)).ConfigureAwait(false);

            // deltaNode with deltas state from current period
            await this.GenerateDeltaNodeAsync(
                this.Config.OfficialDeltaTicketId,
                this.consolidationSegment.DestinationNodeIds[0],
                OwnershipNodeStatusType.DELTAS,
                null).ConfigureAwait(false);

            // delta nodes with rejected state and has been previously approved from current period.
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId, this.consolidationSegment.DestinationNodeIds[1], OwnershipNodeStatusType.REJECTED, DateTime.Today.AddDays(10))
                .ConfigureAwait(false);

            // delta nodes with reopened state from current period.
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId, this.consolidationSegment.DestinationNodeIds[8], OwnershipNodeStatusType.REOPENED, DateTime.Today.AddDays(10))
                .ConfigureAwait(false);

            // delta nodes with approved state from previous period.
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId2, this.consolidationSegment.DestinationNodeIds[1], OwnershipNodeStatusType.APPROVED, DateTime.Today.AddDays(-10))
                .ConfigureAwait(false);

            // delta nodes with approved state from previous period.
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId2, this.consolidationSegment.DestinationNodeIds[8], OwnershipNodeStatusType.APPROVED, DateTime.Today.AddDays(-10))
                .ConfigureAwait(false);

            // delta nodes with deltas state and has been previously approved from current period.
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId, this.consolidationSegment.DestinationNodeIds[9], OwnershipNodeStatusType.DELTAS, DateTime.Today.AddDays(15))
                .ConfigureAwait(false);

            // delta nodes with reopened state from current period.
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId, this.consolidationSegment.SourceNodeIds[0], OwnershipNodeStatusType.REOPENED, DateTime.Today.AddDays(15))
                .ConfigureAwait(false);

            // delta nodes with rejected state and has been previously approved from current period.
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId, this.consolidationSegment.SourceNodeIds[1], OwnershipNodeStatusType.REJECTED, DateTime.Today.AddDays(15))
                .ConfigureAwait(false);

            // delta node is in reopened state from current period.
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId, this.consolidationSegment.SourceNodeIds[2], OwnershipNodeStatusType.REOPENED, DateTime.Today.AddDays(15))
                .ConfigureAwait(false);

            // Movement with sourcesystemId is ManualInvOficial, operational date is initial date of current period minus 1 and
            // created date is less than the approval date of previous period.
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.DestinationNodeIds[0],
                this.Config.OfficialDeltaTicketId,
                null,
                189,
                null,
                true,
                this.Config.StartDate.AddDays(-1)).ConfigureAwait(false);

            // Movement with OfficialDeltaMessageTypeId is OfficialInventoryDelta, operational date is initial date of current period minus 1 and
            // created date is less than the approval date of previous period.
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.DestinationNodeIds[0],
                this.Config.OfficialDeltaTicketId,
                1,
                null,
                null,
                true,
                this.Config.StartDate.AddDays(-1)).ConfigureAwait(false);

            // Movement with sourcesystemId is ManualInvOficial, operational date is equal to start date of the period minus 1 and
            // created date is greater than approval date of previous period and less than  the last approval date of the node in the ticket period and
            // node of the segment ticket is in the Deltas or Rejected state and has been previously approved
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.DestinationNodeIds[1],
                this.Config.OfficialDeltaTicketId,
                null,
                189,
                null,
                true,
                this.Config.StartDate.AddDays(-1)).ConfigureAwait(false);

            // Movement with sourcesystemId is ManualInvOficial, operational date is equal to start date of the period minus 1 and
            // created date is greater than approval date of previous period and less than  the last approval date of the node in the ticket period and
            // node of the segment ticket is in reopened state.
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.DestinationNodeIds[8],
                this.Config.OfficialDeltaTicketId,
                null,
                189,
                null,
                true,
                this.Config.StartDate.AddDays(-1)).ConfigureAwait(false);

            // Movement with OfficialDeltaMessageTypeId equal to OfficialInventoryDelta, operational date is equal to start date of the period minus 1 and
            // created date is greater than approval date of previous period and less than  the last approval date of the node in the ticket period
            // node of the segment ticket is in the Deltas or Rejected state and has been previously approved
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.DestinationNodeIds[1],
                this.Config.OfficialDeltaTicketId,
                1,
                null,
                null,
                true,
                this.Config.StartDate.AddDays(-1)).ConfigureAwait(false);

            // Movement with OfficialDeltaMessageTypeId equal to OfficialInventoryDelta, operational date is equal to start date of the period minus 1 and
            // created date is greater than approval date of previous period and less than  the last approval date of the node in the ticket period
            // and // node of the segment ticket is in reopened state.
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.DestinationNodeIds[8],
                this.Config.OfficialDeltaTicketId,
                1,
                null,
                null,
                true,
                this.Config.StartDate.AddDays(-1)).ConfigureAwait(false);

            // Movement with sourcesystemId is ManualInvOficial, operational date is equal to end date of the period and
            // created date is  less than the last approval date of the node in the ticket period and
            // node of the segment ticket is in the Deltas state and has been previously approved
            await this.GenerateMovementAsync(
                -1000,
                this.consolidationSegment.DestinationNodeIds[9],
                null,
                this.Config.OfficialDeltaTicketId,
                null,
                189,
                null,
                true,
                this.Config.EndDate).ConfigureAwait(false);

            // Movement with OfficialDeltaMessageTypeId equal to OfficialInventoryDelta, operational date is equal to end date of the period and
            // created date is  less than the last approval date of the node in the ticket period and
            // node of the segment ticket is in the Deltas state and has been previously approved
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.DestinationNodeIds[9],
                this.Config.OfficialDeltaTicketId,
                1,
                null,
                null,
                true,
                this.Config.EndDate).ConfigureAwait(false);

            // Movement with sourcesystemId is ManualInvOficial, operational date is equal to end date of the period and
            // created date is  less than the last approval date of the node in the ticket period and
            // node of the segment ticket is  is in reopened state.
            await this.GenerateMovementAsync(
                -1000,
                this.consolidationSegment.SourceNodeIds[0],
                null,
                this.Config.OfficialDeltaTicketId,
                null,
                189,
                null,
                true,
                this.Config.EndDate).ConfigureAwait(false);

            // Movement with sOfficialDeltaMessageTypeId equal to OfficialInventoryDelta, operational date is equal to end date of the period and
            // created date is  less than the last approval date of the node in the ticket period and
            // node of the segment ticket is  is in reopened state.
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.SourceNodeIds[0],
                this.Config.OfficialDeltaTicketId,
                1,
                null,
                null,
                true,
                this.Config.EndDate).ConfigureAwait(false);

            // Movement with sourcesystemId is ManualMovOficial, start and end dates of the movements are equal to the start and end dates of the period and
            // created date of the movements is less than the last approval date of the node in the ticket period and
            // node of the segment ticket is in the Rejected state.
            // need to add 154
            await this.GenerateMovementAsync(
                -1000,
                null,
                this.consolidationSegment.SourceNodeIds[1],
                this.Config.OfficialDeltaTicketId,
                null,
                190,
                null,
                true,
                this.Config.EndDate,
                154).ConfigureAwait(false);

            // Movement with OfficialDeltaMessageTypeId equal to "OfficialMovementDelta" , start and end dates of the movements are equal to the start and end dates of the period and
            // created date of the movements is less than the last approval date of the node in the ticket period and
            // node of the segment ticket is in the Rejected state.
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.SourceNodeIds[1],
                this.Config.OfficialDeltaTicketId,
                3,
                null,
                null,
                true,
                this.Config.EndDate).ConfigureAwait(false);

            // Movement with sourcesystemId is ManualMovOficial, start and end dates of the movements are equal to the start and end dates of the period and
            // created date of the movements is less than the last approval date of the node in the ticket period and
            // node of the segment ticket is in the Reopened state.
            // need to pass 154
            await this.GenerateMovementAsync(
                -1000,
                null,
                this.consolidationSegment.SourceNodeIds[2],
                this.Config.OfficialDeltaTicketId,
                null,
                190,
                null,
                true,
                this.Config.EndDate,
                154).ConfigureAwait(false);

            // Movement with OfficialDeltaMessageTypeId equal to "OfficialMovementDelta" , start and end dates of the movements are equal to the start and end dates of the period and
            // created date of the movements is less than the last approval date of the node in the ticket period and
            // node of the segment ticket is in the Reopened state.
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.SourceNodeIds[2],
                this.Config.OfficialDeltaTicketId,
                3,
                null,
                null,
                true,
                this.Config.EndDate).ConfigureAwait(false);
        }

        private async Task UnHappyPathOfficialDeltaSetupAsync()
        {
            Console.WriteLine($"Data generation started..{Environment.NewLine}");

            // delta nodes with delta state from current period and has not been previously approved.
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId, this.consolidationSegment.DestinationNodeIds[0], OwnershipNodeStatusType.DELTAS, null)
                .ConfigureAwait(false);

            // delta nodes with approved state from previous period.
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId2, this.consolidationSegment.DestinationNodeIds[0], OwnershipNodeStatusType.APPROVED, DateTime.Today.AddDays(-30))
                .ConfigureAwait(false);

            // delta nodes with rejected state from current period and has not been previously approved.
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId, this.consolidationSegment.DestinationNodeIds[1], OwnershipNodeStatusType.REJECTED, null)
                .ConfigureAwait(false);

            // delta nodes with approved state from previous period.
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId2, this.consolidationSegment.DestinationNodeIds[1], OwnershipNodeStatusType.APPROVED, DateTime.Today.AddDays(-30))
                .ConfigureAwait(false);

            // delta nodes with deltas state from current period and has not been previously approved.
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId, this.consolidationSegment.DestinationNodeIds[8], OwnershipNodeStatusType.DELTAS, null)
                .ConfigureAwait(false);

            // delta node in rejected state from current period and has not been previously approved.
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId, this.consolidationSegment.DestinationNodeIds[9], OwnershipNodeStatusType.REJECTED, null)
                .ConfigureAwait(false);

            // delta node in rejected state from current period and has been previously approved.
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId, this.consolidationSegment.SourceNodeIds[0], OwnershipNodeStatusType.REJECTED, DateTime.Today.AddDays(-10))
                .ConfigureAwait(false);

            // delta node in rejected state from current period and has been previously approved.
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId, this.consolidationSegment.SourceNodeIds[1], OwnershipNodeStatusType.REJECTED, DateTime.Today.AddDays(-10))
                .ConfigureAwait(false);

            // delta node in rejected state from current period and has not been previously approved.
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId, this.consolidationSegment.SourceNodeIds[8], OwnershipNodeStatusType.REJECTED, null)
                .ConfigureAwait(false);

            // delta node in deltas state from current period and has been previously approved.
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId, this.consolidationSegment.SourceNodeIds[10], OwnershipNodeStatusType.DELTAS, DateTime.Today.AddDays(-10))
                .ConfigureAwait(false);

            // delta node in rejected state from current period and has been previously approved.
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId, this.consolidationSegment.DestinationNodeIds[10], OwnershipNodeStatusType.REJECTED, DateTime.Today.AddDays(-10))
                .ConfigureAwait(false);

            // Movement with sourcesystemId is ManualInvOficial, operational date is initial date of current period minus 1 and
            // created date is greater than the approval date of previous period and
            // node of the segment ticket is in the Deltas  state
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.DestinationNodeIds[0],
                this.Config.OfficialDeltaTicketId,
                null,
                189,
                null,
                null,
                this.Config.StartDate.AddDays(-1)).ConfigureAwait(false);

            // Movement with OfficialDeltaMessageTypeId equal to "OfficialInventoryDelta", operational date is initial date of current period minus 1 and
            // created date is greater than the approval date of previous period
            // node of the segment ticket is in the Deltas  state
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.DestinationNodeIds[0],
                this.Config.OfficialDeltaTicketId,
                1,
                null,
                null,
                null,
                this.Config.StartDate.AddDays(-1)).ConfigureAwait(false);

            // Movement with sourcesystemId is ManualInvOficial, operational date is initial date of current period minus 1 and
            // created date is greater than the approval date of previous period and
            // node of the segment ticket is in Rejected state and has not been previously approved
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.DestinationNodeIds[1],
                this.Config.OfficialDeltaTicketId,
                null,
                189,
                null,
                null,
                this.Config.StartDate.AddDays(-1)).ConfigureAwait(false);

            // Movement with OfficialDeltaMessageTypeId equal to "OfficialInventoryDelta", operational date is initial date of current period minus 1 and
            // created date is greater than the approval date of previous period
            // node of the segment ticket is in tRejected state and has not been previously approved
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.DestinationNodeIds[1],
                this.Config.OfficialDeltaTicketId,
                1,
                null,
                null,
                null,
                this.Config.StartDate.AddDays(-1)).ConfigureAwait(false);

            // Movement with sourcesystemId is ManualInvOficial and
            // operational date is equal to end date of the period and
            // node of the segment ticket is in the Deltas state and has not been previously approved.
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.DestinationNodeIds[8],
                this.Config.OfficialDeltaTicketId,
                null,
                189,
                null,
                null,
                this.Config.EndDate).ConfigureAwait(false);

            // Movement with OfficialDeltaMessageTypeId equal to "OfficialInventoryDelta" and
            // operational date is equal to end date of the period and
            // node of the segment ticket is in the Deltas state and has not been previously approved.
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.DestinationNodeIds[8],
                this.Config.OfficialDeltaTicketId,
                1,
                null,
                null,
                null,
                this.Config.EndDate).ConfigureAwait(false);

            // Movement with sourcesystemId is ManualInvOficial and
            // operational date is equal to end date of the period and
            // node of the segment ticket is in the Rejected state and has not been previously approved.
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.DestinationNodeIds[9],
                this.Config.OfficialDeltaTicketId,
                null,
                189,
                null,
                null,
                this.Config.EndDate).ConfigureAwait(false);

            // Movement with OfficialDeltaMessageTypeId equal to "OfficialInventoryDelta" and
            // operational date is equal to end date of the period and
            // node of the segment ticket is in the Rejected state and has not been previously approved.
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.DestinationNodeIds[9],
                this.Config.OfficialDeltaTicketId,
                1,
                null,
                null,
                null,
                this.Config.EndDate).ConfigureAwait(false);

            // Movement with sourcesystemId is ManualInvOficial, operational date is equal to start date of the period minus one and
            // created date is greater than the last approval date of the node in the ticket period and
            // node of the segment ticket is in the Rejected state and has been previously approved.
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.SourceNodeIds[0],
                this.Config.OfficialDeltaTicketId,
                null,
                189,
                null,
                null,
                this.Config.StartDate.AddDays(-1)).ConfigureAwait(false);

            // Movement with OfficialDeltaMessageTypeId equal to "OfficialInventoryDelta", operational date is equal to start date of the period minus one and
            // created date is greater than the last approval date of the node in the ticket period and
            // node of the segment ticket is in the Rejected state and has been previously approved.
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.SourceNodeIds[0],
                this.Config.OfficialDeltaTicketId,
                1,
                null,
                null,
                null,
                this.Config.StartDate.AddDays(-1)).ConfigureAwait(false);

            // Movement with sourcesystemId is ManualInvOficial, operational date is equal to the end date of the period and
            // created date is greater than the last approval date of the node in the ticket period and
            // node of the segment ticket is in the Rejected state and has been previously approved.
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.SourceNodeIds[1],
                this.Config.OfficialDeltaTicketId,
                null,
                189,
                null,
                null,
                this.Config.EndDate).ConfigureAwait(false);

            // Movement with OfficialDeltaMessageTypeId equal to "OfficialInventoryDelta", operational date is equal to the end date of the period and
            // created date is greater than the last approval date of the node in the ticket period and
            // node of the segment ticket is in the Rejected state and has been previously approved.
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.SourceNodeIds[1],
                this.Config.OfficialDeltaTicketId,
                1,
                null,
                null,
                null,
                this.Config.EndDate).ConfigureAwait(false);

            // Movement with sourcesystemId is ManualMovOficial, start and end dates of the movements are equal to the start and end dates of the period and
            // A node of the segment ticket is in the  Rejected state and has not been previously approved.
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.SourceNodeIds[8],
                this.Config.OfficialDeltaTicketId,
                null,
                190,
                null,
                null,
                DateTime.Today).ConfigureAwait(false);

            // Movement with OfficialDeltaMessageTypeId equal to "OfficialMovementDelta" , start and end dates of the movements are equal to the start and end dates of the period and
            // A node of the segment ticket is in the  Rejected state and has not been previously approved.
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.SourceNodeIds[8],
                this.Config.OfficialDeltaTicketId,
                3,
                null,
                null,
                null,
                DateTime.Today).ConfigureAwait(false);

            // Movement with sourcesystemId is ManualMovOficial, the start and end dates of the movements are equal to the start and end dates of the period and
            // created date of the movements is greater than the last approval date of the node in the ticket period.
            // A node of the segment ticket is in the  deltas state and has been previously approved.
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.SourceNodeIds[10],
                this.Config.OfficialDeltaTicketId,
                null,
                190,
                null,
                null,
                DateTime.Today).ConfigureAwait(false);

            // Movement with OfficialDeltaMessageTypeId equal to "OfficialMovementDelta", the start and end dates of the movements are equal to the start and end dates of the period and
            // created date of the movements is greater than the last approval date of the node in the ticket period.
            // A node of the segment ticket is in the  deltas state and has been previously approved.
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.SourceNodeIds[10],
                this.Config.OfficialDeltaTicketId,
                3,
                null,
                null,
                null,
                DateTime.Today).ConfigureAwait(false);

            // Movement with sourcesystemId is ManualMovOficial, the start and end dates of the movements are equal to the start and end dates of the period and
            // created date of the movements is greater than the last approval date of the node in the ticket period.
            // A node of the segment ticket is in Rejected state and has been previously approved.
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.DestinationNodeIds[10],
                this.Config.OfficialDeltaTicketId,
                null,
                190,
                null,
                null,
                DateTime.Today).ConfigureAwait(false);

            // Movement with OfficialDeltaMessageTypeId equal to "OfficialMovementDelta", the start and end dates of the movements are equal to the start and end dates of the period and
            // created date of the movements is greater than the last approval date of the node in the ticket period.
            // A node of the segment ticket is in Rejected state and has been previously approved.
            await this.GenerateMovementAsync(
                1000,
                null,
                this.consolidationSegment.DestinationNodeIds[10],
                this.Config.OfficialDeltaTicketId,
                3,
                null,
                null,
                null,
                DateTime.Today).ConfigureAwait(false);
        }

        private async Task HappyPathOfficialScenarioSetupAsync()
        {
            Console.WriteLine($"Data generation started..{Environment.NewLine}");
            ////await this.BaseSetUpAsync().ConfigureAwait(false);

            // Generate official movements with versions
            // Positive movement scenario.
            await this.GenerateMovementAsync(
               ((this.Config.EndDate.Date.Subtract(this.Config.StartDate.Date).Days + 1) * 30000) + 11000,
               this.consolidationSegment.SourceNodeIds[0],
               this.consolidationSegment.DestinationNodeIds[0],
               null,
               null,
               null,
               null,
               null,
               this.Config.OperationalDate).ConfigureAwait(false);

            await this.GenerateMovementAsync(
                ((this.Config.EndDate.Date.Subtract(this.Config.StartDate.Date).Days + 1) * 30000) + 11000,
                this.consolidationSegment.SourceNodeIds[0],
                this.consolidationSegment.DestinationNodeIds[0],
                null,
                null,
                null,
                null,
                null,
                this.Config.OperationalDate).ConfigureAwait(false);

            await this.GenerateMovementAsync(
                ((this.Config.EndDate.Date.Subtract(this.Config.StartDate.Date).Days + 1) * 20000) + 13000,
                this.consolidationSegment.SourceNodeIds[1],
                this.consolidationSegment.DestinationNodeIds[1],
                null,
                null,
                null,
                null,
                null,
                this.Config.OperationalDate).ConfigureAwait(false);

            await this.GenerateMovementAsync(
                15000,
                this.consolidationSegment.SourceNodeIds[8],
                this.consolidationSegment.DestinationNodeIds[8],
                null,
                null,
                null,
                null,
                null,
                this.Config.OperationalDate).ConfigureAwait(false);

            await this.GenerateMovementAsync(
                15000,
                this.consolidationSegment.SourceNodeIds[9],
                this.consolidationSegment.DestinationNodeIds[9],
                null,
                null,
                null,
                null,
                null,
                this.Config.OperationalDate).ConfigureAwait(false);

            await this.GenerateMovementAsync(
                15000,
                this.consolidationSegment.SourceNodeIds[10],
                this.consolidationSegment.DestinationNodeIds[10],
                null,
                null,
                null,
                null,
                null,
                this.Config.OperationalDate).ConfigureAwait(false);

            // Negative movement scenario.
            await this.GenerateMovementAsync(
                ((this.Config.EndDate.Date.Subtract(this.Config.StartDate.Date).Days + 1) * 13000) - 12000,
                this.consolidationSegment.SourceNodeIds[0],
                this.consolidationSegment.DestinationNodeIds[1],
                null,
                null,
                null,
                null,
                null,
                this.Config.OperationalDate).ConfigureAwait(false);
            await this.GenerateMovementAsync(
                ((this.Config.EndDate.Date.Subtract(this.Config.StartDate.Date).Days + 1) * 13000) - 11000,
                this.consolidationSegment.SourceNodeIds[0],
                this.consolidationSegment.DestinationNodeIds[1],
                null,
                null,
                null,
                null,
                null,
                this.Config.OperationalDate).ConfigureAwait(false);

            // Positive inventory scenario.
            await this.GenerateInventoryProductAsync(20000, this.consolidationSegment.SourceNodeIds[0], null, null).ConfigureAwait(false);
            await this.GenerateInventoryProductAsync(18000, this.consolidationSegment.SourceNodeIds[1], null, null).ConfigureAwait(false);

            // Negative inventory scenario.
            await this.GenerateInventoryProductAsync(7000, this.consolidationSegment.DestinationNodeIds[0], null, null).ConfigureAwait(false);
            await this.GenerateInventoryProductAsync(6000, this.consolidationSegment.DestinationNodeIds[1], null, null).ConfigureAwait(false);

            Console.WriteLine($"TestId : {this.Config.TestId} {Environment.NewLine}");
        }

        private async Task DoGenerateMovementsForOfficialDeltaAsync()
        {
            // case movement having oficial delta ticketId.
            await this.GenerateMovementAsync(
                       1400,
                       this.consolidationSegment.SourceNodeIds[2],
                       this.consolidationSegment.DestinationNodeIds[2],
                       this.Config.OfficialDeltaTicketId,
                       3,
                       null,
                       null,
                       null,
                       this.Config.OperationalDate).ConfigureAwait(false);

            // case delta node attached to a movement.
            await this.GenerateMovementAsync(
                      1400,
                      this.consolidationSegment.SourceNodeIds[7],
                      this.consolidationSegment.DestinationNodeIds[7],
                      this.Config.OfficialDeltaTicketId,
                      3,
                      null,
                      null,
                      null,
                      this.Config.OperationalDate).ConfigureAwait(false);

            // case movement of "ManualMovOficial" source system for nodes without approvals
            await this.GenerateMovementAsync(
                      1000,
                      this.consolidationSegment.SourceNodeIds[0],
                      this.consolidationSegment.DestinationNodeIds[0],
                      null,
                      null,
                      190,
                      null,
                      null,
                      this.Config.OperationalDate).ConfigureAwait(false);

            // case movement of "ManualInvOficial" source system for nodes without approvals

            // operational date is equal to start date minus one
            await this.GenerateMovementAsync(
                       2000,
                       this.consolidationSegment.SourceNodeIds[0],
                       this.consolidationSegment.DestinationNodeIds[0],
                       null,
                       null,
                       189,
                       null,
                       null,
                       this.Config.StartDate.AddDays(-1)).ConfigureAwait(false);

            // operational date is equal to end date
            await this.GenerateMovementAsync(
                       3000,
                       this.consolidationSegment.SourceNodeIds[0],
                       this.consolidationSegment.DestinationNodeIds[0],
                       null,
                       null,
                       189,
                       null,
                       null,
                       this.Config.EndDate).ConfigureAwait(false);

            // case movement of "ManualMovOficial" source system for nodes with approvals
            await this.GenerateMovementAsync(
                      6000,
                      this.consolidationSegment.SourceNodeIds[8],
                      this.consolidationSegment.DestinationNodeIds[1],
                      null,
                      null,
                      190,
                      null,
                      null,
                      this.Config.OperationalDate).ConfigureAwait(false);

            // case movement of "ManualInvOficial" source system for nodes with approvals

            // operational date is equal to start date minus one
            await this.GenerateMovementAsync(
                       4000,
                       this.consolidationSegment.SourceNodeIds[8],
                       this.consolidationSegment.DestinationNodeIds[1],
                       null,
                       null,
                       189,
                       null,
                       null,
                       this.Config.StartDate.AddDays(-1)).ConfigureAwait(false);

            // operational date is equal to end date
            await this.GenerateMovementAsync(
                       5000,
                       this.consolidationSegment.SourceNodeIds[8],
                       this.consolidationSegment.DestinationNodeIds[1],
                       null,
                       null,
                       189,
                       null,
                       null,
                       this.Config.EndDate).ConfigureAwait(false);

            // source node destination node belong to the same segment but lowest order node(destination Node) belong to the different system.
            await this.GenerateMovementAsync(
                       1500,
                       this.consolidationSegment.SourceNodeIds[3],
                       this.consolidationSegment.DestinationNodeIds[4],
                       null,
                       null,
                       null,
                       null,
                       null,
                       this.Config.OperationalDate).ConfigureAwait(false);

            // source node belong the different segment. but destination node belong the different system
            await this.GenerateMovementAsync(
            1500,
            this.consolidationSegment.SourceNodeIds[6],
            this.consolidationSegment.DestinationNodeIds[3],
            null,
            null,
            null,
            null,
            null,
            this.Config.OperationalDate).ConfigureAwait(false);

            // destination node belongs to the different segment, source node belong to the different system
            await this.GenerateMovementAsync(
            1500,
            this.consolidationSegment.SourceNodeIds[5],
            this.consolidationSegment.DestinationNodeIds[6],
            null,
            null,
            null,
            null,
            null,
            this.Config.OperationalDate).ConfigureAwait(false);

            // case movements originated by inventory deltas (OfficialDeltaMessageTypeId equal to "OfficialInventoryDelta"),
            // where the source or destination node is equal to one of the nodes without approvals, the operational date is equal to start date of the period minus one day.
            await this.GenerateMovementAsync(
            2500,
            null,
            this.consolidationSegment.SourceNodeIds[0],
            this.Config.OfficialDeltaTicketId,
            1,
            null,
            null,
            true,
            this.Config.StartDate.AddDays(-1)).ConfigureAwait(false);

            // case movements originated by inventory deltas (OfficialDeltaMessageTypeId equal to "ConsolidatedInventoryDelta"),
            // where the source or destination node is equal to one of the nodes without approvals, the operational date is equal to end date.
            await this.GenerateMovementAsync(
            3500,
            this.consolidationSegment.SourceNodeIds[1],
            null,
            this.Config.OfficialDeltaTicketId,
            2,
            null,
            null,
            true,
            this.Config.EndDate).ConfigureAwait(false);

            // case movements originated by movement deltas (OfficialDeltaMessageTypeId equal to "OfficialMovementDelta"),
            // where the source or destination node is equal to one of the nodes without approvals, and the start and end dates are equal to the start and end dates of the period.
            await this.GenerateMovementAsync(
            4500,
            this.consolidationSegment.SourceNodeIds[0],
            this.consolidationSegment.DestinationNodeIds[0],
            this.Config.OfficialDeltaTicketId,
            3,
            null,
            null,
            true,
            this.Config.OperationalDate).ConfigureAwait(false);

            // case movements originated by movement deltas (OfficialDeltaMessageTypeId equal to "ConsolidatedMovementDelta"),
            // where the source or destination node is equal to one of the nodes without approvals, and the start and end dates are equal to the start and end dates of the period.
            await this.GenerateMovementAsync(
            5500,
            this.consolidationSegment.SourceNodeIds[0],
            this.consolidationSegment.DestinationNodeIds[0],
            this.Config.OfficialDeltaTicketId,
            4,
            null,
            "10000002372",
            true,
            this.Config.OperationalDate).ConfigureAwait(false);

            // case movements originated by inventory deltas (OfficialDeltaMessageTypeId equal to "OfficialInventoryDelta"),
            // which were registered after the last approval date of the node, where the source or destination node is equal to the node with approvals,
            // the operational date is equal to start date of the period minus one day.
            await this.GenerateMovementAsync(
            22500,
            this.consolidationSegment.DestinationNodeIds[0],
            null,
            this.Config.OfficialDeltaTicketId,
            1,
            null,
            null,
            true,
            this.Config.StartDate.AddDays(-1)).ConfigureAwait(false);

            // case movements originated by inventory deltas (OfficialDeltaMessageTypeId equal to "ConsolidatedInventoryDelta"),
            // which were registered after the last approval date of the node, where the source or destination node is equal to the node with approvals,
            // the operational date is equal to end date.
            await this.GenerateMovementAsync(
            23500,
            this.consolidationSegment.DestinationNodeIds[1],
            null,
            this.Config.OfficialDeltaTicketId,
            2,
            null,
            null,
            true,
            this.Config.EndDate).ConfigureAwait(false);

            // case movements originated by movement deltas (OfficialDeltaMessageTypeId equal to "OfficialMovementDelta"),
            // which were registered after the last approval date of the node, where the source or destination node is equal to the node with approvals,
            // and the start and end dates are equal to the start and end dates of the period.
            await this.GenerateMovementAsync(
            7500,
            this.consolidationSegment.SourceNodeIds[8],
            this.consolidationSegment.DestinationNodeIds[1],
            this.Config.OfficialDeltaTicketId,
            3,
            null,
            null,
            true,
            this.Config.OperationalDate).ConfigureAwait(false);

            // case movements originated by movement deltas (OfficialDeltaMessageTypeId equal to "ConsolidatedMovementDelta"),
            // which were registered after the last approval date of the node, where the source or destination node is equal to the node with approvals,
            // and the start and end dates are equal to the start and end dates of the period.
            await this.GenerateMovementAsync(
            6500,
            this.consolidationSegment.SourceNodeIds[8],
            this.consolidationSegment.DestinationNodeIds[1],
            this.Config.OfficialDeltaTicketId,
            4,
            null,
            "10000002372",
            true,
            this.Config.OperationalDate).ConfigureAwait(false);
        }

        /// <summary>
        /// DoCreateDeltaNode.
        /// </summary>
        /// <returns>the task.</returns>
        private async Task DoCreateDeltaNodeAsync()
        {
            // delta nodes with nodes having  different system and segment from ticket.
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId, this.consolidationSegment.DestinationNodeIds[7], OwnershipNodeStatusType.DELTAS, null).ConfigureAwait(false);
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId, this.consolidationSegment.SourceNodeIds[7], OwnershipNodeStatusType.DELTAS, null).ConfigureAwait(false);

            // delta nodes without previous approval
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId, this.consolidationSegment.SourceNodeIds[0], OwnershipNodeStatusType.REJECTED, null).ConfigureAwait(false);
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId, this.consolidationSegment.SourceNodeIds[1], OwnershipNodeStatusType.DELTAS, null).ConfigureAwait(false);

            // delta nodes with previous approval
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId, this.consolidationSegment.DestinationNodeIds[0], OwnershipNodeStatusType.DELTAS, DateTime.Today.AddDays(-2))
                .ConfigureAwait(false);
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId, this.consolidationSegment.DestinationNodeIds[1], OwnershipNodeStatusType.REJECTED, DateTime.Today.AddDays(-2))
                .ConfigureAwait(false);

            // delta nodes with reopened state
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId, this.consolidationSegment.SourceNodeIds[8], OwnershipNodeStatusType.REOPENED, DateTime.Today.AddDays(-2))
                .ConfigureAwait(false);
            await this.GenerateDeltaNodeAsync(this.Config.OfficialDeltaTicketId, this.consolidationSegment.DestinationNodeIds[8], OwnershipNodeStatusType.REOPENED, DateTime.Today.AddDays(-2))
                .ConfigureAwait(false);
        }

        private async Task BaseSetUpAsync()
        {
            this.Config.TestId = $"DataGenerator_Official_{DateTime.UtcNow.ToTrue().Ticks.ToString(CultureInfo.InvariantCulture).Substring(5)}_{GetRandomNumber()}";

            // Creating system
            var systemName = $"DataGenerator_{this.Config.TestId}_system";
            this.Config.SystemId = await this.GenerateCategoryElementAsync(systemName, 8, false).ConfigureAwait(false);
            this.Config.SegmentId = this.consolidationSegment.SegmentId;

            // Another ticket for officialDelta with approved date before current Date
            this.Config.OfficialDeltaTicketId2 = await this.GenerateTicketAsync(
                this.consolidationSegment.SegmentId, this.Config.StartDate.AddMonths(-1), this.Config.EndDate.AddMonths(-1), StatusType.DELTA, TicketType.OfficialDelta).ConfigureAwait(false);

            // Creating ticket.
            this.Config.OfficialDeltaTicketId = await this.GenerateTicketAsync(
                this.consolidationSegment.SegmentId, this.Config.StartDate, this.Config.EndDate, StatusType.DELTA, TicketType.OfficialDelta).ConfigureAwait(false);

            var nodeId1 = await this.GenerateNodeAsync($"DataGenerator_Source_{this.Config.SegmentId}_{this.Config.TestId}_10", this.Config.SegmentId, 3).ConfigureAwait(false);
            this.consolidationSegment.SourceNodeIds.Add(nodeId1);

            var nodeId2 = await this.GenerateNodeAsync($"DataGenerator_Source_{this.Config.SegmentId}_{this.Config.TestId}_20", this.Config.SegmentId, 4).ConfigureAwait(false);
            this.consolidationSegment.DestinationNodeIds.Add(nodeId2);

            var nodeId3 = await this.GenerateNodeAsync($"DataGenerator_Dest_{this.Config.SegmentId}_{this.Config.TestId}_30", this.Config.SegmentId, 1).ConfigureAwait(false);
            this.consolidationSegment.SourceNodeIds.Add(nodeId3);

            var nodeId4 = await this.GenerateNodeAsync($"DataGenerator_Dest_{this.Config.SegmentId}_{this.Config.TestId}_40", this.Config.SegmentId, 2).ConfigureAwait(false);
            this.consolidationSegment.DestinationNodeIds.Add(nodeId4);

            // adding new  segment and new  system for  negative cases.
            this.Config.DifferentSegmentId = await this.GenerateCategoryElementAsync($"DataGenerator_{this.Config.TestId}_OD_1", 2, false).ConfigureAwait(false);
            this.Config.DifferentSystemId = await this.GenerateCategoryElementAsync($"DataGenerator_{this.Config.TestId}_OD_2", 8, false).ConfigureAwait(false);
            var nodeId11 = await this.GenerateNodeAsync($"DataGenerator_Dest_{this.Config.DifferentSegmentId}_{this.Config.TestId}_OD_3", this.Config.SegmentId, 1).ConfigureAwait(false);
            this.consolidationSegment.SourceNodeIds.Add(nodeId11);
            var nodeId12 = await this.GenerateNodeAsync($"DataGenerator_Dest_{this.Config.DifferentSegmentId}_{this.Config.TestId}_OD_4", this.Config.SegmentId, 2).ConfigureAwait(false);
            this.consolidationSegment.SourceNodeIds.Add(nodeId12);
            var nodeId13 = await this.GenerateNodeAsync($"DataGenerator_Dest_{this.Config.DifferentSystemId}_{this.Config.TestId}_OD_5", this.Config.DifferentSegmentId, 3).ConfigureAwait(false);
            this.consolidationSegment.SourceNodeIds.Add(nodeId13);
            var nodeId14 = await this.GenerateNodeAsync($"DataGenerator_Dest_{this.Config.DifferentSystemId}_{this.Config.TestId}_OD_6", this.Config.DifferentSegmentId, 1).ConfigureAwait(false);
            this.consolidationSegment.SourceNodeIds.Add(nodeId14);
            var nodeId15 = await this.GenerateNodeAsync($"DataGenerator_Dest_{this.Config.DifferentSegmentId}_{this.Config.TestId}_OD_7", this.Config.SegmentId, 3).ConfigureAwait(false);
            this.consolidationSegment.DestinationNodeIds.Add(nodeId15);
            var nodeId16 = await this.GenerateNodeAsync($"DataGenerator_Dest_{this.Config.DifferentSegmentId}_{this.Config.TestId}_OD_8", this.Config.DifferentSegmentId, 4).ConfigureAwait(false);
            this.consolidationSegment.DestinationNodeIds.Add(nodeId16);
            var nodeId17 = await this.GenerateNodeAsync($"DataGenerator_Dest_{this.Config.DifferentSegmentId}_{this.Config.TestId}_OD_9", this.Config.DifferentSegmentId, 7).ConfigureAwait(false);
            this.consolidationSegment.DestinationNodeIds.Add(nodeId17);
            var nodeId18 = await this.GenerateNodeAsync($"DataGenerator_Dest_{this.Config.DifferentSegmentId}_{this.Config.TestId}_OD_10", this.Config.DifferentSegmentId, 8).ConfigureAwait(false);
            this.consolidationSegment.DestinationNodeIds.Add(nodeId18);

            var nodeId19 = await this.GenerateNodeAsync($"DataGenerator_Source_{this.Config.SegmentId}_{this.Config.TestId}_5", this.Config.SegmentId, 1).ConfigureAwait(false);
            this.consolidationSegment.SourceNodeIds.Add(nodeId19);

            var nodeId20 = await this.GenerateNodeAsync($"DataGenerator_Source_{this.Config.SegmentId}_{this.Config.TestId}_6", this.Config.SegmentId, 1).ConfigureAwait(false);
            this.consolidationSegment.DestinationNodeIds.Add(nodeId20);

            var nodeId21 = await this.GenerateNodeAsync($"DataGenerator_Source_{this.Config.SegmentId}_{this.Config.TestId}_21", this.Config.SegmentId, 1).ConfigureAwait(false);
            this.consolidationSegment.SourceNodeIds.Add(nodeId21);

            var nodeId22 = await this.GenerateNodeAsync($"DataGenerator_Source_{this.Config.SegmentId}_{this.Config.TestId}_22", this.Config.SegmentId, 1).ConfigureAwait(false);
            this.consolidationSegment.DestinationNodeIds.Add(nodeId22);

            var nodeId23 = await this.GenerateNodeAsync($"DataGenerator_Source_{this.Config.SegmentId}_{this.Config.TestId}_23", this.Config.SegmentId, 1).ConfigureAwait(false);
            this.consolidationSegment.SourceNodeIds.Add(nodeId23);

            var nodeId24 = await this.GenerateNodeAsync($"DataGenerator_Source_{this.Config.SegmentId}_{this.Config.TestId}_24", this.Config.SegmentId, 1).ConfigureAwait(false);
            this.consolidationSegment.DestinationNodeIds.Add(nodeId24);

            // Creating node tags
            await this.GenerateNodeTagAsync(this.consolidationSegment.SourceNodeIds[0], this.Config.SystemId, this.Config.StartDate.AddDays(-3)).ConfigureAwait(false);
            await this.GenerateNodeTagAsync(this.consolidationSegment.SourceNodeIds[1], this.Config.SystemId, this.Config.StartDate.AddDays(-3)).ConfigureAwait(false);
            await this.GenerateNodeTagAsync(this.consolidationSegment.DestinationNodeIds[0], this.Config.SystemId, this.Config.StartDate.AddDays(-3)).ConfigureAwait(false);
            await this.GenerateNodeTagAsync(this.consolidationSegment.DestinationNodeIds[1], this.Config.SystemId, this.Config.StartDate.AddDays(-3)).ConfigureAwait(false);
            await this.GenerateNodeTagAsync(this.consolidationSegment.SourceNodeIds[8], this.Config.SystemId, this.Config.StartDate.AddDays(-3)).ConfigureAwait(false);
            await this.GenerateNodeTagAsync(this.consolidationSegment.DestinationNodeIds[8], this.Config.SystemId, this.Config.StartDate.AddDays(-3)).ConfigureAwait(false);

            await this.GenerateNodeTagAsync(this.consolidationSegment.DestinationNodeIds[6], this.Config.SystemId, this.Config.StartDate).ConfigureAwait(false);
            await this.GenerateNodeTagAsync(this.consolidationSegment.DestinationNodeIds[4], this.Config.SystemId, this.Config.StartDate).ConfigureAwait(false);

            // adding lowest order node to same system.
            await this.GenerateNodeTagAsync(this.consolidationSegment.SourceNodeIds[3], this.Config.DifferentSystemId, this.Config.StartDate).ConfigureAwait(false);
            await this.GenerateNodeTagAsync(this.consolidationSegment.DestinationNodeIds[3], this.Config.DifferentSystemId, this.Config.StartDate).ConfigureAwait(false);
            await this.GenerateNodeTagAsync(this.consolidationSegment.SourceNodeIds[2], this.Config.SystemId, this.Config.StartDate).ConfigureAwait(false);
            await this.GenerateNodeTagAsync(this.consolidationSegment.DestinationNodeIds[2], this.Config.SystemId, this.Config.StartDate).ConfigureAwait(false);

            // adding lower order node to different system.
            //// await this.GenerateNodeTagAsync(this.consolidationSegment.DestinationNodeIds[4], this.Config.DifferentSystemId, this.Config.StartDate).ConfigureAwait(false);
            await this.GenerateNodeTagAsync(this.consolidationSegment.SourceNodeIds[4], this.Config.DifferentSystemId, this.Config.StartDate).ConfigureAwait(false);
            await this.GenerateNodeTagAsync(this.consolidationSegment.SourceNodeIds[5], this.Config.DifferentSystemId, this.Config.StartDate).ConfigureAwait(false);
            await this.GenerateNodeTagAsync(this.consolidationSegment.SourceNodeIds[6], this.Config.SystemId, this.Config.StartDate).ConfigureAwait(false);
            await this.GenerateNodeTagAsync(this.consolidationSegment.SourceNodeIds[7], this.Config.DifferentSystemId, this.Config.StartDate).ConfigureAwait(false);
            //// await this.GenerateNodeTagAsync(this.consolidationSegment.SourceNodeIds[8], this.Config.DifferentSystemId, this.Config.StartDate).ConfigureAwait(false);
            await this.GenerateNodeTagAsync(this.consolidationSegment.DestinationNodeIds[5], this.Config.DifferentSystemId, this.Config.StartDate).ConfigureAwait(false);
            await this.GenerateNodeTagAsync(this.consolidationSegment.DestinationNodeIds[7], this.Config.DifferentSystemId, this.Config.StartDate).ConfigureAwait(false);
            ////await this.GenerateNodeTagAsync(this.consolidationSegment.DestinationNodeIds[8], this.Config.DifferentSystemId, this.Config.StartDate).ConfigureAwait(false);
            await this.GenerateNodeTagAsync(this.consolidationSegment.SourceNodeIds[9], this.Config.SystemId, this.Config.StartDate).ConfigureAwait(false);
            await this.GenerateNodeTagAsync(this.consolidationSegment.DestinationNodeIds[9], this.Config.SystemId, this.Config.StartDate).ConfigureAwait(false);
            await this.GenerateNodeTagAsync(this.consolidationSegment.SourceNodeIds[10], this.Config.SystemId, this.Config.StartDate).ConfigureAwait(false);
            await this.GenerateNodeTagAsync(this.consolidationSegment.DestinationNodeIds[10], this.Config.SystemId, this.Config.StartDate).ConfigureAwait(false);
            this.Config.OperationalDate = this.Config.StartDate.Date;
        }

        private async Task GenerateMovementAsync(
            decimal netStandardVolume,
            int? sourceNodeId,
            int? destinationNodeId,
            int? officialDeltaTicketID,
            int? officialDeltaMessageTypeId,
            int? sourceSystemId,
            string productId,
            bool? isSystemGenerated,
            DateTime operationalDate,
            int movementTypeId = 156)
        {
            var movementId = $"DataGenerator_Official_{DateTime.UtcNow.ToTrue().Ticks.ToString(CultureInfo.InvariantCulture).Substring(5)}_{GetRandomNumber()}";
            var movementParametersOne = new Dictionary<string, object>
            {
                { "EventType", EventType.Insert },
                { "MovementTypeId", movementTypeId },
                { "MovementId", movementId },
                { "OperationalDate", operationalDate },
                { "NetStandardVolume", netStandardVolume },
                { "SegmentId", this.consolidationSegment.SegmentId },
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
                { "NoDestination", destinationNodeId == null },
                { "NoSource", sourceNodeId == null },
            };

            if (sourceSystemId != null)
            {
                movementParametersOne.Add("SourceSystemId", sourceSystemId);
            }

            if (isSystemGenerated != null)
            {
                movementParametersOne.Add("IsSystemGenerated", isSystemGenerated);
            }

            if (productId != null)
            {
                movementParametersOne.Add("SourceProductId", productId);
                movementParametersOne.Add("DestinationProductId", productId);
            }

            await this.dataGeneratorFactory.MovementDataGenerator.GenerateAsync(movementParametersOne).ConfigureAwait(false);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        private async Task GenerateInventoryProductAsync(
            decimal productVolume,
            int? nodeId,
            int? officialDeltaTicketID,
            int? officialDeltaMessageTypeId)
        {
            var inventoryProductUniqueId = $"DataGenerator_Official_{DateTime.UtcNow.ToTrue().Ticks.ToString(CultureInfo.InvariantCulture).Substring(5)}_{GetRandomNumber()}";
            var parameters = new Dictionary<string, object>
            {
                { "EventType", EventType.Insert },
                { "InventoryId", inventoryProductUniqueId },
                { "InventoryProductUniqueId", inventoryProductUniqueId },
                { "InventoryDate", this.Config.OperationalDate },
                { "ProductVolume", productVolume },
                { "SegmentId", this.consolidationSegment.SegmentId },
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
    }
}
