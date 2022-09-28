// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMovementOwnershipService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Interfaces
{
    using System.Collections.Generic;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Proxies.OwnershipRules.Response;

    /// <summary>
    /// The IMovementOwnershipService.
    /// </summary>
    public interface IMovementOwnershipService
    {
        /// <summary>
        /// Gets the movement ownerships.
        /// </summary>
        /// <param name="movementList">The movement list.</param>
        /// <returns>The collection of MovementOwnership.</returns>
        IEnumerable<Ownership> GetMovementOwnerships(IEnumerable<OwnershipResultMovement> movementList);
    }
}