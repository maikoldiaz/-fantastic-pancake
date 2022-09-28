// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITrueExpressionEvaluator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Processors.Approval.Interfaces
{
    using Ecp.True.Entities.Admin;

    /// <summary>
    /// The True Expression Evaluator.
    /// </summary>
    public interface ITrueExpressionEvaluator
    {
        /// <summary>
        /// The initialize the variables.
        /// </summary>
        /// <param name="aggregate">The aggregate.</param>
        void InitializeVariables(BalanceSummaryAggregate aggregate);

        /// <summary>
        /// The evaluation of the rule.
        /// </summary>
        /// <param name="rule">The rule to evaluate approval.</param>
        /// <returns>Bool value. </returns>
        bool TryEvaluate(string rule);
    }
}