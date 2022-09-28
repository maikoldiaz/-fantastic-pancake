// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProcessor.cs" company="Microsoft">
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
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The processor contract.
    /// </summary>
    public interface IProcessor
    {
        /// <summary>
        /// Queries all asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>
        /// The entity query.
        /// </returns>
        Task<IQueryable<TEntity>> QueryAllAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, params string[] includeProperties)
            where TEntity : class, IEntity;

        /// <summary>
        /// Queries the view asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>The View Query.</returns>
        Task<IQueryable<TEntity>> QueryViewAsync<TEntity>()
           where TEntity : class, IEntity;
    }
}
