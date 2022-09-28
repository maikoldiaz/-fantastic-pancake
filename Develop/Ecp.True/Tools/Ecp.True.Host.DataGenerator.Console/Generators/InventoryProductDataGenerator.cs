// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryProductDataGenerator.cs" company="Microsoft">
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
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Host.DataGenerator.Console.Interfaces;

    /// <summary>
    /// The InventoryProductDataGenerator.
    /// </summary>
    /// <seealso cref="IDataGenerator" />
    public class InventoryProductDataGenerator : DataGeneratorBase, IDataGenerator
    {
        /// <summary>
        /// The inventory product repository.
        /// </summary>
        private readonly IRepository<InventoryProduct> inventoryProductRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryProductDataGenerator"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public InventoryProductDataGenerator(IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            this.inventoryProductRepository = unitOfWork.CreateRepository<InventoryProduct>();
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

            var inventoryProduct = GetInventoryProduct(parameters);
            this.inventoryProductRepository.Insert(inventoryProduct);
            return Task.FromResult(inventoryProduct.InventoryProductId);
        }

        /// <summary>
        /// Gets the inventory product.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The InventoryProduct.</returns>
        private static InventoryProduct GetInventoryProduct(IDictionary<string, object> parameters)
        {
            var inventoryProduct = new InventoryProduct
            {
                ProductId = GetString(parameters, "ProductId", "10000002318"),
                ProductType = GetInt(parameters, "ProductType", 87194),
                ProductVolume = GetDecimal(parameters, "ProductVolume", null),
                GrossStandardQuantity = GetDecimal(parameters, "GrossStandardQuantity", null),
                MeasurementUnit = 31,
                BlockchainInventoryProductTransactionId = Guid.NewGuid(),
                SystemTypeId = GetInt(parameters, "SystemTypeId", (int)SystemType.TRUE),
                SourceSystemId = GetInt(parameters, "SourceSystemId", (int)SourceSystem.TRUE),
                DestinationSystem = GetString(parameters, "DestinationSystem", SystemType.TRUE.ToString()),
                EventType = GetString(parameters, "EventType", EventType.Insert.ToString()),
                InventoryId = GetString(parameters, "InventoryId", "DataGenerator"),
                TicketId = GetInt(parameters, "TicketId", null),
                OwnershipTicketId = GetInt(parameters, "OwnershipTicketId", null),
                BlockchainStatus = StatusType.PROCESSING,
            };

            PopulateInventoryProduct(inventoryProduct, parameters);
            inventoryProduct.Ownerships.AddRange(GetOwnerships(parameters));
            inventoryProduct.Owners.AddRange(GetOwners(parameters));
            return inventoryProduct;
        }

        private static void PopulateInventoryProduct(InventoryProduct product, IDictionary<string, object> parameters)
        {
            product.InventoryDate = GetDate(parameters, "InventoryDate", DateTime.UtcNow.ToTrue().Date);
            product.NodeId = GetInt(parameters, "NodeId", 123);
            product.ScenarioId = parameters.TryGetValue("ScenarioId", out object scenarioId) ? (ScenarioType)scenarioId : ScenarioType.OPERATIONAL;
            product.SegmentId = GetInt(parameters, "SegmentId", null);
            product.UncertaintyPercentage = GetDecimal(parameters, "UncertaintyPercentage", 0.2M);
            product.InventoryProductUniqueId = GetString(parameters, "InventoryProductUniqueId", "Inventory");
            product.SystemId = GetInt(parameters, "SystemId", null);
            product.SourceSystemId = GetInt(parameters, "SourceSystemId", (int?)null);
        }

        private static IEnumerable<Ownership> GetOwnerships(IDictionary<string, object> parameters)
        {
            var volume = GetDecimal(parameters, "ProductVolume", 10000.00M);
            var ticketId = parameters.TryGetValue("OwnershipTicketId", out object ticket) ? (int?)ticket : null;
            var notRequiresOwnership = !parameters.TryGetValue("NotRequiresOwnership", out object notRequireOwnership) || Convert.ToBoolean(notRequireOwnership, CultureInfo.InvariantCulture);
            if (notRequiresOwnership)
            {
                return new List<Ownership>();
            }

            return new List<Ownership>
            {
                new Ownership
            {
                OwnerId = 30,
                OwnershipPercentage = 60.00M,
                OwnershipVolume = volume * 0.6M,
                AppliedRule = "1",
                RuleVersion = "1",
                TicketId = ticketId.GetValueOrDefault(),
                ExecutionDate = DateTime.UtcNow.ToTrue(),
                MessageTypeId = MessageType.InventoryOwnership,
                BlockchainStatus = StatusType.PROCESSING,
            },
                new Ownership
            {
                OwnerId = 124,
                OwnershipPercentage = 40.00M,
                OwnershipVolume = volume * 0.4M,
                AppliedRule = "1",
                RuleVersion = "1",
                TicketId = ticketId.GetValueOrDefault(),
                ExecutionDate = DateTime.UtcNow.ToTrue(),
                MessageTypeId = MessageType.InventoryOwnership,
                BlockchainStatus = StatusType.PROCESSING,
            },
            };
        }

        private static IEnumerable<Owner> GetOwners(IDictionary<string, object> parameters)
        {
            var requiresOwner = !parameters.TryGetValue("RequiresOwner", out object requireOwner) || Convert.ToBoolean(requireOwner, CultureInfo.InvariantCulture);
            if (!requiresOwner)
            {
                return new List<Owner>();
            }

            return new List<Owner>
            {
                new Owner
            {
                OwnerId = 30,
                OwnershipValue = 60,
                OwnershipValueUnit = "%",
                InventoryProductId = null,
                MovementTransactionId = null,
                BlockchainStatus = StatusType.PROCESSING,
            },
                new Owner
            {
                OwnerId = 124,
                OwnershipValue = 40,
                OwnershipValueUnit = "%",
                InventoryProductId = null,
                MovementTransactionId = null,
                BlockchainStatus = StatusType.PROCESSING,
            },
            };
        }
    }
}
