// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialMovementSpecification.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Specifications
{
    using System;
    using System.Linq.Expressions;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.Specifications;

    /// <summary>
    ///     Gets whether a movement is official.
    /// </summary>
    public class OfficialMovementSpecification : CompositeSpecification<Movement>
    {
        /// <inheritdoc/>
        public override Expression<Func<Movement, bool>> ToExpression() => m => m.ScenarioId == ScenarioType.OFFICER;
    }
}