// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISpecification.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Interfaces
{
    using System;
    using System.Linq.Expressions;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The specification base class.
    /// </summary>
    /// <typeparam name="T">The entity to evaluate.</typeparam>
    public interface ISpecification<T>
        where T : IEntity
    {
        /// <summary>
        /// The expression.
        /// </summary>
        /// <returns>Whether the candidate satisfies de condition.</returns>
        Expression<Func<T, bool>> ToExpression();

        /// <summary>
        /// Gets whether a movement or inventory is manual.
        /// </summary>
        /// <param name="candidate">The movement to be evaluated.</param>
        /// <returns>Whether the movement satisfies the conditions.</returns>
        bool IsSatisfiedBy(T candidate);
    }
}