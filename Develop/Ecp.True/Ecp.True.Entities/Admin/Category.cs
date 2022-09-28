// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Category.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;
    using Newtonsoft.Json;

    /// <summary>
    /// The category.
    /// </summary>
    public class Category : EditableEntity
    {
        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.CategoryNameRequired)]
        [StringLength(150, ErrorMessage = Entities.Constants.NameMaxLength150)]
        [RegularExpression(Entities.Constants.AllowNumbersAndLettersWithSpecialCharactersWithSpaceRegex, ErrorMessage = Entities.Constants.AllowAlphanumericWithSpecialCharactersAndSpaceMessage)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the category description.
        /// </summary>
        /// <value>
        /// The category description.
        /// </value>
        [StringLength(1000, ErrorMessage = Entities.Constants.DescriptionMaxLength1000)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.CategoryStatusIsRequired)]
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is grouper.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is grouper; otherwise, <c>false</c>.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.CategoryGrouperIsRequired)]
        public bool? IsGrouper { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is homologation.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is homologation; otherwise, <c>false</c>.
        /// </value>
        public bool IsHomologation { get; set; }

        /// <summary>
        /// Gets the Homologation Groups.
        /// </summary>
        /// <value>
        ///     The Homologation Groups.
        /// </value>
        public virtual ICollection<HomologationGroup> Groups { get; private set; }

        /// <summary>
        /// Gets the elements.
        /// </summary>
        /// <value>
        /// The elements.
        /// </value>
        [JsonIgnore]
        [JsonProperty]
        public virtual ICollection<CategoryElement> Elements { get; private set; }

        /// <summary>
        /// Gets the report executions.
        /// </summary>
        /// <value>
        /// The report executions.
        /// </value>
        public virtual ICollection<ReportExecution> ReportExecutions { get; private set; }

        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));

            var category = (Category)entity;
            this.Name = category.Name ?? this.Name;
            this.Description = category.Description ?? this.Description;
            this.IsGrouper = category.IsGrouper ?? this.IsGrouper;
            this.IsActive = category.IsActive ?? this.IsActive;
            this.RowVersion = category.RowVersion;

            if (this.Elements != null)
            {
                this.Elements.Merge(category.Elements, e => e.ElementId);
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal void Initialize()
        {
            this.Elements = new List<CategoryElement>();
            this.Groups = new List<HomologationGroup>();
            this.ReportExecutions = new List<ReportExecution>();
        }
    }
}
