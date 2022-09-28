// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Homologation.cs" company="Microsoft">
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
    using Ecp.True.Core.Attributes;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;

    /// <summary>
    ///     The homologation.
    /// </summary>
    public class Homologation : AuditableEntity
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Homologation" /> class.
        /// </summary>
        public Homologation()
        {
            this.HomologationGroups = new List<HomologationGroup>();
        }

        /// <summary>
        ///     Gets or sets the homologation identifier.
        /// </summary>
        /// <value>
        ///     The homologation identifier.
        /// </value>
        public int HomologationId { get; set; }

        /// <summary>
        ///     Gets or sets the source system identifier.
        /// </summary>
        /// <value>
        ///     The source system identifier.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.SourceSystemIdRequired)]
        public int? SourceSystemId { get; set; }

        /// <summary>
        ///     Gets or sets the destination system identifier.
        /// </summary>
        /// <value>
        ///     The destination system identifier.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.DestinationSystemIdRequired)]
        public int? DestinationSystemId { get; set; }

        /// <summary>
        ///     Gets the homologation groups.
        /// </summary>
        /// <value>
        ///     The homologation groups.
        /// </value>
        [MustNotBeEmpty(ErrorMessage = Entities.Constants.HomologationShouldHaveAtleastOneGroup)]
        public virtual ICollection<HomologationGroup> HomologationGroups { get; }

        /// <summary>
        ///     Gets or sets the Source system.
        /// </summary>
        /// <value>
        ///     The Source system.
        /// </value>
        public virtual SystemTypeEntity SourceSystem { get; set; }

        /// <summary>
        ///     Gets or sets the Destination system.
        /// </summary>
        /// <value>
        ///     The Destination system.
        /// </value>
        public virtual SystemTypeEntity DestinationSystem { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (this.SourceSystemId != null && this.DestinationSystemId != null)
            {
                var source = System.Enum.GetName(typeof(SystemType), this.SourceSystemId);
                var destination = System.Enum.GetName(typeof(SystemType), this.DestinationSystemId);
                return $"{source}_{destination}";
            }

            return string.Empty;
        }

        /// <summary>
        /// Copies from.
        /// </summary>
        /// <param name="Homologation">The homologation.</param>
        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var homologation = (Homologation)entity;
            this.SourceSystemId = homologation.SourceSystemId ?? this.SourceSystemId;
            this.DestinationSystemId = homologation.DestinationSystemId ?? this.DestinationSystemId;
            if (this.HomologationGroups != null)
            {
                this.HomologationGroups.Merge(homologation.HomologationGroups, o => o.HomologationGroupId);
            }
        }
    }
}