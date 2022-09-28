// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInventoryOwnershipService.cs" company="Microsoft">
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
    using OwnershipResultInventory = Ecp.True.Proxies.OwnershipRules.Response.OwnershipResultInventory;

    /// <summary>
    /// The IInventoryOwnershipService.
    /// </summary>
    public interface IInventoryOwnershipService
    {
        /// <summary>
        /// Gets the inventory ownerships.
        /// </summary>
        /// <param name="inventoryList">The inventory list.</param>
        /// <returns>The collection of InventoryOwnership.</returns>
        IEnumerable<Ownership> GetInventoryOwnerships(IEnumerable<OwnershipResultInventory> inventoryList);
    }
}