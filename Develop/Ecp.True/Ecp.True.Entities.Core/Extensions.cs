// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Core;

    /// <summary>
    /// The entity extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Merges the two enumerable.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="original">The original.</param>
        /// <param name="input">The input.</param>
        /// <param name="expression">The expression.</param>
        public static void Merge<TSource, TKey>(this ICollection<TSource> original, ICollection<TSource> input, Func<TSource, TKey> expression)
            where TSource : class, IEntity
        {
            ArgumentValidators.ThrowIfNull(original, nameof(original));
            if (input == null)
            {
                return;
            }

            var comparer = ExpressionEqualityComparer.Create(expression);

            // find missing
            var removableList = new List<TSource>();
            original.Except(input, comparer).ForEach(o => removableList.Add(o));

            // update common
            input.Intersect(original, comparer).ForEach(o =>
            {
                var existing = original.Single(p => expression(p).Equals(expression(o)));
                existing.CopyFrom(o);
            });

            // Add new
            var inserts = new List<TSource>();
            input.Where(o => original.All(p => !expression(p).Equals(expression(o)))).ForEach(o => inserts.Add(o));
            inserts.ForEach(original.Add);

            // delete missing
            removableList.ForEach(r => original.Remove(r));
        }

        /// <summary>
        /// Adds elements of source to destination collection.
        /// </summary>
        /// <typeparam name="T">The generic entity.</typeparam>
        /// <param name="destination">destination collection.</param>
        /// <param name="source">The source collection.</param>
        public static void AddRange<T>(this ICollection<T> destination, IEnumerable<T> source)
        {
            ArgumentValidators.ThrowIfNull(destination, nameof(destination));
            ArgumentValidators.ThrowIfNull(source, nameof(source));

            source.ForEach(x => destination.Add(x));
        }
    }
}
