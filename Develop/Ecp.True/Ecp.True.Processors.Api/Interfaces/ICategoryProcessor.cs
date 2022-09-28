// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICategoryProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Processors.Core.Interfaces;

    /// <summary>
    /// The category processor.
    /// </summary>
    public interface ICategoryProcessor : IProcessor
    {
        /// <summary>
        /// Creates the category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>Return the status of create category operation.</returns>
        Task CreateCategoryAsync(Category category);

        /// <summary>
        /// Updates the category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>Return the status of update category operation.</returns>
        Task UpdateCategoryAsync(Category category);

        /// <summary>
        /// Gets the category by identifier.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns>Return category by category id.</returns>
        Task<Category> GetCategoryByIdAsync(int categoryId);

        /// <summary>
        /// Determines whether [is category exists by name] [the specified category name].
        /// </summary>
        /// <param name="categoryName">Name of the category.</param>
        /// <returns>Return the category Id.</returns>
        Task<int> GetCategoryIdByNameAsync(string categoryName);

        /// <summary>
        /// Creates the elements asynchronous.
        /// </summary>
        /// <param name="categoryElement">The category element.</param>
        /// <returns>
        /// Return the task.
        /// </returns>
        Task CreateElementAsync(CategoryElement categoryElement);

        /// <summary>
        /// Elements the color exists async.
        /// </summary>
        /// <param name="categoryElement">The category element.</param>
        /// <returns>The task.</returns>
        Task<string> ElementColorExistsAsync(CategoryElement categoryElement);

        /// <summary>
        /// Updates the element asynchronous.
        /// </summary>
        /// <param name="categoryElement">The category element.</param>
        /// <returns>
        /// Return the task.
        /// </returns>
        Task UpdateElementAsync(CategoryElement categoryElement);

        /// <summary>
        /// Gets the element by identifier asynchronous.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns>
        /// Return element by element id.
        /// </returns>
        Task<CategoryElement> GetElementByIdAsync(int elementId);

        /// <summary>
        /// Determines whether [is element exists by name asynchronous] [the specified element name].
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <returns>
        /// Return if element exists by name.
        /// </returns>
        Task<CategoryElement> GetElementByNameAsync(int categoryId, string elementName);

        /// <summary>
        /// Updates the element asynchronous.
        /// </summary>
        /// <param name="segments">The segments list.</param>
        /// <returns>
        /// Return the task.
        /// </returns>
        Task UpdateOperationalSegmentsAsync(IEnumerable<OperationalSegment> segments);

        /// <summary>
        /// Updates deviation percentage in segment.
        /// </summary>
        /// <param name="categoryElementList">The category element list.</param>
        /// <returns>
        /// Return the task.
        /// </returns>
        Task UpdateDeviationPercentageAsync(IEnumerable<CategoryElement> categoryElementList);
    }
}
