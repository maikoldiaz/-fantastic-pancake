// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AndAlsoSpecification.cs" company="Microsoft">
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
    using System.Linq.Expressions;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Interfaces;

    /// <summary>
    /// Combines two specifications with a logic and.
    /// </summary>
    /// <param name="other">The other specification.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <returns>The resulting specifications.</returns>
    public class AndAlsoSpecification<TEntity> : LogicSpecification<TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndAlsoSpecification{T}" /> class.
        /// </summary>
        /// <param name="left">The left specification.</param>
        /// <param name="right">The right specification.</param>
        public AndAlsoSpecification(ISpecification<TEntity> left, ISpecification<TEntity> right)
            : base(left, right)
        {
        }

        /// <inheritdoc />
        public override Expression<Func<TEntity, bool>> ToExpression() =>
            this.Left.ToExpression().AndAlso(this.Right.ToExpression());

        /// <inheritdoc />
        public override bool IsSatisfiedBy(TEntity candidate) => this.Left.IsSatisfiedBy(candidate) && this.Right.IsSatisfiedBy(candidate);
    }
}