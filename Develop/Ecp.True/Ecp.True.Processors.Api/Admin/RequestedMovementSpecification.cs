// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestedMovementSpecification.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Admin
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.Specifications;

    /// <summary>
    ///     Gets whether a movement in an array exists.
    /// </summary>
    public class RequestedMovementSpecification : CompositeSpecification<Movement>
    {
        /// <summary>
        /// The movement transaction ids.
        /// </summary>
        private readonly int[] movementTransactionIds;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestedMovementSpecification" /> class.
        /// </summary>
        /// <param name="movementTransactionIds">The movementTransactionId array.</param>
        public RequestedMovementSpecification(int[] movementTransactionIds)
        {
            this.movementTransactionIds = movementTransactionIds;
        }

        /// <inheritdoc />
        public override Expression<Func<Movement, bool>> ToExpression() => m => !m.IsDeleted
            && this.movementTransactionIds.Contains(m.MovementTransactionId);
    }
}