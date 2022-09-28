// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageLocationProductMapping.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Admin
{
    using System;
    using System.Collections.Generic;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The StorageLocationProductMapping.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class StorageLocationProductMapping : EditableEntity, IEqualityComparer<StorageLocationProductMapping>
    {
        /// <summary>
        /// Gets or sets the storage location product mapping identifier.
        /// </summary>
        /// <value>
        /// The storage location product mapping identifier.
        /// </value>
        public int StorageLocationProductMappingId { get; set; }

        /// <summary>
        /// Gets or sets the storage location identifier.
        /// </summary>
        /// <value>
        /// The storage location identifier.
        /// </value>
        public string StorageLocationId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        public virtual Product Product { get; set; }

        /// <summary>
        /// Gets or sets the storage location.
        /// </summary>
        /// <value>
        /// The storage location.
        /// </value>
        public virtual StorageLocation StorageLocation { get; set; }

        /// <inheritdoc />
        public bool Equals(StorageLocationProductMapping x, StorageLocationProductMapping y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null))
            {
                return false;
            }

            if (ReferenceEquals(y, null))
            {
                return false;
            }

            if (x.GetType() != y.GetType())
            {
                return false;
            }

            return x.StorageLocationId == y.StorageLocationId && x.ProductId == y.ProductId;
        }

        /// <inheritdoc />
        public int GetHashCode(StorageLocationProductMapping obj)
        {
            ArgumentValidators.ThrowIfNull(obj, nameof(obj));

            return HashCode.Combine(obj.StorageLocationId, obj.ProductId);
        }
    }
}
