// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateHomologationFilterAttribute.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Core;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// The Node Create Validation Attribute.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute" />
    [AttributeUsage(AttributeTargets.Method)]
    [CLSCompliant(false)]
    public sealed class ValidateHomologationFilterAttribute : ActionFilterAttribute
    {
        /// <inheritdoc/>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(next, nameof(next));

            var homologation = (Homologation)context.ActionArguments["homologation"];
            var resourceProvider = (IResourceProvider)context.HttpContext.RequestServices.GetService(typeof(IResourceProvider));
            var repositoryFactory = (IRepositoryFactory)context.HttpContext.RequestServices.GetService(typeof(IRepositoryFactory));
            var errors = await ValidateCreateHomologationAsync(homologation, resourceProvider, repositoryFactory).ConfigureAwait(false);

            if (errors.Count > 0)
            {
                context.Result = context.HttpContext.BuildErrorResult(errors);
                return;
            }

            await base.OnActionExecutionAsync(context, next).ConfigureAwait(false);
        }

        /// <summary>
        /// Validates foreign key references.
        /// </summary>
        /// <param name="homologation">The homologation.</param>
        /// <param name="errors">List of errors.</param>
        /// <param name="resourceProvider">The resource Provider.</param>
        private static void CheckUniqueness(Homologation homologation, ICollection<ErrorInfo> errors, IResourceProvider resourceProvider)
        {
            if (homologation == null || errors == null || resourceProvider == null)
            {
                throw new InvalidDataException(Entities.Constants.InvalidInputType);
            }

            foreach (HomologationGroup homologationGroup in homologation.HomologationGroups)
            {
                var sourceValues = homologationGroup.HomologationDataMapping.Select(x => x.SourceValue);
                if (sourceValues.Count() != sourceValues.Distinct().Count())
                {
                    errors.Add(new ErrorInfo(resourceProvider.GetResource(Entities.Constants.SourceValueShouldBeUnique)));
                }
            }
        }

        /// <summary>
        /// Validate homologation group exists.
        /// </summary>
        /// <param name="homologation">The homologation.</param>
        /// <param name="errors">List of errors.</param>
        /// <param name="resourceProvider">The resource Provider.</param>
        /// <param name="repositoryFactory">The repository Factory.</param>
        private static async Task CheckHomologationGroupExistsAsync(Homologation homologation, ICollection<ErrorInfo> errors, IResourceProvider resourceProvider, IRepositoryFactory repositoryFactory)
        {
            if (homologation == null || errors == null || resourceProvider == null || repositoryFactory == null)
            {
                throw new InvalidDataException(Entities.Constants.InvalidInputType);
            }

            var inputGroup = homologation.HomologationGroups.First();

            var repository = repositoryFactory.CreateRepository<HomologationGroup>();
            var group = await repository.SingleOrDefaultAsync(x => x.GroupTypeId == inputGroup.GroupTypeId
                && x.Homologation.SourceSystemId == homologation.SourceSystemId
                && x.Homologation.DestinationSystemId == homologation.DestinationSystemId).ConfigureAwait(false);

            if (group != null && inputGroup.HomologationGroupId != group.HomologationGroupId)
            {
                errors.Add(new ErrorInfo(resourceProvider.GetResource(Entities.Constants.HomologationAlreadyExists)));
            }
        }

        /// <summary>
        /// Validates foreign key references.
        /// </summary>
        /// <param name="homologation">The homologation.</param>
        /// <param name="errors">The list of errors.</param>
        /// <param name="resourceProvider">The resource provider.</param>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        private static async Task ValidateForeignKeyReferencesAsync(Homologation homologation, ICollection<ErrorInfo> errors, IResourceProvider resourceProvider, IRepositoryFactory repositoryFactory)
        {
            if (homologation == null || errors == null || resourceProvider == null || repositoryFactory == null)
            {
                throw new InvalidDataException(Entities.Constants.InvalidInputType);
            }

            if ((SystemType)homologation.DestinationSystemId == SystemType.TRUE)
            {
                await ValidateHomologationFilterAttribute.ValidateForeignKeyReferencesWhenDestinationTrueAsync(homologation, errors, resourceProvider, repositoryFactory).ConfigureAwait(false);
            }
            else if ((SystemType)homologation.SourceSystemId == SystemType.TRUE)
            {
                await ValidateHomologationFilterAttribute.ValidateForeignKeyReferencesWhenSourceTrueAsync(homologation, errors, resourceProvider, repositoryFactory).ConfigureAwait(false);
            }
            else
            {
                errors.Add(new ErrorInfo(resourceProvider.GetResource(Entities.Constants.InputOrOutputMustBeTrue)));
            }
        }

        /// <summary>
        /// Validates data mapping.
        /// </summary>
        /// <param name="keySelector">The System Value Property Selector.</param>
        /// <param name="homologationGroup">The homologation group.</param>
        /// <param name="errors">The list of errors.</param>
        /// <param name="resourceProvider">The resource provider.</param>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns>
        /// The Task.
        /// </returns>
        private static async Task ValidateDataMappingAsync<TEntity>(
            Func<HomologationDataMapping, string> keySelector,
            HomologationGroup homologationGroup,
            ICollection<ErrorInfo> errors,
            IResourceProvider resourceProvider,
            IRepositoryFactory repositoryFactory)
            where TEntity : class, IEntity
        {
            if (homologationGroup != null && repositoryFactory != null && errors != null && resourceProvider != null)
            {
                var repository = repositoryFactory.CreateRepository<TEntity>();
                List<Task<TEntity>> tasks = new List<Task<TEntity>>();
                tasks.AddRange(homologationGroup.HomologationDataMapping.Select(dataMapping => repository.GetByIdAsync(Convert.ToInt32(keySelector(dataMapping), CultureInfo.InvariantCulture))));
                await Task.WhenAll(tasks).ConfigureAwait(false);
                if (tasks.Any(t => t.Result == null))
                {
                    errors.Add(new ErrorInfo(resourceProvider.GetResource(Entities.Constants.EntityNotExists)));
                }
            }
        }

        /// <summary>
        /// Validates data mapping.
        /// </summary>
        /// <param name="keySelector">The System Value Property Selector.</param>
        /// <param name="homologationGroup">The homologation group.</param>
        /// <param name="errors">The list of errors.</param>
        /// <param name="resourceProvider">The resource provider.</param>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns>
        /// The Task.
        /// </returns>
        private static async Task ValidateDataMappingProductAsync<TEntity>(
            Func<HomologationDataMapping, string> keySelector,
            HomologationGroup homologationGroup,
            ICollection<ErrorInfo> errors,
            IResourceProvider resourceProvider,
            IRepositoryFactory repositoryFactory)
            where TEntity : class, IEntity
        {
            if (homologationGroup != null && repositoryFactory != null && errors != null && resourceProvider != null)
            {
                var repository = repositoryFactory.CreateRepository<TEntity>();
                List<Task<TEntity>> tasks = new List<Task<TEntity>>();
                tasks.AddRange(homologationGroup.HomologationDataMapping.Select(dataMapping => repository.GetByIdAsync(keySelector(dataMapping))));
                await Task.WhenAll(tasks).ConfigureAwait(false);
                if (tasks.Any(t => t.Result == null))
                {
                    errors.Add(new ErrorInfo(resourceProvider.GetResource(Entities.Constants.EntityNotExists)));
                }
            }
        }

        /// <summary>
        /// Validates foreign key references when Source is TRUE.
        /// </summary>
        /// <param name="homologation">The homologation.</param>
        /// <param name="errors">The list of errors.</param>
        /// <param name="resourceProvider">The resource provider.</param>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        private static async Task ValidateForeignKeyReferencesWhenSourceTrueAsync(
            Homologation homologation, ICollection<ErrorInfo> errors, IResourceProvider resourceProvider, IRepositoryFactory repositoryFactory)
        {
            if (homologation == null || errors == null || resourceProvider == null || repositoryFactory == null)
            {
                throw new InvalidDataException(Entities.Constants.InvalidInputType);
            }

            foreach (HomologationGroup homologationGroup in homologation.HomologationGroups)
            {
                switch (homologationGroup.GroupTypeId)
                {
                    case Constants.GroupTypeNode:
                        await ValidateDataMappingAsync<Node>(d => d.SourceValue, homologationGroup, errors, resourceProvider, repositoryFactory).ConfigureAwait(false);
                        break;
                    case Constants.GroupTypeProduct:
                        await ValidateDataMappingProductAsync<Product>(d => d.SourceValue, homologationGroup, errors, resourceProvider, repositoryFactory).ConfigureAwait(false);
                        break;
                    case Constants.GroupTypeStorageLocation:
                        await ValidateDataMappingAsync<NodeStorageLocation>(d => d.SourceValue, homologationGroup, errors, resourceProvider, repositoryFactory).ConfigureAwait(false);
                        break;
                    default:
                        await ValidateDataMappingAsync<CategoryElement>(d => d.SourceValue, homologationGroup, errors, resourceProvider, repositoryFactory).ConfigureAwait(false);
                        break;
                }
            }
        }

        /// <summary>
        /// Validates foreign key references when destination is TRUE.
        /// </summary>
        /// <param name="homologation">The homologation.</param>
        /// <param name="errors">The list of errors.</param>
        /// <param name="resourceProvider">The resource provider.</param>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        private static async Task ValidateForeignKeyReferencesWhenDestinationTrueAsync(
            Homologation homologation, ICollection<ErrorInfo> errors, IResourceProvider resourceProvider, IRepositoryFactory repositoryFactory)
        {
            if (homologation == null || errors == null || resourceProvider == null || repositoryFactory == null)
            {
                throw new InvalidDataException(Entities.Constants.InvalidInputType);
            }

            foreach (HomologationGroup homologationGroup in homologation.HomologationGroups)
            {
                switch (homologationGroup.GroupTypeId)
                {
                    case Constants.GroupTypeNode:
                        await ValidateDataMappingAsync<Node>(d => d.DestinationValue, homologationGroup, errors, resourceProvider, repositoryFactory).ConfigureAwait(false);
                        break;
                    case Constants.GroupTypeProduct:
                        await ValidateDataMappingProductAsync<Product>(d => d.DestinationValue, homologationGroup, errors, resourceProvider, repositoryFactory).ConfigureAwait(false);
                        break;
                    case Constants.GroupTypeStorageLocation:
                        await ValidateDataMappingAsync<NodeStorageLocation>(d => d.DestinationValue, homologationGroup, errors, resourceProvider, repositoryFactory).ConfigureAwait(false);
                        break;
                    default:
                        await ValidateDataMappingAsync<CategoryElement>(d => d.DestinationValue, homologationGroup, errors, resourceProvider, repositoryFactory).ConfigureAwait(false);
                        break;
                }
            }
        }

        /// <summary>
        /// Validates the create node data.
        /// </summary>
        /// <param name="homologation">The node.</param>
        /// <param name="resourceProvider">The resource provider.</param>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <returns>
        /// The collection of ErrorInfo.
        /// </returns>
        private static async Task<List<ErrorInfo>> ValidateCreateHomologationAsync(Homologation homologation, IResourceProvider resourceProvider, IRepositoryFactory repositoryFactory)
        {
            var errors = new List<ErrorInfo>();
            if (homologation.HomologationId != 0)
            {
                errors.Add(new ErrorInfo(resourceProvider.GetResource(Entities.Constants.InvalidInputType)));
            }
            else if (homologation.SourceSystemId == homologation.DestinationSystemId)
            {
                errors.Add(new ErrorInfo(resourceProvider.GetResource(Entities.Constants.HomologationInputOutputDifferent)));
            }
            else if (homologation.SourceSystemId == null || homologation.DestinationSystemId == null)
            {
                errors.Add(new ErrorInfo(resourceProvider.GetResource(Entities.Constants.EntityNotExists)));
            }
            else if (homologation.SourceSystemId != (int)SystemType.TRUE && homologation.DestinationSystemId != (int)SystemType.TRUE)
            {
                errors.Add(new ErrorInfo(resourceProvider.GetResource(Entities.Constants.InputOrOutputMustBeTrue)));
            }
            else if (homologation.HomologationGroups.Count > 1)
            {
                errors.Add(new ErrorInfo(resourceProvider.GetResource(Entities.Constants.HomologationGroupShouldNotRepeat)));
            }
            else
            {
                CheckUniqueness(homologation, errors, resourceProvider);
                await ValidateForeignKeyReferencesAsync(homologation, errors, resourceProvider, repositoryFactory).ConfigureAwait(false);
                await CheckHomologationGroupExistsAsync(homologation, errors, resourceProvider, repositoryFactory).ConfigureAwait(false);
            }

            return errors;
        }
    }
}
