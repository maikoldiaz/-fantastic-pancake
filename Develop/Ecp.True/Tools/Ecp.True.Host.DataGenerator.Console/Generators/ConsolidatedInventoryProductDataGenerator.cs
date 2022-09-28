// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidatedInventoryProductDataGenerator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.DataGenerator.Console.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.DataGenerator.Console.Interfaces;

    /// <summary>
    /// The Consolidated Inventory Product Data Generator.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.DataGenerator.Console.Generators.DataGeneratorBase" />
    /// <seealso cref="Ecp.True.Host.DataGenerator.Console.Interfaces.IDataGenerator" />
    public class ConsolidatedInventoryProductDataGenerator : DataGeneratorBase, IDataGenerator
    {
        /// <summary>
        /// The consolidated inventory product repository.
        /// </summary>
        private readonly IRepository<ConsolidatedInventoryProduct> consolidatedInventoryProductRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsolidatedInventoryProductDataGenerator"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ConsolidatedInventoryProductDataGenerator(IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            this.consolidatedInventoryProductRepository = unitOfWork.CreateRepository<ConsolidatedInventoryProduct>();
        }

        /// <summary>
        /// Generates the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The identity.
        /// </returns>
        public Task<int> GenerateAsync(IDictionary<string, object> parameters)
        {
            ArgumentValidators.ThrowIfNull(parameters, nameof(parameters));

            var consolidatedInventoryProduct = GetConsolidatedInventoryProduct(parameters);
            this.consolidatedInventoryProductRepository.Insert(consolidatedInventoryProduct);
            return Task.FromResult(consolidatedInventoryProduct.ConsolidatedInventoryProductId);
        }

        private static ConsolidatedInventoryProduct GetConsolidatedInventoryProduct(IDictionary<string, object> parameters)
        {
            var consolidatedInventoryProduct = new ConsolidatedInventoryProduct
            {
                NodeId = GetInt(parameters, "NodeId", 123),
                ProductId = GetString(parameters, "ProductId", "10000002318"),
                InventoryDate = GetDate(parameters, "InventoryDate", DateTime.UtcNow.ToTrue()),
                ProductVolume = GetDecimal(parameters, "ProductVolume", 100000.00M),
                GrossStandardQuantity = GetDecimal(parameters, "GrossStandardQuantity", 120000.00M),
                MeasurementUnit = "31",
                TicketId = GetInt(parameters, "TicketId", 1),
                SegmentId = GetInt(parameters, "SegmentId", 1),
                SourceSystemId = GetInt(parameters, "SourceSystemId", (int)SystemType.TRUE),
                ExecutionDate = GetDate(parameters, "ExecutionDate", DateTime.UtcNow.ToTrue()),
                IsActive = true,
            };
            GetConsolidatedOwners(parameters).ForEach(x => consolidatedInventoryProduct.ConsolidatedOwners.Add(x));
            return consolidatedInventoryProduct;
        }

        private static IEnumerable<ConsolidatedOwner> GetConsolidatedOwners(IDictionary<string, object> parameters)
        {
            var volume = parameters.TryGetValue("ProductVolume", out object productVolume) ? Convert.ToDecimal(productVolume, CultureInfo.InvariantCulture) : 10000.00M;
            return new List<ConsolidatedOwner>
            {
                new ConsolidatedOwner
            {
                OwnerId = 30,
                OwnershipVolume = volume * 0.60M,
                OwnershipPercentage = 0.60M,
            },
                new ConsolidatedOwner
            {
                OwnerId = 124,
                OwnershipVolume = volume * 0.40M,
                OwnershipPercentage = 0.40M,
            },
            };
        }
    }
}
