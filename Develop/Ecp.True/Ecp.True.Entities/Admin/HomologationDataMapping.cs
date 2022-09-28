// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologationDataMapping.cs" company="Microsoft">
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
    ///     The homologation mapping.
    /// </summary>
    public class HomologationDataMapping : AuditableEntity
    {
        /// <summary>
        ///     Gets or sets the homologation identifier.
        /// </summary>
        /// <value>
        ///     The homologation identifier.
        /// </value>
        public int HomologationDataMappingId { get; set; }

        /// <summary>
        ///     Gets or sets the name of the source value.
        /// </summary>
        /// <value>
        ///     The name of the source value.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.SourceValueRequired)]
        [MaxLength(100, ErrorMessage = Entities.Constants.SourceValueMaxLength100)]
        public string SourceValue { get; set; }

        /// <summary>
        ///     Gets or sets the name of the destination value.
        /// </summary>
        /// <value>
        ///     The name of the destination value.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.DestinationValueRequired)]
        [MaxLength(100, ErrorMessage = Entities.Constants.DestinationValueMaxLength100)]
        public string DestinationValue { get; set; }

        /// <summary>
        ///     Gets or sets the homologation group identifier.
        /// </summary>
        /// <value>
        ///     The homologation group identifier.
        /// </value>
        public int HomologationGroupId { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the homologation group.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        [ColumnIgnore]
        public virtual HomologationGroup HomologationGroup { get; set; }

        /// <summary>
        /// Copies from.
        /// </summary>
        /// <param name="HomologationDataMapping">The homologation data mapping.</param>
        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var homologationDataMapping = (HomologationDataMapping)entity;
            this.SourceValue = homologationDataMapping.SourceValue ?? this.SourceValue;
            this.DestinationValue = homologationDataMapping.DestinationValue ?? this.DestinationValue;
        }
    }
}
