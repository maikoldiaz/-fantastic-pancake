// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISqlDataContext.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Sql
{
    using Ecp.True.DataAccess.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.Infrastructure;

    /// <summary>
    /// The SQL data context.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Interfaces.IDataContext" />
    public interface ISqlDataContext : IDataContext
    {
        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        DatabaseFacade Database { get; }

        /// <summary>
        /// Sets this instance.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>The db set.</returns>
        DbSet<TEntity> Set<TEntity>()
            where TEntity : class;

        /// <summary>
        /// Entries the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>The entity entry.</returns>
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
            where TEntity : class;

        /// <summary>
        /// Queries this instance.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>The Query entity.</returns>
        DbQuery<TEntity> Query<TEntity>()
            where TEntity : class;

        /// <summary>
        /// Sets the access token.
        /// </summary>
        void SetAccessToken();
    }
}
