// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MasterProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Admin
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Processors.Core;

    /// <summary>
    /// The Master Processor Class.
    /// </summary>
    public class MasterProcessor : ProcessorBase, IMasterProcessor
    {
        /// <summary>
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterProcessor" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        public MasterProcessor(IRepositoryFactory factory, IConfigurationHandler configurationHandler)
            : base(factory)
        {
            this.configurationHandler = configurationHandler;
        }

        /// <inheritdoc/>
        public Task<IEnumerable<LogisticCenter>> GetLogisticCentersAsync()
        {
            return this.CreateRepository<LogisticCenter>().GetAllAsync(s => s.IsActive);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<StorageLocation>> GetStorageLocationsAsync()
        {
            return this.CreateRepository<StorageLocation>().GetAllAsync(s => s.IsActive);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<Algorithm>> GetAlgorithmsAsync()
        {
            return this.CreateRepository<Algorithm>().GetAllAsync(null);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<Product>> GetProductsAsync()
        {
            return this.CreateRepository<Product>().GetAllAsync(s => s.IsActive);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<SystemTypeEntity>> GetSystemTypesAsync()
        {
            return this.CreateRepository<SystemTypeEntity>().GetAllAsync(null);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<User>> GetUsersAsync()
        {
            return this.CreateRepository<User>().GetAllAsync(null);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<VariableTypeEntity>> GetVariableTypesAsync()
        {
            return this.CreateRepository<VariableTypeEntity>().GetAllAsync(null);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<Icon>> GetIconsAsync()
        {
            return this.CreateRepository<Icon>().GetAllAsync(null);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<OriginType>> GetOriginTypesAsync()
        {
            return this.CreateRepository<OriginType>().GetAllAsync(null);
        }

        /// <summary>
        /// Gets the scenarios by role asynchronous.
        /// </summary>call
        /// <param name="roles">The roles.</param>
        /// <returns>
        /// Returns scenarios.
        /// </returns>
        public async Task<IEnumerable<Scenario>> GetScenariosByRoleAsync(IEnumerable<string> roles)
        {
            ArgumentValidators.ThrowIfNull(roles, nameof(roles));
            var scenarios = new List<Scenario>();
            var rolesConfig = await this.configurationHandler.GetConfigurationAsync<UserRoleSettings>(ConfigurationConstants.UserRoleSettings).ConfigureAwait(false);
            var roleIds = rolesConfig.Mapping.Where(a => roles.Any(r => a.Value == r)).Select(d => (int)d.Key);

            var featureRoleRepository = this.CreateRepository<FeatureRole>();
            var featureRoles = await featureRoleRepository.GetAllAsync(fr => roleIds.Contains(fr.RoleId), "Feature", "Role", "Feature.Scenario").ConfigureAwait(false);
            featureRoles = featureRoles.GroupBy(f => f.FeatureId).Select(f => f.First()).ToList();
            var featureRoleGrouping = featureRoles.GroupBy(a => a.Feature.ScenarioId);
            foreach (var featureRole in featureRoleGrouping)
            {
                scenarios.Add(this.CreateNewEntity(featureRole.First().Feature.Scenario, featureRole.Select(a => a.Feature)));
            }

            return scenarios.OrderBy(a => a.ScenarioId);
        }

        /// <summary>
        /// Creates the new entity.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="features">The feature.</param>
        /// <returns>
        /// The Scenario.
        /// </returns>
        private Scenario CreateNewEntity(Scenario input, IEnumerable<Feature> features)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));

            return new Scenario
            {
                ScenarioId = input.ScenarioId,
                Name = input.Name,
                Sequence = input.Sequence,
                Features = features.Select(f => new Feature { Name = f.Name, Description = f.Description, Sequence = f.Sequence }),
            };
        }
    }
}
