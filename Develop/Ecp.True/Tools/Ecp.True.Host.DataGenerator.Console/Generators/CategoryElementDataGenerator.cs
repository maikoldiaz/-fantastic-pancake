// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryElementDataGenerator.cs" company="Microsoft">
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

    /// <summary>
    /// The CategoryElementDataGenerator.
    /// </summary>
    /// <seealso cref="IDataGenerator" />
    public class CategoryElementDataGenerator : DataGeneratorBase, IDataGenerator
    {
        /// <summary>
        /// The category element repository.
        /// </summary>
        private readonly IRepository<CategoryElement> categoryElementRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryElementDataGenerator"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public CategoryElementDataGenerator(IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));

            this.categoryElementRepository = unitOfWork.CreateRepository<CategoryElement>();
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

            var categoryElement = GetCategoryElement(parameters);
            this.categoryElementRepository.Insert(categoryElement);
            return Task.FromResult(categoryElement.ElementId);
        }

        private static CategoryElement GetCategoryElement(IDictionary<string, object> parameters)
        {
            var categoryElement = new CategoryElement
            {
                Name = GetString(parameters, "Name", "Category Element - Data Generator"),
                Description = GetString(parameters, "Description", "Category Element - Data Generator"),
                IsActive = !parameters.TryGetValue("IsActive", out object isActive) || Convert.ToBoolean(isActive, CultureInfo.InvariantCulture),
                CategoryId = GetInt(parameters, "CategoryId", 9),
                IsOperationalSegment = parameters.TryGetValue("IsOperationalSegment", out object isOperationalSegment) && Convert.ToBoolean(isOperationalSegment, CultureInfo.InvariantCulture),
            };

            return categoryElement;
        }
    }
}
