// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionEqualityComparer.cs" company="Microsoft">
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

    /// <summary>
    /// The expression equality comparer builder.
    /// </summary>
    public static class ExpressionEqualityComparer
    {
        /// <summary>
        /// Creates the expression based equality comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>The expression equality comparer.</returns>
        public static ExpressionComparer<TSource, TKey> Create<TSource, TKey>(Func<TSource, TKey> expression)
            where TSource : class
        {
            return new ExpressionComparer<TSource, TKey>(expression);
        }
    }
}
