// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISqlDataAccess.cs" company="Microsoft">
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
    using Ecp.True.Entities.Core;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The sql data access.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="Ecp.True.DataAccess.Interfaces.IDataAccess{TEntity}" />
    public interface ISqlDataAccess<TEntity> : IDataAccess<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Sets this instance.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>The DB Set.</returns>
        DbSet<TEntity> EntitySet();

        /// <summary>
        /// Sets this instance.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The DB Set.</returns>
        DbSet<T> Set<T>()
            where T : class, IEntity;
    }
}
