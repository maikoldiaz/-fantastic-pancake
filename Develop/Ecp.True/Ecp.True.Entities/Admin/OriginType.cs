// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OriginType.cs" company="Microsoft">
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
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The Origin Type.
    /// </summary>
    public class OriginType : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OriginType"/> class.
        /// </summary>
        public OriginType()
        {
            this.SourceNodes = new List<Annulation>();
            this.DestinationNodes = new List<Annulation>();
            this.SourceProducts = new List<Annulation>();
            this.DestinationProducts = new List<Annulation>();
        }

        /// <summary>
        /// Gets or sets the origin type identifier.
        /// </summary>
        /// <value>
        /// The origin type identifier.
        /// </value>
        public int OriginTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the source node.
        /// </summary>
        /// <value>
        /// The source node.
        /// </value>
        public virtual ICollection<Annulation> SourceNodes { get; private set; }

        /// <summary>
        /// Gets the destination node.
        /// </summary>
        /// <value>
        /// The destination node.
        /// </value>
        public virtual ICollection<Annulation> DestinationNodes { get; private set; }

        /// <summary>
        /// Gets the source product.
        /// </summary>
        /// <value>
        /// The source product.
        /// </value>
        public virtual ICollection<Annulation> SourceProducts { get; private set; }

        /// <summary>
        /// Gets the destination product.
        /// </summary>
        /// <value>
        /// The destination product.
        /// </value>
        public virtual ICollection<Annulation> DestinationProducts { get; private set; }
    }
}
