// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductEqualityComparer.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Balance.Calculation
{
    using System.Collections.Generic;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;

    /// <summary>
    /// The product equality comparer.
    /// </summary>
    public class ProductEqualityComparer : IEqualityComparer<Product>
    {
        /// <inheritdoc/>
        public bool Equals(Product x, Product y)
        {
            return x?.ProductId == y?.ProductId;
        }

        /// <inheritdoc/>
        public int GetHashCode(Product obj)
        {
            ArgumentValidators.ThrowIfNull(obj, nameof(obj));
            return string.IsNullOrEmpty(obj.ProductId) ? obj.GetHashCode() : obj.ProductId.GetHashCode(System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
