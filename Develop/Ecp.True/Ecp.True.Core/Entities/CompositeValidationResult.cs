// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompositeValidationResult.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Core
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The composite validation result.
    /// </summary>
    /// <seealso cref="System.ComponentModel.DataAnnotations.ValidationResult" />
    [ExcludeFromCodeCoverage]
    public class CompositeValidationResult : ValidationResult
    {
        private readonly List<ValidationResult> results = new List<ValidationResult>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidationResult"/> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        public CompositeValidationResult(string errorMessage)
            : base(errorMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidationResult"/> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="memberNames">The list of member names that have validation errors.</param>
        public CompositeValidationResult(string errorMessage, IEnumerable<string> memberNames)
            : base(errorMessage, memberNames)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidationResult"/> class.
        /// </summary>
        /// <param name="validationResult">The validation result object.</param>
        protected CompositeValidationResult(ValidationResult validationResult)
            : base(validationResult)
        {
        }

        /// <summary>
        /// Gets the results.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        public IEnumerable<ValidationResult> Results
        {
            get
            {
                return this.results;
            }
        }

        /// <summary>
        /// Adds the result.
        /// </summary>
        /// <param name="validationResult">The validation result.</param>
        public void AddResult(ValidationResult validationResult)
        {
            this.results.Add(validationResult);
        }
    }
}
