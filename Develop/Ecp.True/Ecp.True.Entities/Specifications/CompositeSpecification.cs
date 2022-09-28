// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompositeSpecification.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Interfaces;

    /// <summary>
    /// The composite specification.
    /// </summary>
    /// <typeparam name="TEntity">the type of the candidate.</typeparam>
    public abstract class CompositeSpecification<TEntity> : ISpecification<TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// Gets the include properties.
        /// </summary>
        public virtual ICollection<string> IncludeProperties { get; } = new List<string>();

        /// <summary>
        /// Convert to expression using the <seealso cref="ToExpression"/> method.
        /// </summary>
        /// <param name="specification">The current specification.</param>
        public static implicit operator Expression<Func<TEntity, bool>>(CompositeSpecification<TEntity> specification)
        {
            ArgumentValidators.ThrowIfNull(specification, nameof(specification));
            return specification.ToExpression();
        }

        /// <summary>
        /// Convert to expression using the <seealso cref="ToExpression"/> method.
        /// </summary>
        /// <param name="specification">The current specification.</param>
        public static implicit operator Func<TEntity, bool>(CompositeSpecification<TEntity> specification)
        {
            ArgumentValidators.ThrowIfNull(specification, nameof(specification));
            return specification.ToExpression().Compile();
        }

        /// <summary>
        /// Adds a property to the include collection.
        /// </summary>
        /// <param name="includeProperty">The property.</param>
        /// <returns>The composite specification.</returns>
        public CompositeSpecification<TEntity> Include(string includeProperty)
        {
            this.IncludeProperties.Add(includeProperty);
            return this;
        }

        /// <summary>
        /// Adds a range of properties to the include collection.
        /// </summary>
        /// <param name="includeProperties">The enumerable of include properties.</param>
        /// <returns>The composite specification.</returns>
        public CompositeSpecification<TEntity> Include(IEnumerable<string> includeProperties)
        {
            this.IncludeProperties.AddRange(includeProperties);
            return this;
        }

        /// <summary>
        /// The expression.
        /// </summary>
        /// <returns>Whether the candidate satisfies de condition.</returns>
        public abstract Expression<Func<TEntity, bool>> ToExpression();

        /// <summary>
        /// The expression.
        /// </summary>
        /// <returns>Whether the candidate satisfies de condition.</returns>
        public virtual Func<TEntity, bool> ToFunc() => this.ToExpression().Compile();

        /// <summary>
        /// Gets whether a movement or inventory is manual.
        /// </summary>
        /// <param name="candidate">The movement to be evaluated.</param>
        /// <returns>Whether the movement satisfies the conditions.</returns>
        public virtual bool IsSatisfiedBy(TEntity candidate) => this.ToExpression().Compile()(candidate);

        /// <summary>
        /// Combines two specifications with a logic and.
        /// </summary>
        /// <param name="other">The other specification.</param>
        /// <returns>The resulting specifications.</returns>
        public CompositeSpecification<TEntity> AndAlso(ISpecification<TEntity> other) => new AndAlsoSpecification<TEntity>(this, other);

        /// <summary>
        /// Combines two specifications with a logic or.
        /// </summary>
        /// <param name="other">The other specification.</param>
        /// <returns>The resulting specifications.</returns>
        public CompositeSpecification<TEntity> OrElse(ISpecification<TEntity> other) => new OrElseSpecification<TEntity>(this, other);
    }
}