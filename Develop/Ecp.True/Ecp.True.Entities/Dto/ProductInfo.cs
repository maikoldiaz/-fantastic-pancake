// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductInfo.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Dto
{
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;

    /// <summary>
    /// The product info.
    /// </summary>
    public class ProductInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductInfo"/> class.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <param name="isEditable">Whether the product has movements.</param>
        public ProductInfo(Product product, bool? isEditable)
        {
            ArgumentValidators.ThrowIfNull(product, nameof(product));

            this.ProductId = product.ProductId;
            this.Name = product.Name;
            this.IsActive = product.IsActive;
            this.IsEditable = isEditable;
        }

        /// <summary>
        /// Gets the product id.
        /// </summary>
        public string ProductId { get; }

        /// <summary>
        /// Gets the product name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a value indicating whether the product is active.
        /// </summary>
        public bool IsActive { get; }

        /// <summary>
        /// Gets a value indicating whether the product has movements.
        /// </summary>
        public bool? IsEditable { get; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets a value indicating whether the product has an error.
        /// </summary>
        public bool HasError => !string.IsNullOrEmpty(this.ErrorMessage);
    }
}