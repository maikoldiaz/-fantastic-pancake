// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogicSpecification.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Specifications
{
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Interfaces;

    /// <summary>
    /// Combines two specifications with a logic and.
    /// </summary>
    /// <param name="other">The other specification.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <returns>The resulting specifications.</returns>
    public abstract class LogicSpecification<TEntity> : CompositeSpecification<TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicSpecification{TT}"/> class.
        /// </summary>
        /// <param name="left">The left specification.</param>
        /// <param name="right">The right specification.</param>
        protected LogicSpecification(ISpecification<TEntity> left, ISpecification<TEntity> right)
        {
            this.Left = left;
            this.Right = right;
        }

        /// <summary>
        /// Gets the left specification.
        /// </summary>
        protected ISpecification<TEntity> Left { get; }

        /// <summary>
        /// Gets the right specification.
        /// </summary>
        protected ISpecification<TEntity> Right { get; }
    }
}