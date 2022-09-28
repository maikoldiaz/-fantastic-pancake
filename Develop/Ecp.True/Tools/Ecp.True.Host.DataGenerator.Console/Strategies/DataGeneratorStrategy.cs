// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGeneratorStrategy.cs" company="Microsoft">
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
    using CommandLine;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.DataGenerator.Console.Entities;
    using Ecp.True.Host.DataGenerator.Console.Interfaces;
    using EfCore.Models;

    /// <summary>
    /// The data generator strategy.
    /// </summary>
    public abstract class DataGeneratorStrategy : IDataGeneratorStrategy
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The data generator factory.
        /// </summary>
        private readonly IDataGeneratorFactory dataGeneratorFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGeneratorStrategy" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="dataGeneratorFactory">The data generator factory.</param>
        protected DataGeneratorStrategy(IUnitOfWork unitOfWork, IDataGeneratorFactory dataGeneratorFactory)
        {
            this.unitOfWork = unitOfWork;
            this.dataGeneratorFactory = dataGeneratorFactory;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public ConfigBase Config { get; private set; }

        /// <summary>
        /// Generates asynchronous.
        /// </summary>
        /// <param name="overrideMenu">if set to <c>true</c> [override menu].</param>
        /// <returns>
        /// The ConfigBase.
        /// </returns>
        public abstract Task GenerateAsync(bool overrideMenu = false);

        /// <summary>
        /// Initializes.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void Initialize(string[] args)
        {
            this.Config = new ConfigBase();
            Parser.Default.ParseArguments<ConsoleOptions>(args)
                 .WithParsed(o =>
                 {
                     this.Config.SegmentId = o.Segment;
                     this.Config.StartDate = Convert.ToDateTime(o.StartDate, CultureInfo.InvariantCulture);
                     this.Config.EndDate = Convert.ToDateTime(o.EndDate, CultureInfo.InvariantCulture);
                     this.Config.IsCancellationCase = o.IsCancellationCase;
                 }).WithNotParsed(errors => HandleErrors(errors));
        }

        /// <summary>
        /// Initializes.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="configBase">The unit of work.</param>
        public void Initialize(string[] args, ConfigBase configBase)
        {
            ArgumentValidators.ThrowIfNull(configBase, nameof(configBase));
            this.Config = configBase;
            Parser.Default.ParseArguments<ConsoleOptions>(args)
                 .WithParsed(o =>
                 {
                     this.Config.SegmentId = o.Segment;
                     this.Config.StartDate = Convert.ToDateTime(o.StartDate, CultureInfo.InvariantCulture);
                     this.Config.EndDate = Convert.ToDateTime(o.EndDate, CultureInfo.InvariantCulture);
                     this.Config.IsCancellationCase = o.IsCancellationCase;
                 }).WithNotParsed(errors => HandleErrors(errors));
        }

        /// <summary>
        /// Generates the node tag.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="segmentId">The segment identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <returns>The Task.</returns>
        protected virtual async Task GenerateNodeTagAsync(int nodeId, int segmentId, DateTime startDate)
        {
            var repository = this.unitOfWork.CreateRepository<NodeTag>();
            var tagsCount = await repository.GetCountAsync(a => a.NodeId == nodeId && a.ElementId == segmentId).ConfigureAwait(false);
            if (tagsCount > 0)
            {
                return;
            }

            var nodeTagParametersTwo = new Dictionary<string, object>
            {
                { "NodeId", nodeId },
                { "ElementId", segmentId },
                { "StartDate", startDate },
            };

            await this.dataGeneratorFactory.NodeTagDataGenerator.GenerateAsync(nodeTagParametersTwo).ConfigureAwait(false);
        }

        /// <summary>
        /// Generates the node connection.
        /// </summary>
        /// <returns>The Task.</returns>
        protected virtual async Task GenerateNodeConnectionAsync()
        {
            await this.DoGenerateNodeConnectionAsync(this.Config.SourceNodeId, this.Config.DestinationNodeId).ConfigureAwait(false);
            await this.DoGenerateNodeConnectionAsync(this.Config.InvalidSourceNodeId, this.Config.InvalidDestinationNodeId).ConfigureAwait(false);
        }

        /// <summary>
        /// Generates the annulation asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        protected virtual async Task GenerateAnnulationAsync()
        {
            var annulationRepository = this.unitOfWork.CreateRepository<Annulation>();
            var totalAnnulations = await annulationRepository.GetCountAsync(a =>
            a.SourceMovementTypeId == this.Config.MovementTypeId && a.AnnulationMovementTypeId == this.Config.CancellationTypeId).ConfigureAwait(false);
            if (totalAnnulations == 0)
            {
                var annulationParameters = new Dictionary<string, object>
                {
                    { "SourceMovementTypeId", this.Config.MovementTypeId },
                    { "AnnulationMovementTypeId", this.Config.CancellationTypeId },
                };

                await this.dataGeneratorFactory.AnnulationDataGenerator.GenerateAsync(annulationParameters).ConfigureAwait(false);
                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Generates the annulation asynchronous.
        /// </summary>
        /// <param name="sourceMovementTypeId">The source movement type identifier.</param>
        /// <param name="cancellationTypeId">The cancellation type identifier.</param>
        /// <param name="sourceNodeId">The source node identifier.</param>
        /// <param name="destinationNodeId">The destination node identifier.</param>
        /// <param name="sourceProductId">The source product identifier.</param>
        /// <param name="destinationProductId">The destination product identifier.</param>
        /// <returns>The task.</returns>
        protected virtual async Task GenerateAnnulationAsync(
            int sourceMovementTypeId,
            int cancellationTypeId,
            int sourceNodeId,
            int destinationNodeId,
            int sourceProductId,
            int destinationProductId)
        {
            var annulationRepository = this.unitOfWork.CreateRepository<Annulation>();
            var totalAnnulations = await annulationRepository.GetCountAsync(a =>
            a.SourceMovementTypeId == sourceMovementTypeId && a.AnnulationMovementTypeId == cancellationTypeId).ConfigureAwait(false);
            if (totalAnnulations == 0)
            {
                var parameters = new Dictionary<string, object>
                {
                    { "SourceMovementTypeId", sourceMovementTypeId },
                    { "AnnulationMovementTypeId", cancellationTypeId },
                    { "SourceNodeId", sourceNodeId },
                    { "DestinationNodeId", destinationNodeId },
                    { "SourceProductId", sourceProductId },
                    { "DestinationProductId", destinationProductId },
                    { "IsActive", true },
                };

                await this.dataGeneratorFactory.AnnulationDataGenerator.GenerateAsync(parameters).ConfigureAwait(false);
                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Generates the category element asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="isOperational">if set to <c>true</c> [is operational].</param>
        /// <returns>
        /// The task.
        /// </returns>
        protected virtual async Task<int> GenerateCategoryElementAsync(string name, int categoryId, bool isOperational)
        {
            var categoryElementRepository = this.unitOfWork.CreateRepository<CategoryElement>();
            var categoryElement = await categoryElementRepository.SingleOrDefaultAsync(a => a.Name == name).ConfigureAwait(false);
            if (categoryElement == null)
            {
                var parameters = new Dictionary<string, object>
                {
                    { "Name", name },
                    { "CategoryId", categoryId },
                };

                await this.dataGeneratorFactory.CategoryElementDataGenerator.GenerateAsync(parameters).ConfigureAwait(false);
                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);

                categoryElement = await categoryElementRepository.SingleOrDefaultAsync(a => a.Name.EqualsIgnoreCase(name)).ConfigureAwait(false);

                if (isOperational)
                {
                    categoryElement.IsOperationalSegment = true;
                }

                categoryElementRepository.Update(categoryElement);

                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);

                return categoryElement.ElementId;
            }

            return categoryElement.ElementId;
        }

        /// <summary>
        /// Does the generate node connection asynchronous.
        /// </summary>
        /// <param name="sourceNodeId">The source node identifier.</param>
        /// <param name="destinationNodeId">The destination node identifier.</param>
        /// <returns>The Task.</returns>
        protected virtual async Task DoGenerateNodeConnectionAsync(int sourceNodeId, int destinationNodeId)
        {
            var repository = this.unitOfWork.CreateRepository<NodeConnection>();
            var connectionCount = await repository.GetCountAsync(a => a.SourceNodeId == sourceNodeId && a.DestinationNodeId == destinationNodeId).ConfigureAwait(false);
            if (connectionCount > 0)
            {
                return;
            }

            var nodeConnectionParameters = new Dictionary<string, object>
            {
                { "SourceNodeId", sourceNodeId },
                { "DestinationNodeId", destinationNodeId },
            };

            await this.dataGeneratorFactory.NodeConnectionDataGenerator.GenerateAsync(nodeConnectionParameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Does the generate node asynchronous.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="segmentId">The segment identifier.</param>
        /// <param name="order">The order.</param>
        /// <returns>The Task.</returns>
        protected virtual async Task<int> GenerateNodeAsync(
            string nodeName,
            int segmentId,
            int? order)
        {
            var nodeRepository = this.unitOfWork.CreateRepository<Node>();
            var node = await nodeRepository.SingleOrDefaultAsync(a => a.Name == nodeName).ConfigureAwait(false);
            if (node == null)
            {
                var nodeParameters = new Dictionary<string, object>
                {
                    { "NodeName", nodeName },
                    { "NodeStorageLocationName", "DataGenerator_StorageLocation1" },
                    { "ProductId1", "30000000002" },
                    { "ProductId2", "10000002049" },
                    { "Order", order },
                    { "UnitId", null },
                    { "NodeTypeId", 1 },
                    { "SegmentId", segmentId },
                    { "OperatorId", 14 },
                    { "StorageLocationTypeId", 1 },
                };

                await this.dataGeneratorFactory.NodeDataGenerator.GenerateAsync(nodeParameters).ConfigureAwait(false);
                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);

                node = await nodeRepository.SingleOrDefaultAsync(a => a.Name == nodeName).ConfigureAwait(false);
                return node.NodeId;
            }

            return node.NodeId;
        }

        /// <summary>
        /// Does the generate node asynchronous.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="storageLocationId">The storage location identifier.</param>
        /// <param name="segmentId">The segment identifier.</param>
        /// <param name="order">The order.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        protected virtual async Task<int> GenerateNodeAsync(
            string nodeName,
            string productId,
            string storageLocationId,
            int segmentId,
            int? order)
        {
            var nodeRepository = this.unitOfWork.CreateRepository<Node>();
            var node = await nodeRepository.SingleOrDefaultAsync(a => a.Name == nodeName).ConfigureAwait(false);
            if (node == null)
            {
                var nodeParameters = new Dictionary<string, object>
                {
                    { "NodeName", nodeName },
                    { "NodeStorageLocationName", "DataGenerator_StorageLocation1" },
                    { "ProductId1", productId },
                    { "ProductId2", "10000002049" },
                    { "Order", order },
                    { "UnitId", null },
                    { "NodeTypeId", 1 },
                    { "SegmentId", segmentId },
                    { "OperatorId", 14 },
                    { "StorageLocationTypeId", 1 },
                    { "StorageLocationId", storageLocationId },
                    { "SendToSap", true },
                };

                await this.dataGeneratorFactory.NodeDataGenerator.GenerateAsync(nodeParameters).ConfigureAwait(false);
                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);

                node = await nodeRepository.SingleOrDefaultAsync(a => a.Name == nodeName).ConfigureAwait(false);
                return node.NodeId;
            }

            return node.NodeId;
        }

        /// <summary>
        /// the delta NodeId.
        /// </summary>
        /// <param name="ticketId">ticketId.</param>
        /// <param name="nodeId">NodeId.</param>
        /// <param name="statusType">statusType.</param>
        /// <param name="lastApprovedDate">lastApprovedDate.</param>
        /// <returns>deltaNodeId.</returns>
        protected virtual async Task<int> GenerateDeltaNodeAsync(int? ticketId, int nodeId, OwnershipNodeStatusType statusType, DateTime? lastApprovedDate)
        {
            var deltaNodeRepository = this.unitOfWork.CreateRepository<DeltaNode>();
            var deltaNode = await deltaNodeRepository.SingleOrDefaultAsync(x => x.NodeId == nodeId).ConfigureAwait(false);
            if (deltaNode == null)
            {
                var parameters = new Dictionary<string, object>
                {
                    { "NodeId", nodeId },
                    { "TicketId", ticketId },
                    { "Status", statusType },
                };

                if (lastApprovedDate != null)
                {
                    parameters.Add("LastApprovedDate", lastApprovedDate);
                }

                await this.dataGeneratorFactory.DeltaNodeDataGenerator.GenerateAsync(parameters).ConfigureAwait(false);
                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
                deltaNode = await deltaNodeRepository.SingleOrDefaultAsync(x => x.NodeId == nodeId).ConfigureAwait(false);
                return deltaNode.DeltaNodeId;
            }

            return deltaNode.DeltaNodeId;
        }

        /// <summary>
        /// Generates the ticket asynchronous.
        /// </summary>
        /// <param name="categoryElementId">The category element identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="status">The status.</param>
        /// <param name="ticketType">Type of the ticket.</param>
        /// <returns>The ticket identifier.</returns>
        protected virtual async Task<int> GenerateTicketAsync(
            int categoryElementId,
            DateTime startDate,
            DateTime endDate,
            StatusType status,
            TicketType ticketType)
        {
            var ticketRepository = this.unitOfWork.CreateRepository<Ticket>();
            var ticket = await ticketRepository.SingleOrDefaultAsync(a =>
            a.CategoryElementId == categoryElementId &&
            (a.StartDate >= startDate || a.StartDate <= endDate) &&
            a.TicketTypeId == ticketType).ConfigureAwait(false);

            if (ticket == null || ticket.TicketTypeId == TicketType.OfficialDelta)
            {
                var ticketParameters = new Dictionary<string, object>
                {
                    { "CategoryElementId", categoryElementId },
                    { "StartDate", startDate },
                    { "EndDate", endDate },
                    { "Status", status },
                    { "TicketTypeId", ticketType },
                };

                var ticketId = await this.dataGeneratorFactory.TicketDataGenerator.GenerateAsync(ticketParameters).ConfigureAwait(false);

                ticket = await ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);
                ticket.TicketTypeId = ticketType;
                ticket.Status = status;

                ticketRepository.Update(ticket);
                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);

                return ticket.TicketId;
            }

            return ticket.TicketId;
        }

        /// <summary>
        /// Handles the errors.
        /// </summary>
        /// <param name="errors">The errors.</param>
        private static void HandleErrors(IEnumerable<Error> errors)
        {
            errors.ForEach(e =>
            {
                Console.WriteLine(e.ToString());
            });
        }
    }
}
