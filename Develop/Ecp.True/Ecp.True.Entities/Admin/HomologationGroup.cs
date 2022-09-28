// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologationGroup.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Entities.Core;

    /// <summary>
    ///     The homologation group.
    /// </summary>
    public class HomologationGroup : EditableEntity
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HomologationGroup" /> class.
        /// </summary>
        public HomologationGroup()
        {
            this.HomologationObjects = new List<HomologationObject>();
            this.HomologationDataMapping = new List<HomologationDataMapping>();
        }

        /// <summary>
        ///     Gets or sets the homologation group identifier.
        /// </summary>
        /// <value>
        ///     The homologation group identifier.
        /// </value>
        public int HomologationGroupId { get; set; }

        /// <summary>
        ///     Gets or sets the group type identifier.
        /// </summary>
        /// <value>
        ///     The group type identifier.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.GroupTypeIdRequired)]
        public int? GroupTypeId { get; set; }

        /// <summary>
        ///     Gets or sets the homologation identifier.
        /// </summary>
        /// <value>
        ///     The homologation identifier.
        /// </value>
        public int HomologationId { get; set; }

        /// <summary>
        /// Gets or sets the homologation.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        [ColumnIgnore]
        public virtual Homologation Homologation { get; set; }

        /// <summary>
        ///     Gets the homologation object.
        /// </summary>
        /// <value>
        ///     The homologation object.
        /// </value>
        [MustNotBeEmpty(ErrorMessage = Entities.Constants.HomologationShouldHaveAtleastOneHomologationObjects)]
        public virtual ICollection<HomologationObject> HomologationObjects { get; }

        /// <summary>
        ///     Gets the homologation data mappings.
        /// </summary>
        /// <value>
        ///     The homologation data mappings.
        /// </value>
        [MustNotBeEmpty(ErrorMessage = Entities.Constants.HomologationShouldHaveAtleastOneDataMapping)]
        public virtual ICollection<HomologationDataMapping> HomologationDataMapping { get; }

        /// <summary>
        /// Gets or sets the homologation group.
        /// </summary>
        /// <value>
        ///     The homologation Group.
        /// </value>
        public virtual Category Group { get; set; }

        /// <summary>
        /// Copies from.
        /// </summary>
        /// <param name="HomologationGroup">The homologation group.</param>
        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var homologationGroup = (HomologationGroup)entity;
            this.GroupTypeId = homologationGroup.GroupTypeId;
            this.RowVersion = homologationGroup.RowVersion;
            if (this.HomologationDataMapping != null)
            {
                this.HomologationDataMapping.Merge(homologationGroup.HomologationDataMapping, x => x.HomologationDataMappingId);
            }

            if (this.HomologationObjects != null)
            {
                this.HomologationObjects.Merge(homologationGroup.HomologationObjects, x => x.HomologationObjectId);
            }
        }
    }
}
