// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRepositoryFactory.cs" company="Microsoft">
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
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The repository factory.
    /// </summary>
    public interface IRepositoryFactory
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
        /// Gets the homologation repository.
        /// </summary>
        /// <value>
        /// The homologation repository.
        /// </value>
        IHomologationRepository HomologationRepository { get; }

        /// <summary>
        /// Gets the ticketchartinfo repository.
        /// </summary>
        /// <value>
        /// The ticketchartinfo repository.
        /// </value>
        ITicketInfoRepository TicketInfoRepository { get; }

        /// <summary>
        /// Gets the node repository.
        /// </summary>
        /// <value>
        /// The node repository.
        /// </value>
        INodeRepository NodeRepository { get; }

        /// <summary>
        /// Gets the node ownership repository.
        /// </summary>
        /// <value>
        /// The node ownership repository.
        /// </value>
        INodeOwnershipRepository NodeOwnershipRepository { get; }

        /// <summary>
        /// Creates the repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>
        /// The repository.
        /// </returns>
        IRepository<TEntity> CreateRepository<TEntity>()
            where TEntity : class, IEntity;
    }
}