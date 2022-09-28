// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReopenTicket.cs" company="Microsoft">
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
    /// <summary>
    /// The Origin.
    /// </summary>
    public class ReopenTicket
    {
        /// <summary>Gets or sets the ticket id.</summary>
        /// <value>The ticket id.</value>
        public int OwnershipNodeId { get; set; }

        /// <summary>Gets or sets the comment.</summary>
        /// <value>The comment.</value>
        public string Message { get; set; }

        /// <summary>Gets or sets the status.</summary>
        /// <value>The status.</value>
        public string Status { get; set; }
    }
}