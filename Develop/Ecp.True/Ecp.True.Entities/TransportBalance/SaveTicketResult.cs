// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveTicketResult.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.TransportBalance
{
    using Ecp.True.Entities.Core;

    /// <summary>
    /// Ticket Entity.
    /// </summary>
    public class SaveTicketResult : Entity
    {
        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        public int TicketId { get; set; }

        /// <summary>
        /// Gets or sets the category element identifier.
        /// </summary>
        /// <value>
        /// The category element identifier.
        /// </value>
        public int? CategoryElementId { get; set; }
    }
}
