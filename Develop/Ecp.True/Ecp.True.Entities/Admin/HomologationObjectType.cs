// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologationObjectType.cs" company="Microsoft">
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
    /// The homologation object type.
    /// </summary>
    /// <seealso cref="Entity" />
    public class HomologationObjectType : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomologationObjectType"/> class.
        /// </summary>
        public HomologationObjectType()
        {
            this.HomologationObjects = new List<HomologationObject>();
        }

        /// <summary>
        /// Gets or sets the homologation object type identifier.
        /// </summary>
        /// <value>
        /// The homologation object type identifier.
        /// </value>
        public int HomologationObjectTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the homologation objects.
        /// </summary>
        /// <value>
        /// The homologation objects.
        /// </value>
        public virtual ICollection<HomologationObject> HomologationObjects { get; }
    }
}
