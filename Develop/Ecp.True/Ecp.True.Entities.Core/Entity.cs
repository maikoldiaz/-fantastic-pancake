// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Entity.cs" company="Microsoft">
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
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// The Entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.IEntity" />
    public class Entity : IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        protected Entity()
        {
        }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        public virtual string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        public virtual DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the last modified by.
        /// </summary>
        /// <value>
        /// The last modified by.
        /// </value>
        [JsonIgnore]
        public virtual string LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>
        /// The last modified date.
        /// </value>
        [JsonIgnore]
        public virtual DateTime? LastModifiedDate { get; set; }

        /// <inheritdoc/>
        public virtual void CopyFrom(IEntity entity)
        {
            // do nothing.
        }
    }
}