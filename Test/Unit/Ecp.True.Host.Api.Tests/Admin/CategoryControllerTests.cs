// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryControllerTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Tests.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Api.Controllers;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The category controller tests.
    /// </summary>
    [TestClass]
    public class CategoryControllerTests
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private CategoryController controller;

        /// <summary>
        /// The mock processor.
        /// </summary>
        private Mock<ICategoryProcessor> mockProcessor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockProcessor = new Mock<ICategoryProcessor>();
            this.controller = new CategoryController(this.mockProcessor.Object);
        }

        /// <summary>
        /// Gets the categories asynchronous should return active categories.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetCategoriesAsync_ShouldInvokeProcessor_ToReturnCategoriesAsync()
        {
            var categories = new[] { new Category() }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryAllAsync<Category>(null)).ReturnsAsync(categories);

            var result = await this.controller.QueryCategoriesAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.AreEqual(result, categories);

            this.mockProcessor.Verify(c => c.QueryAllAsync<Category>(null), Times.Once());
        }

        /// <summary>
        /// Creates the category asynchronous should invoke processor to create category.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task CreateCategoryAsync_ShouldInvokeProcessor_ToCreateCategoryAsync()
        {
            var category = new Category();
            this.mockProcessor.Setup(m => m.CreateCategoryAsync(It.IsAny<Category>()));

            var result = await this.controller.CreateCategoryAsync(category).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.CreateCategoryAsync(category), Times.Once());
        }

        /// <summary>
        /// Updates the category asynchronous should invoke processor to update category.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task UpdateCategoryAsync_ShouldInvokeProcessor_ToUpdateCategoryAsync()
        {
            var category = new Category();
            this.mockProcessor.Setup(m => m.UpdateCategoryAsync(It.IsAny<Category>()));

            var result = await this.controller.UpdateCategoryAsync(category).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.UpdateCategoryAsync(category), Times.Once());
        }

        /// <summary>
        /// Gets the category by identifier asynchronous should invoke processor to get category by identifier.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetCategoryByIdAsync_ShouldInvokeProcessor_ToGetCategoryByIdAsync()
        {
            var category = new Category();
            this.mockProcessor.Setup(m => m.GetCategoryByIdAsync(It.IsAny<int>())).ReturnsAsync(category);

            var result = await this.controller.GetCategoryByIdAsync(1).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.GetCategoryByIdAsync(1), Times.Once());
        }

        /// <summary>
        /// Gets the category by identifier asynchronous should invoke processor to get message if category not found asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetCategoryByIdAsync_ShouldInvokeProcessor_ToGetMessageIfCategoryNotFoundAsync()
        {
            this.mockProcessor.Setup(m => m.GetCategoryByIdAsync(1)).ReturnsAsync(default(Category));

            var result = await this.controller.GetCategoryByIdAsync(1).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.GetCategoryByIdAsync(1), Times.Once());
        }

        /// <summary>
        /// Determines whether [is category exists by name asynchronous should invoke processor to verify if category exists by name asynchronous].
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task IsCategoryExistsByNameAsync_ShouldInvokeProcessor_ToVerifyIfCategoryExistsByNameAsync()
        {
            var category = new Category { Name = "Segments" };
            this.mockProcessor.Setup(m => m.GetCategoryIdByNameAsync(category.Name)).ReturnsAsync(10);

            var result = await this.controller.ExistsCategoryAsync(category.Name).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityExistsResult));
            this.mockProcessor.Verify(c => c.GetCategoryIdByNameAsync(category.Name), Times.Once());
        }

        /// <summary>
        /// Creates the element asynchronous should invoke processor to create category element asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CreateElementAsync_ShouldInvokeProcessor_ToCreateCategoryElementAsync()
        {
            var categoryElement = new CategoryElement();
            this.mockProcessor.Setup(m => m.CreateElementAsync(categoryElement));
            this.mockProcessor.Setup(m => m.ElementColorExistsAsync(categoryElement)).ReturnsAsync(string.Empty);

            var result = await this.controller.CreateElementAsync(categoryElement).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.CreateElementAsync(categoryElement), Times.Once());
            this.mockProcessor.Verify(c => c.ElementColorExistsAsync(categoryElement), Times.Once());
        }

        /// <summary>
        /// Creates the element asynchronous should invoke processor to return element exists asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CreateElementAsync_ShouldInvokeProcessor_ToReturnElementExistsAsync()
        {
            var categoryElement = new CategoryElement();
            var outputElement = "element";
            this.mockProcessor.Setup(m => m.CreateElementAsync(categoryElement));
            this.mockProcessor.Setup(m => m.ElementColorExistsAsync(categoryElement)).ReturnsAsync(outputElement);

            var result = await this.controller.CreateElementAsync(categoryElement).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.CreateElementAsync(categoryElement), Times.Never());
            this.mockProcessor.Verify(c => c.ElementColorExistsAsync(categoryElement), Times.Once());
        }

        /// <summary>
        /// Updates the element asynchronous should invoke processor to create category element asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateElementAsync_ShouldInvokeProcessor_ToUpdateCategoryElementAsync()
        {
            var categoryElement = new CategoryElement();
            this.mockProcessor.Setup(m => m.UpdateElementAsync(categoryElement));
            this.mockProcessor.Setup(m => m.ElementColorExistsAsync(categoryElement)).ReturnsAsync(string.Empty);

            var result = await this.controller.UpdateElementAsync(categoryElement).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.UpdateElementAsync(categoryElement), Times.Once());
            this.mockProcessor.Verify(c => c.ElementColorExistsAsync(categoryElement), Times.Once());
        }

        /// <summary>
        /// Updates the element asynchronous should invoke processor to return element exists asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateElementAsync_ShouldInvokeProcessor_ToReturnElementExistsAsync()
        {
            var categoryElement = new CategoryElement();
            var outputElement = "element";
            this.mockProcessor.Setup(m => m.UpdateElementAsync(categoryElement));
            this.mockProcessor.Setup(m => m.ElementColorExistsAsync(categoryElement)).ReturnsAsync(outputElement);

            var result = await this.controller.UpdateElementAsync(categoryElement).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.UpdateElementAsync(categoryElement), Times.Never());
            this.mockProcessor.Verify(c => c.ElementColorExistsAsync(categoryElement), Times.Once());
        }

        /// <summary>
        /// Gets the element by identifier asynchronous should invoke processor to get category element by identifier asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetElementByIdAsync_ShouldInvokeProcessor_ToGetCategoryElementByIdAsync()
        {
            var categoryElement = new CategoryElement();
            this.mockProcessor.Setup(m => m.GetElementByIdAsync(It.IsAny<int>())).ReturnsAsync(categoryElement);

            var result = await this.controller.GetCategoryElementByIdAsync(1).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.GetElementByIdAsync(1), Times.Once());
        }

         /// <summary>
        /// Gets the element by identifier asynchronous should invoke processor to get category element by identifier asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetElementByIdAsync_ShouldInvokeProcessor_ToGetMessageIfNoElementAsync()
        {
            this.mockProcessor.Setup(m => m.GetElementByIdAsync(1)).ReturnsAsync(default(CategoryElement));

            var result = await this.controller.GetCategoryElementByIdAsync(1).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.GetElementByIdAsync(1), Times.Once());
        }

        /// <summary>
        /// Categories the element exists asynchronous should invoke processor to verify if category element exists by name asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CategoryElementExistsAsync_ShouldInvokeProcessor_ToVerifyIfCategoryElementExistsByNameAsync()
        {
            var categoryElement = new CategoryElement { CategoryId = 1,  Name = "Segment Element" };
            this.mockProcessor.Setup(m => m.GetElementByNameAsync(categoryElement.CategoryId.Value, categoryElement.Name)).ReturnsAsync(categoryElement);

            var result = await this.controller.ExistsCategoryElementAsync(categoryElement).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityExistsResult));
            this.mockProcessor.Verify(c => c.GetElementByNameAsync(categoryElement.CategoryId.Value, categoryElement.Name), Times.Once());
        }

        /// <summary>
        /// Gets the categories asynchronous should return active categories.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetCategoryElementsAsync_ShouldInvokeProcessor_ToReturnCategoryElementsAsync()
        {
            var elements = new[] { new CategoryElement() }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryAllAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(elements);

            var result = await this.controller.QueryActiveElementsByCategoryIdAsync(1).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.AreEqual(result, elements);

            this.mockProcessor.Verify(m => m.QueryAllAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>()), Times.Once());
        }

        /// <summary>
        /// Gets the categories asynchronous should return active categories.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetElementsAsync_ShouldInvokeProcessor_ToReturnElementsAsync()
        {
            var elements = new[] { new CategoryElement() }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryAllAsync<CategoryElement>(null)).ReturnsAsync(elements);

            var result = await this.controller.QueryElementsAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.AreEqual(result, elements);

            this.mockProcessor.Verify(m => m.QueryAllAsync<CategoryElement>(null), Times.Once());
        }

        /// <summary>
        /// Gets the categories asynchronous should return active categories.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task UpdateCategoryElementAsync_ShouldInvokeProcessor_ToReturnSuccessResultAsync()
        {
            this.mockProcessor.Setup(m => m.UpdateOperationalSegmentsAsync(It.IsAny<List<OperationalSegment>>())).Returns(Task.CompletedTask);

            var result = await this.controller.UpdateOperationalSegmentsAsync(new List<OperationalSegment>()).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");

            this.mockProcessor.Verify(m => m.UpdateOperationalSegmentsAsync(new List<OperationalSegment>()), Times.Once());
        }

        /// <summary>
        /// Updates the deviation percentage asynchronous should invoke processor to return success result asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task UpdateDeviationPercentageAsync_ShouldInvokeProcessor_ToReturnSuccessResultAsync()
        {
            var categoryElement = new[] { new CategoryElement() };
            this.mockProcessor.Setup(m => m.UpdateDeviationPercentageAsync(categoryElement));

            var result = await this.controller.UpdateDeviationPercentageAsync(categoryElement).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.UpdateDeviationPercentageAsync(categoryElement), Times.Once());
        }
    }
}
