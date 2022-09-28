// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologationProcessor.cs" company="Microsoft">
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
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Processors.Core;
    using EntityConstants = Ecp.True.Entities.Constants;

    /// <summary>
    ///     The Homologation Processor.
    /// </summary>
    public class HomologationProcessor : ProcessorBase, IHomologationProcessor
    {
        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomologationProcessor" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public HomologationProcessor(IRepositoryFactory factory, IUnitOfWorkFactory unitOfWorkFactory)
            : base(factory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Saves the homologation asynchronous.
        /// </summary>
        /// <param name="homologation">The homologation.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        public async Task SaveHomologationAsync(Homologation homologation)
        {
            ArgumentValidators.ThrowIfNull(homologation, nameof(homologation));
            using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var repository = unitOfWork.CreateRepository<Homologation>();
                var existing = await repository.SingleOrDefaultAsync(
                    x => x.SourceSystemId == homologation.SourceSystemId &&
                    x.DestinationSystemId == homologation.DestinationSystemId,
                    "HomologationGroups").ConfigureAwait(false);
                if (existing == null)
                {
                    repository.Insert(homologation);
                }
                else
                {
                    var repo = unitOfWork.CreateRepository<HomologationGroup>();
                    var group = homologation.HomologationGroups.Single();
                    var existingGroup = await repo.FirstOrDefaultAsync(
                        x => x.HomologationGroupId == group.HomologationGroupId,
                        "HomologationObjects",
                        "HomologationDataMapping").ConfigureAwait(false);

                    if (existingGroup == null)
                    {
                        group.HomologationId = existing.HomologationId;
                        repo.Insert(group);
                    }
                    else
                    {
                        existingGroup.CopyFrom(group);
                        repo.Update(existingGroup);
                    }
                }

                var versionRepository = unitOfWork.CreateRepository<Version>();

                var version = await versionRepository.SingleOrDefaultAsync(x => x.Type == nameof(Homologation)).ConfigureAwait(false);
                if (version == null)
                {
                    version = new Version { Type = nameof(Homologation), Number = 1 };
                    versionRepository.Insert(version);
                }
                else
                {
                    version.Number += 1;
                    versionRepository.Update(version);
                }

                await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the Homologation hierarchy by identifier.
        /// </summary>
        /// <param name="homologationId">The homologation identifier.</param>
        /// <returns>Return Homologation hierarchy by homologationId.</returns>
        public Task<Homologation> GetHomologationByIdAsync(int homologationId)
        {
            return this.RepositoryFactory
                        .HomologationRepository
                        .SingleOrDefaultAsync(x => x.HomologationId == homologationId, "HomologationGroups", "HomologationGroups.HomologationObjects", "HomologationGroups.HomologationDataMapping");
        }

        /// <inheritdoc/>
        public Task<HomologationGroup> GetHomologationByIdAndGroupIdAsync(int homologationId, int groupTypeId)
        {
            return this.RepositoryFactory.HomologationRepository.GetHomologationByIdAndGroupIdAsync(homologationId, groupTypeId);
        }

        /// <summary>
        /// Delete Homologation Group.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns>The Task.</returns>
        public async Task DeleteHomologationGroupAsync(DeleteHomologationGroup group)
        {
            ArgumentValidators.ThrowIfNull(group, nameof(group));
            using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var repository = unitOfWork.CreateRepository<Homologation>();
                var homologation = await repository.SingleOrDefaultAsync(
                    x => x.HomologationId == group.HomologationId,
                    "HomologationGroups",
                    "HomologationGroups.HomologationObjects",
                    "HomologationGroups.HomologationDataMapping").ConfigureAwait(false);
                var homologationGroup = homologation?.HomologationGroups.FirstOrDefault(x => x.HomologationGroupId == group.HomologationGroupId);

                if (homologation?.HomologationGroups?.Count == 1
                && homologationGroup?.HomologationGroupId == group.HomologationGroupId)
                {
                    repository.Delete(homologation);
                }
                else if (homologationGroup != null)
                {
                    var homologationGroupRepository = unitOfWork.CreateRepository<HomologationGroup>();
                    var existingGroup = await homologationGroupRepository
                        .SingleOrDefaultAsync(x => x.HomologationGroupId == group.HomologationGroupId, "HomologationObjects", "HomologationDataMapping")
                        .ConfigureAwait(false);
                    existingGroup.RowVersion = group.RowVersion;
                    homologationGroupRepository.Delete(existingGroup);
                }
                else
                {
                    throw new KeyNotFoundException(EntityConstants.HomologationGroupDoesNotExists);
                }

                await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the HomologationObjectType asynchronous.
        /// </summary>
        /// <returns>
        /// The HomologationObjectType.
        /// </returns>
        public async Task<IEnumerable<HomologationObjectType>> GetHomologationObjectTypesAsync()
        {
            var homologationObjectTypeRepository = this.CreateRepository<HomologationObjectType>();
            var homologationObjectTypes = await homologationObjectTypeRepository.GetAllAsync(null).ConfigureAwait(false);
            return homologationObjectTypes;
        }

        /// <summary>
        /// Check Homologation Group Exists.
        /// </summary>
        /// <param name="groupTypeId">The group type Id.</param>
        /// <param name="sourceSystemId">The source system Id.</param>
        /// <param name="destinationSystemId">The destination system Id.</param>
        /// <returns>Check homologation group for source, destination system.</returns>
        public async Task<HomologationGroup> CheckHomologationGroupExistsAsync(int groupTypeId, int sourceSystemId, int destinationSystemId)
        {
            var homologationGroupRepository = this.CreateRepository<HomologationGroup>();
            return await homologationGroupRepository.SingleOrDefaultAsync(x => x.GroupTypeId == groupTypeId
                && x.Homologation.SourceSystemId == sourceSystemId
                && x.Homologation.DestinationSystemId == destinationSystemId)
            .ConfigureAwait(false);
        }

        /// <summary>
        /// Returns homologation group details.
        /// </summary>
        /// <param name="homologationGroupId">The homologation Group Id.</param>
        /// <returns>Check homologation group for source, destination system.</returns>
        public Task<IEnumerable<HomologationDataMapping>> GetHomologationGroupMappingsAsync(int homologationGroupId)
        {
            return this.RepositoryFactory.HomologationRepository.GetHomologationGroupMappingsAsync(homologationGroupId);
        }
    }
}