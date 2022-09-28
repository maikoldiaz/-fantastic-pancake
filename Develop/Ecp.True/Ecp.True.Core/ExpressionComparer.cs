// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionComparer.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The expression based equality comparer.
    /// </summary>
    /// <typeparam name="TSource">The type of source.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public class ExpressionComparer<TSource, TKey> : IEqualityComparer<TSource>
        where TSource : class
    {
        /// <summary>
        /// The expression.
        /// </summary>
        private readonly Func<TSource, TKey> expression;

        /// <summary>
        /// The comparer.
        /// </summary>
        private readonly EqualityComparer<TKey> comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionComparer{TSource, TKey}"/> class.
        /// </summary>
        /// <param name="projection">The projection.</param>
        internal ExpressionComparer(Func<TSource, TKey> projection)
        {
            ArgumentValidators.ThrowIfNull(projection, nameof(projection));
            this.expression = projection;
            this.comparer = EqualityComparer<TKey>.Default;
        }

        /// <inheritdoc/>
        public bool Equals(TSource x, TSource y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return this.comparer.Equals(this.expression(x), this.expression(y));
        }

        /// <inheritdoc/>
        public int GetHashCode(TSource obj)
        {
            ArgumentValidators.ThrowIfNull(obj, nameof(obj));
            return this.comparer.GetHashCode(this.expression(obj));
        }
    }
}
