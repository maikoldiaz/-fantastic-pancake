// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologationMapper.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Ecp.True.Processors.Transform.Tests")]

namespace Ecp.True.Processors.Transform.Homologate
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Transform.Homologate.Entities;

    /// <summary>
    /// The homologation mapper class.
    /// </summary>
    [IoCRegistration(IoCLifetime.Hierarchical)]
    public sealed class HomologationMapper : IHomologationMapper
    {
        /// <summary>
        /// The homologations.
        /// </summary>
        private static readonly ConcurrentDictionary<string, HomologationSystem> Homologations =
            new ConcurrentDictionary<string, HomologationSystem>();

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
        private readonly ITrueLogger<HomologationMapper> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomologationMapper" /> class.
        /// </summary>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <param name="logger">The logger.</param>
        public HomologationMapper(IRepositoryFactory repositoryFactory, ITrueLogger<HomologationMapper> logger)
        {
            this.repositoryFactory = repositoryFactory;
            this.logger = logger;
        }

        /// <summary>
        /// Homologate the specified source value.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="objectName">Name of the object.</param>
        /// <param name="original">The object value.</param>
        /// <returns>
        /// Return homologated value.
        /// </returns>
        public object Homologate(TrueMessage message, string objectName, object original)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            ArgumentValidators.ThrowIfNull(objectName, nameof(objectName));

            if (!Homologations.ContainsKey(message.ToString()))
            {
                this.logger.LogWarning($"{message} has no homologation", message.MessageId);
                throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, Transform.Constants.HomologationNotConfigured, message.SourceSystem, message.TargetSystem));
            }

            var system = Homologations[message.ToString()];

            return message.IsHomologated ? system.TryHomologate(objectName, original, message.Message) : system.Homologate(objectName, original, message.Message);
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        /// <param name="refreshIntervalInSecs">The refresh time in seconds.</param>
        /// <returns>The task.</returns>
        public async Task InitializeAsync(int refreshIntervalInSecs)
        {
            var version = Version.Count > 0 ? Version[0].Item1 : 0;

            // When no version is found
            // Or when last version fetch was refresh secs ago
            if (Version.Count == 0 || Version[0].Item2.AddSeconds(refreshIntervalInSecs) < DateTime.UtcNow.ToTrue())
            {
                this.logger.LogInformation($"Current homologation is v{version}");

                var homologationVersionRepo = this.repositoryFactory.CreateRepository<True.Entities.Admin.Version>();
                var homologationVersion = await homologationVersionRepo.SingleOrDefaultAsync(x => x.Type == nameof(Homologation)).ConfigureAwait(false);

                if (homologationVersion == null)
                {
                    return;
                }

                this.logger.LogInformation($"Latest homologated is v{homologationVersion.Number}");

                if (Version.Count == 0 || homologationVersion.Number > Version[0].Item1)
                {
                    this.logger.LogInformation($"Getting homologations for v{homologationVersion.Number}");
                    await this.DoBuildHomologationsAsync().ConfigureAwait(false);
                }

                if (Version.Count == 0)
                {
                    Version.Add(Tuple.Create(homologationVersion.Number, DateTime.UtcNow.ToTrue()));
                }
                else
                {
                    Version[0] = Tuple.Create(homologationVersion.Number, DateTime.UtcNow.ToTrue());
                }

                this.logger.LogInformation($"Updated homologation to v{homologationVersion.Number}");
                return;
            }

            this.logger.LogInformation($"Skipping homologation for v{version}");
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        internal static void Clear()
        {
            Version.Clear();
            Homologations.Clear();
        }

        /// <summary>
        /// Initialize the mappings.
        /// </summary>
        private async Task DoBuildHomologationsAsync()
        {
            var homologationData = await this.repositoryFactory.HomologationRepository.GetAllAsync(
                                null,
                                "HomologationGroups",
                                "HomologationGroups.Group",
                                "HomologationGroups.HomologationObjects",
                                "HomologationGroups.HomologationDataMapping")
                                .ConfigureAwait(false);

            var homologationObjectTypeRepo = this.repositoryFactory.CreateRepository<HomologationObjectType>();
            var homologationObjectTypes = await homologationObjectTypeRepo.GetAllAsync(null).ConfigureAwait(false);

            foreach (var homologation in homologationData)
            {
                var system = new HomologationSystem(homologationObjectTypes);

                system.AddObjects(homologation.HomologationGroups.SelectMany(g => g.HomologationObjects));
                system.AddMappings(homologation.HomologationGroups.SelectMany(g => g.HomologationDataMapping));

                Homologations.AddOrUpdate(homologation.ToString(), system, (a, b) => system);
            }
        }
    }
}
