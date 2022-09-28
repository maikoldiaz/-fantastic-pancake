// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentEntity.cs" company="Microsoft">
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
    using Newtonsoft.Json;

    /// <summary>
    /// The document.
    /// </summary>
    /// <seealso cref="IDocumentEntity" />
    public class DocumentEntity : Entity, IDocumentEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentEntity"/> class.
        /// </summary>
        protected DocumentEntity()
        {
        }

        /// <summary>
        /// Gets or sets the identifier of the document.
        /// </summary>
        /// <value>
        /// The identifier of the document.
        /// </value>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the partition identifier.
        /// Every Entity should consider setting this to consider as Partitioned Entity.
        /// </summary>
        /// <value>
        /// The partition identifier.
        /// </value>
        public string PartitionId { get; set; }

        /// <summary>
        /// Gets or sets the time to live.
        /// </summary>
        /// <value>
        /// The time to live.
        /// </value>
        public int ttl { get; set; }
    }
}