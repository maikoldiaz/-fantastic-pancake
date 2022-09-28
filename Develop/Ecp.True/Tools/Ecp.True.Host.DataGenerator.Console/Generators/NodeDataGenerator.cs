// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeDataGenerator.cs" company="Microsoft">
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
    using Ecp.True.Host.DataGenerator.Console.Interfaces;
    using EfCore.Models;

    /// <summary>
    /// The NodeDataGenerator.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.DataGenerator.Console.Interfaces.IDataGenerator" />
    public class NodeDataGenerator : DataGeneratorBase, IDataGenerator
    {
        /// <summary>
        /// The node repository.
        /// </summary>
        private readonly IRepository<Node> nodeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeDataGenerator"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public NodeDataGenerator(IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            this.nodeRepository = unitOfWork.CreateRepository<Node>();
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

            var node = GetNode(parameters);
            this.nodeRepository.Insert(node);
            return Task.FromResult(node.NodeId);
        }

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The Node.</returns>
        private static Node GetNode(IDictionary<string, object> parameters)
        {
            var node = new Node
            {
                Name = GetString(parameters, "NodeName", $"DataGenerator_Node{Convert.ToString(DateTime.UtcNow.ToTrue().Ticks, CultureInfo.InvariantCulture)}"),
                Description = $"DataGenerator_Node{Convert.ToString(DateTime.UtcNow.ToTrue().Ticks, CultureInfo.InvariantCulture)}",
                Order = GetInt(parameters, "Order", 1),
                IsActive = true,
                SendToSap = parameters.TryGetValue("SendToSap", out object sendToSap) && Convert.ToBoolean(sendToSap, CultureInfo.InvariantCulture),
                LogisticCenterId = GetString(parameters, "LogisticCenterId", null),
                ControlLimit = GetDecimal(parameters, "ControlLimit", 0.2M),
                AcceptableBalancePercentage = GetDecimal(parameters, "AcceptableBalancePercentage", null),
                UnitId = parameters.TryGetValue("UnitId", out object unitId) ? (int?)unitId : (int?)null,
                Capacity = GetDecimal(parameters, "Capacity", null),
                NodeOwnershipRuleId = GetInt(parameters, "NodeOwnershipRuleId", null),
            };

            node.NodeTags.Add(GetOperatorNodeTag(parameters));
            node.NodeTags.Add(GetSegmentTypeNodeTag(parameters));
            node.NodeTags.Add(GetNodeTypeNodeTag(parameters));

            node.NodeStorageLocations.Add(GetNodeStorageLocation(parameters));

            return node;
        }

        private static NodeStorageLocation GetNodeStorageLocation(IDictionary<string, object> parameters)
        {
            var nodeStorageLocation = new NodeStorageLocation
            {
                Name = parameters.TryGetValue("NodeStorageLocationName", out object name) ?
                Convert.ToString(name, CultureInfo.InvariantCulture) : $"DataGenerator_NodeStorageLocation{Convert.ToString(DateTime.UtcNow.ToTrue().Ticks, CultureInfo.InvariantCulture)}",
                Description = $"DataGenerator_NodeStorageLocation{Convert.ToString(DateTime.UtcNow.ToTrue().Ticks, CultureInfo.InvariantCulture)}",
                StorageLocationTypeId = GetInt(parameters, "StorageLocationTypeId", 1),
                IsActive = true,
                SendToSap = parameters.TryGetValue("SendToSap", out object sendToSap) && Convert.ToBoolean(sendToSap, CultureInfo.InvariantCulture),
                StorageLocationId = GetString(parameters, "StorageLocationId", null),
            };

            var products = GetStorageLocationProduct(parameters);
            foreach (var product in products)
            {
                nodeStorageLocation.Products.Add(product);
            }

            return nodeStorageLocation;
        }

        private static IEnumerable<StorageLocationProduct> GetStorageLocationProduct(IDictionary<string, object> parameters)
        {
            var storageLocationProducts = new List<StorageLocationProduct>();
            storageLocationProducts.Add(new StorageLocationProduct
            {
                ProductId = GetString(parameters, "ProductId1", "30000000004"),
            });
            storageLocationProducts.Add(new StorageLocationProduct
            {
                ProductId = GetString(parameters, "ProductId2", "10000002049"),
            });
            return storageLocationProducts;
        }

        private static NodeTag GetNodeTypeNodeTag(IDictionary<string, object> parameters)
        {
            return new NodeTag
            {
                ElementId = GetInt(parameters, "NodeTypeId", 123),
                StartDate = DateTime.UtcNow.ToTrue().Date.AddMonths(-6),
                EndDate = DateTime.MaxValue.Date,
            };
        }

        private static NodeTag GetSegmentTypeNodeTag(IDictionary<string, object> parameters)
        {
            return new NodeTag
            {
                ElementId = GetInt(parameters, "SegmentId", 123),
                StartDate = DateTime.UtcNow.ToTrue().Date.AddMonths(-6),
                EndDate = DateTime.MaxValue.Date,
            };
        }

        private static NodeTag GetOperatorNodeTag(IDictionary<string, object> parameters)
        {
            return new NodeTag
            {
                ElementId = GetInt(parameters, "OperatorId", 123),
                StartDate = DateTime.UtcNow.ToTrue().Date.AddMonths(-6),
                EndDate = DateTime.MaxValue.Date,
            };
        }
    }
}
