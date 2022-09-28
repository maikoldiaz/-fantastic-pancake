// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Repo.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Ioc.Tests.Types
{
    using Ecp.True.Ioc.Tests.Types.Core;

    /// <summary>
    /// The generic type.
    /// </summary>
    /// <typeparam name="T">The generic type parameter.</typeparam>
    /// <seealso cref="IRepo{T}" />
    public class Repo<T> : IRepo<T>
    {
        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Add(T entity)
        {
        }
    }
}