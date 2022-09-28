// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FeatureRole.cs" company="Microsoft">
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
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The FeatureRole.
    /// </summary>
    /// <seealso cref="Entity" />
    public class FeatureRole : Entity
    {
        /// <summary>
        /// Gets or sets the feature role identifier.
        /// </summary>
        /// <value>
        /// The feature role identifier.
        /// </value>
        public int FeatureRoleId { get; set; }

        /// <summary>
        /// Gets or sets the role identifier.
        /// </summary>
        /// <value>
        /// The role identifier.
        /// </value>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the feature identifier.
        /// </summary>
        /// <value>
        /// The feature identifier.
        /// </value>
        public int FeatureId { get; set; }

        /// <summary>
        /// Gets or sets the feature.
        /// </summary>
        /// <value>
        /// The feature.
        /// </value>
        public virtual Feature Feature { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public virtual Role Role { get; set; }
    }
}
