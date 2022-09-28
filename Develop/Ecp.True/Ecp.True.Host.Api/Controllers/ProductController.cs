// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductController.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The product controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Admin")]
    public class ProductController
    {
        /// <summary>
        /// The product processor.
        /// </summary>
        private readonly IProductProcessor processor;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<ProductController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="processor">The product processor.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public ProductController(IProductProcessor processor, ILoggerFactory loggerFactory)
        {
            ArgumentValidators.ThrowIfNull(loggerFactory, nameof(loggerFactory));

            this.logger = loggerFactory.CreateLogger<ProductController>();
            this.processor = processor;
        }

        /// <summary>
        /// Creates a new product async.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns>The task.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/products")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> CreateProductAsync(Product product)
        {
            var info = await this.processor.CreateProductAsync(product).ConfigureAwait(false);
            return new EntityResult(info);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="product">The product to update.</param>
        /// <param name="productId">The product id.</param>
        /// <returns>The task.</returns>
        [HttpPut]
        [Route("api/v{version:apiVersion}/products/{productId}")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> UpdateProductAsync(Product product, string productId)
        {
            await this.processor.UpdateProductAsync(product, productId).ConfigureAwait(false);

            return new EntityResult(Entities.Constants.ProductUpdatedSuccessfully);
        }

        /// <summary>
        /// Gets the product.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <returns>The task.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/products/{productId}")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> GetProductAsync(string productId)
        {
            var product = await this.processor.GetProductAsync(productId).ConfigureAwait(false);

            return new EntityResult(product);
        }

        /// <summary>
        /// Deletes a product.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <returns>The task.</returns>
        [HttpDelete]
        [Route("api/v{version:apiVersion}/products/{productId}")]
        [TrueAuthorize(Role.Administrator)]
        public async Task DeleteProductAsync(string productId)
        {
            this.logger.LogInformation(productId);
            await this.processor.DeleteProductAsync(productId).ConfigureAwait(false);
        }
    }
}