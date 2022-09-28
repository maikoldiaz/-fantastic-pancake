// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagedResult.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.UI.Models
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The paged result.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    [DataContract]
    public class PagedResult<TEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedResult{TEntity}"/> class.
        /// </summary>
        public PagedResult()
        {
            this.Value = new List<TEntity>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedResult{TEntity}"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="count">The count.</param>
        public PagedResult(IEnumerable<TEntity> items, int? count)
        {
            this.Value = items;
            this.TotalResultCount = count.GetValueOrDefault();
        }

        /// <summary>
        /// Gets the total result count.
        /// </summary>
        /// <value>
        /// Gets the total result count for Web view (JSX) rendering.
        /// </value>
        [DataMember(Name = "count")]
        public int Count => this.TotalResultCount;

        /// <summary>
        /// Gets or sets the OData count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        [DataMember(Name = "@odata.count")]
        public int TotalResultCount { get; set; }

        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        [DataMember(Name = "value")]
        public IEnumerable<TEntity> Value { get; set; }
    }
}