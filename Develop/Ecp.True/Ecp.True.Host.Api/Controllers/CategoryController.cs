// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryController.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Routing;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The category controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Admin")]
    public class CategoryController : ODataController
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private readonly ICategoryProcessor processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryController"/> class.
        /// </summary>
        /// <param name="processor">The processor.</param>
        public CategoryController(ICategoryProcessor processor)
        {
            this.processor = processor;
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>Returns the status.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/categories")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] Category category)
        {
            await this.processor.CreateCategoryAsync(category).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.CategoryCreatedSuccessfully);
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>Returns the status.</returns>
        [HttpPut]
        [Route("api/v{version:apiVersion}/categories")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> UpdateCategoryAsync([FromBody] Category category)
        {
            await this.processor.UpdateCategoryAsync(category).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.CategoryUpdateSuccessfully);
        }

        /// <summary>
        /// Gets all the categories(both active and inactive).
        /// This method supports ODATA query.
        /// </summary>
        /// <returns>The categories response.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("categories")]
        [ODataRoute("categories")]
        [TrueAuthorize]
        public Task<IQueryable<Category>> QueryCategoriesAsync()
        {
            return this.processor.QueryAllAsync<Category>(null);
        }

        /// <summary>
        /// Gets the category by category id.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns>Returns the category.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/categories/{categoryId}")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> GetCategoryByIdAsync(int categoryId)
        {
            var result = await this.processor.GetCategoryByIdAsync(categoryId).ConfigureAwait(false);
            return new EntityResult(result, Entities.Constants.CategoryNotExist);
        }

        /// <summary>
        /// Determines whether the category exists with the same name.
        /// </summary>
        /// <param name="categoryName">Name of the category.</param>
        /// <returns>Return true if category exists by name.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/categories/{categoryName}/exists")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> ExistsCategoryAsync(string categoryName)
        {
            ArgumentValidators.ThrowIfNullOrEmpty(categoryName, nameof(categoryName));

            var result = await this.processor.GetCategoryIdByNameAsync(categoryName).ConfigureAwait(false);
            return new EntityExistsResult(nameof(Category), result > 0);
        }

        /// <summary>
        /// Creates a new category element.
        /// </summary>
        /// <param name="categoryElement">The category element.</param>
        /// <returns>
        /// Return the status of operation.
        /// </returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/categoryelements")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> CreateElementAsync([FromBody] CategoryElement categoryElement)
        {
            var elementName = await this.processor.ElementColorExistsAsync(categoryElement).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(elementName))
            {
                return new EntityResult(elementName);
            }

            await this.processor.CreateElementAsync(categoryElement).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.CategoryElementCreatedSuccessfully);
        }

        /// <summary>
        /// Updates an existing element.
        /// </summary>
        /// <param name="categoryElement">The category element.</param>
        /// <returns>
        /// Return the status of operation.
        /// </returns>
        [HttpPut]
        [Route("api/v{version:apiVersion}/categoryelements")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> UpdateElementAsync([FromBody] CategoryElement categoryElement)
        {
            var elementName = await this.processor.ElementColorExistsAsync(categoryElement).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(elementName))
            {
                return new EntityResult(elementName);
            }

            await this.processor.UpdateElementAsync(categoryElement).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.CategoryElementUpdatedSuccessfully);
        }

        /// <summary>
        /// Gets all the category elements(active and inactive)
        /// This method supports ODATA query.
        /// </summary>
        /// <returns>
        /// The category elements.
        /// </returns>
        [HttpGet]
        [EnableQuery]
        [Route("categoryelements")]
        [ODataRoute("categoryelements")]
        [TrueAuthorize]
        public Task<IQueryable<CategoryElement>> QueryElementsAsync()
        {
            return this.processor.QueryAllAsync<CategoryElement>(null);
        }

        /// <summary>
        /// Gets all the active category elements in a category.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns>Returns the category elements.</returns>
        /// <response code="200">The ODATA query response.</response>
        [HttpGet]
        [EnableQuery]
        [Route("categories/{categoryId}/elements")]
        [ODataRoute("categories/{categoryId}/elements")]
        [TrueAuthorize(Role.Administrator)]
        public Task<IQueryable<CategoryElement>> QueryActiveElementsByCategoryIdAsync(int categoryId)
        {
            return this.processor.QueryAllAsync<CategoryElement>(c => c.CategoryId == categoryId && c.IsActive == true);
        }

        /// <summary>
        /// Gets the category element by category element id.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns>The catgory element.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/categoryelements/{elementId}")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> GetCategoryElementByIdAsync(int elementId)
        {
            var result = await this.processor.GetElementByIdAsync(elementId).ConfigureAwait(false);
            return new EntityResult(result, Entities.Constants.ElementDoesNotExist);
        }

        /// <summary>
        /// Determines whether the category element exists with same name.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// Returns true if element exists by name.
        /// </returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/categories/elements/exists")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> ExistsCategoryElementAsync(CategoryElement element)
        {
            ArgumentValidators.ThrowIfNull(element, nameof(element));

            var result = await this.processor.GetElementByNameAsync(element.CategoryId.Value, element.Name).ConfigureAwait(false);
            return new EntityExistsResult(nameof(CategoryElement), result);
        }

        /// <summary>
        /// Updates an existing element.
        /// </summary>
        /// <param name="segments">The segments list.</param>
        /// <returns>
        /// Return the status of operation.
        /// </returns>
        [HttpPut]
        [Route("api/v{version:apiVersion}/segments/operational")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> UpdateOperationalSegmentsAsync([FromBody] IEnumerable<OperationalSegment> segments)
        {
            ArgumentValidators.ThrowIfNull(segments, nameof(segments));
            await this.processor.UpdateOperationalSegmentsAsync(segments).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.CategoryElementUpdatedSuccessfully);
        }

        /// <summary>
        /// Updates deviation percentage in segment.
        /// </summary>
        /// <param name="categoryElementList">The category element list.</param>
        /// <returns>
        /// Return the status of operation.
        /// </returns>
        [HttpPut]
        [Route("api/v{version:apiVersion}/segments/deviationpercentage")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> UpdateDeviationPercentageAsync([FromBody] IEnumerable<CategoryElement> categoryElementList)
        {
            await this.processor.UpdateDeviationPercentageAsync(categoryElementList).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.CategoryElementUpdatedSuccessfully);
        }
    }
}
