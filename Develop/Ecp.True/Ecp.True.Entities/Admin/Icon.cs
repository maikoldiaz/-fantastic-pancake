// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Icon.cs" company="Microsoft">
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
    /// The icon.
    /// </summary>
    public class Icon : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Icon"/> class.
        /// </summary>
        public Icon()
        {
            this.CategoryElements = new List<CategoryElement>();
        }

        /// <summary>
        /// Gets or sets the icon identifier.
        /// </summary>
        /// <value>
        /// The icon identifier.
        /// </value>
        public int IconId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public string Content { get; set; }

        /// <summary>
        /// Gets the category elements.
        /// </summary>
        /// <value>
        /// The category elements.
        /// </value>
        public virtual ICollection<CategoryElement> CategoryElements { get; private set; }
    }
}
