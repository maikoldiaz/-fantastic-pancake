// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryProcessorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Tests.Admin
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Processors.Api.Admin;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The category processor tests.
    /// </summary>
    [TestClass]
    public class CategoryProcessorTests : ProcessorTestBase
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private CategoryProcessor processor;

        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockFactory;

        /// <summary>The unit of work mock factory.</summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The mock configuration handler.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigurationHandler;

        /// <summary>
        /// The mock category repository.
        /// </summary>
        private Mock<IRepository<Category>> mockCategoryRepository;

        /// <summary>
        ///  The mock category element repository.
        /// </summary>
        private Mock<IRepository<CategoryElement>> mockCategoryElementRepository;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockFactory = new Mock<IRepositoryFactory>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockConfigurationHandler = new Mock<IConfigurationHandler>();
            this.mockCategoryRepository = new Mock<IRepository<Category>>();
            this.mockCategoryElementRepository = new Mock<IRepository<CategoryElement>>();
            this.processor = new CategoryProcessor(this.mockFactory.Object, this.mockUnitOfWorkFactory.Object, this.mockConfigurationHandler.Object);
        }

        /// <summary>
        /// Creates the category asynchronous should create category from repository when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task CreateCategoryAsync_ShouldCreateCategoryFromRepository_WhenInvokedAsync()
        {
            var category = new Category();
            var token = new CancellationToken(false);

            var repoMock = new Mock<IRepository<Category>>();
            repoMock.Setup(r => r.Insert(It.IsAny<Category>()));

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Category>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));
            this.mockFactory.Setup(m => m.CreateRepository<Category>()).Returns(repoMock.Object);

            await this.processor.CreateCategoryAsync(category).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Category>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Once);
            repoMock.Verify(r => r.Insert(It.IsAny<Category>()), Times.Once);
        }

        /// <summary>
        /// Updates the category asynchronous should update category from repository when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task UpdateCategoryAsync_ShouldUpdateCategoryFromRepository_WhenInvokedAsync()
        {
            var category = new Category();
            var token = new CancellationToken(false);

            var repoMock = new Mock<IRepository<Category>>();
            repoMock.Setup(r => r.GetByIdAsync(category.CategoryId)).ReturnsAsync(category);

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Category>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));
            this.mockFactory.Setup(m => m.CreateRepository<Category>()).Returns(repoMock.Object);
            await this.processor.UpdateCategoryAsync(category).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Category>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Once);
            repoMock.Verify(r => r.GetByIdAsync(category.CategoryId), Times.Once);
        }

        /// <summary>
        /// Updates the category asynchronous should throw exception if category category from repository when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task UpdateCategoryAsync_ShouldThrowExceptionIfCategoryNotExistFromRepository_WhenInvokedAsync()
        {
            var category = new Category();
            var token = new CancellationToken(false);

            var repoMock = new Mock<IRepository<Category>>();
            repoMock.Setup(r => r.GetByIdAsync(category.CategoryId)).ReturnsAsync(default(Category));

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Category>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));
            repoMock.Setup(r => r.Update(category));
            this.mockFactory.Setup(m => m.CreateRepository<Category>()).Returns(repoMock.Object);
            await this.processor.UpdateCategoryAsync(category).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Category>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Never);
            repoMock.Verify(r => r.GetByIdAsync(category.CategoryId), Times.Once);
            repoMock.Verify(r => r.Update(category), Times.Never);
        }

        /// <summary>
        /// Updates the category asynchronous should throw invalid data exception if category not exist from repository when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task UpdateCategoryAsync_ShouldThrowInvalidDataExceptionIfCategoryNotExistFromRepository_WhenInvokedAsync()
        {
            var token = new CancellationToken(false);
            var category = new Category { CategoryId = 1 };
            var repoMock = new Mock<IRepository<Category>>();
            repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<Expression<Func<Category, int>>>())).ReturnsAsync(123);

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Category>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));
            repoMock.Setup(r => r.Update(category));
            this.mockFactory.Setup(m => m.CreateRepository<Category>()).Returns(repoMock.Object);

            var ex = await Assert.ThrowsExceptionAsync<InvalidDataException>(() => this.processor.UpdateCategoryAsync(category)).ConfigureAwait(false);
            Assert.AreEqual(ex.Message, Ecp.True.Entities.Constants.CategoryNameAlreadyExist);
        }

        /// <summary>
        /// Gets the category by identifier asynchronous should get category by identifier from repository when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetCategoryByIdAsync_ShouldGetCategoryByIdFromRepository_WhenInvokedAsync()
        {
            var categoryId = 1;
            var category = new Category() { CategoryId = categoryId };
            var repoMock = new Mock<IRepository<Category>>();
            repoMock.Setup(r => r.GetByIdAsync(categoryId)).ReturnsAsync(category);
            this.mockFactory.Setup(m => m.CreateRepository<Category>()).Returns(repoMock.Object);

            var result = await this.processor.GetCategoryByIdAsync(categoryId).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(result, category);

            this.mockFactory.Verify(m => m.CreateRepository<Category>(), Times.Once);
            repoMock.Verify(r => r.GetByIdAsync(categoryId), Times.Once);
        }

        /// <summary>
        /// Determines whether [is category exists by name asynchronous should determine if category exists by name from repository when invoked].
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetCategoryByByNameAsync_ShouldDetermineIfCategoryExistsByNameFromRepository_WhenInvokedAsync()
        {
            var repoMock = new Mock<IRepository<Category>>();
            repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<Expression<Func<Category, int>>>())).ReturnsAsync(10);
            this.mockFactory.Setup(m => m.CreateRepository<Category>()).Returns(repoMock.Object);

            var result = await this.processor.GetCategoryIdByNameAsync("Test").ConfigureAwait(false);

            // Assert
            Assert.AreEqual(10, result);

            this.mockFactory.Verify(m => m.CreateRepository<Category>(), Times.Once);
            repoMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<Expression<Func<Category, int>>>()), Times.Once);
        }

        /// <summary>
        /// Creates the element asynchronous should create category element from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task CreateElementAsync_ShouldCreateCategoryElementFromRepository_WhenInvokedAsync()
        {
            var categoryElement = new CategoryElement() { CategoryId = 1 };
            var token = new CancellationToken(false);

            var repoMock = new Mock<IRepository<CategoryElement>>();
            repoMock.Setup(r => r.Insert(It.IsAny<CategoryElement>()));

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<CategoryElement>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));

            this.mockFactory.Setup(m => m.CreateRepository<CategoryElement>()).Returns(repoMock.Object);
            await this.processor.CreateElementAsync(categoryElement).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<CategoryElement>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Once);
            repoMock.Verify(r => r.Insert(It.IsAny<CategoryElement>()), Times.Once);
        }

        /// <summary>
        /// Updates the element asynchronous should update category element from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task UpdateElementAsync_ShouldUpdateCategoryElementFromRepository_WhenInvokedAsync()
        {
            var categoryElement = new CategoryElement() { CategoryId = 1 };
            var token = new CancellationToken(false);

            var repoMock = new Mock<IRepository<CategoryElement>>();
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(categoryElement);
            repoMock.Setup(r => r.Update(It.IsAny<CategoryElement>()));

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<CategoryElement>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));
            this.mockFactory.Setup(m => m.CreateRepository<CategoryElement>()).Returns(repoMock.Object);
            await this.processor.UpdateElementAsync(categoryElement).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<CategoryElement>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Once);
            repoMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
            repoMock.Verify(r => r.Update(It.IsAny<CategoryElement>()), Times.Once);
        }

        /// <summary>
        /// Updates the element asynchronous should update category element from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task UpdateElementAsync_ShouldThrowExceptionIfInvalidCategoryInfoElementFromRepository_WhenInvokedAsync()
        {
            var categoryElement = new CategoryElement() { CategoryId = 1 };
            var token = new CancellationToken(false);

            var repoMock = new Mock<IRepository<CategoryElement>>();
            repoMock.Setup(r => r.GetByIdAsync(categoryElement.CategoryId)).ReturnsAsync(default(CategoryElement));
            repoMock.Setup(r => r.Update(It.IsAny<CategoryElement>()));

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<CategoryElement>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));
            this.mockFactory.Setup(m => m.CreateRepository<CategoryElement>()).Returns(repoMock.Object);
            await this.processor.UpdateElementAsync(categoryElement).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<CategoryElement>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Once);
            repoMock.Verify(r => r.GetByIdAsync(categoryElement.CategoryId), Times.Once);
            repoMock.Verify(r => r.Update(categoryElement), Times.Never);
        }

        /// <summary>
        /// Elements the color exists asynchronous should return null.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ElementColorExistsAsync_ShouldReturnNullAsync()
        {
            CategoryElement categoryElement = null;
            var inputCategoryElement = new CategoryElement() { CategoryId = 1 };
            var repoMock = new Mock<IRepository<CategoryElement>>();
            repoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(categoryElement);
            this.mockFactory.Setup(m => m.CreateRepository<CategoryElement>()).Returns(repoMock.Object);

            await this.processor.ElementColorExistsAsync(inputCategoryElement).ConfigureAwait(false);

            this.mockFactory.Verify(m => m.CreateRepository<CategoryElement>(), Times.Once);
            repoMock.Verify(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Elements the color exists asynchronous should return category element.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ElementColorExistsAsync_ShouldReturnCategoryElementAsync()
        {
            CategoryElement categoryElement = new CategoryElement() { CategoryId = 1 };
            var repoMock = new Mock<IRepository<CategoryElement>>();
            repoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(categoryElement);
            this.mockFactory.Setup(m => m.CreateRepository<CategoryElement>()).Returns(repoMock.Object);

            await this.processor.ElementColorExistsAsync(categoryElement).ConfigureAwait(false);

            this.mockFactory.Verify(m => m.CreateRepository<CategoryElement>(), Times.Once);
            repoMock.Verify(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Gets the element by identifier asynchronous should get category element by identifier from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task GetElementByIdAsync_ShouldGetCategoryElementByIdFromRepository_WhenInvokedAsync()
        {
            var elementId = 1;
            var categoryElement = new CategoryElement() { ElementId = elementId };
            var repoMock = new Mock<IRepository<CategoryElement>>();
            repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>(), nameof(Category))).ReturnsAsync(categoryElement);
            this.mockFactory.Setup(m => m.CreateRepository<CategoryElement>()).Returns(repoMock.Object);

            var result = await this.processor.GetElementByIdAsync(elementId).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(result, categoryElement);

            this.mockFactory.Verify(m => m.CreateRepository<CategoryElement>(), Times.Once);
            repoMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>(), nameof(Category)), Times.Once);
        }

        /// <summary>
        /// Gets the element by name asynchronous should get category element by name from repository when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetElementByNameAsync_ShouldGetCategoryElementByNameFromRepository_WhenInvokedAsync()
        {
            var elementName = "Segment element";
            var categoryId = 1;
            var categoryElement = new CategoryElement();
            var repoMock = new Mock<IRepository<CategoryElement>>();
            repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(categoryElement);
            this.mockFactory.Setup(m => m.CreateRepository<CategoryElement>()).Returns(repoMock.Object);

            var result = await this.processor.GetElementByNameAsync(categoryId, elementName).ConfigureAwait(false);

            Assert.AreEqual(result, categoryElement);
            this.mockFactory.Verify(m => m.CreateRepository<CategoryElement>(), Times.Once);
            repoMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Updates the category asynchronous should throw exception if category exists.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public async Task UpdateCategoryAsync_ShouldThrowExceptionIfCategoryExistFromRepository_WhenInvokedAsync()
        {
            var categoryToUpdate = new Category
            {
                CategoryId = 2,
                Name = "segment",
            };

            this.mockCategoryRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<Expression<Func<Category, int>>>())).ReturnsAsync(10);
            this.mockFactory.Setup(a => a.CreateRepository<Category>()).Returns(this.mockCategoryRepository.Object);

            await this.processor.UpdateCategoryAsync(categoryToUpdate).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates the category asynchronous should throw exception if category  exists.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public async Task CreateCategoryAsync_ShouldThrowExceptionIfCategoryExistFromRepository_WhenInvokedAsync()
        {
            var newCatogory = new Category
            {
                Name = "segment",
            };

            this.mockCategoryRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<Expression<Func<Category, int>>>())).ReturnsAsync(10);
            this.mockFactory.Setup(a => a.CreateRepository<Category>()).Returns(this.mockCategoryRepository.Object);

            await this.processor.CreateCategoryAsync(newCatogory).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates the element asynchronous should throw exception if element  exists.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public async Task CreateElementAsync_ShouldThrowExceptionIfCategoryExistFromRepository_WhenInvokedAsync()
        {
            var existingCategory = new CategoryElement
            {
                CategoryId = 1,
                Name = "segment",
            };

            var newCatogory = new CategoryElement
            {
                CategoryId = 2,
                Name = "segment",
            };

            this.mockCategoryElementRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(existingCategory);
            this.mockFactory.Setup(a => a.CreateRepository<CategoryElement>()).Returns(this.mockCategoryElementRepository.Object);

            await this.processor.CreateElementAsync(newCatogory).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the element asynchronous should throw exception if element  exists.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public async Task UpdateElementAsync_ShouldThrowExceptionIfCategoryExistFromRepository_WhenInvokedAsync()
        {
            var existingCategory = new CategoryElement
            {
                CategoryId = 1,
                Name = "segment",
            };

            var newCatogory = new CategoryElement
            {
                CategoryId = 2,
                Name = "segment",
            };

            this.mockCategoryElementRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(existingCategory);
            this.mockFactory.Setup(a => a.CreateRepository<CategoryElement>()).Returns(this.mockCategoryElementRepository.Object);

            await this.processor.UpdateElementAsync(newCatogory).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the element by name asynchronous should get category element by name from repository when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task UpdateCategoryElementAsync_ShouldUpdateCategoryElementAsync_WhenInvokedAsync()
        {
            var categoryElement = new CategoryElement();
            categoryElement.ElementId = 1;
            var token = new CancellationToken(false);
            var operationalSegment = new OperationalSegment();
            operationalSegment.ElementId = 1;

            var repoMock = new Mock<IRepository<CategoryElement>>();
            repoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<CategoryElement> { categoryElement });
            repoMock.Setup(r => r.UpdateAll(It.IsAny<IEnumerable<CategoryElement>>()));
            this.mockFactory.Setup(m => m.CreateRepository<CategoryElement>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<CategoryElement>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));

            await this.processor.UpdateOperationalSegmentsAsync(new List<OperationalSegment> { operationalSegment }).ConfigureAwait(false);

            repoMock.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>(), It.IsAny<string[]>()), Times.Exactly(2));
        }

        /// <summary>
        /// Updates deviation percentage asynchronous should update category element asynchronous when invoked asynchronous.
        /// </summary>
        /// /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task UpdateDeviationPercentageAsync_ShouldUpdateCategoryElementAsync_WhenInvokedAsync()
        {
            var categoryElement = new CategoryElement() { CategoryId = 2, DeviationPercentage = 1 };
            var categoryElementList = new[] { categoryElement };
            var token = new CancellationToken(false);
            var systemSettings = new SystemSettings() { MaxDeviationPercentage = 3.00M };

            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<SystemSettings>(It.IsAny<string>())).ReturnsAsync(systemSettings);
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<CategoryElement>()).Returns(this.mockCategoryElementRepository.Object);
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().SaveAsync(token));
            this.mockCategoryElementRepository.Setup(x => x.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(categoryElement);
            this.mockCategoryElementRepository.Setup(x => x.Update(It.IsAny<CategoryElement>()));
            this.mockFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(this.mockCategoryElementRepository.Object);

            await this.processor.UpdateDeviationPercentageAsync(categoryElementList).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(x => x.GetUnitOfWork().CreateRepository<CategoryElement>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(x => x.GetUnitOfWork().SaveAsync(token), Times.Once);
        }

        /// <summary>
        /// Updates deviation percentage asynchronous should throw exception if deviation percentage is invalid when invoked asynchronous.
        /// </summary>
        /// /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public async Task UpdateDeviationPercentageAsync_ShouldThrowExceptionIfDeviationPercentageIsInvalid_WhenInvokedAsync()
        {
            var categoryElement = new CategoryElement() { CategoryId = 2, DeviationPercentage = -1 };
            var categoryElementList = new[] { categoryElement };
            var token = new CancellationToken(false);
            var systemSettings = new SystemSettings() { MaxDeviationPercentage = 3.00M };

            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<SystemSettings>(It.IsAny<string>())).ReturnsAsync(systemSettings);
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<CategoryElement>()).Returns(this.mockCategoryElementRepository.Object);
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().SaveAsync(token));
            this.mockCategoryElementRepository.Setup(x => x.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(categoryElement);
            this.mockCategoryElementRepository.Setup(x => x.Update(It.IsAny<CategoryElement>()));
            this.mockFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(this.mockCategoryElementRepository.Object);

            await this.processor.UpdateDeviationPercentageAsync(categoryElementList).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates deviation percentage asynchronous should throw exception if greater than maximum allowed when invoked asynchronous.
        /// </summary>
        /// /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public async Task UpdateDeviationPercentageAsync_ShouldThrowExceptionIfGreaterThanMaximumAllowed_WhenInvokedAsync()
        {
            var categoryElement = new CategoryElement() { CategoryId = 2, DeviationPercentage = 10 };
            var categoryElementList = new[] { categoryElement };
            var token = new CancellationToken(false);
            var systemSettings = new SystemSettings() { MaxDeviationPercentage = 3.00M };

            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<SystemSettings>(It.IsAny<string>())).ReturnsAsync(systemSettings);
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<CategoryElement>()).Returns(this.mockCategoryElementRepository.Object);
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().SaveAsync(token));
            this.mockCategoryElementRepository.Setup(x => x.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(categoryElement);
            this.mockCategoryElementRepository.Setup(x => x.Update(It.IsAny<CategoryElement>()));
            this.mockFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(this.mockCategoryElementRepository.Object);

            await this.processor.UpdateDeviationPercentageAsync(categoryElementList).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates deviation percentage asynchronous should throw exception if get by id is null when invoked asynchronous.
        /// </summary>
        /// /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task UpdateDeviationPercentageAsync_ShouldThrowExceptionIfGetByIdIsNull_WhenInvokedAsync()
        {
            var categoryElement = new CategoryElement() { CategoryId = 2, DeviationPercentage = 1 };
            var categoryElementList = new[] { categoryElement };
            CategoryElement element = null;
            var token = new CancellationToken(false);
            var systemSettings = new SystemSettings() { MaxDeviationPercentage = 3.00M };

            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<SystemSettings>(It.IsAny<string>())).ReturnsAsync(systemSettings);
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().CreateRepository<CategoryElement>()).Returns(this.mockCategoryElementRepository.Object);
            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork().SaveAsync(token));
            this.mockCategoryElementRepository.Setup(x => x.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(element);
            this.mockCategoryElementRepository.Setup(x => x.Update(It.IsAny<CategoryElement>()));
            this.mockFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(this.mockCategoryElementRepository.Object);

            await this.processor.UpdateDeviationPercentageAsync(categoryElementList).ConfigureAwait(false);
        }
    }
}
