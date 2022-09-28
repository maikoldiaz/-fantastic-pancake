// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeEntity.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Entities.Registration
{
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The Inventory Attribute class.
    /// </summary>
    public class AttributeEntity : Entity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the attribute identifier.
        /// </summary>
        /// <value>
        /// The attribute identifier.
        /// </value>
        [Required(ErrorMessage = Constants.AttributeIdRequired)]
        public int AttributeId { get; set; }

        /// <summary>
        /// Gets or sets the attribute value.
        /// </summary>
        /// <value>
        /// The attribute value.
        /// </value>
        [Required(ErrorMessage = Constants.AttributeValueRequired)]
        public string AttributeValue { get; set; }

        /// <summary>
        /// Gets or sets the value attribute unit.
        /// </summary>
        /// <value>
        /// The value attribute unit.
        /// </value>
        [Required(ErrorMessage = Constants.ValueAttributeUnitRequired)]
        public int ValueAttributeUnit { get; set; }

        /// <summary>
        /// Gets or sets the attribute description.
        /// </summary>
        /// <value>
        /// The attribute description.
        /// </value>
        [StringLength(150, ErrorMessage = Constants.AttributeDescriptionRequired)]
        public string AttributeDescription { get; set; }

        /// <summary>
        /// Gets or sets the inventory product identifier.
        /// </summary>
        /// <value>
        /// The inventory product identifier.
        /// </value>
        public int? InventoryProductId { get; set; }

        /// <summary>
        /// Gets or sets the movement transaction identifier.
        /// </summary>
        /// <value>
        /// The movement transaction identifier.
        /// </value>
        public int? MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the type of the attribute.
        /// </summary>
        /// <value>
        /// The type of the attribute.
        /// </value>
        public string AttributeType { get; set; }

        /// <summary>
        /// Gets or sets the inventory product.
        /// </summary>
        /// <value>
        /// The inventory product.
        /// </value>
        public virtual InventoryProduct InventoryProduct { get; set; }

        /// <summary>
        /// Gets or sets the movement transaction.
        /// </summary>
        /// <value>
        /// The movement transaction.
        /// </value>
        public virtual Movement MovementTransaction { get; set; }

        /// <summary>
        /// Gets or sets the attribute category element.
        /// </summary>
        /// <value>
        /// The attribute category element.
        /// </value>
        public virtual CategoryElement Attribute { get; set; }

        /// <summary>
        /// Gets or sets the value attribute unit category element.
        /// </summary>
        /// <value>
        /// The value attribute unit category element.
        /// </value>
        public virtual CategoryElement ValueAttributeUnitElement { get; set; }

        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            True.Core.ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var element = (AttributeEntity)entity;

            this.AttributeId = element.AttributeId;
            this.AttributeValue = element.AttributeValue;
            this.ValueAttributeUnit = element.ValueAttributeUnit;
            this.AttributeDescription = element.AttributeDescription;
            this.AttributeType = element.AttributeType;
        }
    }
}