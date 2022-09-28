// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartnerOwnerMapping.cs" company="Microsoft">
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
    /// The PartnerOwnerMapping.
    /// </summary>
    public class PartnerOwnerMapping : Entity
    {
        /// <summary>
        /// Gets or sets the partner owner mapping identifier.
        /// </summary>
        /// <value>
        /// The partner owner mapping identifier.
        /// </value>
        public int PartnerOwnerMappingId { get; set; }

        /// <summary>
        /// Gets or sets the grand owner identifier.
        /// </summary>
        /// <value>
        /// The grand owner identifier.
        /// </value>
        public int GrandOwnerId { get; set; }

        /// <summary>
        /// Gets or sets the partner owner identifier.
        /// </summary>
        /// <value>
        /// The partner owner identifier.
        /// </value>
        public int PartnerOwnerId { get; set; }

        /// <summary>
        /// Gets or sets the grand owner.
        /// </summary>
        /// <value>
        /// The grand owner.
        /// </value>
        public virtual CategoryElement GrandOwner { get; set; }

        /// <summary>
        /// Gets or sets the partner owner.
        /// </summary>
        /// <value>
        /// The partner owner.
        /// </value>
        public virtual CategoryElement PartnerOwner { get; set; }
    }
}
