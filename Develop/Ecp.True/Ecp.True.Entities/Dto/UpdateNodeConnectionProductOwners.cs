// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateNodeConnectionProductOwners.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Dto
{
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The UpdateStorageLocationProductOwners Dto.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.VersionableEntity" />
    public class UpdateNodeConnectionProductOwners : IVersionable
    {
        /// <summary>
        /// Gets or sets the owners.
        /// </summary>
        /// <value>
        /// The owners.
        /// </value>
        public NodeConnectionProductOwner[] Owners { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        /// <value>
        /// The row version.
        /// </value>
        public byte[] RowVersion { get; set; }
    }
}
