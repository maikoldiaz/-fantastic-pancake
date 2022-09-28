// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryProductConsolidationStrategy.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Consolidation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Delta.Entities;

    /// <summary>
    /// The InventoryProductConsolidationStrategy.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Delta.Consolidation.ConsolidationStrategyBase" />
    public class InventoryProductConsolidationStrategy : ConsolidationStrategyBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryProductConsolidationStrategy"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public InventoryProductConsolidationStrategy(
         ITrueLogger logger)
         : base(logger)
        {
        }

        /// <summary>
        /// Consolidates the asynchronous.
        /// </summary>
        /// <param name="batchInfo">The batch info.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        public override async Task ConsolidateAsync(ConsolidationBatch batchInfo, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(batchInfo, nameof(batchInfo));
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));

            var inventoryConsolidationData = new List<ConsolidationInventoryProductData>();
            var repository = unitOfWork.CreateRepository<ConsolidationInventoryProductData>();

            if (batchInfo.ShouldProcessInventory)
            {
                if (await ShouldProcessInitialAsync(unitOfWork, batchInfo.Ticket).ConfigureAwait(false))
                {
                    var initialInventoryParameters = new Dictionary<string, object>
                    {
                        { "@SegmentId", batchInfo.Ticket.CategoryElementId },
                        { "@InventoryDate", batchInfo.Ticket.StartDate.AddDays(-1) },
                    };

                    var initialInventoryConsolidationData = await repository.ExecuteQueryAsync(Repositories.Constants.GetInventoriesForConsolidation, initialInventoryParameters).ConfigureAwait(false);
                    inventoryConsolidationData.AddRange(initialInventoryConsolidationData);
                }
                else
                {
                    this.Logger.LogInformation($"Consolidated Inventories already exist on {batchInfo.Ticket.StartDate.AddDays(-1)} for ticketId {batchInfo.Ticket.TicketId}");
                }

                var parameters = new Dictionary<string, object>
                {
                    { "@SegmentId", batchInfo.Ticket.CategoryElementId },
                    { "@InventoryDate", batchInfo.Ticket.EndDate },
                };

                var finalInventoryConsolidationData = await repository.ExecuteQueryAsync(Repositories.Constants.GetInventoriesForConsolidation, parameters).ConfigureAwait(false);
                inventoryConsolidationData.AddRange(finalInventoryConsolidationData);
            }

            var groupedConsolidatedInventoriesList = inventoryConsolidationData.GroupBy(x => x.InventoryProductId).Select(grouped => grouped.ToList());
            foreach (var groupedConsolidatedInventories in groupedConsolidatedInventoriesList)
            {
                var totalVolume = 0.00M;
                var index = 0;
                if (this.ValidateOwnershipPercentage(groupedConsolidatedInventories))
                {
                    continue;
                }

                CalculateOwnerShipPercentage(groupedConsolidatedInventories, index, totalVolume);
            }

            var consolidatedInventoryRepository = unitOfWork.CreateRepository<ConsolidatedInventoryProduct>();
            var consolidatedInventoryProducts = BuildConsolidatedInventoryProducts(inventoryConsolidationData, batchInfo);

            consolidatedInventoryRepository.InsertAll(consolidatedInventoryProducts);
        }

        private static void CalculateOwnerShipPercentage(List<ConsolidationInventoryProductData> groupedConsolidatedInventories, int index, decimal totalVolume)
        {
            foreach (var groupedConsolidatedInventory in groupedConsolidatedInventories)
            {
                index++;
                groupedConsolidatedInventory.OwnershipPercentage = groupedConsolidatedInventory.OwnershipVolume;
                if (index == groupedConsolidatedInventories.Count)
                {
                    groupedConsolidatedInventory.OwnershipVolume = groupedConsolidatedInventory.ProductVolume - totalVolume;
                }
                else
                {
                    groupedConsolidatedInventory.OwnershipVolume = Math.Round(groupedConsolidatedInventory.ProductVolume * groupedConsolidatedInventory.OwnershipVolume / 100, 2);
                    totalVolume += groupedConsolidatedInventory.OwnershipVolume;
                }
            }
        }

        private static IEnumerable<ConsolidatedInventoryProduct> BuildConsolidatedInventoryProducts(
            IEnumerable<ConsolidationInventoryProductData> inventoryConsolidationData,
            ConsolidationBatch batchInfo)
        {
            var comparer = ExpressionEqualityComparer.Create<ConsolidationInventoryProductData, int>(x => x.InventoryProductId);
            var consolidatedInventoryProducts = inventoryConsolidationData.Distinct(comparer).GroupBy(x => new
            {
                x.NodeId,
                x.ProductId,
                x.InventoryDate,
                x.MeasurementUnit,
            }).Select(y => new ConsolidatedInventoryProduct
            {
                NodeId = y.Key.NodeId,
                ProductId = y.Key.ProductId,
                SourceSystemId = (int)SourceSystem.TRUE,
                InventoryDate = y.Key.InventoryDate,
                MeasurementUnit = y.Key.MeasurementUnit.HasValue ? y.Key.MeasurementUnit.ToString() : null,
                ProductVolume = y.Sum(z => z.ProductVolume),
                GrossStandardQuantity = y.Sum(z => z.GrossStandardQuantity.GetValueOrDefault()),
                ExecutionDate = DateTime.UtcNow.ToTrue(),
                TicketId = batchInfo.Ticket.TicketId,
                SegmentId = batchInfo.Ticket.CategoryElementId,
            }).ToList();

            foreach (var i in consolidatedInventoryProducts)
            {
                var consolidationInventoryProducts = inventoryConsolidationData.Where(x => x.NodeId == i.NodeId
                && string.Equals(x.ProductId, i.ProductId, StringComparison.OrdinalIgnoreCase) && x.InventoryDate == i.InventoryDate);

                var owners = BuildOwners(consolidationInventoryProducts);
                owners.ForEach(o => i.ConsolidatedOwners.Add(o));
            }

            return consolidatedInventoryProducts;
        }

        private static IEnumerable<ConsolidatedOwner> BuildOwners(IEnumerable<ConsolidationInventoryProductData> consolidationInventoryProducts)
        {
            var consolidatedOwners = new List<ConsolidatedOwner>();
            var index = 0;
            var totalPercentages = 0.00M;
            var totalOwnershipVolume = consolidationInventoryProducts.Sum(x => x.OwnershipVolume);
            var groupedConsolidationInventoryProducts = consolidationInventoryProducts.GroupBy(x => x.OwnerId);
            var length = groupedConsolidationInventoryProducts.Count();
            foreach (var x in groupedConsolidationInventoryProducts)
            {
                index++;
                var ownershipVolume = Math.Round(x.Sum(y => y.OwnershipVolume), 2);
                var percentage = totalOwnershipVolume == 0.0M ? 0.0M : Math.Round((ownershipVolume / totalOwnershipVolume) * 100, 2);
                consolidatedOwners.Add(new ConsolidatedOwner
                {
                    OwnerId = x.Key,
                    OwnershipVolume = ownershipVolume,
                    OwnershipPercentage = index == length && totalOwnershipVolume != 0.0M ? 100 - totalPercentages : percentage,
                });
                totalPercentages += percentage;
            }

            return consolidatedOwners;
        }

        /// <summary>
        /// Validates the asynchronous.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="ticket">The ticket.</param>
        /// <returns>true or false.</returns>
        private static async Task<bool> ShouldProcessInitialAsync(IUnitOfWork unitOfWork, Ticket ticket)
        {
            var consolidatedInventoryProductRepository = unitOfWork.CreateRepository<ConsolidatedInventoryProduct>();

            var count = await consolidatedInventoryProductRepository.GetCountAsync(a =>
            a.InventoryDate < ticket.StartDate.Date && a.SegmentId == ticket.CategoryElementId && a.IsActive).ConfigureAwait(false);

            return count == 0;
        }

        /// <summary>
        /// Validates the ownership percentage.
        /// </summary>
        /// <param name="groupedConsolidatedInventories">The grouped consolidated inventories.</param>
        /// <returns>the boolean.</returns>
        private bool ValidateOwnershipPercentage(IEnumerable<ConsolidationInventoryProductData> groupedConsolidatedInventories)
        {
            if (groupedConsolidatedInventories.Any(a => string.Equals(a.OwnershipValueUnit, "%", StringComparison.OrdinalIgnoreCase)))
            {
                var percentageSum = Math.Round(groupedConsolidatedInventories.Sum(a => a.OwnershipVolume), 2);
                if (!groupedConsolidatedInventories.All(a => string.Equals(a.OwnershipValueUnit, "%", StringComparison.OrdinalIgnoreCase)) || percentageSum != 100.00M)
                {
                    var errorMessage = $"The sum of ownership percentage is not 100.00% for inventoryProductId: {groupedConsolidatedInventories.First().InventoryProductId}";
                    this.Logger.LogInformation(errorMessage, $"{groupedConsolidatedInventories.First().InventoryProductId}");
                    throw new InvalidOperationException(errorMessage);
                }

                return false;
            }

            return true;
        }
    }
}
