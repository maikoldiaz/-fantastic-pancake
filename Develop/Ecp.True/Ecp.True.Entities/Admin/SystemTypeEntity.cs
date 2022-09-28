// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemTypeEntity.cs" company="Microsoft">
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
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The system type.
    /// </summary>
    public class SystemTypeEntity : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemTypeEntity"/> class.
        /// </summary>
        public SystemTypeEntity()
        {
            this.InventoryProducts = new List<InventoryProduct>();
            this.SourceHomologationSystems = new List<Homologation>();
            this.DestinationHomologationSystems = new List<Homologation>();
        }

        /// <summary>
        /// Gets or sets the system type identifier.
        /// </summary>
        /// <value>
        /// The system type identifier.
        /// </value>
        public int SystemTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets a value indicating whether filetype is contact or event.
        /// </summary>
        /// <value>
        /// The file type.
        /// </value>
        public bool IsFileType
        {
            get => (this.SystemTypeId == (int)SystemType.CONTRACT) || (this.SystemTypeId == (int)SystemType.EVENTS);
        }

        /// <summary>
        /// Gets the source homologation systems.
        /// </summary>
        /// <value>
        /// The source homologation systems.
        /// </value>
        public virtual ICollection<Homologation> SourceHomologationSystems { get; }

        /// <summary>
        /// Gets the destination homologation systems.
        /// </summary>
        /// <value>
        /// The destination homologation systems.
        /// </value>
        public virtual ICollection<Homologation> DestinationHomologationSystems { get; }

        /// <summary>
        /// Gets the inventory product.
        /// </summary>
        /// <value>
        /// The inventory product.
        /// </value>
        public virtual ICollection<InventoryProduct> InventoryProducts { get; }
    }
}
