// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologationObject.cs" company="Microsoft">
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
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;

    /// <summary>
    ///     The homologation object.
    /// </summary>
    public class HomologationObject : AuditableEntity
    {
        /// <summary>
        ///     Gets or sets the homologation object identifier.
        /// </summary>
        /// <value>
        ///     The homologation identifier.
        /// </value>
        public int HomologationObjectId { get; set; }

        /// <summary>
        /// Gets or sets the homologation object type identifier.
        /// </summary>
        /// <value>
        /// The homologation object type identifier.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.HomologationObjectTypeIdRequired)]
        public int? HomologationObjectTypeId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether gets or sets the IsRequiredMapping.
        /// </summary>
        /// <value>
        ///     The IsRequiredMapping.
        /// </value>
        [Required]
        public bool IsRequiredMapping { get; set; } = true;

        /// <summary>
        ///     Gets or sets the homologation group identifier.
        /// </summary>
        /// <value>
        ///     The homologation group identifier.
        /// </value>
        public int HomologationGroupId { get; set; }

        /// <summary>
        /// Gets or sets the homologation group.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        [ColumnIgnore]
        public virtual HomologationGroup HomologationGroup { get; set; }

        /// <summary>
        /// Gets or sets the homologation object.
        /// </summary>
        /// <value>
        /// The homologation object.
        /// </value>
        [ColumnIgnore]
        public virtual HomologationObjectType HomologationObjectType { get; set; }

        /// <summary>
        /// Copies from.
        /// </summary>
        /// <param name="HomologationObjects">The homologation objects.</param>
        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var homologationObject = (HomologationObject)entity;
            this.IsRequiredMapping = homologationObject.IsRequiredMapping;
            this.HomologationObjectTypeId = homologationObject.HomologationObjectTypeId ?? this.HomologationObjectTypeId;
        }
    }
}
