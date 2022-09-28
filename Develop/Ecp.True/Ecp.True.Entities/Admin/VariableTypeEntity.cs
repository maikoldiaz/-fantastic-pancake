// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VariableTypeEntity.cs" company="Microsoft">
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

    /// <summary>
    /// VariableType Entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class VariableTypeEntity : Entity
    {
        /// <summary>
        /// Gets or sets the variable type identifier.
        /// </summary>
        /// <value>
        /// The variable type identifier.
        /// </value>
        public int VariableTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the short name.
        /// </summary>
        /// <value>
        /// The short name.
        /// </value>
        public string ShortName { get; set; }

        /// <summary>
        /// Gets or sets the short name.
        /// </summary>
        /// <value>
        /// The short name.
        /// </value>
        public string FicoName { get; set; }

        /// <summary>
        /// Gets or sets the is configurable.
        /// </summary>
        /// <value>
        /// The is configurable.
        /// </value>
        public bool? IsConfigurable { get; set; }

        /// <summary>
        /// Gets the storage location product variables.
        /// </summary>
        /// <value>
        /// The storage location product variables.
        /// </value>
        public ICollection<StorageLocationProductVariable> StorageLocationProductVariables { get; private set; }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal void Initialize()
        {
            this.StorageLocationProductVariables = new List<StorageLocationProductVariable>();
        }
    }
}
