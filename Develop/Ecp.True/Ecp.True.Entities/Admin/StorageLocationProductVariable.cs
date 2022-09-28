// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageLocationProductVariable.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Admin
{
    using Ecp.True.Entities.Core;

    /// <summary>
    ///     The StorageLocationProductVariable.
    /// </summary>
    public class StorageLocationProductVariable : EditableEntity
    {
        /// <summary>
        /// Gets or sets the storage location product variable identifier.
        /// </summary>
        /// <value>
        /// The storage location product variable identifier.
        /// </value>
        public int StorageLocationProductVariableId { get; set; }

        /// <summary>
        /// Gets or sets the storage location product identifier.
        /// </summary>
        /// <value>
        /// The storage location product identifier.
        /// </value>
        public int StorageLocationProductId { get; set; }

        /// <summary>
        /// Gets or sets the variable identifier.
        /// </summary>
        /// <value>
        /// The variable identifier.
        /// </value>
        public int VariableTypeId { get; set; }

        /// <summary>
        /// Gets or sets the storage location product.
        /// </summary>
        /// <value>
        /// The storage location product.
        /// </value>
        public virtual StorageLocationProduct StorageLocationProduct { get; set; }

        /// <summary>
        /// Gets or sets the variable type.
        /// </summary>
        /// <value>
        /// The variable type.
        /// </value>
        public virtual VariableTypeEntity VariableType { get; set; }
    }
}