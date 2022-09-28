// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductSpecificationTests.cs" company="Microsoft">
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
    using Ecp.True.Entities.Admin;
    using Ecp.True.Processors.Api.Specifications;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The productSpecificationTests test class.
    /// </summary>
    [TestClass]
    public class ProductSpecificationTests
    {
        /// <summary>
        /// The sapec.
        /// </summary>
        private ProductSpecification spec;

        /// <summary>
        /// The test product.
        /// </summary>
        private Product product;

        /// <summary>
        /// ShouldBeSatisfiedWithEqualId.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <param name="candidateProductId">The candidate product id.</param>
        /// <param name="satisfied">Whether it should be satisfied.</param>
        [DataTestMethod]
        [DataRow("123456", "123456", true)]
        [DataRow("123456", "999999", false)]
        public void ShouldBeSatisfiedWithEqualId(string productId, string candidateProductId, bool satisfied)
        {
            // Prepare
            this.product = CreateProduct(candidateProductId);
            this.spec = new ProductSpecification(productId);

            // Execute
            var isSatisfied = this.spec.IsSatisfiedBy(this.product);

            // Assert
            Assert.AreEqual(satisfied, isSatisfied);
        }

        /// <summary>
        /// Creates a test product.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <returns>The product.</returns>
        private static Product CreateProduct(string productId)
        {
            return new Product
            {
                ProductId = productId,
                Name = "Crudo Boyacá",
                IsActive = true,
            };
        }
    }
}