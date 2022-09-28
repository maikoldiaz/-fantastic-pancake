// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPrototype.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Common
{
    using Ecp.True.Entities.Core;

    /// <summary>
    /// Shallow copy interface.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public interface IPrototype<out TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// Shallows copy an entity.
        /// </summary>
        /// <returns>the copied entity.</returns>
        public abstract TEntity ShallowCopy();
    }
}