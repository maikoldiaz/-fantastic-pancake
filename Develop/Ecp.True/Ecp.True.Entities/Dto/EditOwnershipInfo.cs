// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditOwnershipInfo.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Dto
{
    /// <summary>
    /// The Edit Ownership Info.
    /// </summary>
    /// <typeparam name="T">The  type.</typeparam>
    public class EditOwnershipInfo<T>
    {
        /// <summary>
        /// Gets or sets the movement ownerships.
        /// </summary>
        /// <value>
        /// The movement ownerships.
        /// </value>
        public T Ownership { get; set; }

        /// <summary>
        /// Gets or sets the reason for change.
        /// </summary>
        /// <value>
        /// The reason for change.
        /// </value>
        public int ReasonId { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>
        /// The comment.
        /// </value>
        public string Comment { get; set; }
    }
}
