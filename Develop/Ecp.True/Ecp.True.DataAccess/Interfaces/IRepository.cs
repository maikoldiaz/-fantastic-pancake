﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRepository.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Specifications;

    /// <summary>
    /// The repository type.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IRepository<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Insert(TEntity entity);

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void InsertAll(IEnumerable<TEntity> entities);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void UpdateAll(IEnumerable<TEntity> entities);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Upsert the entities.
        /// </summary>
        /// <param name="existing">The true entities.</param>
        /// <param name="newEntities">The new entities.</param>
        /// <param name="predicate">The predicate.</param>
        void Upsert(IEnumerable<TEntity> existing, IEnumerable<TEntity> newEntities, Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void DeleteAll(IEnumerable<TEntity> entities);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The identifier asynchronously.
        /// </returns>
        Task<TEntity> GetByIdAsync(object id);

        /// <summary>
        /// Gets the count asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// The count.
        /// </returns>
        Task<long> GetCountAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Firsts the or default asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// The first or default predicate.
        /// </returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Firsts the or default asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>
        /// The first or default properties.
        /// </returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params string[] includeProperties);

        /// <summary>
        /// Firsts the or default asynchronous.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>The result.</returns>
        Task<TResult> FirstOrDefaultAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, params string[] includeProperties);

        /// <summary>
        /// Singles the or default asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// The singled out or default predicate.
        /// </returns>
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Singles the or default asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>The single or default properties.</returns>
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params string[] includeProperties);

        /// <summary>
        /// Singles the or default asynchronous.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>The result.</returns>
        Task<TResult> SingleOrDefaultAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, params string[] includeProperties);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>The all entities.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, params string[] includeProperties);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of other entity.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>
        /// The all entities.
        /// </returns>
        Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate, params string[] includeProperties)
            where T : class, IEntity;

        /// <summary>
        /// Singles the or default asynchronous.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>The result.</returns>
        Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, params string[] includeProperties);

        /// <summary>
        /// Gets all entities that satisfy the specification tih the include properties.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>The entities that satisfy the specification.</returns>
        Task<IEnumerable<TEntity>> GetAllSpecificAsync(CompositeSpecification<TEntity> specification);

        /// <summary>
        /// Orders the by asynchronous.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="take">The take.</param>
        /// <returns>The enumerable entities.</returns>
        Task<IEnumerable<TEntity>> OrderByAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy, int? take);

        /// <summary>
        /// Orders the by descending asynchronous.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="take">The take.</param>
        /// <returns>The enumerable entities.</returns>
        Task<IEnumerable<TEntity>> OrderByDescendingAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy, int? take);

        /// <summary>
        /// Query all items.
        /// Use this method for ODATA enabled queries.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>
        /// The list of all entities asynchronously.
        /// </returns>
        Task<IQueryable<TEntity>> QueryAllAsync(Expression<Func<TEntity, bool>> predicate, params string[] includeProperties);

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="data">The data.</param>
        /// <returns>The task.</returns>
        Task ExecuteAsync(object args, IDictionary<string, object> data);

        /// <summary>
        /// Executes the query asynchronous.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="data">The data.</param>
        /// <returns>The Result.</returns>
        Task<IEnumerable<TEntity>> ExecuteQueryAsync(object args, IDictionary<string, object> data);

        /// <summary>Executes the view asynchronous.</summary>
        /// <returns>The Result.</returns>
        Task<IQueryable<TEntity>> ExecuteViewAsync();
    }
}