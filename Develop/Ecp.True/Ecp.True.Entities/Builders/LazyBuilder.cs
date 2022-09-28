// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LazyBuilder.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The lazy builder abstract class.
    /// </summary>
    /// <typeparam name="TSubject">The subject of the builder.</typeparam>
    /// <typeparam name="TSelf">A recursive parameter to enable fluent builder inheritance.</typeparam>
    public abstract class LazyBuilder<TSubject, TSelf> : ILazyBuilder<TSubject>
        where TSelf : LazyBuilder<TSubject, TSelf>
    {
        /// <summary>
        /// The list of actions to build the subject lazily.
        /// </summary>
        private readonly List<Func<TSubject, TSubject>> actions = new List<Func<TSubject, TSubject>>();

        /// <summary>
        /// The builder method that aggregates/ executes the builder actions.
        /// </summary>
        /// <returns>The built subject.</returns>
        public virtual TSubject Build()
        {
            return this.actions.Aggregate(this.Construct(), (p, f) => f(p));
        }

        /// <summary>
        /// Constructs the subject with one constructor.
        /// </summary>
        /// <returns>The built basic subject.</returns>
        protected abstract TSubject Construct();

        /// <summary>
        /// Adds an action to the list of actions.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The builder.</returns>
        protected TSelf Do(Action<TSubject> action)
        {
            return this.AddAction(action);
        }

        /// <summary>
        /// Adds an action to the list of actions.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The builder.</returns>
        private TSelf AddAction(Action<TSubject> action)
        {
            this.actions.Add(b =>
            {
                action(b);
                return b;
            });

            return (TSelf)this;
        }
    }
}