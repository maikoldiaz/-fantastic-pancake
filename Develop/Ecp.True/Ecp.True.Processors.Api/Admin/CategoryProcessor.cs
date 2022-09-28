// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryProcessor.cs" company="Microsoft">
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
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Processors.Core;
    using EntityConstants = Ecp.True.Entities.Constants;

    /// <summary>
    /// The category processor.
    /// </summary>
    public class CategoryProcessor : ProcessorBase, ICategoryProcessor
    {
        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        /// <summary>
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryProcessor" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        public CategoryProcessor(IRepositoryFactory factory, IUnitOfWorkFactory unitOfWorkFactory, IConfigurationHandler configurationHandler)
            : base(factory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.configurationHandler = configurationHandler;
        }

        /// <summary>
        /// Creates the category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>
        /// Return the status of create category operation.
        /// </returns>
        public async Task CreateCategoryAsync(Category category)
        {
            ArgumentValidators.ThrowIfNull(category, nameof(category));

            var existingId = await this.GetCategoryIdByNameAsync(category.Name).ConfigureAwait(false);
            if (existingId > 0)
            {
                throw new InvalidDataException(EntityConstants.CategoryNameAlreadyExist);
            }

            using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
            var repository = unitOfWork.CreateRepository<Category>();
            repository.Insert(category);

            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>
        /// Return the status of update category operation.
        /// </returns>
        public async Task UpdateCategoryAsync(Category category)
        {
            ArgumentValidators.ThrowIfNull(category, nameof(category));
            var existingId = await this.GetCategoryIdByNameAsync(category.Name).ConfigureAwait(false);
            if (existingId > 0 && existingId != category.CategoryId)
            {
                throw new InvalidDataException(EntityConstants.CategoryNameAlreadyExist);
            }

            using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
            var repository = unitOfWork.CreateRepository<Category>();
            var existing = await repository.GetByIdAsync(category.CategoryId).ConfigureAwait(false);
            if (existing == null)
            {
                throw new KeyNotFoundException(EntityConstants.CategoryNotExists);
            }

            existing.CopyFrom(category);
            repository.Update(existing);

            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the category by identifier.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns>
        /// Return category by category id.
        /// </returns>
        public Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return this.CreateRepository<Category>().GetByIdAsync(categoryId);
        }

        /// <summary>
        /// Determines whether [is category exists by name] [the specified category name].
        /// </summary>
        /// <param name="categoryName">Name of the category.</param>
        /// <returns>
        /// Return true, if category exists by name.
        /// </returns>
        public Task<int> GetCategoryIdByNameAsync(string categoryName)
        {
            return this.CreateRepository<Category>().SingleOrDefaultAsync(x => x.Name == categoryName, x => x.CategoryId);
        }

        /// <summary>
        /// Creates the elements asynchronous.
        /// </summary>
        /// <param name="categoryElement">The category element.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task CreateElementAsync(CategoryElement categoryElement)
        {
            ArgumentValidators.ThrowIfNull(categoryElement, nameof(categoryElement));
            var existsElement = await this.GetElementByNameAsync((int)categoryElement.CategoryId, categoryElement.Name).ConfigureAwait(false);
            if (existsElement != null)
            {
                throw new InvalidDataException(EntityConstants.ElementNameExists);
            }

            using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
            var repository = unitOfWork.CreateRepository<CategoryElement>();
            repository.Insert(categoryElement);

            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Elements the color exists async.
        /// </summary>
        /// <param name="categoryElement">The category element.</param>
        /// <returns>The task.</returns>
        public async Task<string> ElementColorExistsAsync(CategoryElement categoryElement)
        {
            ArgumentValidators.ThrowIfNull(categoryElement, nameof(categoryElement));
            var repository = this.CreateRepository<CategoryElement>();
            var value = await repository.FirstOrDefaultAsync(a =>
                   a.Color == categoryElement.Color &&
                   a.CategoryId == categoryElement.CategoryId && (categoryElement.ElementId == 0 || a.ElementId != categoryElement.ElementId)).ConfigureAwait(false);
            return (value != null && value.Color != null) ? value.Name : string.Empty;
        }

        /// <summary>
        /// Updates the element asynchronous.
        /// </summary>
        /// <param name="categoryElement">The category element.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task UpdateElementAsync(CategoryElement categoryElement)
        {
            ArgumentValidators.ThrowIfNull(categoryElement, nameof(categoryElement));
            var existsElement = await this.GetElementByNameAsync((int)categoryElement.CategoryId, categoryElement.Name).ConfigureAwait(false);
            if (existsElement != null && existsElement.CategoryId != categoryElement.CategoryId)
            {
                throw new InvalidDataException(EntityConstants.ElementNameExists);
            }

            using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var repository = unitOfWork.CreateRepository<CategoryElement>();
                var element = await repository.GetByIdAsync(categoryElement.ElementId).ConfigureAwait(false);
                if (element == null)
                {
                    throw new KeyNotFoundException(EntityConstants.CategoryElementNotExists);
                }

                element.CopyFrom(categoryElement);
                repository.Update(element);

                await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the element by identifier asynchronous.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns>
        /// Return element by element id.
        /// </returns>
        public Task<CategoryElement> GetElementByIdAsync(int elementId)
        {
            var repository = this.CreateRepository<CategoryElement>();
            return repository.SingleOrDefaultAsync(x => x.ElementId == elementId, nameof(Category));
        }

        /// <summary>
        /// Determines whether [is element exists by name asynchronous] [the specified element name].
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <returns>
        /// Return if element exists by name.
        /// </returns>
        public Task<CategoryElement> GetElementByNameAsync(int categoryId, string elementName)
        {
            var repository = this.CreateRepository<CategoryElement>();
            return repository.SingleOrDefaultAsync(x => x.CategoryId == categoryId && x.Name == elementName);
        }

        /// <summary>
        /// Updates the element asynchronous.
        /// </summary>
        /// <param name="segments">The segments list.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task UpdateOperationalSegmentsAsync(IEnumerable<OperationalSegment> segments)
        {
            using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var repository = unitOfWork.CreateRepository<CategoryElement>();

                var existing = await repository.GetAllAsync(x => x.CategoryId == 2 && x.IsOperationalSegment == true && x.IsActive == true).ConfigureAwait(false);
                var input = segments.Select(s => new CategoryElement { ElementId = s.ElementId, RowVersion = s.RowVersion });
                var comparer = ExpressionEqualityComparer.Create<CategoryElement, int>(e => e.ElementId);

                Merge(existing, input, comparer);

                var newSegments = input.Except(existing, comparer);
                var newIds = newSegments.Select(s => s.ElementId);
                var existingNonSon = await repository.GetAllAsync(x => newIds.Contains(x.ElementId)).ConfigureAwait(false);

                Merge(existingNonSon, newSegments, comparer);

                repository.UpdateAll(existing);
                repository.UpdateAll(existingNonSon);

                await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Updates deviation percentage in segment.
        /// </summary>
        /// <param name="categoryElementList">The category element list.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task UpdateDeviationPercentageAsync(IEnumerable<CategoryElement> categoryElementList)
        {
            var systemSettings = await this.configurationHandler.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings).ConfigureAwait(false);

            using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
            {
                foreach (CategoryElement categoryElement in categoryElementList.ToList())
                {
                    if (categoryElement.DeviationPercentage.IsNegative() || categoryElement.DeviationPercentage > 100)
                    {
                        throw new InvalidDataException(EntityConstants.PercentageValidationMessage);
                    }

                    if (categoryElement.DeviationPercentage > systemSettings.MaxDeviationPercentage.GetValueOrDefault())
                    {
                        throw new InvalidDataException(EntityConstants.PercentageOutOfTolerance);
                    }

                    var repository = unitOfWork.CreateRepository<CategoryElement>();
                    var element = await repository.GetByIdAsync(categoryElement.ElementId).ConfigureAwait(false);

                    if (element == null)
                    {
                        throw new KeyNotFoundException(EntityConstants.CategoryElementNotExists);
                    }

                    element.CopyFrom(categoryElement);
                    repository.Update(element);
                }

                await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        private static void Merge(IEnumerable<CategoryElement> existing, IEnumerable<CategoryElement> input, IEqualityComparer<CategoryElement> comparer)
        {
            existing.ForEach(e =>
            {
                e.IsOperationalSegment = input.Contains(e, comparer);
                if (e.IsOperationalSegment == true)
                {
                    e.RowVersion = input.First(c => c.ElementId == e.ElementId).RowVersion;
                }
            });
        }
    }
}
