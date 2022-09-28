// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductWithoutMovementsSpec.cs" company="Microsoft">
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
    using Microsoft.EntityFrameworkCore.Internal;

    /// <summary>
    /// The productWithoutMovement specification.
    /// </summary>
    public class ProductWithoutMovementsSpec : ProductSpecification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductWithoutMovementsSpec"/> class.
        /// </summary>
        /// <param name="productId">The product id.</param>
        public ProductWithoutMovementsSpec(string productId)
            : base(productId)
        {
        }

        /// <inheritdoc />
        public override Expression<Func<Product, bool>> ToExpression()
        {
            Expression<Func<Product, bool>> noMovements = p => !p.Sources.Any() && !p.Destinations.Any();

            return noMovements.AndAlso(base.ToExpression());
        }
    }
}