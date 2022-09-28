// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEntity.cs" company="Microsoft">
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

    /// <summary>
    /// The base entity interface.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>
        /// The last modified date.
        /// </value>
        DateTime? LastModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the last modified by.
        /// </summary>
        /// <value>
        /// The last modified by.
        /// </value>
        string LastModifiedBy { get; set; }

        /// <summary>
        /// Copies data from another entity.
        /// </summary>
        /// <param name="entity">The entity to copy from.</param>
        void CopyFrom(IEntity entity);
    }
}