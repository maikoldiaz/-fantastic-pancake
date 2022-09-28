// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFailureHandler.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Interfaces
{
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Processors.Core.HandleFailure;

    /// <summary>
    /// IFailureHandler.
    /// </summary>
    public interface IFailureHandler
    {
        /// <summary>
        /// Gets the type of the ticket.
        /// </summary>
        /// <value>
        /// The type of the ticket.
        /// </value>
        TicketType TicketType { get; }

        /// <summary>
        /// HandleFailure the asynchronous.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="failureInfo">The failure info.</param>
        /// <returns>The Task.</returns>
        Task HandleFailureAsync(IUnitOfWork unitOfWork, FailureInfo failureInfo);
    }
}