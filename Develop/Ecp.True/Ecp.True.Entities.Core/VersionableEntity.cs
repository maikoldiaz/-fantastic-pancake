// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersionableEntity.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Core
{
    using Ecp.True.Core;

    /// <summary>
    /// The Entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.IEntity" />
    public class VersionableEntity : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VersionableEntity"/> class.
        /// </summary>
        protected VersionableEntity()
        {
        }

        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        /// <value>
        /// The row version.
        /// </value>
        public byte[] RowVersion { get; set; }

        /// <summary>
        /// Gets the row version.
        /// </summary>
        /// <value>
        /// The version string.
        /// </value>
        public string Version
        {
            get
            {
                return this.RowVersion?.ToBase64();
            }
        }
    }
}