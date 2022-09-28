// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestRepositoryFactory.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Tests.Admin
{
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Core;

    public abstract class TestRepositoryFactory<T>
        where T : Entity
    {
        /// <summary>
        /// Gets a in memory testing repository.
        /// </summary>
        /// <typeparam name="T">The entity.</typeparam>
        /// <returns>The repository.</returns>
        public abstract (IRepository<T>, SqlDataContext, ISqlTokenProvider) CreateRepository();

        /// <summary>
        /// Gets a in memory testing repository.
        /// </summary>
        /// <param name="type">The repository type.</param>
        /// <returns>The repository.</returns>
        public (IRepository<T>, SqlDataContext, ISqlTokenProvider) GetRepository()
        {
            return this.CreateRepository();
        }
    }
}