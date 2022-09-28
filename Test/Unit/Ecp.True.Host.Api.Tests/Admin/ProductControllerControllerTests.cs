// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductControllerControllerTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Tests.Admin
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Api.Controllers;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The ProductController test class.
    /// </summary>
    [TestClass]
    public class ProductControllerControllerTests
    {
        /// <summary>
        /// The ProductProcessor mock.
        /// </summary>
        private Mock<IProductProcessor> productProcessorMock;

        /// <summary>
        /// The product controller.
        /// </summary>
        private ProductController productController;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.productProcessorMock = new Mock<IProductProcessor>();
            var loggerFactoryMock = InitializeLoggerFactoryMock();

            this.productController = new ProductController(this.productProcessorMock.Object, loggerFactoryMock.Object);
        }

        /// <summary>
        /// Create_ShouldInvokeTheProcessorAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Create_ShouldInvokeTheProcessorAsync()
        {
            // Prepare
            var product = GetTestProduct();

            this.productProcessorMock
                .Setup(p => p.CreateProductAsync(product))
                .ReturnsAsync(new ProductInfo(product, null));

            // Execute
            await this.productController.CreateProductAsync(product).ConfigureAwait(false);

            // Assert
            this.productProcessorMock.Verify(p => p.CreateProductAsync(product), Times.Once);
        }

        /// <summary>
        /// CreateShouldReturnEntityResultIfProductInfoHasNoError.
        /// </summary>
        /// <param name="error">The error message if any.</param>
        /// <param name="resultType">The expected result type.</param>
        /// <returns>The task.</returns>
        [TestMethod]
        [DataRow("", typeof(EntityResult))]
        public async Task CreateShouldReturnEntityResultIfProductInfoHasNoErrorAsync(string error, Type resultType)
        {
            // Prepare
            var product = GetTestProduct();

            var info = new ProductInfo(product, null) { ErrorMessage = error };

            this.productProcessorMock
                .Setup(p => p.CreateProductAsync(product))
                .ReturnsAsync(info);

            // Act
            var result = await this.productController.CreateProductAsync(product).ConfigureAwait(false);

            // Assert
            Assert.IsInstanceOfType(result, resultType);
        }

        /// <summary>
        /// Update_ShouldInvokeTheProcessorAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Update_ShouldInvokeTheProcessorAsync()
        {
            // Prepare
            var product = GetTestProduct();

            this.productProcessorMock
                .Setup(p => p.UpdateProductAsync(product, product.ProductId))
                .Returns(Task.CompletedTask);

            // Execute
            await this.productController.UpdateProductAsync(product, product.ProductId).ConfigureAwait(false);

            // Assert
            this.productProcessorMock.Verify(p => p.UpdateProductAsync(product, product.ProductId), Times.Once);
        }

        /// <summary>
        /// Delete_ShouldInvokeTheProcessorAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Delete_ShouldInvokeTheProcessorAsync()
        {
            // Arrange
            var product = GetTestProduct();

            this.productProcessorMock
                .Setup(p => p.DeleteProductAsync(product.ProductId))
                .Returns(Task.CompletedTask);

            // Execute
            await this.productController.DeleteProductAsync(product.ProductId).ConfigureAwait(false);

            // Assert
            this.productProcessorMock.Verify(p => p.DeleteProductAsync(product.ProductId), Times.Once);
        }

        /// <summary>
        /// Get_ShouldInvokeTheProcessorAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Get_ShouldInvokeTheProcessorAsync()
        {
            // Prepare
            var product = GetTestProduct();

            this.productProcessorMock
                .Setup(p => p.GetProductAsync(product.ProductId))
                .ReturnsAsync(new ProductInfo(product, true));

            // Execute
            await this.productController.GetProductAsync(product.ProductId).ConfigureAwait(false);

            // Assert
            this.productProcessorMock.Verify(p => p.GetProductAsync(product.ProductId), Times.Once);
        }

        /// <summary>
        /// Initializes the logger factory.
        /// </summary>
        /// <returns>The logger mock.</returns>
        private static Mock<ILoggerFactory> InitializeLoggerFactoryMock()
        {
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            var logger = new Mock<ILogger<ProductController>>();
            logger.Setup(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
            loggerFactoryMock.Setup(l => l.CreateLogger(It.IsAny<string>()))
                .Returns(logger.Object);
            return loggerFactoryMock;
        }

        /// <summary>
        /// Gets the test product.
        /// </summary>
        /// <returns>The product.</returns>
        private static Product GetTestProduct()
        {
            var product = new Product
            {
                ProductId = "1",
                IsActive = true,
                Name = "Crudo Boyacá",
            };
            return product;
        }
    }
}