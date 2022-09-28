// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrueExpressionEvaluator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Processors.Approval
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using CodingSeb.ExpressionEvaluator;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Processors.Approval.Interfaces;

    /// <summary>
    /// The True Expression Evaluator.
    /// </summary>
    public class TrueExpressionEvaluator : CodingSeb.ExpressionEvaluator.ExpressionEvaluator, ITrueExpressionEvaluator
    {
        /// <inheritdoc/>
        public void InitializeVariables(BalanceSummaryAggregate aggregate)
        {
            ArgumentValidators.ThrowIfNull(aggregate, nameof(aggregate));
            this.Variables = new Dictionary<string, object>
            {
               { "Io", Convert.ToDouble(aggregate.InitialInventory, CultureInfo.InvariantCulture) },
               { "E", Convert.ToDouble(aggregate.Inputs, CultureInfo.InvariantCulture) },
               { "S", Convert.ToDouble(aggregate.Outputs, CultureInfo.InvariantCulture) },
               { "If", Convert.ToDouble(aggregate.FinalInventory, CultureInfo.InvariantCulture) },
               { "PI", Convert.ToDouble(aggregate.IdentifiedLosses, CultureInfo.InvariantCulture) },
               { "I", Convert.ToDouble(aggregate.Interface, CultureInfo.InvariantCulture) },
               { "T", Convert.ToDouble(aggregate.Tolerance, CultureInfo.InvariantCulture) },
               { "PNI", Convert.ToDouble(aggregate.UnidentifiedLosses, CultureInfo.InvariantCulture) },
            };
        }

        /// <inheritdoc/>
        public bool TryEvaluate(string rule)
        {
            return bool.TryParse(this.Evaluate(rule).ToString(), out bool result) && result;
        }

        /// <summary>
        /// The initialization of expression operators.
        /// </summary>
        protected override void Init()
        {
            this.operatorsDictionary.Clear();
            this.operatorsDictionary.Add("AND", ExpressionOperator.ConditionalAnd);
            this.operatorsDictionary.Add("OR", ExpressionOperator.ConditionalOr);
            this.operatorsDictionary.Add("+", ExpressionOperator.Plus);
            this.operatorsDictionary.Add("-", ExpressionOperator.Minus);
            this.operatorsDictionary.Add("*", ExpressionOperator.Multiply);
            this.operatorsDictionary.Add("/", ExpressionOperator.Divide);
            this.operatorsDictionary.Add("<", ExpressionOperator.Lower);
            this.operatorsDictionary.Add(">", ExpressionOperator.Greater);
            this.operatorsDictionary.Add("<=", ExpressionOperator.LowerOrEqual);
            this.operatorsDictionary.Add(">=", ExpressionOperator.GreaterOrEqual);
            this.operatorsDictionary.Add("=", ExpressionOperator.Equal);

            this.defaultVariables.Remove("Pi");
            this.defaultVariables.Remove("E");
        }
    }
}