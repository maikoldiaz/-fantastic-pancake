// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpecificationExtensions.cs" company="Microsoft">
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
    using Ecp.True.Core;

    /// <summary>
    /// Specification extensions.
    /// </summary>
    public static class SpecificationExtensions
    {
        /// <summary>
        /// Combines two expressions using an and operator.
        /// </summary>
        /// <typeparam name="T">The type of parameter.</typeparam>
        /// <param name="expr1">The first expression.</param>
        /// <param name="expr2">The second expression.</param>
        /// <returns>The resulting expression.</returns>
        public static Expression<Func<T, bool>> AndAlso<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            ArgumentValidators.ThrowIfNull(expr1, nameof(expr1));
            ArgumentValidators.ThrowIfNull(expr2, nameof(expr2));

            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left, right), parameter);
        }

        /// <summary>
        /// Combines two expressions using an and not operator.
        /// </summary>
        /// <typeparam name="T">The type of parameter.</typeparam>
        /// <param name="expr1">The first expression.</param>
        /// <returns>The resulting expression.</returns>
        public static Expression<Func<T, bool>> Not<T>(
            this Expression<Func<T, bool>> expr1)
        {
            ArgumentValidators.ThrowIfNull(expr1, nameof(expr1));

            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            return Expression.Lambda<Func<T, bool>>(Expression.Not(left), parameter);
        }

        /// <summary>
        /// Combines two expressions using an and operator.
        /// </summary>
        /// <typeparam name="T">The type of parameter.</typeparam>
        /// <param name="expr1">The first expression.</param>
        /// <param name="expr2">The second expression.</param>
        /// <returns>The resulting expression.</returns>
        public static Expression<Func<T, bool>> AndNot<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            ArgumentValidators.ThrowIfNull(expr1, nameof(expr1));
            ArgumentValidators.ThrowIfNull(expr2, nameof(expr2));

            var negatedExpr2 = expr2.Not();

            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(negatedExpr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(negatedExpr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left, right), parameter);
        }

        /// <summary>
        /// Combines two expressions using an or operator.
        /// </summary>
        /// <typeparam name="T">The type of parameter.</typeparam>
        /// <param name="expr1">The first expression.</param>
        /// <param name="expr2">The second expression.</param>
        /// <returns>The resulting expression.</returns>
        public static Expression<Func<T, bool>> OrElse<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            ArgumentValidators.ThrowIfNull(expr1, nameof(expr1));
            ArgumentValidators.ThrowIfNull(expr2, nameof(expr2));

            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(left, right), parameter);
        }

        /// <summary>
        /// Visitor to replace the parameters of a expression.
        /// </summary>
        private class ReplaceExpressionVisitor
            : ExpressionVisitor
        {
            /// <summary>
            /// Old parameter value.
            /// </summary>
            private readonly ParameterExpression oldValue;

            /// <summary>
            /// New parameter value.
            /// </summary>
            private readonly Expression newValue;

            /// <summary>
            /// Initializes a new instance of the <see cref="ReplaceExpressionVisitor"/> class.
            /// </summary>
            /// <param name="oldValue">Old parameter value.</param>
            /// <param name="newValue">New parameter value.</param>
            public ReplaceExpressionVisitor(ParameterExpression oldValue, Expression newValue)
            {
                this.oldValue = oldValue;
                this.newValue = newValue;
            }

            /// <summary>
            /// Visits the expression node.
            /// </summary>
            /// <param name="node">The expression node.</param>
            /// <returns>The visited expression.</returns>
            public override Expression Visit(Expression node)
            {
                if (node == this.oldValue)
                {
                    return this.newValue;
                }
                else
                {
                    var visited = base.Visit(node);
                    return visited;
                }
            }

            /// <summary>
            /// Visits the expression parameter.
            /// </summary>
            /// <param name="node">The expression parameter.</param>
            /// <returns>The visited expression.</returns>
            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (node == this.oldValue)
                {
                    return this.newValue;
                }
                else
                {
                    return node;
                }
            }
        }
    }
}