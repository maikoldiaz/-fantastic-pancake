// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnalyticalOwnershipCalculationService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Query;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Ownership.Calculation.Request;
    using Ecp.True.Processors.Ownership.Calculation.Response;
    using Ecp.True.Processors.Ownership.Interfaces;

    /// <summary>
    /// The analytical ownership calculation service.
    /// </summary>
    public class AnalyticalOwnershipCalculationService : ProcessorBase, IAnalyticalOwnershipCalculationService
    {
        /// <summary>
        /// The analytics client.
        /// </summary>
        private readonly IAnalyticsClient analyticsClient;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<AnalyticalOwnershipCalculationService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyticalOwnershipCalculationService" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="analyticsClient">The analysis client.</param>
        /// <param name="logger">The logger.</param>
        public AnalyticalOwnershipCalculationService(
            IRepositoryFactory factory,
            IAnalyticsClient analyticsClient,
            ITrueLogger<AnalyticalOwnershipCalculationService> logger)
            : base(factory)
        {
            this.analyticsClient = analyticsClient;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TransferPointMovement>> GetTransferPointMovementsAsync(int ticketId)
        {
            IEnumerable<TransferPointMovement> transferPointMovementDetails = new List<TransferPointMovement>();

            var parameters = new Dictionary<string, object>
            {
                { "@TicketId", ticketId },
            };

            await this.GetDataAsync<TransferPointMovement>(
                (mov) => transferPointMovementDetails = mov,
                Repositories.Constants.GetTransferPointMovementDetails,
                parameters).ConfigureAwait(false);

            return transferPointMovementDetails;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<OwnershipAnalytics>> GetOwnershipAnalyticalDataAsync(IEnumerable<TransferPointMovement> transferPointMovements)
        {
            ArgumentValidators.ThrowIfNull(transferPointMovements, nameof(transferPointMovements));

            var inputs = transferPointMovements.GroupBy(x => new
                                                     {
                                                         x.AlgorithmId,
                                                         x.MovementType,
                                                         x.SourceNode,
                                                         x.SourceNodeType,
                                                         x.DestinationNode,
                                                         x.DestinationNodeType,
                                                         x.SourceProduct,
                                                         x.SourceProductType,
                                                         x.StartDate,
                                                         x.EndDate,
                                                     })
                                                     .Select(y => new
                                                     {
                                                         Request = new AnalyticalServiceRequestData
                                                         {
                                                             AlgorithmId = y.Key.AlgorithmId.ToString(CultureInfo.InvariantCulture),
                                                             MovementType = y.Key.MovementType,
                                                             SourceNode = y.Key.SourceNode,
                                                             SourceNodeType = y.Key.SourceNodeType,
                                                             DestinationNode = y.Key.DestinationNode,
                                                             DestinationNodeType = y.Key.DestinationNodeType,
                                                             SourceProduct = y.Key.SourceProduct,
                                                             SourceProductType = y.Key.SourceProductType,
                                                             StartDate = y.Key.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                                                             EndDate = y.Key.EndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                                                         },
                                                         Records = y.Select(z => new OwnershipAnalytics
                                                         {
                                                             TicketId = z.TicketId,
                                                             AlgorithmId = z.AlgorithmId,
                                                             MovementId = z.MovementId,
                                                             MovementTransactionId = z.MovementTransactionId,
                                                             MovementTypeId = z.MovementTypeId.ToString(CultureInfo.InvariantCulture),
                                                             SourceNodeId = z.SourceNodeId,
                                                             SourceNodeTypeId = z.SourceNodeTypeId,
                                                             DestinationNodeId = z.DestinationNodeId,
                                                             DestinationNodeTypeId = z.DestinationNodeTypeId,
                                                             SourceProductId = z.SourceProductId,
                                                             SourceProductTypeId = z.SourceProductTypeId.HasValue ? z.SourceProductTypeId.ToString() : null,
                                                             ExecutionDate = z.OperationalDate,
                                                             NetVolume = z.NetVolume,
                                                         }),
                                                     });

            var tasks = new List<Task<IEnumerable<OwnershipAnalytics>>>();
            tasks.AddRange(inputs.Select(a => this.GetHistoricalOwnershipAsync(a.Request, a.Records)));
            var ownershipAnalytics = await Task.WhenAll(tasks).ConfigureAwait(false);

            return ownershipAnalytics.SelectMany(x => x);
        }

        private static IEnumerable<OwnershipAnalytics> PopulateMatchingRecords(IEnumerable<AnalyticalServiceResponseData> serviceResponse, IEnumerable<OwnershipAnalytics> records)
        {
            var otherRecords = new List<OwnershipAnalytics>();
            var ownershipAnalytics = records.ToList();
            var responses = serviceResponse.Distinct(ExpressionEqualityComparer.Create<AnalyticalServiceResponseData, DateTime>(x => x.OperationalDate));
            foreach (var response in responses)
            {
                foreach (var record in ownershipAnalytics.Where(r => r.ExecutionDate.Date == response.OperationalDate.Date))
                {
                    record.OwnershipVolume = response.OwnershipPercentage * record.NetVolume;
                    record.OwnershipPercentage = response.OwnershipPercentage * 100;
                    record.OwnerId = 30;
                    otherRecords.Add(record.GetOtherCopy(100 - record.OwnershipPercentage.GetValueOrDefault(), record.NetVolume - record.OwnershipVolume.GetValueOrDefault(), 124));
                }
            }

            ownershipAnalytics.AddRange(otherRecords);
            return ownershipAnalytics.AsEnumerable();
        }

        private async Task<IEnumerable<OwnershipAnalytics>> GetHistoricalOwnershipAsync(AnalyticalServiceRequestData input, IEnumerable<OwnershipAnalytics> records)
        {
            var ticketId = records.FirstOrDefault()?.TicketId;
            try
            {
                var result = await this.analyticsClient.GetOwnershipAnalyticsAsync(input).ConfigureAwait(false);
                var ownershipAnalytics = PopulateMatchingRecords(result, records);
                return ownershipAnalytics;
            }
            catch (Exception e)
            {
                this.logger.LogError(e, $"{ticketId}", $"Error calling ML service for ticket: {ticketId} with message: {e.Message}.");
            }

            return records;
        }

        /// <summary>
        /// Gets the data from repository asynchronous.
        /// </summary>
        /// <typeparam name="T">The T type.</typeparam>
        /// <param name="setter">The setter.</param>
        /// <param name="procName">Name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The tasks.</returns>
        private async Task GetDataAsync<T>(Action<IEnumerable<T>> setter, string procName, IDictionary<string, object> parameters)
        where T : class, IEntity
        {
            var result = await this.CreateRepository<T>().ExecuteQueryAsync(procName, parameters).ConfigureAwait(false);
            setter(result);
        }
    }
}
