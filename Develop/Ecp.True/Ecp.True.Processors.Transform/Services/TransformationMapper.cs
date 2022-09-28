// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformationMapper.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Transform.Services.Interfaces;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The transformation mapper.
    /// </summary>
    [IoCRegistration(IoCLifetime.Hierarchical)]
    public class TransformationMapper : ITransformationMapper
    {
        /// <summary>
        /// The movement transformations map.
        /// </summary>
        private static readonly ConcurrentDictionary<string, Transformation> MovementMap =
            new ConcurrentDictionary<string, Transformation>();

        /// <summary>
        /// The inventory transformations map.
        /// </summary>
        private static readonly ConcurrentDictionary<string, Transformation> InventoryMap =
            new ConcurrentDictionary<string, Transformation>();

        /// <summary>
        /// The version.
        /// </summary>
        private static readonly IList<Tuple<int, DateTime>> Version = new List<Tuple<int, DateTime>>();

        /// <summary>
        /// The repository factory.
        /// </summary>
        private readonly IRepositoryFactory repositoryFactory;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<TransformationMapper> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformationMapper" /> class.
        /// </summary>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <param name="logger">The logger.</param>
        public TransformationMapper(IRepositoryFactory repositoryFactory, ITrueLogger<TransformationMapper> logger)
        {
            this.repositoryFactory = repositoryFactory;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task InitializeAsync(int refreshIntervalInSecs)
        {
            var version = Version.Count > 0 ? Version[0].Item1 : 0;

            // When no version is found
            // Or when last version fetch was refresh secs ago
            if (Version.Count == 0 || Version[0].Item2.AddSeconds(refreshIntervalInSecs) < DateTime.UtcNow.ToTrue())
            {
                this.logger.LogInformation($"Current transformation is v{version}");

                var versionRepository = this.repositoryFactory.CreateRepository<True.Entities.Admin.Version>();
                var versionEntity = await versionRepository.SingleOrDefaultAsync(x => x.Type == nameof(Transformation)).ConfigureAwait(false);

                if (versionEntity == null)
                {
                    return;
                }

                this.logger.LogInformation($"Latest transformation is v{versionEntity.Number}");

                if (Version.Count == 0 || versionEntity.Number > Version[0].Item1)
                {
                    this.logger.LogInformation($"Getting transformations for v{versionEntity.Number}");
                    await this.DoBuildTransformationsAsync().ConfigureAwait(false);
                }

                if (Version.Count == 0)
                {
                    Version.Add(Tuple.Create(versionEntity.Number, DateTime.UtcNow.ToTrue()));
                }
                else
                {
                    Version[0] = Tuple.Create(versionEntity.Number, DateTime.UtcNow.ToTrue());
                }

                this.logger.LogInformation($"Updated transformation to v{versionEntity.Number}");
                return;
            }

            this.logger.LogInformation($"Skipping transformation for v{version}");
        }

        /// <inheritdoc/>
        public void Transform(JToken jobject)
        {
            ArgumentValidators.ThrowIfNull(jobject, nameof(jobject));
            string messageType = jobject.Value<string>(Constants.Type);
            if (messageType.EqualsIgnoreCase(MessageType.Movement.ToString()))
            {
                TransformMovement(jobject);
            }

            if (messageType.EqualsIgnoreCase(MessageType.Inventory.ToString()))
            {
                TransformInventory(jobject);
            }
        }

        private static void TransformInventory(JToken inventory)
        {
            ArgumentValidators.ThrowIfNull(inventory, nameof(inventory));
            string messageType = inventory.Value<string>(Constants.Type);
            if (!messageType.EqualsIgnoreCase(nameof(MessageType.Inventory)))
            {
                return;
            }

            var nodeId = inventory.Value<string>(Constants.NodeId);

            var productId = inventory.Value<string>("ProductId");
            var measurementUnit = inventory.Value<string>("MeasurementUnit");
            var key = $"{nodeId}-{productId}-{measurementUnit}";

            if (InventoryMap.ContainsKey(key))
            {
                var transformation = InventoryMap[key];
                inventory["ProductId"] = transformation.DestinationSourceProductId;
                inventory["MeasurementUnit"] = transformation.DestinationMeasurementId;
                inventory[Constants.NodeId] = transformation.DestinationSourceNodeId;
            }
        }

        private static void TransformMovement(JToken movement)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));
            string messageType = movement.Value<string>(Constants.Type);
            if (!messageType.EqualsIgnoreCase(nameof(MessageType.Movement)))
            {
                return;
            }

            var movementSource = movement.Value<JToken>("MovementSource");
            var movementDestination = movement.Value<JToken>("MovementDestination");
            if (IsNullOrEmpty(movementSource) || IsNullOrEmpty(movementDestination))
            {
                return;
            }

            var sourceNodeId = movementSource.Value<string>("SourceNodeId");
            var sourceProductId = movementSource.Value<string>("SourceProductId");
            var destinationNodeId = movementDestination.Value<string>("DestinationNodeId");
            var destinationProductId = movementDestination.Value<string>("DestinationProductId");

            var key = $"{sourceNodeId}-{sourceProductId}-{destinationNodeId}-{destinationProductId}";

            if (MovementMap.ContainsKey(key))
            {
                var transformation = MovementMap[key];
                movementSource["SourceNodeId"] = transformation.DestinationSourceNodeId;
                movementSource["SourceProductId"] = transformation.DestinationSourceProductId;
                movementDestination["DestinationNodeId"] = transformation.DestinationDestinationNodeId;
                movementDestination["DestinationProductId"] = transformation.DestinationDestinationProductId;
                movement["MovementSource"] = movementSource;
                movement["MovementDestination"] = movementDestination;
            }
        }

        private static bool IsNullOrEmpty(JToken token)
        {
            return (token == null) ||
                   (token.Type == JTokenType.Object && !token.HasValues) ||
                   (token.Type == JTokenType.Null);
        }

        /// <summary>
        /// Initialize the mappings.
        /// </summary>
        private async Task DoBuildTransformationsAsync()
        {
            var transformationRepository = this.repositoryFactory.CreateRepository<Transformation>();
            var transformations = await transformationRepository.GetAllAsync(t => t.IsDeleted == false).ConfigureAwait(false);

            MovementMap.Clear();
            InventoryMap.Clear();

            transformations.ForEach(t =>
            {
                if (t.MessageTypeId == (int)MessageType.Movement)
                {
                    var key = $"{t.OriginSourceNodeId}-{t.OriginSourceProductId}-{t.OriginDestinationNodeId}-{t.OriginDestinationProductId}";
                    MovementMap.AddOrUpdate(key, t, (a, b) => t);
                }
                else
                {
                    var key = $"{t.OriginSourceNodeId}-{t.OriginSourceProductId}-{t.OriginMeasurementId}";
                    InventoryMap.AddOrUpdate(key, t, (a, b) => t);
                }
            });
        }
    }
}
