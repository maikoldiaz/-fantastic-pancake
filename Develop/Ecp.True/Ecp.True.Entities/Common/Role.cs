// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Role.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities
{
    using System.Collections.Generic;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The Role.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class Role : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        public Role()
        {
            this.FeatureRoles = new List<FeatureRole>();
        }

        /// <summary>
        /// Gets or sets the role identifier.
        /// </summary>
        /// <value>
        /// The role identifier.
        /// </value>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>
        /// The name of the role.
        /// </value>
        public string RoleName { get; set; }

        /// <summary>
        /// Gets the feature role.
        /// </summary>
        /// <value>
        /// The feature role.
        /// </value>
        public virtual ICollection<FeatureRole> FeatureRoles { get; }
    }
}
