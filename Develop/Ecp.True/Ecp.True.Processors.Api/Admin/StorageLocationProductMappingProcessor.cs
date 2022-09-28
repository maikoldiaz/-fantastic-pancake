// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageLocationProductMappingProcessor.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;

    /// <inheritdoc />
    public class StorageLocationProductMappingProcessor : IStorageLocationProductMappingProcessor
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<StorageLocationProductMapping> mappingRepository;
        private readonly IRepository<Product> productRepository;
        private readonly IRepository<StorageLocation> storageLocationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageLocationProductMappingProcessor"/> class.
        /// </summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public StorageLocationProductMappingProcessor(IUnitOfWorkFactory unitOfWorkFactory)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));

            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.mappingRepository = this.unitOfWork.CreateRepository<StorageLocationProductMapping>();
            this.productRepository = this.unitOfWork.CreateRepository<Product>();
            this.storageLocationRepository = this.unitOfWork.CreateRepository<StorageLocation>();
        }

        /// <summary>
        /// Creates a range of new storage location product mappings.
        /// </summary>
        /// <param name="mappings">The storage location mappings.</param>
        /// <returns>The info about the creation process.</returns>
        public async Task<IEnumerable<StorageLocationProductMappingInfo>> CreateStorageLocationProductMappingAsync(
            IEnumerable<StorageLocationProductMapping> mappings)
        {
            var mappingList = mappings.ThrowIfNullOrEmpty(nameof(mappings));

            var (uniques, duplicated) = GetUniqueAndDuplicated(mappingList);

            var infoList = new List<StorageLocationProductMappingInfo>();

            var existing = mappingList.Where(m =>
            {
                var query = this.mappingRepository.QueryAllAsync(mr =>
                            mr.ProductId == m.ProductId && mr.StorageLocationId == m.StorageLocationId).Result;
                return query.Any();
            }).ToList();

            duplicated = duplicated.Union(existing).ToList();

            duplicated.ForEach(duplicate => AddInfo(infoList, duplicate, EntityInfoCreationStatus.Duplicated, new List<string> { Constants.MappingAlreadyExists }));

            var newMappings = new List<StorageLocationProductMapping>();

            foreach (var unique in uniques.Except(existing))
            {
                var valid = await this.ValidateUniqueMappingsAsync(unique, infoList)
                    .ConfigureAwait(false);
                if (valid is { })
                {
                    newMappings.Add(valid);
                }
            }

            if (!newMappings.Any())
            {
                await this.AddNamesToInfoAsync(infoList).ConfigureAwait(false);
                return infoList;
            }

            this.mappingRepository.InsertAll(newMappings);
            await this.unitOfWork.SaveAsync(CancellationToken.None)
                .ConfigureAwait(false);

            newMappings.ForEach(newMapping => AddInfo(infoList, newMapping, EntityInfoCreationStatus.Created, default));

            await this.AddNamesToInfoAsync(infoList).ConfigureAwait(false);

            return infoList;
        }

        /// <summary>
        /// Remove a storage location product mapping  if has not movements.
        /// </summary>
        /// <param name="mappingId">The storage location product mapping id.</param>
        /// <returns>The task.</returns>
        public async Task DeleteStorageLocationProductMappingAsync(string mappingId)
        {
            ArgumentValidators.ThrowIfNullOrEmpty(mappingId, nameof(mappingId));
            var mapping = await this.mappingRepository.GetByIdAsync(Convert.ToInt32(mappingId, System.Globalization.CultureInfo.InvariantCulture)).ConfigureAwait(false);

            if (mapping == null)
            {
                throw new KeyNotFoundException(Constants.MappingDoesNotExist);
            }

            var result = await this.IsRemovableAsync(mapping).ConfigureAwait(false);

            if (!result)
            {
                throw new KeyNotFoundException(Constants.MappingHasMovements);
            }
            else
            {
                this.mappingRepository.Delete(mapping);
                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        private static void AddInfo(ICollection<StorageLocationProductMappingInfo> infoList, StorageLocationProductMapping mapping, EntityInfoCreationStatus status, IEnumerable<string> errors)
        {
            infoList.Add(new StorageLocationProductMappingInfo
            {
                ProductId = mapping.ProductId,
                StorageLocationId = mapping.StorageLocationId,
                Errors = errors != null ?
                    new ErrorResponse(errors.Select(e => new ErrorInfo(e)).ToList()) : null,
                Status = status,
            });
        }

        private static (IEnumerable<StorageLocationProductMapping>, IEnumerable<StorageLocationProductMapping>) GetUniqueAndDuplicated(IList<StorageLocationProductMapping> mappingList)
        {
            var unique = mappingList
                .Distinct(new StorageLocationProductMapping());

            var duplicated = mappingList.ToList();

            unique.ForEach(u => duplicated.Remove(u));

            return (unique, duplicated);
        }

        private static bool AddErrorMessages(
            StorageLocationProductMapping unique, List<StorageLocationProductMappingInfo> infoList, IQueryable<Product> product, List<string> errors, IQueryable<StorageLocation> storageLocation)
        {
            if (product.Any())
            {
                if (!product.First().IsActive)
                {
                    errors.Add(Constants.ProductIsInactive);
                }
            }
            else
            {
                errors.Add(Constants.ProductDoesNotExist);
            }

            if (storageLocation.Any(s => s.LogisticCenter.IsActive))
            {
                if (!storageLocation.First().IsActive)
                {
                    errors.Add(Constants.StorageLocationIsInactive);
                }
            }
            else
            {
                errors.Add(Constants.StorageLocationDoesNotExist);
            }

            if (errors.Any())
            {
                AddInfo(infoList, unique, EntityInfoCreationStatus.Error, errors);
            }

            return !errors.Any();
        }

        private async Task AddNamesToInfoAsync(IEnumerable<StorageLocationProductMappingInfo> infoList)
        {
            foreach (var info in infoList)
            {
                await this.AddNamesAsync(info).ConfigureAwait(false);
            }
        }

        private async Task AddNamesAsync(StorageLocationProductMappingInfo mapping)
        {
            var product = await this.productRepository.FirstOrDefaultAsync(p => p.ProductId == mapping.ProductId).ConfigureAwait(false);
            var storageLocation = await this.storageLocationRepository
                .FirstOrDefaultAsync(p => p.StorageLocationId == mapping.StorageLocationId, nameof(StorageLocation.LogisticCenter)).ConfigureAwait(false);

            mapping.ProductName = product?.Name;
            mapping.StorageLocationName = storageLocation?.Name;
            mapping.LogisticCenterName = storageLocation?.LogisticCenter.Name;
        }

        private async Task<StorageLocationProductMapping> ValidateUniqueMappingsAsync(
            StorageLocationProductMapping unique, List<StorageLocationProductMappingInfo> infoList)
        {
            var product = await this.productRepository.QueryAllAsync(p =>
                p.ProductId == unique.ProductId).ConfigureAwait(false);

            var storageLocation = await this.storageLocationRepository.QueryAllAsync(
                p => p.StorageLocationId == unique.StorageLocationId,
                nameof(StorageLocation.LogisticCenter)).ConfigureAwait(false);

            var errors = new List<string>();
            var valid = AddErrorMessages(unique, infoList, product, errors, storageLocation);

            return valid ? unique : null;
        }

        /// <summary>
        /// Gets a task that queries whether a StorageLocationProductMapping is removable.
        /// </summary>
        /// <param name="existing">The product.</param>
        /// <returns>The task.</returns>
        private async Task<bool> IsRemovableAsync(StorageLocationProductMapping existing)
        {
            var movementRepo = this.unitOfWork.CreateRepository<Movement>();

            var sourcesWithMovements = await movementRepo
                .QueryAllAsync(m => m.MovementSource.SourceProductId == existing.ProductId)
                .ConfigureAwait(false);

            var destinationWithMovements = await movementRepo
                .QueryAllAsync(m => m.MovementDestination.DestinationProductId == existing.ProductId)
                .ConfigureAwait(false);

            return !(sourcesWithMovements.Any() || destinationWithMovements.Any());
        }
    }
}
