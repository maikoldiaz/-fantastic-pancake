// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductWithoutMovementsTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Tests.Specifications
{
    using System.Collections.Generic;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Api.Specifications;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The productWithoutMovementsTests test class.
    /// </summary>
    [TestClass]
    public class ProductWithoutMovementsTests
    {
        /// <summary>
        /// The test product.
        /// </summary>
        private Product testProduct;

        [TestInitialize]
        public void Initialize()
        {
            this.testProduct = GetTestProduct();
        }

        /// <summary>
        /// ShouldNotBeSatisfiedWithWrongId.
        /// </summary>
        [TestMethod]
        public void ShouldNotBeSatisfiedWithWrongId()
        {
            // Prepare
            var spec = new ProductWithoutMovementsSpec("789456");

            // Execute
            var isSatisfied = spec.IsSatisfiedBy(this.testProduct);

            // Assert
            Assert.IsFalse(isSatisfied);
        }

        /// <summary>
        /// ShouldNotBeSatisfiedWithProductWithMovements.
        /// </summary>
        [TestMethod]
        public void ShouldNotBeSatisfiedWithProductWithSourceMovements()
        {
            // Prepare
            this.testProduct.Sources.Add(new MovementSource());

            var spec = new ProductWithoutMovementsSpec(this.testProduct.ProductId);

            // Execute
            var isSatisfied = spec.IsSatisfiedBy(this.testProduct);

            // Assert
            Assert.IsFalse(isSatisfied);
        }

        /// <summary>
        /// ShouldNotBeSatisfiedWithProductWithMovements.
        /// </summary>
        [TestMethod]
        public void ShouldNotBeSatisfiedWithProductWithDestinationMovements()
        {
            // Prepare
            this.testProduct.Destinations.Add(new MovementDestination());

            var spec = new ProductWithoutMovementsSpec(this.testProduct.ProductId);

            // Execute
            var isSatisfied = spec.IsSatisfiedBy(this.testProduct);

            // Assert
            Assert.IsFalse(isSatisfied);
        }

        /// <summary>
        /// Gets a test product.
        /// </summary>
        /// <returns>The product.</returns>
        private static Product GetTestProduct()
        {
            return new Product()
            {
                ProductId = "123456",
                Name = "Crudo Boyacá",
                IsActive = true,
            };
        }
    }
}