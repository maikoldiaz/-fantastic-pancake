// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDocumentEntity.cs" company="Microsoft">
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
    /// <summary>
    /// Base interface for Document.
    /// </summary>
    public interface IDocumentEntity
    {
        /// <summary>
        /// Gets or sets the identifier of the document.
        /// </summary>
        /// <value>
        /// The identifier of the document.
        /// </value>
        string Id { get; set; }

        /// <summary>
        /// Gets or sets the partition identifier.
        /// Every Entity should consider setting this to consider as Partitioned Entity.
        /// </summary>
        /// <value>
        /// The partition identifier.
        /// </value>
        string PartitionId { get; set; }

        /// <summary>
        /// Gets or sets the time to live.
        /// </summary>
        /// <value>
        /// The time to live.
        /// </value>
        int ttl { get; set; }
    }
}