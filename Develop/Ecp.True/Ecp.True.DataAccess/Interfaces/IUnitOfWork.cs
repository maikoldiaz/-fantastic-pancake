// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUnitOfWork.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Interfaces
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The unit of work.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the movement repository.
        /// </summary>
        /// <value>
        /// The movement repository.
        /// </value>
        IMovementRepository MovementRepository { get; }

        /// <summary>
        /// Gets the inventory product repository.
        /// </summary>
        /// <value>
        /// The inventory product repository.
        /// </value>
        IInventoryProductRepository InventoryProductRepository { get; }

        /// <summary>
        /// Gets the ticketInfoRepository .
        /// </summary>
        /// <value>
        /// ticketInfoRepository repository.
        /// </value>
        ITicketInfoRepository TicketInfoRepository { get; }

        /// <summary>
        /// Creates the repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>
        /// The repository.
        /// </returns>
        IRepository<TEntity> CreateRepository<TEntity>()
            where TEntity : class, IEntity;

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>1, when saved successfully, 0 otherwise.</returns>
        Task<int> SaveAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Clears this instance.
        /// </summary>
        void Clear();
    }
}