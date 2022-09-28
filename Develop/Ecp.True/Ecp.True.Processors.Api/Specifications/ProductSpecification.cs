// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductSpecification.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Specifications
{
    using System;
    using System.Linq.Expressions;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Specifications;

    /// <summary>
    /// Specifies a product by productId.
    /// </summary>
    public class ProductSpecification : CompositeSpecification<Product>
    {
        /// <summary>
        /// The product productId.
        /// </summary>
        private readonly string productId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductSpecification"/> class.
        /// </summary>
        /// <param name="productId">The product productId.</param>
        public ProductSpecification(string productId)
        {
            this.productId = productId;
        }

        /// <inheritdoc />
        public override Expression<Func<Product, bool>> ToExpression() => p => p.ProductId == this.productId;
    }
}